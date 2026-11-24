using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 채우기 · 테두리

        public static void Fill(Rect r, Color c)
        {
            if (!Repaint || c.a <= 0f || Off(r, 1f, 1f)) return;
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(r), Texture2D.whiteTexture);
            GUI.color = old;
        }

        public static void Border(Rect r, Color c, float w = 1f)
        {
            Fill(new Rect(r.x, r.y, r.width, w), c);
            Fill(new Rect(r.x, r.yMax - w, r.width, w), c);
            Fill(new Rect(r.x, r.y + w, w, r.height - 2f * w), c);
            Fill(new Rect(r.xMax - w, r.y + w, w, r.height - 2f * w), c);
        }

        /// <summary>
        /// 9-slice 모서리 화소: 화면 칸 짧은 변의 절반까지(넘지 않게). 알약처럼 radius = 높이/2 일 때
        /// 반올림으로 모서리가 칸 절반보다 커지면 IMGUI 가 모서리를 찌그러뜨려 팔각형 · 가운데 겹친 상자가 보이고,
        /// 절반보다 작게 자르면 스위치 · 알약 끝이 반원이 아니게 됨.
        /// </summary>
        static int SlicePx(Rect r, float radius)
        {
            int fit = Mathf.FloorToInt(Mathf.Min(r.width, r.height) * S * 0.5f);
            return Mathf.Max(1, Mathf.Min(Mathf.RoundToInt(radius * S), fit));
        }

        /// <summary>둥근 모서리 채우기. radius는 논리 단위.</summary>
        public static void Round(Rect r, Color c, float radius)
        {
            if (!Repaint || c.a <= 0f || Off(r, 1f, 1f)) return;
            int px = SlicePx(r, radius);
            if (!roundStyles.TryGetValue(px, out var style))
            {
                if (!CanBake()) { Fill(r, c); return; } // 이번 틀 굽기 몫을 다 씀: 네모로 그리고 다음 틀에 구움
                style = NineSlice(MakeRounded(px, 0), px);
                Baked();
                roundStyles[px] = style;
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            style.Draw(Snap(ToScreen(r)), false, false, false, false);
            GUI.color = old;
        }

        /// <summary>
        /// 아래 두 모서리만 둥근 채우기(창 몸통 바닥 띠 — 위는 머리 · 탭 줄에 붙음). 네모 Fill 로 창 바닥까지 칠하면 둥근 창 아래 모서리가 직각으로 보였음.
        /// 겹쳐 칠하지 않음(열기 애니 투명도에서 띠가 진해지지 않게): 위 = 네모, 아래 radius 띠 = 2배 높이 둥근 면의 아래 절반만.
        /// </summary>
        public static void RoundBottom(Rect r, Color c, float radius)
        {
            if (!Repaint || c.a <= 0f) return;
            radius = Mathf.Min(radius, r.height * 0.5f);
            Fill(new Rect(r.x, r.y, r.width, r.height - radius), c);
            BeginClip(new Rect(r.x, r.yMax - radius, r.width, radius), Vector2.zero);
            Round(new Rect(0f, -radius, r.width, radius * 2f), c, radius);
            EndClip();
        }

        /// <summary>화면 픽셀 경계로 맞춤 — 칸 끝이 소수 픽셀이면 9칸 그림이 번져 테두리가 변마다 두께 · 진하기가 달라 보였음.</summary>
        static Rect Snap(Rect s)
        {
            float x0 = Mathf.Round(s.x), y0 = Mathf.Round(s.y);
            return new Rect(x0, y0, Mathf.Round(s.xMax) - x0, Mathf.Round(s.yMax) - y0);
        }

        /// <summary>둥근 모서리 테두리.</summary>
        public static void RoundOutline(Rect r, Color c, float radius, float width = 1f)
        {
            if (!Repaint || c.a <= 0f || Off(r, 1f, 1f)) return;
            int px = SlicePx(r, radius);
            int wpx = Mathf.Max(1, Mathf.RoundToInt(width * S));
            long key = ((long)px << 16) | (uint)wpx;
            if (!outlineStyles.TryGetValue(key, out var style))
            {
                if (!CanBake()) { Border(r, c, width); return; } // 이번 틀 굽기 몫을 다 씀: 네모 테두리로 그리고 다음 틀에 구움
                style = NineSlice(MakeRounded(Mathf.Max(px, wpx), wpx), Mathf.Max(px, wpx));
                Baked();
                outlineStyles[key] = style;
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            style.Draw(Snap(ToScreen(r)), false, false, false, false);
            GUI.color = old;
        }

        /// <summary>채우고 테두리까지(카드 · 버튼).</summary>
        public static void Box(Rect r, Color fill, Color line, float radius, float lineWidth = 1f)
        {
            Round(r, fill, radius);
            RoundOutline(r, line, radius, lineWidth);
        }

        // 떠 있는 판 그림자(Look.Elevated) 구운 9칸: 열쇠 = 모서리 화소 · 배율 · 진하기
        static readonly Dictionary<long, GUIStyle> shadowStyles = new();

        /// <summary>
        /// 떠 있는 판 그림자 8겹(겹 i = 1 ~ 8: 칸 좌우 i · 위 −(2 − 0.5i) · 아래 2 + 1.5i 넓힘, 모서리 radius + i, 검정 알파 alpha × (1 − (i − 1)/8))을
        /// 미리 구운 한 장으로 그림. 판이 모서리에 비해 작아 9칸으로 늘릴 가운데가 없으면 false(부른 쪽이 겹으로 그림).
        /// </summary>
        public static bool Shadow(Rect r, float radius, float alpha)
        {
            if (!Repaint || alpha <= 0f) return true;
            int rp = Mathf.RoundToInt(Mathf.Max(0f, radius) * S);
            if (r.width * S < rp * 2 + 8 || r.height * S < rp * 2 + 4f * S + 8) return false;
            var outer = new Rect(r.x - 8f, r.y - 2f, r.width + 16f, r.height + 16f);
            if (Off(outer, 1f, 1f)) return true;
            long key = ((long)rp << 32) | ((long)Mathf.RoundToInt(S * 1000f) << 12) | (uint)(Mathf.RoundToInt(alpha * 1000f) & 0xFFF);
            if (!shadowStyles.TryGetValue(key, out var style))
            {
                if (!CanBake()) return true; // 이번 틀 굽기 몫을 다 씀: 그림자는 다음 틀부터
                shadowStyles[key] = style = BakeShadow(rp, alpha);
                Baked();
            }
            Color old = GUI.color;
            GUI.color = Op(Color.white);
            style.Draw(Snap(ToScreen(outer)), false, false, false, false);
            GUI.color = old;
            return true;
        }

        /// <summary>Shadow 의 한 장: 화소마다 4×4 표본으로 겹마다 덮인 넓이, 겹은 알파 겹침(1 − Π(1 − a)). 판 칸 바깥 여백 = 좌우 8 · 위 2 · 아래 14(논리).</summary>
        static GUIStyle BakeShadow(int rp, float alpha)
        {
            int w = Mathf.CeilToInt(16f * S) + rp * 2 + 8, h = Mathf.CeilToInt(20f * S) + rp * 2 + 8;
            float fx = 8f * S, fy = 2f * S, iw = w - 16f * S, ih = h - 16f * S;
            var px = new Color32[w * h];
            const int N = 4;
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                float keep = 1f;
                for (int i = 1; i <= 8; i++)
                {
                    float x0 = fx - i * S, x1 = fx + iw + i * S, y0 = fy + (2f - 0.5f * i) * S, y1 = fy + ih + (2f + 1.5f * i) * S, rad = rp + i * S;
                    int hit = 0;
                    for (int sy = 0; sy < N; sy++)
                    for (int sx = 0; sx < N; sx++)
                    {
                        float u = x + (sx + 0.5f) / N, v = h - y - 1 + (sy + 0.5f) / N;
                        if (u < x0 || u > x1 || v < y0 || v > y1) continue;
                        float cx = Mathf.Clamp(u, x0 + rad, x1 - rad), cy = Mathf.Clamp(v, y0 + rad, y1 - rad);
                        if ((u - cx) * (u - cx) + (v - cy) * (v - cy) <= rad * rad) hit++;
                    }
                    keep *= 1f - alpha * (1f - (i - 1f) / 8f) * hit / (N * N);
                }
                px[y * w + x] = new Color32(0, 0, 0, (byte)Mathf.RoundToInt((1f - keep) * 255f));
            }
            var tex = new Texture2D(w, h, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
            tex.SetPixels32(px);
            tex.Apply();
            var style = new GUIStyle();
            style.normal.background = tex;
            // 늘리지 않는 가장자리: 모든 겹의 둥근 모서리를 덮음(좌우 = 여백 + 모서리, 위 = 여백 + 모서리 + 6, 아래 = 모서리 + 12)
            int side = Mathf.CeilToInt(fx + rp) + 2;
            style.border = new RectOffset(side, side, Mathf.CeilToInt(fy + rp + 6f * S) + 2, Mathf.CeilToInt(rp + 12f * S) + 2);
            return style;
        }

        static GUIStyle NineSlice(Texture2D tex, int border)
        {
            var style = new GUIStyle();
            style.normal.background = tex;
            style.border = new RectOffset(border, border, border, border);
            return style;
        }

        /// <summary>radius 화소 둥근 사각 텍스처. lineWidth > 0이면 테두리만.</summary>
        static Texture2D MakeRounded(int radius, int lineWidth)
        {
            int size = radius * 2 + 2;
            var tex = new Texture2D(size, size, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
            var px = new Color32[size * size];
            float r = radius, ir = Mathf.Max(0f, r - lineWidth);
            // 화소마다 8×8 표본으로 실제로 덮인 넓이를 셈 — 거리 근사는 모서리 호가 두 화소에 반씩 번져 곧은 변보다 얇고 얼룩져 보였음.
            const int N = 8;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                int hit = 0;
                for (int sy = 0; sy < N; sy++)
                for (int sx = 0; sx < N; sx++)
                {
                    float u = x + (sx + 0.5f) / N, v = y + (sy + 0.5f) / N;
                    if (InRounded(u, v, 0f, size, r) && !(lineWidth > 0 && InRounded(u, v, lineWidth, size - lineWidth, ir))) hit++;
                }
                px[y * size + x] = new Color32(255, 255, 255, (byte)(hit * 255 / (N * N)));
            }
            tex.SetPixels32(px);
            tex.Apply();
            return tex;
        }

        /// <summary>점 (u, v)가 [lo, hi] 정사각의 반지름 rad 둥근 사각 안인가(rad 0 = 각진 사각).</summary>
        static bool InRounded(float u, float v, float lo, float hi, float rad)
        {
            if (u < lo || u > hi || v < lo || v > hi) return false;
            float cx = Mathf.Clamp(u, lo + rad, hi - rad), cy = Mathf.Clamp(v, lo + rad, hi - rad);
            return (u - cx) * (u - cx) + (v - cy) * (v - cy) <= rad * rad;
        }

    }
}
