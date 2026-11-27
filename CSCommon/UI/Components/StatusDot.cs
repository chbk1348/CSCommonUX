using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 상태 점(지름 8, 줄 안에서 글과 사이 8) · 툴바 칸 구석 점 · 서브 칸 안 읽은 수 알약(FIA 캔버스 R5TokColor · R5Nav · R5PartRows).
    /// 색은 부르는 쪽이 고름: Pal.SemGood · SemWarn · SemBad · 시리즈 강조. 툴바 점은 모드가 Toolbar.Bar(extra) · SubToolBar.Bar(extra, extraW) 콜백에서 Corner 로 그림(툴바 파일은 그대로).
    /// </summary>
    public static class StatusDot
    {
        /// <summary>줄 안 상태 점. live = '세션 진행 중'(투명도 1 ↔ 0.4, 1.2초마다 오감) — 나머지 점은 멈춤.</summary>
        public static void Paint(Vector2 center, Color c, bool live = false)
        {
            if (!Draw.Repaint) return;
            Draw.Dot(center, D35.Dot * 0.5f, live ? Draw.Alpha(c, LiveAlpha(Time.unscaledTime)) : c);
        }

        /// <summary>진행 중 점 투명도: 1.2초마다 1 → 0.4 → 1 로 오감(곡선 Ease, CSS alternate 와 같음).</summary>
        public static float LiveAlpha(float time)
        {
            float k = Mathf.Repeat(time, D35.LiveBlink * 2f) / D35.LiveBlink; // 0 ~ 2
            float e = k < 1f ? D35.Ease(k) : D35.Ease(2f - k);
            return Mathf.Lerp(1f, D35.LiveAlphaMin, e);
        }

        /// <summary>툴바 칸 구석 점 가운데: 메인 칸 위 6 · 오른쪽 6, 서브 칸(sub) 위 4 · 오른쪽 6. 칸 폭은 그대로.</summary>
        public static Vector2 CornerCenter(Rect cell, bool sub)
        {
            float r = D35.Dot * 0.5f;
            return new Vector2(cell.xMax - (sub ? D35.DotSubRight : D35.DotMainRight) - r, cell.y + (sub ? D35.DotSubTop : D35.DotMainTop) + r);
        }

        /// <summary>
        /// 툴바 칸 오른쪽 위 점 8 + 둘레 2(ring = 알약 바탕색, 기본 Look.BarSurface). 우선순위 진행 중(SemBad) &gt; 새 소식 · 안 읽은 수(시리즈 강조)는 부르는 쪽이 정함.
        /// 진행 중은 고른 칸에도, 새 소식 · 수는 안 고른 칸만. 뜻은 툴팁 · 이름에('레이스 · 진행 중').
        /// </summary>
        public static void Corner(Rect cell, Color c, bool sub = false) => Corner(cell, c, Look.BarSurface, sub);

        public static void Corner(Rect cell, Color c, Color ring, bool sub)
        {
            if (!Draw.Repaint) return;
            var p = CornerCenter(cell, sub);
            Draw.Dot(p, D35.Dot * 0.5f + D35.DotRing, ring);
            Draw.Dot(p, D35.Dot * 0.5f, c);
        }

        static string[] counts;

        /// <summary>안 읽은 수 글: 1 ~ 99 그대로, 100 이상 '99+', 0 이하 null(그리지 않음). 글은 한 번 만들어 둠(할당 0).</summary>
        public static string CountText(int n)
        {
            if (n <= 0) return null;
            if (counts == null)
            {
                counts = new string[101];
                for (int i = 1; i < 100; i++) counts[i] = i.ToString();
                counts[100] = "99+";
            }
            return counts[Mathf.Min(n, 100)];
        }

        /// <summary>숫자 알약 폭(n 이 0 이하면 0): 좌우 8 + 글 폭.</summary>
        public static float CountWidth(int n)
        {
            string t = CountText(n);
            return t == null ? 0f : Draw.NumWidth(t, D35.FontCaption) + D35.CountPadX * 2f;
        }

        /// <summary>
        /// 서브 칸 안 읽은 수 알약(높이 20 · 반지름 10 · 면 GroundPage · 글 12 InkTxt): 왼쪽 x · 세로 가운데 centerY 에 그리고 폭을 돌려줌. 안 고른 서브 칸에서만 부를 것.
        /// 칸 글 뒤 8(CountGap) 에 둠.
        /// </summary>
        public static float Count(float x, float centerY, int n)
        {
            float w = CountWidth(n);
            if (w <= 0f || !Draw.Repaint) return w;
            var r = new Rect(x, centerY - D35.CountH * 0.5f, w, D35.CountH);
            Draw.Round(r, Pal.GroundPage, D35.CountH * 0.5f);
            Draw.Num(r, CountText(n), D35.FontCaption, Pal.InkTxt, TextAnchor.MiddleCenter);
            return w;
        }
    }
}
