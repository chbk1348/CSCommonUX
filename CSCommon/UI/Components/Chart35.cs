using System.Collections.Generic;
using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 차트(FIA 캔버스 R5DashLive '선두와의 간격' 본보기, AGENTS 7절): 가로 눈금선만 · 축 숫자는 눈금선 높이에 세로 가운데 ·
    /// 선은 부드러운 곡선(Catmull-Rom) 굵기 2 · 강조는 계열 하나(시리즈 강조), 나머지는 회색 단계(Gray) · 세로 막대는 끝 4 둥글게 · 바닥 0.
    /// 그리는 순서: Grid → EventBand → 가장 어두운 회색 선부터 → 강조 선 → End → Cursor · 축 글 · Legend. 한 화면에 주인공 차트는 하나.
    /// </summary>
    public static class Chart35
    {
        /// <summary>그림 영역: 칸에서 왼쪽 축 숫자 44 · 위 16 · 아래 x 축 글 28 · 오른쪽 범례 88 을 뺀 곳.</summary>
        public static Rect Plot(Rect r) =>
            new(r.x + D35.ChartAxisW, r.y + D35.ChartTop, Mathf.Max(0f, r.width - D35.ChartAxisW - D35.ChartLegendW), Mathf.Max(0f, r.height - D35.ChartTop - D35.ChartAxisH));

        /// <summary>값 → y: top 값이 영역 위, bottom 값이 영역 바닥(간격 차트는 top = 0 — 선두가 위). 범위 밖은 영역 끝에 붙음.</summary>
        public static float Y(Rect plot, float v, float top, float bottom)
        {
            float span = bottom - top;
            return Mathf.Abs(span) < 1e-6f ? plot.y : plot.y + plot.height * Mathf.Clamp01((v - top) / span);
        }

        /// <summary>값 → x: left 값이 영역 왼쪽, right 값이 오른쪽.</summary>
        public static float X(Rect plot, float v, float left, float right)
        {
            float span = right - left;
            return Mathf.Abs(span) < 1e-6f ? plot.x : plot.x + plot.width * Mathf.Clamp01((v - left) / span);
        }

        /// <summary>강조 아닌 계열 색: 가까운 차례 rank 0 부터 #9aa3af → #848c98 → #6b7380 → #565d68(그 뒤는 마지막 색).</summary>
        public static Color Gray(int rank) => rank <= 0 ? Pal.ChartGray1 : rank == 1 ? Pal.ChartGray2 : rank == 2 ? Pal.ChartGray3 : Pal.ChartGray4;

        /// <summary>
        /// 가로 눈금선 1px(같은 간격, labels.Length 줄) + y 축 숫자 12 InkDim(오른쪽 끝 = 영역 왼쪽 − 8, 세로 가운데 = 그 눈금선).
        /// labels[0] = 맨 위 줄. 0 기준선(zeroTop 이면 맨 위, 아니면 맨 아래)은 ChartBase .12, 나머지는 ChartGrid .06. 단위는 첫 눈금 글에만('0초' · '+4').
        /// </summary>
        public static void Grid(Rect plot, string[] labels, bool zeroTop = true)
        {
            if (!Draw.Repaint || labels == null || labels.Length == 0) return;
            int n = labels.Length, zero = zeroTop ? 0 : n - 1;
            for (int i = 0; i < n; i++)
            {
                float y = TickY(plot, n, i);
                Draw.Fill(new Rect(plot.x, y - 0.5f, plot.width, 1f), i == zero ? Pal.ChartBase : Pal.ChartGrid);
                Draw.Text(new Rect(plot.x - D35.ChartAxisW, y - D35.LineCaption * 0.5f, D35.ChartAxisW - D35.ChartYLabelGap, D35.LineCaption), labels[i], D35.FontCaption, Pal.InkDim, false, TextAnchor.MiddleRight);
            }
        }

        /// <summary>눈금 n 줄 중 i 번째 y(맨 위 0 → 바닥 n − 1).</summary>
        public static float TickY(Rect plot, int n, int i) => n <= 1 ? plot.y : plot.y + plot.height * i / (n - 1);

        /// <summary>x 축 글 12(가운데 = x, 세로 가운데 = 영역 바닥 + 18). now = '지금' 값(InkTxt, 단위 붙여 '23랩'), 아니면 InkDim.</summary>
        public static void XLabel(Rect plot, float x, string text, bool now = false)
        {
            if (!Draw.Repaint) return;
            float w = D35.ChartAxisW * 2f;
            Draw.Text(new Rect(x - w * 0.5f, plot.yMax + D35.ChartXLabelY - D35.LineCaption * 0.5f, w, D35.LineCaption), text, D35.FontCaption, now ? Pal.InkTxt : Pal.InkDim, false, TextAnchor.MiddleCenter);
        }

        /// <summary>'지금' 세로 점선: 1px ChartCursor · 대시 2 · 틈 4 · 영역 위에서 바닥까지.</summary>
        public static void Cursor(Rect plot, float x)
        {
            if (!Draw.Repaint) return;
            Draw.Dashed(new Vector2(x, plot.y), new Vector2(x, plot.yMax), D35.ChartCursorW, Pal.ChartCursor, D35.ChartDash, D35.ChartDashGap);
        }

        /// <summary>사건 구간 띠(SC · VSC): x0 ~ x1 세로 띠 ChartEventBand · 이름 12 InkDim 은 띠 가운데, 영역 위 − 8 에 세로 가운데('세이프티카').</summary>
        public static void EventBand(Rect plot, float x0, float x1, string name)
        {
            if (!Draw.Repaint) return;
            float a = Mathf.Min(x0, x1), b = Mathf.Max(x0, x1);
            Draw.Fill(new Rect(a, plot.y, b - a, plot.height), Pal.ChartEventBand);
            float w = Mathf.Max(b - a, D35.ChartAxisW * 2f);
            Draw.Text(new Rect((a + b - w) * 0.5f, plot.y - D35.ChartBandLabelY - D35.LineCaption * 0.5f, w, D35.LineCaption), name, D35.FontCaption, Pal.InkDim, false, TextAnchor.MiddleCenter);
        }

        /// <summary>계열 선(굵기 2 · 부드러운 곡선 · 채움 없음). pts = 화면 점(X · Y 로 바꾼 것, 부르는 쪽 목록을 다시 씀). 조절점은 영역 안으로 자름.</summary>
        public static void Line(Rect plot, List<Vector2> pts, Color c) => Draw.Smooth(pts, D35.ChartLine, c, plot.y, plot.yMax);

        /// <summary>강조 선 끝 표식: 반지름 4 원 · 채움 강조 · 테두리 2 GroundCard(SVG r4 stroke 2 처럼 바깥 5 · 안 3).</summary>
        public static void End(Vector2 p, Color accent)
        {
            if (!Draw.Repaint) return;
            Draw.Dot(p, D35.ChartEndR + D35.ChartEndRing * 0.5f, Pal.GroundCard);
            Draw.Dot(p, D35.ChartEndR - D35.ChartEndRing * 0.5f, accent);
        }

        /// <summary>
        /// 선 끝 이름표(세로 가운데 = 그 선 끝 y): 팀 색 막대 3 × 12(x = 영역 오른쪽 + 8) + 이름 12(x = + 16).
        /// ink: 강조 계열 InkTxt · 나머지 InkSub · 선 없는 기준(선두) InkDim. 막대 색 = 팀 색(선 색 아님).
        /// </summary>
        public static void Legend(Rect plot, float y, Color team, string name, Color ink)
        {
            if (!Draw.Repaint) return;
            Draw.Round(new Rect(plot.xMax + D35.LegendBarX, y - D35.LegendBarH * 0.5f, D35.LegendBarW, D35.LegendBarH), team, D35.LegendBarR);
            float x = plot.xMax + D35.LegendNameX;
            Draw.Text(new Rect(x, y - D35.LineCaption * 0.5f, D35.ChartLegendW - D35.LegendNameX, D35.LineCaption), name, D35.FontCaption, ink);
        }

        /// <summary>세로 막대 칸: 가로 x · 폭 w, 값 v(0 ~ max)만큼 바닥에서 올라옴(바닥 = 0). 범위 밖은 0 · 영역 높이로 자름.</summary>
        public static Rect Bar(Rect plot, float x, float w, float v, float max)
        {
            float h = max <= 0f ? 0f : plot.height * Mathf.Clamp01(v / max);
            return new Rect(x, plot.yMax - h, w, h);
        }

        /// <summary>세로 막대 그리기: 끝(위) 반지름 4 · 바닥 각짐. 색은 강조 하나 · 나머지 Gray.</summary>
        public static void PaintBar(Rect bar, Color c) => Draw.RoundTop(bar, c, D35.BarEndR);
    }
}
