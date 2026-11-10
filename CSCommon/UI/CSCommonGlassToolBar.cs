using System;
using UnityEngine;

namespace CSCommon
{
    /// <summary>
    /// .GlassToolBar — 하단 툴바(iOS 27 리퀴드 글래스, 디자인 아티팩트 「CSCommon 컴포넌트 · 하단 툴바」).
    /// 창 아래에 떠 있는 유리 알약. 가만히 선택된 칸은 옅은 배경과 강조색 아이콘 · 글자만, 유리 렌즈는 칸을 옮기거나 누르는 동안에만 나타남
    /// (두 칸에 걸쳐 늘어났다 도착하면 가라앉음 · 누르면 94%로 작아지며 밝아짐).
    /// 내용은 이 판 밑까지 이어지고, 스크롤 끝에서만 SafeH 만큼 더 올라와 마지막 줄이 가려지지 않음(호출쪽: 내용 끝에 SafeH 를 더함).
    /// 게임 IMGUI 에는 뒤 화면 블러가 없어 어두운 유리 바탕 + 위 반짝임 + 가는 테두리로 어림함.
    /// </summary>
    public static class GlassToolBar
    {
        /// <summary>판 높이 · 창 아래 여백 · 스크롤 끝 안전 영역(판 + 여백 + 숨) · 안쪽 여백 · 모서리.</summary>
        public const float Height = 60f, Bottom = 14f, SafeH = 84f, Pad = 5f, Radius = 30f, CellH = 50f;

        /// <summary>호버 · 선택 바탕: 다크는 흰색, 라이트는 어두운 잉크(흰색은 밝은 유리 위에서 안 보임).</summary>
        static Color Tint(float a) => Draw.Alpha(CSCommonTheme.Light ? Look.Ink : Color.white, a);

        /// <summary>창 r 아래 가운데에 놓이는 판 자리와 칸 폭(창이 좁아 넘치면 칸을 줄임).</summary>
        public static Rect BarRect(Rect window, int count, float maxCellW, out float cellW)
        {
            cellW = Mathf.Floor(Mathf.Min(maxCellW, (window.width - 40f - Pad * 2f) / Mathf.Max(1, count)));
            float w = cellW * count + Pad * 2f;
            return new Rect(window.center.x - w * 0.5f, window.yMax - Bottom - Height, w, Height);
        }

        /// <summary>
        /// 판 + 렌즈를 그리고 칸마다 cell(칸 번호, 자리, 고름, 마우스 올림 0~1)을 불러 내용을 그리게 함. 눌러서 뗀 칸 번호(없으면 -1).
        /// 모든 이벤트 종류에서 불러야 함(컨트롤 순서). id 는 같은 화면에 판이 둘일 때만 다르게.
        /// </summary>
        public static int Bar(Rect window, int count, int cur, Action<int, Rect, bool, float> cell, float maxCellW = 88f, string id = "main")
        {
            if (count <= 0) return -1;
            var bar = BarRect(window, count, maxCellW, out float cw);
            var cells = new Rect[count];
            int pressed = -1;
            for (int i = 0; i < count; i++)
            {
                cells[i] = new Rect(bar.x + Pad + i * cw, bar.y + Pad, cw, CellH);
                if (pressed < 0 && Draw.Pressed(cells[i])) pressed = i;
            }

            if (Draw.Repaint)
            {
                Draw.Round(bar, Draw.Alpha(Pal.Win, 0.68f), Radius);
                Draw.Round(bar, new Color(1f, 1f, 1f, 0.07f), Radius);
            }
            // 위 반짝임: 판과 중심이 같은 안쪽 알약(반지름 Radius - 2)의 위 46% 만 잘라 그림. 예전처럼 높이 46% 칸에 반지름 28 을 주면
            // 칸 절반(약 14)으로 줄어든 작은 모서리가 판 모서리(30) 밖으로 최대 4px 삐져나와 위 모서리가 계단처럼 보였음.
            // 잘라내기(GUI.BeginGroup)는 컨트롤 번호를 쓰므로 모든 이벤트에서 부름(Round 는 Repaint 때만 그림).
            Draw.BeginClip(new Rect(bar.x, bar.y, bar.width, 2f + bar.height * 0.46f), Vector2.zero);
            Draw.Round(new Rect(2f, 2f, bar.width - 4f, bar.height - 4f), new Color(1f, 1f, 1f, 0.06f), Radius - 2f);
            Draw.EndClip();

            // 렌즈: 앞쪽(head)이 먼저 도착하고 뒤쪽(tail)이 늦게 따라와 옮기는 동안 두 칸에 걸쳐 늘어남. 누른 칸이 있으면 그 칸 밑으로
            int target = pressed >= 0 ? pressed : Mathf.Clamp(cur, 0, count - 1);
            float head = CSCommonAnim.Smooth(CSCommonAnim.Key("glasstoolbar.head", id), target * cw, 22f);
            float tail = CSCommonAnim.Smooth(CSCommonAnim.Key("glasstoolbar.tail", id), target * cw, 11f);
            bool moving = Mathf.Abs(head - tail) > 1.5f;
            float amt = CSCommonAnim.Toward(CSCommonAnim.Key("glasstoolbar.glass", id), moving || pressed >= 0 ? 1f : 0f, 16f);
            if (Draw.Repaint)
            {
                float left = Mathf.Min(head, tail), right = Mathf.Max(head, tail) + cw;
                var lens = new Rect(bar.x + Pad + left - 2f * amt, bar.y + Pad - 2f * amt, right - left + 4f * amt, CellH + 4f * amt);
                if (pressed >= 0) lens = new Rect(lens.x + 1.5f * amt, lens.y + 1.5f * amt, lens.width - 3f * amt, lens.height - 3f * amt);
                float round = lens.height * 0.5f;
                // 가만히 고른 칸: 떠오른 면(Look.Raised, 테마에 맞는 밝은 면 + 선). 옮기거나 누르는 동안에는 유리 렌즈가 그 위로 나타남
                if (amt < 0.98f) Look.Raised(new Rect(bar.x + Pad + left, bar.y + Pad, right - left, CellH), round, 1f - amt);
                if (amt > 0.02f) Draw.Round(lens, new Color(1f, 1f, 1f, (pressed >= 0 ? 0.26f : 0.22f) * amt), round);
            }

            int hit = -1;
            for (int i = 0; i < count; i++)
            {
                bool sel = i == cur;
                float hover = Look.Hover(Look.Key(cells[i], "glasstab", id + i), cells[i], !sel);
                cell?.Invoke(i, cells[i], sel, hover);
                if (Draw.Button(cells[i])) hit = i;
            }
            return hit;
        }

        /// <summary>칸 기본 내용: 위 아이콘(22) + 아래 이름(12). 고른 칸은 강조색(ink) 굵게, 나머지는 보조 글색. 마우스를 올리면 옅게 밝아짐.</summary>
        public static void Cell(Rect cell, string icon, string label, bool sel, float hover, Color? ink = null)
        {
            if (!Draw.Repaint) return;
            Color on = ink ?? Pal.Accent;
            // 마우스 올림: 테마에 맞는 옅은 바탕 · 가만히 고른 칸: 강조색 옅은 바탕(떠오른 면 위에 얹어 라이트에서도 보임)
            if (hover > 0.01f) Draw.Round(cell, Tint(0.10f * hover), cell.height * 0.5f);
            if (sel) Draw.Round(cell, Draw.Alpha(on, 0.16f), cell.height * 0.5f);
            if (!string.IsNullOrEmpty(icon)) Draw.Icon(icon, new Rect(cell.x + (cell.width - 22f) * 0.5f, cell.y + 5f, 22f, 22f), sel ? on : Pal.Sub);
            Draw.Text(new Rect(cell.x, cell.yMax - 20f, cell.width, 16f), label, 12f, sel ? on : Pal.Sub, sel, TextAnchor.MiddleCenter);
        }
    }
}
