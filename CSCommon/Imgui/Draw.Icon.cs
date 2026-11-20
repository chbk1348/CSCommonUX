using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 선명한 아이콘 · 그림

        /// <summary>
        /// 한 틀에 새로 굽는 아이콘 수 상한(3.1): 넘치면 같은 아이콘의 다른 크기 구운 것을 늘려 찍고 다음 틀에 구움 — 크기가 움직이는 아이콘이 한 틀에 여러 장 굽지 않게.
        /// 처음 보는 아이콘(다른 크기도 없음)은 상한과 상관없이 바로 구움(빈칸이 보이지 않게).
        /// </summary>
        const int IconBakesPerFrame = 6;
        static int iconBakeFrame = -1, iconBakes;
        /// <summary>경로 글 → 가장 최근에 구운 텍스처(늘려 찍을 대신 그림).</summary>
        static readonly Dictionary<string, Texture2D> iconAny = new();

        // 키 = (경로 글, 화소, 선 굵기 · viewBox 를 1/10000 단위 정수로) — 예전 문자열 이어 붙인 키와 같은 구분, 찾을 때 쓰레기 없음
        static readonly TexCache<TextKey, Texture2D> iconCache = new(1024, t => t, TextKeyComparer.I);

        /// <summary>
        /// 아웃라인 아이콘(SVG 경로, viewBox 0 ~ viewBox 정사각): 화면 화소 크기에 맞춰 선까지 거리로 한 번 구운 텍스처(둥근 끝 · 둥근 이음)를
        /// 정수 화소 자리에 그대로 찍음 — 짧은 선 띠 · 점을 이어 그리면 작은 크기에서 뭉개졌음. stroke = viewBox 단위 선 굵기.
        /// </summary>
        public static void Icon(string d, Rect r, Color c, float viewBox = 24f, float stroke = 1.75f)
        {
            if (!Repaint || c.a <= 0f || string.IsNullOrEmpty(d) || Off(new Rect(r.x, r.y, r.width, r.width), 4f, 4f)) return;
            Rect sr = ToScreen(r);
            int px = Mathf.Max(4, Mathf.RoundToInt(sr.width));
            var key = new TextKey(d, px, Mathf.RoundToInt(stroke * 10000f), Mathf.RoundToInt(viewBox * 10000f));
            if (!iconCache.TryGet(key, out var tex))
            {
                if (iconBakeFrame != Time.frameCount)
                {
                    iconBakeFrame = Time.frameCount;
                    iconBakes = 0;
                }
                if (iconBakes >= IconBakesPerFrame && iconAny.TryGetValue(d, out var any) && any != null)
                {
                    // 이번 틀 굽기 몫을 다 씀: 다른 크기 것을 늘려 찍고 다음 틀에 구움
                    Color o = GUI.color;
                    GUI.color = Op(c);
                    GUI.DrawTexture(new Rect(Mathf.Round(sr.x), Mathf.Round(sr.y), px, px), any);
                    GUI.color = o;
                    return;
                }
                iconBakes++;
                tex = BakeIcon(d, px, viewBox, stroke);
                iconCache.Put(key, tex);
                iconAny[d] = tex;
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(new Rect(Mathf.Round(sr.x), Mathf.Round(sr.y), px, px), tex);
            GUI.color = old;
        }

        static Texture2D BakeIcon(string d, int px, float viewBox, float stroke)
        {
            float k = px / viewBox, half = stroke * k * 0.5f;
            var segs = new List<Vector4>();
            foreach (var poly in ParsePath(d))
            {
                if (poly.Count == 1) segs.Add(new Vector4(poly[0].x * k, poly[0].y * k, poly[0].x * k, poly[0].y * k));
                for (int i = 0; i + 1 < poly.Count; i++)
                    segs.Add(new Vector4(poly[i].x * k, poly[i].y * k, poly[i + 1].x * k, poly[i + 1].y * k));
            }
            var tex = new Texture2D(px, px, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point, hideFlags = HideFlags.HideAndDontSave };
            var pxs = new Color32[px * px];
            for (int y = 0; y < px; y++)
            for (int x = 0; x < px; x++)
            {
                // 텍스처 0행 = 아래 → 아이콘 y는 위에서 아래로
                float qx = x + 0.5f, qy = px - y - 0.5f, best = float.MaxValue;
                foreach (var s in segs)
                {
                    float ax = s.x, ay = s.y, dx = s.z - ax, dy = s.w - ay;
                    float len2 = dx * dx + dy * dy;
                    float t = len2 > 1e-6f ? Mathf.Clamp01(((qx - ax) * dx + (qy - ay) * dy) / len2) : 0f;
                    float ex = ax + dx * t - qx, ey = ay + dy * t - qy;
                    float dist2 = ex * ex + ey * ey;
                    if (dist2 < best) best = dist2;
                }
                float a = Mathf.Clamp01(half - Mathf.Sqrt(best) + 0.5f);
                pxs[y * px + x] = new Color32(255, 255, 255, (byte)(a * 255f));
            }
            tex.SetPixels32(pxs);
            tex.Apply(false);
            return tex;
        }

        static readonly TexCache<long, Texture2D> sharpCache = new(200, t => t);

        /// <summary>
        /// 그림을 화면 화소 크기로 미리 줄여(넓이 평균, 투명도 곱해 섞음) 정수 화소 자리에 찍음 — 큰 로고를 작게 그릴 때 흐려지지 않게.
        /// 원본을 못 읽으면(읽기 막힌 텍스처) 보통 Image.
        /// </summary>
        public static void ImageSharp(Rect r, Texture2D src, float alpha = 1f)
        {
            if (!Repaint || src == null || Off(r, 1f, 1f)) return;
            Rect sr = ToScreen(r);
            // 비율 유지(ScaleToFit)
            float aspect = src.width / (float)Mathf.Max(1, src.height);
            float w = sr.width, h = sr.height;
            if (w / h > aspect) w = h * aspect; else h = w / aspect;
            int pw = Mathf.Max(1, Mathf.RoundToInt(w)), ph = Mathf.Max(1, Mathf.RoundToInt(h));
            long key = ((long)src.GetInstanceID() << 24) ^ ((long)pw << 12) ^ ph;
            if (!sharpCache.TryGet(key, out var tex))
            {
                tex = Downscale(src, pw, ph);
                if (tex == null) { sharpFailed.Add(src.GetInstanceID()); ImageRaw(r, src, alpha, ScaleMode.ScaleToFit); return; }
                // 크기가 계속 바뀌는 그림(움직임)이 줄인 그림을 끝없이 쌓지 않게: 상한에서 오래 안 쓴 것부터 지움(TexCache)
                sharpCache.Put(key, tex);
            }
            Color old = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, alpha * old.a * Opacity);
            GUI.DrawTexture(new Rect(Mathf.Round(sr.x + (sr.width - pw) * 0.5f), Mathf.Round(sr.y + (sr.height - ph) * 0.5f), pw, ph), tex);
            GUI.color = old;
        }

        static Texture2D Downscale(Texture2D src, int w, int h)
        {
            Color32[] from;
            try { from = src.GetPixels32(); }
            catch { return null; }
            int sw = src.width, sh = src.height;
            var to = new Color32[w * h];
            for (int y = 0; y < h; y++)
            {
                int y0 = y * sh / h, y1 = Mathf.Max(y0 + 1, (y + 1) * sh / h);
                for (int x = 0; x < w; x++)
                {
                    int x0 = x * sw / w, x1 = Mathf.Max(x0 + 1, (x + 1) * sw / w);
                    float r = 0f, g = 0f, b = 0f, a = 0f;
                    for (int yy = y0; yy < y1; yy++)
                    for (int xx = x0; xx < x1; xx++)
                    {
                        var p = from[yy * sw + xx];
                        float pa = p.a / 255f;
                        r += p.r * pa; g += p.g * pa; b += p.b * pa; a += pa;
                    }
                    float n = (x1 - x0) * (y1 - y0);
                    to[y * w + x] = a > 0f
                        ? new Color32((byte)(r / a), (byte)(g / a), (byte)(b / a), (byte)(a / n * 255f))
                        : new Color32(0, 0, 0, 0);
                }
            }
            var tex = new Texture2D(w, h, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point, hideFlags = HideFlags.HideAndDontSave };
            tex.SetPixels32(to);
            tex.Apply(false);
            return tex;
        }

    }
}
