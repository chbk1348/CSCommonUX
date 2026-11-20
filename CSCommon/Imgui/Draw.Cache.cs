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
                if (map.TryGetValue(key, out var e) && texOf(e.Value) != null)
                {
                    e.Used = Time.frameCount;
                    map[key] = e;
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
                    var t = texOf(map[order[i].Value].Value);
                    if (t != null) UnityEngine.Object.Destroy(t);
                    map.Remove(order[i].Value);
                }
                order.Clear();
            }

            public void Clear()
            {
                foreach (var e in map.Values)
                {
                    var t = texOf(e.Value);
                    if (t != null) UnityEngine.Object.Destroy(t);
                }
                map.Clear();
            }
        }

    }
}
