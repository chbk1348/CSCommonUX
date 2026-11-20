using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Gantt — 시간 띠 표(시안 3.0 올림픽 'Games' 그날 경기): 머리 눈금 줄 28(시각 12 Pal.Dim) · 줄(경기장) 64 · 왼쪽 이름 칸 148 ~ 240(이름 12.5 굵게 18 · 보조 12 16 · 오른쪽 12),
    /// 눈금 선 1(Section.WellFill) · 줄 위 선 1, 지금 선 2(Pal.Red) + 시각 알약 44 × 20(12 굵게). 블록: 위아래 8 · 좌우 2 · 반지름 14 · 안쪽 왼쪽 10 / 오른쪽 8 · 글 12 두 줄(14 · 사이 2, 아래 줄 0.78),
    /// 예정 = Look.PillNeutral · 경기 중 = Section.Plate + 테두리 1.5(Pal.Dim) + 빨간 점 7 · 종료 = Section.WellFill(글 Pal.Dim) · 메달 결정전 = Pal.MedalGold(끝나면 옅은 금), 흐림 0.3.
    /// 범례 줄 20(견본 14 × 10 · 반지름 4 · 견본과 글 6 · 묶음 사이 16).
    /// </summary>
    public static class Gantt
    {
        public const int FirstHour = 9, LastHour = 22;
        public const float AxisH = 28f, RowH = 64f, LabelW = 148f, LabelMaxW = 240f, LabelPadR = 12f, NameFont = 12.5f, NameH = 18f, SubFont = 12f, SubH = 16f;
        public const float NowLine = 2f, NowW = 44f, NowH = 20f, NowFont = 12f, GridLine = 1f;
        public const float BlockInsetY = 8f, BlockInsetX = 2f, BlockRadius = 14f, BlockPadL = 10f, BlockPadR = 8f, BlockFont = 12f, BlockLineH = 14f, BlockLineGap = 2f;
        public const float LiveDot = 7f, LiveDotGap = 5f, LiveRing = 1.5f, DimAlpha = 0.3f, TimeAlpha = 0.78f;
        public const float LegendH = 20f, SwatchW = 14f, SwatchH = 10f, SwatchRadius = 4f, LegendGap = 6f, LegendGroupGap = 16f;

        public enum State { Next, Live, Done }


        public static Color Grid => Section.WellFill;
        /// <summary>메달 결정전 면 · 글(진행 전 · 중 / 끝남) — 히어로 '메달 결정전' 알약도 같은 색.</summary>
        public static Color MedalFill => Pal.MedalGold;
        public static Color MedalInk => Pal.GanttMedalInkLight;
        public static Color MedalDoneFill => Pal.GanttMedalDoneLight;
        public static Color MedalDoneInk => Pal.GanttMedalDoneInkLight;

        public static Color Fill(State st, bool medal) => medal ? (st == State.Done ? MedalDoneFill : MedalFill) : st == State.Done ? Section.WellFill : st == State.Live ? Section.Plate : Look.PillNeutral;

        public static Color Ink(State st, bool medal) => medal ? (st == State.Done ? MedalDoneInk : MedalInk) : st == State.Done ? Pal.Dim : st == State.Live ? Pal.Txt : Pal.Light;

        /// <summary>시각 → x(area = 눈금 칸, FirstHour ~ LastHour).</summary>
        public static float X(Rect area, float hour) => area.x + (Mathf.Clamp(hour, FirstHour, LastHour) - FirstHour) * area.width / (LastHour - FirstHour);

        /// <summary>블록 한 칸(r = 시각 칸 · 줄 높이 전체, 여백은 여기서 뺌): 두 줄 글(제목 · 아래 줄 흐리게), 줄임 없이 맞춤. 누르면 true(흐린 칸은 안 눌림).</summary>
        public static bool Block(Rect r, string title, string sub, State st, bool medal, bool dim, bool bold)
        {
            var b = new Rect(r.x + BlockInsetX, r.y + BlockInsetY, Mathf.Max(0f, r.width - BlockInsetX * 2f), Mathf.Max(0f, r.height - BlockInsetY * 2f));
            if (Draw.Repaint)
            {
                float a = dim ? DimAlpha : 1f;
                Color ink = Draw.Alpha(Ink(st, medal), a);
                Draw.Round(b, Draw.Alpha(Fill(st, medal), a), BlockRadius);
                if (st == State.Live && !medal) Draw.RoundOutline(b, Draw.Alpha(Pal.Dim, a), BlockRadius, LiveRing);
                float x = b.x + BlockPadL, right = b.xMax - BlockPadR, top = Mathf.Round(b.center.y - (BlockLineH * 2f + BlockLineGap) * 0.5f);
                if (st == State.Live)
                {
                    Draw.Dot(new Vector2(x + LiveDot * 0.5f, top + BlockLineH * 0.5f), LiveDot * 0.5f, Draw.Alpha(Pal.Red, a));
                    x += LiveDot + LiveDotGap;
                }
                Draw.TextFit(new Rect(x, top, Mathf.Max(0f, right - x), BlockLineH), title, BlockFont, ink, bold);
                Draw.TextFit(new Rect(b.x + BlockPadL, top + BlockLineH + BlockLineGap, Mathf.Max(0f, right - b.x - BlockPadL), BlockLineH), sub, BlockFont, Draw.Alpha(ink, TimeAlpha));
            }
            return !dim && Draw.Button(b);
        }

        /// <summary>지금 시각 알약(가운데 cx · cy): Pal.Red 면 · 흰 글.</summary>
        public static void Now(float cx, float cy, string text)
        {
            if (!Draw.Repaint) return;
            var p = new Rect(Mathf.Round(cx - NowW * 0.5f), Mathf.Round(cy - NowH * 0.5f), NowW, NowH);
            Draw.Round(p, Pal.Red, NowH * 0.5f);
            Draw.TextPill(p, text, NowFont, Draw.On(Pal.Red), true);
        }

        /// <summary>범례 묶음 하나(견본 면 또는 경기 중 점 + 글 12 Pal.Sub). 쓴 폭을 돌려줌.</summary>
        public static float Swatch(float x, float cy, Color fill, bool dot, string label)
        {
            float lw = Mathf.Ceil(Draw.TextWidth(label, SubFont)), sw = dot ? LiveDot : SwatchW;
            if (Draw.Repaint)
            {
                if (dot) Draw.Dot(new Vector2(x + LiveDot * 0.5f, cy), LiveDot * 0.5f, fill);
                else Draw.Round(new Rect(x, Mathf.Round(cy - SwatchH * 0.5f), SwatchW, SwatchH), fill, SwatchRadius);
                Draw.Text(new Rect(x + sw + LegendGap, Mathf.Round(cy - SubH * 0.5f), lw + 2f, SubH), label, SubFont, Pal.Sub);
            }
            return sw + LegendGap + lw;
        }
    }
}
