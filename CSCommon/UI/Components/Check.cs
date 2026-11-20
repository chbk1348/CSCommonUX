using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Check — 여럿 고르기 칸(시안 '경기장 · 호텔 표' 왼쪽): 20 · 반지름 6, 평소 Section.Plate 면 + 1px Look.PillHoverLine,
    /// 고르면 모드 파스텔 면 + 1px Look.PastelInk + ✓ 14(Look.PastelInk). 누르면 true(누름 칸은 r 그대로 — 부르는 쪽이 줄보다 먼저 부를 것).
    /// </summary>
    public static class Check
    {
        public const float Size = 20f, Radius = 6f, Icon = 14f, Stroke = 3f;

        public static bool In(Rect r, bool on, bool enabled = true)
        {
            if (Draw.Repaint)
            {
                var box = new Rect(Mathf.Round(r.center.x - Size * 0.5f), Mathf.Round(r.center.y - Size * 0.5f), Size, Size);
                Draw.Round(box, on ? Look.PastelFill : Section.Plate, Radius);
                Draw.RoundOutline(box, on ? Look.PastelInk : Look.PillHoverLine, Radius);
                if (on) Draw.Icon(Icons.Done, new Rect(Mathf.Round(box.center.x - Icon * 0.5f), Mathf.Round(box.center.y - Icon * 0.5f), Icon, Icon), Look.PastelInk, 24f, Stroke);
            }
            return enabled && Draw.Button(r);
        }
    }
}
