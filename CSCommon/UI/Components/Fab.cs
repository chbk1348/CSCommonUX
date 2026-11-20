using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>FAB 색 종류: 판(설정 등 — 고르면 모드 파스텔) · 모드 파스텔(짓기 · 확인) · 되돌리기 파랑 파스텔 · 취소 빨강 파스텔 · 모드 나가기(판 면 + 빨강 아이콘).</summary>
    public enum FabTone
    {
        Plain,
        Primary,
        Undo,
        Cancel,
        Exit,
    }

    /// <summary>CSCommonUX.Fab — C안 둥근 단추(지름 PillBar.FabSize 64): 아이콘 24 가운데 · 1px 선 · 호버 옅은 덮개 · 누르면 0.96배 · 툴팁(CSCommonToolbar.Tip).</summary>
    public static class Fab
    {
        /// <summary>칸 r 에 그리고 눌러 떼면 true. on = 고름(Plain 이면 파스텔로 채움).</summary>
        public static bool In(Rect r, string icon, FabTone tone = FabTone.Plain, bool on = false, string tip = null)
        {
            int key = CSCommonAnim.Key("fab", icon) ^ (int)tone * 0x01000193; // 자리와 무관 · 색 종류별로 나눔
            float hover = Look.Hover(key, r), press = Look.Press(key, r);
            if (Draw.Repaint)
            {
                Color fill = tone switch
                {
                    FabTone.Undo => Look.FabUndoFill,
                    FabTone.Cancel => Look.FabCancelFill,
                    FabTone.Exit => Look.BarSurface,
                    FabTone.Primary => Look.PastelFill,
                    _ => on ? Look.PastelFill : Look.BarSurface,
                };
                Color ink = tone switch
                {
                    FabTone.Undo => Look.FabUndoInk,
                    FabTone.Cancel => Look.FabCancelInk,
                    FabTone.Exit => Look.FabExitInk,
                    FabTone.Primary => Look.PastelInk,
                    _ => on ? Look.PastelInk : Look.BarOff,
                };
                float rad = Mathf.Min(r.width, r.height) * 0.5f, s = PillBar.IconSize;
                Draw.BeginScale(r.center, Look.PressScale(press, PillBar.PressScale));
                Draw.Round(r, fill, rad);
                if (hover > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Haze, PillBar.HoverAlpha * hover), rad);
                Draw.RoundOutline(r, Look.BarLine, rad);
                if (!string.IsNullOrEmpty(icon)) Draw.Icon(icon, new Rect(Mathf.Round(r.center.x - s * 0.5f), Mathf.Round(r.center.y - s * 0.5f), s, s), ink, 24f, PillBar.Stroke);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            return Draw.Button(r);
        }

        /// <summary>줄 배치(독 fabs 콜백 — x 왼쪽 · cy 가운데 · draw = false 면 폭만): 지름을 돌려주고 눌러 떼면 click.</summary>
        public static float At(float x, float cy, string icon, FabTone tone, bool on, string tip, bool draw, Action click)
        {
            float d = PillBar.FabSize;
            if (draw && In(new Rect(x, Mathf.Round(cy - d * 0.5f), d, d), icon, tone, on, tip)) click?.Invoke();
            return d;
        }
    }
}
