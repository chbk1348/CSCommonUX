using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>3.5 부품 순수 함수 자가 검사(게임 밖 하니스 · 모드 진단에서 부름). check(통과, 이름).</summary>
    internal static class Check35
    {
        public static void SelfCheck(Action<bool, string> check)
        {
            static bool Near(float a, float b) => Mathf.Abs(a - b) < 0.01f;
            static bool NearV(Vector2 a, Vector2 b) => Near(a.x, b.x) && Near(a.y, b.y);
            int steps = CSCommonDesign.CurveSteps;

            // 곡선: 모든 점을 지남 · Catmull-Rom 조절점(/6) · 끝 밖 점은 끝점 · 조절점 y 는 영역 안
            var pts = new List<Vector2> { new(0f, 0f), new(10f, 10f), new(20f, 0f), new(30f, 10f) };
            var line = new List<Vector2>();
            Draw.SmoothInto(line, pts, -100f, 100f);
            check(line.Count == 1 + 3 * steps && NearV(line[steps], pts[1]) && NearV(line[2 * steps], pts[2]) && NearV(line[line.Count - 1], pts[3]), "smooth passes points");
            Draw.CatmullControls(pts, 1, -100f, 100f, out var c1, out var c2);
            check(NearV(c1, new Vector2(10f + 20f / 6f, 10f)) && NearV(c2, new Vector2(20f - 20f / 6f, 0f)), "smooth catmull controls");
            Draw.CatmullControls(pts, 0, -100f, 100f, out c1, out _);
            check(NearV(c1, new Vector2(10f / 6f, 10f / 6f)), "smooth end uses itself");
            var spike = new List<Vector2> { new(0f, 0f), new(10f, 10f), new(20f, 10f), new(30f, 0f) };
            Draw.CatmullControls(spike, 1, 0f, 10f, out c1, out c2);
            check(c1.y <= 10f && c2.y <= 10f && Near(c1.y, 10f), "smooth controls clamped");

            // 차트 축 · 막대
            var plot = new Rect(0f, 10f, 100f, 200f);
            check(Near(Chart35.Y(plot, 0f, 0f, 12f), 10f) && Near(Chart35.Y(plot, 6f, 0f, 12f), 110f) && Near(Chart35.Y(plot, 99f, 0f, 12f), 210f), "chart y map");
            check(Near(Chart35.TickY(plot, 4, 0), 10f) && Near(Chart35.TickY(plot, 4, 3), 210f), "chart ticks span plot");
            var bar = Chart35.Bar(plot, 10f, 8f, 50f, 100f);
            check(Near(bar.yMax, plot.yMax) && Near(bar.height, 100f), "chart bar from floor");
            check(Near(Chart35.Bar(plot, 0f, 8f, 150f, 100f).height, 200f) && Near(Chart35.Bar(plot, 0f, 8f, -5f, 100f).height, 0f), "chart bar clamped");
            check(Chart35.Gray(0) == Pal.ChartGray1 && Chart35.Gray(3) == Pal.ChartGray4 && Chart35.Gray(9) == Pal.ChartGray4, "chart gray steps");
            var area = Chart35.Plot(new Rect(0f, 0f, 700f, 236f));
            check(Near(area.x, 44f) && Near(area.y, 16f) && Near(area.yMax, 208f) && Near(area.xMax, 612f), "chart plot area");

            // 숫자 자리 폭: 1 이 좁은 글꼴에서도 같은 꼴 글의 끝이 같음
            var w = new float[] { 10f, 6f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f };
            float cell = Draw.DigitCell(w, out bool same);
            check(Near(cell, 10f) && !same, "num cell widest digit");
            float Total(string t)
            {
                float x = 0f;
                foreach (char ch in t) x += Draw.Advance(ch, cell, ch >= '0' && ch <= '9' ? w[ch - '0'] : 3f);
                return x;
            }
            check(Near(Total("1:11.111"), Total("0:00.000")) && Near(Total("+1.1"), Total("+9.0")), "num tabular advance");
            Draw.DigitCell(new float[] { 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f }, out same);
            check(same, "num cell same font");

            // 점수 · 단계 막대 · KPI
            ScoreBar.Split(30f, 20f, 100f, 200f, out float nowW, out float addW);
            check(Near(nowW, 60f) && Near(addW, 40f), "score split");
            ScoreBar.Split(90f, 50f, 100f, 200f, out nowW, out addW);
            check(Near(nowW + addW, 200f), "score clipped at end");
            ScoreBar.Split(10f, -5f, 0f, 200f, out nowW, out addW);
            check(nowW == 0f && addW == 0f, "score empty max");
            var track = new Rect(0f, 0f, 168f, 8f);
            check(Near(StageTrack.Cell(track, 3, 2).xMax, 168f) && Near(StageTrack.Cell(track, 3, 1).x - StageTrack.Cell(track, 3, 0).xMax, 4f), "track cells gap 4");
            var row = new Rect(0f, 0f, 1000f, 108f);
            check(Near(Kpi.Col(row, 3).xMax, 1000f) && Near(Kpi.Col(row, 1).x - Kpi.Col(row, 0).xMax, 16f) && Near(Kpi.H, 108f), "kpi columns");

            // 상태 점 · 숫자 알약
            check(Near(StatusDot.LiveAlpha(0f), 1f) && Near(StatusDot.LiveAlpha(1.2f), 0.4f) && Near(StatusDot.LiveAlpha(2.4f), 1f) && StatusDot.LiveAlpha(0.6f) < 1f && StatusDot.LiveAlpha(0.6f) > 0.4f, "dot live blink");
            check(NearV(StatusDot.CornerCenter(new Rect(0f, 0f, 48f, 48f), false), new Vector2(38f, 10f)) && NearV(StatusDot.CornerCenter(new Rect(0f, 0f, 64f, 32f), true), new Vector2(54f, 8f)), "dot corner inset");
            check(StatusDot.CountText(5) == "5" && StatusDot.CountText(99) == "99" && StatusDot.CountText(100) == "99+" && StatusDot.CountText(0) == null, "count text");

            // 곡선 Ease · 아이콘 단계 · 빈 상태 · 판
            bool rising = true;
            for (int i = 1; i <= 20; i++) rising &= CSCommonDesign35.Ease(i / 20f) >= CSCommonDesign35.Ease((i - 1) / 20f);
            check(Near(CSCommonDesign35.Ease(0f), 0f) && Near(CSCommonDesign35.Ease(1f), 1f) && CSCommonDesign35.Ease(0.5f) > 0.8f && rising, "ease out curve");
            check(Icons.Stage(12f) == 12f && Icons.Stage(16f) == 16f && Icons.Stage(20f) == 18f && Icons.Stage(24f) == 24f && Icons.Stage(13.9f) == 12f, "icon stage");
            var seen = new HashSet<string>();
            int consts = 0;
            foreach (var f in typeof(Icons).GetFields(BindingFlags.Public | BindingFlags.Static))
                if (f.IsLiteral && f.FieldType == typeof(string)) { consts++; seen.Add((string)f.GetValue(null)); }
            check(consts == 59 && seen.Count == 59, "icons 59 unique");
            check(Near(EmptyState.Height(false, false), 72f) && Near(EmptyState.Height(true, true), 152f), "empty height");
            check(Near(Surface.Height(Surface.Kind.Card, 100f, true, true), 240f) && Near(Surface.Inset(new Rect(0f, 0f, 560f, 300f), Surface.Kind.Modal).height, 300f), "surface height");
            check(Surface.Raise(Surface.Kind.Card) == Pal.GroundRaised && Surface.Raise(Surface.Kind.Hero) == Pal.GroundCard, "surface raise");
        }
    }
}
