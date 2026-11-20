using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Tabs — 묶음 탭(시안 'CSCommonUX.Tabs'): 알약 홈(Look.Pill · 안쪽 4 · 칸 사이 2 · 반지름 = 높이/2) 안에 이름이 늘 보이는 알약 칸 40.
    /// 칸 폭 = 아이콘 있으면 16 + [아이콘 16 · 8 · 이름] + 18, 없으면 20 + 이름 + 20(글 13.5), 내용은 칸 가운데. 고른 칸 = 모드 파스텔 + 굵은 파스텔 글,
    /// 안 고른 칸 = Look.BarOff 글 · 호버 Look.NeutralHover. 색 전환 0.2초 · 누르면 0.95배. 수 알약(Badge.Count 18)은 이름 뒤 8.
    /// Modal.Tabs · SheetTabs 도 같은 칸(Cell)을 씀. 펼쳐지는 칸(아이콘만 → 이름)은 Toolbar · Dockbar 전용(PillBar).
    /// </summary>
    public static class Tabs
    {
        public const float H = 40f, Pad = 4f, Gap = 2f, PadL = 16f, PadR = 18f, PadX = 20f, Icon = 16f, IconGap = 8f, Font = 13.5f, Stroke = 1.9f;
        public const float CountH = 18f, CountGap = 8f, PressScale = 0.95f, FadeSeconds = 0.2f;

        /// <summary>칸 폭(이름은 굵게 재서 고를 때 폭이 안 바뀜). 아이콘이 없으면 좌우 padX.</summary>
        public static float CellWidth(string icon, string label, string badge, float font = Font, float padX = PadX)
        {
            float w = Mathf.Ceil(Draw.TextWidth(label ?? "", font, true));
            if (!string.IsNullOrEmpty(badge)) w += CountGap + Badge.Count(0f, 0f, badge, false, false, CountH);
            return string.IsNullOrEmpty(icon) ? padX * 2f + w : PadL + Icon + IconGap + w + PadR;
        }

        /// <summary>줄 가운데 cy · 왼쪽 x 에 묶음 탭(draw = false 면 재기만). badgeColors[i] = 수 알약 색(없으면 주황). 그린 폭을 돌려줌.</summary>
        public static float At(float x, float cy, string[] icons, string[] labels, int cur, string[] badges, Color[] badgeColors, string[] tips, bool draw, Action<int> pick)
        {
            int n = labels.Length;
            var widths = new float[n];
            float total = Pad * 2f + Gap * Mathf.Max(0, n - 1);
            for (int i = 0; i < n; i++)
            {
                widths[i] = CellWidth(icons != null ? icons[i] : null, labels[i], badges != null ? badges[i] : null);
                total += widths[i];
            }
            if (!draw || n == 0) return total;
            float fh = H + Pad * 2f;
            var frame = new Rect(x, Mathf.Round(cy - fh * 0.5f), total, fh);
            if (Draw.Repaint) Draw.Round(frame, Look.Pill, fh * 0.5f);
            int skey = CSCommonAnim.Key(labels[0], labels[n - 1]) ^ n * 7919 ^ 0x7a3c1e5d;
            float ix = frame.x + Pad;
            for (int i = 0; i < n; i++)
            {
                var r = new Rect(ix, frame.y + Pad, widths[i], H);
                Color? bc = badgeColors != null && badgeColors[i] != Pal.Warn ? badgeColors[i] : null;
                if (Cell(r, skey ^ (i + 1) * 0x2f6b8d3, i == cur, icons != null ? icons[i] : null, labels[i], Font, badges != null ? badges[i] : null, bc, tips != null ? tips[i] : null))
                    pick?.Invoke(i);
                ix += widths[i] + Gap;
            }
            return total;
        }

        /// <summary>탭 칸 하나(그리기 + 누름): [아이콘 16 · 8 · 이름 · 8 · 수 알약] 을 칸 가운데(좁으면 왼쪽 8 부터 · 이름 줄임). 눌러 떼면 true.</summary>
        internal static bool Cell(Rect c, int key, bool on, string icon, string label, float font, string badge = null, Color? badgeColor = null, string tip = null)
        {
            float onT = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / FadeSeconds);
            int hk = key ^ 0x51ed270b;
            float hv = 0f, pr = Look.Press(hk, c);
            if (Draw.Repaint)
            {
                float rad = c.height * 0.5f;
                Draw.BeginScale(c.center, Look.PressScale(pr, PressScale));
                float ha = hv * (1f - onT);
                if (ha > 0.01f) Draw.Round(c, Draw.Alpha(Look.NeutralHover, ha), rad);
                if (onT > 0.01f) Draw.Round(c, Draw.Alpha(Look.PastelFill, onT), rad);
                Color ink = Color.Lerp(Look.BarOff, Look.PastelInk, onT);
                bool hasIcon = !string.IsNullOrEmpty(icon), hasBadge = !string.IsNullOrEmpty(badge);
                float tw = Mathf.Ceil(Draw.TextWidth(label ?? "", font, true));
                float iw = hasIcon ? Icon + IconGap : 0f, bw = hasBadge ? CountGap + Badge.Count(0f, 0f, badge, false, false, CountH) : 0f;
                float x = Mathf.Max(c.x + IconGap, Mathf.Round(c.center.x - (iw + tw + bw) * 0.5f));
                float lw = Mathf.Max(0f, Mathf.Min(tw, c.xMax - IconGap - x - iw - bw));
                if (hasIcon) Draw.Icon(icon, new Rect(x, Mathf.Round(c.center.y - Icon * 0.5f), Icon, Icon), ink, 24f, Stroke);
                Draw.TextPill(new Rect(x + iw, c.y, lw, c.height), label ?? "", font, ink, on, TextAnchor.MiddleCenter);
                if (hasBadge) Badge.Count(x + iw + lw + CountGap, c.center.y, badge, true, false, CountH, badgeColor);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(c, tip);
            return Draw.Button(c);
        }
    }
}
