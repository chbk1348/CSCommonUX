using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Spinner — 구글 머티리얼 서클 인디케이터(Google Circular Progress Indicator):
    /// 호가 16° ↔ 286° 로 늘었다 줄어들며 회전하는 둥근 끝(round cap) 원형 로딩 인디케이터.
    /// </summary>
    public static class Spinner
    {
        public static void At(Vector2 center, float size = 20f, float stroke = 2.2f, Color? color = null)
        {
            if (Draw.Repaint) Draw.Spinner(center, Mathf.Max(2f, (size - stroke) * 0.5f), stroke, color ?? Pal.Primary);
        }

        public static void In(Rect r, Color? color = null, float stroke = 2.2f)
        {
            float size = Mathf.Min(r.width, r.height);
            At(r.center, size, stroke, color);
        }
    }
}
