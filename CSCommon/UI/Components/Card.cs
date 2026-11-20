using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Card — 선 카드(시안 'CSCommonUX.Card', 떠 있는 판 안에서만): 반지름 20 · Pal.Card 면 + 안쪽 1px Look.PillLine, 그림자 없음.
    /// 제목 16 굵게 Look.BarInk · 오른쪽 보조 12 Section.SubInk. 안 여백은 CSCommonDesign.Card*. 창 띠 안 묶음은 Section.Frame(플레이트 24) — 카드는 떠 있는 판 안 묶음.
    /// </summary>
    public static class Card
    {
        public const float Radius = 20f, TitleFont = 16f, SubFont = 12f;

        public static void Frame(Rect r, Color? fill = null)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, fill ?? Pal.Card, Radius);
            Draw.RoundOutline(r, Look.PillLine, Radius);
        }

        /// <summary>제목 줄: 제목 왼쪽 · 보조 글 오른쪽(r 의 45% 까지).</summary>
        public static void Title(Rect r, string title, string sub = null)
        {
            if (!Draw.Repaint) return;
            float sw = string.IsNullOrEmpty(sub) ? 0f : Mathf.Min(r.width * 0.45f, Draw.TextWidth(sub, SubFont) + 8f);
            Draw.TextFit(new Rect(r.x, r.y, r.width - sw, r.height), title, TitleFont, Look.BarInk, true);
            if (sw > 0f) Draw.TextFit(new Rect(r.xMax - sw, r.y, sw, r.height), sub, SubFont, Section.SubInk, false, TextAnchor.MiddleRight);
        }
    }
}
