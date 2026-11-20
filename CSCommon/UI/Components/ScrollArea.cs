using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.ScrollArea — 판 안 따로 스크롤하는 칸(시안 표 · 인스펙터 overflow-y): 휠 30 · 오른쪽 막대 = CSCommonUX.ScrollBar(잡고 끌기 · 길 누르기).
    /// 창 몸통이 휠을 먼저 써도(Event.Use) rawType 으로 받음 — 창 몸통이 스크롤하지 않게 부르는 쪽이 내용을 창 높이에 맞출 것.
    /// 스크롤 자리는 id 마다 남음.
    /// 3.1 부드러운 스크롤: 휠 · 길 누르기는 목표 자리를 바꾸고 내용 · 막대는 그 자리로 다가감(SmoothRate, 끝에서 느려짐). 막대를 끄는 동안은 바로 따라옴.
    /// </summary>
    public static class ScrollArea
    {
        public const float Step = 30f, BarW = CSCommonDesign.ScrollBarW, BarInset = CSCommonDesign.ScrollBarInset, BarMinH = CSCommonDesign.ScrollBarMinH;
        /// <summary>부드러운 스크롤 빠르기(CSCommonAnim.Smooth 비율, 16 ≈ 0.2초에 거의 다 옴 — 설정 패널 스크롤과 같음).</summary>
        public const float SmoothRate = 16f;

        /// <summary>id → 목표 자리(휠 · 막대가 바꿈).</summary>
        static readonly Dictionary<string, float> offsets = new();

        static int AnimKey(string id) => CSCommonAnim.Key("scrollarea", id);

        /// <summary>처음(맨 위)으로 — 다가가지 않고 바로(탭을 바꾸면 새 화면이 위에서 시작).</summary>
        public static void Reset(string id)
        {
            offsets.Remove(id);
            CSCommonAnim.Set(AnimKey(id), 0f);
        }

        /// <summary>r 안을 잘라 그림: paint 는 안쪽 좌표(위 0)에서 지금 보이는 칸을 받음. wheel = 휠 · 막대를 받을지(위에 모달이 떠 있으면 false).</summary>
        public static void In(Rect r, string id, float contentH, bool wheel, Action<Rect> paint)
        {
            var e = Event.current;
            float max = Mathf.Max(0f, contentH - r.height);
            offsets.TryGetValue(id, out float s);
            if (wheel && max > 0f && e.type == EventType.ScrollWheel && Draw.Hover(r))
            {
                float to = Mathf.Clamp(s + e.delta.y * Step, 0f, max);
                if (to != s) { s = to; e.Use(); } // 끝에 닿으면 휠을 창으로 넘김
            }
            int key = AnimKey(id);
            // 내용보다 먼저: 막대 띠 밑 칸 단추가 누름을 가져가지 않게. 끄는 동안은 다가가지 않고 바로
            if (ScrollBar.Handle(r, contentH, ref s, wheel) && ScrollBar.Dragging) CSCommonAnim.Set(key, Mathf.Clamp(s, 0f, max));
            s = Mathf.Clamp(s, 0f, max);
            offsets[id] = s;
            // 내용이 줄어 목표가 당겨지면 보이는 자리도 범위 안으로
            float shown = Mathf.Clamp(CSCommonAnim.Smooth(key, s, SmoothRate), 0f, max);
            Draw.BeginClip(r, new Vector2(0f, Mathf.Round(shown)));
            paint(new Rect(0f, Mathf.Round(shown), r.width, r.height));
            Draw.EndClip();
            ScrollBar.Paint(r, contentH, shown);
        }
    }
}
