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
    /// CSCommonUX.ScrollArea — 판 안 따로 스크롤하는 칸(시안 표 · 인스펙터 overflow-y): 휠 30 · 오른쪽 막대 = CSCommonUX.ScrollBar(잡고 끌기 · 길 누르기).
    /// 창 몸통이 휠을 먼저 써도(Event.Use) rawType 으로 받음 — 창 몸통이 스크롤하지 않게 부르는 쪽이 내용을 창 높이에 맞출 것.
    /// 스크롤 자리는 id 마다 남음.
    /// </summary>
    public static class ScrollArea
    {
        public const float Step = 30f, BarW = CSCommonDesign.ScrollBarW, BarInset = CSCommonDesign.ScrollBarInset, BarMinH = CSCommonDesign.ScrollBarMinH;

        static readonly Dictionary<string, float> offsets = new();

        public static void Reset(string id) => offsets.Remove(id);

        /// <summary>r 안을 잘라 그림: paint 는 안쪽 좌표(위 0)에서 지금 보이는 칸을 받음. wheel = 휠 · 막대를 받을지(위에 모달이 떠 있으면 false).</summary>
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
            ScrollBar.Handle(r, contentH, ref s, wheel); // 내용보다 먼저: 막대 띠 밑 칸 단추가 누름을 가져가지 않게
            s = Mathf.Clamp(s, 0f, max);
            offsets[id] = s;
            Draw.BeginClip(r, new Vector2(0f, s));
            paint(new Rect(0f, s, r.width, r.height));
            Draw.EndClip();
            ScrollBar.Paint(r, contentH, s);
        }
    }

    /// <summary>
    /// CSCommonUX.ScrollBar — 세로 스크롤 막대(창 몸통 · 목록 · 판 안 칸 · 알림 창이 같이 씀): view 오른쪽 안쪽에 막대 CSCommonDesign.ScrollBarW
    /// (칸 끝 → ScrollBarInset · 최소 ScrollBarMinH · 반지름 폭/2), 보통 Look.PillLine · 호버 Look.PillSub · 끄는 중 Look.PastelStrong(색만 바뀌고 크기는 그대로).
    /// 누름 띠 = view 오른쪽 끝 ScrollBarHit 폭. 막대를 잡고 끌면 잡은 자리 그대로 따라오고, 길을 누르면 막대 가운데가 그 자리로 온 뒤 이어 끎.
    /// 끄는 동안: Draw.Holding 이 참(지도 도구 · 툴팁), Draw.Begin 이 다른 누름 · 휠을 먹고, InputBlocker 가 온 화면을 막음(게임 도구 · 카메라).
    /// 마우스를 창 밖에서 떼도 다음 이벤트에 끝남. 부르는 법: 내용 그리기 전에 Handle(막대 띠 밑 칸 단추보다 먼저), 내용을 다 그린 뒤 Paint(위에 그림).
    /// scroll = 목표 값(부드러운 스크롤이면 다가가기 전 값 — 막대가 마우스에 붙고 내용이 따라옴). 막대 하나는 view 자리로 알아봄.
    /// </summary>
    public static class ScrollBar
    {
        const float W = CSCommonDesign.ScrollBarW, Inset = CSCommonDesign.ScrollBarInset, MinH = CSCommonDesign.ScrollBarMinH, HitW = CSCommonDesign.ScrollBarHit;

        static bool drag;
        static int dragKey;
        static float grab; // 막대 위 끝 → 잡은 자리

        /// <summary>막대를 끄는 중. 마우스를 떼면(창 밖 · 막대가 안 그려지는 동안이어도) 여기서 풂.</summary>
        public static bool Dragging
        {
            get
            {
                if (drag && !Input.GetMouseButton(0)) drag = false;
                return drag;
            }
        }

        static Rect Track(Rect view) => new(view.xMax - Inset - W, view.y + Inset, W, Mathf.Max(0f, view.height - Inset * 2f));
        static Rect Hit(Rect view) => new(view.xMax - HitW, view.y, HitW, view.height);
        static int Key(Rect view) { unchecked { return (Mathf.RoundToInt(view.xMax) * 397 ^ Mathf.RoundToInt(view.y)) * 397 ^ Mathf.RoundToInt(view.height); } }

        /// <summary>막대 길이: 길 × 보이는 몫, 최소 MinH · 길보다 길지 않음.</summary>
        public static float ThumbH(float track, float viewH, float contentH) => Mathf.Min(track, Mathf.Max(MinH, contentH > 0f ? track * viewH / contentH : track));
        /// <summary>스크롤(0 ~ max) → 막대 위 끝(길 위에서 0 ~ track − thumb).</summary>
        public static float ToThumb(float scroll, float max, float track, float thumb) => max > 0f ? (track - thumb) * Mathf.Clamp01(scroll / max) : 0f;
        /// <summary>막대 위 끝 → 스크롤(길 밖은 끝에 붙음).</summary>
        public static float ToScroll(float thumbY, float max, float track, float thumb) => track > thumb ? max * Mathf.Clamp01(thumbY / (track - thumb)) : 0f;

        /// <summary>누름 · 끌기를 받아 scroll 을 바꿈(바뀌었으면 true). enabled = false(위에 모달)면 새로 잡지 않음. 내용을 그리기 전에 부를 것.</summary>
        public static bool Handle(Rect view, float contentH, ref float scroll, bool enabled = true)
        {
            int key = Key(view);
            bool mine = Dragging && dragKey == key;
            float max = contentH - view.height;
            if (max <= 0f)
            {
                if (mine) drag = false;
                return false;
            }
            var e = Event.current;
            var t = Track(view);
            float th = ThumbH(t.height, view.height, contentH), before = scroll;
            if (mine)
            {
                if (e.rawType == EventType.MouseUp && e.button == 0) { drag = false; e.Use(); }
                else if (e.type == EventType.MouseDrag) { scroll = ToScroll(Draw.Mouse.y - t.y - grab, max, t.height, th); e.Use(); }
            }
            else if (enabled && !Dragging && e.type == EventType.MouseDown && e.button == 0 && Draw.Hover(Hit(view)))
            {
                float top = t.y + ToThumb(scroll, max, t.height, th), m = Draw.Mouse.y;
                grab = m >= top && m <= top + th ? m - top : th * 0.5f;
                drag = true;
                dragKey = key;
                scroll = ToScroll(m - t.y - grab, max, t.height, th);
                e.Use();
            }
            return scroll != before;
        }

        /// <summary>막대를 그림(넘칠 때 · Repaint 만). 내용을 다 그린 뒤 부를 것.</summary>
        public static void Paint(Rect view, float contentH, float scroll)
        {
            float max = contentH - view.height;
            if (max <= 0f || !Draw.Repaint) return;
            var t = Track(view);
            float th = ThumbH(t.height, view.height, contentH);
            Color c = Dragging && dragKey == Key(view) ? Look.PastelStrong : !Dragging && Draw.Hover(Hit(view)) ? Look.PillSub : Look.PillLine;
            Draw.Round(new Rect(t.x, t.y + ToThumb(scroll, max, t.height, th), W, th), c, W * 0.5f);
        }

        /// <summary>자가 검사: 스크롤 ↔ 막대 왕복 · 양 끝 · 막대 최소 · 길 밖 누름은 끝에 붙음.</summary>
        public static void SelfCheck(Action<bool, string> check)
        {
            static bool Near(float a, float b) => Mathf.Abs(a - b) < 0.01f;
            float track = 400f, max = 1600f, th = ThumbH(track, 400f, 2000f);
            check(Near(th, 80f), "scrollbar thumb share");
            check(Near(ThumbH(track, 10f, 100000f), MinH) && Near(ThumbH(track, 500f, 400f), track), "scrollbar thumb min/max");
            check(Near(ToScroll(ToThumb(600f, max, track, th), max, track, th), 600f), "scrollbar round trip");
            check(Near(ToThumb(0f, max, track, th), 0f) && Near(ToThumb(max, max, track, th), track - th), "scrollbar ends");
            check(Near(ToScroll(-50f, max, track, th), 0f) && Near(ToScroll(track + 50f, max, track, th), max), "scrollbar clamp");
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
