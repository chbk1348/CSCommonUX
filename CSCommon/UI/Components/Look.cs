using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>컴포넌트 눌림 · 그림자 · 열쇠(색 토큰은 Style/Look.cs).</summary>
    public static partial class Look
    {
        /// <summary>눌림: 누르는 데 걸리는 초 · 떼고 돌아오는 초 · 돌아오는 곡선 cubic-bezier(.2, .9, .3, 1.3)(시안 '.press' — 살짝 넘쳤다 앉음).</summary>
        public const float PressInSeconds = 0.03f, PressOutSeconds = 0.26f, PressX1 = 0.2f, PressY1 = 0.9f, PressX2 = 0.3f, PressY2 = 1.3f;

        struct PressRun
        {
            public float V, From, Rel, Last;
            public bool Down;
        }

        static readonly Dictionary<int, PressRun> presses = new();
        static float nextPressPrune;
        static readonly List<int> stalePresses = new();

        /// <summary>CSCommonAnim 과 같은 주기로 오래 안 쓴 눌림 열쇠를 지움(화면 자리 열쇠가 쌓이지 않게, 할당 없음).</summary>
        static void PrunePresses(float now)
        {
            if (now < nextPressPrune) return;
            nextPressPrune = now + CSCommonAnim.PruneEvery;
            foreach (var kv in presses) if (now - kv.Value.Last > CSCommonAnim.PruneUnused) stalePresses.Add(kv.Key);
            foreach (int k in stalePresses) presses.Remove(k);
            stalePresses.Clear();
        }

        /// <summary>
        /// 누름 정도: 누르는 동안 0.03초에 0 → 1, 떼면 0.26초 동안 시안 곡선으로 0 쪽으로 돌아가며 잠깐 0 아래(음수)로 넘침.
        /// 배율은 PressScale(press, 눌림 배율) 로 — 넘침이 잘리지 않게(Mathf.Lerp 는 0 ~ 1 로 자름). 그리기(Repaint) 때만 움직임.
        /// </summary>
        public static float Press(int key, Rect r, bool enabled = true)
        {
            key ^= 0x27d4eb2f;
            bool down = enabled && Draw.Hover(r) && Draw.Pressed(r);
            float now = Time.unscaledTime;
            PrunePresses(now);
            if (!presses.TryGetValue(key, out var s)) s = new PressRun { Rel = -100f, Last = now };
            if (Event.current == null || Event.current.type == EventType.Repaint)
            {
                if (down) s.V = Mathf.MoveTowards(Mathf.Max(s.V, 0f), 1f, Mathf.Clamp(now - s.Last, 0f, 0.1f) / PressInSeconds);
                else
                {
                    if (s.Down) { s.From = s.V; s.Rel = now; }
                    float t = (now - s.Rel) / PressOutSeconds;
                    s.V = t >= 1f ? 0f : s.From * (1f - Modal.Ease(t, PressX1, PressY1, PressX2, PressY2));
                }
                s.Down = down;
                s.Last = now;
                presses[key] = s;
            }
            return s.V;
        }

        /// <summary>눌림 정도 → 그릴 배율(1 ↔ scale, 뗄 때 1 을 살짝 넘음).</summary>
        public static float PressScale(float press, float scale) => Mathf.LerpUnclamped(1f, scale, press);

        public static int Key(Rect r, string a, string b = null)
        {
            // 잘라 그리는 영역 자리(ClipBase)를 더함: 영역 안 좌표만 쓰면 다른 섹션 몸통의 같은 자리 행이 호버 면을 같이 켰음
            var b0 = Draw.ClipBase;
            unchecked { return CSCommonAnim.Key(a ?? "", b ?? "") ^ Mathf.RoundToInt(r.x + b0.x) * 397 ^ Mathf.RoundToInt(r.y + b0.y) * 31; }
        }

        /// <summary>
        /// 떠 있는 판 그림자 + 바탕 + 1px 선. 그림자는 1px 씩 넓어지며 옅어지는 여러 겹(흐림 흉내) — 예전 두 겹 큰 상자는
        /// 가장자리가 띠처럼 겹겹이 보였음. 겹은 미리 구운 한 장(Draw.Shadow)으로, 모서리에 비해 작은 판만 겹마다 그림.
        /// </summary>
        public static void Elevated(Rect r, float radius, Color fill, float shadow = 1f)
        {
            if (!Draw.Repaint) return;
            const int layers = 8;
            if (!Draw.Shadow(r, radius, ShadowAlpha * shadow))
            for (int i = layers; i >= 1; i--)
            {
                float k = 1f - (i - 1f) / layers;
                Draw.Round(new Rect(r.x - i, r.y + 2f + i * 0.5f - i, r.width + i * 2f, r.height + i * 2f), Draw.Alpha(Color.black, ShadowAlpha * k * shadow), radius + i);
            }
            Draw.Round(r, fill, radius);
            Draw.RoundOutline(r, PanelLine, radius);
        }

        /// <summary>고른 칸(떠오른 면): 한 단계 밝은 면 + 위 1px 밝은 선, 그림자 · 밑줄 · 테두리 없음. t = 나타남 0 ~ 1.</summary>
        public static void Raised(Rect r, float radius, float t = 1f)
        {
            if (!Draw.Repaint || t <= 0.01f) return;
            // 그림자 없음: 알약 · 둥근 칸은 1px 만 내려도 아래가 어두운 초승달로 보였음 — 밝은 면 + 위 1px 밝은 선으로만 떠오름
            Draw.Round(r, Draw.Alpha(Pal.Selected, t), radius);
            // 다크: 위 1px 밝은 선 / 라이트: 흰 면이 바탕과 구분되게 1px 테두리
            if (CSCommonTheme.Light) Draw.RoundOutline(r, Draw.Alpha(Look.PanelLine, t), radius);
            else Draw.Fill(new Rect(r.x + radius, r.y, r.width - radius * 2f, 1f), Draw.Alpha(Color.white, 0.08f * t));
        }
    }
}
