using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
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
                PillBar.SubCell(c, s.Labels[i], sel, on && !sel && dim != null && dim(i), Look.Press(k, c, live), ew);
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
