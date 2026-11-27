using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 칩(FIA 캔버스 R5PartRows · R5Window 상태 칩): 높이 32 · 좌우 12 · r8 · [점 8 → 8 →] 글 14 InkTxt 가운데.
    /// 면은 놓인 면보다 한 단 — 창 머리 · 카드 위 GroundRaised, 창 몸 위 GroundCard(부르는 쪽이 넘김). 지금은 상태 칩(누름 없음)만.
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
        public static float Status(float x, float cy, string label, Color dot, bool live, Color face)
        {
            float w = Width(label, true);
            if (!Draw.Repaint) return w;
            var r = new Rect(x, Mathf.Round(cy - D35.ChipH * 0.5f), w, D35.ChipH);
            Draw.Round(r, face, D35.RadiusCtrl);
            float dx = r.x + D35.ChipPadX, tx = dx + D35.Dot + D35.IconTextGap;
            StatusDot.Paint(new Vector2(dx + D35.Dot * 0.5f, r.center.y), dot, live);
            Draw.Text(new Rect(tx, r.y, r.xMax - D35.ChipPadX - tx, r.height), label, D35.FontBody, Pal.InkTxt, false, TextAnchor.MiddleCenter);
            return w;
        }
    }
}
