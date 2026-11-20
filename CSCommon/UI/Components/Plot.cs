using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Plot — 점 배치도(시안 3.0 '고른 경기장' 위 구역 그림, 실제 지도 아님): Tile 면 · 높이 = 폭 × Aspect,
    /// 구역 범위 = Look.HeroFill 면 + Look.PastelStrong 선 1.5, 점 10(Section.Plate 테두리 2) — 보통 Look.PillSub · 고름 Look.PastelInk · 뺌 Look.PillLine,
    /// 지금 고른 점 16(Look.PastelStrong · 바깥 Look.PastelInk 고리 2) + 위 이름 알약 24(Section.Plate · Section.PlateLine).
    /// 게임 좌표(x, z — 북쪽 z 큼)를 같은 축척(가로세로 비율 유지)으로 칸 가운데에 놓음, 범위가 MinSpan 보다 작으면 MinSpan 으로.
    /// </summary>
    public static class Plot
    {
        public const float Aspect = 0.35f, Inset = 20f, Dot = 10f, DotOn = 16f, Halo = 2f, Ring = 2f, ZoneLine = 1.5f, ZonePad = 10f;
        public const float LabelH = 24f, LabelPadX = 10f, LabelGap = 6f, Hit = 20f, ActionPad = 8f, MinSpan = 200f;

        public enum Kind { Normal, Picked, Off }

        /// <summary>게임 좌표 → r 안 자리(bounds = 축척 기준 범위, 게임 x · z).</summary>
        public static Vector2 Place(Rect r, Rect bounds, Vector2 world)
        {
            float w = Mathf.Max(MinSpan, bounds.width), h = Mathf.Max(MinSpan, bounds.height);
            float s = Mathf.Min(Mathf.Max(0f, r.width - Inset * 2f) / w, Mathf.Max(0f, r.height - Inset * 2f) / h);
            var c = bounds.center;
            return new Vector2(r.center.x + (world.x - c.x) * s, r.center.y - (world.y - c.y) * s);
        }

        /// <summary>배치도를 그리고 누른 점 번호(없으면 -1). zone.width ≤ 0 이면 구역 범위 면을 안 그림. 부르는 쪽 단추가 있으면 이것보다 먼저 부를 것.</summary>
        public static int In(Rect r, Rect bounds, Rect zone, IList<Vector2> world, IList<Kind> kinds, int cur, string curLabel)
        {
            if (Draw.Repaint)
            {
                Tile.Frame(r, false, true, false);
                if (zone.width > 0f)
                {
                    Vector2 a = Place(r, bounds, new Vector2(zone.xMin, zone.yMax)), b = Place(r, bounds, new Vector2(zone.xMax, zone.yMin));
                    var z = Rect.MinMaxRect(a.x - ZonePad, a.y - ZonePad, b.x + ZonePad, b.y + ZonePad);
                    float rad = Mathf.Min(Tile.Radius, Mathf.Min(z.width, z.height) * 0.5f);
                    Draw.Round(z, Look.HeroFill, rad);
                    Draw.RoundOutline(z, Look.PastelStrong, rad, ZoneLine);
                }
                for (int i = 0; i < world.Count; i++)
                {
                    if (i == cur) continue;
                    var p = Place(r, bounds, world[i]);
                    Draw.Dot(p, Dot * 0.5f + Halo, Section.Plate);
                    Draw.Dot(p, Dot * 0.5f, kinds[i] == Kind.Off ? Look.PillLine : kinds[i] == Kind.Picked ? Look.PastelInk : Look.PillSub);
                }
                if (cur >= 0 && cur < world.Count)
                {
                    var p = Place(r, bounds, world[cur]);
                    Draw.Dot(p, DotOn * 0.5f + Halo + Ring, Look.PastelInk);
                    Draw.Dot(p, DotOn * 0.5f + Halo, Section.Plate);
                    Draw.Dot(p, DotOn * 0.5f, Look.PastelStrong);
                    if (!string.IsNullOrEmpty(curLabel))
                    {
                        float tw = Mathf.Ceil(Draw.TextWidth(curLabel, CSCommonDesign.FontMin, true)), lw = Mathf.Min(r.width - ActionPad * 2f, tw + LabelPadX * 2f);
                        float ly = p.y - DotOn * 0.5f - Halo - Ring - LabelGap - LabelH;
                        if (ly < r.y + ActionPad) ly = p.y + DotOn * 0.5f + Halo + Ring + LabelGap;
                        var l = new Rect(Mathf.Clamp(Mathf.Round(p.x - lw * 0.5f), r.x + ActionPad, r.xMax - ActionPad - lw), Mathf.Round(ly), lw, LabelH);
                        Draw.Round(l, Section.Plate, LabelH * 0.5f);
                        Draw.RoundOutline(l, Section.PlateLine, LabelH * 0.5f);
                        Draw.TextFit(new Rect(l.x + LabelPadX, l.y, l.width - LabelPadX * 2f, l.height), curLabel, CSCommonDesign.FontMin, Look.BarInk, true, TextAnchor.MiddleCenter);
                    }
                }
            }
            int hit = -1;
            for (int i = 0; i < world.Count; i++)
            {
                var p = Place(r, bounds, world[i]);
                if (Draw.Button(new Rect(p.x - Hit * 0.5f, p.y - Hit * 0.5f, Hit, Hit))) hit = i;
            }
            return hit;
        }

        /// <summary>자가 검사: 범위 네 모서리가 칸 안쪽(Inset)에 들고 가로세로 축척이 같음 · 작은 범위는 MinSpan.</summary>
        public static void SelfCheck(Action<bool, string> check)
        {
            static bool Near(float a, float b) => Mathf.Abs(a - b) < 0.01f;
            var r = new Rect(10f, 20f, 400f, 140f);
            var bounds = Rect.MinMaxRect(-1000f, -300f, 1000f, 500f);
            Vector2 lt = Place(r, bounds, new Vector2(bounds.xMin, bounds.yMax)), rb = Place(r, bounds, new Vector2(bounds.xMax, bounds.yMin));
            check(lt.x >= r.x + Inset - 0.01f && lt.y >= r.y + Inset - 0.01f && rb.x <= r.xMax - Inset + 0.01f && rb.y <= r.yMax - Inset + 0.01f, "plot inside inset");
            check(Near((rb.x - lt.x) / bounds.width, (rb.y - lt.y) / bounds.height), "plot same scale");
            check(Near(Place(r, bounds, bounds.center).x, r.center.x) && Near(Place(r, bounds, bounds.center).y, r.center.y), "plot centered");
            var tiny = new Rect(0f, 0f, 10f, 10f);
            check(Mathf.Abs(Place(r, tiny, new Vector2(10f, 0f)).x - Place(r, tiny, Vector2.zero).x) <= (r.height - Inset * 2f) * 10f / MinSpan + 0.01f, "plot min span");
        }
    }
}
