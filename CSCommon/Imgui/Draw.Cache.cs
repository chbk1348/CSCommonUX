using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 구운 텍스처 캐시(3.1): 상한이 있고, 넘치면 오래 안 쓴 것부터 1/4 을 지움(텍스처 Destroy)
        // 예전: 아이콘 · 고리는 상한 없이 쌓였고(크기가 움직이면 끝없이), 그림 · 호는 상한에서 통째로 비워 다음 틀에 전부 다시 구움(틀 튐).
        // texOf = null 이면 텍스처 없는 값 캐시(글 폭 · 높이) — 지울 때 Destroy 없이 같은 방식으로 오래된 것부터 지움.

        sealed class TexCache<TKey, TValue>
        {
            struct Entry
            {
                public TValue Value;
                public int Used;
            }

            readonly Dictionary<TKey, Entry> map;
            readonly int cap;
            readonly Func<TValue, Texture2D> texOf;
            readonly List<KeyValuePair<int, TKey>> order = new();

            public TexCache(int cap, Func<TValue, Texture2D> texOf, IEqualityComparer<TKey> cmp = null)
            {
                this.cap = cap;
                this.texOf = texOf;
                map = cmp != null ? new Dictionary<TKey, Entry>(cmp) : new Dictionary<TKey, Entry>();
            }

            public int Count => map.Count;

            public bool TryGet(TKey key, out TValue value)
            {
                if (map.TryGetValue(key, out var e) && (texOf == null || texOf(e.Value) != null))
                {
                    int f = Time.frameCount;
                    if (e.Used != f) { e.Used = f; map[key] = e; } // 같은 틀에 여러 번 찾는 글 폭은 다시 쓰지 않음
                    value = e.Value;
                    return true;
                }
                value = default;
                return false;
            }

            public void Put(TKey key, TValue value)
            {
                if (map.Count >= cap && !map.ContainsKey(key)) Evict();
                map[key] = new Entry { Value = value, Used = Time.frameCount };
            }

            /// <summary>가장 오래 안 쓴 1/4 을 지움(이번 틀에 쓴 것은 남김).</summary>
            void Evict()
            {
                order.Clear();
                foreach (var kv in map) order.Add(new KeyValuePair<int, TKey>(kv.Value.Used, kv.Key));
                order.Sort((a, b) => a.Key.CompareTo(b.Key));
                int n = Mathf.Max(1, map.Count / 4), frame = Time.frameCount;
                for (int i = 0; i < n && i < order.Count && order[i].Key != frame; i++)
                {
                    var t = texOf?.Invoke(map[order[i].Value].Value);
                    if (t != null) UnityEngine.Object.Destroy(t);
                    map.Remove(order[i].Value);
                }
                order.Clear();
            }

            public void Clear()
            {
                if (texOf != null)
                foreach (var e in map.Values)
                {
                    var t = texOf(e.Value);
                    if (t != null) UnityEngine.Object.Destroy(t);
                }
                map.Clear();
            }
        }

        // ---------------- 처음 굽기 몫(3.2): 창을 열 때 처음 보는 모양(둥근 판 · 그림자 · 아이콘)을 한 틀에 다 구워 멈칫했음
        // 한 틀에 굽기에 쓰는 시간을 BakeBudgetMs 로 막고, 넘친 것은 그 틀에 대신 그림(네모 · 없음)으로 그린 뒤 다음 틀에 구움. 틀마다 첫 장은 늘 구움(멈추지 않고 나아감).

        const long BakeBudgetMs = 3;
        static readonly long bakeBudget = System.Diagnostics.Stopwatch.Frequency * BakeBudgetMs / 1000;
        static readonly PerfWatch bakeWatch = new("텍스처 굽기 한 장");
        static int bakeFrame = -1, frameBakes, frameDeferred, burstFrames, burstBakes, burstDeferred;
        static long bakeT0, frameTicks, burstTicks, burstMax;

        /// <summary>지금 하나 더 구워도 되나 — 참이면 구운 뒤 꼭 Baked(). 거짓이면 이번 틀은 대신 그림으로.</summary>
        static bool CanBake()
        {
            int f = Time.frameCount;
            if (bakeFrame != f) { FoldBakeFrame(); bakeFrame = f; }
            if (frameBakes > 0 && frameTicks >= bakeBudget) { frameDeferred++; return false; }
            bakeWatch.Begin();
            bakeT0 = System.Diagnostics.Stopwatch.GetTimestamp();
            return true;
        }

        static void Baked()
        {
            frameTicks += System.Diagnostics.Stopwatch.GetTimestamp() - bakeT0;
            frameBakes++;
            bakeWatch.End();
        }

        static void FoldBakeFrame()
        {
            if (frameBakes == 0 && frameDeferred == 0) return;
            burstFrames++;
            burstBakes += frameBakes;
            burstDeferred += frameDeferred;
            burstTicks += frameTicks;
            if (frameTicks > burstMax) burstMax = frameTicks;
            frameBakes = frameDeferred = 0;
            frameTicks = 0;
        }

        /// <summary>
        /// Begin 에서: 굽기가 한 틀 넘게 끊기면 이어진 굽기 몰림(창 열기 한 번)을 한 줄로 — 합 50ms 이상은 늘(SlowLog), 2ms 이상은 자세한 로그(PerfWatch.On)일 때만.
        /// shortcut: 매 틀 굽기가 끊기지 않으면(크기가 계속 움직이는 그림) 줄이 안 나옴, 필요해지면 N틀마다 끊어 남기게.
        /// </summary>
        static void FlushBakeBurst()
        {
            if (bakeFrame < 0 || bakeFrame >= Time.frameCount - 1) return;
            FoldBakeFrame();
            bakeFrame = -1;
            double ms = 1000.0 / System.Diagnostics.Stopwatch.Frequency;
            bool big = burstTicks * 20 >= System.Diagnostics.Stopwatch.Frequency;
            if (big || burstTicks * 500 >= System.Diagnostics.Stopwatch.Frequency && PerfWatch.On())
            {
                string line = $"성능: 텍스처 굽기 몰림 {burstBakes}장 · 합 {burstTicks * ms:0.0}ms · {burstFrames}틀(한 틀 최대 {burstMax * ms:0.0}ms) · 미룬 그리기 {burstDeferred}회(부른 횟수, 같은 모양 중복 포함)";
                if (big) PerfWatch.SlowLog(line); else PerfWatch.Log(line);
            }
            burstFrames = burstBakes = burstDeferred = 0;
            burstTicks = burstMax = 0;
        }

    }
}
