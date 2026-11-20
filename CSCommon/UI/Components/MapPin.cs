using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.MapPin — 지도 위 건물 머리 위에 뜨는 흰 알약 표식(시안 '[CSCommonUX 3.0] 정치 모드' 정부 건물 표식):
    /// 알약(높이 PinH · 끝 둥금 · Pal.MapTag) 안 왼쪽 원 아이콘(청록 · 주의면 주황) + 이름(굵게) · 상태 줄(점 + 글), 아래 기둥 + 발 점이 건물 자리(foot)를 가리킴.
    /// compact = 원 아이콘만(멀리서 · 많을 때). Card 로 칸을 먼저 재 막는 칸(InputBlocker) · 가림 판정에 쓰고, In 이 그리고 눌림을 돌려줌.
    /// </summary>
    public static class MapPin
    {
        /// <summary>발 점 가운데가 foot(논리 좌표)일 때 알약 칸. 글 폭에 맞춰 늘어남(말줄임 없음).</summary>
        public static Rect Card(Vector2 foot, string name, string state, bool compact)
        {
            float h = CSCommonDesign.PinH, w = h;
            if (!compact)
            {
                float tw = Mathf.Max(Draw.TextWidth(name, CSCommonDesign.PinNameFont, true), CSCommonDesign.PinDot + CSCommonDesign.PinDotGap + Draw.TextWidth(state, CSCommonDesign.PinStateFont));
                w = CSCommonDesign.PinPadL + CSCommonDesign.PinIconD + CSCommonDesign.PinGap + Mathf.Ceil(tw) + CSCommonDesign.PinPadR;
            }
            float bottom = foot.y - CSCommonDesign.PinFoot * 0.5f - CSCommonDesign.PinStemH;
            return new Rect(Mathf.Round(foot.x - w * 0.5f), Mathf.Round(bottom - h), w, h);
        }

        /// <summary>표식 하나(card = Card 값). dot = 상태 점 색 · tip = 마우스를 올렸을 때(아이콘만일 때 이름 · 상태) · image = 아이콘 대신 원 안 그림(로고 등, 흰 원). 눌렀다 떼면 true.</summary>
        public static bool In(Rect card, Vector2 foot, string icon, string name, string state, bool warn, Color dot, bool compact, string tip = null, Texture image = null, Color? fill = null, Color? ink = null)
        {
            if (Draw.Repaint)
            {
                float rad = card.height * 0.5f;
                Draw.Fill(new Rect(Mathf.Round(foot.x - CSCommonDesign.PinStemW * 0.5f), card.yMax, CSCommonDesign.PinStemW, foot.y - card.yMax), Pal.MapPinStem);
                Draw.Dot(foot, CSCommonDesign.PinFoot * 0.5f + CSCommonDesign.PinFootRing, Pal.MapPinFootRing);
                Draw.Dot(foot, CSCommonDesign.PinFoot * 0.5f, Pal.MapTag);
                if (!Draw.Shadow(card, rad, CSCommonDesign.PinShadow)) Draw.Round(new Rect(card.x, card.y + CSCommonDesign.PinShadowY, card.width, card.height), Pal.MapTagShadow, rad);
                Draw.Round(card, Pal.MapTag, rad);
                Draw.RoundOutline(card, Pal.MapPinLine, rad);
                float d = CSCommonDesign.PinIconD, g = CSCommonDesign.PinGlyph;
                var ic = new Rect(card.x + CSCommonDesign.PinPadL, Mathf.Round(card.center.y - d * 0.5f), d, d);
                if (image != null)
                {
                    Draw.Round(ic, Pal.MapTag, d * 0.5f);
                    Draw.RoundOutline(ic, Pal.MapPinLine, d * 0.5f);
                    Draw.Image(new Rect(Mathf.Round(ic.center.x - g * 0.5f), Mathf.Round(ic.center.y - g * 0.5f), g, g), image);
                }
                else
                {
                    Draw.Round(ic, warn ? Pal.MapPinWarnFill : fill ?? Pal.MapPinOkFill, d * 0.5f);
                    Draw.Icon(icon, new Rect(Mathf.Round(ic.center.x - g * 0.5f), Mathf.Round(ic.center.y - g * 0.5f), g, g), warn ? Pal.MapPinWarnInk : ink ?? Pal.MapPinOkInk);
                }
                if (!compact)
                {
                    float x = ic.xMax + CSCommonDesign.PinGap, right = card.xMax - CSCommonDesign.PinPadR + Draw.FitSlack;
                    float top = Mathf.Round(card.center.y - (CSCommonDesign.PinNameH + CSCommonDesign.PinStateH) * 0.5f), sy = top + CSCommonDesign.PinNameH;
                    Draw.Text(new Rect(x, top, right - x, CSCommonDesign.PinNameH), name, CSCommonDesign.PinNameFont, Pal.MapTagInk, true);
                    Draw.Dot(new Vector2(x + CSCommonDesign.PinDot * 0.5f, sy + CSCommonDesign.PinStateH * 0.5f), CSCommonDesign.PinDot * 0.5f, dot);
                    float tx = x + CSCommonDesign.PinDot + CSCommonDesign.PinDotGap;
                    Draw.Text(new Rect(tx, sy, right - tx, CSCommonDesign.PinStateH), state, CSCommonDesign.PinStateFont, Pal.MapTagSub);
                }
                if (tip != null && Draw.Hover(card)) Tooltip.Draw(card, tip);
            }
            return Draw.Button(card);
        }
    }
}
