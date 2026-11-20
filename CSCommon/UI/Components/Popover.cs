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


        /// <summary>카드 · 판 면(Dialog 도 같이 씀).</summary>
        public static Color Surface => CSCommonTheme.Light ? Pal.DialogSurfaceLight : Pal.Card;

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
}
