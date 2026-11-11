using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Toolbar — 모드 창 하단 툴바(시안 'Toolbar · SubToolBar'): 창 아래 가운데 불투명 알약(높이 64 · 안쪽 8 · 칸 48 · 반지름 = 높이/2 · 칸 사이 2 · 창 아래 14)
    /// + 오른쪽 12 에 지름 64 설정 FAB. 그림자 없음 — Look.BarSurface + 1px Look.BarLine 만. 칸 모양 · 계산 · 뜯어져 나오기는 CSCommonUX.PillBar.
    /// 내용은 이 판 밑까지 이어지고, 스크롤 끝에서만 SafeH 만큼 더 올라와 마지막 줄이 가려지지 않음(호출쪽: 내용 끝에 SafeH, 서브가 있으면 SubToolBar.SafeH 를 더함).
    /// </summary>
    public static class Toolbar
    {
        /// <summary>창 아래 여백 · 스크롤 끝 안전 영역(판 + 여백 + 숨 10).</summary>
        public const float Bottom = 14f, SafeH = PillBar.Height + Bottom + 10f;

        static float[] wBuf = new float[0], lwBuf = new float[0];

        /// <summary>창 window 아래에 놓이는 메인 알약의 위 y.</summary>
        internal static float Top(Rect window) => window.yMax - Bottom - PillBar.Height;

        /// <summary>
        /// 하단 툴바: 안 고른 칸 = 아이콘만(폭 44), 고른 칸 = 모드 파스텔 채움 + [아이콘 24 · 8 · 이름 14 굵게] 가운데(폭 = 24 + 8 + 글폭 + 28).
        /// 고른 칸을 옮기면 칸 폭이 CSCommonAnim.Smooth 로 늘고 줄며 글은 열린 만큼 나타남. 누르면 0.96배, 안 고른 칸 호버 = 옅은 알약. cur = -1 이면 고른 칸 없음(설정 FAB 를 고름).
        /// fabIcon 이 있으면 알약 오른쪽 12 에 지름 64 설정 FAB(fabSelected 면 파스텔 채움, fabLabel = 툴팁)를 두고 둘을 묶어 창 가운데 정렬.
        /// extra(칸 번호, 칸, 고름) 로 칸 위에 뱃지 등을 덧그림. 눌러서 뗀 칸 번호(없으면 -1), FAB 를 눌러 떼면 fabHit.
        /// 서브 알약이 있으면 SubToolBar.Bar 를 이것보다 먼저 부를 것(서브 알약이 이 알약 뒤 층에서 뜯어져 나옴). 모든 이벤트 종류에서 불러야 함.
        /// </summary>
        public static int Bar(Rect window, int count, int cur, Func<int, string> icon, Func<int, string> label, string fabIcon, string fabLabel, bool fabSelected, out bool fabHit, Action<int, Rect, bool> extra = null, string id = "main")
        {
            fabHit = false;
            if (count <= 0 || label == null) return -1;
            if (wBuf.Length < count)
            {
                wBuf = new float[count];
                lwBuf = new float[count];
            }
            int wKey = CSCommonAnim.Key("toolbar.w", id), tabKey = CSCommonAnim.Key("toolbar.tab", id);
            float sum = 0f;
            for (int i = 0; i < count; i++)
            {
                lwBuf[i] = Mathf.Ceil(Draw.TextWidth(label(i), PillBar.Font, true));
                wBuf[i] = CSCommonAnim.Smooth(wKey ^ (i + 1) * 7919, i == cur ? PillBar.CellW(lwBuf[i]) : PillBar.IdleW, 18f);
                sum += wBuf[i];
            }
            bool fab = !string.IsNullOrEmpty(fabIcon);
            float barW = PillBar.Width(sum, count, PillBar.DockGap, PillBar.PadX);
            // shortcut: 창보다 넓어도 칸을 줄이지 않음(넘침) — 탭이 아주 많은 모드가 생기면 안 고른 칸 폭부터 줄일 것
            var bar = new Rect(PillBar.Left(window.center.x, barW + (fab ? PillBar.FabGap + PillBar.FabSize : 0f)), Top(window), barW, PillBar.Height);
            // 알약 판 + 설정 FAB 줄 전체(칸 사이 · 여백 · FAB 사이 포함)가 클릭 · 호버 · 휠을 먹음 — 먼저 그린 창 몸통 단추로 새지 않게
            Draw.Block(new Rect(bar.x, bar.y, barW + (fab ? PillBar.FabGap + PillBar.FabSize : 0f), PillBar.Height));
            Draw.BeginOverlay();
            PillBar.Plate(bar);

            int hit = -1;
            float x = bar.x + PillBar.PadX;
            for (int i = 0; i < count; i++)
            {
                var c = new Rect(x, bar.y + PillBar.Pad, wBuf[i], PillBar.CellH);
                bool sel = i == cur;
                int k = tabKey ^ (i + 1) * 7919;
                string name = label(i);
                PillBar.Cell(c, icon?.Invoke(i), name, lwBuf[i], PillBar.Open(wBuf[i], PillBar.CellW(lwBuf[i])), Look.Hover(k, c, !sel), Look.Press(k, c));
                extra?.Invoke(i, c, sel);
                if (!sel) CSCommonUX.UI.CSCommonToolbar.Tip(c, name);
                if (Draw.Button(c)) hit = i;
                x += wBuf[i] + PillBar.DockGap;
            }
            if (fab) fabHit = Fab.In(new Rect(bar.xMax + PillBar.FabGap, bar.y, PillBar.FabSize, PillBar.FabSize), fabIcon, FabTone.Plain, fabSelected, fabLabel);
            Draw.EndOverlay();
            return hit;
        }
    }

    /// <summary>
    /// CSCommonUX.SubToolBar — 하단 툴바 위 서브 알약(하위 화면 전환): 메인 알약 위 12 가운데(높이 48 · 안쪽 8 · 칸 32 · 칸 폭 = 글폭 + 28, 최소 64 · 칸 사이 4 · 글 13 가운데).
    /// 고른 칸 = 서브 파스텔 채움 + 서브 파스텔 글, 안 고른 칸 = 보조 글(dim 이 참인 칸(잠김)은 흐린 글).
    /// </summary>
    public static class SubToolBar
    {
        /// <summary>서브 알약이 있을 때 스크롤 끝 안전 영역(툴바 안전 영역 + 알약 + 간격).</summary>
        public const float SafeH = Toolbar.SafeH + PillBar.SubH + PillBar.SubGap;

        /// <summary>숨는 동안 그릴 마지막 서브 알약(이름 · 칸 폭 · 고른 칸).</summary>
        sealed class Snap
        {
            public string[] Labels = new string[0];
            public float[] W = new float[0];
            public int Count, Cur;
        }

        static readonly Dictionary<int, Snap> snaps = new();

        /// <summary>
        /// show(그리고 칸이 둘 이상)가 켜지면 메인 알약 뒤에서 납작하게(가로 62% · 세로 55% · 아래로 68) 숨어 있다 올라오며 뜯어져 나오고(0.46초, 끝에서 살짝 튕김 · 투명 0.22초),
        /// 꺼지면 마지막 모습으로 거꾸로 메인 속으로 들어감 — 숨는 동안 계속 show = false 로 불러야 함. 다 나오기 전에는 클릭을 받지 않음.
        /// extraW · extra 로 칸 오른쪽에 뱃지 자리(폭)와 그림. 메인 알약 뒤 층이 되도록 Toolbar.Bar 보다 먼저 부를 것. 눌러서 뗀 칸 번호(없으면 -1). 모든 이벤트 종류에서 불러야 함.
        /// </summary>
        public static int Bar(Rect window, bool show, int count, int cur, Func<int, string> label, Func<int, float> extraW = null, Action<int, Rect> extra = null, Func<int, bool> dim = null, string id = "sub")
        {
            bool on = show && count > 1 && label != null;
            int key = CSCommonAnim.Key("subtoolbar", id);
            float t = PillBar.TearProgress(key, on);
            if (t <= 0f) return -1;
            if (!snaps.TryGetValue(key, out var s)) snaps[key] = s = new Snap();
            if (on)
            {
                if (s.Labels.Length < count)
                {
                    s.Labels = new string[count];
                    s.W = new float[count];
                }
                s.Count = count;
                s.Cur = cur;
                for (int i = 0; i < count; i++)
                {
                    s.Labels[i] = label(i);
                    s.W[i] = PillBar.SubCellW(Mathf.Ceil(Draw.TextWidth(s.Labels[i], PillBar.SubFont, true)), extraW?.Invoke(i) ?? 0f);
                }
            }
            float sum = 0f;
            for (int i = 0; i < s.Count; i++) sum += s.W[i];
            float total = PillBar.Width(sum, s.Count, PillBar.SubCellGap, PillBar.SubPad);
            var bar = new Rect(PillBar.Left(window.center.x, total), Toolbar.Top(window) - PillBar.SubGap - PillBar.SubH, total, PillBar.SubH);
            bool live = on && t >= 1f;

            int hit = -1;
            // 보이는 동안(숨는 중 포함) 서브 알약 판 전체가 입력을 먹음
            Draw.Block(bar);
            Draw.BeginOverlay();
            PillBar.BeginTear(bar, t, PillBar.SubGap + PillBar.Height - PillBar.Pad);
            PillBar.Plate(bar);
            float x = bar.x + PillBar.SubPad;
            for (int i = 0; i < s.Count; i++)
            {
                var c = new Rect(x, bar.y + PillBar.SubPad, s.W[i], PillBar.SubCellH);
                bool sel = i == s.Cur;
                int k = key ^ (i + 1) * 7919;
                float ew = on ? (extraW?.Invoke(i) ?? 0f) : 0f;
                PillBar.SubCell(c, s.Labels[i], sel, on && !sel && dim != null && dim(i), Look.Hover(k, c, live && !sel), Look.Press(k, c, live), ew);
                // 이름이 있으면 [이름 · 뱃지] 의 오른쪽 끝, 이름이 없으면(아이콘만 — 언론 탭 인스타그램) 칸 가운데
                float ex = string.IsNullOrEmpty(s.Labels[i]) ? Mathf.Round(c.center.x - ew * 0.5f) : c.xMax - PillBar.SubCellPadX - ew;
                if (ew > 0f) extra?.Invoke(i, new Rect(ex, c.y, ew, c.height));
                if (live && Draw.Button(c)) hit = i;
                x += s.W[i] + PillBar.SubCellGap;
            }
            PillBar.EndTear();
            Draw.EndOverlay();
            return hit;
        }
    }
}
