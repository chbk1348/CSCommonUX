using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 호 · 빛 번짐(계기판)

        static readonly TexCache<long, BakedArc> arcCache = new(400, a => a.Tex);
        static Texture2D glowTex;

        /// <summary>
        /// 호 띠(고리 한 조각): center 중심, radius = 띠 가운데 반지름, width = 굵기(논리 단위). 각도는 도 — 0 = 오른쪽, 시계 방향 +(화면 y가 아래라 90 = 아래).
        /// feather > 0이면 가장자리가 그만큼(논리 단위) 부드럽게 퍼짐(빛 번짐), 0이면 1화소 안티앨리어싱만.
        /// 화면 화소 크기로 한 번 구운 텍스처(호를 감싼 사각만)를 정수 화소 자리에 찍음 — 각도는 0.5° 단위로 맞춰 움직이는 호(값 막대)도 텍스처가 끝없이 늘지 않음.
        /// </summary>
        public static void Arc(Vector2 center, float radius, float width, float fromDeg, float toDeg, Color c, float feather = 0f)
        {
            if (!Repaint || c.a <= 0f || width <= 0f) return;
            if (toDeg < fromDeg) { float t = fromDeg; fromDeg = toDeg; toDeg = t; }
            int a0 = Mathf.RoundToInt(fromDeg * 2f), a1 = Mathf.RoundToInt(toDeg * 2f);
            if (a1 - a0 > 720) a1 = a0 + 720;
            if (a1 <= a0) return;
            // 각도는 −720 ~ 720도 안으로(열쇠 칸 12비트)
            int shift = Mathf.FloorToInt(a0 / 720f) * 720;
            a0 -= shift;
            a1 -= shift;
            int rq = Mathf.Clamp(Mathf.RoundToInt(radius * S * 4f), 1, 16383);
            int wq = Mathf.Clamp(Mathf.RoundToInt(width * S * 4f), 1, 4095);
            int fq = Mathf.Clamp(Mathf.RoundToInt(feather * S * 2f), 0, 511);
            long key = ((long)rq << 42) | ((long)wq << 30) | ((long)fq << 21) | ((long)a0 << 11) | (uint)a1;
            if (!arcCache.TryGet(key, out var baked))
            {
                var tex = BakeArc(rq / 4f, wq / 4f, fq / 2f, a0 * 0.5f, a1 * 0.5f, out int ox, out int oy);
                baked = new BakedArc { Tex = tex, Ox = ox, Oy = oy };
                arcCache.Put(key, baked);
            }
            Vector2 sc = ToScreen(center);
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(new Rect(Mathf.Round(sc.x) - baked.Ox, Mathf.Round(sc.y) - baked.Oy, baked.Tex.width, baked.Tex.height), baked.Tex);
            GUI.color = old;
        }

        /// <summary>구운 호: 텍스처와 그 안 원 중심 자리(왼쪽 위에서 화소).</summary>
        struct BakedArc
        {
            public Texture2D Tex;
            public int Ox, Oy;
        }

        /// <summary>호 텍스처(화소 단위). ox · oy = 텍스처 왼쪽 위에서 원 중심까지(정수 화소 모서리).</summary>
        static Texture2D BakeArc(float r, float w, float f, float from, float to, out int ox, out int oy)
        {
            float half = w * 0.5f, inner = Mathf.Max(0f, r - half), outer = r + half;
            float pad = f + 2f;
            // 감싼 사각: 두 끝의 안 · 밖 점 + 범위 안 축 방향(0 · 90 · 180 · 270)의 바깥 점
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            void Take(float deg, float rad)
            {
                float x = Mathf.Cos(deg * Mathf.Deg2Rad) * rad, y = Mathf.Sin(deg * Mathf.Deg2Rad) * rad;
                minX = Mathf.Min(minX, x); maxX = Mathf.Max(maxX, x);
                minY = Mathf.Min(minY, y); maxY = Mathf.Max(maxY, y);
            }
            Take(from, inner); Take(from, outer); Take(to, inner); Take(to, outer);
            for (float axis = Mathf.Ceil(from / 90f) * 90f; axis <= to; axis += 90f) Take(axis, outer);
            ox = Mathf.CeilToInt(-minX + pad);
            oy = Mathf.CeilToInt(-minY + pad);
            int tw = Mathf.Max(1, ox + Mathf.CeilToInt(maxX + pad)), th = Mathf.Max(1, oy + Mathf.CeilToInt(maxY + pad));
            var tex = new Texture2D(tw, th, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point, hideFlags = HideFlags.HideAndDontSave };
            var px = new Color32[tw * th];
            float mid = (from + to) * 0.5f, halfSpan = (to - from) * 0.5f;
            bool full = to - from >= 359.9f;
            for (int y = 0; y < th; y++)
            for (int x = 0; x < tw; x++)
            {
                // 텍스처 0행 = 아래 → 화면 y는 위에서 아래로
                float dx = x + 0.5f - ox, dy = th - y - 0.5f - oy;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                float d = Mathf.Abs(dist - r) - half;
                if (!full)
                {
                    float ex = Mathf.Abs(Mathf.DeltaAngle(mid, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg)) - halfSpan;
                    float da = ex <= 0f ? ex * Mathf.Deg2Rad * dist : ex < 90f ? dist * Mathf.Sin(ex * Mathf.Deg2Rad) : dist;
                    d = Mathf.Max(d, da);
                }
                float a;
                if (f > 0.01f)
                {
                    a = Mathf.Clamp01((f - d) / (2f * f));
                    a = a * a * (3f - 2f * a);
                }
                else a = Mathf.Clamp01(0.5f - d);
                px[y * tw + x] = new Color32(255, 255, 255, (byte)(a * 255f));
            }
            tex.SetPixels32(px);
            tex.Apply(false);
            return tex;
        }

        /// <summary>부드러운 빛 번짐(가운데 진하고 가장자리로 0 — 타원으로 늘려 찍음). 계기판 뒤 후광 · 페달 빛.</summary>
        public static void Glow(Rect r, Color c)
        {
            if (!Repaint || c.a <= 0f || Off(r, 1f, 1f)) return;
            if (glowTex == null)
            {
                const int size = 128;
                glowTex = new Texture2D(size, size, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[size * size];
                float h = size * 0.5f;
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float d = Mathf.Sqrt((x + 0.5f - h) * (x + 0.5f - h) + (y + 0.5f - h) * (y + 0.5f - h)) / h;
                    float a = Mathf.Clamp01(1f - d);
                    a = a * a * (3f - 2f * a);
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
                glowTex.SetPixels32(px);
                glowTex.Apply(false);
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(r), glowTex);
            GUI.color = old;
        }

    }
}
