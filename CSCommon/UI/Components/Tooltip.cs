using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Tooltip — 짙은 알약 말풍선(시안): Look.TipFill 면 · 반지름 14 · 안쪽 위 10 · 아래 11 · 좌우 14 · 글 12.5 Look.TipInk · 줄바꿈 최대 폭 300,
    /// 가리키는 칸과 10 떨어짐 · 꼬리 높이 5(왼쪽 끝에서 24 안) · 그림자 없음. 0.4초 머물면 뜸(Delay — CSCommonToolbar.DrawTip).
    /// </summary>
    public static class Tooltip
    {
        public const float MaxW = 300f, PadX = 14f, PadTop = 10f, PadBottom = 11f, Radius = 14f, Font = 12.5f, Gap = 10f, Tail = 5f, TailInset = 24f, Delay = 0.4f;

        /// <summary>anchor(가리키는 칸) 아래에(화면 밖이면 위로) 말풍선을 그림.</summary>
        public static void Draw(Rect anchor, string text)
        {
            if (!CSCommonUX.Draw.Repaint || string.IsNullOrEmpty(text)) return;
            float tw = Mathf.Min(MaxW - PadX * 2f, CSCommonUX.Draw.TextWidth(text, Font) + 2f);
            float th = CSCommonUX.Draw.TextHeight(text, Font, tw);
            float w = tw + PadX * 2f, h = th + PadTop + PadBottom;
            float x = Mathf.Clamp(anchor.center.x - w * 0.5f, 8f, CSCommonUX.Draw.Width - w - 8f);
            bool above = anchor.yMax + Gap + h > CSCommonUX.Draw.Height - 8f;
            float y = above ? anchor.y - Gap - h : anchor.yMax + Gap;
            var r = new Rect(x, y, w, h);
            CSCommonUX.Draw.Round(r, Look.TipFill, Mathf.Min(Radius, h * 0.5f));
            float tx = Mathf.Clamp(anchor.center.x, r.x + TailInset, r.xMax - TailInset);
            // 꼬리: 가리키는 쪽을 향한 세모(1px 줄을 쌓아 그림)
            for (int i = 0; i < Tail; i++)
                CSCommonUX.Draw.Fill(new Rect(tx - (i + 1), above ? r.yMax + Tail - 1f - i : r.y - Tail + i, 2f * (i + 1), 1f), Look.TipFill);
            CSCommonUX.Draw.Text(new Rect(r.x + PadX, r.y + PadTop, tw, th), text, Font, Look.TipInk, false, TextAnchor.UpperLeft, true);
        }
    }
}
