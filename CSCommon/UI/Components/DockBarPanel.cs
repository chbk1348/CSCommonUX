using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.DockBar_Panel — 독(Dockbar · SubDockBar · 안내 알약) 아래 12 에 붙는 설정 판(독 버튼으로 열고 닫음 — 열린 동안 그 버튼은 파스텔로 켬).
    /// 판 반지름 28 · 안쪽 16 · 줄 사이 16 · 1px Look.BarLine · 그림자 없음, 면 Look.BarSurface. 열 때 위 가장자리 기준으로 높이 0 → 판 높이 0.44초 cubic-bezier(.2,.8,.2,1)
    /// (Modal.Expand 재사용 — 투명도 0.28초) · 12 위에서 내려오며 0.98 → 1배, 닫을 때 거꾸로. 부품: 알약 탭 줄(Tabs, 36) · 5열 칸 격자(Cell — 위 이름 12 · 아래 입력 36).
    /// 쓰는 법: if (DockBarPanel.Begin(id, open, width, height, out var body, out var frame)) { … body 안(안쪽 좌표)에 그림 …; DockBarPanel.End(); } — frame = 지도 클릭 막을 화면 칸.
    /// </summary>
    public static class DockBarPanel
    {
        /// <summary>반지름 · 안쪽 · 줄 사이 · 탭 높이 · 탭 좌우 · 탭 사이 · 탭 글 · 칸 열 수 · 칸 이름 글 · 이름 줄 높이 · 이름↔입력 · 입력 높이 · 열 때 내려오는 거리 · 시작 배율.</summary>
        public const float Radius = 28f, Pad = 16f, Gap = 16f, TabH = 36f, TabPadX = 16f, TabGap = 8f, TabFont = 13.5f;
        public const int Cols = 5;
        public const float LabelFont = 12f, LabelH = 16f, LabelGap = 6f, CellH = 36f, Rise = 12f, ScaleFrom = 0.98f;

        static float anchor = -1f;
        static int anchorFrame = -1;

        /// <summary>Dockbar 가 그릴 때마다 남기는 판 위 y(독 맨 아래 + 12).</summary>
        internal static void Anchor(float y)
        {
            anchor = y;
            anchorFrame = Time.frameCount;
        }

        /// <summary>판 위 y: 이번 · 지난 프레임 Dockbar 아래 + 12(독이 없으면 화면 위 CSCommonDesign.BarTop).</summary>
        public static float Top => anchorFrame >= Time.frameCount - 1 && anchor >= 0f ? anchor : CSCommonDesign.BarTop;

        static readonly Stack<Vector4> saved = new(); // x = 투명도, y = 내린 거리, z = 배율을 썼나
        static readonly Stack<Matrix4x4> savedMatrix = new();

        /// <summary>
        /// 판 그리기 시작(화면 가운데 · 위 = Top): 펼친 정도만큼 잘라 보이고 흐리게 · 내려오며 커짐. 다 닫혔으면 false(End 부르지 말 것).
        /// body = 안쪽 칸(잘라 그리기 안쪽 좌표), frame = 지금 보이는 판의 화면 칸(입력 막기).
        /// </summary>
        public static bool Begin(string id, bool open, float width, float height, out Rect body, out Rect frame)
        {
            float t = Modal.Expand(CSCommonAnim.Key("dockbarpanel", id), open, out float fade);
            body = frame = default;
            if (t <= 0.001f && !open) return false;
            var r = new Rect(PillBar.Left(Draw.Width * 0.5f, width), Top, width, height);
            float dy = Mathf.Round(-Rise * (1f - t)), s = Mathf.Lerp(ScaleFrom, 1f, t);
            saved.Push(new Vector4(Draw.Opacity, dy, s < 1f ? 1f : 0f, 0f));
            Draw.Opacity *= fade;
            Draw.Shift(new Vector2(0f, dy));
            if (s < 1f)
            {
                savedMatrix.Push(GUI.matrix);
                GUIUtility.ScaleAroundPivot(new Vector2(s, s), Draw.ToScreen(new Vector2(r.center.x, r.y)));
            }
            // 1px 선이 잘리지 않게 바깥 1 을 더 둠
            float shown = Mathf.Round((height + 2f) * t);
            frame = new Rect(r.x, r.y + dy, r.width, Mathf.Max(0f, shown - 2f));
            // 앞 판: 밑(창 몸통)의 단추 · 호버가 이 칸 안 클릭을 못 받게(End 에서 EndOverlay). 지금 좌표는 이미 dy 만큼 밀림
            Draw.Block(new Rect(r.x, r.y, r.width, frame.height));
            Draw.BeginOverlay();
            Draw.BeginClip(new Rect(r.x - 1f, r.y - 1f, r.width + 2f, shown), Vector2.zero);
            var plate = new Rect(1f, 1f, width, height);
            if (Draw.Repaint)
            {
                Draw.Round(plate, Look.BarSurface, Radius);
                Draw.RoundOutline(plate, Look.BarLine, Radius);
            }
            body = new Rect(plate.x + Pad, plate.y + Pad, width - Pad * 2f, height - Pad * 2f);
            return true;
        }

        public static void End()
        {
            if (saved.Count == 0) return;
            Draw.EndOverlay();
            Draw.EndClip();
            var v = saved.Pop();
            if (v.z > 0f && savedMatrix.Count > 0) GUI.matrix = savedMatrix.Pop();
            Draw.Shift(new Vector2(0f, -v.y));
            Draw.Opacity = v.x;
        }

        /// <summary>
        /// 알약 탭 줄(높이 36 · 좌우 16 · 사이 8 · 글 13.5 굵게): 고른 칸 = 모드 파스텔 채움, 나머지 = 1px 선 알약(호버 옅은 면) · 누르면 0.96배.
        /// x 왼쪽 · y 위. 눌러서 뗀 번호(없으면 -1), width = 줄 폭.
        /// </summary>
        public static int Tabs(float x, float y, string[] labels, int cur, out float width)
        {
            int hit = -1;
            float x0 = x;
            for (int i = 0; i < labels.Length; i++)
            {
                float w = Mathf.Ceil(Draw.TextWidth(labels[i], TabFont, true)) + TabPadX * 2f;
                var r = new Rect(x, y, w, TabH);
                bool sel = i == cur;
                int k = Look.Key(r, "dockbarpanel.tab", labels[i]);
                float hv = Look.Hover(k, r, !sel), pr = Look.Press(k, r);
                if (Draw.Repaint)
                {
                    float rad = TabH * 0.5f;
                    Draw.BeginScale(r.center, Look.PressScale(pr, PillBar.PressScale));
                    if (sel) Draw.Round(r, Look.PastelFill, rad);
                    else
                    {
                        if (hv > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Haze, PillBar.HoverAlpha * hv), rad);
                        Draw.RoundOutline(r, Look.BarLine, rad);
                    }
                    Draw.Text(r, labels[i], TabFont, sel ? Look.PastelInk : Look.BarInk, true, TextAnchor.MiddleCenter);
                    Draw.EndScale();
                }
                if (Draw.Button(r)) hit = i;
                x += w + TabGap;
            }
            width = labels.Length > 0 ? x - x0 - TabGap : 0f;
            return hit;
        }

        /// <summary>칸 격자 한 줄 높이(이름 + 사이 + 입력).</summary>
        public const float RowH = LabelH + LabelGap + CellH;

        /// <summary>칸 count 개 격자 높이(5열, 줄 사이 16).</summary>
        public static float GridHeight(int count)
        {
            int rows = (count + Cols - 1) / Cols;
            return rows <= 0 ? 0f : rows * RowH + (rows - 1) * Gap;
        }

        /// <summary>5열 칸 격자(사이 16)의 i 번째 칸: 위에 이름(12 · Pal.Dim)을 그리고 아래 입력 자리(높이 36, Textfield · Select · Stepper · Segmented · Switch · Button 이 씀)를 돌려줌.</summary>
        public static Rect Cell(Rect grid, int i, string label)
        {
            float cw = Mathf.Floor((grid.width - Gap * (Cols - 1)) / Cols);
            var c = new Rect(grid.x + (i % Cols) * (cw + Gap), grid.y + (i / Cols) * (RowH + Gap), cw, RowH);
            if (Draw.Repaint && !string.IsNullOrEmpty(label)) Draw.TextFit(new Rect(c.x, c.y, cw, LabelH), label, LabelFont, Pal.Dim);
            return new Rect(c.x, c.y + LabelH + LabelGap, cw, CellH);
        }
    }
}
