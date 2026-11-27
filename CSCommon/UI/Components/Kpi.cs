using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 KPI 칸(FIA 캔버스 R5DashLive .kc): 면 GroundCard + Hairline · 모서리 16 · 위아래 16 · 좌우 20 ·
    /// 라벨 12 InkDim → 4 → 숫자 28 / 36 굵기 600 → 4 → 보조 12 InkDim(비교 대상을 붙임, '5분 전 30%'). 미니 차트 · 아이콘 없음.
    /// 숫자 강조색은 화면에서 하나만.
    /// </summary>
    public static class Kpi
    {
        /// <summary>칸 높이(내용으로 정해짐) = 16 + 16 + 4 + 36 + 4 + 16 + 16.</summary>
        public const float H = D35.KpiPadY * 2f + D35.LineCaption * 2f + D35.KpiGap * 2f + D35.LineNumber;

        /// <summary>한 줄(row)을 cols 열(기본 4) · 사이 16 으로 나눈 i 번째 칸(높이는 row 그대로).</summary>
        public static Rect Col(Rect row, int i, int cols = D35.KpiCols)
        {
            cols = Mathf.Max(1, cols);
            float w = (row.width - D35.KpiColGap * (cols - 1)) / cols;
            return new Rect(row.x + (w + D35.KpiColGap) * i, row.y, w, row.height);
        }

        /// <summary>KPI 칸(숫자 InkTxt). tip = 툴팁 한 줄(없으면 null).</summary>
        public static void Cell(Rect r, string label, string value, string sub = null, string tip = null) => Cell(r, label, value, sub, Pal.InkTxt, tip);

        /// <summary>KPI 칸 — 숫자 색을 줌(강조할 칸 하나만 시리즈 강조색).</summary>
        public static void Cell(Rect r, string label, string value, string sub, Color valueInk, string tip = null)
        {
            if (Draw.Repaint)
            {
                Draw.Box(r, Pal.GroundCard, Pal.Hairline, D35.KpiRadius);
                float x = r.x + D35.KpiPadX, w = r.width - D35.KpiPadX * 2f, y = r.y + D35.KpiPadY;
                Draw.Text(new Rect(x, y, w, D35.LineCaption), label, D35.FontCaption, Pal.InkDim);
                y += D35.LineCaption + D35.KpiGap;
                Draw.Num(new Rect(x, y, w, D35.LineNumber), value, D35.FontNumber, valueInk, TextAnchor.MiddleLeft);
                y += D35.LineNumber + D35.KpiGap;
                Draw.Num(new Rect(x, y, w, D35.LineCaption), sub, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleLeft);
            }
            if (tip != null) CSCommonToolbar.Tip(r, tip);
        }
    }
}
