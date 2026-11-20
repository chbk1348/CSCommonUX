using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Switch — 켜고 끄기(시안 'CSCommonUX.Switch', 48 × 28 알약 = CSCommonDesign.SwitchW · SwitchH): 꺼짐 = Look.PillNeutral 홈 + Look.KnobOff 손잡이,
    /// 켜짐 = 모드 파스텔 홈 + Look.PastelInk 손잡이, 쓸 수 없음 = Look.BtnOff 홈 + Look.IconOffInk 손잡이. 손잡이 22(안쪽 3)는 0.24초 cubic-bezier(.2,.9,.3,1.2)로
    /// 살짝 넘쳤다 자리에 앉고 색은 0.22초, 누르면 0.95배. 누르면 true.
    /// </summary>
    public static class Switch
    {
        public const float Inset = 3f, MoveSeconds = 0.24f, FadeSeconds = 0.22f, PressScale = 0.95f;
        /// <summary>손잡이 곡선 cubic-bezier(.2, .9, .3, 1.2).</summary>
        const float X1 = 0.2f, Y1 = 0.9f, X2 = 0.3f, Y2 = 1.2f;

        public static bool In(Rect r, bool on, bool enabled = true)
        {
            int key = Look.Key(r, "switch");
            float c = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / FadeSeconds);
            float mv = CSCommonAnim.Toward(key ^ 0x3c6ef372, on ? 1f : 0f, 1f / MoveSeconds);
            float pr = Look.Press(key ^ 0x1b873593, r, enabled);
            if (Draw.Repaint)
            {
                float k = r.height - Inset * 2f;
                // 켤 때 곡선 그대로, 끌 때 거꾸로(왼쪽에서도 살짝 넘침)
                float t = on ? Modal.Ease(mv, X1, Y1, X2, Y2) : 1f - Modal.Ease(1f - mv, X1, Y1, X2, Y2);
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                Draw.Round(r, !enabled ? Look.BtnOff : Color.Lerp(Look.PillNeutral, Look.PastelFill, c), r.height * 0.5f);
                // 켜짐 테두리 = 손잡이 색(PastelInk): 같은 PastelFill 알약 · 판 위에서도 바탕 모양이 보이게
                if (enabled && c > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Look.PastelInk, c), r.height * 0.5f, CSCommonDesign.SwitchRing);
                float cx = Mathf.LerpUnclamped(r.x + Inset + k * 0.5f, r.xMax - Inset - k * 0.5f, t);
                Draw.Dot(new Vector2(cx, r.center.y), k * 0.5f, !enabled ? Look.IconOffInk : Color.Lerp(Look.KnobOff, Look.PastelInk, c));
                Draw.EndScale();
            }
            return enabled && Draw.Button(r);
        }
    }
}
