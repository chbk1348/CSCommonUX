using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 원

        static Texture2D Disc
        {
            get
            {
                if (disc != null) return disc;
                const int size = 128;
                disc = new Texture2D(size, size, TextureFormat.ARGB32, true) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Trilinear, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[size * size];
                float r = size * 0.5f;
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float d = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r));
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(r - d) * 255f));
                }
                disc.SetPixels32(px);
                disc.Apply(true);
                return disc;
            }
        }

        public static void Dot(Vector2 center, float radius, Color c)
        {
            if (!Repaint || c.a <= 0f || Off(new Rect(center.x - radius, center.y - radius, radius * 2f, radius * 2f), 1f, 1f)) return;
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(new Rect(center.x - radius, center.y - radius, radius * 2f, radius * 2f)), Disc);
            GUI.color = old;
        }

        public static void Ring(Vector2 center, float radius, float width, Color c)
        {
            if (!Repaint || c.a <= 0f || Off(new Rect(center.x - radius, center.y - radius, radius * 2f, radius * 2f), 1f, 1f)) return;
            int d = Mathf.Max(2, Mathf.RoundToInt(radius * 2f * S));
            int w = Mathf.Max(1, Mathf.RoundToInt(width * S));
            long key = ((long)d << 16) | (uint)w;
            if (!rings.TryGet(key, out var tex))
            {
                tex = new Texture2D(d, d, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[d * d];
                float r = d * 0.5f;
                for (int y = 0; y < d; y++)
                for (int x = 0; x < d; x++)
                {
                    float dist = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r));
                    float a = Mathf.Clamp01(r - dist + 0.5f) * Mathf.Clamp01(dist - (r - w) + 0.5f);
                    px[y * d + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
                tex.SetPixels32(px);
                tex.Apply();
                rings.Put(key, tex);
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(new Rect(center.x - radius, center.y - radius, radius * 2f, radius * 2f)), tex);
            GUI.color = old;
        }

        // ---------------- 선

        static Texture2D lineTex;

        /// <summary>선 띠: 세로 32칸, 위아래 끝 몇 칸만 옅어짐(늘여도 가장자리만 부드러움).</summary>
        static Texture2D LineTexture
        {
            get
            {
                if (lineTex != null) return lineTex;
                const int h = 32;
                lineTex = new Texture2D(1, h, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[h];
                for (int y = 0; y < h; y++)
                {
                    float edge = Mathf.Min(y + 0.5f, h - y - 0.5f); // 끝에서 몇 칸
                    px[y] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(edge / 2.5f) * 255f));
                }
                lineTex.SetPixels32(px);
                lineTex.Apply(false);
                return lineTex;
            }
        }

        static readonly Dictionary<int, Texture2D> softLines = new();

        /// <summary>굵기 w(화소) 선의 세로 띠: 높이 w + 1 화소, 화소당 8행, 행 알파 = 가장자리 덮임 비율. 1/4 화소 단위로 캐시.</summary>
        static Texture2D SoftLineTexture(float w)
        {
            int key = Mathf.Clamp(Mathf.RoundToInt(w * 4f), 8, 256);
            if (softLines.TryGetValue(key, out var tex) && tex != null) return tex;
            float width = key / 4f, total = width + 1f;
            int rows = Mathf.Clamp(Mathf.CeilToInt(total * 8f), 8, 512);
            tex = new Texture2D(1, rows, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
            var px = new Color32[rows];
            for (int y = 0; y < rows; y++)
            {
                float d = Mathf.Abs((y + 0.5f) / rows * total - total * 0.5f);
                px[y] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(0.5f + width * 0.5f - d) * 255f));
            }
            tex.SetPixels32(px);
            tex.Apply(false);
            softLines[key] = tex;
            return tex;
        }

        public static void Line(Vector2 a, Vector2 b, float width, Color c)
        {
            if (!Repaint || c.a <= 0f || Off(Rect.MinMaxRect(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y)), width + 2f, width + 2f)) return;
            Vector2 sa = ToScreen(a), sb = ToScreen(b);
            Vector2 d = sb - sa;
            float len = d.magnitude;
            if (len < 0.01f) return;
            float w = Mathf.Max(1f, width * S);
            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
            Matrix4x4 old = GUI.matrix;
            Color oldColor = GUI.color;
            GUI.color = Op(c);
            GUIUtility.RotateAroundPivot(angle, sa);
            // 가장자리를 부드럽게(위아래 1화소 남짓 옅어지는 띠 그림) — 꺾인 선 · 지도 서킷이 계단처럼 보이지 않게.
            // 아주 가는 선(2화소 미만)은 옅어지면 흐려 보여 그대로 흰 사각.
            // 굵기(1/4 화소 단위)마다 가장자리 1화소가 정확히 옅어지는 띠 그림 — 사각형 가장자리는 화소 가운데 표본만 잡아 계단이 되므로
            // 그림 자체가 가장자리 덮임 비율(0.5 + 반폭 − 중심 거리)을 담음. 아주 가는 선(2화소 미만)은 그대로 흰 사각.
            if (w >= 2f) GUI.DrawTexture(new Rect(sa.x, sa.y - (w + 1f) * 0.5f, len, w + 1f), SoftLineTexture(w));
            else GUI.DrawTexture(new Rect(sa.x, sa.y - w * 0.5f, len, w), Texture2D.whiteTexture);
            GUI.matrix = old;
            GUI.color = oldColor;
        }

        /// <summary>굵은 선은 끝을 둥글게(stroke-linecap: round).</summary>
        public static void RoundLine(Vector2 a, Vector2 b, float width, Color c)
        {
            Line(a, b, width, c);
            if (width * S >= 3f)
            {
                Dot(a, width * 0.5f, c);
                Dot(b, width * 0.5f, c);
            }
        }

        /// <summary>
        /// 꺾은선. 완만한 이음새(30° 미만)는 앞뒤 선을 반폭 × tan(꺾인 각/2)만큼 늘여 바깥 쐐기를 메우고, 둥근 점은 크게 꺾인 곳 · 열린 끝에만 —
        /// 점마다 원을 찍으면 원과 선 띠의 굵기가 1화소 남짓 달라 촘촘한 곡선 가장자리가 울퉁불퉁해짐.
        /// </summary>
        public static void Polyline(List<Vector2> pts, float width, Color c, bool closed = false, bool roundJoins = true)
        {
            if (!Repaint || pts == null || pts.Count < 2) return;
            int count = pts.Count, n = closed ? count : count - 1;
            bool round = roundJoins && width * S >= 3f;
            for (int i = 0; i < n; i++)
            {
                Vector2 a = pts[i], b = pts[(i + 1) % count];
                Vector2 d = b - a;
                float len = d.magnitude;
                if (len < 0.01f) continue;
                d /= len;
                Line(a - d * JoinExtend(pts, i, closed, width), b + d * JoinExtend(pts, (i + 1) % count, closed, width), width, c);
            }
            if (!round) return;
            for (int i = 0; i < count; i++)
            {
                bool end = !closed && (i == 0 || i == count - 1);
                if (end || JoinAngle(pts, i, closed) >= 30f) Dot(pts[i], width * 0.5f, c);
            }
        }

        /// <summary>부드러운 꺾은선(차트): 점 사이마다 가운데 x 를 조절점으로 쓰는 3차 베지어 (mid x, 앞 y) · (mid x, 뒤 y) — 시안 SVG 'C' 와 같음, 점은 그대로 지남.</summary>
        public static void Curve(List<Vector2> pts, float width, Color c)
        {
            if (!Repaint || pts == null || pts.Count < 2) return;
            var line = new List<Vector2> { pts[0] };
            for (int i = 1; i < pts.Count; i++)
            {
                Vector2 a = pts[i - 1], b = pts[i];
                float m = (a.x + b.x) * 0.5f;
                Cubic(line, a, new Vector2(m, a.y), new Vector2(m, b.y), b);
            }
            Polyline(line, width, c);
        }

        /// <summary>점 i에서 꺾인 각(도, 0 = 곧게). 열린 끝 · 겹친 점이면 0.</summary>
        static float JoinAngle(List<Vector2> pts, int i, bool closed)
        {
            int count = pts.Count;
            if (!closed && (i == 0 || i == count - 1)) return 0f;
            Vector2 a = pts[(i - 1 + count) % count], p = pts[i], b = pts[(i + 1) % count];
            Vector2 d0 = p - a, d1 = b - p;
            if (d0.sqrMagnitude < 0.0001f || d1.sqrMagnitude < 0.0001f) return 0f;
            return Vector2.Angle(d0, d1);
        }

        /// <summary>이음새 i에서 선을 늘일 길이: 완만하면 반폭 × tan(각/2)(바깥 쐐기 메움), 크게 꺾이면 0(둥근 점이 메움).</summary>
        static float JoinExtend(List<Vector2> pts, int i, bool closed, float width)
        {
            // 열린 끝은 그대로, 이음새는 늘 1화소 남짓 겹침 — 맞닿기만 하면 양쪽 가장자리가 반투명으로 겹쳐 틈으로 바탕이 비침
            // (인트로 서킷 선에 흰 눈금처럼 보였음)
            if (!closed && (i == 0 || i == pts.Count - 1)) return 0f;
            float seam = 1f / Mathf.Max(0.5f, S);
            float angle = JoinAngle(pts, i, closed);
            if (angle <= 0f || angle >= 30f) return seam;
            return seam + width * 0.5f * Mathf.Tan(angle * 0.5f * Mathf.Deg2Rad);
        }

        public static void Dashed(Vector2 a, Vector2 b, float width, Color c, float dash, float gap)
        {
            float len = Vector2.Distance(a, b);
            if (len < 0.01f) return;
            Vector2 dir = (b - a) / len;
            for (float t = 0f; t < len; t += dash + gap)
                Line(a + dir * t, a + dir * Mathf.Min(len, t + dash), width, c);
        }

        // ---------------- 그림

        /// <summary>
        /// 그림 그리기. 읽을 수 있는 Texture2D(로고 · 이미지)를 그릴 크기보다 큰 원본에서 줄여 그릴 때는 늘 미리 줄인 선명한 그림(ImageSharp)으로 그림 —
        /// GUI.DrawTexture 의 바로 줄이기는 밉맵 없이 건너뛰어 계단 · 깨짐이 생김. 영상(RenderTexture) · 자르기 · 확대는 그대로.
        /// </summary>
        public static void Image(Rect r, Texture tex, float alpha = 1f, ScaleMode mode = ScaleMode.ScaleToFit)
        {
            if (!Repaint || tex == null || Off(r, 1f, 1f)) return;
            if (mode == ScaleMode.ScaleToFit && tex is Texture2D t2 && !sharpFailed.Contains(t2.GetInstanceID()))
            {
                Rect sr = ToScreen(r);
                if (sr.width < t2.width || sr.height < t2.height) { ImageSharp(r, t2, alpha); return; }
            }
            ImageRaw(r, tex, alpha, mode);
        }

        static void ImageRaw(Rect r, Texture tex, float alpha, ScaleMode mode)
        {
            Color old = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, alpha * old.a * Opacity);
            GUI.DrawTexture(ToScreen(r), tex, mode, true);
            GUI.color = old;
        }

        /// <summary>미리 줄이지 못하는 그림(읽기 불가 등) — 다시 시도하지 않고 바로 그림.</summary>
        static readonly HashSet<int> sharpFailed = new();

        public static void Image(Rect r, Texture tex, Color tint, ScaleMode mode = ScaleMode.ScaleToFit)
        {
            if (!Repaint || tex == null || Off(r, 1f, 1f)) return;
            Color old = GUI.color;
            GUI.color = Op(tint);
            GUI.DrawTexture(ToScreen(r), tex, mode, true);
            GUI.color = old;
        }

    }
}
