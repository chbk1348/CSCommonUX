using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Stepper — − 값 +(시안 'CSCommonUX.Stepper'): 알약 홈(Look.Pill · 안쪽 4 · 사이 4 · 반지름 = 높이/2) 안에 원 단추 둘(Look.PillNeutral 면 · Look.PillNeutralInk − + 18)
    /// · 가운데 값(14 굵게 Look.BarInk). 끝이면 그쪽 단추 꺼짐(동작 null — 면 없이 Look.IconOffInk). 누르면 0.95배. 시안 값 칸 최소 폭 76.
    /// </summary>
    public static class Stepper
    {
        public const float Pad = 4f, Gap = 4f, Icon = 18f, Stroke = 2f, Font = 14f, ValueMinW = 76f, PressScale = 0.95f;

        /// <param name="enabled">false = 잠금(두 단추 꺼짐 · 값 흐린 글, 예: 다른 설정이 값을 정하는 동안).</param>
        /// <param name="canMinus">false = 최소 끝(− 꺼짐). minus 가 null 이어도 꺼짐(예전 방식).</param>
        /// <param name="canPlus">false = 최대 끝(+ 꺼짐). plus 가 null 이어도 꺼짐.</param>
        public static void In(Rect r, string value, Action minus, Action plus, string minusTip = null, string plusTip = null, bool enabled = true, bool canMinus = true, bool canPlus = true)
        {
            if (Draw.Repaint) Draw.Round(r, Look.Pill, r.height * 0.5f);
            float s = r.height - Pad * 2f;
            if (Round(new Rect(r.x + Pad, r.y + Pad, s, s), Icons.Minus, minusTip, enabled && canMinus && minus != null)) minus();
            if (Draw.Repaint) Draw.TextPill(new Rect(r.x + Pad + s + Gap, r.y, Mathf.Max(0f, r.width - (Pad + s + Gap) * 2f), r.height), value, Font, enabled ? Look.BarInk : Look.OffText, true, TextAnchor.MiddleCenter);
            if (Round(new Rect(r.xMax - Pad - s, r.y + Pad, s, s), Icons.Plus, plusTip, enabled && canPlus && plus != null)) plus();
        }

        /// <summary>원 단추(켜짐 = 회색 원 · 꺼짐 = 면 없이 흐린 아이콘). 누르면 true.</summary>
        static bool Round(Rect b, string icon, string tip, bool enabled)
        {
            int key = Look.Key(b, "stepper", icon);
            float pr = Look.Press(key, b, enabled);
            if (Draw.Repaint)
            {
                Draw.BeginScale(b.center, Look.PressScale(pr, PressScale));
                if (enabled) Draw.Round(b, Look.PillRaised, b.height * 0.5f);
                Draw.Icon(icon, new Rect(Mathf.Round(b.center.x - Icon * 0.5f), Mathf.Round(b.center.y - Icon * 0.5f), Icon, Icon), enabled ? Look.PillNeutralInk : Look.IconOffInk, 24f, Stroke);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(b, tip);
            return enabled && Draw.Button(b);
        }
    }
}
