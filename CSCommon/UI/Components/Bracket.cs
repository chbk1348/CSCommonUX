using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Bracket — 대진표(시안 GameDetailBracket): 열 머리 28(12 굵게 Pal.Sub) · 경기 칸 = 팀 줄 32 × 2 + 상태 줄 16, 칸 사이 16 · 열 사이 32.
    /// 칸 = Look.Pill 면 · 반지름 12 · 안쪽 10, 팀 줄 = 코드(Section.Code) · 이름 13 · 점수 14(오른쪽), 이긴 팀 굵게 · 우리 소속 Look.PastelInk,
    /// 이 경기 = Look.PastelStrong 테두리 2. 연결선 1.5(Section.PlateLine, 꺾은 선).
    /// </summary>
    public static class Bracket
    {
        public const float HeadH = 28f, TeamH = 32f, StateH = 16f, BoxGap = 16f, ColGap = 32f, Radius = 12f, PadX = 10f, NameFont = 13f, ScoreFont = 14f, StateFont = 12f, Ring = 2f, Link = 1.5f;

        public static float BoxH => TeamH * 2f + StateH;

        /// <summary>경기 칸 하나: win = 이긴 쪽(0 = 위 · 1 = 아래 · -1 = 아직), ours = 우리 소속 줄(0 · 1 · -1).</summary>
        public static void Box(Rect r, string codeA, string nameA, string scoreA, string codeB, string nameB, string scoreB, int win, int ours, string state, bool current)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, Look.Pill, Radius);
            if (current) Draw.RoundOutline(r, Look.PastelStrong, Radius, Ring);
            Team(new Rect(r.x + PadX, r.y, r.width - PadX * 2f, TeamH), codeA, nameA, scoreA, win == 0, ours == 0);
            Team(new Rect(r.x + PadX, r.y + TeamH, r.width - PadX * 2f, TeamH), codeB, nameB, scoreB, win == 1, ours == 1);
            Draw.TextFit(new Rect(r.x + PadX, r.y + TeamH * 2f, r.width - PadX * 2f, StateH), state, StateFont, Pal.Sub);
        }

        static void Team(Rect r, string code, string name, string score, bool won, bool ours)
        {
            float cy = r.center.y, x = r.x + Section.Code(r.x, cy, code, ours) + Section.RowGap * 0.5f;
            float sw = string.IsNullOrEmpty(score) ? 0f : Mathf.Ceil(Draw.TextWidth(score, ScoreFont, true));
            var ink = ours ? Look.PastelInk : Pal.Txt;
            if (sw > 0f) Draw.Text(new Rect(r.xMax - sw - 2f, r.y, sw + 2f, r.height), score, ScoreFont, won ? ink : Pal.Sub, won, TextAnchor.MiddleRight);
            Draw.TextFit(new Rect(x, r.y, Mathf.Max(0f, r.xMax - sw - Section.RowGap * 0.5f - x), r.height), name, NameFont, ink, won || ours);
        }

        /// <summary>앞 칸 오른쪽 가운데 → 뒤 칸 왼쪽 가운데 꺾은 선.</summary>
        public static void Connect(Vector2 from, Vector2 to)
        {
            if (!Draw.Repaint) return;
            float mx = Mathf.Round((from.x + to.x) * 0.5f);
            Draw.Line(from, new Vector2(mx, from.y), Link, Section.PlateLine);
            Draw.Line(new Vector2(mx, from.y), new Vector2(mx, to.y), Link, Section.PlateLine);
            Draw.Line(new Vector2(mx, to.y), to, Link, Section.PlateLine);
        }
    }
}
