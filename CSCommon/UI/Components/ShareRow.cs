using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.ShareRow — 비중 줄(시안): 색 점 8 + 8 · 이름 14 Look.BarInk · 값 15 굵게 · 비율 12 Section.SubInk(폭 40 오른쪽) 사이 8,
    /// 6 아래 막대 8(반지름 4 · Look.PillNeutral 홈 · 채움 color, 없으면 Look.PastelStrong). r 높이 = ShareRowH. share = 0 ~ 1, ratio 를 주면 비율 글 대신 그 글.
    /// </summary>
    public static class ShareRow
    {
        public const float LineH = 20f, BarGap = 6f, Bar = 8f, Dot = 8f, Gap = 8f, RatioW = 40f, NameFont = 14f, ValueFont = 15f;

        public static void In(Rect r, string name, string value, float share, Color? color = null, string ratio = null)
        {
            if (!Draw.Repaint) return;
            share = Mathf.Clamp01(share);
            float top = r.y + CSCommonDesign.ShareRowPad, tx = r.x;
            if (color.HasValue)
            {
                Draw.Dot(new Vector2(tx + Dot * 0.5f, top + LineH * 0.5f), Dot * 0.5f, color.Value);
                tx += Dot + Gap;
            }
            float vw = Mathf.Ceil(Draw.TextWidth(value, ValueFont, true)), vx = r.xMax - RatioW - Gap - vw;
            Draw.TextFit(new Rect(tx, top, Mathf.Max(0f, vx - Gap - tx), LineH), name, NameFont, Look.BarInk);
            Draw.Text(new Rect(vx, top, vw + 2f, LineH), value, ValueFont, Look.BarInk, true, TextAnchor.MiddleLeft);
            Draw.TextFit(new Rect(r.xMax - RatioW, top, RatioW, LineH), ratio ?? Mathf.RoundToInt(share * 100f) + "%", CSCommonDesign.FontMin, Section.SubInk, false, TextAnchor.MiddleRight);
            var bar = new Rect(r.x, top + LineH + BarGap, r.width, Bar);
            Draw.Round(bar, Look.PillNeutral, Bar * 0.5f);
            if (share > 0f) Draw.Round(new Rect(bar.x, bar.y, Mathf.Max(Bar, bar.width * share), Bar), color ?? Look.PastelStrong, Bar * 0.5f);
        }
    }
}
