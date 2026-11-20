using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Progress — 막대 8(시안): 반지름 4 · Look.PillNeutral 홈, 목표 선(2 × 16 · Look.BarInk), 넘으면 Look.PastelStrong · 못 미치면 Look.ProgressLow(color 를 주면 그 색).
    /// 채움 폭은 값이 바뀌면 부드럽게 따라감(시안 0.4초 cubic-bezier(.2,.8,.2,1) 어림 — CSCommonAnim.Smooth).
    /// </summary>
    public static class Progress
    {
        public const float Bar = 8f, GoalW = 2f, GoalH = 16f, Rate = 8f;

        public static void In(Rect r, float t, float goal = -1f, Color? color = null)
        {
            t = Mathf.Clamp01(t);
            var bar = new Rect(r.x, Mathf.Round(r.center.y - Bar * 0.5f), r.width, Bar);
            float w = CSCommonAnim.Smooth(Look.Key(r, "progress"), bar.width * t, Rate);
            if (!Draw.Repaint) return;
            Draw.Round(bar, Look.PillNeutral, Bar * 0.5f);
            Color c = color ?? (goal < 0f || t >= goal ? Look.PastelStrong : Look.ProgressLow);
            if (w > 0.5f) Draw.Round(new Rect(bar.x, bar.y, Mathf.Max(Bar, w), Bar), c, Bar * 0.5f);
            if (goal >= 0f)
                Draw.Round(new Rect(Mathf.Round(bar.x + bar.width * Mathf.Clamp01(goal) - GoalW * 0.5f), Mathf.Round(bar.center.y - GoalH * 0.5f), GoalW, GoalH), Look.BarInk, GoalW * 0.5f);
        }
    }
}
