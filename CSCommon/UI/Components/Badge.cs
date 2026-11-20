using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Badge — 상태 알약(시안 'CSCommonUX.Badge'): 높이 24 · 좌우 12 · 반지름 = 높이/2 · 글 12 굵게 · 아이콘 13(선 2.4) + 5.
    /// 색은 뜻 → 파스텔 짝(Colors): 청록 · 초록 · 하늘 = 모드 파스텔, 주황 · 노랑 = Look.WarnFill/Ink, 빨강 = Look.FabCancel*, 회색 글 = Look.PillNeutral + Look.BarOff,
    /// 그 밖 뜻 색(팀 색 등)은 그 색 16% 면 + 그 색 글. 수 알약 Count: 높이 20(탭 안 18) · 최소 폭 = 높이 · 좌우 = 높이/2 − 3 · Look.CountBg + Pal.Warn 굵은 글.
    /// </summary>
    public static class Badge
    {
        public const float H = 24f, PadX = 12f, Icon = 13f, IconGap = 5f, Stroke = 2.4f, CountH = 20f, CountAlpha = 0.22f, CountOnAlpha = 0.26f, DotD = 6f;

        /// <summary>뜻 색 → 파스텔 면 · 글.</summary>
        public static void Colors(Color c, out Color fill, out Color ink)
        {
            // 초록(잘됨) = 청록 파스텔(팔레트와 상관없이 — 금색 · 빨강 팔레트에서도 '양호'가 모드 색이 되지 않게)
            if (c == Pal.Green || c == Pal.DoneInk) { fill = Look.OkFill; ink = Look.OkInk; }
            else if (c == Pal.Primary || c == Pal.Teal || c == Pal.Ok) { fill = Look.PastelFill; ink = Look.PastelInk; }
            else if (c == Pal.Warn || c == Pal.Yellow) { fill = Look.WarnFill; ink = Look.WarnInk; }
            else if (c == Pal.Red || c == Pal.MapDanger) { fill = Look.FabCancelFill; ink = Look.FabCancelInk; }
            else if (c == Pal.Sub || c == Pal.Dim || c == Pal.Light || c == Look.OffText) { fill = Look.PillNeutral; ink = Look.BarOff; }
            else { fill = Draw.Alpha(c, 0.16f); ink = c; }
        }

        /// <param name="dot">true = 글 앞 상태 점(지름 DotD, 글색 — 아이콘이 있으면 무시).</param>
        /// <param name="minW">최소 폭(글이 자주 바뀌는 알약이 옆을 밀지 않게). 글 · 아이콘 · 점은 가운데.</param>
        public static float At(float x, float cy, string text, Color color, string icon = null, bool draw = true, float height = H, bool dot = false, float minW = 0f)
        {
            Colors(color, out var fill, out var ink);
            return Paint(x, cy, text, fill, ink, icon, draw, height, dot, minW);
        }

        /// <summary>파스텔 짝을 직접 준 상태 알약(모드가 고른 색 짝 — 예: Look.SeriesYellowFill · SeriesYellowInk). 그린 너비를 돌려줌.</summary>
        public static float Pair(float x, float cy, string text, Color fill, Color ink, bool draw = true) => Paint(x, cy, text, fill, ink, null, draw, H, false, 0f);

        static float Paint(float x, float cy, string text, Color fill, Color ink, string icon, bool draw, float height, bool dot, float minW)
        {
            float tw = Mathf.Ceil(Draw.TextWidth(text, CSCommonDesign.FontMin, true)), iw = icon != null ? Icon + IconGap : dot ? DotD + IconGap : 0f;
            float w = Mathf.Max(minW, PadX * 2f + iw + tw);
            if (draw && Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - height * 0.5f), w, height);
                Draw.Round(r, fill, height * 0.5f);
                float tx = Mathf.Round(r.center.x - (iw + tw) * 0.5f);
                if (icon != null) Draw.Icon(icon, new Rect(tx, Mathf.Round(r.center.y - Icon * 0.5f), Icon, Icon), ink, 24f, Stroke);
                else if (dot) Draw.Dot(new Vector2(tx + DotD * 0.5f, r.center.y), DotD * 0.5f, ink);
                Draw.Text(new Rect(tx + iw, r.y, tw + 2f, r.height), text, CSCommonDesign.FontMin, ink, true, TextAnchor.MiddleLeft);
            }
            return w;
        }

        /// <summary>주어진 칸에 상태 알약(뜻 색 → 파스텔 짝 · 굵은 글 가운데).</summary>
        public static void In(Rect r, string text, Color color)
        {
            if (!Draw.Repaint || string.IsNullOrEmpty(text)) return;
            Colors(color, out var fill, out var ink);
            Draw.Round(r, fill, r.height * 0.5f);
            Draw.TextPill(r, text, CSCommonDesign.FontMin, ink, true, TextAnchor.MiddleCenter);
        }

        /// <summary>수 알약(높이 h · 주황, color 를 주면 그 색 22% 면 + 그 색 글). on = 고른 칸 위(조금 진한 면).</summary>
        public static float Count(float x, float cy, string text, bool draw = true, bool on = false, float h = CountH, Color? color = null)
        {
            float w = Mathf.Max(h, Mathf.Ceil(Draw.TextWidth(text, CSCommonDesign.FontMin, true)) + (h * 0.5f - 3f) * 2f);
            if (draw && Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
                Color ink = color ?? Pal.Warn;
                Draw.Round(r, color.HasValue ? Draw.Alpha(ink, CountAlpha) : on ? Draw.Alpha(Pal.Warn, CountOnAlpha) : Look.CountBg, h * 0.5f);
                Draw.Text(r, text, CSCommonDesign.FontMin, ink, true, TextAnchor.MiddleCenter);
            }
            return w;
        }
    }
}
