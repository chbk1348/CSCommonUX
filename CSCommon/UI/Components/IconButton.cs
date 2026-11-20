using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.IconButton — 아이콘 원 단추(시안 'CSCommonUX.IconButton'): 지름 40 · 아이콘 20 · 평소 면 없이 Look.BarOff, 호버 = 회색 원 면 + 짙은 아이콘,
    /// 켜짐 = 모드 파스텔 원(파스텔 글), 꺼짐 = 흐린 아이콘. 누르면 0.88배.
    /// </summary>
    public static class IconButton
    {
        /// <summary>지름 · 아이콘 · 누름 배율.</summary>
        public const float Size = 40f, IconSize = 20f, PressScale = 0.88f;

        public static float At(float x, float cy, string icon, bool on, string tip, bool draw, Action click, Color? color = null, bool enabled = true)
        {
            if (!draw) return Size;
            var r = new Rect(x, Mathf.Round(cy - Size * 0.5f), Size, Size);
            int key = Look.Key(r, icon, tip) ^ 0x4cf5ad43;
            float hv = 0f, pr = Look.Press(key, r, enabled);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 9f);
            if (Draw.Repaint)
            {
                float rad = Size * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                if (hv * (1f - onT) > 0.01f) Draw.Round(r, Draw.Alpha(Look.PillNeutral, hv * (1f - onT)), rad);
                if (onT > 0.01f) Draw.Round(r, Draw.Alpha(Color.Lerp(Look.PastelFill, Look.PastelHover, hv), onT), rad);
                Color fg = !enabled ? Look.IconOffInk : Color.Lerp(color ?? Color.Lerp(Look.BarOff, Look.BarInk, hv), Look.PastelInk, onT);
                Draw.Icon(icon, new Rect(Mathf.Round(r.center.x - IconSize * 0.5f), Mathf.Round(r.center.y - IconSize * 0.5f), IconSize, IconSize), fg, 24f, 1.9f);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            if (enabled && Draw.Button(r)) click?.Invoke();
            return Size;
        }
    }
}
