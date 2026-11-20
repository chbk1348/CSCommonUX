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
        /// fabIcon 이 있으면 알약 오른쪽 12 에 지름 64 설정 FAB(fabSelected 면 파스텔 채움, fabLabel = 툴팁)를 둠. 알약은 늘 창 가운데(FAB 는 바깥 오른쪽).
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
            var bar = new Rect(PillBar.Left(window.center.x, barW), Top(window), barW, PillBar.Height); // 알약은 늘 창 가운데(서브 알약과 같은 축) · FAB 는 그 오른쪽 12
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
                PillBar.Cell(c, icon?.Invoke(i), name, lwBuf[i], PillBar.Open(wBuf[i], PillBar.CellW(lwBuf[i])), Look.Press(k, c));
                extra?.Invoke(i, c, sel);
                if (!sel) CSCommonUX.CSCommonToolbar.Tip(c, name);
                if (Draw.Button(c)) hit = i;
                x += wBuf[i] + PillBar.DockGap;
            }
            if (fab) fabHit = Fab.In(new Rect(bar.xMax + PillBar.FabGap, bar.y, PillBar.FabSize, PillBar.FabSize), fabIcon, FabTone.Plain, fabSelected, fabLabel);
            Draw.EndOverlay();
            return hit;
        }

        /// <summary>
        /// 상세 화면 뒤로 FAB: 알약 왼쪽 FabGap 에 지름 FabSize 원(꺾쇠 왼쪽, 설정 FAB 와 대칭). count · selectedLabel 은 Bar 에 넘긴 칸 수 · 고른 칸 이름 —
        /// 알약 폭은 고른 칸이 다 열린 값으로 셈(상세 안에서는 탭이 안 바뀜). tooltip = 툴팁 · 접근성 이름("○○으로"). Bar 다음에 부를 것. 눌러서 떼면 참.
        /// </summary>
        public static bool BackFab(Rect window, int count, string selectedLabel, string tooltip)
        {
            if (count <= 0) return false;
            float sum = PillBar.IdleW * (count - 1) + PillBar.CellW(Mathf.Ceil(Draw.TextWidth(selectedLabel ?? "", PillBar.Font, true)));
            float left = PillBar.Left(window.center.x, PillBar.Width(sum, count, PillBar.DockGap, PillBar.PadX));
            var b = new Rect(left - PillBar.FabGap - PillBar.FabSize, Top(window), PillBar.FabSize, PillBar.FabSize);
            Draw.Block(b);
            Draw.BeginOverlay();
            bool hit = Fab.In(b, Icons.Left, FabTone.Plain, false, tooltip);
            Draw.EndOverlay();
            return hit;
        }
    }
}
