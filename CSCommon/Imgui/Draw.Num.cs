using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 숫자(3.5): 굵기 600 · 자리 폭 같음(tabular) · 할당 0
        // 굵기: 글꼴 0 = 게임 기본 UI 글꼴(바닐라 OpenSans-Semibold = 600, output_log '보통 OpenSans-Semibold'). 굵게(face 1 = 엔진 덧칠)는 쓰지 않음 → 700 없음.
        //   글꼴 파일을 싣지 않음. 한글은 Unity 대체 글꼴(보통 굵기)로 그려짐 — 숫자 · 라틴 · 기호만 600.
        // 자리 폭: 숫자 0 ~ 9 를 가장 넓은 숫자 폭 칸 가운데에 둠. 글꼴 숫자 폭이 이미 같고 시계 콜론이 없으면 한 번에 그림(빠른 길).

        static readonly string[] asciiChars = AsciiChars();
        static readonly Dictionary<char, string> otherChars = new();
        static readonly Dictionary<int, Vector2> digitCells = new(); // (화면 px, 게임 글꼴 찾음) → (숫자 칸 폭 px, 1 = 글꼴 숫자 폭이 원래 같음)
        static readonly float[] digitWidths = new float[10];

        static string[] AsciiChars()
        {
            var a = new string[95];
            for (int i = 0; i < a.Length; i++) a[i] = ((char)(32 + i)).ToString();
            return a;
        }

        /// <summary>한 글자 글(ASCII 는 표, 그 밖은 처음 볼 때 한 번 만들어 둠).</summary>
        static string One(char ch)
        {
            if (ch >= ' ' && ch <= '~') return asciiChars[ch - 32];
            if (!otherChars.TryGetValue(ch, out var s))
            {
                if (otherChars.Count > 512) otherChars.Clear();
                s = ch.ToString();
                otherChars[ch] = s;
            }
            return s;
        }

        /// <summary>숫자 칸 폭 = 가장 넓은 숫자 폭. same = 가장 넓은 것 − 가장 좁은 것이 0.01 px 안(글꼴 숫자가 원래 tabular).</summary>
        internal static float DigitCell(float[] widths, out bool same)
        {
            float lo = float.MaxValue, hi = 0f;
            for (int i = 0; i < widths.Length; i++)
            {
                lo = Mathf.Min(lo, widths[i]);
                hi = Mathf.Max(hi, widths[i]);
            }
            same = hi - lo < 0.01f;
            return hi;
        }

        /// <summary>한 글자 나아감: 숫자는 칸 폭, 그 밖은 제 폭.</summary>
        internal static float Advance(char ch, float cell, float own) => ch >= '0' && ch <= '9' ? cell : own;

        static float CellPx(int px, out bool same)
        {
            int key = px * 2 + (facesFound ? 1 : 0); // 게임 글꼴을 찾기 전(로딩 중 기본 글꼴)에 잰 칸은 따로 둠
            if (!digitCells.TryGetValue(key, out var v))
            {
                for (int i = 0; i < 10; i++) digitWidths[i] = FaceWidthPx(asciiChars['0' - 32 + i], px, 0);
                v = new Vector2(DigitCell(digitWidths, out bool s), s ? 1f : 0f);
                digitCells[key] = v;
            }
            same = v.y > 0.5f;
            return v.x;
        }

        static float NumWidthPx(string text, int px, float cell)
        {
            float w = 0f;
            for (int i = 0; i < text.Length; i++) w += Advance(text[i], cell, FaceWidthPx(One(text[i]), px, 0));
            return w;
        }

        /// <summary>숫자 글 폭(논리) — Num 과 같은 자리 폭.</summary>
        public static float NumWidth(string text, float size)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            int px = Px(size);
            return NumWidthPx(text, px, CellPx(px, out _)) / S;
        }

        /// <summary>
        /// 3.5 숫자 글(KPI · 타이밍 · 축 · 수): 굵기 600 · 숫자 자리 폭 같음. 줄이지 않음(칸 폭은 부르는 쪽이 NumWidth 로 잡음).
        /// 숫자 사이 콜론('1:23.456')은 Text 처럼 점 둘로 그림. 글자 · 숫자 섞인 글('1위' · '₡1,234')도 그대로 됨. 숫자 열은 MiddleRight.
        /// </summary>
        public static void Num(Rect r, string text, float size, Color c, TextAnchor anchor = TextAnchor.MiddleRight)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f || TextOff(r, size, false)) return;
            int px = Px(size);
            float cell = CellPx(px, out bool same);
            bool clock = HasClockColon(text);
            if (same && !clock)
            {
                var whole = TextStyle(px, 0, anchor, false);
                whole.normal.textColor = Op(c);
                content.text = text;
                whole.Draw(ToScreen(r), content, false, false, false, false);
                return;
            }
            float total = NumWidthPx(text, px, cell) / S, cellW = cell / S;
            float x = anchor is TextAnchor.UpperCenter or TextAnchor.MiddleCenter or TextAnchor.LowerCenter ? r.center.x - total * 0.5f
                : anchor is TextAnchor.UpperRight or TextAnchor.MiddleRight or TextAnchor.LowerRight ? r.xMax - total : r.x;
            var left = anchor is TextAnchor.UpperLeft or TextAnchor.UpperCenter or TextAnchor.UpperRight ? TextAnchor.UpperLeft
                : anchor is TextAnchor.LowerLeft or TextAnchor.LowerCenter or TextAnchor.LowerRight ? TextAnchor.LowerLeft : TextAnchor.MiddleLeft;
            var style = TextStyle(px, 0, left, false);
            style.normal.textColor = Op(c);
            float cy = left == TextAnchor.UpperLeft ? r.y + size * 0.62f : left == TextAnchor.LowerLeft ? r.yMax - size * 0.5f : r.center.y + size * 0.04f;
            float dot = Mathf.Max(0.9f, size * 0.085f);
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                string one = One(ch);
                float own = FaceWidthPx(one, px, 0) / S, step = Advance(ch, cellW, own);
                if (ch == ':' && i > 0 && i + 1 < text.Length && char.IsDigit(text[i - 1]) && char.IsDigit(text[i + 1]))
                {
                    float mx = x + step * 0.5f;
                    Dot(new Vector2(mx, cy - size * 0.2f), dot, c);
                    Dot(new Vector2(mx, cy + size * 0.2f), dot, c);
                }
                else if (ch != ' ')
                {
                    content.text = one;
                    style.Draw(ToScreen(new Rect(x + (step - own) * 0.5f, r.y, own + 2f, r.height)), content, false, false, false, false);
                }
                x += step;
            }
        }
    }
}
