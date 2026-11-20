using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.RunwaySign — 게임 화면 활주로 끝 표지(대중교통 캔버스 Runways · CSCommonUX 3.0 캔버스): 공항 의무 안내 표지처럼 빨강 바탕 · 흰 글 · 검은 테,
    /// 글은 활주로 번호(예 "32L-14R", 이 끝에서 보는 번호가 앞). 막대 없이 발(foot) 바로 위에 판만.
    /// </summary>
    public static class RunwaySign
    {
        /// <summary>판 아래 끝 가운데가 foot(논리 좌표)일 때 판 칸.</summary>
        public static Rect Card(Vector2 foot, string text)
        {
            float w = Mathf.Ceil(Draw.TextWidth(text ?? "", CSCommonDesign.RunwaySignFont, true)) + (CSCommonDesign.RunwaySignPad + CSCommonDesign.RunwaySignEdge) * 2f;
            float h = CSCommonDesign.RunwaySignH;
            return new Rect(Mathf.Round(foot.x - w * 0.5f), Mathf.Round(foot.y - h), w, h);
        }

        public static void In(Rect card, string text)
        {
            if (!Draw.Repaint) return;
            float e = CSCommonDesign.RunwaySignEdge;
            Draw.Round(card, Pal.RunwaySignEdge, CSCommonDesign.RunwaySignRadius);
            var inner = new Rect(card.x + e, card.y + e, card.width - e * 2f, card.height - e * 2f);
            Draw.Round(inner, Pal.RunwaySignRed, Mathf.Max(0f, CSCommonDesign.RunwaySignRadius - e));
            Draw.TextFit(inner, text ?? "", CSCommonDesign.RunwaySignFont, Pal.RunwaySignInk, true, TextAnchor.MiddleCenter);
        }
    }
}
