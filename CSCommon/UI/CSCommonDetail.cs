using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.MasterDetail — 작업 화면(시안 3.0 올림픽 'Zone'): 위 머리 줄 56(판 · 왼쪽 20 · 오른쪽 8 · 묶음 사이 16 · 이름 18 굵게 · 정보 12.5)
    /// → 필터 · 선택 막대(Section.Tray 48) → 본문 왼쪽 표(Ratio) | 오른쪽 고정 인스펙터. 표 머리 28 · 줄 앞 그림 원 32(그림 18) · 칸 사이 10.
    /// 접는 줄 44(펼침 화살표 16). 판 하나를 가운데 둘 때(빈 화면) 폭 = CenterRatio.
    /// </summary>
    public static class MasterDetail
    {
        public const float Ratio = 0.6f, CenterRatio = 0.6f, HeadH = 56f, HeadPadL = 20f, HeadPadR = 8f, HeadGap = 16f, NameFont = 18f, InfoFont = 12.5f;
        /// <summary>머리 줄 왼쪽 묶음에서 이름이 가질 수 있는 몫 · 본문 최소 높이(작은 화면에서는 창이 스크롤).</summary>
        public const float NameShare = 0.45f, MinBodyH = 240f;
        public const float TableHeadH = 28f, IconD = 32f, Icon = 18f, ColGap = 10f, NameMinW = 120f;
        public const float FoldH = 44f, FoldIcon = 16f;
    }

    /// <summary>
    /// CSCommonUX.ScrollArea — 판 안 따로 스크롤하는 칸(시안 표 · 인스펙터 overflow-y): 휠 30 · 오른쪽 막대 4(안쪽 4 · 최소 30, Look.PillLine).
    /// 창 몸통이 휠을 먼저 써도(Event.Use) rawType 으로 받음 — 창 몸통이 스크롤하지 않게 부르는 쪽이 내용을 창 높이에 맞출 것.
    /// 스크롤 자리는 id 마다 남음.
    /// </summary>
    public static class ScrollArea
    {
        public const float Step = 30f, BarW = 4f, BarInset = 4f, BarMinH = 30f;

        static readonly Dictionary<string, float> offsets = new();

        public static void Reset(string id) => offsets.Remove(id);

        /// <summary>r 안을 잘라 그림: paint 는 안쪽 좌표(위 0)에서 지금 보이는 칸을 받음. wheel = 휠을 받을지(위에 모달이 떠 있으면 false).</summary>
        public static void In(Rect r, string id, float contentH, bool wheel, Action<Rect> paint)
        {
            var e = Event.current;
            float max = Mathf.Max(0f, contentH - r.height);
            offsets.TryGetValue(id, out float s);
            if (wheel && max > 0f && e.type == EventType.ScrollWheel && Draw.Hover(r))
            {
                float to = Mathf.Clamp(s + e.delta.y * Step, 0f, max);
                if (to != s) { s = to; e.Use(); } // 끝에 닿으면 휠을 창으로 넘김
            }
            s = Mathf.Clamp(s, 0f, max);
            offsets[id] = s;
            Draw.BeginClip(r, new Vector2(0f, s));
            paint(new Rect(0f, s, r.width, r.height));
            Draw.EndClip();
            if (max <= 0f || !Draw.Repaint) return;
            float track = r.height - BarInset * 2f, bar = Mathf.Max(BarMinH, track * r.height / contentH);
            Draw.Round(new Rect(r.xMax - BarInset - BarW, r.y + BarInset + (track - bar) * (s / max), BarW, bar), Look.PillLine, BarW * 0.5f);
        }
    }

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
