using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Swatch — 색 칸(시안 'CSCommonUX.Swatch', 원 30): 평소 안쪽 1px Look.SwatchLine, 고르면 이중 고리(판 바탕 Pal.Win 2px + Look.BarInk 2px, 0.16초) ·
    /// 누르면 0.95배. 누르면 true.
    /// </summary>
    public static class Swatch
    {
        public const float Ring = 2f, FadeSeconds = 0.16f, PressScale = 0.95f;

        public static bool In(Rect r, Color color, bool on, string tip = null)
        {
            int key = Look.Key(r, "swatch");
            float onT = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / FadeSeconds);
            float pr = Look.Press(key, r);
            if (Draw.Repaint)
            {
                float rad = Mathf.Min(r.width, r.height) * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                if (onT > 0.01f)
                {
                    Draw.Round(new Rect(r.x - Ring * 2f, r.y - Ring * 2f, r.width + Ring * 4f, r.height + Ring * 4f), Draw.Alpha(Look.BarInk, onT), rad + Ring * 2f);
                    Draw.Round(new Rect(r.x - Ring, r.y - Ring, r.width + Ring * 2f, r.height + Ring * 2f), Draw.Alpha(Pal.Win, onT), rad + Ring);
                }
                Draw.Round(r, color, rad);
                if (onT < 0.99f) Draw.RoundOutline(r, Draw.Alpha(Look.SwatchLine, 1f - onT), rad);
                Draw.EndScale();
            }
            CSCommonUX.CSCommonToolbar.Tip(r, tip);
            return Draw.Button(r);
        }
    }
}
