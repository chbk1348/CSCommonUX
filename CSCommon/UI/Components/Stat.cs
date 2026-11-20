using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Stat — 숫자 칸(시안: 이름 12 Look.PillSub · 큰 숫자 24 굵게(줄 30) · 설명 12 Look.PillSub, 들임 없음 · 칸 세로 가운데) ·
    /// 숫자 줄 Line(시안 높이 26: 이름 12.5 Section.SubInk 왼쪽 · 값 굵게 Look.BarInk 오른쪽).
    /// </summary>
    public static class Stat
    {
        public const float LabelH = 16f, ValueH = 30f, ValueFont = 24f, LineFont = 12.5f, LineH = 26f;
        /// <summary>숫자 뒤 단위(시안 '62 / 92' 의 '/ 92'): 글 14 · Look.PillSub · 숫자 → 단위 사이.</summary>
        public const float UnitFont = 14f, UnitGap = 5f;

        /// <summary>칸 높이(이름 · 숫자 · 있으면 설명).</summary>
        public static float Height(bool sub) => LabelH + ValueH + (sub ? LabelH : 0f);

        /// <summary>글에 맞춘 폭(이름 · 숫자 + 단위 · 설명 중 가장 넓은 것) — 숫자 줄(Section.HeroStatGap 사이)에 늘어놓을 때.</summary>
        public static float Width(string label, string value, string sub = null, string unit = null)
        {
            float v = Draw.TextWidth(value ?? "", ValueFont, true) + (string.IsNullOrEmpty(unit) ? 0f : UnitGap + Draw.TextWidth(unit, UnitFont));
            float s = string.IsNullOrEmpty(sub) ? 0f : Draw.TextWidth(sub, CSCommonDesign.FontMin);
            return Mathf.Ceil(Mathf.Max(Draw.TextWidth(label ?? "", CSCommonDesign.FontMin), Mathf.Max(v, s)));
        }

        public static void In(Rect r, string label, string value, string sub = null, Color? color = null, string unit = null)
        {
            if (!Draw.Repaint) return;
            // 세 줄(이름 16 · 숫자 30 · 설명 16)을 칸 세로 가운데로
            bool hasSub = !string.IsNullOrEmpty(sub);
            float y = r.y + Mathf.Max(0f, Mathf.Round((r.height - LabelH - ValueH - (hasSub ? LabelH : 0f)) * 0.5f));
            Draw.TextFit(new Rect(r.x, y, r.width, LabelH), label, CSCommonDesign.FontMin, Look.PillSub);
            if (string.IsNullOrEmpty(unit)) Draw.TextFit(new Rect(r.x, y + LabelH, r.width, ValueH), value, ValueFont, color ?? Look.BarInk, true);
            else
            {
                float vw = Mathf.Min(r.width, Mathf.Ceil(Draw.TextWidth(value, ValueFont, true)));
                Draw.TextFit(new Rect(r.x, y + LabelH, vw + 2f, ValueH), value, ValueFont, color ?? Look.BarInk, true);
                Draw.TextFit(new Rect(r.x + vw + UnitGap, y + LabelH, Mathf.Max(0f, r.width - vw - UnitGap), ValueH), unit, UnitFont, Look.PillSub);
            }
            if (hasSub) Draw.TextFit(new Rect(r.x, y + LabelH + ValueH, r.width, LabelH), sub, CSCommonDesign.FontMin, Look.PillSub);
        }

        public static void Line(Rect r, string label, string value, Color? color = null)
        {
            if (!Draw.Repaint) return;
            Draw.Text(new Rect(r.x, r.y, r.width * 0.5f, r.height), label, LineFont, Section.SubInk, false, TextAnchor.MiddleLeft);
            Draw.TextFit(new Rect(r.x + r.width * 0.4f, r.y, r.width * 0.6f, r.height), value, LineFont, color ?? Look.BarInk, true, TextAnchor.MiddleRight);
        }
    }
}
