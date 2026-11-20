using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>CSCommonUX.Well — 안쪽 우물판(시안: 지도 · 미니맵 · 그림이 올라감): 반지름 24(작은 칸은 짧은 변/2) · Section.Plate 면 + 안쪽 1px Look.PillLine(line 을 주면 그 색).</summary>
    public static class Well
    {
        public const float Radius = 24f;

        public static void Frame(Rect r, Color? line = null)
        {
            if (!Draw.Repaint) return;
            float rad = Mathf.Min(Radius, Mathf.Min(r.width, r.height) * 0.5f);
            Draw.Round(r, Section.Plate, rad);
            Draw.RoundOutline(r, line ?? Look.PillLine, rad);
        }
    }
}
