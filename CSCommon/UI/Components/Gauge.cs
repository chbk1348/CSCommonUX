using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Gauge — 반원 게이지(시안 준비 중 '예상 준비도' 440 × 250) · 고리(시안 개최 중 '운영 점수' 176).
    /// 홈 Look.PastelFill · 채움 Look.PastelStrong · 둥근 끝, 목표 선 3 · Look.BarInk + 이름 12 굵게. 가운데 값 굵게 · 아래 이름 13 SubInk.
    /// </summary>
    public static class Gauge
    {
        /// <summary>반원: 굵기 · 위(목표 이름 자리) · 아래(0 · 100 자리) · 옆 · 값 글 · 값 줄 · 이름 글 · 줄 · 목표 선 · 선 안쪽 · 바깥 · 목표 이름 거리 · 옆으로.</summary>
        public const float HalfStroke = 26f, HalfTop = 38f, HalfBottom = 44f, HalfSide = 52f, HalfValueFont = 60f, HalfValueH = 64f, LabelFont = 13f, LabelH = 18f;
        public const float TickW = 3f, TickIn = 22f, TickOut = 20f, TickLabel = 36f, TickLabelShift = 14f, EndLabelY = 30f;
        /// <summary>고리: 굵기 · 값 글 · 줄.</summary>
        public const float RingStroke = 16f, RingValueFont = 40f, RingValueH = 44f;

        /// <summary>반원 높이(폭 w): 시안 비율(반지름 = 폭/2 − 옆).</summary>
        public static float HalfHeight(float w) => Mathf.Max(0f, w * 0.5f - HalfSide) + HalfTop + HalfBottom;

        static void Band(Vector2 c, float radius, float stroke, float from, float to, Color color)
        {
            if (to - from <= 0.01f) return;
            Draw.Arc(c, radius, stroke, from, to, color);
            // 둥근 끝
            float a0 = from * Mathf.Deg2Rad, a1 = to * Mathf.Deg2Rad;
            Draw.Dot(c + new Vector2(Mathf.Cos(a0), Mathf.Sin(a0)) * radius, stroke * 0.5f, color);
            Draw.Dot(c + new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * radius, stroke * 0.5f, color);
        }

        static Vector2 At(Vector2 c, float deg, float radius) => c + new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad)) * radius;

        /// <summary>반원(왼쪽 0 → 위 → 오른쪽 1): t 채움 · goal 목표 선(goalLabel 은 선 바깥) · 가운데 value / label · 양끝 아래 min · max.</summary>
        public static void Half(Rect r, float t, float goal, string value, string label, string goalLabel, string min, string max)
        {
            if (!Draw.Repaint) return;
            float radius = Mathf.Max(HalfStroke, Mathf.Min(r.width * 0.5f - HalfSide, r.height - HalfTop - HalfBottom));
            var c = new Vector2(r.center.x, r.y + HalfTop + radius);
            Band(c, radius, HalfStroke, 180f, 360f, Look.PastelFill);
            Band(c, radius, HalfStroke, 180f, 180f + 180f * Mathf.Clamp01(t), Look.PastelStrong);
            if (goal >= 0f)
            {
                float deg = 180f + 180f * Mathf.Clamp01(goal);
                Draw.RoundLine(At(c, deg, radius - TickIn), At(c, deg, radius + TickOut), TickW, Look.BarInk);
                if (!string.IsNullOrEmpty(goalLabel))
                {
                    var p = At(c, deg, radius + TickLabel);
                    float w = Mathf.Ceil(Draw.TextWidth(goalLabel, CSCommonDesign.FontMin, true));
                    Draw.Text(new Rect(p.x + TickLabelShift - w * 0.5f - 1f, p.y - LabelH * 0.5f, w + 2f, LabelH), goalLabel, CSCommonDesign.FontMin, Look.BarInk, true, TextAnchor.MiddleCenter);
                }
            }
            Draw.TextFit(new Rect(c.x - radius + HalfStroke, c.y - LabelH - HalfValueH, (radius - HalfStroke) * 2f, HalfValueH), value, HalfValueFont, Look.BarInk, true, TextAnchor.MiddleCenter);
            Draw.TextFit(new Rect(c.x - radius + HalfStroke, c.y - LabelH, (radius - HalfStroke) * 2f, LabelH), label, LabelFont, Section.SubInk, false, TextAnchor.MiddleCenter);
            if (!string.IsNullOrEmpty(min)) Draw.Text(new Rect(c.x - radius - HalfSide, c.y + EndLabelY - LabelH * 0.5f, HalfSide * 2f, LabelH), min, CSCommonDesign.FontMin, Look.PillSub, false, TextAnchor.MiddleCenter);
            if (!string.IsNullOrEmpty(max)) Draw.Text(new Rect(c.x + radius - HalfSide, c.y + EndLabelY - LabelH * 0.5f, HalfSide * 2f, LabelH), max, CSCommonDesign.FontMin, Look.PillSub, false, TextAnchor.MiddleCenter);
        }

        /// <summary>고리(위에서 시계 방향): r 안 가장 큰 원 · t 채움 · 가운데 value 40 굵게 / label 12.</summary>
        public static void Ring(Rect r, float t, string value, string label)
        {
            if (!Draw.Repaint) return;
            float radius = Mathf.Min(r.width, r.height) * 0.5f - RingStroke * 0.5f;
            var c = r.center;
            Draw.Arc(c, radius, RingStroke, 0f, 360f, Look.PastelFill);
            Band(c, radius, RingStroke, -90f, -90f + 360f * Mathf.Clamp01(t), Look.PastelStrong);
            float inner = (radius - RingStroke) * 2f, top = Mathf.Round(c.y - (RingValueH + CSCommonDesign.LineSub) * 0.5f);
            Draw.TextFit(new Rect(c.x - inner * 0.5f, top, inner, RingValueH), value, RingValueFont, Look.BarInk, true, TextAnchor.MiddleCenter);
            Draw.TextFit(new Rect(c.x - inner * 0.5f, top + RingValueH, inner, CSCommonDesign.LineSub), label, CSCommonDesign.FontMin, Section.SubInk, false, TextAnchor.MiddleCenter);
        }
    }
}
