using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Popover — 떠 있는 카드(시안 'CtlD'): 반지름 20 · 면 Popover.Surface(라이트 #FEFEFE · 다크 Pal.Card) + 1px Look.PillLine · 그림자 없음 ·
    /// 안쪽 위 14 · 좌우 18 · 아래 8 · 아래(또는 위)를 가리키는 꼬리 = 변 12 마름모. 머리 = 이름 14 굵게 + 옆 12 보조 + 아래 줄(Look.FootLine), 줄 = 라벨 12.5 보조 · 값 굵게.
    /// </summary>
    public static class Popover
    {
        public const float Radius = 20f, PadX = 18f, PadTop = 14f, PadBottom = 8f, HeadGap = 8f, RowH = 36f;
        public const float TitleFont = 14f, SubFont = 12f, LabelFont = 12.5f, TitleSubGap = 10f, TailSide = 12f, TailEdge = 1f;

        static readonly Color LightSurface = Draw.Hex("#FEFEFE");

        /// <summary>카드 · 판 면(Dialog 도 같이 씀).</summary>
        public static Color Surface => CSCommonTheme.Light ? LightSurface : Pal.Card;

        /// <summary>카드 바탕 + 선 + 꼬리(tailX 에서 아래 변 — tailBelow = false 면 위 변을 가리킴).</summary>
        public static void Frame(Rect r, float tailX, bool tailBelow = true)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, Surface, Radius);
            Draw.RoundOutline(r, Look.PillLine, Radius);
            // 꼬리: 변 12 마름모의 바깥 절반(대각선 반 = 변 × √2 / 2) — 가로줄 면 + 양 변 선. shortcut: 모서리 반지름 2 는 뾰족한 채로, 둥글게 하려면 Draw.Round 마름모로
            float half = TailSide * 0.7071f, tx = Mathf.Clamp(tailX, r.x + Radius, r.xMax - Radius), dir = tailBelow ? 1f : -1f, edge = tailBelow ? r.yMax : r.y;
            Color fill = Surface;
            // 카드 선을 꼬리 밑변에서 지움(안쪽 1px 포함)
            Draw.Fill(new Rect(tx - half + TailEdge, tailBelow ? edge - TailEdge : edge, (half - TailEdge) * 2f, TailEdge), fill);
            for (int i = 0; i < (int)half; i++)
            {
                float w = half - i - 1f;
                if (w <= 0f) break;
                Draw.Fill(new Rect(tx - w, tailBelow ? edge + i : edge - i - 1f, w * 2f, 1f), fill);
            }
            var tip = new Vector2(tx, edge + dir * half);
            Draw.Line(new Vector2(tx - half, edge), tip, TailEdge, Look.PillLine);
            Draw.Line(tip, new Vector2(tx + half, edge), TailEdge, Look.PillLine);
        }

        /// <summary>머리 줄: r 안 위쪽(아래 HeadGap 8 뺀 칸) 이름 굵게 · 옆 작은 글, r 맨 아래 1px 줄. r 을 카드 안쪽 폭(PadX 안)으로 줄 것. right = 오른쪽에 비워 둘 폭(단추 자리).</summary>
        public static void Head(Rect r, string title, string sub = null, float right = 0f)
        {
            if (!Draw.Repaint) return;
            var row = new Rect(r.x, r.y, r.width - right, r.height - HeadGap);
            float tw = Mathf.Min(Mathf.Ceil(Draw.TextWidth(title, TitleFont, true)), row.width);
            Draw.TextFit(new Rect(row.x, row.y, tw + 2f, row.height), title, TitleFont, Look.BarInk, true);
            if (!string.IsNullOrEmpty(sub) && row.width - tw - TitleSubGap > 0f)
                Draw.TextFit(new Rect(row.x + tw + TitleSubGap, row.y, row.width - tw - TitleSubGap, row.height), sub, SubFont, Section.SubInk);
            Draw.Fill(new Rect(r.x, r.yMax - 1f, r.width, 1f), Look.FootLine);
        }

        /// <summary>줄 라벨(왼쪽 · 12.5 보조 글). 값은 부르는 쪽이 굵은 Look.BarInk 로.</summary>
        public static void Label(Rect r, string text)
        {
            if (Draw.Repaint) Draw.TextFit(r, text, LabelFont, Section.SubInk);
        }
    }

    /// <summary>
    /// CSCommonUX.Dialog — 확인 창(시안 'CtlD'): 420 × 224 고정(설명이 길면 그만큼만 늘어남) · 반지름 14 · 안쪽 20 · 면 Popover.Surface + 1px Look.PillLine · 그림자 없음.
    /// 위에서 아래로 뜻 아이콘 원 44(파스텔 · 지움은 빨강 파스텔) → 10 → 제목 17 굵게 → 10 → 설명 13.5, 단추는 늘 오른쪽 아래 [취소(회색) · 실행(파스텔 / 지움)] 40 알약 사이 8.
    /// 열 때 10 아래에서 0.18초(Modal.Ease · 곡선 cubic-bezier(.2,.8,.2,1)) — 부르는 쪽이 Place 로 자리를 잡고 덮개는 Modal.Scrim.
    /// </summary>
    public static class Dialog
    {
        public const float Width = 420f, MinHeight = 224f, Radius = 14f, Pad = 20f, Gap = 10f, IconD = 44f, IconSize = 20f, TitleFont = 17f, TitleH = 24f, TextFont = 13.5f, BtnGap = 8f;

        static float TextTop => Pad + IconD + Gap + TitleH + Gap;

        /// <summary>판 높이: 224(설명이 두세 줄이면 그대로), 더 길면 설명만큼.</summary>
        public static float Height(string text, float width = Width)
        {
            float th = string.IsNullOrEmpty(text) ? 0f : Draw.TextHeight(text, TextFont, width - Pad * 2f);
            return Mathf.Max(MinHeight, TextTop + th + Gap + CSCommonDesign.ActionHeight + Pad);
        }

        /// <summary>area 가운데 판 자리(폭은 area − 40 안) + 열 때 10 아래에서 떠오름. appear = 나타난 정도 0 ~ 1(Modal.Ease 를 거친 값).</summary>
        public static Rect Place(Rect area, string text, float appear = 1f)
        {
            float w = Mathf.Min(Width, area.width - Pad * 2f), h = Height(text, w);
            return new Rect(Mathf.Round(area.center.x - w * 0.5f), Mathf.Round(area.center.y - h * 0.5f + (1f - appear) * Modal.Rise), w, h);
        }

        /// <summary>판을 그리고 누른 단추를 돌려줌: 1 = 실행 · 0 = 취소 · -1 = 없음. appear = 나타난 정도(투명도).</summary>
        public static int In(Rect box, string title, string text, string ok, string cancel, bool danger, float appear = 1f)
        {
            float opacity = Draw.Opacity;
            Draw.Opacity *= appear;
            float x = box.x + Pad, y = box.y + Pad, inner = box.width - Pad * 2f;
            if (Draw.Repaint)
            {
                Draw.Round(box, Popover.Surface, Radius);
                Draw.RoundOutline(box, Look.PillLine, Radius);
                var ic = new Vector2(x + IconD * 0.5f, y + IconD * 0.5f);
                Draw.Dot(ic, IconD * 0.5f, danger ? Look.FabCancelFill : Look.PastelFill);
                Draw.Icon(danger ? Icons.Warn : Icons.Info, new Rect(ic.x - IconSize * 0.5f, ic.y - IconSize * 0.5f, IconSize, IconSize), danger ? Look.FabCancelInk : Look.PastelInk, 24f, 2f);
                Draw.TextFit(new Rect(x, y + IconD + Gap, inner, TitleH), title, TitleFont, Look.BarInk, true);
                if (!string.IsNullOrEmpty(text))
                    Draw.Text(new Rect(x, box.y + TextTop, inner, Draw.TextHeight(text, TextFont, inner)), text, TextFont, Section.SubInk, false, TextAnchor.UpperLeft, true);
            }
            float by = box.yMax - Pad - CSCommonDesign.ActionHeight;
            float okW = Button.Width(danger ? Icons.Trash : Icons.Check, ok), cancelW = Button.Width(null, cancel);
            var okR = new Rect(box.xMax - Pad - okW, by, okW, CSCommonDesign.ActionHeight);
            var cancelR = new Rect(okR.x - BtnGap - cancelW, by, cancelW, CSCommonDesign.ActionHeight);
            int hit = -1;
            // 두 단추를 늘 불러 컨트롤 순서를 지킴
            Button.In(cancelR, null, cancel, null, Tone.Neutral, null, true, () => hit = 0);
            Button.In(okR, danger ? Icons.Trash : Icons.Check, ok, null, danger ? Tone.Danger : Tone.Primary, null, true, () => hit = 1);
            Draw.Opacity = opacity;
            return hit;
        }
    }
}
