using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 칩(FIA 캔버스 R5PartRows · R5Window 상태 칩): 높이 32 · 좌우 12 · r8 · [점 8 → 8 →] 글 14 InkTxt 가운데.
    /// 면은 놓인 면보다 한 단 — 창 머리 · 카드 위 GroundRaised, 창 몸 위 GroundCard(부르는 쪽이 넘김). 상태 칩 · 점 없는 칩 · 고르기 칩(3.6).
    /// </summary>
    public static class Chip35
    {
        /// <summary>칩 폭: 좌우 12 + 글 폭(+ 점 8 · 사이 8).</summary>
        public static float Width(string label, bool dot) =>
            D35.ChipPadX * 2f + Mathf.Ceil(Draw.TextWidth(label ?? "", D35.FontBody)) + (dot ? D35.Dot + D35.IconTextGap : 0f);

        /// <summary>
        /// 상태 칩(왼쪽 x · 세로 가운데 cy): 점 = 뜻 색(SemGood · SemWarn · SemBad · 시리즈 강조), live = '세션 진행 중'(점만 1.2초마다 1 ↔ 0.4).
        /// face = 칩 면(놓인 면보다 한 단). 폭을 돌려줌.
        /// </summary>
        public static float Status(float x, float cy, string label, Color dot, bool live, Color face) => Status(x, cy, label, dot, live, face, 0f);

        /// <summary>상태 칩 — 폭 고정(w &gt; 0, 페널티 칩 ChipFixedW 120: 점은 왼쪽 12, 글은 남은 칸 가운데).</summary>
        public static float Status(float x, float cy, string label, Color dot, bool live, Color face, float w)
        {
            if (w <= 0f) w = Width(label, true);
            if (!Draw.Repaint) return w;
            var r = new Rect(x, Mathf.Round(cy - D35.ChipH * 0.5f), w, D35.ChipH);
            Draw.Round(r, face, D35.RadiusCtrl);
            float dx = r.x + D35.ChipPadX, tx = dx + D35.Dot + D35.IconTextGap;
            StatusDot.Paint(new Vector2(dx + D35.Dot * 0.5f, r.center.y), dot, live);
            Draw.Text(new Rect(tx, r.y, r.xMax - D35.ChipPadX - tx, r.height), label, D35.FontBody, Pal.InkTxt, false, TextAnchor.MiddleCenter);
            return w;
        }

        /// <summary>
        /// 점 없는 칩(3.6: 히어로 '비 40%' · '선두 LAP 82' · 시설 매력 종류 · 창 머리 '서킷 고르기 전'): 32 · 좌우 12 · r8 · 글 14 가운데.
        /// face = 놓인 면 한 단 · ink = InkTxt(기본) 또는 InkSub(시설). w &gt; 0 이면 폭 고정. 누름 없음. 폭을 돌려줌.
        /// </summary>
        public static float Plain(float x, float cy, string label, Color face, Color ink, float w = 0f)
        {
            if (w <= 0f) w = Width(label, false);
            if (!Draw.Repaint) return w;
            var r = new Rect(x, Mathf.Round(cy - D35.ChipH * 0.5f), w, D35.ChipH);
            Draw.Round(r, face, D35.RadiusCtrl);
            Draw.Text(r, label, D35.FontBody, ink, false, TextAnchor.MiddleCenter);
            return w;
        }

        /// <summary>
        /// 고르기 칩(필터 — 뉴스 분류): 32 · 좌우 12 · r8 · [점 8 → 8] · 글 14. 고름 = GroundRaised + 위 1px SelectedTopLine · InkTxt · 점 dot,
        /// 안 고름 = 투명 + Hairline · InkSub · 점 InkDim. 면 0.15초. 폭을 돌려주고 안 고른 칩을 눌러 떼면 clicked.
        /// </summary>
        public static float Filter(float x, float cy, string label, bool on, Color? dot, out bool clicked)
        {
            bool hasDot = dot.HasValue;
            var r = new Rect(x, Mathf.Round(cy - D35.ChipH * 0.5f), Width(label, hasDot), D35.ChipH);
            float t = CSCommonAnim.Toward(Look.Key(r, "chipf35", label), on ? 1f : 0f, 1f / D35.DurFeedback);
            if (Draw.Repaint)
            {
                if (t < 0.99f) Draw.RoundOutline(r, Draw.Alpha(Pal.Hairline, 1f - t), D35.RadiusCtrl, 1f);
                Segmented35.Selected(r, D35.RadiusCtrl, t);
                float tx = r.x + D35.ChipPadX;
                if (hasDot)
                {
                    StatusDot.Paint(new Vector2(tx + D35.Dot * 0.5f, r.center.y), on ? dot.Value : Pal.InkDim);
                    tx += D35.Dot + D35.IconTextGap;
                }
                Draw.Text(new Rect(tx, r.y, r.xMax - D35.ChipPadX - tx, r.height), label, D35.FontBody, Color.Lerp(Pal.InkSub, Pal.InkTxt, t), false, TextAnchor.MiddleCenter);
            }
            clicked = Draw.Button(r) && !on;
            return r.width;
        }
    }
}
