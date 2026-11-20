using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
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
                if (value.Length == 0 && !(focus && Draw.Composing) && !string.IsNullOrEmpty(placeholder)) Draw.TextFit(new Rect(r.x, r.y, r.width, r.height - SheetLineFocus), placeholder, SheetTitleFont, Look.OffText);
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
}
