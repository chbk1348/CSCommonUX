using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Slider — 끌기 막대(시안 'CSCommonUX.Slider'): 막대 4(반지름 2 · 빈 쪽 Look.PillLine · 채운 쪽 fill — 보통 Look.PastelStrong) · 손잡이 16 Look.Knob 원 + fill 2px 테두리(그림자 없음),
    /// 호버 18 + fill 18% 고리 6, 끄는 중 고리 8(28%) + 값 말풍선(bubble). 12단계 이하는 2 × 2 눈금(채운 쪽 Look.Knob · 빈 쪽 Look.KnobOff), 꺼짐 40%.
    /// 손잡이 크기는 0.1초로 바뀜. 호버 중 ← → 키로 한 단계. 손을 떼면 released.
    /// </summary>
    public static class Slider
    {
        const float TrackH = 4f, Knob = 8f, KnobHover = 9f, Border = 2f, RingHover = 6f, RingDrag = 8f, Tick = 2f, MaxTicks = 12f;
        const float OffAlpha = 0.4f, HoverAlpha = 0.18f, DragAlpha = 0.28f, GrowSeconds = 0.1f;

        static bool sliding;
        static int slideKey;

        /// <summary>끄는 중(지도 도구가 클릭을 가져가지 않게).</summary>
        public static bool Sliding => sliding && Input.GetMouseButton(0);

        public static float In(Rect r, float value, float min, float max, float step, Color fill, out bool released) =>
            In(r, value, min, max, step, fill, out released, null);

        /// <summary>bubble = 끄는 중 손잡이 위 말풍선 글(null 이면 없음), enabled = false 면 흐리게 · 입력 없음.</summary>
        public static float In(Rect r, float value, float min, float max, float step, Color fill, out bool released, string bubble, bool enabled = true)
        {
            released = false;
            int key;
            unchecked { key = Mathf.RoundToInt(r.x) * 397 ^ Mathf.RoundToInt(r.y); }
            float Snap(float v)
            {
                v = Mathf.Clamp(v, Mathf.Min(min, max), Mathf.Max(min, max));
                return step > 0f ? Mathf.Clamp(min + Mathf.Round((v - min) / step) * step, Mathf.Min(min, max), Mathf.Max(min, max)) : v;
            }
            float At() => Snap(Mathf.Lerp(min, max, Mathf.Clamp01((Draw.Mouse.x - r.x) / r.width)));
            Event e = Event.current;
            var hit = new Rect(r.x - 8f, r.y - 8f, r.width + 16f, r.height + 16f);
            bool hover = enabled && Draw.Hover(hit);
            if (sliding && slideKey == key && !Input.GetMouseButton(0)) { sliding = false; released = true; }
            if (enabled)
            {
                switch (e.type)
                {
                    case EventType.MouseDown:
                        if (e.button == 0 && hover)
                        {
                            sliding = true;
                            slideKey = key;
                            value = At();
                            e.Use();
                        }
                        break;
                    case EventType.MouseDrag:
                        if (sliding && slideKey == key) { value = At(); e.Use(); }
                        break;
                    case EventType.MouseUp:
                        if (sliding && slideKey == key && e.button == 0) { sliding = false; released = true; e.Use(); }
                        break;
                    case EventType.KeyDown:
                        if (hover && (e.keyCode == KeyCode.LeftArrow || e.keyCode == KeyCode.RightArrow))
                        {
                            float d = step > 0f ? step : (max - min) / 100f;
                            value = Snap(value + (e.keyCode == KeyCode.RightArrow ? d : -d));
                            released = true;
                            e.Use();
                        }
                        break;
                }
            }
            bool drag = sliding && slideKey == key;
            float grow = CSCommonAnim.Toward(key ^ 0x2545f491, drag ? 1f : 0f, 1f / GrowSeconds);
            if (!Draw.Repaint) return value;

            float old = Draw.Opacity;
            if (!enabled) Draw.Opacity = old * OffAlpha;
            float t = max > min ? Mathf.Clamp01((value - min) / (max - min)) : 0f;
            var track = new Rect(r.x, Mathf.Round(r.center.y - TrackH * 0.5f), r.width, TrackH);
            Draw.Round(track, Look.PillLine, TrackH * 0.5f);
            if (t > 0f) Draw.Round(new Rect(track.x, track.y, Mathf.Max(TrackH, track.width * t), TrackH), fill, TrackH * 0.5f);
            // 단계 눈금(12단계 이하): 2 × 2, 채운 쪽은 손잡이 색 · 빈 쪽은 흐린 회색
            if (step > 0f && max > min && (max - min) / step <= MaxTicks)
            {
                int n = Mathf.RoundToInt((max - min) / step);
                for (int i = 0; i <= n; i++)
                {
                    float tt = i / (float)n;
                    var c = new Rect(Mathf.Round(track.x + track.width * tt - Tick * 0.5f), Mathf.Round(track.center.y - Tick * 0.5f), Tick, Tick);
                    Draw.Fill(c, tt <= t + 0.001f ? Look.Knob : Look.KnobOff);
                }
            }
            var knob = new Vector2(Mathf.Round(track.x + track.width * t), track.center.y);
            float rad = Mathf.Lerp(Knob, KnobHover, grow);
            if (drag) Draw.Dot(knob, rad + RingDrag, Draw.Alpha(fill, DragAlpha));
            else if (grow > 0.01f) Draw.Dot(knob, rad + RingHover, Draw.Alpha(fill, HoverAlpha * grow));
            Draw.Dot(knob, rad, fill);
            Draw.Dot(knob, rad - Border, Look.Knob);
            if (drag && !string.IsNullOrEmpty(bubble))
            {
                float bw = Draw.TextWidth(bubble, 12f, true) + 16f;
                var b = new Rect(Mathf.Round(knob.x - bw * 0.5f), Mathf.Round(knob.y - rad - RingDrag - 22f), bw, 20f);
                Draw.Round(b, fill, b.height * 0.5f);
                Draw.Text(b, bubble, 12f, Draw.On(fill), true, TextAnchor.MiddleCenter);
            }
            Draw.Opacity = old;
            return value;
        }
    }
}
