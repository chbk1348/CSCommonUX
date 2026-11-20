using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Button — 공통 단추(시안 'CSCommonUX.Button'): 알약(반지름 = 높이/2) · 그림자 · 테두리 없음, [아이콘 + 글] 한 덩어리를 칸 정가운데.
    /// 보통 높이 40 · 좌우 20 · 사이 8 · 글 14.5 · 아이콘 18, 작게(small · 높이 36 미만 칸) 32 · 16 · 6 · 12.5 · 15. 아이콘만이면 정사각(40 · 32).
    /// 톤: 주 = 모드 파스텔(굵게) · 보통 = 회색 알약 · 지움 = 빨강 파스텔(굵게) · 글 = 면 없이 파스텔 글(호버 · 누름에 옅은 파스텔 면). 호버 · 누름은 한 단씩 진해지고 누르면 0.95배.
    /// 꺼짐 = 옅은 면(글 톤은 면 없음) + 흐린 글. 글에 맞춘 너비(At) · 주어진 칸(In) · 아이콘만(Square).
    /// </summary>
    public static class Button
    {
        /// <summary>높이 · 좌우 · 아이콘↔글 · 글 · 아이콘(보통 / 작게) · 누름 배율.</summary>
        public const float H = 40f, PadX = 20f, Gap = 8f, Font = 14.5f, Icon = 18f;
        public const float HSmall = 32f, PadXSmall = 16f, GapSmall = 6f, FontSmall = 12.5f, IconSmall = 15f, PressScale = 0.95f;
        /// <summary>줄 · 타일 안 단추 높이(시안 3.0 '.btn.sm'). 36 이상이라 In 은 보통 글 · 아이콘 — 너비는 Width(icon, label) 로.</summary>
        public const float HCompact = 36f;

        /// <summary>글에 맞춘 너비(small = 높이 32). 글이 없으면 정사각.</summary>
        public static float Width(string icon, string label, bool small = false)
        {
            float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, small ? FontSmall : Font, true));
            if (tw <= 0f) return small ? HSmall : H;
            float iw = string.IsNullOrEmpty(icon) ? 0f : (small ? IconSmall + GapSmall : Icon + Gap);
            return (small ? PadXSmall : PadX) * 2f + iw + tw;
        }

        /// <summary>줄 가운데 cy · 왼쪽 x 에 글에 맞춘 단추(draw = false 면 재기만). 그린 너비를 돌려줌.</summary>
        public static float At(float x, float cy, string icon, string label, string tip, Tone tone, Color? iconColor = null, bool enabled = true, Action click = null, bool draw = true, bool small = false, bool loading = false)
        {
            float w = Width(icon, label, small);
            float h = small ? HSmall : H;
            if (draw) In(new Rect(x, Mathf.Round(cy - h * 0.5f), w, h), icon, label, tip, tone, iconColor, enabled, click, loading);
            return w;
        }

        /// <summary>주어진 칸에 단추. 높이 36 미만이면 작은 글 · 아이콘. iconColor = 아이콘 뜻 색(없으면 톤 글색). loading = 글 대신 스피너 · 누름 안 받음.</summary>
        public static void In(Rect r, string icon, string label, string tip, Tone tone, Color? iconColor = null, bool enabled = true, Action click = null, bool loading = false)
        {
            int key = Look.Key(r, icon, label) ^ 0x2f9a6b1d;
            bool live = enabled && !loading;
            float pr = Look.Press(key, r, live);
            if (Draw.Repaint)
            {
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                bool text = tone == Tone.Text;
                Color bg, ink;
                if (!enabled)
                {
                    bg = text ? Color.clear : Look.BtnOff;
                    ink = Look.BtnOffInk;
                }
                else
                {
                    switch (tone)
                    {
                        case Tone.Primary: bg = Color.Lerp(Look.PastelFill, Look.PastelPress, pr); ink = Look.PastelInk; break;
                        case Tone.Danger: bg = Color.Lerp(Look.FabCancelFill, Look.DangerPress, pr); ink = Look.FabCancelInk; break;
                        case Tone.Text: bg = Color.Lerp(Color.clear, Look.TextPress, pr); ink = Look.PastelLink; break;
                        default: bg = Color.Lerp(Look.PillNeutral, Look.NeutralPress, pr); ink = Look.PillNeutralInk; break;
                    }
                }
                if (bg.a > 0f) Draw.Round(r, bg, Mathf.Min(r.width, r.height) * 0.5f);
                bool small = r.height < 36f;
                bool bold = tone == Tone.Primary || tone == Tone.Danger;
                float font = small ? FontSmall : Font, isz = small ? IconSmall : Icon;
                float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, font, bold));
                float iw = string.IsNullOrEmpty(icon) ? 0f : isz + (tw > 0f ? (small ? GapSmall : Gap) : 0f);
                // 칸보다 길면 왼쪽 6 부터(넘친 글은 TextFit 이 줄임)
                float x = Mathf.Max(r.x + 6f, Mathf.Round(r.center.x - (iw + tw) * 0.5f));
                if (loading)
                    Draw.Spinner(r.center, isz * 0.5f - 1f, 2f, ink);
                else
                {
                    if (!string.IsNullOrEmpty(icon))
                        Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - isz * 0.5f), isz, isz), enabled ? iconColor ?? ink : ink, 24f, 2f);
                    // 글만 있는 단추는 칸 정가운데(가로 · 세로)
                    if (tw > 0f && iw <= 0f)
                        Draw.TextPill(new Rect(r.x + 6f, r.y, r.width - 12f, r.height), label, font, ink, bold, TextAnchor.MiddleCenter);
                    else if (tw > 0f)
                        Draw.TextPill(new Rect(x + iw, r.y, Mathf.Max(0f, r.xMax - x - iw - 6f), r.height), label, font, ink, bold, TextAnchor.MiddleLeft);
                }
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            if (live && Draw.Button(r)) click?.Invoke();
        }

        /// <summary>평소 → 호버 → 누름 면(한 단씩 진하게).</summary>

        /// <summary>아이콘만 정사각 단추(− · + · 지우기 · 닫기 · ‹ ›). 누르면 true.</summary>
        public static bool Square(Rect r, string icon, string tip = null, Tone tone = Tone.Neutral, bool enabled = true, Color? iconColor = null)
        {
            bool hit = false;
            In(r, icon, null, tip, tone, iconColor, enabled, () => hit = true);
            return hit;
        }

        /// <summary>한 글자 정사각(예전 '−' '+' '‹' '›' 단추): 아는 글자는 아이콘, 아니면 글.</summary>
        public static bool Glyph(Rect r, string text, bool enabled = true)
        {
            string icon = Icons.ForGlyph(text);
            if (icon != null) return Square(r, icon, null, Tone.Neutral, enabled);
            bool hit = false;
            In(r, null, text, null, Tone.Neutral, null, enabled, () => hit = true);
            return hit;
        }

        /// <summary>예전 테두리 색 → 톤(청록 · 하늘 = 주, 빨강 = 지움, 그 밖 = 보통)과 아이콘 뜻 색(주황 = 되돌림 · 경고).</summary>
        public static Tone ToneOf(Color outline, out Color? iconColor)
        {
            iconColor = null;
            if (outline == Pal.Teal || outline == Pal.Primary || outline == Pal.Ok) return Tone.Primary;
            if (outline == Pal.Red || outline == Pal.MapDanger) return Tone.Danger;
            if (outline == Pal.Warn) iconColor = Pal.Warn;
            return Tone.Neutral;
        }
    }
}
