using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 구글 머티리얼 서클 인디케이터(Spinner)

        // 크기(칸 · 반지름 · 굵기 화소)별 텍스처 + 고리 띠 화소(가운데에서 x · y · 거리 · 각도 미리 셈). 띠 밖 화소는 늘 투명이라 다시 칠하지 않음.
        sealed class SpinnerTex
        {
            public Texture2D Tex;
            public Color32[] Px;
            public int[] Ring;
            public float[] Dx, Dy, Dist, Ang;
            public int Frame = -1;
        }

        static readonly Dictionary<long, SpinnerTex> spinners = new();

        /// <summary>
        /// 구글 머티리얼 서클 인디케이터(Google Circular Progress Indicator):
        /// 1.333초 주기로 호의 머리(head)가 먼저 270° 치고 나가며 늘어나고(16° → 286°), 뒤이어 꼬리(tail)가 따라붙으며 줄어듦(286° → 16°).
        /// 동시에 전체 고리가 끊임없이 회전하며, 양 끝은 둥근 마감(stroke-linecap: round).
        /// 크기별 재사용 텍스처(고리 띠 화소만 프레임마다 다시 칠함)라 매 프레임 새 Texture2D · GC 할당이 없고, GUI.matrix 회전을 쓰지 않아 BeginClip 안에서도 잘리지 않음.
        /// </summary>
        public static void Spinner(Vector2 center, float radius, float width, Color c)
        {
            if (!Repaint || c.a <= 0f || radius <= 0f || width <= 0f) return;

            const float Cycle = 1.333f;
            float t = Time.unscaledTime;
            float tCycle = t / Cycle;
            int cycleIdx = Mathf.FloorToInt(tCycle);
            float u = tCycle - cycleIdx;

            static float EaseInOut(float x)
            {
                x = Mathf.Clamp01(x);
                return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) * 0.5f;
            }

            float head = u < 0.5f ? EaseInOut(u * 2f) * 270f : 270f;
            float tail = u < 0.5f ? 0f : EaseInOut((u - 0.5f) * 2f) * 270f;
            float baseDeg = -90f + cycleIdx * 270f + t * 156f;
            float from = baseDeg + tail;
            float to = baseDeg + head + 16f;

            float rPx = Mathf.Max(2f, radius * S);
            float half = Mathf.Max(0.75f, width * S * 0.5f);
            int d = Mathf.Clamp(Mathf.CeilToInt((rPx + half + 2f) * 2f), 8, 256);

            // 반지름 · 굵기는 0.1 화소 단위로 같은 칸(예전: 0.1 넘게 바뀌면 다시 칠함)
            rPx = Mathf.Round(rPx * 10f) / 10f;
            half = Mathf.Round(half * 10f) / 10f;
            long key = ((long)d << 40) | ((long)Mathf.RoundToInt(rPx * 10f) << 20) | (uint)Mathf.RoundToInt(half * 10f);
            if (!spinners.TryGetValue(key, out var sp))
            {
                sp = new SpinnerTex
                {
                    Tex = new Texture2D(d, d, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave },
                    Px = new Color32[d * d],
                };
                // 고리 띠: |거리 − 반지름| < 굵기 반 + 0.5 인 화소만 알파가 0 보다 클 수 있음(끝 둥근 마감도 고리 위 점까지 거리라 이보다 멂)
                var ring = new List<int>();
                float cMid0 = d * 0.5f;
                for (int y = 0; y < d; y++)
                for (int x = 0; x < d; x++)
                {
                    float dx = x + 0.5f - cMid0, dy = d - y - 0.5f - cMid0;
                    if (Mathf.Abs(Mathf.Sqrt(dx * dx + dy * dy) - rPx) < half + 0.5f) ring.Add(y * d + x);
                }
                int n = ring.Count;
                sp.Ring = ring.ToArray();
                sp.Dx = new float[n];
                sp.Dy = new float[n];
                sp.Dist = new float[n];
                sp.Ang = new float[n];
                for (int k = 0; k < n; k++)
                {
                    int x = sp.Ring[k] % d, y = sp.Ring[k] / d;
                    float dx = x + 0.5f - cMid0, dy = d - y - 0.5f - cMid0;
                    sp.Dx[k] = dx;
                    sp.Dy[k] = dy;
                    sp.Dist[k] = Mathf.Sqrt(dx * dx + dy * dy);
                    sp.Ang[k] = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
                }
                spinners[key] = sp;
            }

            if (sp.Frame != Time.frameCount)
            {
                sp.Frame = Time.frameCount;
                float mid = (from + to) * 0.5f;
                float halfSpan = (to - from) * 0.5f;
                float rad0 = from * Mathf.Deg2Rad, rad1 = to * Mathf.Deg2Rad;
                float x0 = Mathf.Cos(rad0) * rPx, y0 = Mathf.Sin(rad0) * rPx;
                float x1 = Mathf.Cos(rad1) * rPx, y1 = Mathf.Sin(rad1) * rPx;
                var px = sp.Px;
                for (int k = 0; k < sp.Ring.Length; k++)
                {
                    float dx = sp.Dx[k], dy = sp.Dy[k];
                    float distToArc;
                    if (Mathf.Abs(Mathf.DeltaAngle(mid, sp.Ang[k])) <= halfSpan)
                    {
                        distToArc = Mathf.Abs(sp.Dist[k] - rPx);
                    }
                    else
                    {
                        float d0 = (dx - x0) * (dx - x0) + (dy - y0) * (dy - y0);
                        float d1 = (dx - x1) * (dx - x1) + (dy - y1) * (dy - y1);
                        distToArc = Mathf.Sqrt(d0 < d1 ? d0 : d1);
                    }
                    float a = Mathf.Clamp01(half + 0.5f - distToArc);
                    px[sp.Ring[k]] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
                sp.Tex.SetPixels32(px);
                sp.Tex.Apply(false);
            }

            Vector2 sc = ToScreen(center);
            float halfD = d * 0.5f;
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(new Rect(Mathf.Round(sc.x - halfD), Mathf.Round(sc.y - halfD), d, d), sp.Tex);
            GUI.color = old;
        }

    }
}
