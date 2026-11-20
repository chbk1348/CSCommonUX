using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.ScrollBar — 세로 스크롤 막대(창 몸통 · 목록 · 판 안 칸 · 알림 창이 같이 씀): view 오른쪽 안쪽에 막대 CSCommonDesign.ScrollBarW
    /// (칸 끝 → ScrollBarInset · 최소 ScrollBarMinH · 반지름 폭/2), 보통 Look.PillLine · 호버 Look.PillSub · 끄는 중 Look.PastelStrong(색만 바뀌고 크기는 그대로).
    /// 누름 띠 = view 오른쪽 끝 ScrollBarHit 폭. 막대를 잡고 끌면 잡은 자리 그대로 따라오고, 길을 누르면 막대 가운데가 그 자리로 온 뒤 이어 끎.
    /// 끄는 동안: Draw.Holding 이 참(지도 도구 · 툴팁), Draw.Begin 이 다른 누름 · 휠을 먹고, InputBlocker 가 온 화면을 막음(게임 도구 · 카메라).
    /// 마우스를 창 밖에서 떼도 다음 이벤트에 끝남. 부르는 법: 내용 그리기 전에 Handle(막대 띠 밑 칸 단추보다 먼저), 내용을 다 그린 뒤 Paint(위에 그림).
    /// scroll = 목표 값(부드러운 스크롤이면 다가가기 전 값 — 막대가 마우스에 붙고 내용이 따라옴). 막대 하나는 view 자리로 알아봄.
    /// </summary>
    public static class ScrollBar
    {
        const float W = CSCommonDesign.ScrollBarW, Inset = CSCommonDesign.ScrollBarInset, MinH = CSCommonDesign.ScrollBarMinH, HitW = CSCommonDesign.ScrollBarHit;

        static bool drag;
        static int dragKey;
        static float grab; // 막대 위 끝 → 잡은 자리

        /// <summary>막대를 끄는 중. 마우스를 떼면(창 밖 · 막대가 안 그려지는 동안이어도) 여기서 풂.</summary>
        public static bool Dragging
        {
            get
            {
                if (drag && !Input.GetMouseButton(0)) drag = false;
                return drag;
            }
        }

        static Rect Track(Rect view) => new(view.xMax - Inset - W, view.y + Inset, W, Mathf.Max(0f, view.height - Inset * 2f));
        static Rect Hit(Rect view) => new(view.xMax - HitW, view.y, HitW, view.height);
        static int Key(Rect view) { unchecked { return (Mathf.RoundToInt(view.xMax) * 397 ^ Mathf.RoundToInt(view.y)) * 397 ^ Mathf.RoundToInt(view.height); } }

        /// <summary>막대 길이: 길 × 보이는 몫, 최소 MinH · 길보다 길지 않음.</summary>
        public static float ThumbH(float track, float viewH, float contentH) => Mathf.Min(track, Mathf.Max(MinH, contentH > 0f ? track * viewH / contentH : track));
        /// <summary>스크롤(0 ~ max) → 막대 위 끝(길 위에서 0 ~ track − thumb).</summary>
        public static float ToThumb(float scroll, float max, float track, float thumb) => max > 0f ? (track - thumb) * Mathf.Clamp01(scroll / max) : 0f;
        /// <summary>막대 위 끝 → 스크롤(길 밖은 끝에 붙음).</summary>
        public static float ToScroll(float thumbY, float max, float track, float thumb) => track > thumb ? max * Mathf.Clamp01(thumbY / (track - thumb)) : 0f;

        /// <summary>누름 · 끌기를 받아 scroll 을 바꿈(바뀌었으면 true). enabled = false(위에 모달)면 새로 잡지 않음. 내용을 그리기 전에 부를 것.</summary>
        public static bool Handle(Rect view, float contentH, ref float scroll, bool enabled = true)
        {
            int key = Key(view);
            bool mine = Dragging && dragKey == key;
            float max = contentH - view.height;
            if (max <= 0f)
            {
                if (mine) drag = false;
                return false;
            }
            var e = Event.current;
            var t = Track(view);
            float th = ThumbH(t.height, view.height, contentH), before = scroll;
            if (mine)
            {
                if (e.rawType == EventType.MouseUp && e.button == 0) { drag = false; e.Use(); }
                else if (e.type == EventType.MouseDrag) { scroll = ToScroll(Draw.Mouse.y - t.y - grab, max, t.height, th); e.Use(); }
            }
            else if (enabled && !Dragging && e.type == EventType.MouseDown && e.button == 0 && Draw.Hover(Hit(view)))
            {
                float top = t.y + ToThumb(scroll, max, t.height, th), m = Draw.Mouse.y;
                grab = m >= top && m <= top + th ? m - top : th * 0.5f;
                drag = true;
                dragKey = key;
                scroll = ToScroll(m - t.y - grab, max, t.height, th);
                e.Use();
            }
            return scroll != before;
        }

        /// <summary>막대를 그림(넘칠 때 · Repaint 만). 내용을 다 그린 뒤 부를 것.</summary>
        public static void Paint(Rect view, float contentH, float scroll)
        {
            float max = contentH - view.height;
            if (max <= 0f || !Draw.Repaint) return;
            var t = Track(view);
            float th = ThumbH(t.height, view.height, contentH);
            Color c = Dragging && dragKey == Key(view) ? Look.PastelStrong : !Dragging && Draw.Hover(Hit(view)) ? Look.PillSub : Look.PillLine;
            Draw.Round(new Rect(t.x, t.y + ToThumb(scroll, max, t.height, th), W, th), c, W * 0.5f);
        }

        /// <summary>자가 검사: 스크롤 ↔ 막대 왕복 · 양 끝 · 막대 최소 · 길 밖 누름은 끝에 붙음.</summary>
        public static void SelfCheck(Action<bool, string> check)
        {
            static bool Near(float a, float b) => Mathf.Abs(a - b) < 0.01f;
            float track = 400f, max = 1600f, th = ThumbH(track, 400f, 2000f);
            check(Near(th, 80f), "scrollbar thumb share");
            check(Near(ThumbH(track, 10f, 100000f), MinH) && Near(ThumbH(track, 500f, 400f), track), "scrollbar thumb min/max");
            check(Near(ToScroll(ToThumb(600f, max, track, th), max, track, th), 600f), "scrollbar round trip");
            check(Near(ToThumb(0f, max, track, th), 0f) && Near(ToThumb(max, max, track, th), track - th), "scrollbar ends");
            check(Near(ToScroll(-50f, max, track, th), 0f) && Near(ToScroll(track + 50f, max, track, th), max), "scrollbar clamp");
        }
    }
}
