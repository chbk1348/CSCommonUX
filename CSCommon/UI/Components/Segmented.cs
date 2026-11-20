using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Segmented — 여럿 중 하나(시안 'CSCommonUX.Segmented'): 홈 Look.Pill · 안쪽 3 · 칸 사이 2(홈 반지름 = 높이/2 · 최대 24, 칸 반지름 = 홈 − 3),
    /// 칸 34 · 좌우 16 · 글 13.5. 고른 칸 = 떠오른 알약(Look.SegmentOn 면 + 1px Look.BarLine) · 굵은 Look.BarInk 글, 안 고른 칸 = Look.BarOff 글 · 호버 Look.NeutralHover.
    /// 색 점 8(왼쪽 14 · 점 → 글 8) · 서브라벨(칸 50 · 최소 폭 104 · 이름 아래 3 · 12 Look.PillSub). 색 전환 0.2초 · 누르면 0.95배.
    /// </summary>
    public static class Segmented
    {
        public const float H = 34f, Pad = 3f, Gap = 2f, PadX = 16f, DotPadL = 14f, Dot = 8f, DotGap = 8f, Font = 13.5f;
        public const float SubH = 50f, SubMinW = 104f, SubFont = 12f, SubGap = 3f, NameH = 18f, SubLineH = 16f, MaxRadius = 24f, PressScale = 0.95f, FadeSeconds = 0.2f;

        static float Radius(float grooveH) => Mathf.Min(grooveH * 0.5f, MaxRadius);

        static void Groove(Rect r)
        {
            if (Draw.Repaint) Draw.Round(r, Look.Pill, Radius(r.height));
        }

        /// <summary>칸 하나(그리기 + 누름): [점 · 이름] 칸 가운데(sub 가 있으면 이름 · 작은 글 두 줄). onInk = 고른 칸 글 색(없으면 Look.BarInk). 눌러 떼면 true.</summary>
        static bool Cell(Rect c, float rad, int key, bool on, string label, Color? dot, string sub, Color? onInk, string tip)
        {
            float onT = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / FadeSeconds);
            int hk = key ^ 0x51ed270b;
            float hv = Look.Hover(hk, c, !on), pr = Look.Press(hk, c);
            if (Draw.Repaint)
            {
                Draw.BeginScale(c.center, Look.PressScale(pr, PressScale));
                float ha = hv * (1f - onT);
                if (ha > 0.01f) Draw.Round(c, Draw.Alpha(Look.NeutralHover, ha), rad);
                if (onT > 0.01f)
                {
                    Draw.Round(c, Draw.Alpha(Look.SegmentOn, onT), rad);
                    Draw.RoundOutline(c, Draw.Alpha(Look.BarLine, onT), rad);
                }
                Color ink = Color.Lerp(Look.BarOff, onInk ?? Look.BarInk, onT);
                if (string.IsNullOrEmpty(sub))
                {
                    float tw = Mathf.Ceil(Draw.TextWidth(label, Font, true)), dw = dot.HasValue ? Dot + DotGap : 0f;
                    float x = Mathf.Max(c.x + DotGap, Mathf.Round(c.center.x - (dw + tw) * 0.5f));
                    if (dot.HasValue) Draw.Dot(new Vector2(x + Dot * 0.5f, c.center.y), Dot * 0.5f, dot.Value);
                    Draw.TextPill(new Rect(x + dw, c.y, Mathf.Max(0f, Mathf.Min(tw, c.xMax - DotGap - x - dw)), c.height), label, Font, ink, on, TextAnchor.MiddleCenter);
                }
                else
                {
                    // 이름 18 + 3 + 작은 글 16 을 가운데로
                    float top = Mathf.Round(c.center.y - (NameH + SubGap + SubLineH) * 0.5f);
                    Draw.TextPill(new Rect(c.x + Pad, top, c.width - Pad * 2f, NameH), label, Font, ink, on, TextAnchor.MiddleCenter);
                    Draw.TextFit(new Rect(c.x + Pad, top + NameH + SubGap, c.width - Pad * 2f, SubLineH), sub, SubFont, Look.PillSub, false, TextAnchor.MiddleCenter);
                }
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(c, tip);
            return Draw.Button(c);
        }

        /// <summary>줄 가운데 cy · 왼쪽 x 에 글에 맞춘 칸들(draw = false 면 재기만). dots[i] 가 있으면 글 앞 색 점. 그린 폭을 돌려줌.</summary>
        public static float At(float x, float cy, string[] labels, int cur, string[] tips, Color?[] dots, bool draw, Action<int> pick)
        {
            int n = labels.Length;
            var widths = new float[n];
            float total = Pad * 2f + Gap * Mathf.Max(0, n - 1);
            for (int i = 0; i < n; i++)
            {
                // 점 칸: 왼쪽 14 + 점 8 + 8 + 글 + 오른쪽 16
                widths[i] = PadX * 2f + Mathf.Ceil(Draw.TextWidth(labels[i], Font, true)) + (dots != null && dots[i].HasValue ? Dot + DotGap - (PadX - DotPadL) : 0f);
                total += widths[i];
            }
            if (!draw || n == 0) return total;
            var frame = new Rect(x, Mathf.Round(cy - (H + Pad * 2f) * 0.5f), total, H + Pad * 2f);
            Groove(frame);
            float rad = Radius(frame.height) - Pad;
            int skey = CSCommonAnim.Key(labels[0], labels[n - 1]) ^ n * 7919 ^ Mathf.RoundToInt(cy);
            float ix = frame.x + Pad;
            for (int i = 0; i < n; i++)
            {
                var r = new Rect(ix, frame.y + Pad, widths[i], H);
                if (Cell(r, rad, skey ^ (i + 1) * 0x2f6b8d3, i == cur, labels[i], dots != null ? dots[i] : null, null, null, tips != null ? tips[i] : null)) pick?.Invoke(i);
                ix += widths[i] + Gap;
            }
            return total;
        }

        /// <summary>칸 폭을 똑같이 나눔(창 위 고르기 줄 · 에셋 자리 등). subs = 이름 아래 작은 글(칸 높이 SubH) · colors = 고른 칸 이름 색(팀 · 세션 색).</summary>
        public static void In(Rect r, string[] labels, int cur, Action<int> pick, string[] tips = null, string[] subs = null, Color?[] colors = null)
        {
            int n = labels.Length;
            if (n == 0) return;
            Groove(r);
            float rad = Radius(r.height) - Pad;
            float w = (r.width - Pad * 2f - Gap * (n - 1)) / n, h = r.height - Pad * 2f;
            int skey = CSCommonAnim.Key(labels[0], labels[n - 1]) ^ 0x3c6ef372 ^ Mathf.RoundToInt(r.y);
            for (int i = 0; i < n; i++)
            {
                var c = new Rect(r.x + Pad + i * (w + Gap), r.y + Pad, w, h);
                bool on = i == cur;
                if (Cell(c, rad, skey ^ (i + 1) * 0x2f6b8d3, on, labels[i], null, subs != null && i < subs.Length ? subs[i] : null,
                    colors != null && i < colors.Length ? colors[i] : null, tips != null ? tips[i] : null) && !on) pick?.Invoke(i);
            }
        }
    }
}
