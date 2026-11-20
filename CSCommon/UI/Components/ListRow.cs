using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.ListRow — 목록 한 줄 바탕(시안 'CSCommonUX.ListRow'): 알약(반지름 = 높이/2, 시안 높이 44 · 좌우 18), 평소 투명 · 호버 Look.NeutralHover · 고르면 모드 파스텔(0.2초).
    /// 안 글은 부르는 쪽이 Ink(14, 고르면 굵게) · Note(오른쪽 메모 12) 색으로. 누르면 true.
    /// </summary>
    public static class ListRow
    {
        public const float H = 44f, PadX = 18f, Font = 14f, NoteFont = 12f, FadeSeconds = 0.2f;

        public static bool In(Rect r, bool on, bool enabled = true, string tip = null)
        {
            int key = Look.Key(r, "listrow");
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 1f / FadeSeconds);
            if (Draw.Repaint)
            {
                float rad = r.height * 0.5f;
                if (onT > 0.01f) Draw.Round(r, Draw.Alpha(Look.PastelFill, onT), rad);
            }
            CSCommonUX.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>줄 글 색(고르면 파스텔 글).</summary>
        public static Color Ink(bool on) => on ? Look.PastelInk : Look.BarInk;

        /// <summary>줄 메모 · 보조 글 색(고르면 파스텔 보조 글).</summary>
        public static Color Note(bool on) => on ? Look.PastelNote : Look.PillSub;
    }
}
