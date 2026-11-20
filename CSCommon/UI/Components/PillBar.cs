using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.PillBar — 툴바 C안 알약 공용(하단 Toolbar · SubToolBar · Dockbar 도구 줄 · SubDockBar): 치수 · 순수 계산 · 판 · 칸 · 뜯어져 나오기.
    /// 글래스 · 그림자 없음: 불투명 Look.BarSurface + 1px Look.BarLine 만. 고른 칸 = 파스텔 채움 + [아이콘 24 · 8 · 이름 14 굵게] 한 덩어리 가운데,
    /// 안 고른 칸 = 아이콘만(폭 44). 칸 폭은 부르는 쪽이 CSCommonAnim.Smooth 로 늘리고 줄이며 글은 폭이 열린 만큼 나타남.
    /// </summary>
    public static class PillBar
    {
        /// <summary>판 높이 · 안쪽 여백 · 칸 높이 · 칸 사이 · 아이콘 · 아이콘→이름 · 이름 글 · 고른 칸 좌우 · 안 고른 칸 폭 · 누름 배율.</summary>
        public const float Height = 64f, Pad = 8f, PadX = 12f, CellH = 48f, Gap = 2f, IconSize = 24f, IconGap = 8f, Font = 14f, CellPadX = 14f, IdleW = 44f, PressScale = 0.96f;
        /// <summary>서브 알약 · 독 옵션 막대: 높이 · 안쪽 · 칸 높이 · 칸 좌우 · 칸 최소 폭 · 칸 사이 · 글 · 메인 알약과 간격(독은 옵션 막대와 안내 알약 사이도 같음).</summary>
        public const float SubH = 48f, SubPad = 8f, SubCellH = 32f, SubCellPadX = 14f, SubCellMinW = 64f, SubCellGap = 4f, SubFont = 13f, SubGap = 12f;
        /// <summary>둥근 FAB 지름 · 알약과 FAB · FAB 끼리 간격.</summary>
        public const float FabSize = 64f, FabGap = 12f;
        /// <summary>독 도구 칸 사이(Dockbar 도구 줄 · IconGroup) — 아이콘 사이를 넓게.</summary>
        public const float DockGap = 12f;
        /// <summary>서브 막대 이름 글 · 구분선: 글 크기 · 좌우 여백 · 구분선 높이.</summary>
        public const float SubTextFont = 12f, SubTextPadX = 6f, SubSepH = 18f;
        /// <summary>뜯어져 나오기: 걸리는 초 · 투명도 0→1 초 · 숨은 가로 · 세로 배율.</summary>
        public const float TearSeconds = 0.46f, TearFade = 0.22f, TearSx = 0.62f, TearSy = 0.55f;
        /// <summary>안 고른 칸 호버 바탕 알파(Pal.Haze) · 아이콘 선 두께.</summary>
        public const float HoverAlpha = 0.06f, Stroke = 1.7f;

        // ---- 순수 계산(SelfCheck)

        /// <summary>고른 칸 폭 = 아이콘 24 + 8 + 글폭 + 좌우 14씩.</summary>
        public static float CellW(float labelW) => IconSize + IconGap + labelW + CellPadX * 2f;

        /// <summary>서브 칸 폭 = 글폭 + 좌우 14씩 + 뱃지 폭(최소 64).</summary>
        public static float SubCellW(float textW, float extraW = 0f) => Mathf.Max(SubCellMinW, textW + SubCellPadX * 2f + extraW);

        /// <summary>판 폭 = 칸 폭 합 + 칸 사이 + 좌우 여백.</summary>
        public static float Width(float cellsW, int count, float gap, float pad) => cellsW + gap * Mathf.Max(0, count - 1) + pad * 2f;

        /// <summary>가운데 정렬 왼쪽 x(화소 맞춤).</summary>
        public static float Left(float centerX, float w) => Mathf.Round(centerX - w * 0.5f);

        /// <summary>칸이 열린 정도 0 ~ 1: 지금 폭 w 가 안 고른 폭(44) ~ 고른 폭 selW 사이 어디인가.</summary>
        public static float Open(float w, float selW) => selW > IdleW ? Mathf.Clamp01((w - IdleW) / (selW - IdleW)) : 1f;

        /// <summary>끝에서 살짝(약 7%) 넘쳤다 돌아오는 이징(ease-out back — 시안 cubic-bezier(.3,1.35,.45,1) 어림). 0 → 0, 1 → 1.</summary>
        public static float Overshoot(float t)
        {
            const float c = 1.4f;
            t = Mathf.Clamp01(t) - 1f;
            return 1f + (c + 1f) * t * t * t + c * t * t;
        }

        /// <summary>뜯어져 나오기 진행도 t(0 숨음 ~ 1 다 나옴, 시간 비례) → 가로 · 세로 배율 · 아래로 밀린 거리(숨으면 hideDy) · 투명도.</summary>
        public static void Tear(float t, float hideDy, out float sx, out float sy, out float dy, out float alpha)
        {
            float e = Overshoot(t);
            sx = Mathf.LerpUnclamped(TearSx, 1f, e);
            sy = Mathf.LerpUnclamped(TearSy, 1f, e);
            dy = hideDy * (1f - e);
            alpha = Mathf.Clamp01(t * TearSeconds / TearFade);
        }

        /// <summary>순수 계산 점검: check(맞나, 이름).</summary>
        public static void SelfCheck(Action<bool, string> check)
        {
            static bool Near(float a, float b) => Mathf.Abs(a - b) < 0.001f;
            check(Near(CellW(30f), 90f), "pill cell width 24+8+30+28");
            check(Near(Width(IdleW * 3f + 90f, 4, Gap, Pad), IdleW * 3f + 90f + 6f + 16f), "pill bar width");
            check(Near(SubCellW(10f), SubCellMinW) && Near(SubCellW(50f, 20f), 98f), "sub cell width min 64");
            check(Near(Left(500f, 100f), 450f), "center left");
            check(Near(Open(IdleW, 90f), 0f) && Near(Open(90f, 90f), 1f) && Near(Open(67f, 90f), 0.5f), "open");
            Tear(0f, 76f, out float sx, out float sy, out float dy, out float a);
            check(Near(sx, TearSx) && Near(sy, TearSy) && Near(dy, 76f) && Near(a, 0f), "tear hidden");
            Tear(1f, 76f, out sx, out sy, out dy, out a);
            check(Near(sx, 1f) && Near(sy, 1f) && Near(dy, 0f) && Near(a, 1f), "tear shown");
            float peak = 0f;
            for (int i = 1; i < 40; i++) peak = Mathf.Max(peak, Overshoot(i / 40f));
            check(peak > 1.02f && peak < 1.12f, "tear overshoot");
            // 말줄임표 없는 맞춤: 들어가면 그대로 · 넘치면 비례로 줄이되 하한(12) 밑으론 안 줄임 · 원래 하한보다 작은 글은 키우지 않음
            check(Near(Draw.FitSize(80f, 14f, 12f, 100f), 14f) && Near(Draw.FitSize(105f, 14f, 12f, 100f), 14f * 100f / 105f) && Near(Draw.FitSize(200f, 14f, 12f, 100f), 12f) && Near(Draw.FitSize(200f, 10f, 12f, 100f), 10f), "text fit floor");
        }

        // ---- 움직임

        struct TearState
        {
            public float T, At, Seen;
            public int Frame;
        }

        static readonly Dictionary<int, TearState> tears = new();

        /// <summary>
        /// 뜯어져 나오기 진행도(0 ~ 1): show 쪽으로 TearSeconds 동안 시간 비례로 감(그리기 때만). 0.25초 넘게 · 한 틀 넘게 안 불렸으면 0 에서 다시 — 다시 생기면 또 뜯어져 나옴.
        /// 틀 수도 보는 까닭: 한 틀이 0.25초 넘게 걸리면(탭 다시 짓기 · 자동 저장 · GC) 매 틀 불렸는데도 다시 뜯어져 나왔음.
        /// </summary>
        public static float TearProgress(int key, bool show)
        {
            float now = Time.unscaledTime;
            int frame = Time.frameCount;
            bool had = tears.TryGetValue(key, out var s);
            if (had && now - s.Seen > 0.25f)
            {
                // 진단(잠시): 서브 알약이 다시 뜯어져 나올 뻔한 때 한 줄 — 긴 한 틀이면 그대로 둠
                bool gone = frame - s.Frame > 1;
                if (show) CSCommonLog.Info("[진단] 알약 다시 나옴 " + (gone ? "시작" : "막음(긴 틀)") + " key=" + key + " 쉰 시간=" + (now - s.Seen).ToString("0.00") + "초 쉰 틀=" + (frame - s.Frame));
                if (gone) had = false;
            }
            if (!had) s = new TearState { At = now };
            s.Seen = now;
            s.Frame = frame;
            if (Event.current == null || Event.current.type == EventType.Repaint)
            {
                s.T = Mathf.MoveTowards(s.T, show ? 1f : 0f, Mathf.Clamp(now - s.At, 0f, 0.1f) / TearSeconds);
                s.At = now;
            }
            tears[key] = s;
            return s.T;
        }

        static readonly Stack<Matrix4x4> tearMatrix = new();
        static readonly Stack<Vector2> tearSaved = new(); // x = 투명도, y = 아래로 민 거리

        /// <summary>
        /// 뒤에 그리는 것을 t 만큼 뜯어져 나오게: 숨으면 hideDy 만큼 밀림 · 가로 62% · 세로 55% · 투명. 판정 칸은 그대로(숨은 동안 클릭은 부르는 쪽이 막음).
        /// hideDy &gt; 0 = 메인 알약 위로 나옴(r 아래 가운데 기준 — 하단 툴바 서브), &lt; 0 = 메인 알약 아래로 나옴(r 위 가운데 기준 — 독 옵션 막대).
        /// EndTear 와 짝.
        /// </summary>
        public static void BeginTear(Rect r, float t, float hideDy)
        {
            Tear(t, hideDy, out float sx, out float sy, out float dy, out float a);
            tearMatrix.Push(GUI.matrix);
            tearSaved.Push(new Vector2(Draw.Opacity, dy));
            Draw.Opacity *= a;
            Draw.Shift(new Vector2(0f, dy));
            if (t < 1f) GUIUtility.ScaleAroundPivot(new Vector2(sx, sy), Draw.ToScreen(new Vector2(r.center.x, hideDy < 0f ? r.yMin : r.yMax)));
        }

        public static void EndTear()
        {
            if (tearMatrix.Count == 0) return;
            GUI.matrix = tearMatrix.Pop();
            var s = tearSaved.Pop();
            Draw.Shift(new Vector2(0f, -s.y));
            Draw.Opacity = s.x;
        }

        // ---- 그리기

        /// <summary>판: 불투명 바탕 + 1px 선. 반지름 = 짧은 변 / 2(radius 를 주면 그것).</summary>
        public static void Plate(Rect r, float radius = -1f)
        {
            if (!Draw.Repaint || r.width <= 0f || r.height <= 0f) return;
            float rad = radius >= 0f ? radius : Mathf.Min(r.width, r.height) * 0.5f;
            Draw.Round(r, Look.BarSurface, rad);
            Draw.RoundOutline(r, Look.BarLine, rad);
        }

        /// <summary>
        /// 알약 칸(그리기만): open = 열린 정도(Open) — 열린 만큼 파스텔 채움 · 아이콘 색 · 이름이 나타남. 안 고른 칸은 hover 때 옅은 알약 바탕.
        /// [아이콘 + 8 + 이름] 한 덩어리를 칸 가운데(아이콘이 없으면 이름만 가운데), 누르면(press 0 ~ 1) 0.96배.
        /// </summary>
        public static void Cell(Rect cell, string icon, string label, float labelW, float open, float hover, float press)
        {
            if (!Draw.Repaint) return;
            float rad = cell.height * 0.5f;
            Draw.BeginScale(cell.center, Look.PressScale(press, PressScale));
            if (open > 0.01f) Draw.Round(cell, Draw.Alpha(Look.PastelFill, open), rad);
            if (hover > 0.01f) Draw.Round(cell, Draw.Alpha(Pal.Haze, HoverAlpha * hover), rad);
            bool hasIcon = !string.IsNullOrEmpty(icon);
            Color ink = Color.Lerp(Look.BarOff, Look.PastelInk, open);
            // 이름이 없는 칸(독 설정 막대)은 아이콘만 가운데
            float gap = hasIcon && !string.IsNullOrEmpty(label) ? IconGap * open : 0f, iw = hasIcon ? IconSize : 0f;
            float x = cell.center.x - (hasIcon ? iw + gap + labelW * open : labelW) * 0.5f;
            if (hasIcon) Draw.Icon(icon, new Rect(Mathf.Round(x), Mathf.Round(cell.center.y - IconSize * 0.5f), IconSize, IconSize), ink, 24f, Stroke);
            float a = hasIcon ? Mathf.Clamp01(open * 2f - 1f) : 1f;
            if (a > 0.01f && !string.IsNullOrEmpty(label))
                Draw.Text(new Rect(Mathf.Round(x + iw + gap), cell.y, labelW + 2f, cell.height), label, Font, Draw.Alpha(ink, a), true, TextAnchor.MiddleLeft);
            Draw.EndScale();
        }

        /// <summary>
        /// 서브 칸(그리기만): 고름 = 서브 파스텔 채움 + 서브 파스텔 글(굵게), 안 고름 = 보조 글(dim 이면 흐린 글) · hover 때 옅은 바탕. 누르면 0.96배.
        /// 글은 (칸 − 오른쪽 뱃지 extraW) 가운데.
        /// </summary>
        public static void SubCell(Rect cell, string label, bool sel, bool dim, float hover, float press, float extraW = 0f)
        {
            if (!Draw.Repaint) return;
            float rad = cell.height * 0.5f;
            Draw.BeginScale(cell.center, Look.PressScale(press, PressScale));
            if (sel) Draw.Round(cell, Look.PastelSubFill, rad);
            else if (hover > 0.01f) Draw.Round(cell, Draw.Alpha(Pal.Haze, HoverAlpha * hover), rad);
            Draw.Text(new Rect(cell.x, cell.y, cell.width - extraW, cell.height), label, SubFont, sel ? Look.PastelSubInk : dim ? Pal.Dim : Look.BarOff, sel, TextAnchor.MiddleCenter);
            Draw.EndScale();
        }

        /// <summary>
        /// 서브 칸 단추(독 옵션 막대 안 — 독 콜백과 같은 꼴 x 왼쪽 · cy 가운데 · draw = false 면 폭만): 폭을 돌려주고 눌러 떼면 click.
        /// on = 고름(서브 파스텔), enabled = false 면 흐린 글 · 클릭 안 받음.
        /// </summary>
        public static float SubChip(float x, float cy, string label, bool on, string tip, bool draw, Action click, bool enabled = true)
        {
            float w = SubCellW(Mathf.Ceil(Draw.TextWidth(label ?? "", SubFont, true)));
            if (!draw) return w;
            var r = new Rect(x, Mathf.Round(cy - SubCellH * 0.5f), w, SubCellH);
            int key = CSCommonAnim.Key("pillbar.subchip", label); // 자리와 무관(움직이는 막대에서 presses · slots 가 쌓이지 않게)
            SubCell(r, label, on, !enabled, 0f, Look.Press(key, r, enabled));
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            if (Draw.Button(r) && enabled) click?.Invoke();
            return w;
        }

        /// <summary>서브 막대 이름 글(SubDockBar 안 — x 왼쪽 · cy 가운데 · draw = false 면 폭만): 좌우 6 · 글 12 굵게 · Look.BarOff. 폭을 돌려줌.</summary>
        public static float SubText(float x, float cy, string label, bool draw)
        {
            float tw = Mathf.Ceil(Draw.TextWidth(label ?? "", SubTextFont, true)), w = tw + SubTextPadX * 2f;
            if (draw && Draw.Repaint) Draw.Text(new Rect(x + SubTextPadX, Mathf.Round(cy - SubCellH * 0.5f), tw + 2f, SubCellH), label, SubTextFont, Look.BarOff, true, TextAnchor.MiddleLeft);
            return w;
        }

        /// <summary>서브 막대 세로 구분선: 1 × 18 · 좌우 6 · Look.BarLine. 폭을 돌려줌.</summary>
        public static float SubSep(float x, float cy, bool draw)
        {
            if (draw && Draw.Repaint) Draw.Fill(new Rect(x + SubTextPadX, Mathf.Round(cy - SubSepH * 0.5f), 1f, SubSepH), Look.BarLine);
            return SubTextPadX * 2f + 1f;
        }

        /// <summary>
        /// 독 설정 막대(Dockbar FAB 줄 안 — fabs 콜백과 같은 꼴 x 왼쪽 · cy 가운데 · draw = false 면 폭만): 판 높이 64 · 안쪽 8 · 아이콘만 칸 48 원 · 칸 사이 2.
        /// 켜진 칸(on(칸 번호) — 고른 도구 · 열린 판 등 여럿 가능) = 파스텔 채움 · 파스텔 아이콘. 폭을 돌려주고 눌러 떼면 pick(칸 번호).
        /// </summary>
        public static float IconGroup(float x, float cy, string[] icons, string[] tips, Func<int, bool> on, bool draw, Action<int> pick)
        {
            int n = icons.Length;
            float w = Width(CellH * n, n, DockGap, PadX);
            if (!draw) return w;
            var bar = new Rect(x, Mathf.Round(cy - Height * 0.5f), w, Height);
            Plate(bar);
            for (int i = 0; i < n; i++)
            {
                var c = new Rect(bar.x + PadX + i * (CellH + DockGap), bar.y + Pad, CellH, CellH);
                int k = CSCommonAnim.Key("pillbar.icongroup", icons[i]); // 자리와 무관
                bool sel = on != null && on(i);
                Cell(c, icons[i], null, 0f, sel ? 1f : 0f, 0f, Look.Press(k, c));
                CSCommonUX.UI.CSCommonToolbar.Tip(c, tips != null && i < tips.Length ? tips[i] : null);
                if (Draw.Button(c)) pick?.Invoke(i);
            }
            return w;
        }
    }
}
