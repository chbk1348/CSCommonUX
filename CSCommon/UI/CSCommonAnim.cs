using System.Collections.Generic;
using UnityEngine;

namespace CSCommon
{
    /// <summary>
    /// IMGUI 부품 애니메이션: 열쇠(부품마다 고정된 정수)별 0 ~ 1 값을 목표 쪽으로 시간에 맞춰 다가가게 함(게임 일시 정지와 상관없이 실제 시간).
    /// 그리기(Repaint) 때만 움직이고, 한동안 안 쓴 열쇠는 그대로 남아도 작음(부품 수만큼).
    /// </summary>
    internal static class CSCommonAnim
    {
        struct Slot
        {
            public float Value;
            public float At;
        }

        static readonly Dictionary<int, Slot> slots = new();

        /// <summary>목표(0 또는 1 등)로 다가가는 값. speed = 초당 다가가는 비율(12 ≈ 0.15초에 거의 다 옴). 처음 보는 열쇠는 목표에서 시작.</summary>
        public static float Toward(int key, float target, float speed = 12f) => Toward(key, target, speed, target);

        /// <summary>Toward 와 같고, 처음 보는 열쇠는 start 에서 시작(첫 등장부터 움직이게).</summary>
        public static float Toward(int key, float target, float speed, float start)
        {
            float now = Time.unscaledTime;
            if (!slots.TryGetValue(key, out var s))
            {
                s = new Slot { Value = start, At = now };
                slots[key] = s;
                if (start == target) return target;
            }
            if (Event.current == null || Event.current.type == EventType.Repaint)
            {
                float dt = Mathf.Clamp(now - s.At, 0f, 0.1f);
                s.Value = Mathf.MoveTowards(s.Value, target, dt * speed);
                s.At = now;
                slots[key] = s;
            }
            return s.Value;
        }

        /// <summary>
        /// 화소 값(자리 · 너비)을 목표로 부드럽게: 남은 거리를 rate 비율로 줄임(18 ≈ 0.15초에 거의 다 옴, 끝에서 느려짐).
        /// 처음 보는 열쇠는 목표에서 시작.
        /// </summary>
        public static float Smooth(int key, float target, float rate = 18f)
        {
            float now = Time.unscaledTime;
            if (!slots.TryGetValue(key, out var s))
            {
                slots[key] = new Slot { Value = target, At = now };
                return target;
            }
            if (Event.current == null || Event.current.type == EventType.Repaint)
            {
                float dt = Mathf.Clamp(now - s.At, 0f, 0.1f);
                s.Value = Mathf.Abs(target - s.Value) < 0.25f ? target : Mathf.Lerp(s.Value, target, 1f - Mathf.Exp(-rate * dt));
                s.At = now;
                slots[key] = s;
            }
            return s.Value;
        }

        // ---------------- 모달(IMGUI 창) 열림 · 닫힘

        static float modalOpacity, modalShift;

        /// <summary>모달이 보일 정도 0 ~ 1: 열면 0 → 1(0.18초), 닫으면 1 → 0(0.14초). 0 이면 그리지 말 것.</summary>
        public static float Modal(int key, bool open) => Toward(key, open ? 1f : 0f, open ? 5.5f : 7f, 0f);

        /// <summary>모달 그리기 시작: 투명도를 v 로, 10px 아래에서 떠오름(닫을 땐 가라앉음). EndModal 과 짝.</summary>
        public static void BeginModal(float v)
        {
            modalOpacity = Draw.Opacity;
            modalShift = Mathf.Round((1f - EaseOut(v)) * 10f);
            Draw.Opacity *= EaseOut(v);
            Draw.Shift(new Vector2(0f, modalShift));
        }

        public static void EndModal()
        {
            Draw.Shift(new Vector2(0f, -modalShift));
            Draw.Opacity = modalOpacity;
        }

        /// <summary>0 ~ 1 을 끝에서 느려지게(ease-out cubic).</summary>
        public static float EaseOut(float t)
        {
            t = 1f - Mathf.Clamp01(t);
            return 1f - t * t * t;
        }

        /// <summary>
        /// 등장 전환: 열쇠의 '보이는 상태'가 이번에 켜졌거나(지난번 그린 뒤 gap 초 넘게 안 그림) id 가 바뀌면 처음부터.
        /// 0 → 1 로 duration 초 동안 가는 진행도(ease-out)를 돌려줌.
        /// </summary>
        public static float Appear(int key, int id, float duration = 0.18f, float gap = 0.25f)
        {
            float now = Time.unscaledTime;
            if (!appear.TryGetValue(key, out var a) || a.Id != id || now - a.Seen > gap)
                a = new Show { Id = id, Start = now };
            a.Seen = now;
            appear[key] = a;
            return EaseOut((now - a.Start) / duration);
        }

        struct Show
        {
            public int Id;
            public float Start, Seen;
        }

        static readonly Dictionary<int, Show> appear = new();

        /// <summary>안 보이게 됐을 때: 다음에 보이면 등장 전환을 처음부터.</summary>
        public static void Forget(int key) => appear.Remove(key);

        /// <summary>문자열 두 개로 고정 열쇠(부품 종류 · 이름).</summary>
        public static int Key(string a, string b)
        {
            unchecked { return ((a?.GetHashCode() ?? 0) * 397) ^ (b?.GetHashCode() ?? 0); }
        }
    }
}
