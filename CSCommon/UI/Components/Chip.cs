using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Chip — 필터 알약(시안 'CSCommonUX.Chip'): 높이 32 · 좌우 14 · 글 13 · 반지름 = 높이/2. 평소 Look.Pill + 안쪽 1px Look.PillLine(호버 Look.NeutralHover),
    /// 고르면 선 없이 모드 파스텔 + 굵은 파스텔 글(0.2초). color = 팀 색이면 고른 칩을 그 색의 옅은 면 · 짙은 글로(팀 칩은 호버 없음).
    /// ▾(dropdown — 목록을 여는 칩) · ✕(고른 칩에 clear 를 주면) 14 · 글 뒤 4, 누르면 0.95배. 폭은 굵은 글로 재서 고를 때 안 바뀜.
    /// </summary>
    public static class Chip
    {
        /// <summary>팀 색 → 고른 칩 면(흰색에 섞는 비율 라이트 · 다크) · 글(어둡게 하는 배율 라이트 · 다크) · 색 전환 초.</summary>
        const float TeamFillLight = 0.13f, TeamFillDark = 0.22f, TeamInkLight = 0.6f, TeamInkDark = 0.4f, FadeSeconds = 0.2f, Stroke = 2.4f;

        public static float Width(string label, bool on, bool icon)
            => CSCommonDesign.ChipPadX * 2f + Mathf.Ceil(Draw.TextWidth(label, CSCommonDesign.ChipFont, true)) + (icon ? CSCommonDesign.ChipIconGap + CSCommonDesign.ChipIcon : 0f);

        /// <summary>줄 가운데 cy · 왼쪽 x 에 칩(draw = false 면 재기만). 그린 너비를 돌려줌.</summary>
        public static float At(float x, float cy, string label, bool on, Action click, bool draw = true, string tip = null, Color? color = null, bool dropdown = false, Action clear = null)
        {
            bool canClear = on && clear != null;
            float w = Width(label, on, canClear || dropdown), h = CSCommonDesign.ChipHeight;
            if (!draw) return w;
            var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
            int key = Look.Key(r, "chip", label);
            float hv = Look.Hover(key, r, !on && color == null), pr = Look.Press(key, r);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 1f / FadeSeconds);
            if (Draw.Repaint)
            {
                float rad = h * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, CSCommonDesign.ChipPressScale));
                Color fill = Look.PastelFill, ink = Look.PastelInk;
                if (color.HasValue)
                {
                    bool light = CSCommonTheme.Light;
                    fill = Color.Lerp(Color.white, color.Value, light ? TeamFillLight : TeamFillDark);
                    ink = Draw.Shade(color.Value, light ? TeamInkLight : TeamInkDark);
                    fill.a = ink.a = 1f;
                }
                Draw.Round(r, Color.Lerp(Color.Lerp(Look.Pill, Look.NeutralHover, hv), fill, onT), rad);
                if (onT < 0.99f) Draw.RoundOutline(r, Draw.Alpha(Look.PillLine, 1f - onT), rad);
                Color fg = Color.Lerp(Look.BarInk, ink, onT);
                float tx = r.x + CSCommonDesign.ChipPadX, tw = Mathf.Ceil(Draw.TextWidth(label, CSCommonDesign.ChipFont, true));
                Draw.Text(new Rect(tx, r.y, tw, h), label, CSCommonDesign.ChipFont, fg, on, TextAnchor.MiddleCenter);
                if (canClear || dropdown)
                {
                    float s = CSCommonDesign.ChipIcon;
                    Draw.Icon(canClear ? Icons.Close : Icons.Chevron, new Rect(tx + tw + CSCommonDesign.ChipIconGap, Mathf.Round(r.center.y - s * 0.5f), s, s), fg, 24f, Stroke);
                }
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            // ✕ 자리를 먼저(칩 오른쪽 끝 · 아이콘보다 넓게)
            if (canClear && Draw.Button(new Rect(r.xMax - CSCommonDesign.ChipPadX - CSCommonDesign.ChipIcon - 4f, r.y, CSCommonDesign.ChipPadX + CSCommonDesign.ChipIcon + 4f, h)))
            {
                clear();
                return w;
            }
            if (Draw.Button(r)) click?.Invoke();
            return w;
        }

        /// <summary>보기 칩 폭(굵은 글로 재서 켜고 끌 때 안 바뀜).</summary>
        public static float ViewWidth(string label) => CSCommonDesign.ViewChipPadX * 2f + Mathf.Ceil(Draw.TextWidth(label, CSCommonDesign.ViewChipFont, true));

        /// <summary>
        /// 보기 칩(시안 3.0 '.vc' — 판 머리의 보기 켜기 · 끄기): 높이 28 · 좌우 12 · 글 12. 켜짐 = 글색 면(다크 흰 면 · 라이트 짙은 면) + 바탕색 굵은 글,
        /// 꺼짐 = 면 없이 1px Pal.Line + 글색. 누르면 0.95배. 그린 너비를 돌려줌.
        /// </summary>
        public static float View(float x, float cy, string label, bool on, Action click, bool draw = true, string tip = null)
        {
            float w = ViewWidth(label), h = CSCommonDesign.ViewChipH;
            if (!draw) return w;
            var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
            int key = Look.Key(r, "viewchip", label);
            float pr = Look.Press(key, r);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 1f / FadeSeconds);
            if (Draw.Repaint)
            {
                float rad = h * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, CSCommonDesign.ChipPressScale));
                if (onT > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Txt, onT), rad);
                if (onT < 0.99f) Draw.RoundOutline(r, Draw.Alpha(Pal.Line, 1f - onT), rad);
                Draw.Text(r, label, CSCommonDesign.ViewChipFont, Color.Lerp(Pal.Txt, Pal.Win, onT), on, TextAnchor.MiddleCenter);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            if (Draw.Button(r)) click?.Invoke();
            return w;
        }
    }
}
