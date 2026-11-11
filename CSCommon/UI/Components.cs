using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    // CSCommon 컴포넌트(디자인 아티팩트 'CSCommon 컴포넌트' 페이지 · 입력 / 표시 · 알림 / 판 · 창 보드 그대로).
    // 공통 규칙: 톤 바탕 · 테두리 없음(초점 · 고름만 청록 1.5px) · 높이 40(작게 32) · 글 14.5 · 아이콘 18 뜻 색 · 꺼짐 #5B6370.
    // IMGUI(Draw) 위에서 그리고, 모든 모드 화면(디자인 에디터 · F1 창)이 이것만 씀.

    /// <summary>단추 톤: 주(청록 톤) · 보통(회색 톤) · 지움(회색 톤 + 빨강 아이콘) · 글(면 없음 · 청록 글, GLDS Text).</summary>
    public enum Tone
    {
        Primary,
        Neutral,
        Danger,
        Text,
    }

    /// <summary>컴포넌트가 같이 쓰는 색 · 아이콘 · 움직임.</summary>
    public static class Look
    {
        public static Color Field = Draw.Hex("#1B1F26"), FieldFocus = Draw.Hex("#14171C");
        /// <summary>입력 칸 · 고르기 칸 · 펼친 목록 테두리 — 칸 바탕 · 창 바탕 모두와 3:1 이상(UI 경계 기준). 라이트 #7A8089 · 다크 #737B88.</summary>
        public static Color FieldLine = Draw.Hex("#737B88");
        public static Color Tone0 = Draw.Hex("#262B33"), Tone1 = Draw.Hex("#30363F"), Tone2 = Draw.Hex("#22262E");
        public static Color OffInk = Draw.Hex("#4A505B"), OffText = Draw.Hex("#5B6370");
        public static Color Ink = Draw.Hex("#0B0D11"), PanelLine = Draw.Hex("#2A2F38"), Divider = Draw.Hex("#22262E");
        public static Color CountBg = new(1f, 169f / 255f, 77f / 255f, 0.18f);
        public static Color Off = new(1f, 1f, 1f, 0.03f);
        /// <summary>떠 있는 판 그림자 한 겹 알파(다크 0.045 / 라이트 0.03).</summary>
        public static float ShadowAlpha = 0.045f;

        /// <summary>
        /// C안 알약(하단 툴바 · 서브 알약 · 독 · FAB): 판 바탕 · 1px 선 · 안 고른 칸 아이콘/글 · 모드 파스텔(고른 칸 채움 · 글) · 서브 파스텔(채움 · 글) ·
        /// FAB 되돌리기(파랑) · 취소(빨강). CSCommonTheme.Use 가 테마 · 팔레트에 맞춰 채움(기본값 = 다크 청록).
        /// </summary>
        public static Color BarSurface = Draw.Hex("#1B1E24"), BarLine = new(1f, 1f, 1f, 0.08f), BarOff = Draw.Hex("#A7ADB8");
        public static Color PastelFill = Draw.Hex("#B8EFE6"), PastelInk = Draw.Hex("#0A3D37"), PastelSubFill = new(184f / 255f, 239f / 255f, 230f / 255f, 0.18f), PastelSubInk = Draw.Hex("#D3F5EF");
        public static Color FabUndoFill = Draw.Hex("#BCD4FF"), FabUndoInk = Draw.Hex("#0F2A5C"), FabCancelFill = Draw.Hex("#FFC9C4"), FabCancelInk = Draw.Hex("#5C0F0B");
        /// <summary>독 모드 나가기 FAB 아이콘(빨강 — 면은 판 바탕) · 독 안내 알약 머리 글.</summary>
        public static Color FabExitInk = Draw.Hex("#FF6B5E"), BarInk = Color.white;
        /// <summary>CSCommonUX.Button · IconButton(시안 'CSCommonUX.Button'): 주 · 지움 · 보통 · 글 톤 호버 · 누름 면, 꺼진 단추 면 · 글, 꺼진 아이콘 단추 아이콘.</summary>
        public static Color PastelHover = Draw.Hex("#A8E8DE"), PastelPress = Draw.Hex("#98E0D5"), DangerHover = Draw.Hex("#FFBAB4"), DangerPress = Draw.Hex("#FFAAA3");
        public static Color NeutralHover = Draw.Hex("#30363F"), NeutralPress = Draw.Hex("#22262E"), TextHover = new(184f / 255f, 239f / 255f, 230f / 255f, 0.10f), TextPress = new(184f / 255f, 239f / 255f, 230f / 255f, 0.18f);
        public static Color BtnOff = new(1f, 1f, 1f, 0.03f), BtnOffInk = Draw.Hex("#5B6370"), IconOffInk = Draw.Hex("#4A505B");

        /// <summary>
        /// 모달 · 알약 글 칸(시안 'CSCommonUX.Modal' · 'CSCommonUX.Textfield'): 덮개 · 발/머리 띠 면 · 발 위 선 · 알약 바탕(글 칸 · 탭 홈 · 타일) · 회색 알약(보통 단추 · 닫기 원) ·
        /// 회색 알약 글 · 알약 호버 선 · 밑줄 제목 선 · 파스텔 테두리(초점 · 커서 · 밑줄) · 파스텔 글 단추 · 손잡이 점 투명도 · 모달 그림자(알파 · 아래 · 흐림).
        /// </summary>
        public static Color Scrim = new(5f / 255f, 6f / 255f, 8f / 255f, 0.55f), Foot = Draw.Hex("#171A20"), FootLine = Draw.Hex("#22262E");
        public static Color Pill = Draw.Hex("#1B1F26"), PillNeutral = Draw.Hex("#262B33"), PillNeutralInk = Draw.Hex("#C9CFD8"), PillHoverLine = Draw.Hex("#30363F"), TitleLine = Draw.Hex("#3A404B");
        public static Color PastelRing = Draw.Hex("#B8EFE6"), PastelLink = Draw.Hex("#B8EFE6");
        /// <summary>알약 홈(Look.Pill) 안 원 단추 면(스테퍼 − +): 홈과 한 단 다르게. 라이트 = PillNeutral 값 · 다크 = 한 단 밝게.</summary>
        public static Color PillRaised = Draw.Hex("#30363F");
        public static float GripAlpha = 0.6f, ModalShadow = 0.5f, ModalShadowY = 10f, ModalShadowBlur = 26f;

        /// <summary>
        /// 컨트롤 시안(CtlA · B · C): 분절 고른 칸 면 · 보조 글(서브라벨 · 타일 · 줄 메모 · 숫자 이름) · 알약 1px 선(칩 · 우물 · 카드 · 토스트 · 슬라이더 빈 막대) ·
        /// 펼친 목록 면 · 선 · 줄 호버 · 손잡이(슬라이더 · 채운 눈금) · 꺼진 손잡이(스위치 · 빈 눈금) · 파스텔 위 보조 글 · 색 칸 선 · 진한 파스텔(슬라이더 · 목표 넘은 막대) ·
        /// 못 미친 막대 · 말풍선 면 · 글 · 상태 알약 주황 면 · 글. CSCommonTheme.UsePastel 이 채움(기본값 = 다크 청록).
        /// </summary>
        public static Color SegmentOn = Draw.Hex("#3A404B"), PillSub = Draw.Hex("#7F8894"), PillLine = Draw.Hex("#30363F");
        public static Color MenuSurface = Draw.Hex("#1F232B"), MenuLine = new(1f, 1f, 1f, 0.14f), MenuHover = Draw.Hex("#2A3039");
        public static Color Knob = Draw.Hex("#FCFCFC"), KnobOff = Draw.Hex("#8A919B"), PastelNote = Draw.Hex("#0A3D37"), SwatchLine = new(1f, 1f, 1f, 0.12f);
        public static Color PastelStrong = Draw.Hex("#B8EFE6"), ProgressLow = Draw.Hex("#FFA94D"), TipFill = Draw.Hex("#0B0D11"), TipInk = Draw.Hex("#ECEDF2");
        public static Color WarnFill = Draw.Hex("#FFD9A8"), WarnInk = Draw.Hex("#5A3000");

        /// <summary>호버 0 ~ 1(서서히).</summary>
        public static float Hover(int key, Rect r, bool enabled = true) => 0f; // 호버 효과 없음(호출부 호환)

        /// <summary>눌림: 누르는 데 걸리는 초 · 떼고 돌아오는 초 · 돌아오는 곡선 cubic-bezier(.2, .9, .3, 1.3)(시안 '.press' — 살짝 넘쳤다 앉음).</summary>
        public const float PressInSeconds = 0.03f, PressOutSeconds = 0.26f, PressX1 = 0.2f, PressY1 = 0.9f, PressX2 = 0.3f, PressY2 = 1.3f;

        struct PressRun
        {
            public float V, From, Rel, Last;
            public bool Down;
        }

        static readonly Dictionary<int, PressRun> presses = new();

        /// <summary>
        /// 누름 정도: 누르는 동안 0.03초에 0 → 1, 떼면 0.26초 동안 시안 곡선으로 0 쪽으로 돌아가며 잠깐 0 아래(음수)로 넘침.
        /// 배율은 PressScale(press, 눌림 배율) 로 — 넘침이 잘리지 않게(Mathf.Lerp 는 0 ~ 1 로 자름). 그리기(Repaint) 때만 움직임.
        /// </summary>
        public static float Press(int key, Rect r, bool enabled = true)
        {
            key ^= 0x27d4eb2f;
            bool down = enabled && Draw.Hover(r) && Draw.Pressed(r);
            float now = Time.unscaledTime;
            if (!presses.TryGetValue(key, out var s)) s = new PressRun { Rel = -100f, Last = now };
            if (Event.current == null || Event.current.type == EventType.Repaint)
            {
                if (down) s.V = Mathf.MoveTowards(Mathf.Max(s.V, 0f), 1f, Mathf.Clamp(now - s.Last, 0f, 0.1f) / PressInSeconds);
                else
                {
                    if (s.Down) { s.From = s.V; s.Rel = now; }
                    float t = (now - s.Rel) / PressOutSeconds;
                    s.V = t >= 1f ? 0f : s.From * (1f - Modal.Ease(t, PressX1, PressY1, PressX2, PressY2));
                }
                s.Down = down;
                s.Last = now;
                presses[key] = s;
            }
            return s.V;
        }

        /// <summary>눌림 정도 → 그릴 배율(1 ↔ scale, 뗄 때 1 을 살짝 넘음).</summary>
        public static float PressScale(float press, float scale) => Mathf.LerpUnclamped(1f, scale, press);

        public static int Key(Rect r, string a, string b = null)
        {
            // 잘라 그리는 영역 자리(ClipBase)를 더함: 영역 안 좌표만 쓰면 다른 섹션 몸통의 같은 자리 행이 호버 면을 같이 켰음
            var b0 = Draw.ClipBase;
            unchecked { return CSCommonAnim.Key(a ?? "", b ?? "") ^ Mathf.RoundToInt(r.x + b0.x) * 397 ^ Mathf.RoundToInt(r.y + b0.y) * 31; }
        }

        /// <summary>
        /// 떠 있는 판 그림자 + 바탕 + 1px 선. 그림자는 1px 씩 넓어지며 옅어지는 여러 겹(흐림 흉내) — 예전 두 겹 큰 상자는
        /// 가장자리가 띠처럼 겹겹이 보였음.
        /// </summary>
        public static void Elevated(Rect r, float radius, Color fill, float shadow = 1f)
        {
            if (!Draw.Repaint) return;
            const int layers = 8;
            for (int i = layers; i >= 1; i--)
            {
                float k = 1f - (i - 1f) / layers;
                Draw.Round(new Rect(r.x - i, r.y + 2f + i * 0.5f - i, r.width + i * 2f, r.height + i * 2f), Draw.Alpha(Color.black, ShadowAlpha * k * shadow), radius + i);
            }
            Draw.Round(r, fill, radius);
            Draw.RoundOutline(r, PanelLine, radius);
        }

        /// <summary>고른 칸(떠오른 면): 한 단계 밝은 면 + 위 1px 밝은 선, 그림자 · 밑줄 · 테두리 없음. t = 나타남 0 ~ 1.</summary>
        public static void Raised(Rect r, float radius, float t = 1f)
        {
            if (!Draw.Repaint || t <= 0.01f) return;
            // 그림자 없음: 알약 · 둥근 칸은 1px 만 내려도 아래가 어두운 초승달로 보였음 — 밝은 면 + 위 1px 밝은 선으로만 떠오름
            Draw.Round(r, Draw.Alpha(Pal.Selected, t), radius);
            // 다크: 위 1px 밝은 선 / 라이트: 흰 면이 바탕과 구분되게 1px 테두리
            if (CSCommonTheme.Light) Draw.RoundOutline(r, Draw.Alpha(Look.PanelLine, t), radius);
            else Draw.Fill(new Rect(r.x + radius, r.y, r.width - radius * 2f, 1f), Draw.Alpha(Color.white, 0.08f * t));
        }
    }

    /// <summary>컴포넌트 안에서 쓰는 선 아이콘(24 상자 SVG 경로).</summary>
    public static class Icons
    {
        public const string Minus = "M5 12h14";
        public const string Plus = "M12 5v14M5 12h14";
        public const string Close = "M6 6l12 12M18 6L6 18";
        public const string Check = "M5 12l5 5 9-10";
        public const string Chevron = "M6 9l6 6 6-6";
        public const string Left = "M15 6l-6 6 6 6";
        public const string Right = "M9 6l6 6-6 6";
        public const string Search = "M11 18a7 7 0 1 0 0-14 7 7 0 0 0 0 14zM21 21l-4.3-4.3";
        public const string Warn = "M12 3l10 18H2L12 3zM12 10v4M12 17h.01";
        public const string Info = "M12 22a10 10 0 1 0 0-20 10 10 0 0 0 0 20zM12 16v-5M12 8h.01";
        public const string Trash = "M3 6h18M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2";
        public const string Undo = "M3 10h10a5 5 0 0 1 5 5v2M3 10l5-5M3 10l5 5";
        /// <summary>모드 나가기(문 + 바깥 화살표) · 확정 체크(독 FAB).</summary>
        public const string Exit = "M9 21H5V3h4M16 17l5-5-5-5M21 12H9";
        public const string Done = "M5 12.5L9.5 17L19 7.5";

        /// <summary>한 글자 단추 글 → 아이콘(− + × ‹ › ✓), 없으면 null.</summary>
        public static string ForGlyph(string text) => text switch
        {
            "−" or "-" => Minus,
            "+" => Plus,
            "×" or "x" => Close,
            "‹" or "<" => Left,
            "›" or ">" => Right,
            "✓" => Check,
            _ => null,
        };
    }

    /// <summary>
    /// CSCommonUX.Button — 공통 단추(시안 'CSCommonUX.Button'): 알약(반지름 = 높이/2) · 그림자 · 테두리 없음, [아이콘 + 글] 한 덩어리를 칸 정가운데.
    /// 보통 높이 40 · 좌우 20 · 사이 8 · 글 14.5 · 아이콘 18, 작게(small · 높이 36 미만 칸) 32 · 16 · 6 · 12.5 · 15. 아이콘만이면 정사각(40 · 32).
    /// 톤: 주 = 모드 파스텔(굵게) · 보통 = 회색 알약 · 지움 = 빨강 파스텔(굵게) · 글 = 면 없이 파스텔 글(호버 · 누름에 옅은 파스텔 면). 호버 · 누름은 한 단씩 진해지고 누르면 0.95배.
    /// 꺼짐 = 옅은 면(글 톤은 면 없음) + 흐린 글. 글에 맞춘 너비(At) · 주어진 칸(In) · 아이콘만(Square).
    /// </summary>
    public static class Button
    {
        /// <summary>높이 · 좌우 · 아이콘↔글 · 글 · 아이콘(보통 / 작게) · 누름 배율.</summary>
        public const float H = 40f, PadX = 20f, Gap = 8f, Font = 14.5f, Icon = 18f;
        public const float HSmall = 32f, PadXSmall = 16f, GapSmall = 6f, FontSmall = 12.5f, IconSmall = 15f, PressScale = 0.95f;

        /// <summary>글에 맞춘 너비(small = 높이 32). 글이 없으면 정사각.</summary>
        public static float Width(string icon, string label, bool small = false)
        {
            float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, small ? FontSmall : Font, true));
            if (tw <= 0f) return small ? HSmall : H;
            float iw = string.IsNullOrEmpty(icon) ? 0f : (small ? IconSmall + GapSmall : Icon + Gap);
            return (small ? PadXSmall : PadX) * 2f + iw + tw;
        }

        /// <summary>줄 가운데 cy · 왼쪽 x 에 글에 맞춘 단추(draw = false 면 재기만). 그린 너비를 돌려줌.</summary>
        public static float At(float x, float cy, string icon, string label, string tip, Tone tone, Color? iconColor = null, bool enabled = true, Action click = null, bool draw = true, bool small = false, bool loading = false)
        {
            float w = Width(icon, label, small);
            float h = small ? HSmall : H;
            if (draw) In(new Rect(x, Mathf.Round(cy - h * 0.5f), w, h), icon, label, tip, tone, iconColor, enabled, click, loading);
            return w;
        }

        /// <summary>주어진 칸에 단추. 높이 36 미만이면 작은 글 · 아이콘. iconColor = 아이콘 뜻 색(없으면 톤 글색). loading = 글 대신 스피너 · 누름 안 받음.</summary>
        public static void In(Rect r, string icon, string label, string tip, Tone tone, Color? iconColor = null, bool enabled = true, Action click = null, bool loading = false)
        {
            int key = Look.Key(r, icon, label) ^ 0x2f9a6b1d;
            bool live = enabled && !loading;
            float hv = Look.Hover(key, r, live), pr = Look.Press(key, r, live);
            if (Draw.Repaint)
            {
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                bool text = tone == Tone.Text;
                Color bg, ink;
                if (!enabled)
                {
                    bg = text ? Color.clear : Look.BtnOff;
                    ink = Look.BtnOffInk;
                }
                else
                {
                    switch (tone)
                    {
                        case Tone.Primary: bg = Tri(Look.PastelFill, Look.PastelHover, Look.PastelPress, hv, pr); ink = Look.PastelInk; break;
                        case Tone.Danger: bg = Tri(Look.FabCancelFill, Look.DangerHover, Look.DangerPress, hv, pr); ink = Look.FabCancelInk; break;
                        case Tone.Text: bg = Tri(Color.clear, Look.TextHover, Look.TextPress, hv, pr); ink = Look.PastelLink; break;
                        default: bg = Tri(Look.PillNeutral, Look.NeutralHover, Look.NeutralPress, hv, pr); ink = Look.PillNeutralInk; break;
                    }
                }
                if (bg.a > 0f) Draw.Round(r, bg, Mathf.Min(r.width, r.height) * 0.5f);
                bool small = r.height < 36f;
                bool bold = tone == Tone.Primary || tone == Tone.Danger;
                float font = small ? FontSmall : Font, isz = small ? IconSmall : Icon;
                float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, font, bold));
                float iw = string.IsNullOrEmpty(icon) ? 0f : isz + (tw > 0f ? (small ? GapSmall : Gap) : 0f);
                // 칸보다 길면 왼쪽 6 부터(넘친 글은 TextFit 이 줄임)
                float x = Mathf.Max(r.x + 6f, Mathf.Round(r.center.x - (iw + tw) * 0.5f));
                if (loading)
                    Draw.Spinner(r.center, isz * 0.5f - 1f, 2f, ink);
                else
                {
                    if (!string.IsNullOrEmpty(icon))
                        Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - isz * 0.5f), isz, isz), enabled ? iconColor ?? ink : ink, 24f, 2f);
                    // 글만 있는 단추는 칸 정가운데(가로 · 세로)
                    if (tw > 0f && iw <= 0f)
                        Draw.TextPill(new Rect(r.x + 6f, r.y, r.width - 12f, r.height), label, font, ink, bold, TextAnchor.MiddleCenter);
                    else if (tw > 0f)
                        Draw.TextPill(new Rect(x + iw, r.y, Mathf.Max(0f, r.xMax - x - iw - 6f), r.height), label, font, ink, bold, TextAnchor.MiddleLeft);
                }
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            if (live && Draw.Button(r)) click?.Invoke();
        }

        /// <summary>평소 → 호버 → 누름 면(한 단씩 진하게).</summary>
        static Color Tri(Color idle, Color hover, Color press, float hv, float pr) => Color.Lerp(Color.Lerp(idle, hover, hv), press, pr);

        /// <summary>아이콘만 정사각 단추(− · + · 지우기 · 닫기 · ‹ ›). 누르면 true.</summary>
        public static bool Square(Rect r, string icon, string tip = null, Tone tone = Tone.Neutral, bool enabled = true, Color? iconColor = null)
        {
            bool hit = false;
            In(r, icon, null, tip, tone, iconColor, enabled, () => hit = true);
            return hit;
        }

        /// <summary>한 글자 정사각(예전 '−' '+' '‹' '›' 단추): 아는 글자는 아이콘, 아니면 글.</summary>
        public static bool Glyph(Rect r, string text, bool enabled = true)
        {
            string icon = Icons.ForGlyph(text);
            if (icon != null) return Square(r, icon, null, Tone.Neutral, enabled);
            bool hit = false;
            In(r, null, text, null, Tone.Neutral, null, enabled, () => hit = true);
            return hit;
        }

        /// <summary>예전 테두리 색 → 톤(청록 · 하늘 = 주, 빨강 = 지움, 그 밖 = 보통)과 아이콘 뜻 색(주황 = 되돌림 · 경고).</summary>
        public static Tone ToneOf(Color outline, out Color? iconColor)
        {
            iconColor = null;
            if (outline == Pal.Teal || outline == Pal.Primary || outline == Pal.Ok) return Tone.Primary;
            if (outline == Pal.Red || outline == Pal.MapDanger) return Tone.Danger;
            if (outline == Pal.Warn) iconColor = Pal.Warn;
            return Tone.Neutral;
        }
    }

    /// <summary>
    /// CSCommonUX.IconButton — 아이콘 원 단추(시안 'CSCommonUX.IconButton'): 지름 40 · 아이콘 20 · 평소 면 없이 Look.BarOff, 호버 = 회색 원 면 + 짙은 아이콘,
    /// 켜짐 = 모드 파스텔 원(파스텔 글), 꺼짐 = 흐린 아이콘. 누르면 0.88배.
    /// </summary>
    public static class IconButton
    {
        /// <summary>지름 · 아이콘 · 누름 배율.</summary>
        public const float Size = 40f, IconSize = 20f, PressScale = 0.88f;

        public static float At(float x, float cy, string icon, bool on, string tip, bool draw, Action click, Color? color = null, bool enabled = true)
        {
            if (!draw) return Size;
            var r = new Rect(x, Mathf.Round(cy - Size * 0.5f), Size, Size);
            int key = Look.Key(r, icon, tip) ^ 0x4cf5ad43;
            float hv = Look.Hover(key, r, enabled), pr = Look.Press(key, r, enabled);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 9f);
            if (Draw.Repaint)
            {
                float rad = Size * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                if (hv * (1f - onT) > 0.01f) Draw.Round(r, Draw.Alpha(Look.PillNeutral, hv * (1f - onT)), rad);
                if (onT > 0.01f) Draw.Round(r, Draw.Alpha(Color.Lerp(Look.PastelFill, Look.PastelHover, hv), onT), rad);
                Color fg = !enabled ? Look.IconOffInk : Color.Lerp(color ?? Color.Lerp(Look.BarOff, Look.BarInk, hv), Look.PastelInk, onT);
                Draw.Icon(icon, new Rect(Mathf.Round(r.center.x - IconSize * 0.5f), Mathf.Round(r.center.y - IconSize * 0.5f), IconSize, IconSize), fg, 24f, 1.9f);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            if (enabled && Draw.Button(r)) click?.Invoke();
            return Size;
        }
    }

    /// <summary>
    /// CSCommonUX.Chip — 필터 알약(시안 'CSCommonUX.Chip'): 높이 32 · 좌우 14 · 글 13 · 반지름 = 높이/2. 평소 Look.Pill + 안쪽 1px Look.PillLine(호버 Look.NeutralHover),
    /// 고르면 선 없이 모드 파스텔 + 굵은 파스텔 글(0.2초). color = 팀 색이면 고른 칩을 그 색의 옅은 면 · 짙은 글로(팀 칩은 호버 없음).
    /// ▾(dropdown — 목록을 여는 칩) · ✕(고른 칩에 clear 를 주면) 14 · 글 뒤 4, 누르면 0.95배. 폭은 굵은 글로 재서 고를 때 안 바뀜.
    /// </summary>
    public static class Chip
    {
        /// <summary>팀 색 → 고른 칩 면(흰색에 섞는 비율 라이트 · 다크) · 글(어둡게 하는 배율 라이트 · 다크) · 색 전환 초.</summary>
        const float TeamFillLight = 0.13f, TeamFillDark = 0.22f, TeamInkLight = 0.6f, TeamInkDark = 0.4f, FadeSeconds = 0.2f, Stroke = 2.4f;

        public static float Width(string label, bool on, bool icon)
            => CSCommonDesign.ChipPadX * 2f + Mathf.Ceil(Draw.TextWidth(label, CSCommonDesign.ChipFont, true)) + (icon ? CSCommonDesign.ChipIconGap + CSCommonDesign.ChipIcon : 0f);

        /// <summary>줄 가운데 cy · 왼쪽 x 에 칩(draw = false 면 재기만). 그린 너비를 돌려줌.</summary>
        public static float At(float x, float cy, string label, bool on, Action click, bool draw = true, string tip = null, Color? color = null, bool dropdown = false, Action clear = null)
        {
            bool canClear = on && clear != null;
            float w = Width(label, on, canClear || dropdown), h = CSCommonDesign.ChipHeight;
            if (!draw) return w;
            var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
            int key = Look.Key(r, "chip", label);
            float hv = Look.Hover(key, r, !on && color == null), pr = Look.Press(key, r);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 1f / FadeSeconds);
            if (Draw.Repaint)
            {
                float rad = h * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, CSCommonDesign.ChipPressScale));
                Color fill = Look.PastelFill, ink = Look.PastelInk;
                if (color.HasValue)
                {
                    bool light = CSCommonTheme.Light;
                    fill = Color.Lerp(Color.white, color.Value, light ? TeamFillLight : TeamFillDark);
                    ink = Draw.Shade(color.Value, light ? TeamInkLight : TeamInkDark);
                    fill.a = ink.a = 1f;
                }
                Draw.Round(r, Color.Lerp(Color.Lerp(Look.Pill, Look.NeutralHover, hv), fill, onT), rad);
                if (onT < 0.99f) Draw.RoundOutline(r, Draw.Alpha(Look.PillLine, 1f - onT), rad);
                Color fg = Color.Lerp(Look.BarInk, ink, onT);
                float tx = r.x + CSCommonDesign.ChipPadX, tw = Mathf.Ceil(Draw.TextWidth(label, CSCommonDesign.ChipFont, true));
                Draw.Text(new Rect(tx, r.y, tw, h), label, CSCommonDesign.ChipFont, fg, on, TextAnchor.MiddleCenter);
                if (canClear || dropdown)
                {
                    float s = CSCommonDesign.ChipIcon;
                    Draw.Icon(canClear ? Icons.Close : Icons.Chevron, new Rect(tx + tw + CSCommonDesign.ChipIconGap, Mathf.Round(r.center.y - s * 0.5f), s, s), fg, 24f, Stroke);
                }
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            // ✕ 자리를 먼저(칩 오른쪽 끝 · 아이콘보다 넓게)
            if (canClear && Draw.Button(new Rect(r.xMax - CSCommonDesign.ChipPadX - CSCommonDesign.ChipIcon - 4f, r.y, CSCommonDesign.ChipPadX + CSCommonDesign.ChipIcon + 4f, h)))
            {
                clear();
                return w;
            }
            if (Draw.Button(r)) click?.Invoke();
            return w;
        }
    }

    /// <summary>
    /// CSCommonUX.Badge — 상태 알약(시안 'CSCommonUX.Badge'): 높이 24 · 좌우 12 · 반지름 = 높이/2 · 글 12 굵게 · 아이콘 13(선 2.4) + 5.
    /// 색은 뜻 → 파스텔 짝(Colors): 청록 · 초록 · 하늘 = 모드 파스텔, 주황 · 노랑 = Look.WarnFill/Ink, 빨강 = Look.FabCancel*, 회색 글 = Look.PillNeutral + Look.BarOff,
    /// 그 밖 뜻 색(팀 색 등)은 그 색 16% 면 + 그 색 글. 수 알약 Count: 높이 20(탭 안 18) · 최소 폭 = 높이 · 좌우 = 높이/2 − 3 · Look.CountBg + Pal.Warn 굵은 글.
    /// </summary>
    public static class Badge
    {
        public const float H = 24f, PadX = 12f, Icon = 13f, IconGap = 5f, Stroke = 2.4f, CountH = 20f, CountAlpha = 0.22f, CountOnAlpha = 0.26f;

        /// <summary>뜻 색 → 파스텔 면 · 글.</summary>
        public static void Colors(Color c, out Color fill, out Color ink)
        {
            if (c == Pal.Primary || c == Pal.Teal || c == Pal.Ok || c == Pal.Green || c == Pal.DoneInk) { fill = Look.PastelFill; ink = Look.PastelInk; }
            else if (c == Pal.Warn || c == Pal.Yellow) { fill = Look.WarnFill; ink = Look.WarnInk; }
            else if (c == Pal.Red || c == Pal.MapDanger) { fill = Look.FabCancelFill; ink = Look.FabCancelInk; }
            else if (c == Pal.Sub || c == Pal.Dim || c == Pal.Light || c == Look.OffText) { fill = Look.PillNeutral; ink = Look.BarOff; }
            else { fill = Draw.Alpha(c, 0.16f); ink = c; }
        }

        public static float At(float x, float cy, string text, Color color, string icon = null, bool draw = true, float height = H)
        {
            float tw = Mathf.Ceil(Draw.TextWidth(text, CSCommonDesign.FontMin, true)), iw = icon != null ? Icon + IconGap : 0f;
            float w = PadX * 2f + iw + tw;
            if (draw && Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - height * 0.5f), w, height);
                Colors(color, out var fill, out var ink);
                Draw.Round(r, fill, height * 0.5f);
                float tx = r.x + PadX;
                if (icon != null) { Draw.Icon(icon, new Rect(tx, Mathf.Round(r.center.y - Icon * 0.5f), Icon, Icon), ink, 24f, Stroke); tx += iw; }
                Draw.Text(new Rect(tx, r.y, tw + 2f, r.height), text, CSCommonDesign.FontMin, ink, true, TextAnchor.MiddleLeft);
            }
            return w;
        }

        /// <summary>주어진 칸에 상태 알약(뜻 색 → 파스텔 짝 · 굵은 글 가운데).</summary>
        public static void In(Rect r, string text, Color color)
        {
            if (!Draw.Repaint || string.IsNullOrEmpty(text)) return;
            Colors(color, out var fill, out var ink);
            Draw.Round(r, fill, r.height * 0.5f);
            Draw.TextPill(r, text, CSCommonDesign.FontMin, ink, true, TextAnchor.MiddleCenter);
        }

        /// <summary>수 알약(높이 h · 주황, color 를 주면 그 색 22% 면 + 그 색 글). on = 고른 칸 위(조금 진한 면).</summary>
        public static float Count(float x, float cy, string text, bool draw = true, bool on = false, float h = CountH, Color? color = null)
        {
            float w = Mathf.Max(h, Mathf.Ceil(Draw.TextWidth(text, CSCommonDesign.FontMin, true)) + (h * 0.5f - 3f) * 2f);
            if (draw && Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
                Color ink = color ?? Pal.Warn;
                Draw.Round(r, color.HasValue ? Draw.Alpha(ink, CountAlpha) : on ? Draw.Alpha(Pal.Warn, CountOnAlpha) : Look.CountBg, h * 0.5f);
                Draw.Text(r, text, CSCommonDesign.FontMin, ink, true, TextAnchor.MiddleCenter);
            }
            return w;
        }
    }

    /// <summary>
    /// CSCommonUX.Tabs — 묶음 탭(시안 'CSCommonUX.Tabs'): 알약 홈(Look.Pill · 안쪽 4 · 칸 사이 2 · 반지름 = 높이/2) 안에 이름이 늘 보이는 알약 칸 40.
    /// 칸 폭 = 아이콘 있으면 16 + [아이콘 16 · 8 · 이름] + 18, 없으면 20 + 이름 + 20(글 13.5), 내용은 칸 가운데. 고른 칸 = 모드 파스텔 + 굵은 파스텔 글,
    /// 안 고른 칸 = Look.BarOff 글 · 호버 Look.NeutralHover. 색 전환 0.2초 · 누르면 0.95배. 수 알약(Badge.Count 18)은 이름 뒤 8.
    /// Modal.Tabs · SheetTabs 도 같은 칸(Cell)을 씀. 펼쳐지는 칸(아이콘만 → 이름)은 Toolbar · Dockbar 전용(PillBar).
    /// </summary>
    public static class Tabs
    {
        public const float H = 40f, Pad = 4f, Gap = 2f, PadL = 16f, PadR = 18f, PadX = 20f, Icon = 16f, IconGap = 8f, Font = 13.5f, Stroke = 1.9f;
        public const float CountH = 18f, CountGap = 8f, PressScale = 0.95f, FadeSeconds = 0.2f;

        /// <summary>칸 폭(이름은 굵게 재서 고를 때 폭이 안 바뀜). 아이콘이 없으면 좌우 padX.</summary>
        public static float CellWidth(string icon, string label, string badge, float font = Font, float padX = PadX)
        {
            float w = Mathf.Ceil(Draw.TextWidth(label ?? "", font, true));
            if (!string.IsNullOrEmpty(badge)) w += CountGap + Badge.Count(0f, 0f, badge, false, false, CountH);
            return string.IsNullOrEmpty(icon) ? padX * 2f + w : PadL + Icon + IconGap + w + PadR;
        }

        /// <summary>줄 가운데 cy · 왼쪽 x 에 묶음 탭(draw = false 면 재기만). badgeColors[i] = 수 알약 색(없으면 주황). 그린 폭을 돌려줌.</summary>
        public static float At(float x, float cy, string[] icons, string[] labels, int cur, string[] badges, Color[] badgeColors, string[] tips, bool draw, Action<int> pick)
        {
            int n = labels.Length;
            var widths = new float[n];
            float total = Pad * 2f + Gap * Mathf.Max(0, n - 1);
            for (int i = 0; i < n; i++)
            {
                widths[i] = CellWidth(icons != null ? icons[i] : null, labels[i], badges != null ? badges[i] : null);
                total += widths[i];
            }
            if (!draw || n == 0) return total;
            float fh = H + Pad * 2f;
            var frame = new Rect(x, Mathf.Round(cy - fh * 0.5f), total, fh);
            if (Draw.Repaint) Draw.Round(frame, Look.Pill, fh * 0.5f);
            int skey = CSCommonAnim.Key(labels[0], labels[n - 1]) ^ n * 7919 ^ 0x7a3c1e5d;
            float ix = frame.x + Pad;
            for (int i = 0; i < n; i++)
            {
                var r = new Rect(ix, frame.y + Pad, widths[i], H);
                Color? bc = badgeColors != null && badgeColors[i] != Pal.Warn ? badgeColors[i] : null;
                if (Cell(r, skey ^ (i + 1) * 0x2f6b8d3, i == cur, icons != null ? icons[i] : null, labels[i], Font, badges != null ? badges[i] : null, bc, tips != null ? tips[i] : null))
                    pick?.Invoke(i);
                ix += widths[i] + Gap;
            }
            return total;
        }

        /// <summary>탭 칸 하나(그리기 + 누름): [아이콘 16 · 8 · 이름 · 8 · 수 알약] 을 칸 가운데(좁으면 왼쪽 8 부터 · 이름 줄임). 눌러 떼면 true.</summary>
        internal static bool Cell(Rect c, int key, bool on, string icon, string label, float font, string badge = null, Color? badgeColor = null, string tip = null)
        {
            float onT = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / FadeSeconds);
            int hk = key ^ 0x51ed270b;
            float hv = Look.Hover(hk, c, !on), pr = Look.Press(hk, c);
            if (Draw.Repaint)
            {
                float rad = c.height * 0.5f;
                Draw.BeginScale(c.center, Look.PressScale(pr, PressScale));
                float ha = hv * (1f - onT);
                if (ha > 0.01f) Draw.Round(c, Draw.Alpha(Look.NeutralHover, ha), rad);
                if (onT > 0.01f) Draw.Round(c, Draw.Alpha(Look.PastelFill, onT), rad);
                Color ink = Color.Lerp(Look.BarOff, Look.PastelInk, onT);
                bool hasIcon = !string.IsNullOrEmpty(icon), hasBadge = !string.IsNullOrEmpty(badge);
                float tw = Mathf.Ceil(Draw.TextWidth(label ?? "", font, true));
                float iw = hasIcon ? Icon + IconGap : 0f, bw = hasBadge ? CountGap + Badge.Count(0f, 0f, badge, false, false, CountH) : 0f;
                float x = Mathf.Max(c.x + IconGap, Mathf.Round(c.center.x - (iw + tw + bw) * 0.5f));
                float lw = Mathf.Max(0f, Mathf.Min(tw, c.xMax - IconGap - x - iw - bw));
                if (hasIcon) Draw.Icon(icon, new Rect(x, Mathf.Round(c.center.y - Icon * 0.5f), Icon, Icon), ink, 24f, Stroke);
                Draw.TextPill(new Rect(x + iw, c.y, lw, c.height), label ?? "", font, ink, on, TextAnchor.MiddleCenter);
                if (hasBadge) Badge.Count(x + iw + lw + CountGap, c.center.y, badge, true, false, CountH, badgeColor);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(c, tip);
            return Draw.Button(c);
        }
    }

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

    /// <summary>
    /// CSCommonUX.Textfield — 알약 글 칸(시안 'CSCommonUX.Textfield'): 높이 40 · 반지름 = 높이 절반 · 안쪽 왼쪽 16 · 오른쪽 8 · 요소 사이 4 · 그림자 없음.
    /// 바탕 Look.Pill, 호버 = 안쪽 1px Look.PillHoverLine, 초점 = 바탕 Look.FieldFocus + 안쪽 1.5px 파스텔 테두리(Look.PastelRing, 커서도 같은 색),
    /// 잘못 = 안쪽 1.5px 빨강(초점이어도 유지), 꺼짐 = Look.Off 바탕 · 글 · 단위 OffText · 호버 · 오류 테두리 없음. 호버 · 초점 전환 0.12초.
    /// 앞 아이콘 16(Dim, 오른쪽 4) · 앞뒤 단위(값이 있을 때만 · 굵은 Sub) · 지우기 ✕(지름 28 원 · 아이콘 14 · 초점이고 값이 있을 때만 · 누름 영역은 칸 오른쪽 끝).
    /// </summary>
    public static class Textfield
    {
        /// <summary>안쪽 왼쪽 · 오른쪽 · 요소 사이 · 앞 아이콘 · 지우기 원 · 지우기 아이콘 · 호버 선 · 초점/오류 선 · 전환 초 · Draw.Field 안쪽 여백(글 시작 맞춤) · 도움말 줄 높이.</summary>
        public const float PadL = 16f, PadR = 8f, Gap = 4f, Icon = 16f, ClearD = 28f, ClearIcon = 14f, HoverW = 1f, RingW = 1.5f, FadeSeconds = 0.12f, FieldInset = 10f, HelpH = 18f;

        /// <summary>글 칸(name = 초점 이름, 없으면 칸 자리로). 바뀐 글을 돌려줌.</summary>
        public static string In(Rect r, string value, int maxLength, string placeholder = null, string icon = null, bool error = false, bool enabled = true, string name = null, float font = 14.5f,
            string prefix = null, string suffix = null, bool clearable = false)
        {
            name ??= "csf" + Look.Key(r, "field");
            value ??= "";
            bool focus = GUI.GetNameOfFocusedControl() == name;
            bool has = value.Length > 0;
            int key = CSCommonAnim.Key("textfield", name);
            float hv = 0f; // 호버 효과 없음
            float fo = CSCommonAnim.Toward(key ^ 0x3b9ac9ff, enabled && focus ? 1f : 0f, 1f / FadeSeconds);
            float rad = r.height * 0.5f;
            if (Draw.Repaint)
            {
                Draw.Round(r, !enabled ? Look.Off : Color.Lerp(Look.Pill, Look.FieldFocus, fo), rad);
                if (!enabled) { }
                else if (error) Draw.RoundOutline(r, Pal.Red, rad, RingW);
                else
                {
                    if (hv * (1f - fo) > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Look.PillHoverLine, hv * (1f - fo)), rad, HoverW);
                    if (fo > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Look.PastelRing, fo), rad, RingW);
                }
            }
            // 글 시작 x(Draw.Field 는 안쪽 FieldInset 만큼 띄워 씀)
            float tx = r.x + PadL;
            if (!string.IsNullOrEmpty(icon))
            {
                if (Draw.Repaint) Draw.Icon(icon, new Rect(tx, Mathf.Round(r.center.y - Icon * 0.5f), Icon, Icon), Pal.Dim, 24f, 2f);
                tx += Icon + Gap + Gap;
            }
            Color unit = enabled ? Pal.Sub : Look.OffText;
            // 앞 단위: 값 앞(값이 비면 숨겨 자리 글과 붙어 읽히지 않게)
            if (has && !string.IsNullOrEmpty(prefix))
            {
                float pw = Mathf.Ceil(Draw.TextWidth(prefix, font, true));
                if (Draw.Repaint) Draw.Text(new Rect(tx, r.y, pw + 2f, r.height), prefix, font, unit, true);
                tx += pw + Gap;
            }
            // 지우기 ✕: 오른쪽 끝 원(초점 · 값이 있을 때)
            bool clear = clearable && enabled && focus && has;
            var clearR = new Rect(r.xMax - PadR - ClearD, Mathf.Round(r.center.y - ClearD * 0.5f), ClearD, ClearD);
            float right = r.xMax - PadR - (clear ? ClearD + Gap : 0f);
            // 뒤 단위: 값 바로 뒤(넘치면 오른쪽 끝에 붙음)
            float sw = has && !string.IsNullOrEmpty(suffix) ? Mathf.Ceil(Draw.TextWidth(suffix, font, true)) : 0f;
            if (sw > 0f) right -= sw + Gap;
            var fr = new Rect(tx - FieldInset, r.y, Mathf.Max(0f, right - tx + FieldInset * 2f), r.height);
            if (sw > 0f && Draw.Repaint)
            {
                float vx = tx + Mathf.Ceil(Draw.TextWidth(value, font)) + Gap;
                Draw.Text(new Rect(Mathf.Min(vx, right + Gap), r.y, sw + 2f, r.height), suffix, font, unit, true);
            }
            if (!enabled)
            {
                if (Draw.Repaint) Draw.TextFit(new Rect(tx, r.y, Mathf.Max(0f, right - tx), r.height), value, font, Look.OffText);
                return value;
            }
            if (clear)
            {
                if (Draw.Repaint) Draw.Icon(Icons.Close, new Rect(Mathf.Round(clearR.center.x - ClearIcon * 0.5f), Mathf.Round(clearR.center.y - ClearIcon * 0.5f), ClearIcon, ClearIcon), Pal.Dim, 24f, 2.2f);
                // 누름 영역 = 원부터 칸 오른쪽 끝까지(칸 높이 전체), 눌림 표시 없음
                if (Draw.Button(new Rect(clearR.x, r.y, r.xMax - clearR.x, r.height))) return "";
            }
            if (!has && !string.IsNullOrEmpty(placeholder) && Draw.Repaint)
                Draw.TextFit(new Rect(tx, r.y, Mathf.Max(0f, right - tx), r.height), placeholder, font, Look.OffText);
            // 커서 색 = 초점 테두리 색(파스텔)
            var cursor = GUI.skin.settings.cursorColor;
            GUI.skin.settings.cursorColor = Look.PastelRing;
            GUI.SetNextControlName(name);
            string v = Draw.Field(fr, value, font, maxLength);
            GUI.skin.settings.cursorColor = cursor;
            return v;
        }

        /// <summary>이름(위, 12 보조색) · 칸(40) · 도움말(아래 12 · 높이 18 고정, 잘못이면 빨강).</summary>
        public static string Labeled(Rect r, string label, string value, int maxLength, string help = null, string placeholder = null, bool error = false, bool enabled = true, string name = null)
        {
            if (Draw.Repaint) Draw.Text(new Rect(r.x, r.y, r.width, 16f), label, CSCommonDesign.FontMin, Pal.Sub);
            var box = new Rect(r.x, r.y + 20f, r.width, CSCommonDesign.ActionHeight);
            string v = In(box, value, maxLength, placeholder, null, error, enabled, name);
            if (!string.IsNullOrEmpty(help) && Draw.Repaint) Draw.TextFit(new Rect(r.x, box.yMax + Gap, r.width, HelpH), help, CSCommonDesign.FontMin, error ? Pal.Red : Pal.Dim);
            return v;
        }
    }

    /// <summary>
    /// CSCommonUX.Select — 목록에서 고르기(시안 'CSCommonUX.Select'): 알약 칸(bg 없으면 Look.Pill · 반지름 = 높이/2 · 왼쪽 16 · 오른쪽 12 · 색 점 8 + 8 · 글 14.5 Look.BarInk · ▾ 16 Section.SubInk).
    /// 누르면 칸 아래 6 에 둥근 목록(반지름 24 · 안쪽 6 · Look.MenuSurface + 1px Look.MenuLine, 그림자 없음 — 다크에서도 바탕에 안 묻힘): 줄 40 · 반지름 20 · 좌우 14 · 사이 10 ·
    /// 왼쪽 표시 18(하나 ○ 고리 1.5 + 점 8 · 여럿 □ 반지름 6 + ✓ 12) · 글 14, 고른 줄 = 모드 파스텔 + 굵은 파스텔 글 · 호버 Look.MenuHover · 누르면 0.95배.
    /// 열린 칸 = 안쪽 1.5px Look.PastelRing + ▾ 180° 돌기(0.22초). 목록은 칸 쪽에서 펼쳐짐(0.24초 Modal.Ease) + 투명 0.16초, 닫을 때 거꾸로(그동안 목록 자리 클릭은 먹음).
    /// 하나 고르기는 고르면 닫힘 · 여럿 고르기(Many)는 열어 둠 · 밖을 누르면 닫힘. Close() 는 움직임 없이 바로 닫음(창을 닫거나 탭 · 스크롤이 바뀔 때).
    /// 목록은 다른 칸보다 위에 그려야 하므로 그 프레임 끝 Flush() 에서 그림(창 · 판 OnGUI 끝에서 부름).
    /// </summary>
    public static class Select
    {
        /// <summary>칸: 왼쪽 · 오른쪽 · 점 · 점 → 글 · 글 · ▾ · ▾ 선 · 열림 테두리 · 누름 배율 · ▾ 도는 초.</summary>
        public const float PadL = 16f, PadR = 12f, Dot = 8f, DotGap = 8f, Font = 14.5f, Chevron = 16f, ChevronStroke = 2.2f, RingW = 1.5f, PressScale = 0.95f, TurnSeconds = 0.22f;
        /// <summary>목록: 칸과 사이 · 반지름 · 최소 폭 · 줄 좌우 · 줄 안 사이 · 줄 글 · 고른 점 · 상자 반지름 · ✓ · ✓ 선.</summary>
        public const float MenuGap = 6f, MenuRadius = 24f, MenuMinW = 200f, RowPadX = 14f, RowGap = 10f, RowFont = 14f, MarkDot = 8f, BoxRadius = 6f, Check = 12f, CheckStroke = 3f;

        static int openKey;
        static Rect openRect;
        static string[] openItems;
        static Color?[] openDots;
        static int openCur;
        static bool[] openOn;
        static string[] openHeads;
        static Action<int> openPick;
        static float openAt;
        /// <summary>열린 칸을 마지막으로 그린 프레임 — 칸이 안 그려지면(창을 닫음) 목록도 닫음.</summary>
        static int openSeenFrame;

        /// <summary>지난 그림의 목록 판 · 줄 자리(바탕 좌표) — 창보다 먼저 클릭을 받는 데 씀(PreInput).</summary>
        static Rect menuRect;
        static readonly List<Rect> itemRects = new();
        static int pressed = -1;

        /// <summary>닫히는 중: 닫기 시작한 때(없으면 −1) · 그때 펼친 정도 · 투명도 · 닫히는 칸 · 그 칸을 마지막으로 그린 프레임.</summary>
        static float closeAt = -1f, closeFrom, closeFade;
        static int closingKey, closingSeen;

        public static bool AnyOpen => openKey != 0;

        static bool Closing => closeAt >= 0f && Time.unscaledTime - closeAt < CSCommonDesign.MenuOpenSeconds;

        /// <summary>열린 목록을 바로 닫음(탭을 바꾸거나 스크롤할 때 — 움직임 없음).</summary>
        public static void Close()
        {
            openKey = 0;
            closeAt = -1f;
            itemRects.Clear();
            pressed = -1;
        }

        /// <summary>사용자가 닫음(고름 · 밖 누름 · 칸 다시 누름): 지금 펼친 정도에서 거꾸로 접히며 사라짐.</summary>
        static void BeginClose()
        {
            float now = Time.unscaledTime, el = now - openAt;
            closeFrom = Modal.Ease(el / CSCommonDesign.MenuOpenSeconds);
            closeFade = Mathf.Clamp01(el / CSCommonDesign.MenuFadeSeconds);
            closeAt = now;
            closingKey = openKey;
            closingSeen = Time.frameCount;
            openKey = 0;
            itemRects.Clear();
            pressed = -1;
        }

        /// <summary>
        /// GUI 차례 맨 앞(Draw.Begin)에서 열린 목록이 먼저 클릭을 받음: 목록 안 누름은 여기서 쓰고(떼면 고름), 밖 누름은 목록을 닫고 아래로 보냄.
        /// 목록은 창 위에 그리지만(Flush) 창이 먼저 그려져 — 목록 아래 카드 단추나 창의 '창 위 클릭은 여기서 끝' 처리가 누름을 먼저 써 버려
        /// 목록 줄을 눌러도 고를 수 없던 것(올림픽 구역 지구 고르기).
        /// </summary>
        internal static void PreInput()
        {
            var e = Event.current;
            if (e == null) return;
            Vector2 m = e.mousePosition / Draw.S;
            if (openKey == 0)
            {
                // 접히는 동안 목록 자리 누름 · 끌기 · 뗌은 여기서 먹음(아래 창 · 지도로 새지 않게, 위로 열린 목록도 같은 자리)
                if (Closing && menuRect.Contains(m) && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag || e.rawType == EventType.MouseUp)) e.Use();
                return;
            }
            if (itemRects.Count == 0) return;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (menuRect.Contains(m))
                {
                    pressed = itemRects.FindIndex(it => it.Contains(m));
                    if (pressed < 0) pressed = -2; // 판 여백 — 쓰기만
                    e.Use();
                }
                else if (!openRect.Contains(m)) BeginClose();
            }
            else if (e.type == EventType.MouseDrag && pressed != -1) e.Use();
            else if (e.rawType == EventType.MouseUp && pressed != -1)
            {
                int i = pressed;
                pressed = -1;
                e.Use();
                if (i >= 0 && i < itemRects.Count && itemRects[i].Contains(m))
                {
                    var pick = openPick;
                    if (openOn == null) BeginClose();
                    pick?.Invoke(i);
                }
            }
        }

        /// <summary>하나 고르기 칸(bg = 닫힌 칸 바탕, 없으면 Look.Pill). heads[i] = 줄 i 위에 둘 묶음 머리(없으면 null).</summary>
        public static void In(Rect r, string text, Color? dot, string[] items, Color?[] dots, int cur, Action<int> pick, bool enabled = true, string tip = null, Color? bg = null, string[] heads = null)
            => Field(r, text, dot, items, dots, cur, null, pick, enabled, tip, bg, heads);

        /// <summary>여럿 고르기 칸: on[i] = 고름, 누르면 toggle(i) · 목록은 열어 둠. text = 칸에 보일 요약(예: '세션 2개').</summary>
        public static void Many(Rect r, string text, string[] items, bool[] on, Action<int> toggle, bool enabled = true, string tip = null, Color? bg = null, string[] heads = null)
            => Field(r, text, null, items, null, -1, on, toggle, enabled, tip, bg, heads);

        static void Field(Rect r, string text, Color? dot, string[] items, Color?[] dots, int cur, bool[] on, Action<int> pick, bool enabled, string tip, Color? bg, string[] heads)
        {
            int key = Look.Key(r, "select", on != null ? "many" : text) | 1;
            bool open = openKey == key;
            if (open) openSeenFrame = Time.frameCount;
            if (key == closingKey) closingSeen = Time.frameCount;
            float turn = CSCommonAnim.Toward(key ^ 0x6a09e667, open ? 1f : 0f, 1f / TurnSeconds);
            float pr = Look.Press(key, r, enabled);
            if (Draw.Repaint)
            {
                float rad = r.height * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                Draw.Round(r, !enabled ? Look.BtnOff : bg ?? Look.Pill, rad);
                if (turn > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Look.PastelRing, turn), rad, RingW);
                float tx = r.x + PadL;
                if (dot.HasValue) { Draw.Dot(new Vector2(tx + Dot * 0.5f, r.center.y), Dot * 0.5f, enabled ? dot.Value : Look.IconOffInk); tx += Dot + DotGap; }
                var cr = new Rect(r.xMax - PadR - Chevron, Mathf.Round(r.center.y - Chevron * 0.5f), Chevron, Chevron);
                Draw.TextPill(new Rect(tx, r.y, Mathf.Max(0f, cr.x - DotGap - tx), r.height), text, Font, enabled ? Look.BarInk : Look.BtnOffInk, false, TextAnchor.MiddleLeft);
                // ▾ 돌기: 시안 곡선(열 때 Ease, 닫을 때 거꾸로)
                float deg = 180f * (open ? Modal.Ease(turn) : 1f - Modal.Ease(1f - turn));
                var m = GUI.matrix;
                if (deg > 0.5f) GUIUtility.RotateAroundPivot(deg, Draw.ToScreen(cr).center);
                Draw.Icon(Icons.Chevron, cr, enabled ? Section.SubInk : Look.BtnOffInk, 24f, ChevronStroke);
                GUI.matrix = m;
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            if (!enabled) return;
            bool hit = Draw.Button(r);
            if (hit && open)
            {
                BeginClose();
                return;
            }
            if (hit)
            {
                closeAt = -1f;
                openKey = key;
                openSeenFrame = Time.frameCount;
                itemRects.Clear();
                pressed = -1;
                openAt = Time.unscaledTime;
                open = true;
            }
            if (open)
            {
                // 같은 프레임 끝에 그릴 목록(자리 · 값 갱신)
                openRect = Draw.ToRoot(r);
                openItems = items;
                openDots = dots;
                openCur = cur;
                openOn = on;
                openHeads = heads;
                openPick = pick;
            }
        }

        /// <summary>열린 목록을 맨 위에 그림 · 하나 고르기는 고르면 닫음 · 밖을 누르면 닫음. 닫히는 중이면 접히는 목록만(누름 없음).</summary>
        public static void Flush()
        {
            // 열린 목록은 맨 위 판 — 툴바 · 독 칸(Draw.Block)에 막히지 않음
            Draw.BeginOverlay();
            try { FlushMenu(); }
            finally { Draw.EndOverlay(); }
        }

        static void FlushMenu()
        {
            if (openItems == null) return;
            bool live = openKey != 0;
            float now = Time.unscaledTime, shown, fade;
            if (live)
            {
                // 목록을 연 칸이 이번(또는 바로 앞) 프레임에 안 그려졌으면 그 창이 닫힌 것 — 목록만 남아 잔상으로 보이던 것(열린 채 X 로 닫기)
                if (Time.frameCount - openSeenFrame > 1) { Close(); return; }
                float el = now - openAt;
                shown = Modal.Ease(el / CSCommonDesign.MenuOpenSeconds);
                fade = Mathf.Clamp01(el / CSCommonDesign.MenuFadeSeconds);
            }
            else
            {
                // 닫히는 중: 연 칸이 안 그려지면(창이 닫힘) 접힘도 그만
                if (!Closing || Time.frameCount - closingSeen > 1) { closeAt = -1f; return; }
                float el = now - closeAt;
                shown = closeFrom * (1f - Modal.Ease(el / CSCommonDesign.MenuOpenSeconds));
                fade = closeFade * (1f - Mathf.Clamp01(el / CSCommonDesign.MenuFadeSeconds));
            }
            float row = CSCommonDesign.MenuRowH, pad = CSCommonDesign.MenuPad, mark = CSCommonDesign.MenuMark;
            bool many = openOn != null;
            // 묶음 머리: 첫 묶음은 머리 줄만, 다음 묶음부터 위 구분선 + 4
            float HeadAt(int i) => openHeads != null && i < openHeads.Length && !string.IsNullOrEmpty(openHeads[i]) ? CSCommonDesign.MenuHeadH + (i > 0 ? 4f : 0f) : 0f;
            float body = 0f;
            for (int i = 0; i < openItems.Length; i++) body += HeadAt(i) + row;
            var menu = new Rect(openRect.x, openRect.yMax + MenuGap, Mathf.Max(openRect.width, MenuMinW), pad * 2f + body);
            bool above = menu.yMax > Draw.Height - 8f;
            if (above) menu.y = openRect.y - MenuGap - menu.height;
            menuRect = menu;
            if (live) itemRects.Clear();
            // 칸 쪽에서 펼쳐짐(아래로 열면 위부터, 위로 열면 아래부터 보임) — 1px 선이 잘리지 않게 바깥 1 을 더 둠
            float sh = Mathf.Round((menu.height + 2f) * shown);
            var clip = new Rect(menu.x - 1f, above ? menu.yMax + 1f - sh : menu.y - 1f, menu.width + 2f, sh);
            float opacity = Draw.Opacity;
            Draw.Opacity *= fade;
            // 안쪽 좌표를 바깥과 같게(scroll = 자른 칸 자리) — 줄 자리(itemRects)는 그대로 바탕 좌표
            Draw.BeginClip(clip, clip.position);
            if (Draw.Repaint)
            {
                Draw.Round(menu, Look.MenuSurface, MenuRadius);
                Draw.RoundOutline(menu, Look.MenuLine, MenuRadius);
            }
            int picked = -1;
            float iy = menu.y + pad;
            for (int i = 0; i < openItems.Length; i++)
            {
                float hh = HeadAt(i);
                if (hh > 0f && Draw.Repaint)
                {
                    if (i > 0) Draw.Fill(new Rect(menu.x + pad + RowPadX, iy + 2f, menu.width - (pad + RowPadX) * 2f, 1f), Look.Divider);
                    Draw.TextFit(new Rect(menu.x + pad + RowPadX, iy + hh - CSCommonDesign.MenuHeadH, menu.width - (pad + RowPadX) * 2f, CSCommonDesign.MenuHeadH), openHeads[i], CSCommonDesign.FontMin, Section.SubInk, true);
                }
                iy += hh;
                var it = new Rect(menu.x + pad, iy, menu.width - pad * 2f, row);
                if (live) itemRects.Add(it);
                iy += row;
                bool on = many ? i < openOn.Length && openOn[i] : i == openCur;
                int k = Look.Key(it, "select.row", openItems[i]);
                float onT = CSCommonAnim.Toward(k, on ? 1f : 0f, 1f / Tabs.FadeSeconds);
                float hv = Look.Hover(k ^ 0x51ed270b, it, live && !on), pr = Look.Press(k ^ 0x51ed270b, it, live);
                if (Draw.Repaint)
                {
                    float rad = row * 0.5f;
                    Draw.BeginScale(it.center, Look.PressScale(pr, PressScale));
                    float ha = hv * (1f - onT);
                    if (ha > 0.01f) Draw.Round(it, Draw.Alpha(Look.MenuHover, ha), rad);
                    if (onT > 0.01f) Draw.Round(it, Draw.Alpha(Look.PastelFill, onT), rad);
                    var m = new Rect(it.x + RowPadX, Mathf.Round(it.center.y - mark * 0.5f), mark, mark);
                    if (many) Box(m, onT);
                    else Radio(m, onT);
                    float tx = m.xMax + RowGap;
                    if (openDots != null && i < openDots.Length && openDots[i].HasValue) { Draw.Dot(new Vector2(tx + Dot * 0.5f, it.center.y), Dot * 0.5f, openDots[i].Value); tx += Dot + RowGap; }
                    Draw.TextFit(new Rect(tx, it.y, it.xMax - tx - RowPadX, it.height), openItems[i], RowFont, Color.Lerp(Look.BarInk, Look.PastelInk, onT), on);
                    Draw.EndScale();
                }
                if (live && picked < 0 && Draw.Button(it)) picked = i;
            }
            Draw.EndClip();
            Draw.Opacity = opacity;
            if (!live) return;
            if (picked >= 0)
            {
                var pick = openPick;
                if (!many) BeginClose();
                pick?.Invoke(picked);
                return;
            }
            var e = Event.current;
            if (e.type == EventType.MouseDown && !menu.Contains(Draw.Mouse) && !openRect.Contains(Draw.Mouse)) BeginClose();
        }

        /// <summary>하나 고르기 표시(t = 고름 0 ~ 1): 1.5 고리(안 고름 Look.BtnOffInk → 고름 Look.PastelInk) + 가운데 점 8.</summary>
        static void Radio(Rect m, float t)
        {
            Draw.Ring(m.center, m.width * 0.5f - RingW * 0.5f, RingW, Color.Lerp(Look.BtnOffInk, Look.PastelInk, t));
            if (t > 0.01f) Draw.Dot(m.center, MarkDot * 0.5f, Draw.Alpha(Look.PastelInk, t));
        }

        /// <summary>여럿 고르기 표시(t = 고름 0 ~ 1): 반지름 6 상자 1.5 선 + ✓ 12.</summary>
        static void Box(Rect m, float t)
        {
            Draw.RoundOutline(m, Color.Lerp(Look.BtnOffInk, Look.PastelInk, t), BoxRadius, RingW);
            if (t > 0.01f) Draw.Icon(Icons.Done, new Rect(Mathf.Round(m.center.x - Check * 0.5f), Mathf.Round(m.center.y - Check * 0.5f), Check, Check), Draw.Alpha(Look.PastelInk, t), 24f, CheckStroke);
        }
    }

    /// <summary>
    /// CSCommonUX.Stepper — − 값 +(시안 'CSCommonUX.Stepper'): 알약 홈(Look.Pill · 안쪽 4 · 사이 4 · 반지름 = 높이/2) 안에 원 단추 둘(Look.PillNeutral 면 · Look.PillNeutralInk − + 18)
    /// · 가운데 값(14 굵게 Look.BarInk). 끝이면 그쪽 단추 꺼짐(동작 null — 면 없이 Look.IconOffInk). 누르면 0.95배. 시안 값 칸 최소 폭 76.
    /// </summary>
    public static class Stepper
    {
        public const float Pad = 4f, Gap = 4f, Icon = 18f, Stroke = 2f, Font = 14f, ValueMinW = 76f, PressScale = 0.95f;

        public static void In(Rect r, string value, Action minus, Action plus, string minusTip = null, string plusTip = null)
        {
            if (Draw.Repaint) Draw.Round(r, Look.Pill, r.height * 0.5f);
            float s = r.height - Pad * 2f;
            if (Round(new Rect(r.x + Pad, r.y + Pad, s, s), Icons.Minus, minusTip, minus != null)) minus?.Invoke();
            if (Draw.Repaint) Draw.TextPill(new Rect(r.x + Pad + s + Gap, r.y, Mathf.Max(0f, r.width - (Pad + s + Gap) * 2f), r.height), value, Font, Look.BarInk, true, TextAnchor.MiddleCenter);
            if (Round(new Rect(r.xMax - Pad - s, r.y + Pad, s, s), Icons.Plus, plusTip, plus != null)) plus?.Invoke();
        }

        /// <summary>원 단추(켜짐 = 회색 원 · 꺼짐 = 면 없이 흐린 아이콘). 누르면 true.</summary>
        static bool Round(Rect b, string icon, string tip, bool enabled)
        {
            int key = Look.Key(b, "stepper", icon);
            float pr = Look.Press(key, b, enabled);
            if (Draw.Repaint)
            {
                Draw.BeginScale(b.center, Look.PressScale(pr, PressScale));
                if (enabled) Draw.Round(b, Look.PillRaised, b.height * 0.5f);
                Draw.Icon(icon, new Rect(Mathf.Round(b.center.x - Icon * 0.5f), Mathf.Round(b.center.y - Icon * 0.5f), Icon, Icon), enabled ? Look.PillNeutralInk : Look.IconOffInk, 24f, Stroke);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(b, tip);
            return enabled && Draw.Button(b);
        }
    }

    /// <summary>
    /// CSCommonUX.Switch — 켜고 끄기(시안 'CSCommonUX.Switch', 48 × 28 알약 = CSCommonDesign.SwitchW · SwitchH): 꺼짐 = Look.PillNeutral 홈 + Look.KnobOff 손잡이,
    /// 켜짐 = 모드 파스텔 홈 + Look.PastelInk 손잡이, 쓸 수 없음 = Look.BtnOff 홈 + Look.IconOffInk 손잡이. 손잡이 22(안쪽 3)는 0.24초 cubic-bezier(.2,.9,.3,1.2)로
    /// 살짝 넘쳤다 자리에 앉고 색은 0.22초, 누르면 0.95배. 누르면 true.
    /// </summary>
    public static class Switch
    {
        public const float Inset = 3f, MoveSeconds = 0.24f, FadeSeconds = 0.22f, PressScale = 0.95f;
        /// <summary>손잡이 곡선 cubic-bezier(.2, .9, .3, 1.2).</summary>
        const float X1 = 0.2f, Y1 = 0.9f, X2 = 0.3f, Y2 = 1.2f;

        public static bool In(Rect r, bool on, bool enabled = true)
        {
            int key = Look.Key(r, "switch");
            float c = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / FadeSeconds);
            float mv = CSCommonAnim.Toward(key ^ 0x3c6ef372, on ? 1f : 0f, 1f / MoveSeconds);
            float pr = Look.Press(key ^ 0x1b873593, r, enabled);
            if (Draw.Repaint)
            {
                float k = r.height - Inset * 2f;
                // 켤 때 곡선 그대로, 끌 때 거꾸로(왼쪽에서도 살짝 넘침)
                float t = on ? Modal.Ease(mv, X1, Y1, X2, Y2) : 1f - Modal.Ease(1f - mv, X1, Y1, X2, Y2);
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                Draw.Round(r, !enabled ? Look.BtnOff : Color.Lerp(Look.PillNeutral, Look.PastelFill, c), r.height * 0.5f);
                float cx = Mathf.LerpUnclamped(r.x + Inset + k * 0.5f, r.xMax - Inset - k * 0.5f, t);
                Draw.Dot(new Vector2(cx, r.center.y), k * 0.5f, !enabled ? Look.IconOffInk : Color.Lerp(Look.KnobOff, Look.PastelInk, c));
                Draw.EndScale();
            }
            return enabled && Draw.Button(r);
        }
    }

    /// <summary>
    /// CSCommonUX.Slider — 끌기 막대(시안 'CSCommonUX.Slider'): 막대 4(반지름 2 · 빈 쪽 Look.PillLine · 채운 쪽 fill — 보통 Look.PastelStrong) · 손잡이 16 Look.Knob 원 + fill 2px 테두리(그림자 없음),
    /// 호버 18 + fill 18% 고리 6, 끄는 중 고리 8(28%) + 값 말풍선(bubble). 12단계 이하는 2 × 2 눈금(채운 쪽 Look.Knob · 빈 쪽 Look.KnobOff), 꺼짐 40%.
    /// 손잡이 크기는 0.1초로 바뀜. 호버 중 ← → 키로 한 단계. 손을 떼면 released.
    /// </summary>
    public static class Slider
    {
        const float TrackH = 4f, Knob = 8f, KnobHover = 9f, Border = 2f, RingHover = 6f, RingDrag = 8f, Tick = 2f, MaxTicks = 12f;
        const float OffAlpha = 0.4f, HoverAlpha = 0.18f, DragAlpha = 0.28f, GrowSeconds = 0.1f;

        static bool sliding;
        static int slideKey;

        /// <summary>끄는 중(지도 도구가 클릭을 가져가지 않게).</summary>
        public static bool Sliding => sliding && Input.GetMouseButton(0);

        public static float In(Rect r, float value, float min, float max, float step, Color fill, out bool released) =>
            In(r, value, min, max, step, fill, out released, null);

        /// <summary>bubble = 끄는 중 손잡이 위 말풍선 글(null 이면 없음), enabled = false 면 흐리게 · 입력 없음.</summary>
        public static float In(Rect r, float value, float min, float max, float step, Color fill, out bool released, string bubble, bool enabled = true)
        {
            released = false;
            int key;
            unchecked { key = Mathf.RoundToInt(r.x) * 397 ^ Mathf.RoundToInt(r.y); }
            float Snap(float v)
            {
                v = Mathf.Clamp(v, Mathf.Min(min, max), Mathf.Max(min, max));
                return step > 0f ? Mathf.Clamp(min + Mathf.Round((v - min) / step) * step, Mathf.Min(min, max), Mathf.Max(min, max)) : v;
            }
            float At() => Snap(Mathf.Lerp(min, max, Mathf.Clamp01((Draw.Mouse.x - r.x) / r.width)));
            Event e = Event.current;
            var hit = new Rect(r.x - 8f, r.y - 8f, r.width + 16f, r.height + 16f);
            bool hover = enabled && Draw.Hover(hit);
            if (sliding && slideKey == key && !Input.GetMouseButton(0)) { sliding = false; released = true; }
            if (enabled)
            {
                switch (e.type)
                {
                    case EventType.MouseDown:
                        if (e.button == 0 && hover)
                        {
                            sliding = true;
                            slideKey = key;
                            value = At();
                            e.Use();
                        }
                        break;
                    case EventType.MouseDrag:
                        if (sliding && slideKey == key) { value = At(); e.Use(); }
                        break;
                    case EventType.MouseUp:
                        if (sliding && slideKey == key && e.button == 0) { sliding = false; released = true; e.Use(); }
                        break;
                    case EventType.KeyDown:
                        if (hover && (e.keyCode == KeyCode.LeftArrow || e.keyCode == KeyCode.RightArrow))
                        {
                            float d = step > 0f ? step : (max - min) / 100f;
                            value = Snap(value + (e.keyCode == KeyCode.RightArrow ? d : -d));
                            released = true;
                            e.Use();
                        }
                        break;
                }
            }
            bool drag = sliding && slideKey == key;
            float grow = CSCommonAnim.Toward(key ^ 0x2545f491, drag ? 1f : 0f, 1f / GrowSeconds);
            if (!Draw.Repaint) return value;

            float old = Draw.Opacity;
            if (!enabled) Draw.Opacity = old * OffAlpha;
            float t = max > min ? Mathf.Clamp01((value - min) / (max - min)) : 0f;
            var track = new Rect(r.x, Mathf.Round(r.center.y - TrackH * 0.5f), r.width, TrackH);
            Draw.Round(track, Look.PillLine, TrackH * 0.5f);
            if (t > 0f) Draw.Round(new Rect(track.x, track.y, Mathf.Max(TrackH, track.width * t), TrackH), fill, TrackH * 0.5f);
            // 단계 눈금(12단계 이하): 2 × 2, 채운 쪽은 손잡이 색 · 빈 쪽은 흐린 회색
            if (step > 0f && max > min && (max - min) / step <= MaxTicks)
            {
                int n = Mathf.RoundToInt((max - min) / step);
                for (int i = 0; i <= n; i++)
                {
                    float tt = i / (float)n;
                    var c = new Rect(Mathf.Round(track.x + track.width * tt - Tick * 0.5f), Mathf.Round(track.center.y - Tick * 0.5f), Tick, Tick);
                    Draw.Fill(c, tt <= t + 0.001f ? Look.Knob : Look.KnobOff);
                }
            }
            var knob = new Vector2(Mathf.Round(track.x + track.width * t), track.center.y);
            float rad = Mathf.Lerp(Knob, KnobHover, grow);
            if (drag) Draw.Dot(knob, rad + RingDrag, Draw.Alpha(fill, DragAlpha));
            else if (grow > 0.01f) Draw.Dot(knob, rad + RingHover, Draw.Alpha(fill, HoverAlpha * grow));
            Draw.Dot(knob, rad, fill);
            Draw.Dot(knob, rad - Border, Look.Knob);
            if (drag && !string.IsNullOrEmpty(bubble))
            {
                float bw = Draw.TextWidth(bubble, 12f, true) + 16f;
                var b = new Rect(Mathf.Round(knob.x - bw * 0.5f), Mathf.Round(knob.y - rad - RingDrag - 22f), bw, 20f);
                Draw.Round(b, fill, b.height * 0.5f);
                Draw.Text(b, bubble, 12f, Draw.On(fill), true, TextAnchor.MiddleCenter);
            }
            Draw.Opacity = old;
            return value;
        }
    }

    /// <summary>
    /// CSCommonUX.Swatch — 색 칸(시안 'CSCommonUX.Swatch', 원 30): 평소 안쪽 1px Look.SwatchLine, 고르면 이중 고리(판 바탕 Pal.Win 2px + Look.BarInk 2px, 0.16초) ·
    /// 누르면 0.95배. 누르면 true.
    /// </summary>
    public static class Swatch
    {
        public const float Ring = 2f, FadeSeconds = 0.16f, PressScale = 0.95f;

        public static bool In(Rect r, Color color, bool on, string tip = null)
        {
            int key = Look.Key(r, "swatch");
            float onT = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / FadeSeconds);
            float pr = Look.Press(key, r);
            if (Draw.Repaint)
            {
                float rad = Mathf.Min(r.width, r.height) * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                if (onT > 0.01f)
                {
                    Draw.Round(new Rect(r.x - Ring * 2f, r.y - Ring * 2f, r.width + Ring * 4f, r.height + Ring * 4f), Draw.Alpha(Look.BarInk, onT), rad + Ring * 2f);
                    Draw.Round(new Rect(r.x - Ring, r.y - Ring, r.width + Ring * 2f, r.height + Ring * 2f), Draw.Alpha(Pal.Win, onT), rad + Ring);
                }
                Draw.Round(r, color, rad);
                if (onT < 0.99f) Draw.RoundOutline(r, Draw.Alpha(Look.SwatchLine, 1f - onT), rad);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            return Draw.Button(r);
        }
    }

    /// <summary>
    /// CSCommonUX.Stat — 숫자 칸(시안: 이름 12 Look.PillSub · 큰 숫자 24 굵게(줄 30) · 설명 12 Look.PillSub, 들임 없음 · 칸 세로 가운데) ·
    /// 숫자 줄 Line(시안 높이 26: 이름 12.5 Section.SubInk 왼쪽 · 값 굵게 Look.BarInk 오른쪽).
    /// </summary>
    public static class Stat
    {
        public const float LabelH = 16f, ValueH = 30f, ValueFont = 24f, LineFont = 12.5f, LineH = 26f;

        public static void In(Rect r, string label, string value, string sub = null, Color? color = null)
        {
            if (!Draw.Repaint) return;
            // 세 줄(이름 16 · 숫자 30 · 설명 16)을 칸 세로 가운데로
            bool hasSub = !string.IsNullOrEmpty(sub);
            float y = r.y + Mathf.Max(0f, Mathf.Round((r.height - LabelH - ValueH - (hasSub ? LabelH : 0f)) * 0.5f));
            Draw.TextFit(new Rect(r.x, y, r.width, LabelH), label, CSCommonDesign.FontMin, Look.PillSub);
            Draw.TextFit(new Rect(r.x, y + LabelH, r.width, ValueH), value, ValueFont, color ?? Look.BarInk, true);
            if (hasSub) Draw.TextFit(new Rect(r.x, y + LabelH + ValueH, r.width, LabelH), sub, CSCommonDesign.FontMin, Look.PillSub);
        }

        public static void Line(Rect r, string label, string value, Color? color = null)
        {
            if (!Draw.Repaint) return;
            Draw.Text(new Rect(r.x, r.y, r.width * 0.5f, r.height), label, LineFont, Section.SubInk, false, TextAnchor.MiddleLeft);
            Draw.TextFit(new Rect(r.x + r.width * 0.4f, r.y, r.width * 0.6f, r.height), value, LineFont, color ?? Look.BarInk, true, TextAnchor.MiddleRight);
        }
    }

    /// <summary>
    /// CSCommonUX.Metric — GLDS 2.1 지표 칸: 타일 면 없이 값 15 굵게 위 · 라벨 12 Sub 아래(설명은 그 아래 12 Dim), 왼쪽 정렬 · 들임 없음.
    /// 격자는 Grid: 3열(4개면 2열) · 행 간격 MetricRowGap.
    /// </summary>
    public static class Metric
    {
        public static float Height(bool sub) => CSCommonDesign.MetricValueH + CSCommonDesign.MetricLabelH + (sub ? CSCommonDesign.MetricLabelH : 0f);

        public static void In(Rect r, string value, string label, string sub = null, Color? color = null)
        {
            if (!Draw.Repaint) return;
            float y = r.y;
            Draw.TextFit(new Rect(r.x, y, r.width, CSCommonDesign.MetricValueH), value, CSCommonDesign.FontValue, color ?? Pal.Txt, true);
            y += CSCommonDesign.MetricValueH;
            Draw.TextFit(new Rect(r.x, y, r.width, CSCommonDesign.MetricLabelH), label, CSCommonDesign.FontMin, Pal.Sub);
            if (!string.IsNullOrEmpty(sub))
                Draw.TextFit(new Rect(r.x, y + CSCommonDesign.MetricLabelH, r.width, CSCommonDesign.MetricLabelH), sub, CSCommonDesign.FontMin, Pal.Dim);
        }

        /// <summary>열 수: 4개면 2열, 그 밖 3열(좁으면 2열).</summary>
        public static int Columns(int count, float width) => count == 4 || width < 420f ? 2 : 3;

        public static float GridHeight(int count, bool sub, float width)
        {
            if (count == 0) return 0f;
            int rows = Mathf.CeilToInt(count / (float)Columns(count, width));
            return rows * Height(sub) + (rows - 1) * CSCommonDesign.MetricRowGap;
        }
    }

    /// <summary>
    /// CSCommonUX.ShareRow — 비중 줄(시안): 색 점 8 + 8 · 이름 14 Look.BarInk · 값 15 굵게 · 비율 12 Section.SubInk(폭 40 오른쪽) 사이 8,
    /// 6 아래 막대 8(반지름 4 · Look.PillNeutral 홈 · 채움 color, 없으면 Look.PastelStrong). r 높이 = ShareRowH. share = 0 ~ 1, ratio 를 주면 비율 글 대신 그 글.
    /// </summary>
    public static class ShareRow
    {
        public const float LineH = 20f, BarGap = 6f, Bar = 8f, Dot = 8f, Gap = 8f, RatioW = 40f, NameFont = 14f, ValueFont = 15f;

        public static void In(Rect r, string name, string value, float share, Color? color = null, string ratio = null)
        {
            if (!Draw.Repaint) return;
            share = Mathf.Clamp01(share);
            float top = r.y + CSCommonDesign.ShareRowPad, tx = r.x;
            if (color.HasValue)
            {
                Draw.Dot(new Vector2(tx + Dot * 0.5f, top + LineH * 0.5f), Dot * 0.5f, color.Value);
                tx += Dot + Gap;
            }
            float vw = Mathf.Ceil(Draw.TextWidth(value, ValueFont, true)), vx = r.xMax - RatioW - Gap - vw;
            Draw.TextFit(new Rect(tx, top, Mathf.Max(0f, vx - Gap - tx), LineH), name, NameFont, Look.BarInk);
            Draw.Text(new Rect(vx, top, vw + 2f, LineH), value, ValueFont, Look.BarInk, true, TextAnchor.MiddleLeft);
            Draw.TextFit(new Rect(r.xMax - RatioW, top, RatioW, LineH), ratio ?? Mathf.RoundToInt(share * 100f) + "%", CSCommonDesign.FontMin, Section.SubInk, false, TextAnchor.MiddleRight);
            var bar = new Rect(r.x, top + LineH + BarGap, r.width, Bar);
            Draw.Round(bar, Look.PillNeutral, Bar * 0.5f);
            if (share > 0f) Draw.Round(new Rect(bar.x, bar.y, Mathf.Max(Bar, bar.width * share), Bar), color ?? Look.PastelStrong, Bar * 0.5f);
        }
    }

    /// <summary>
    /// CSCommonUX.Progress — 막대 8(시안): 반지름 4 · Look.PillNeutral 홈, 목표 선(2 × 16 · Look.BarInk), 넘으면 Look.PastelStrong · 못 미치면 Look.ProgressLow(color 를 주면 그 색).
    /// 채움 폭은 값이 바뀌면 부드럽게 따라감(시안 0.4초 cubic-bezier(.2,.8,.2,1) 어림 — CSCommonAnim.Smooth).
    /// </summary>
    public static class Progress
    {
        public const float Bar = 8f, GoalW = 2f, GoalH = 16f, Rate = 8f;

        public static void In(Rect r, float t, float goal = -1f, Color? color = null)
        {
            t = Mathf.Clamp01(t);
            var bar = new Rect(r.x, Mathf.Round(r.center.y - Bar * 0.5f), r.width, Bar);
            float w = CSCommonAnim.Smooth(Look.Key(r, "progress"), bar.width * t, Rate);
            if (!Draw.Repaint) return;
            Draw.Round(bar, Look.PillNeutral, Bar * 0.5f);
            Color c = color ?? (goal < 0f || t >= goal ? Look.PastelStrong : Look.ProgressLow);
            if (w > 0.5f) Draw.Round(new Rect(bar.x, bar.y, Mathf.Max(Bar, w), Bar), c, Bar * 0.5f);
            if (goal >= 0f)
                Draw.Round(new Rect(Mathf.Round(bar.x + bar.width * Mathf.Clamp01(goal) - GoalW * 0.5f), Mathf.Round(bar.center.y - GoalH * 0.5f), GoalW, GoalH), Look.BarInk, GoalW * 0.5f);
        }
    }

    /// <summary>
    /// CSCommonUX.Spinner — 구글 머티리얼 서클 인디케이터(Google Circular Progress Indicator):
    /// 호가 16° ↔ 286° 로 늘었다 줄어들며 회전하는 둥근 끝(round cap) 원형 로딩 인디케이터.
    /// </summary>
    public static class Spinner
    {
        public static void At(Vector2 center, float size = 20f, float stroke = 2.2f, Color? color = null)
        {
            if (Draw.Repaint) Draw.Spinner(center, Mathf.Max(2f, (size - stroke) * 0.5f), stroke, color ?? Pal.Primary);
        }

        public static void In(Rect r, Color? color = null, float stroke = 2.2f)
        {
            float size = Mathf.Min(r.width, r.height);
            At(r.center, size, stroke, color);
        }
    }

    /// <summary>
    /// CSCommonUX.Tooltip — 짙은 알약 말풍선(시안): Look.TipFill 면 · 반지름 14 · 안쪽 위 10 · 아래 11 · 좌우 14 · 글 12.5 Look.TipInk · 줄바꿈 최대 폭 300,
    /// 가리키는 칸과 10 떨어짐 · 꼬리 높이 5(왼쪽 끝에서 24 안) · 그림자 없음. 0.4초 머물면 뜸(Delay — CSCommonToolbar.DrawTip).
    /// </summary>
    public static class Tooltip
    {
        public const float MaxW = 300f, PadX = 14f, PadTop = 10f, PadBottom = 11f, Radius = 14f, Font = 12.5f, Gap = 10f, Tail = 5f, TailInset = 24f, Delay = 0.4f;

        /// <summary>anchor(가리키는 칸) 아래에(화면 밖이면 위로) 말풍선을 그림.</summary>
        public static void Draw(Rect anchor, string text)
        {
            if (!CSCommonUX.Draw.Repaint || string.IsNullOrEmpty(text)) return;
            float tw = Mathf.Min(MaxW - PadX * 2f, CSCommonUX.Draw.TextWidth(text, Font) + 2f);
            float th = CSCommonUX.Draw.TextHeight(text, Font, tw);
            float w = tw + PadX * 2f, h = th + PadTop + PadBottom;
            float x = Mathf.Clamp(anchor.center.x - w * 0.5f, 8f, CSCommonUX.Draw.Width - w - 8f);
            bool above = anchor.yMax + Gap + h > CSCommonUX.Draw.Height - 8f;
            float y = above ? anchor.y - Gap - h : anchor.yMax + Gap;
            var r = new Rect(x, y, w, h);
            CSCommonUX.Draw.Round(r, Look.TipFill, Mathf.Min(Radius, h * 0.5f));
            float tx = Mathf.Clamp(anchor.center.x, r.x + TailInset, r.xMax - TailInset);
            // 꼬리: 가리키는 쪽을 향한 세모(1px 줄을 쌓아 그림)
            for (int i = 0; i < Tail; i++)
                CSCommonUX.Draw.Fill(new Rect(tx - (i + 1), above ? r.yMax + Tail - 1f - i : r.y - Tail + i, 2f * (i + 1), 1f), Look.TipFill);
            CSCommonUX.Draw.Text(new Rect(r.x + PadX, r.y + PadTop, tw, th), text, Font, Look.TipInk, false, TextAnchor.UpperLeft, true);
        }
    }

    /// <summary>
    /// 독 안내 알약(Dockbar 내부 부품 — 이름 미정, 후보 DockHint): 높이 34 · 반지름 17 · 좌우 16 · 글 13, 굵은 머리(문구의 첫 세 칸 띄움 앞, Look.BarInk) + 6 + 설명(Look.BarOff).
    /// 판은 PillBar.Plate(불투명 + 1px 선, 그림자 없음).
    /// </summary>
    internal static class DockHint
    {
        public const float Height = 34f, PadX = 16f, HeadGap = 6f, Font = 13f;

        public static Rect At(float centerX, float y, string text)
        {
            string head = null, rest = text ?? "";
            int cut = rest.IndexOf("   ", StringComparison.Ordinal);
            if (cut > 0) { head = rest.Substring(0, cut); rest = rest.Substring(cut + 3); }
            float hw = head != null ? Mathf.Ceil(Draw.TextWidth(head, Font, true)) + HeadGap : 0f;
            float w = Mathf.Min(Draw.Width - 64f, hw + Mathf.Ceil(Draw.TextWidth(rest, Font)) + PadX * 2f);
            var r = new Rect(PillBar.Left(centerX, w), y, w, Height);
            if (!Draw.Repaint) return r;
            PillBar.Plate(r);
            float tx = r.x + PadX;
            if (head != null) { Draw.Text(new Rect(tx, r.y, hw, Height), head, Font, Look.BarInk, true, TextAnchor.MiddleLeft); tx += hw; }
            Draw.TextFit(new Rect(tx, r.y, r.xMax - tx - PadX, Height), rest, Font, Look.BarOff);
            return r;
        }
    }

    /// <summary>토스트 상태: 완료(청록 ✓) · 안내(회색 i) · 오류(빨강 !).</summary>
    public enum ToastKind
    {
        Success,
        Info,
        Error,
    }

    /// <summary>
    /// CSCommonUX.Toast — 작업 알림(시안 'CSCommonUX.Toast'): 알약 60(반지름 30 · Section.Plate 면 + 1px Look.PillLine, 그림자 없음) · 왼쪽 12 ·
    /// 상태 원 36(완료 = 모드 파스텔 · 안내 = Look.PillNeutral · 오류 = Look.FabCancel*) · 12 · 제목 14 굵게 Look.BarInk + 3 + 설명 12 Section.SubInk(문구의 첫 ' — ' 뒤) ·
    /// 오른쪽 작은 주 단추 34(되돌리기 등, 앞 18) · 오른쪽 안쪽 단추 있으면 12 · 없으면 28.
    /// 8 아래에서 올라오며 0.26초(Modal.Ease) 나타남 · 끝나기 0.18초 전부터 사라짐(age · life 를 넘기면).
    /// </summary>
    public static class Toast
    {
        const string SuccessPath = "M5 12.5L9.5 17L19 7.5", InfoPath = "M12 11V18M12 6.5h.01", ErrorPath = "M12 5.5V13.5M12 18h.01";

        /// <summary>문구의 첫 ' — ' 앞 = 제목, 뒤 = 설명(없으면 null).</summary>
        static void Split(string text, out string title, out string desc)
        {
            text ??= "";
            int cut = text.IndexOf(" — ", StringComparison.Ordinal);
            title = cut > 0 ? text.Substring(0, cut) : text;
            desc = cut > 0 ? text.Substring(cut + 3) : null;
        }

        public static float Width(string text, string action = null, string actionIcon = null)
        {
            Split(text, out string title, out string desc);
            float tw = Mathf.Max(Mathf.Ceil(Draw.TextWidth(title, 14f, true)), desc != null ? Mathf.Ceil(Draw.TextWidth(desc, 12f)) : 0f);
            float w = CSCommonDesign.ToastPadL + CSCommonDesign.ToastCircle + CSCommonDesign.ToastGap + tw + PadR(action);
            if (!string.IsNullOrEmpty(action)) w += CSCommonDesign.ToastGap + CSCommonDesign.ToastActionGap + Button.Width(actionIcon, action, true);
            return Mathf.Min(w, CSCommonDesign.ToastMaxW);
        }

        /// <summary>오른쪽 안쪽 여백: 단추가 있으면 12 · 없으면 28(시안 — 글만 있는 토스트가 오른쪽에 붙어 보이지 않게).</summary>
        static float PadR(string action) => string.IsNullOrEmpty(action) ? CSCommonDesign.ToastPadR : CSCommonDesign.ToastPadRAction;

        // ---- 한 번에 하나(GLDS 토스트 자리): Show 로 띄우면 바꿔 끼우고 시간을 다시 잼, Flush 가 화면 아래 16 · 겹치는 떠 있는 것(Avoid) 위 8 에 그림
        static string showText;
        static ToastKind showKind;
        static string showAction, showIcon;
        static Action showClick;
        static float showAt = -100f, showLife;
        static readonly List<Rect> avoid = new();

        /// <summary>지금 그린 토스트 자리(그 위 클릭은 지도로 보내지 않음). 없으면 빈 칸.</summary>
        public static Rect Bounds { get; private set; }

        /// <summary>토스트를 띄움(앞의 것은 바꿔 끼움). life 0 = 기본(단추가 있으면 ToastLifeAction · 없으면 ToastLife).</summary>
        public static void Show(string text, ToastKind kind = ToastKind.Info, string action = null, string actionIcon = null, Action click = null, float life = 0f)
        {
            showText = text ?? "";
            showKind = kind;
            showAction = action;
            showIcon = actionIcon;
            showClick = click;
            showAt = Time.realtimeSinceStartup;
            showLife = life > 0f ? life : !string.IsNullOrEmpty(action) ? CSCommonDesign.ToastLifeAction : CSCommonDesign.ToastLife;
        }

        public static void Hide() => showAt = -100f;

        /// <summary>이번 프레임 토스트가 비켜 갈 떠 있는 것(독 · 안내 한 줄 · 판). Flush 전에 부름.</summary>
        public static void Avoid(Rect r)
        {
            if (r.width > 0f && r.height > 0f) avoid.Add(r);
        }

        /// <summary>지금 토스트를 그림(OnGUI 끝 · 다른 판보다 뒤에서 부름).</summary>
        public static void Flush()
        {
            Bounds = default;
            float age = Time.realtimeSinceStartup - showAt;
            if (string.IsNullOrEmpty(showText) || age >= showLife)
            {
                avoid.Clear();
                return;
            }
            float w = Width(showText, showAction, showIcon), h = CSCommonDesign.ToastHeight;
            float x = Mathf.Round((Draw.Width - w) * 0.5f), y = Draw.Height - CSCommonDesign.ToastSafeGap - h;
            // 겹치면 그 위로 — 올린 자리에서 다시 봐서 쌓인 것들 중 가장 높은 것 위
            for (int pass = 0; pass < 8; pass++)
            {
                bool moved = false;
                foreach (var a in avoid)
                    if (a.xMax > x && a.x < x + w && a.yMax > y && a.y < y + h)
                    {
                        y = a.y - CSCommonDesign.FloatingGap - h;
                        moved = true;
                    }
                if (!moved) break;
            }
            avoid.Clear();
            var r = new Rect(x, Mathf.Round(y), w, h);
            Bounds = r;
            var click = showClick;
            In(r, showText, showKind, showAction, showIcon, () =>
            {
                showAt = -100f;
                click?.Invoke();
            }, age, showLife);
        }

        /// <summary>r 높이 = ToastHeight. age = 뜬 뒤 초 · life = 보일 초(넘기지 않으면 움직임 없이 그대로).</summary>
        public static void In(Rect r, string text, ToastKind kind, string action = null, string actionIcon = null, Action click = null, float age = float.MaxValue, float life = float.MaxValue)
        {
            float show = Modal.Ease(age / CSCommonDesign.ToastInSeconds);
            float fade = life < float.MaxValue ? Mathf.Min(show, Mathf.Clamp01((life - age) / CSCommonDesign.ToastOutSeconds)) : show;
            if (fade <= 0f) return;
            float opacity = Draw.Opacity;
            var shift = new Vector2(0f, Mathf.Round((1f - show) * CSCommonDesign.ToastRise));
            Draw.Opacity *= fade;
            Draw.Shift(shift);
            Split(text, out string title, out string desc);
            float bw = string.IsNullOrEmpty(action) ? 0f : Button.Width(actionIcon, action, true), padR = PadR(action);
            if (Draw.Repaint)
            {
                float rad = r.height * 0.5f;
                Draw.Round(r, Section.Plate, rad);
                Draw.RoundOutline(r, Look.PillLine, rad);
                float d = CSCommonDesign.ToastCircle, isz = CSCommonDesign.ToastIcon;
                var c = new Vector2(r.x + CSCommonDesign.ToastPadL + d * 0.5f, r.center.y);
                Color fill = kind == ToastKind.Success ? Look.PastelFill : kind == ToastKind.Error ? Look.FabCancelFill : Look.PillNeutral;
                Color ink = kind == ToastKind.Success ? Look.PastelInk : kind == ToastKind.Error ? Look.FabCancelInk : Look.PillNeutralInk;
                Draw.Dot(c, d * 0.5f, fill);
                Draw.Icon(kind == ToastKind.Success ? SuccessPath : kind == ToastKind.Error ? ErrorPath : InfoPath, new Rect(c.x - isz * 0.5f, c.y - isz * 0.5f, isz, isz), ink, 24f, 2.4f);
                float tx = r.x + CSCommonDesign.ToastPadL + d + CSCommonDesign.ToastGap;
                float tw = r.xMax - tx - padR - (bw > 0f ? bw + CSCommonDesign.ToastGap + CSCommonDesign.ToastActionGap : 0f);
                if (desc == null) Draw.TextFit(new Rect(tx, r.y, tw, r.height), title, 14f, Look.BarInk, true);
                else
                {
                    // 제목 18 + 사이 3 + 설명 16 을 가운데로
                    float top = Mathf.Round(r.center.y - 18.5f);
                    Draw.TextFit(new Rect(tx, top, tw, 18f), title, 14f, Look.BarInk, true);
                    Draw.TextFit(new Rect(tx, top + 21f, tw, 16f), desc, 12f, Section.SubInk);
                }
            }
            if (bw > 0f)
                Button.In(new Rect(r.xMax - padR - bw, Mathf.Round(r.center.y - CSCommonDesign.ToastActionH * 0.5f), bw, CSCommonDesign.ToastActionH), actionIcon, action, null, Tone.Primary, null, true, click);
            Draw.Shift(-shift);
            Draw.Opacity = opacity;
        }
    }

    /// <summary>
    /// CSCommonUX.Card — 선 카드(시안 'CSCommonUX.Card', 떠 있는 판 안에서만): 반지름 20 · Pal.Card 면 + 안쪽 1px Look.PillLine, 그림자 없음.
    /// 제목 16 굵게 Look.BarInk · 오른쪽 보조 12 Section.SubInk. 안 여백은 CSCommonDesign.Card*. 창 띠 안 묶음은 Section.Frame(플레이트 24) — 카드는 떠 있는 판 안 묶음.
    /// </summary>
    public static class Card
    {
        public const float Radius = 20f, TitleFont = 16f, SubFont = 12f;

        public static void Frame(Rect r, Color? fill = null)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, fill ?? Pal.Card, Radius);
            Draw.RoundOutline(r, Look.PillLine, Radius);
        }

        /// <summary>제목 줄: 제목 왼쪽 · 보조 글 오른쪽(r 의 45% 까지).</summary>
        public static void Title(Rect r, string title, string sub = null)
        {
            if (!Draw.Repaint) return;
            float sw = string.IsNullOrEmpty(sub) ? 0f : Mathf.Min(r.width * 0.45f, Draw.TextWidth(sub, SubFont) + 8f);
            Draw.TextFit(new Rect(r.x, r.y, r.width - sw, r.height), title, TitleFont, Look.BarInk, true);
            if (sw > 0f) Draw.TextFit(new Rect(r.xMax - sw, r.y, sw, r.height), sub, SubFont, Section.SubInk, false, TextAnchor.MiddleRight);
        }
    }

    /// <summary>
    /// CSCommonUX.Tile — 고를 수 있는 타일(시안 'CSCommonUX.Tile' · 모달 4열 격자 타일도 이것): 반지름 20(낮은 타일은 높이/2) · 평소 Look.Pill · 호버 Look.NeutralHover ·
    /// 고르면 모드 파스텔(0.2초) · 잠김 Look.BtnOff, 누르면 0.95배. 안 글은 부르는 쪽이 Ink(이름 14 굵게) · Sub(보조 12) 색으로 — 시안 왼쪽 18 · 이름 → 보조 3.
    /// </summary>
    public static class Tile
    {
        public const float Radius = 20f, PadX = 18f, NameFont = 14f, SubFont = 12f, LineGap = 3f, PressScale = 0.95f, FadeSeconds = 0.2f;

        /// <summary>바탕만 그림(안에 단추가 있는 타일 — 타일 자체는 누르지 않음).</summary>
        public static void Frame(Rect r, bool on, bool enabled = true, bool hoverable = true)
        {
            int key = Look.Key(r, "tile");
            Back(r, key, on, enabled, Look.Hover(key, r, enabled && hoverable && !on));
        }

        static void Back(Rect r, int key, bool on, bool enabled, float hv)
        {
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 1f / FadeSeconds);
            if (!Draw.Repaint) return;
            Color bg = !enabled ? Look.BtnOff : Color.Lerp(Color.Lerp(Look.Pill, Look.NeutralHover, hv), Look.PastelFill, onT);
            Draw.Round(r, bg, Mathf.Min(Radius, r.height * 0.5f));
        }

        /// <summary>바탕 + 누르기. content = 타일 안 그림(눌림 배율 안에서 부름). 누르면 true(잠김이면 늘 false).</summary>
        public static bool In(Rect r, bool on, bool enabled = true, string tip = null, Action content = null)
        {
            int key = Look.Key(r, "tile");
            float hv = Look.Hover(key, r, enabled && !on), pr = Look.Press(key, r, enabled);
            bool paint = Draw.Repaint;
            if (paint) Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
            Back(r, key, on, enabled, hv);
            content?.Invoke();
            if (paint) Draw.EndScale();
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>타일 이름 글 색(고르면 파스텔 글 · 잠김 흐린 글).</summary>
        public static Color Ink(bool on, bool enabled = true) => !enabled ? Look.BtnOffInk : on ? Look.PastelInk : Look.BarInk;

        /// <summary>타일 보조 글 색(고르면 파스텔 보조 글 · 잠김 더 흐린 글).</summary>
        public static Color Sub(bool on, bool enabled = true) => !enabled ? Look.IconOffInk : on ? Look.PastelNote : Look.PillSub;
    }

    /// <summary>
    /// CSCommonUX.ListRow — 목록 한 줄 바탕(시안 'CSCommonUX.ListRow'): 알약(반지름 = 높이/2, 시안 높이 44 · 좌우 18), 평소 투명 · 호버 Look.NeutralHover · 고르면 모드 파스텔(0.2초).
    /// 안 글은 부르는 쪽이 Ink(14, 고르면 굵게) · Note(오른쪽 메모 12) 색으로. 누르면 true.
    /// </summary>
    public static class ListRow
    {
        public const float H = 44f, PadX = 18f, Font = 14f, NoteFont = 12f, FadeSeconds = 0.2f;

        public static bool In(Rect r, bool on, bool enabled = true, string tip = null)
        {
            int key = Look.Key(r, "listrow");
            float hv = Look.Hover(key, r, enabled && !on);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 1f / FadeSeconds);
            if (Draw.Repaint)
            {
                float rad = r.height * 0.5f, ha = hv * (1f - onT);
                if (ha > 0.01f) Draw.Round(r, Draw.Alpha(Look.NeutralHover, ha), rad);
                if (onT > 0.01f) Draw.Round(r, Draw.Alpha(Look.PastelFill, onT), rad);
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>줄 글 색(고르면 파스텔 글).</summary>
        public static Color Ink(bool on) => on ? Look.PastelInk : Look.BarInk;

        /// <summary>줄 메모 · 보조 글 색(고르면 파스텔 보조 글).</summary>
        public static Color Note(bool on) => on ? Look.PastelNote : Look.PillSub;
    }

    /// <summary>CSCommonUX.Well — 안쪽 우물판(시안: 지도 · 미니맵 · 그림이 올라감): 반지름 24(작은 칸은 짧은 변/2) · Section.Plate 면 + 안쪽 1px Look.PillLine(line 을 주면 그 색).</summary>
    public static class Well
    {
        public const float Radius = 24f;

        public static void Frame(Rect r, Color? line = null)
        {
            if (!Draw.Repaint) return;
            float rad = Mathf.Min(Radius, Mathf.Min(r.width, r.height) * 0.5f);
            Draw.Round(r, Section.Plate, rad);
            Draw.RoundOutline(r, line ?? Look.PillLine, rad);
        }
    }

    /// <summary>CSCommonUX.Panel — 위 판 · 아래 독 · 펼침 메뉴 바탕: #14171C · 그림자 두 겹 · 1px 선 · 모서리 14(메뉴 12).</summary>
    public static class Panel
    {
        public static void Frame(Rect r, float radius = 14f) => Look.Elevated(r, radius, Pal.Win);
    }

    /// <summary>
    /// CSCommonUX.Modal — 큰 창(시안 'CSCommonUX.Modal · 라이트 / 다크'): 화면 덮개 Look.Scrim · 가운데 판(폭 최대 560 · 모서리 14 · Look.FieldFocus 면 · 1px Look.BarLine · 모달 그림자 Look.ModalShadow*),
    /// 머리 56(제목 17 굵게 · 설명 12.5 Sub · 지름 32 원 닫기) · 몸(좌우 20 · 위 4 · 아래 16 · 요소 사이 12) · 발 64(Look.Foot + 위 1px Look.FootLine, 오른쪽부터 알약 단추 40 · 사이 8 · 좌우 16).
    /// 몸 부품: Tabs(알약 홈 + 고른 칸 파스텔 알약 36, 칸은 CSCommonUX.Tabs.Cell) · Textfield(알약 40) · 타일 격자(4열 · 높이 72 · 사이 8 — 칸은 CSCommonUX.Tile).
    /// 움직임: CSCommonAnim.Modal(열기 0.18초 · 닫기 0.14초) + BeginModal(10 아래에서 떠오름 · Ease). 단추 눌림 0.96배(PillBar.PressScale).
    /// 캘린더형(시안 'CSCommonUX.Modal · 캘린더형'): Sheet(머리 띠 44 · 6점 손잡이 · 원 닫기) · SheetTitle(밑줄 제목) · SheetTabs · SheetRow · More · Save,
    /// Expand(폭 448 → 640 · 추가 영역 0 → 230 을 0.44초 cubic-bezier(.2,.8,.2,1) · 투명도 0.28초, 열 때만 0.14초 지연).
    /// </summary>
    public static class Modal
    {
        public const float HeadH = 56f, FootH = 64f;
        /// <summary>판 최대 폭 · 모서리 · 몸 좌우 · 몸 위 · 몸 아래 · 요소 사이 · 화면 가장자리 여백.</summary>
        public const float Width = 560f, WideWidth = 720f, Radius = 14f, PadX = 20f, BodyTop = 4f, BodyBottom = 16f, Gap = 12f, Margin = 24f;
        /// <summary>머리: 제목 글 · 설명 글 · 제목 → 설명 · 닫기 원 지름 · 닫기 아이콘 · 닫기 오른쪽.</summary>
        public const float TitleFont = 17f, SubFont = 12.5f, TitleGap = 12f, CloseD = 32f, CloseIcon = 16f, CloseRight = 16f;
        /// <summary>알약 단추: 높이 · 사이 · 발 좌우 · 주 단추 좌우 · 보통 단추 좌우 · 글 · 아이콘 · 아이콘 → 글.</summary>
        public const float BtnH = 40f, BtnGap = 8f, FootPad = 16f, BtnPadPrimary = 20f, BtnPadNeutral = 18f, BtnFont = 14.5f, BtnIcon = 18f, BtnIconGap = 8f;
        /// <summary>몸 부품: 탭 높이 · 홈 안쪽 · 탭 사이 · 탭 좌우 · 탭 글 · 글 칸 높이 · 타일 높이 · 타일 사이 · 타일 열 수.</summary>
        public const float TabH = 36f, TabPad = 4f, TabGap = 4f, TabPadX = 18f, TabPadXMin = 10f, TabFont = 13.5f, FieldH = 40f, TileH = 72f, TileGap = 8f;
        public const int TileCols = 4;
        /// <summary>움직임: 떠오르는 거리 · 열기 · 닫기 · 고른 칸 색 전환 · 줄 호버 · 밑줄 초점 초.</summary>
        public const float Rise = 10f, OpenSeconds = 0.18f, CloseSeconds = 0.14f, PickSeconds = 0.2f, HoverSeconds = 0.12f, TitleSeconds = 0.14f;

        /// <summary>캘린더형 판: 접힌 폭 · 펼친 폭 · 머리 띠 · 몸 좌우 · 몸 위 · 들임 · 제목 칸 · 제목 글 · 밑줄 · 초점 밑줄.</summary>
        public const float SheetW = 448f, SheetWideW = 640f, SheetHeadH = 44f, SheetPadX = 24f, SheetTop = 8f, SheetIndent = 40f, SheetTitleH = 56f, SheetTitleFont = 22f, SheetLine = 1f, SheetLineFocus = 2f;
        /// <summary>캘린더형 탭(홈 없음): 높이 · 좌우 · 글 · 위 · 아래.</summary>
        public const float SheetTabH = 32f, SheetTabPadX = 16f, SheetTabFont = 13f, SheetTabTop = 12f, SheetTabBottom = 4f;
        /// <summary>캘린더형 줄: 높이 · 아이콘 칸 · 아이콘 · 아이콘 → 글 · 모서리 · 글 · 보조 글 · 보조 오른쪽 · 색 점 지름.</summary>
        public const float SheetRowH = 48f, SheetRowIconW = 24f, SheetRowIcon = 20f, SheetRowGap = 16f, SheetRowRadius = 10f, SheetRowFont = 14f, SheetRowSubFont = 12.5f, SheetRowSubPad = 8f, SheetDot = 14f;
        /// <summary>캘린더형 머리 · 발: 손잡이 점 · 점 사이 · 닫기 오른쪽 · 위 · 발 위 여백 · 발 왼쪽 · 쉐브론 · 글 단추 글 · 글 단추 왼쪽 · 오른쪽 · 글 → 쉐브론 · 저장 좌우 · 추가 영역 최대 높이.</summary>
        public const float GripDot = 3f, GripGap = 3f, SheetCloseRight = 8f, SheetCloseTop = 6f, SheetFootTop = 8f, SheetFootPadL = 20f, Chevron = 16f, LinkFont = 14f, LinkPadL = 16f, LinkPadR = 14f, LinkGap = 6f, SavePadX = 22f, ExtraH = 230f;
        /// <summary>Expand: 펼침 · 투명도 · 투명도 지연(열 때만) · 저장됨 표시 초.</summary>
        public const float ExpandSeconds = 0.44f, FadeSeconds = 0.28f, FadeDelay = 0.14f, SavedSeconds = 1.6f;
        /// <summary>시안 곡선 cubic-bezier(x1, y1, x2, y2).</summary>
        public const float EaseX1 = 0.2f, EaseY1 = 0.8f, EaseX2 = 0.2f, EaseY2 = 1f;

        static Rect Screen => new(-Draw.Width, -Draw.Height, Draw.Width * 3f, Draw.Height * 3f);

        /// <summary>화면 가운데 판 자리: 폭 = min(width, 화면 − 여백 2) · 높이도 화면 안(높이가 바뀌어도 매번 다시 가운데).</summary>
        public static Rect Place(float height, float width = Width)
        {
            float w = Mathf.Min(width, Draw.Width - Margin * 2f), h = Mathf.Min(height, Draw.Height - Margin * 2f);
            return new Rect(Mathf.Round((Draw.Width - w) * 0.5f), Mathf.Round((Draw.Height - h) * 0.5f), w, h);
        }

        /// <summary>덮개: area 를 Look.Scrim 으로 덮음(k = 나타난 정도, radius = 창 모서리).</summary>
        public static void Scrim(Rect area, float k = 1f, float radius = 0f)
        {
            // 덮개가 깔리면 그 위 모달이 맨 위 — 툴바 칸이 모달 단추를 막지 않게(모든 이벤트에서)
            Draw.ClearBlocks();
            if (!Draw.Repaint) return;
            var c = Draw.Alpha(Look.Scrim, k);
            if (radius > 0f) Draw.Round(area, c, radius);
            else Draw.Fill(area, c);
        }

        /// <summary>판: 모달 그림자 + 불투명 면(Look.FieldFocus) + 1px 선(Look.BarLine).</summary>
        public static void Panel(Rect r, float radius = Radius)
        {
            Surface(r, radius);
            Outline(r, radius);
        }

        static void Surface(Rect r, float radius)
        {
            if (!Draw.Repaint) return;
            // 그림자: 판 가장자리 기준 −흐림/2 ~ +흐림/2 로 퍼지는 겹 — 가운데로 갈수록 쌓여 Look.ModalShadow 에 닿음(CSS 0 y blur 흉내)
            const int layers = 10;
            float blur = Look.ModalShadowBlur, a = 1f - Mathf.Pow(1f - Look.ModalShadow, 1f / layers);
            for (int i = 0; i < layers; i++)
            {
                float e = blur * ((i + 0.5f) / layers - 0.5f);
                Draw.Round(new Rect(r.x - e, r.y + Look.ModalShadowY - e, r.width + e * 2f, r.height + e * 2f), Draw.Alpha(Color.black, a), Mathf.Max(0f, radius + e));
            }
            Draw.Round(r, Look.FieldFocus, radius);
        }

        static void Outline(Rect r, float radius)
        {
            if (Draw.Repaint) Draw.RoundOutline(r, Look.BarLine, radius);
        }

        /// <summary>판 위(top) 또는 아래 띠(Look.Foot) — 바깥 모서리만 둥글게. 아래 띠는 위에 1px Look.FootLine.</summary>
        static void Band(Rect f, bool top)
        {
            if (!Draw.Repaint) return;
            Draw.Round(f, Look.Foot, Radius);
            Draw.Fill(top ? new Rect(f.x, f.yMax - Radius, f.width, Radius) : new Rect(f.x, f.y, f.width, Radius), Look.Foot);
            if (!top) Draw.Fill(new Rect(f.x, f.y, f.width, 1f), Look.FootLine);
        }

        /// <summary>판 · 머리를 그리고 몸 칸을 돌려줌. closed = 닫기를 누름. footer = 발 띠(단추는 FootButton).</summary>
        public static Rect Frame(Rect r, string title, string sub, out bool closed, bool footer = false, bool scrim = true)
        {
            if (scrim) Scrim(Screen);
            Surface(r, Radius);
            if (footer) Band(new Rect(r.x, r.yMax - FootH, r.width, FootH), false);
            Outline(r, Radius);
            var close = new Rect(r.xMax - CloseRight - CloseD, r.y + (HeadH - CloseD) * 0.5f, CloseD, CloseD);
            if (Draw.Repaint)
            {
                float x = r.x + PadX, room = close.x - TitleGap - x;
                float tw = Mathf.Min(Mathf.Ceil(Draw.TextWidth(title, TitleFont, true)), room);
                Draw.TextFit(new Rect(x, r.y, tw + 2f, HeadH), title, TitleFont, Pal.Txt, true);
                float sx = x + tw + TitleGap;
                if (!string.IsNullOrEmpty(sub) && close.x - TitleGap - sx > 0f) Draw.TextFit(new Rect(sx, r.y, close.x - TitleGap - sx, HeadH), sub, SubFont, Pal.Sub);
            }
            closed = CloseButton(close);
            return new Rect(r.x + PadX, r.y + HeadH + BodyTop, r.width - PadX * 2f, r.height - HeadH - BodyTop - BodyBottom - (footer ? FootH : 0f));
        }

        /// <summary>발 칸(단추 줄 가운데 y) — 오른쪽부터 단추를 놓을 때 씀.</summary>
        public static float FooterCy(Rect r) => r.yMax - FootH * 0.5f;

        /// <summary>지름 32 원 닫기: 회색 알약 면(filled = false 면 면 없이) + X 16 가운데 · 호버 옅은 덮개 · 눌림 0.96배. 누르면 true.</summary>
        public static bool CloseButton(Rect c, bool filled = true)
        {
            int key = CSCommonAnim.Key("modal.close", filled ? "f" : "b");
            float hv = Look.Hover(key, c), pr = Look.Press(key, c);
            if (Draw.Repaint)
            {
                float rad = c.height * 0.5f;
                Draw.BeginScale(c.center, Look.PressScale(pr, PillBar.PressScale));
                if (filled) Draw.Round(c, Look.PillNeutral, rad);
                if (hv > 0.01f) Draw.Round(c, Draw.Alpha(Pal.Haze, PillBar.HoverAlpha * hv), rad);
                Draw.Icon(Icons.Close, new Rect(Mathf.Round(c.center.x - CloseIcon * 0.5f), Mathf.Round(c.center.y - CloseIcon * 0.5f), CloseIcon, CloseIcon), Look.BarOff, 24f, 2f);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(c, "닫기(Esc)");
            return Draw.Button(c);
        }

        /// <summary>알약 단추 폭(좌우 여백 대칭 + 아이콘 + 글). padX 를 안 주면 주 20 · 보통 18.</summary>
        public static float ButtonWidth(string icon, string label, bool primary, float padX = -1f)
        {
            float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, BtnFont, primary));
            float iw = string.IsNullOrEmpty(icon) ? 0f : BtnIcon + (tw > 0f ? BtnIconGap : 0f);
            if (padX < 0f) padX = primary ? BtnPadPrimary : BtnPadNeutral;
            return padX * 2f + iw + tw;
        }

        /// <summary>알약 단추 40: 주 = 모드 파스텔(굵은 글) · 보통 = 회색 알약(Dim 아이콘). 아이콘 + 글 한 덩어리 칸 가운데 · 호버 옅은 덮개 · 눌림 0.96배. 누르면 true.</summary>
        public static bool Button(Rect b, string icon, string label, bool primary, bool enabled = true, string tip = null)
        {
            int key = Look.Key(b, "modal.btn", label);
            float hv = Look.Hover(key, b, enabled), pr = Look.Press(key, b, enabled);
            if (Draw.Repaint)
            {
                float rad = b.height * 0.5f;
                Draw.BeginScale(b.center, Look.PressScale(pr, PillBar.PressScale));
                Draw.Round(b, !enabled ? Look.Off : primary ? Look.PastelFill : Look.PillNeutral, rad);
                if (enabled && hv > 0.01f) Draw.Round(b, Draw.Alpha(Pal.Haze, PillBar.HoverAlpha * hv), rad);
                Color ink = !enabled ? Look.OffText : primary ? Look.PastelInk : Look.PillNeutralInk;
                Color iconInk = !enabled ? Look.OffInk : primary ? Look.PastelInk : Pal.Dim;
                float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, BtnFont, primary));
                float iw = string.IsNullOrEmpty(icon) ? 0f : BtnIcon + (tw > 0f ? BtnIconGap : 0f);
                float x = Mathf.Max(b.x, Mathf.Round(b.center.x - (iw + tw) * 0.5f));
                if (iw > 0f) Draw.Icon(icon, new Rect(x, Mathf.Round(b.center.y - BtnIcon * 0.5f), BtnIcon, BtnIcon), iconInk, 24f, 2f);
                if (tw > 0f) Draw.TextFit(new Rect(x + iw, b.y, Mathf.Max(0f, b.xMax - x - iw), b.height), label, BtnFont, ink, primary);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(b, tip);
            return enabled && Draw.Button(b);
        }

        /// <summary>발 단추를 오른쪽부터 놓음(주 단추 먼저): right = 다음 단추 오른쪽 끝(처음엔 0 → 판 오른쪽 − 16), 놓을 때마다 왼쪽으로 옮겨 줌. 누르면 true.</summary>
        public static bool FootButton(Rect frame, ref float right, string icon, string label, bool primary = false, bool enabled = true, string tip = null)
        {
            if (right <= 0f) right = frame.xMax - FootPad;
            float w = ButtonWidth(icon, label, primary);
            var b = new Rect(right - w, Mathf.Round(FooterCy(frame) - BtnH * 0.5f), w, BtnH);
            right = b.x - BtnGap;
            return Button(b, icon, label, primary, enabled, tip);
        }

        /// <summary>탭 묶음(알약 홈 Look.Pill + 고른 칸 파스텔 알약 36 · 글 13.5, 고른 칸 굵게 · 색 0.2초): 왼쪽 위 (x, y) 에 내용 폭, maxW 를 넘으면 칸을 똑같이 나눔. 그린 폭을 돌려줌(높이 = TabH + TabPad × 2).</summary>
        public static float Tabs(float x, float y, float maxW, string[] labels, int cur, Action<int> pick)
            => PillTabs(x, y, maxW, labels, cur, pick, true, TabH, TabPadX, TabFont);

        static float PillTabs(float x, float y, float maxW, string[] labels, int cur, Action<int> pick, bool groove, float h, float padX, float font)
        {
            int n = labels?.Length ?? 0;
            if (n == 0) return 0f;
            float pad = groove ? TabPad : 0f;
            var widths = new float[n];
            float total = pad * 2f + TabGap * (n - 1);
            for (int i = 0; i < n; i++)
            {
                widths[i] = CSCommonUX.Tabs.CellWidth(null, labels[i], null, font, padX);
                total += widths[i];
            }
            // 넘치면 알약을 바로 나눠 이름을 자르지 않고 좌우 여백 → 글 한 단계 순으로 줄여 한 줄에 맞춤
            for (int step = 0; step < 2 && total > maxW; step++)
            {
                if (step == 0) padX = Mathf.Min(padX, TabPadXMin);
                else font -= Draw.PillFontStep;
                total = pad * 2f + TabGap * (n - 1);
                for (int i = 0; i < n; i++)
                {
                    widths[i] = CSCommonUX.Tabs.CellWidth(null, labels[i], null, font, padX);
                    total += widths[i];
                }
            }
            // 그래도 넘치면(칸 수가 너무 많음) 예전처럼 똑같이 나눔(이름 줄임)
            if (total > maxW)
            {
                float each = (maxW - pad * 2f - TabGap * (n - 1)) / n;
                for (int i = 0; i < n; i++) widths[i] = each;
                total = maxW;
            }
            if (groove && Draw.Repaint) Draw.Round(new Rect(x, y, total, h + pad * 2f), Look.Pill, (h + pad * 2f) * 0.5f);
            int skey = CSCommonAnim.Key(labels[0], labels[n - 1]) ^ n * 7919 ^ (groove ? 0x6d2b79f5 : 0x1b873593);
            float cx = x + pad;
            for (int i = 0; i < n; i++)
            {
                var c = new Rect(cx, y + pad, widths[i], h);
                bool on = i == cur;
                if (CSCommonUX.Tabs.Cell(c, skey ^ (i + 1) * 0x2f6b8d3, on, null, labels[i], font) && !on) pick?.Invoke(i);
                cx += widths[i] + TabGap;
            }
            return total;
        }

        // ---------------- 캘린더형(Sheet) · 펼침(Expand)
        // 공용 API 이름 Sheet · Expand 는 시안 제안(확정 아님) — 바꾸면 F1 Board 모달 · README 도 같이.

        /// <summary>
        /// 캘린더형 틀: 덮개 · 판(Panel 과 같은 면 · 선 · 그림자) · 머리 띠 44(Look.Foot · 가운데 6점 손잡이 · 오른쪽 원 닫기, 면 없음).
        /// 몸 칸(좌우 24 · 위 8 · 아래는 발 위 여백 8 + 발 64)을 돌려줌. 폭은 Expand 로 448 ↔ 640, 자리는 Place 로 매번 가운데.
        /// </summary>
        public static Rect Sheet(Rect r, out bool closed, bool scrim = true)
        {
            if (scrim) Scrim(Screen);
            Surface(r, Radius);
            var head = new Rect(r.x, r.y, r.width, SheetHeadH);
            Band(head, true);
            if (Draw.Repaint)
            {
                float gw = GripDot * 6f + GripGap * 5f, gx = Mathf.Round(head.center.x - gw * 0.5f), gy = Mathf.Round(head.center.y - GripDot * 0.5f);
                for (int i = 0; i < 6; i++) Draw.Round(new Rect(gx + i * (GripDot + GripGap), gy, GripDot, GripDot), Draw.Alpha(Pal.Sub, Look.GripAlpha), GripDot * 0.5f);
            }
            Outline(r, Radius);
            closed = CloseButton(new Rect(r.xMax - SheetCloseRight - CloseD, r.y + SheetCloseTop, CloseD, CloseD), false);
            return new Rect(r.x + SheetPadX, r.y + SheetHeadH + SheetTop, r.width - SheetPadX * 2f, r.height - SheetHeadH - SheetTop - SheetFootTop - FootH);
        }

        /// <summary>밑줄 제목 칸(알약 아님): body 왼쪽 40 들임 · 높이 56 · 글 22 · 평소 1px Look.TitleLine · 초점 2px Look.PastelRing(0.14초) · 자리 글 OffText · 커서 파스텔.</summary>
        public static string SheetTitle(Rect body, float y, string value, int maxLength, string placeholder = "제목 추가", string name = null)
        {
            var r = new Rect(body.x + SheetIndent, y, body.width - SheetIndent, SheetTitleH);
            name ??= "csm" + CSCommonAnim.Key("sheet.title", placeholder);
            value ??= "";
            bool focus = GUI.GetNameOfFocusedControl() == name;
            float fo = CSCommonAnim.Toward(CSCommonAnim.Key("sheet.title", name), focus ? 1f : 0f, 1f / TitleSeconds);
            if (Draw.Repaint)
            {
                Draw.Fill(new Rect(r.x, r.yMax - SheetLine, r.width, SheetLine), Look.TitleLine);
                if (fo > 0.01f) Draw.Fill(new Rect(r.x, r.yMax - SheetLineFocus, r.width, SheetLineFocus), Draw.Alpha(Look.PastelRing, fo));
                if (value.Length == 0 && !string.IsNullOrEmpty(placeholder)) Draw.TextFit(new Rect(r.x, r.y, r.width, r.height - SheetLineFocus), placeholder, SheetTitleFont, Look.OffText);
            }
            var cursor = GUI.skin.settings.cursorColor;
            GUI.skin.settings.cursorColor = Look.PastelRing;
            GUI.SetNextControlName(name);
            string v = Draw.Field(new Rect(r.x - Textfield.FieldInset, r.y, r.width + Textfield.FieldInset, r.height - SheetLineFocus), value, SheetTitleFont, maxLength);
            GUI.skin.settings.cursorColor = cursor;
            return v;
        }

        /// <summary>캘린더형 탭(홈 없음 · 높이 32 · 좌우 16 · 글 13 · 고른 칸 파스텔 알약): body 왼쪽 40 들임, y = 위. 그린 폭을 돌려줌.</summary>
        public static float SheetTabs(Rect body, float y, string[] labels, int cur, Action<int> pick)
            => PillTabs(body.x + SheetIndent, y, body.width - SheetIndent, labels, cur, pick, false, SheetTabH, SheetTabPadX, SheetTabFont);

        /// <summary>캘린더형 줄 48: 왼쪽 24 칸에 아이콘 20(Sub) 또는 색 점 14 · 사이 16 · 글 14 · 오른쪽 보조 글 12.5(Dim, 오른쪽 8) · 모서리 10 · 호버 Look.Pill(0.12초). 누르면 true.</summary>
        public static bool SheetRow(Rect r, string icon, string text, string sub = null, Color? dot = null, bool enabled = true)
        {
            int key = CSCommonAnim.Key("sheet.row", text) ^ Mathf.RoundToInt(r.y) * 31;
            float hv = 0f; // 호버 효과 없음
            if (Draw.Repaint)
            {
                if (hv > 0.01f) Draw.Round(r, Draw.Alpha(Look.Pill, hv), SheetRowRadius);
                var col = new Rect(r.x, r.y, SheetRowIconW, r.height);
                if (dot.HasValue) Draw.Dot(col.center, SheetDot * 0.5f, dot.Value);
                else if (!string.IsNullOrEmpty(icon)) Draw.Icon(icon, new Rect(Mathf.Round(col.center.x - SheetRowIcon * 0.5f), Mathf.Round(col.center.y - SheetRowIcon * 0.5f), SheetRowIcon, SheetRowIcon), Pal.Sub, 24f, 1.8f);
                float sw = string.IsNullOrEmpty(sub) ? 0f : Mathf.Ceil(Draw.TextWidth(sub, SheetRowSubFont));
                float tx = col.xMax + SheetRowGap, right = r.xMax - (sw > 0f ? sw + SheetRowSubPad + SheetRowGap : 0f);
                Draw.TextFit(new Rect(tx, r.y, Mathf.Max(0f, right - tx), r.height), text, SheetRowFont, enabled ? Pal.Txt : Look.OffText);
                if (sw > 0f) Draw.Text(new Rect(r.xMax - SheetRowSubPad - sw - 2f, r.y, sw + 2f, r.height), sub, SheetRowSubFont, Pal.Dim, false, TextAnchor.MiddleRight);
            }
            return enabled && Draw.Button(r);
        }

        /// <summary>발 왼쪽 글 단추 "옵션 더보기" ↔ "간단히 보기"(Look.PastelLink 글 14 · 높이 40 알약 호버 · 오른쪽 쉐브론 16 이 t(Expand 진행도)에 따라 0 → 180°). 누르면 true.</summary>
        public static bool More(Rect sheet, bool open, float t)
        {
            string label = open ? "간단히 보기" : "옵션 더보기";
            float tw = Mathf.Ceil(Draw.TextWidth(label, LinkFont));
            var b = new Rect(sheet.x + SheetFootPadL, Mathf.Round(FooterCy(sheet) - BtnH * 0.5f), LinkPadL + tw + LinkGap + Chevron + LinkPadR, BtnH);
            // 폭이 움직이는 동안에도 같은 열쇠(자리로 만들면 호버가 끊김)
            int key = CSCommonAnim.Key("sheet.more", "link");
            float hv = Look.Hover(key, b), pr = Look.Press(key, b);
            if (Draw.Repaint)
            {
                Draw.BeginScale(b.center, Look.PressScale(pr, PillBar.PressScale));
                if (hv > 0.01f) Draw.Round(b, Draw.Alpha(Pal.Haze, PillBar.HoverAlpha * hv), BtnH * 0.5f);
                Draw.Text(new Rect(b.x + LinkPadL, b.y, tw + 2f, b.height), label, LinkFont, Look.PastelLink);
                var cr = new Rect(b.x + LinkPadL + tw + LinkGap, Mathf.Round(b.center.y - Chevron * 0.5f), Chevron, Chevron);
                var m = GUI.matrix;
                if (t > 0.001f) GUIUtility.RotateAroundPivot(180f * t, Draw.ToScreen(cr).center);
                Draw.Icon(Icons.Chevron, cr, Look.PastelLink, 24f, 2.2f);
                GUI.matrix = m;
                Draw.EndScale();
            }
            return Draw.Button(b);
        }

        static int savedKey;
        static float savedAt = -100f;

        /// <summary>발 오른쪽 저장(파스텔 알약 40 · 좌우 22 대칭) — 누르면 1.6초 동안 ✓ + done 글. 누르면 true.</summary>
        public static bool Save(Rect sheet, string label = "저장", string done = "저장됨", bool enabled = true, string tip = null)
        {
            int key = CSCommonAnim.Key("sheet.save", label);
            bool shown = savedKey == key && Time.unscaledTime - savedAt < SavedSeconds;
            string text = shown ? done : label, icon = shown ? Icons.Check : null;
            float w = ButtonWidth(icon, text, true, SavePadX);
            var b = new Rect(sheet.xMax - FootPad - w, Mathf.Round(FooterCy(sheet) - BtnH * 0.5f), w, BtnH);
            bool hit = Button(b, icon, text, true, enabled, tip);
            if (hit)
            {
                savedKey = key;
                savedAt = Time.unscaledTime;
            }
            return hit;
        }

        struct Run
        {
            public bool Open;
            public float From, FadeFrom, At;
        }

        static readonly Dictionary<int, Run> runs = new();

        /// <summary>
        /// Modal.Expand — 펼침 진행도 0 ~ 1 을 돌려줌: 방향이 바뀐 때부터 0.44초 동안 지금 값 → 목표를 cubic-bezier(.2,.8,.2,1) 로(CSS transition 과 같음).
        /// fade = 추가 영역 투명도(열 때 0.14초 기다렸다 0.28초 · 닫을 때 바로 0.28초, 선형). 폭 = Mathf.Lerp(SheetW, SheetWideW, t) · 추가 영역 높이 = ExtraH × t · 쉐브론 180° × t.
        /// 처음 보는 열쇠는 open 상태로 멈춰 있음(열자마자 그 상태로 보임). 추가 영역은 BeginExtra · EndExtra 로 자르고 흐리게.
        /// </summary>
        public static float Expand(int key, bool open, out float fade)
        {
            float now = Time.unscaledTime;
            if (!runs.TryGetValue(key, out var s)) s = new Run { Open = open, From = open ? 1f : 0f, FadeFrom = open ? 1f : 0f, At = now - ExpandSeconds - FadeDelay - FadeSeconds };
            if (s.Open != open)
            {
                s.From = RunValue(s, now, out s.FadeFrom);
                s.Open = open;
                s.At = now;
            }
            runs[key] = s;
            return RunValue(s, now, out fade);
        }

        static float RunValue(Run s, float now, out float fade)
        {
            float el = now - s.At, to = s.Open ? 1f : 0f;
            fade = Mathf.Lerp(s.FadeFrom, to, Mathf.Clamp01((el - (s.Open ? FadeDelay : 0f)) / FadeSeconds));
            return Mathf.Lerp(s.From, to, Ease(el / ExpandSeconds));
        }

        static float extraOpacity;

        /// <summary>펼친 추가 영역 그리기 시작: r 위쪽부터 ExtraH × t 만 보이게 자르고 투명도 fade. EndExtra 와 짝. 안쪽 좌표는 r 왼쪽 위 기준.</summary>
        public static void BeginExtra(Rect r, float t, float fade)
        {
            extraOpacity = Draw.Opacity;
            Draw.Opacity *= fade;
            Draw.BeginClip(new Rect(r.x, r.y, r.width, Mathf.Round(ExtraH * t)), Vector2.zero);
        }

        public static void EndExtra()
        {
            Draw.EndClip();
            Draw.Opacity = extraOpacity;
        }

        /// <summary>시안 곡선 cubic-bezier(.2, .8, .2, 1): 진행 x(0 ~ 1)에 맞는 값.</summary>
        public static float Ease(float x) => Ease(x, EaseX1, EaseY1, EaseX2, EaseY2);

        /// <summary>cubic-bezier(x1, y1, x2, y2): 진행 x(0 ~ 1)에 맞는 값(뉴턴법으로 곡선 t 를 찾음, y 가 1 을 넘으면 살짝 넘침).</summary>
        public static float Ease(float x, float x1, float y1, float x2, float y2)
        {
            x = Mathf.Clamp01(x);
            if (x <= 0f || x >= 1f) return x;
            float t = x;
            for (int i = 0; i < 8; i++)
            {
                float u = 1f - t;
                float bx = 3f * u * u * t * x1 + 3f * u * t * t * x2 + t * t * t - x;
                float dx = 3f * u * u * x1 + 6f * u * t * (x2 - x1) + 3f * t * t * (1f - x2);
                if (Mathf.Abs(bx) < 1e-5f || Mathf.Abs(dx) < 1e-6f) break;
                t = Mathf.Clamp01(t - bx / dx);
            }
            float w = 1f - t;
            return 3f * w * w * t * y1 + 3f * w * t * t * y2 + t * t * t;
        }
    }

    /// <summary>
    /// CSCommonUX.Phone — 휴대폰(아이폰 프로 맥스 440×956 · 다이나믹 아일랜드 · 리퀴드 글라스 면). 크기는 고정(CSCommonDesign.PhoneW · PhoneH).
    /// Frame 이 틀(티타늄 테두리 · 화면 바탕 · 상태 줄 · 아일랜드 · 홈 막대)을 그리고 안쪽 쓸 자리를 돌려줌. 그 안은 Glass · GlassButton 으로 채움.
    /// 게임 화면은 흐려 비출 수 없어, 반투명 면 + 위 반짝임 + 가는 테두리로 유리 느낌을 냄.
    /// </summary>
    public static class Phone
    {
        /// <summary>화면이 낮으면 틀 전체를 줄이는 배율(1 = 440×956 그대로). 크기 규격은 그대로이고 보이는 크기만 줄어듦.</summary>
        public static float Scale => Mathf.Clamp((Draw.Height - CSCommonDesign.PhoneMargin * 2f) / CSCommonDesign.PhoneH, 0.4f, 1f);

        /// <summary>화면 오른쪽 가운데에 놓은 틀 자리. 크기는 논리 440×956(그리는 좌표) — 낮은 화면에서는 BeginScale 로 줄여 그림.</summary>
        public static Rect Place()
        {
            float m = CSCommonDesign.PhoneMargin, k = Scale;
            return new Rect(Mathf.Max(m, Draw.Width - CSCommonDesign.PhoneW * k - m), Mathf.Max(m, (Draw.Height - CSCommonDesign.PhoneH * k) * 0.5f), CSCommonDesign.PhoneW, CSCommonDesign.PhoneH);
        }

        /// <summary>화면에 보이는 틀 자리(배율 적용 — 클릭 막기 · 마우스 검사용).</summary>
        public static Rect Visual(Rect frame) => new(frame.x, frame.y, frame.width * Scale, frame.height * Scale);

        static Matrix4x4 savedMatrix;

        /// <summary>틀을 그리기 전에: 낮은 화면이면 틀 왼쪽 위를 기준으로 GUI 를 줄임. 그린 뒤 EndScale.</summary>
        public static void BeginScale(Rect frame)
        {
            savedMatrix = GUI.matrix;
            float k = Scale;
            if (k < 0.999f) GUIUtility.ScaleAroundPivot(new Vector2(k, k), new Vector2(frame.x, frame.y) * Draw.S);
        }

        public static void EndScale() => GUI.matrix = savedMatrix;

        /// <summary>틀 그리기. time = 왼쪽 상태 글(시각) · status = 오른쪽 상태 글 · island = 아일랜드에 펼칠 한 줄(없으면 알약만). 안쪽 쓸 자리를 돌려줌.</summary>
        public static Rect Frame(Rect r, string time, string status, string island = null)
        {
            var screen = new Rect(r.x + CSCommonDesign.PhoneBezel, r.y + CSCommonDesign.PhoneBezel, r.width - CSCommonDesign.PhoneBezel * 2f, r.height - CSCommonDesign.PhoneBezel * 2f);
            if (Draw.Repaint)
            {
                // 티타늄 테두리(바깥 선 · 안쪽 검은 틈) + 화면 바탕(iOS 에는 그림자가 없음)
                Draw.Round(r, Pal.PhoneBezel, CSCommonDesign.PhoneRadius);
                Draw.RoundOutline(r, Pal.PhoneBezelEdge, CSCommonDesign.PhoneRadius, 1.5f);
                Draw.Round(new Rect(r.x + 3f, r.y + 3f, r.width - 6f, r.height - 6f), Color.black, CSCommonDesign.PhoneRadius - 3f);
                Draw.Round(screen, Pal.PhoneWall, CSCommonDesign.ScreenRadius);
                // 바탕 무늬: 안쪽에 번지는 색 덩어리 셋(유리가 비출 것)
                // 상태 줄 · 홈 막대
                float sy = screen.y + CSCommonDesign.IslandTop, sh = CSCommonDesign.IslandH;
                Draw.Text(new Rect(screen.x + 34f, sy, 90f, sh), time, 16f, Pal.PhoneInk, true, TextAnchor.MiddleLeft);
                Draw.Text(new Rect(screen.xMax - 34f - 140f, sy, 140f, sh), status, 14f, Pal.PhoneInk, true, TextAnchor.MiddleRight);
                Draw.Round(new Rect(screen.center.x - CSCommonDesign.HomeBarW * 0.5f, screen.yMax - 14f, CSCommonDesign.HomeBarW, 5f), Draw.Alpha(Pal.PhoneInk, 0.85f), 2.5f);
                Island(screen, island);
            }
            return new Rect(screen.x + CSCommonDesign.GlassPad, screen.y + CSCommonDesign.StatusH, screen.width - CSCommonDesign.GlassPad * 2f, screen.height - CSCommonDesign.StatusH - CSCommonDesign.HomeH);
        }

        /// <summary>색이 번지는 덩어리(겹친 원 여럿).</summary>
        static void Glow(Vector2 c, float radius, Color color)
        {
            for (int i = 0; i < 7; i++)
                Draw.Dot(c, radius * (1f - i * 0.12f), Draw.Alpha(color, color.a * 0.35f));
        }

        /// <summary>다이나믹 아일랜드: 검은 알약, 글이 있으면 옆으로 펼쳐 글을 보임(부드럽게 늘었다 줄어듦).</summary>
        static void Island(Rect screen, string text)
        {
            bool open = !string.IsNullOrEmpty(text);
            float want = open ? Mathf.Min(CSCommonDesign.IslandMaxW, Draw.TextWidth(text, 13f, true) + 52f) : CSCommonDesign.IslandW;
            float w = CSCommonAnim.Toward(0x1514d1, Mathf.Max(want, CSCommonDesign.IslandW), 14f, CSCommonDesign.IslandW);
            var pill = new Rect(screen.center.x - w * 0.5f, screen.y + CSCommonDesign.IslandTop, w, CSCommonDesign.IslandH);
            Draw.Round(pill, Color.black, CSCommonDesign.IslandH * 0.5f);
            if (open && w > want - 6f) Draw.TextFit(new Rect(pill.x + 20f, pill.y, pill.width - 40f, pill.height), text, 13f, Color.white, true, TextAnchor.MiddleCenter);
        }

        /// <summary>유리 면(카드 · 줄 바탕): 반투명 바탕 + 위 반짝임 + 가는 테두리(그림자 없음). strength 로 진하기.</summary>
        public static void Glass(Rect r, float radius = -1f, float strength = 1f)
        {
            if (!Draw.Repaint) return;
            if (radius < 0f) radius = CSCommonDesign.GlassRadius;
            Draw.Round(r, Draw.Alpha(Pal.GlassFill, strength), radius);
            Draw.Round(new Rect(r.x + 2f, r.y + 2f, r.width - 4f, r.height * 0.46f), Draw.Alpha(Pal.GlassSheen, strength), Mathf.Max(2f, radius - 2f));
            Draw.RoundOutline(r, Draw.Alpha(Pal.GlassEdge, strength), radius, CSCommonDesign.GlassStroke);
            Draw.Fill(new Rect(r.x + radius, r.y + 1f, Mathf.Max(0f, r.width - radius * 2f), 1f), Draw.Alpha(Pal.GlassShine, strength));
        }

        /// <summary>유리 단추: on 이면 강조색으로 물든 유리. 누르면 true.</summary>
        public static bool GlassButton(Rect r, string label, bool on, string tip = null, bool enabled = true)
        {
            int key = Look.Key(r, "glassbtn", label);
            float hv = Look.Hover(key, r, enabled);
            if (Draw.Repaint)
            {
                Glass(r, CSCommonDesign.GlassRadius - 6f, 0.8f + 0.5f * hv);
                if (on) Draw.Round(r, Pal.Accent, CSCommonDesign.GlassRadius - 6f);
                Draw.TextFit(new Rect(r.x + 12f, r.y, r.width - 24f, r.height), label, 15f, on ? Color.white : enabled ? Pal.PhoneInk : Draw.Alpha(Pal.PhoneInk, 0.4f), true, TextAnchor.MiddleCenter);
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>하단 툴바(탭 바): CSCommonUX.Toolbar 와 같은 알약(설정 FAB 없음). content = Frame 이 돌려준 쓸 자리.</summary>
        public static void TabBar(Rect content, string[] icons, string[] labels, int cur, Action<int> pick)
        {
            int hit = Toolbar.Bar(content, labels.Length, cur, i => icons != null && i < icons.Length ? icons[i] : null, i => labels[i], null, null, false, out _);
            if (hit >= 0) pick(hit);
        }
    }

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
        }

        // ---- 움직임

        struct TearState
        {
            public float T, At, Seen;
        }

        static readonly Dictionary<int, TearState> tears = new();

        /// <summary>
        /// 뜯어져 나오기 진행도(0 ~ 1): show 쪽으로 TearSeconds 동안 시간 비례로 감(그리기 때만). 0.25초 넘게 안 불렸으면 0 에서 다시 — 다시 생기면 또 뜯어져 나옴.
        /// </summary>
        public static float TearProgress(int key, bool show)
        {
            float now = Time.unscaledTime;
            if (!tears.TryGetValue(key, out var s) || now - s.Seen > 0.25f) s = new TearState { At = now };
            s.Seen = now;
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
            SubCell(r, label, on, !enabled, Look.Hover(key, r, enabled && !on), Look.Press(key, r, enabled));
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
                Cell(c, icons[i], null, 0f, sel ? 1f : 0f, Look.Hover(k, c, !sel), Look.Press(k, c));
                CSCommonUX.UI.CSCommonToolbar.Tip(c, tips != null && i < tips.Length ? tips[i] : null);
                if (Draw.Button(c)) pick?.Invoke(i);
            }
            return w;
        }
    }

    /// <summary>FAB 색 종류: 판(설정 등 — 고르면 모드 파스텔) · 모드 파스텔(짓기 · 확인) · 되돌리기 파랑 파스텔 · 취소 빨강 파스텔 · 모드 나가기(판 면 + 빨강 아이콘).</summary>
    public enum FabTone
    {
        Plain,
        Primary,
        Undo,
        Cancel,
        Exit,
    }

    /// <summary>CSCommonUX.Fab — C안 둥근 단추(지름 PillBar.FabSize 64): 아이콘 24 가운데 · 1px 선 · 호버 옅은 덮개 · 누르면 0.96배 · 툴팁(CSCommonToolbar.Tip).</summary>
    public static class Fab
    {
        /// <summary>칸 r 에 그리고 눌러 떼면 true. on = 고름(Plain 이면 파스텔로 채움).</summary>
        public static bool In(Rect r, string icon, FabTone tone = FabTone.Plain, bool on = false, string tip = null)
        {
            int key = CSCommonAnim.Key("fab", icon) ^ (int)tone * 0x01000193; // 자리와 무관 · 색 종류별로 나눔
            float hover = Look.Hover(key, r), press = Look.Press(key, r);
            if (Draw.Repaint)
            {
                Color fill = tone switch
                {
                    FabTone.Undo => Look.FabUndoFill,
                    FabTone.Cancel => Look.FabCancelFill,
                    FabTone.Exit => Look.BarSurface,
                    FabTone.Primary => Look.PastelFill,
                    _ => on ? Look.PastelFill : Look.BarSurface,
                };
                Color ink = tone switch
                {
                    FabTone.Undo => Look.FabUndoInk,
                    FabTone.Cancel => Look.FabCancelInk,
                    FabTone.Exit => Look.FabExitInk,
                    FabTone.Primary => Look.PastelInk,
                    _ => on ? Look.PastelInk : Look.BarOff,
                };
                float rad = Mathf.Min(r.width, r.height) * 0.5f, s = PillBar.IconSize;
                Draw.BeginScale(r.center, Look.PressScale(press, PillBar.PressScale));
                Draw.Round(r, fill, rad);
                if (hover > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Haze, PillBar.HoverAlpha * hover), rad);
                Draw.RoundOutline(r, Look.BarLine, rad);
                if (!string.IsNullOrEmpty(icon)) Draw.Icon(icon, new Rect(Mathf.Round(r.center.x - s * 0.5f), Mathf.Round(r.center.y - s * 0.5f), s, s), ink, 24f, PillBar.Stroke);
                Draw.EndScale();
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            return Draw.Button(r);
        }

        /// <summary>줄 배치(독 fabs 콜백 — x 왼쪽 · cy 가운데 · draw = false 면 폭만): 지름을 돌려주고 눌러 떼면 click.</summary>
        public static float At(float x, float cy, string icon, FabTone tone, bool on, string tip, bool draw, Action click)
        {
            float d = PillBar.FabSize;
            if (draw && In(new Rect(x, Mathf.Round(cy - d * 0.5f), d, d), icon, tone, on, tip)) click?.Invoke();
            return d;
        }
    }

    /// <summary>
    /// CSCommonUX.DockBar_Panel — 독(Dockbar · SubDockBar · 안내 알약) 아래 12 에 붙는 설정 판(독 버튼으로 열고 닫음 — 열린 동안 그 버튼은 파스텔로 켬).
    /// 판 반지름 28 · 안쪽 16 · 줄 사이 16 · 1px Look.BarLine · 그림자 없음, 면 Look.BarSurface. 열 때 위 가장자리 기준으로 높이 0 → 판 높이 0.44초 cubic-bezier(.2,.8,.2,1)
    /// (Modal.Expand 재사용 — 투명도 0.28초) · 12 위에서 내려오며 0.98 → 1배, 닫을 때 거꾸로. 부품: 알약 탭 줄(Tabs, 36) · 5열 칸 격자(Cell — 위 이름 12 · 아래 입력 36).
    /// 쓰는 법: if (DockBarPanel.Begin(id, open, width, height, out var body, out var frame)) { … body 안(안쪽 좌표)에 그림 …; DockBarPanel.End(); } — frame = 지도 클릭 막을 화면 칸.
    /// </summary>
    public static class DockBarPanel
    {
        /// <summary>반지름 · 안쪽 · 줄 사이 · 탭 높이 · 탭 좌우 · 탭 사이 · 탭 글 · 칸 열 수 · 칸 이름 글 · 이름 줄 높이 · 이름↔입력 · 입력 높이 · 열 때 내려오는 거리 · 시작 배율.</summary>
        public const float Radius = 28f, Pad = 16f, Gap = 16f, TabH = 36f, TabPadX = 16f, TabGap = 8f, TabFont = 13.5f;
        public const int Cols = 5;
        public const float LabelFont = 12f, LabelH = 16f, LabelGap = 6f, CellH = 36f, Rise = 12f, ScaleFrom = 0.98f;

        static float anchor = -1f;
        static int anchorFrame = -1;

        /// <summary>Dockbar 가 그릴 때마다 남기는 판 위 y(독 맨 아래 + 12).</summary>
        internal static void Anchor(float y)
        {
            anchor = y;
            anchorFrame = Time.frameCount;
        }

        /// <summary>판 위 y: 이번 · 지난 프레임 Dockbar 아래 + 12(독이 없으면 화면 위 CSCommonDesign.BarTop).</summary>
        public static float Top => anchorFrame >= Time.frameCount - 1 && anchor >= 0f ? anchor : CSCommonDesign.BarTop;

        static readonly Stack<Vector4> saved = new(); // x = 투명도, y = 내린 거리, z = 배율을 썼나
        static readonly Stack<Matrix4x4> savedMatrix = new();

        /// <summary>
        /// 판 그리기 시작(화면 가운데 · 위 = Top): 펼친 정도만큼 잘라 보이고 흐리게 · 내려오며 커짐. 다 닫혔으면 false(End 부르지 말 것).
        /// body = 안쪽 칸(잘라 그리기 안쪽 좌표), frame = 지금 보이는 판의 화면 칸(입력 막기).
        /// </summary>
        public static bool Begin(string id, bool open, float width, float height, out Rect body, out Rect frame)
        {
            float t = Modal.Expand(CSCommonAnim.Key("dockbarpanel", id), open, out float fade);
            body = frame = default;
            if (t <= 0.001f && !open) return false;
            var r = new Rect(PillBar.Left(Draw.Width * 0.5f, width), Top, width, height);
            float dy = Mathf.Round(-Rise * (1f - t)), s = Mathf.Lerp(ScaleFrom, 1f, t);
            saved.Push(new Vector4(Draw.Opacity, dy, s < 1f ? 1f : 0f, 0f));
            Draw.Opacity *= fade;
            Draw.Shift(new Vector2(0f, dy));
            if (s < 1f)
            {
                savedMatrix.Push(GUI.matrix);
                GUIUtility.ScaleAroundPivot(new Vector2(s, s), Draw.ToScreen(new Vector2(r.center.x, r.y)));
            }
            // 1px 선이 잘리지 않게 바깥 1 을 더 둠
            float shown = Mathf.Round((height + 2f) * t);
            frame = new Rect(r.x, r.y + dy, r.width, Mathf.Max(0f, shown - 2f));
            // 앞 판: 밑(창 몸통)의 단추 · 호버가 이 칸 안 클릭을 못 받게(End 에서 EndOverlay). 지금 좌표는 이미 dy 만큼 밀림
            Draw.Block(new Rect(r.x, r.y, r.width, frame.height));
            Draw.BeginOverlay();
            Draw.BeginClip(new Rect(r.x - 1f, r.y - 1f, r.width + 2f, shown), Vector2.zero);
            var plate = new Rect(1f, 1f, width, height);
            if (Draw.Repaint)
            {
                Draw.Round(plate, Look.BarSurface, Radius);
                Draw.RoundOutline(plate, Look.BarLine, Radius);
            }
            body = new Rect(plate.x + Pad, plate.y + Pad, width - Pad * 2f, height - Pad * 2f);
            return true;
        }

        public static void End()
        {
            if (saved.Count == 0) return;
            Draw.EndOverlay();
            Draw.EndClip();
            var v = saved.Pop();
            if (v.z > 0f && savedMatrix.Count > 0) GUI.matrix = savedMatrix.Pop();
            Draw.Shift(new Vector2(0f, -v.y));
            Draw.Opacity = v.x;
        }

        /// <summary>
        /// 알약 탭 줄(높이 36 · 좌우 16 · 사이 8 · 글 13.5 굵게): 고른 칸 = 모드 파스텔 채움, 나머지 = 1px 선 알약(호버 옅은 면) · 누르면 0.96배.
        /// x 왼쪽 · y 위. 눌러서 뗀 번호(없으면 -1), width = 줄 폭.
        /// </summary>
        public static int Tabs(float x, float y, string[] labels, int cur, out float width)
        {
            int hit = -1;
            float x0 = x;
            for (int i = 0; i < labels.Length; i++)
            {
                float w = Mathf.Ceil(Draw.TextWidth(labels[i], TabFont, true)) + TabPadX * 2f;
                var r = new Rect(x, y, w, TabH);
                bool sel = i == cur;
                int k = Look.Key(r, "dockbarpanel.tab", labels[i]);
                float hv = Look.Hover(k, r, !sel), pr = Look.Press(k, r);
                if (Draw.Repaint)
                {
                    float rad = TabH * 0.5f;
                    Draw.BeginScale(r.center, Look.PressScale(pr, PillBar.PressScale));
                    if (sel) Draw.Round(r, Look.PastelFill, rad);
                    else
                    {
                        if (hv > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Haze, PillBar.HoverAlpha * hv), rad);
                        Draw.RoundOutline(r, Look.BarLine, rad);
                    }
                    Draw.Text(r, labels[i], TabFont, sel ? Look.PastelInk : Look.BarInk, true, TextAnchor.MiddleCenter);
                    Draw.EndScale();
                }
                if (Draw.Button(r)) hit = i;
                x += w + TabGap;
            }
            width = labels.Length > 0 ? x - x0 - TabGap : 0f;
            return hit;
        }

        /// <summary>칸 격자 한 줄 높이(이름 + 사이 + 입력).</summary>
        public const float RowH = LabelH + LabelGap + CellH;

        /// <summary>칸 count 개 격자 높이(5열, 줄 사이 16).</summary>
        public static float GridHeight(int count)
        {
            int rows = (count + Cols - 1) / Cols;
            return rows <= 0 ? 0f : rows * RowH + (rows - 1) * Gap;
        }

        /// <summary>5열 칸 격자(사이 16)의 i 번째 칸: 위에 이름(12 · Pal.Dim)을 그리고 아래 입력 자리(높이 36, Textfield · Select · Stepper · Segmented · Switch · Button 이 씀)를 돌려줌.</summary>
        public static Rect Cell(Rect grid, int i, string label)
        {
            float cw = Mathf.Floor((grid.width - Gap * (Cols - 1)) / Cols);
            var c = new Rect(grid.x + (i % Cols) * (cw + Gap), grid.y + (i / Cols) * (RowH + Gap), cw, RowH);
            if (Draw.Repaint && !string.IsNullOrEmpty(label)) Draw.TextFit(new Rect(c.x, c.y, cw, LabelH), label, LabelFont, Pal.Dim);
            return new Rect(c.x, c.y + LabelH + LabelGap, cw, CellH);
        }
    }
}
