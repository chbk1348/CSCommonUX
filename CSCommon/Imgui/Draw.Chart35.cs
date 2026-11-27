using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 3.5 차트 바탕: Catmull-Rom 부드러운 선 · 위만 둥근 막대

        /// <summary>Smooth 가 다시 쓰는 꺾은선 버퍼(매 틀 new List 없음 — 3.0 Curve 와 다름).</summary>
        static readonly List<Vector2> smoothLine = new(256);

        /// <summary>
        /// 부드러운 선(3.5 차트): 점을 모두 지나는 Catmull-Rom 을 3차 베지어로 바꿔(SmoothInto) 굵기 width 꺾은선으로 그림. 둥근 끝 · 이음, 채움 없음.
        /// 조절점 y 는 [yMin, yMax](그림 영역 위 · 바닥) 안으로 자름 — 선이 영역 밖으로 부풀지 않게. 3.0 Curve(가운데 x 조절점)와 모양이 다름.
        /// </summary>
        public static void Smooth(List<Vector2> pts, float width, Color c, float yMin, float yMax)
        {
            if (!Repaint || pts == null || pts.Count < 2 || c.a <= 0f) return;
            SmoothInto(smoothLine, pts, yMin, yMax);
            Polyline(smoothLine, width, c);
        }

        /// <summary>점 p 를 지나는 부드러운 꺾은선을 into 에 채움(먼저 비움): 첫 점 + 구간마다 CurveSteps 개.</summary>
        internal static void SmoothInto(List<Vector2> into, List<Vector2> p, float yMin, float yMax)
        {
            into.Clear();
            if (p == null || p.Count == 0) return;
            into.Add(p[0]);
            for (int i = 0; i + 1 < p.Count; i++)
            {
                CatmullControls(p, i, yMin, yMax, out var c1, out var c2);
                Cubic(into, p[i], c1, c2, p[i + 1]);
            }
        }

        /// <summary>
        /// 구간 i(p[i] → p[i+1])의 베지어 조절점: c1 = p[i] + (p[i+1] − p[i−1]) / 6 · c2 = p[i+1] − (p[i+2] − p[i]) / 6.
        /// 끝 밖 점은 끝점 자신. 조절점 y 는 [yMin, yMax] 로 자름(yMin &gt; yMax 면 둘을 바꿔 봄).
        /// </summary>
        internal static void CatmullControls(List<Vector2> p, int i, float yMin, float yMax, out Vector2 c1, out Vector2 c2)
        {
            int last = p.Count - 1;
            Vector2 p0 = p[Mathf.Max(0, i - 1)], p1 = p[i], p2 = p[Mathf.Min(last, i + 1)], p3 = p[Mathf.Min(last, i + 2)];
            c1 = p1 + (p2 - p0) / 6f;
            c2 = p2 - (p3 - p1) / 6f;
            float lo = Mathf.Min(yMin, yMax), hi = Mathf.Max(yMin, yMax);
            c1.y = Mathf.Clamp(c1.y, lo, hi);
            c2.y = Mathf.Clamp(c2.y, lo, hi);
        }

        /// <summary>위 두 모서리만 둥근 채우기(세로 막대 차트 — 끝 radius, 바닥 0). 겹쳐 칠하지 않음(투명 색도 고르게).</summary>
        public static void RoundTop(Rect r, Color c, float radius)
        {
            if (!Repaint || c.a <= 0f || r.width <= 0f || r.height <= 0f) return;
            radius = Mathf.Min(radius, r.height, r.width * 0.5f);
            Fill(new Rect(r.x, r.y + radius, r.width, r.height - radius), c);
            BeginClip(new Rect(r.x, r.y, r.width, radius), Vector2.zero);
            Round(new Rect(0f, 0f, r.width, radius * 2f), c, radius);
            EndClip();
        }
    }
}
