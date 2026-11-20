using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Scoreboard — 두 팀 점수판(시안 GameDetailTeam 히어로 '대한민국 2 : 1 멕시코'): 높이 80, 가운데 점수 40 굵게(48) + 아래 상태 12(16) Look.PillSub,
    /// 양쪽 = 이름 16 굵게(우리 소속 Look.PastelInk) · 코드(Section.Code, 점수 쪽), 점수와 코드 사이 24.
    /// </summary>
    public static class Scoreboard
    {
        public const float H = 80f, ScoreFont = 40f, ScoreH = 48f, NoteFont = 12f, NoteH = 16f, NameFont = 16f, Gap = 24f;

        public static void In(Rect r, string codeA, string nameA, string codeB, string nameB, string score, string note, bool oursA, bool oursB)
        {
            if (!Draw.Repaint) return;
            float sw = Mathf.Max(Mathf.Ceil(Draw.TextWidth(score ?? "", ScoreFont, true)), Mathf.Ceil(Draw.TextWidth(note ?? "", NoteFont)));
            float top = Mathf.Round(r.center.y - (ScoreH + NoteH) * 0.5f), cx = r.center.x, cy = r.center.y;
            Draw.Text(new Rect(cx - sw * 0.5f - 1f, top, sw + 2f, ScoreH), score ?? "", ScoreFont, Pal.Txt, true, TextAnchor.MiddleCenter);
            Draw.Text(new Rect(cx - sw * 0.5f - 1f, top + ScoreH, sw + 2f, NoteH), note ?? "", NoteFont, Look.PillSub, false, TextAnchor.MiddleCenter);
            float ax = cx - sw * 0.5f - Gap - Section.CodeW;
            Section.Code(ax, cy, codeA, oursA);
            Draw.TextFit(new Rect(r.x, r.y, Mathf.Max(0f, ax - Section.RowGap - r.x), r.height), nameA, NameFont, oursA ? Look.PastelInk : Pal.Txt, true, TextAnchor.MiddleRight);
            float bx = cx + sw * 0.5f + Gap;
            Section.Code(bx, cy, codeB, oursB);
            float nx = bx + Section.CodeW + Section.RowGap;
            Draw.TextFit(new Rect(nx, r.y, Mathf.Max(0f, r.xMax - nx), r.height), nameB, NameFont, oursB ? Look.PastelInk : Pal.Txt, true);
        }
    }
}
