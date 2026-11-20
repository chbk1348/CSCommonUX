using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// 독 안내 알약(Dockbar 내부 부품 — 이름 미정, 후보 DockHint): 높이 34 · 반지름 17 · 좌우 16 · 글 13, 굵은 머리(문구의 첫 세 칸 띄움 앞, Look.BarInk) + 6 + 설명(Look.BarOff).
    /// 판은 PillBar.Plate(불투명 + 1px 선, 그림자 없음).
    /// </summary>
    internal static class DockHint
    {
        public const float Height = 34f, PadX = 16f, HeadGap = 6f, Font = 13f;

        public static Rect At(float centerX, float y, string text)
        {
            string head = null, rest = text ?? "";
            int cut = rest.IndexOf("   ", StringComparison.Ordinal);
            if (cut > 0) { head = rest.Substring(0, cut); rest = rest.Substring(cut + 3); }
            float hw = head != null ? Mathf.Ceil(Draw.TextWidth(head, Font, true)) + HeadGap : 0f;
            float w = Mathf.Min(Draw.Width - 64f, hw + Mathf.Ceil(Draw.TextWidth(rest, Font)) + PadX * 2f);
            var r = new Rect(PillBar.Left(centerX, w), y, w, Height);
            if (!Draw.Repaint) return r;
            PillBar.Plate(r);
            float tx = r.x + PadX;
            if (head != null) { Draw.Text(new Rect(tx, r.y, hw, Height), head, Font, Look.BarInk, true, TextAnchor.MiddleLeft); tx += hw; }
            Draw.TextFit(new Rect(tx, r.y, r.xMax - tx - PadX, Height), rest, Font, Look.BarOff);
            return r;
        }
    }
}
