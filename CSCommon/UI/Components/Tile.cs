using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Tile — 고를 수 있는 타일(시안 'CSCommonUX.Tile' · 모달 4열 격자 타일도 이것): 반지름 20(낮은 타일은 높이/2) · 평소 Look.Pill · 호버 Look.NeutralHover ·
    /// 고르면 모드 파스텔(0.2초) · 잠김 Look.BtnOff, 누르면 0.95배. 안 글은 부르는 쪽이 Ink(이름 14 굵게) · Sub(보조 12) 색으로 — 시안 왼쪽 18 · 이름 → 보조 3.
    /// </summary>
    public static class Tile
    {
        public const float Radius = 20f, PadX = 18f, NameFont = 14f, SubFont = 12f, LineGap = 3f, PressScale = 0.95f, FadeSeconds = 0.2f;

        /// <summary>바탕만 그림(안에 단추가 있는 타일 — 타일 자체는 누르지 않음).</summary>
        public static void Frame(Rect r, bool on, bool enabled = true, bool hoverable = true)
        {
            int key = Look.Key(r, "tile");
            Back(r, key, on, enabled);
        }

        static void Back(Rect r, int key, bool on, bool enabled, bool danger = false)
        {
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 1f / FadeSeconds);
            if (!Draw.Repaint) return;
            Color bg = !enabled ? Look.BtnOff : Color.Lerp(Look.Pill, danger ? Look.FabCancelFill : Look.PastelFill, onT);
            Draw.Round(r, bg, Mathf.Min(Radius, r.height * 0.5f));
        }

        /// <summary>바탕 + 누르기. content = 타일 안 그림(눌림 배율 안에서 부름). danger = 고르면 빨강 파스텔(지우기 분류). 누르면 true(잠김이면 늘 false).</summary>
        public static bool In(Rect r, bool on, bool enabled = true, string tip = null, Action content = null, bool danger = false)
        {
            int key = Look.Key(r, "tile");
            float pr = Look.Press(key, r, enabled);
            bool paint = Draw.Repaint;
            if (paint) Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
            Back(r, key, on, enabled, danger);
            content?.Invoke();
            if (paint) Draw.EndScale();
            CSCommonUX.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>타일 이름 글 색(고르면 파스텔 글 · 잠김 흐린 글).</summary>
        public static Color Ink(bool on, bool enabled = true, bool danger = false) => !enabled ? Look.BtnOffInk : on ? danger ? Look.FabCancelInk : Look.PastelInk : Look.BarInk;

        /// <summary>타일 보조 글 색(고르면 파스텔 보조 글 · 잠김 더 흐린 글).</summary>
        public static Color Sub(bool on, bool enabled = true, bool danger = false) => !enabled ? Look.IconOffInk : on ? danger ? Look.FabCancelInk : Look.PastelNote : Look.PillSub;
    }
}
