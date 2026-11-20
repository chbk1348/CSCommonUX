using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Metric — GLDS 2.1 지표 칸: 타일 면 없이 값 15 굵게 위 · 라벨 12 Sub 아래(설명은 그 아래 12 Dim), 왼쪽 정렬 · 들임 없음.
    /// 격자는 Grid: 3열(4개면 2열) · 행 간격 MetricRowGap.
    /// </summary>
    public static class Metric
    {
        public static float Height(bool sub) => CSCommonDesign.MetricValueH + CSCommonDesign.MetricLabelH + (sub ? CSCommonDesign.MetricLabelH : 0f);

        public static void In(Rect r, string value, string label, string sub = null, Color? color = null)
        {
            if (!Draw.Repaint) return;
            float y = r.y;
            Draw.TextFit(new Rect(r.x, y, r.width, CSCommonDesign.MetricValueH), value, CSCommonDesign.FontValue, color ?? Pal.Txt, true);
            y += CSCommonDesign.MetricValueH;
            Draw.TextFit(new Rect(r.x, y, r.width, CSCommonDesign.MetricLabelH), label, CSCommonDesign.FontMin, Pal.Sub);
            if (!string.IsNullOrEmpty(sub))
                Draw.TextFit(new Rect(r.x, y + CSCommonDesign.MetricLabelH, r.width, CSCommonDesign.MetricLabelH), sub, CSCommonDesign.FontMin, Pal.Dim);
        }

        /// <summary>열 수: 4개면 2열, 그 밖 3열(좁으면 2열).</summary>
        public static int Columns(int count, float width) => count == 4 || width < 420f ? 2 : 3;

        public static float GridHeight(int count, bool sub, float width)
        {
            if (count == 0) return 0f;
            int rows = Mathf.CeilToInt(count / (float)Columns(count, width));
            return rows * Height(sub) + (rows - 1) * CSCommonDesign.MetricRowGap;
        }
    }
}
