using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- SVG 경로(탭 아이콘 · 설계 그림) — M L H V C S Q T A Z 지원, 결과는 꺾은선 목록

        static readonly Dictionary<string, List<List<Vector2>>> pathCache = new();

        static void Cubic(List<Vector2> into, Vector2 p, Vector2 c1, Vector2 c2, Vector2 e)
        {
            if (into == null) return;
            for (int k = 1; k <= CSCommonDesign.CurveSteps; k++)
            {
                float t = k / (float)CSCommonDesign.CurveSteps, u = 1f - t;
                into.Add(u * u * u * p + 3f * u * u * t * c1 + 3f * u * t * t * c2 + t * t * t * e);
            }
        }

        static void Quad(List<Vector2> into, Vector2 p, Vector2 c1, Vector2 e)
        {
            if (into == null) return;
            for (int k = 1; k <= 10; k++)
            {
                float t = k / 10f, u = 1f - t;
                into.Add(u * u * p + 2f * u * t * c1 + t * t * e);
            }
        }

        public static List<List<Vector2>> ParsePath(string d)
        {
            if (pathCache.TryGetValue(d, out var cached)) return cached;
            var result = new List<List<Vector2>>();
            List<Vector2> cur = null;
            Vector2 p = Vector2.zero, start = Vector2.zero;
            // S · T가 반사할 앞 곡선의 마지막 제어점(앞 명령이 C/S · Q/T일 때만)
            Vector2 lastCubic = Vector2.zero, lastQuad = Vector2.zero;
            char prev = ' ';
            int i = 0;
            char cmd = 'M';
            while (i < d.Length)
            {
                SkipSpace(d, ref i);
                if (i >= d.Length) break;
                if (char.IsLetter(d[i])) cmd = d[i++];
                bool rel = char.IsLower(cmd);
                char upper = char.ToUpperInvariant(cmd);
                char last = prev;
                prev = upper;
                switch (upper)
                {
                    case 'M':
                    {
                        var q = new Vector2(Num(d, ref i), Num(d, ref i));
                        p = rel ? p + q : q;
                        start = p;
                        cur = new List<Vector2> { p };
                        result.Add(cur);
                        cmd = rel ? 'l' : 'L';
                        break;
                    }
                    case 'L':
                    {
                        var q = new Vector2(Num(d, ref i), Num(d, ref i));
                        p = rel ? p + q : q;
                        cur?.Add(p);
                        break;
                    }
                    case 'H':
                    {
                        float x = Num(d, ref i);
                        p = new Vector2(rel ? p.x + x : x, p.y);
                        cur?.Add(p);
                        break;
                    }
                    case 'V':
                    {
                        float y = Num(d, ref i);
                        p = new Vector2(p.x, rel ? p.y + y : y);
                        cur?.Add(p);
                        break;
                    }
                    case 'C':
                    {
                        Vector2 c1 = new(Num(d, ref i), Num(d, ref i)), c2 = new(Num(d, ref i), Num(d, ref i)), e = new(Num(d, ref i), Num(d, ref i));
                        if (rel) { c1 += p; c2 += p; e += p; }
                        Cubic(cur, p, c1, c2, e);
                        lastCubic = c2;
                        p = e;
                        break;
                    }
                    case 'S':
                    {
                        // 첫 제어점 = 앞 C/S의 둘째 제어점을 지금 점에 대해 반사(앞이 곡선이 아니면 지금 점)
                        Vector2 c2 = new(Num(d, ref i), Num(d, ref i)), e = new(Num(d, ref i), Num(d, ref i));
                        if (rel) { c2 += p; e += p; }
                        Vector2 c1 = last is 'C' or 'S' ? 2f * p - lastCubic : p;
                        Cubic(cur, p, c1, c2, e);
                        lastCubic = c2;
                        p = e;
                        break;
                    }
                    case 'Q':
                    {
                        Vector2 c1 = new(Num(d, ref i), Num(d, ref i)), e = new(Num(d, ref i), Num(d, ref i));
                        if (rel) { c1 += p; e += p; }
                        Quad(cur, p, c1, e);
                        lastQuad = c1;
                        p = e;
                        break;
                    }
                    case 'T':
                    {
                        var e = new Vector2(Num(d, ref i), Num(d, ref i));
                        if (rel) e += p;
                        Vector2 c1 = last is 'Q' or 'T' ? 2f * p - lastQuad : p;
                        Quad(cur, p, c1, e);
                        lastQuad = c1;
                        p = e;
                        break;
                    }
                    case 'A':
                    {
                        float rx = Num(d, ref i), ry = Num(d, ref i), rot = Num(d, ref i);
                        bool large = Num(d, ref i) != 0f, sweep = Num(d, ref i) != 0f;
                        var e = new Vector2(Num(d, ref i), Num(d, ref i));
                        if (rel) e += p;
                        Arc(cur, p, e, rx, ry, rot, large, sweep);
                        p = e;
                        break;
                    }
                    case 'Z':
                        p = start;
                        cur?.Add(start);
                        break;
                    default:
                        i++;
                        break;
                }
            }
            pathCache[d] = result;
            return result;
        }

        static void Arc(List<Vector2> into, Vector2 p0, Vector2 p1, float rx, float ry, float rotDeg, bool large, bool sweep)
        {
            if (into == null) return;
            if (rx == 0f || ry == 0f) { into.Add(p1); return; }
            float phi = rotDeg * Mathf.Deg2Rad, cos = Mathf.Cos(phi), sin = Mathf.Sin(phi);
            Vector2 h = (p0 - p1) * 0.5f;
            float x1 = cos * h.x + sin * h.y, y1 = -sin * h.x + cos * h.y;
            rx = Mathf.Abs(rx); ry = Mathf.Abs(ry);
            float lambda = x1 * x1 / (rx * rx) + y1 * y1 / (ry * ry);
            if (lambda > 1f) { float s = Mathf.Sqrt(lambda); rx *= s; ry *= s; }
            float num = rx * rx * ry * ry - rx * rx * y1 * y1 - ry * ry * x1 * x1;
            float den = rx * rx * y1 * y1 + ry * ry * x1 * x1;
            float coef = (large == sweep ? -1f : 1f) * Mathf.Sqrt(Mathf.Max(0f, num / den));
            float cx1 = coef * rx * y1 / ry, cy1 = -coef * ry * x1 / rx;
            Vector2 mid = (p0 + p1) * 0.5f;
            float cx = cos * cx1 - sin * cy1 + mid.x, cy = sin * cx1 + cos * cy1 + mid.y;
            float a1 = Mathf.Atan2((y1 - cy1) / ry, (x1 - cx1) / rx);
            float a2 = Mathf.Atan2((-y1 - cy1) / ry, (-x1 - cx1) / rx);
            float da = a2 - a1;
            if (sweep && da < 0f) da += 2f * Mathf.PI;
            if (!sweep && da > 0f) da -= 2f * Mathf.PI;
            int steps = Mathf.Max(6, Mathf.CeilToInt(Mathf.Abs(da) / (Mathf.PI / 12f)));
            for (int k = 1; k <= steps; k++)
            {
                float a = a1 + da * k / steps;
                float ex = rx * Mathf.Cos(a), ey = ry * Mathf.Sin(a);
                into.Add(new Vector2(cos * ex - sin * ey + cx, sin * ex + cos * ey + cy));
            }
        }

        static void SkipSpace(string d, ref int i)
        {
            while (i < d.Length && (d[i] == ' ' || d[i] == ',' || d[i] == '\n' || d[i] == '\t')) i++;
        }

        static float Num(string d, ref int i)
        {
            SkipSpace(d, ref i);
            int start = i;
            if (i < d.Length && (d[i] == '-' || d[i] == '+')) i++;
            bool dot = false;
            while (i < d.Length && (char.IsDigit(d[i]) || (d[i] == '.' && !dot)))
            {
                if (d[i] == '.') dot = true;
                i++;
            }
            if (i < d.Length && (d[i] == 'e' || d[i] == 'E'))
            {
                i++;
                if (i < d.Length && (d[i] == '-' || d[i] == '+')) i++;
                while (i < d.Length && char.IsDigit(d[i])) i++;
            }
            if (start == i) { i++; return 0f; }
            return float.Parse(d.Substring(start, i - start), NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        /// <summary>SVG 경로를 origin에서 scale 배로 선 그리기.</summary>
        public static void Path(string d, Vector2 origin, float scale, float width, Color c)
        {
            if (!Repaint) return;
            foreach (var poly in ParsePath(d))
            {
                for (int k = 0; k + 1 < poly.Count; k++)
                    Line(origin + poly[k] * scale, origin + poly[k + 1] * scale, width, c);
                if (width * S >= 2.5f)
                    foreach (var q in poly) Dot(origin + q * scale, width * 0.5f, c);
            }
        }

    }
}
