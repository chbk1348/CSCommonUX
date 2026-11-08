using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommon
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

        /// <summary>호버 0 ~ 1(서서히).</summary>
        public static float Hover(int key, Rect r, bool enabled = true) => CSCommonAnim.Toward(key, enabled && Draw.Hover(r) ? 1f : 0f, 12f);

        /// <summary>누름 0 ~ 1(누르는 동안 빠르게).</summary>
        public static float Press(int key, Rect r, bool enabled = true) => CSCommonAnim.Toward(key ^ 0x27d4eb2f, enabled && Draw.Hover(r) && Draw.Pressed(r) ? 1f : 0f, 22f);

        public static int Key(Rect r, string a, string b = null)
        {
            unchecked { return CSCommonAnim.Key(a ?? "", b ?? "") ^ Mathf.RoundToInt(r.x) * 397 ^ Mathf.RoundToInt(r.y) * 31; }
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

    /// <summary>CSCommon.Button — 공통 단추(톤 바탕 + 뜻 색 아이콘). 글에 맞춘 너비(At) · 주어진 칸(In) · 아이콘만 네모(Square).</summary>
    public static class Button
    {
        /// <summary>글에 맞춘 너비(small = 높이 32 · 글 12.5).</summary>
        public static float Width(string icon, string label, bool small = false)
        {
            float font = small ? CSCommonDesign.ActionFontSmall : CSCommonDesign.ActionFont;
            float isz = small ? CSCommonDesign.ActionIconSmall : CSCommonDesign.ActionIcon;
            float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, font));
            float iw = string.IsNullOrEmpty(icon) ? 0f : isz + (tw > 0f ? CSCommonDesign.ActionGap : 0f);
            if (tw <= 0f) return small ? CSCommonDesign.ActionHeightSmall : CSCommonDesign.ActionHeight;
            return (small ? 10f : CSCommonDesign.ActionPadL) + iw + tw + (small ? 12f : CSCommonDesign.ActionPadR);
        }

        /// <summary>줄 가운데 cy · 왼쪽 x 에 글에 맞춘 단추(draw = false 면 재기만). 그린 너비를 돌려줌.</summary>
        public static float At(float x, float cy, string icon, string label, string tip, Tone tone, Color? iconColor = null, bool enabled = true, Action click = null, bool draw = true, bool small = false, bool loading = false)
        {
            float w = Width(icon, label, small);
            float h = small ? CSCommonDesign.ActionHeightSmall : CSCommonDesign.ActionHeight;
            if (draw) In(new Rect(x, Mathf.Round(cy - h * 0.5f), w, h), icon, label, tip, tone, iconColor, enabled, click, false, loading);
            return w;
        }

        /// <summary>주어진 칸에 단추. 아이콘 + 글 묶음은 늘 칸 정가운데(UX 1.1 — left 는 예전 호출 호환용, 무시). 높이 36 미만이면 작은 글 · 아이콘. loading = 글 대신 스피너 · 누름 안 받음.</summary>
        public static void In(Rect r, string icon, string label, string tip, Tone tone, Color? iconColor = null, bool enabled = true, Action click = null, bool left = true, bool loading = false)
        {
            int key = Look.Key(r, icon, label) ^ 0x2f9a6b1d;
            float hv = Look.Hover(key, r, enabled && !loading), pr = Look.Press(key, r, enabled && !loading);
            if (Draw.Repaint)
            {
                // 누르면 0.97배로 줄고 한 단 진해짐(GLDS 눌림)
                Draw.BeginScale(r.center, Mathf.Lerp(1f, CSCommonDesign.PressScale, pr));
                bool text = tone == Tone.Text;
                Color bg;
                if (!enabled) bg = text ? Color.clear : Look.Off;
                // UX 1.1: 주 단추는 주 색으로 꽉 채움 · 호버는 한 단 진하게(라이트) / 밝게(다크)
                else if (tone == Tone.Primary) bg = Color.Lerp(Pal.Primary, Pal.Haze, 0.10f * hv + 0.08f * pr);
                else if (text) bg = Color.Lerp(Draw.Alpha(Pal.Primary, 0.08f * hv), Draw.Alpha(Pal.Primary, 0.12f), pr);
                else bg = Color.Lerp(Color.Lerp(Look.Tone0, Look.Tone1, hv), Look.Tone2, pr);
                if (bg.a > 0f) Draw.Round(r, bg, CSCommonDesign.ActionRadius);
                bool big = r.height >= 36f;
                float font = big ? CSCommonDesign.ActionFont : CSCommonDesign.ActionFontSmall;
                float isz = big ? CSCommonDesign.ActionIcon : CSCommonDesign.ActionIconSmall;
                bool filled = enabled && tone == Tone.Primary;
                Color ink = !enabled ? Look.OffInk : filled ? Pal.OnPrimary : iconColor ?? (text ? Pal.Primary : tone == Tone.Danger ? Pal.Red : Pal.Sub);
                Color textInk = !enabled ? Look.OffText : filled ? Pal.OnPrimary : text ? Pal.PrimaryInk : Color.Lerp(Pal.Txt, Pal.Strong, hv);
                float tw = string.IsNullOrEmpty(label) ? 0f : Mathf.Ceil(Draw.TextWidth(label, font));
                float iw = string.IsNullOrEmpty(icon) ? 0f : isz + (tw > 0f ? CSCommonDesign.ActionGap : 0f);
                // 칸보다 길면 왼쪽 6 부터(넘친 글은 TextFit 이 줄임)
                float x = Mathf.Max(r.x + 6f, Mathf.Round(r.center.x - (iw + tw) * 0.5f));
                if (loading)
                {
                    // 로딩: 글 · 아이콘 대신 구글 서클 인디케이터(아이콘 크기 · 선 2 · 글자색), 폭은 그대로
                    Draw.Spinner(r.center, isz * 0.5f - 1f, 2f, textInk);
                }
                else if (!string.IsNullOrEmpty(icon))
                    Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - isz * 0.5f), isz, isz), ink, 24f, 2f);
                // 글만 있는 가운데 단추는 칸 정가운데(가로 · 세로)
                if (loading) { }
                else if (tw > 0f && iw <= 0f)
                    Draw.TextFit(new Rect(r.x + 6f, r.y, r.width - 12f, r.height), label, font, textInk, false, TextAnchor.MiddleCenter);
                else if (tw > 0f)
                    Draw.TextFit(new Rect(x + iw, r.y, Mathf.Max(0f, r.xMax - x - iw - 6f), r.height), label, font, textInk, false, TextAnchor.MiddleLeft);
                Draw.EndScale();
            }
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
            if (enabled && !loading && Draw.Button(r)) click?.Invoke();
        }

        /// <summary>아이콘만 네모 단추(− · + · 지우기 · 닫기 · ‹ ›). 누르면 true.</summary>
        public static bool Square(Rect r, string icon, string tip = null, Tone tone = Tone.Neutral, bool enabled = true, Color? iconColor = null)
        {
            bool hit = false;
            In(r, icon, null, tip, tone, iconColor ?? (tone == Tone.Neutral ? Pal.Txt : (Color?)null), enabled, () => hit = true, false);
            return hit;
        }

        /// <summary>한 글자 네모(예전 '−' '+' '‹' '›' 단추): 아는 글자는 아이콘, 아니면 글.</summary>
        public static bool Glyph(Rect r, string text, bool enabled = true)
        {
            string icon = Icons.ForGlyph(text);
            if (icon != null) return Square(r, icon, null, Tone.Neutral, enabled);
            bool hit = false;
            In(r, null, text, null, Tone.Neutral, null, enabled, () => hit = true, false);
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

    /// <summary>CSCommon.IconButton — 위 판 아이콘 단추(38): 평소 투명 · 호버 톤 · 켜짐 청록 톤.</summary>
    public static class IconButton
    {
        public static float At(float x, float cy, string icon, bool on, string tip, bool draw, Action click, Color? color = null, bool enabled = true)
        {
            float s = CSCommonDesign.IconBtnSize;
            if (!draw) return s;
            var r = new Rect(x, Mathf.Round(cy - s * 0.5f), s, s);
            int key = Look.Key(r, icon, tip) ^ 0x4cf5ad43;
            float hv = Look.Hover(key, r, enabled), pr = Look.Press(key, r, enabled);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 9f);
            if (Draw.Repaint)
            {
                Draw.Round(r, Draw.Alpha(Look.Tone0, hv * (1f - onT)), CSCommonDesign.RadiusTile);
                if (onT > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Primary, 0.16f * onT), CSCommonDesign.RadiusTile);
                Color fg = !enabled ? Look.OffInk : color ?? Color.Lerp(Color.Lerp(Pal.Sub, Pal.Txt, hv), Pal.Primary, onT);
                Draw.Icon(icon, new Rect(Mathf.Round(r.center.x - 9f), Mathf.Round(r.center.y - 9f + pr), 18f, 18f), fg, 24f, 1.9f);
            }
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
            if (enabled && Draw.Button(r)) click?.Invoke();
            return s;
        }
    }

    /// <summary>
    /// CSCommon.Chip — 필터 알약 32(GLDS 칩 D): 평소 Field + 안쪽 1 Tone1, 고르면 테두리 없이 기본 팔레트 12% + Ink 굵게(color = 팀 색이면 그 색),
    /// ▾(dropdown — 목록을 여는 칩) · ✕(고른 칩에 clear 를 주면), 누르면 0.95배.
    /// </summary>
    public static class Chip
    {
        public static float Width(string label, bool on, bool icon)
            => CSCommonDesign.ChipPadX * 2f + Mathf.Ceil(Draw.TextWidth(label, CSCommonDesign.ChipFont, on)) + (icon ? CSCommonDesign.ChipIconGap + CSCommonDesign.ChipIcon : 0f);

        /// <summary>줄 가운데 cy · 왼쪽 x 에 칩(draw = false 면 재기만). 그린 너비를 돌려줌.</summary>
        public static float At(float x, float cy, string label, bool on, Action click, bool draw = true, string tip = null, Color? color = null, bool dropdown = false, Action clear = null)
        {
            bool canClear = on && clear != null;
            float w = Width(label, on, canClear || dropdown), h = CSCommonDesign.ChipHeight;
            if (!draw) return w;
            var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
            int key = Look.Key(r, "chip", label);
            float hv = Look.Hover(key, r), pr = Look.Press(key, r);
            if (Draw.Repaint)
            {
                Draw.BeginScale(r.center, Mathf.Lerp(1f, CSCommonDesign.ChipPressScale, pr));
                Color fg = on ? color ?? Pal.PrimaryInk : Pal.Txt;
                if (on) Draw.Round(r, Draw.Alpha(color ?? Pal.Primary, 0.12f + 0.06f * hv), h * 0.5f);
                else
                {
                    Draw.Round(r, Color.Lerp(Look.Field, Look.Tone0, hv), h * 0.5f);
                    Draw.RoundOutline(r, Look.Tone1, h * 0.5f);
                }
                float tx = r.x + CSCommonDesign.ChipPadX, tw = Mathf.Ceil(Draw.TextWidth(label, CSCommonDesign.ChipFont, on));
                Draw.Text(new Rect(tx, r.y, tw + 2f, h), label, CSCommonDesign.ChipFont, fg, on, TextAnchor.MiddleLeft);
                if (canClear || dropdown)
                {
                    float s = CSCommonDesign.ChipIcon;
                    Draw.Icon(canClear ? Icons.Close : Icons.Chevron, new Rect(tx + tw + CSCommonDesign.ChipIconGap, Mathf.Round(r.center.y - s * 0.5f), s, s), on ? fg : Pal.Sub, 24f, 2.4f);
                }
                Draw.EndScale();
            }
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
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

    /// <summary>CSCommon.Badge — 상태 알약(그 색 16% 바탕 · 그 색 글) · 수 알약(주황).</summary>
    public static class Badge
    {
        public static float At(float x, float cy, string text, Color color, string icon = null, bool draw = true, float height = 24f)
        {
            float font = CSCommonDesign.FontMin;
            float tw = Mathf.Ceil(Draw.TextWidth(text, font, true)), iw = icon != null ? 13f + 5f : 0f;
            float w = 10f + iw + tw + 10f;
            if (draw && Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - height * 0.5f), w, height);
                Draw.Round(r, Draw.Alpha(color, 0.16f), height * 0.5f);
                float tx = r.x + 10f;
                if (icon != null) { Draw.Icon(icon, new Rect(tx, r.center.y - 6.5f, 13f, 13f), color, 24f, 2f); tx += iw; }
                Draw.Text(new Rect(tx, r.y, tw + 2f, r.height), text, font, color, true, TextAnchor.MiddleLeft);
            }
            return w;
        }

        /// <summary>주어진 칸에 상태 알약(그 색 16% 바탕 · 그 색 굵은 글, 글 크기는 칸 높이에 맞춤).</summary>
        public static void In(Rect r, string text, Color color)
        {
            if (!Draw.Repaint || string.IsNullOrEmpty(text)) return;
            Draw.Round(r, Draw.Alpha(color, 0.16f), r.height * 0.5f);
            Draw.TextFit(r, text, CSCommonDesign.FontMin, color, true, TextAnchor.MiddleCenter);
        }

        /// <summary>수 알약(18 · 주황 · 고정폭 숫자).</summary>
        public static float Count(float x, float cy, string text, bool draw = true, bool on = false)
        {
            float w = Mathf.Max(18f, Draw.TextWidth(text, CSCommonDesign.FontMin, true) + 12f);
            if (draw && Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - 9f), w, 18f);
                Draw.Round(r, on ? Draw.Alpha(Pal.Warn, 0.26f) : Look.CountBg, CSCommonDesign.RadiusField);
                Draw.Text(r, text, CSCommonDesign.FontMin, Pal.Warn, true, TextAnchor.MiddleCenter);
            }
            return w;
        }
    }

    /// <summary>CSCommon.Tabs — 묶음 탭: 홈 안에서 켜짐 = 떠오른 면(Look.Raised, 미끄러져 옮겨 감) · 굵은 글, 수 · 점수 알약.</summary>
    public static class Tabs
    {
        public static float At(float x, float cy, string[] icons, string[] labels, int cur, string[] badges, Color[] badgeColors, Color[] accents, string[] tips, bool draw, Action<int> pick)
        {
            float h = CSCommonDesign.TabHeight, pad = 4f, gap = 2f;
            const float font = 13.5f;
            var widths = new float[labels.Length];
            float total = pad * 2f;
            for (int i = 0; i < labels.Length; i++)
            {
                float bw = badges != null && !string.IsNullOrEmpty(badges[i]) ? 8f + Mathf.Max(18f, Draw.TextWidth(badges[i], CSCommonDesign.FontMin, true) + 12f) : 0f;
                float iw = icons != null && !string.IsNullOrEmpty(icons[i]) ? 16f + 8f : 0f; // 아이콘 없는 탭은 자리도 안 잡음(글이 가운데)
                widths[i] = CSCommonDesign.TabPadX * 2f + iw + Mathf.Ceil(Draw.TextWidth(labels[i], font, true)) + bw;
                total += widths[i] + (i > 0 ? gap : 0f);
            }
            if (!draw) return total;
            var frame = new Rect(x, Mathf.Round(cy - (h + pad * 2f) * 0.5f), total, h + pad * 2f);
            if (Draw.Repaint) Draw.Round(frame, Look.Field, CSCommonDesign.IconBtnRadius);
            int skey = CSCommonAnim.Key(labels[0], labels[labels.Length - 1]) ^ 0x7a3c1e5d;
            if (cur >= 0 && cur < labels.Length)
            {
                float cx0 = pad;
                for (int i = 0; i < cur; i++) cx0 += widths[i] + gap;
                float px = CSCommonAnim.Smooth(skey, cx0), pw = CSCommonAnim.Smooth(skey ^ 0x51ed270b, widths[cur]);
                if (Draw.Repaint)
                {
                    var sel = new Rect(frame.x + px, frame.y + pad, pw, h);
                    Look.Raised(sel, 8f);
                }
            }
            float ix = x + pad;
            for (int i = 0; i < labels.Length; i++)
            {
                var r = new Rect(ix, frame.y + pad, widths[i], h);
                bool on = i == cur;
                float hv = CSCommonAnim.Toward(skey ^ (i + 1) * 0x2f6b8d3, Draw.Hover(r) && !on ? 1f : 0f, 12f);
                float pr = Look.Press(skey ^ (i + 7) * 0x1b56c4e9, r);
                if (Draw.Repaint)
                {
                    if (hv > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Haze, 0.05f * hv), CSCommonDesign.RadiusTile);
                    Color fg = on ? Pal.OnSelected : Color.Lerp(Pal.Sub, Pal.Txt, hv);
                    float tx = r.x + CSCommonDesign.TabPadX, yy = r.center.y + pr;
                    if (icons != null && !string.IsNullOrEmpty(icons[i]))
                    {
                        Draw.Icon(icons[i], new Rect(tx, Mathf.Round(yy - 8f), 16f, 16f), fg, 24f, 1.8f);
                        tx += 24f;
                    }
                    float lw = Mathf.Ceil(Draw.TextWidth(labels[i], font, true));
                    Draw.Text(new Rect(tx, r.y + pr, lw + 2f, r.height), labels[i], font, fg, on, TextAnchor.MiddleLeft);
                    if (badges != null && !string.IsNullOrEmpty(badges[i]))
                    {
                        Color bc = badgeColors != null ? badgeColors[i] : Pal.Warn;
                        float bw = Mathf.Max(18f, Draw.TextWidth(badges[i], CSCommonDesign.FontMin, true) + 12f);
                        var pill = new Rect(tx + lw + 8f, Mathf.Round(yy - 9f), bw, 18f);
                        Draw.Round(pill, Draw.Alpha(bc, 0.18f), CSCommonDesign.RadiusField);
                        Draw.Text(pill, badges[i], CSCommonDesign.FontMin, bc, true, TextAnchor.MiddleCenter);
                    }
                }
                if (tips != null) CSCommon.UI.CSCommonToolbar.Tip(r, tips[i]);
                if (Draw.Button(r)) pick?.Invoke(i);
                ix += widths[i] + gap;
            }
            return total;
        }
    }

    /// <summary>CSCommon.Segmented — 하나 고르기: 홈 #1B1F26 · 켜짐 칸 떠오른 면(Look.Raised, 미끄러져 옮겨 감) · 굵은 글, 높이 34 · 글 13.5.</summary>
    public static class Segmented
    {
        public const float H = 34f, Pad = 3f, Gap = 2f, Font = 13.5f;

        /// <summary>홈: 판 색 #14171C + 1px 선 — 카드(#1B1F26) 위에서도 홈이 보이게(예전 Field 는 카드와 같은 색이라 칸만 흩어져 보였음).</summary>
        static void Groove(Rect r)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, Pal.Win, CSCommonDesign.RadiusField);
            Draw.RoundOutline(r, Look.PanelLine, CSCommonDesign.RadiusField);
        }

        public static float At(float x, float cy, string[] labels, int cur, string[] tips, Color?[] dots, bool draw, Action<int> pick)
        {
            var widths = new float[labels.Length];
            float total = Pad * 2f;
            for (int i = 0; i < labels.Length; i++)
            {
                widths[i] = 28f + Mathf.Ceil(Draw.TextWidth(labels[i], Font, true)) + (dots != null && dots[i].HasValue ? 14f : 0f);
                total += widths[i] + (i > 0 ? Gap : 0f);
            }
            if (!draw) return total;
            var frame = new Rect(x, Mathf.Round(cy - (H + Pad * 2f) * 0.5f), total, H + Pad * 2f);
            Groove(frame);
            int skey = CSCommonAnim.Key(labels[0], labels[labels.Length - 1]) ^ labels.Length * 7919 ^ Mathf.RoundToInt(cy);
            if (cur >= 0 && cur < labels.Length)
            {
                float cx0 = Pad;
                for (int i = 0; i < cur; i++) cx0 += widths[i] + Gap;
                float px = CSCommonAnim.Smooth(skey, cx0), pw = CSCommonAnim.Smooth(skey ^ 0x51ed270b, widths[cur]);
                if (Draw.Repaint)
                {
                    var sel = new Rect(frame.x + px, frame.y + Pad, pw, H);
                    Look.Raised(sel, 7f);
                }
            }
            float ix = x + Pad;
            for (int i = 0; i < labels.Length; i++)
            {
                var r = new Rect(ix, frame.y + Pad, widths[i], H);
                bool on = i == cur;
                float hv = CSCommonAnim.Toward(skey ^ (i + 1) * 0x2f6b8d3, Draw.Hover(r) && !on ? 1f : 0f, 12f);
                if (Draw.Repaint)
                {
                    if (hv > 0.01f) Draw.Round(r, Draw.Alpha(Pal.Haze, 0.05f * hv), CSCommonDesign.RadiusTool);
                    float tx = r.x + 14f;
                    if (dots != null && dots[i].HasValue) { Draw.Dot(new Vector2(tx + 4f, r.center.y), 4f, dots[i].Value); tx += 14f; }
                    Draw.Text(new Rect(tx, r.y, r.xMax - tx, r.height), labels[i], Font, on ? Pal.OnSelected : Color.Lerp(Pal.Sub, Pal.Txt, hv), on, TextAnchor.MiddleLeft);
                }
                if (tips != null) CSCommon.UI.CSCommonToolbar.Tip(r, tips[i]);
                if (Draw.Button(r)) pick?.Invoke(i);
                ix += widths[i] + Gap;
            }
            return total;
        }

        /// <summary>칸 폭을 똑같이 나눔(창 위 고르기 줄 · 에셋 자리 등).</summary>
        /// <summary>subs = 이름 아래 작은 글(칸 높이 SegmentTwoH) · colors = 고른 칸 이름 색(팀 · 세션 색).</summary>
        public static void In(Rect r, string[] labels, int cur, Action<int> pick, string[] tips = null, string[] subs = null, Color?[] colors = null)
        {
            Groove(r);
            float w = (r.width - Pad * 2f - Gap * (labels.Length - 1)) / labels.Length, h = r.height - Pad * 2f;
            int skey = CSCommonAnim.Key(labels[0], labels[labels.Length - 1]) ^ 0x3c6ef372 ^ Mathf.RoundToInt(r.y);
            if (cur >= 0 && cur < labels.Length && Draw.Repaint)
            {
                float px = CSCommonAnim.Smooth(skey, cur * (w + Gap));
                var sel = new Rect(r.x + Pad + px, r.y + Pad, w, h);
                Look.Raised(sel, 7f);
            }
            for (int i = 0; i < labels.Length; i++)
            {
                var c = new Rect(r.x + Pad + i * (w + Gap), r.y + Pad, w, h);
                bool on = i == cur;
                if (Draw.Repaint)
                {
                    if (!on && Draw.Hover(c)) Draw.Round(c, Draw.Alpha(Pal.Haze, 0.05f), CSCommonDesign.RadiusTool);
                    Color fg = on ? (colors != null && i < colors.Length && colors[i].HasValue ? colors[i].Value : Pal.OnSelected) : Draw.Hover(c) ? Pal.Txt : Pal.Sub;
                    string sub = subs != null && i < subs.Length ? subs[i] : null;
                    if (string.IsNullOrEmpty(sub)) Draw.TextFit(c, labels[i], Font, fg, on, TextAnchor.MiddleCenter);
                    else
                    {
                        // 이름 18 + 사이 2 + 작은 글 16 을 가운데로
                        float top = Mathf.Round(c.center.y - 18f);
                        Draw.TextFit(new Rect(c.x + 4f, top, c.width - 8f, 18f), labels[i], Font, fg, on, TextAnchor.MiddleCenter);
                        Draw.TextFit(new Rect(c.x + 4f, top + 20f, c.width - 8f, 16f), sub, CSCommonDesign.FontMin, Pal.Dim, false, TextAnchor.MiddleCenter);
                    }
                }
                if (tips != null) CSCommon.UI.CSCommonToolbar.Tip(c, tips[i]);
                if (Draw.Button(c) && !on) pick?.Invoke(i);
            }
        }
    }

    /// <summary>
    /// CSCommon.Textfield — 글 칸: 바탕 #1B1F26, 초점 = 청록 테두리, 잘못 = 빨강 테두리, 자리 글 · 앞 아이콘,
    /// 앞뒤 단위(값이 있을 때만 · 굵은 회색) · 지우기 ✕(초점 · 값이 있을 때). 꺼진 칸은 오류 테두리를 안 보임.
    /// </summary>
    public static class Textfield
    {
        /// <summary>글 칸(name = 초점 이름, 없으면 칸 자리로). 바뀐 글을 돌려줌.</summary>
        public static string In(Rect r, string value, int maxLength, string placeholder = null, string icon = null, bool error = false, bool enabled = true, string name = null, float font = 14.5f,
            string prefix = null, string suffix = null, bool clearable = false)
        {
            name ??= "csf" + Look.Key(r, "field");
            value ??= "";
            bool focus = GUI.GetNameOfFocusedControl() == name;
            bool has = value.Length > 0;
            if (Draw.Repaint)
            {
                Draw.Round(r, !enabled ? Look.Off : focus ? Look.FieldFocus : Look.Field, CSCommonDesign.ActionRadius);
                if (!enabled) { }
                else if (error) Draw.RoundOutline(r, Pal.Red, CSCommonDesign.ActionRadius, CSCommonDesign.FieldFocusW);
                else if (focus) Draw.RoundOutline(r, Pal.Primary, CSCommonDesign.ActionRadius, CSCommonDesign.FieldFocusW);
                // UX 1.1: 입력 칸은 늘 테두리(FieldLine 3:1 이상 · 상하좌우 같은 두께), 호버는 한 단 진하게
                else Draw.RoundOutline(r, Draw.Hover(r) ? Pal.Sub : Look.FieldLine, CSCommonDesign.ActionRadius, CSCommonDesign.FieldLineW);
            }
            float lead = 0f;
            if (!string.IsNullOrEmpty(icon))
            {
                if (Draw.Repaint) Draw.Icon(icon, new Rect(r.x + 12f, r.center.y - 8f, 16f, 16f), Pal.Dim, 24f, 2f);
                lead = 24f;
            }
            Color unit = enabled ? Pal.Sub : Look.OffText;
            // 앞 단위: 값 앞(값이 비면 숨겨 자리 글과 붙어 읽히지 않게)
            if (has && !string.IsNullOrEmpty(prefix))
            {
                float pw = Mathf.Ceil(Draw.TextWidth(prefix, font, true));
                if (Draw.Repaint) Draw.Text(new Rect(r.x + lead + 10f, r.y, pw + 2f, r.height), prefix, font, unit, true);
                lead += pw + CSCommonDesign.FieldAffixGap;
            }
            // 지우기 ✕: 오른쪽 끝(초점 · 값이 있을 때)
            float trail = 0f;
            bool clear = clearable && enabled && focus && has;
            var clearR = new Rect(r.xMax - 12f - CSCommonDesign.FieldClear, r.center.y - CSCommonDesign.FieldClear * 0.5f, CSCommonDesign.FieldClear, CSCommonDesign.FieldClear);
            if (clear) trail = CSCommonDesign.FieldClear + 8f;
            // 뒤 단위: 값 바로 뒤(넘치면 오른쪽 끝에 붙음)
            float sw = has && !string.IsNullOrEmpty(suffix) ? Mathf.Ceil(Draw.TextWidth(suffix, font, true)) : 0f;
            if (sw > 0f) trail += sw + CSCommonDesign.FieldAffixGap;
            var fr = new Rect(r.x + lead, r.y, r.width - lead - trail, r.height);
            if (sw > 0f && Draw.Repaint)
            {
                float vx = fr.x + 10f + Mathf.Ceil(Draw.TextWidth(value, font)) + CSCommonDesign.FieldAffixGap;
                Draw.Text(new Rect(Mathf.Min(vx, fr.xMax - 8f + CSCommonDesign.FieldAffixGap), r.y, sw + 2f, r.height), suffix, font, unit, true);
            }
            if (!enabled)
            {
                if (Draw.Repaint) Draw.TextFit(new Rect(fr.x + 12f, fr.y, fr.width - 24f, fr.height), value, font, Look.OffText);
                return value;
            }
            if (clear)
            {
                if (Draw.Repaint) Draw.Icon(Icons.Close, clearR, Look.OffText, 24f, 2f);
                // 누름 영역은 그림보다 넓게(28), 눌림 표시 없음
                if (Draw.Button(new Rect(clearR.x - 6f, r.y, clearR.width + 12f, r.height))) return "";
            }
            if (!has && !string.IsNullOrEmpty(placeholder) && Draw.Repaint)
                Draw.TextFit(new Rect(fr.x + 12f, fr.y, fr.width - 24f, fr.height), placeholder, font, Look.OffText);
            GUI.SetNextControlName(name);
            return Draw.Field(fr, value, font, maxLength);
        }

        /// <summary>이름(위) · 칸 · 도움말(아래, 잘못이면 빨강). 칸 높이 40 + 위 18 + 아래 18.</summary>
        public static string Labeled(Rect r, string label, string value, int maxLength, string help = null, string placeholder = null, bool error = false, bool enabled = true, string name = null)
        {
            if (Draw.Repaint) Draw.Text(new Rect(r.x, r.y, r.width, 16f), label, 12f, Pal.Sub);
            var box = new Rect(r.x, r.y + 20f, r.width, CSCommonDesign.ActionHeight);
            string v = In(box, value, maxLength, placeholder, null, error, enabled, name);
            if (!string.IsNullOrEmpty(help) && Draw.Repaint) Draw.TextFit(new Rect(r.x, box.yMax + 4f, r.width, 16f), help, CSCommonDesign.FontMin, error ? Pal.Red : Pal.Dim);
            return v;
        }
    }

    /// <summary>
    /// CSCommon.Select — 목록에서 고르기: 칸(색 점 · 글 · ▾) · 누르면 칸 아래로 목록(왼쪽 표시 ○ 하나 · □ 여럿, 고른 줄 굵은 흰 글 · 호버 톤).
    /// 펼칠 때 칸 쪽에서 0.92 → 1배 + 페이드. 하나 고르기는 고르면 닫힘 · 여럿 고르기(Many)는 열어 둠 · 밖을 누르면 닫힘.
    /// 목록은 다른 칸보다 위에 그려야 하므로 그 프레임 끝 Flush() 에서 그림(창 · 판 OnGUI 끝에서 부름).
    /// </summary>
    public static class Select
    {
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

        public static bool AnyOpen => openKey != 0;

        /// <summary>열린 목록을 닫음(탭을 바꾸거나 스크롤할 때).</summary>
        public static void Close()
        {
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
            if (openKey == 0 || itemRects.Count == 0) return;
            var e = Event.current;
            if (e == null) return;
            Vector2 m = e.mousePosition / Draw.S;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (menuRect.Contains(m))
                {
                    pressed = itemRects.FindIndex(it => it.Contains(m));
                    if (pressed < 0) pressed = -2; // 판 여백 — 쓰기만
                    e.Use();
                }
                else if (!openRect.Contains(m)) Close();
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
                    if (openOn == null) Close();
                    pick?.Invoke(i);
                }
            }
        }

        /// <summary>하나 고르기 칸(bg = 닫힌 칸 바탕, 없으면 Field). heads[i] = 줄 i 위에 둘 묶음 머리(없으면 null).</summary>
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
            if (Draw.Repaint)
            {
                Draw.Round(r, !enabled ? Look.Off : open ? Look.FieldFocus : bg ?? Look.Field, CSCommonDesign.ActionRadius);
                if (open) Draw.RoundOutline(r, Pal.Primary, CSCommonDesign.ActionRadius, CSCommonDesign.FieldFocusW);
                else if (enabled) Draw.RoundOutline(r, Draw.Hover(r) ? Pal.Sub : Look.FieldLine, CSCommonDesign.ActionRadius, CSCommonDesign.FieldLineW);
                float tx = r.x + 12f;
                if (dot.HasValue) { Draw.Dot(new Vector2(tx + 4f, r.center.y), 4f, enabled ? dot.Value : Look.OffInk); tx += 16f; }
                Draw.TextFit(new Rect(tx, r.y, r.xMax - tx - 34f, r.height), text, 14.5f, enabled ? Pal.Txt : Look.OffText);
                Draw.Icon(Icons.Chevron, new Rect(r.xMax - 28f, r.center.y - 8f, 16f, 16f), enabled ? Pal.Sub : Look.OffInk, 24f, 2f);
            }
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
            if (!enabled) return;
            bool hit = Draw.Button(r);
            if (hit && open)
            {
                openKey = 0;
                return;
            }
            if (hit)
            {
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

        /// <summary>열린 목록을 맨 위에 그림 · 하나 고르기는 고르면 닫음 · 밖을 누르면 닫음.</summary>
        public static void Flush()
        {
            if (openKey == 0 || openItems == null) return;
            // 목록을 연 칸이 이번(또는 바로 앞) 프레임에 안 그려졌으면 그 창이 닫힌 것 — 목록만 남아 잔상으로 보이던 것(열린 채 X 로 닫기)
            if (Time.frameCount - openSeenFrame > 1) { Close(); return; }
            float row = CSCommonDesign.MenuRowH, pad = CSCommonDesign.MenuPad, mark = CSCommonDesign.MenuMark;
            bool many = openOn != null;
            // 묶음 머리: 첫 묶음은 머리 줄만, 다음 묶음부터 위 구분선 + 4
            float HeadAt(int i) => openHeads != null && i < openHeads.Length && !string.IsNullOrEmpty(openHeads[i]) ? CSCommonDesign.MenuHeadH + (i > 0 ? 4f : 0f) : 0f;
            float body = 0f;
            for (int i = 0; i < openItems.Length; i++) body += HeadAt(i) + row;
            var menu = new Rect(openRect.x, openRect.yMax + 6f, Mathf.Max(openRect.width, 200f), pad * 2f + body);
            bool above = menu.yMax > Draw.Height - 8f;
            if (above) menu.y = openRect.y - 6f - menu.height;
            menuRect = menu;
            itemRects.Clear();
            float t = CSCommonAnim.EaseOut((Time.unscaledTime - openAt) / CSCommonDesign.MenuOpenSeconds);
            float opacity = Draw.Opacity;
            Draw.Opacity *= t;
            Draw.BeginScale(new Vector2(menu.x, above ? menu.yMax : menu.y), Mathf.Lerp(CSCommonDesign.MenuOpenScale, 1f, t));
            Look.Elevated(menu, 10f, Look.FieldFocus);
            // 펼친 목록도 칸과 같은 테두리(떠 있는 판 선은 옅어 목록 끝이 안 보였음)
            Draw.RoundOutline(menu, Look.FieldLine, 10f, CSCommonDesign.FieldLineW);
            int picked = -1;
            float iy = menu.y + pad;
            for (int i = 0; i < openItems.Length; i++)
            {
                float hh = HeadAt(i);
                if (hh > 0f && Draw.Repaint)
                {
                    if (i > 0) Draw.Fill(new Rect(menu.x + pad, iy + 2f, menu.width - pad * 2f, 1f), Look.Divider);
                    Draw.TextFit(new Rect(menu.x + pad + 10f, iy + hh - CSCommonDesign.MenuHeadH, menu.width - pad * 2f - 20f, CSCommonDesign.MenuHeadH), openHeads[i], CSCommonDesign.FontMin, Pal.Sub, true);
                }
                iy += hh;
                var it = new Rect(menu.x + pad, iy, menu.width - pad * 2f, row);
                itemRects.Add(it);
                iy += row;
                bool on = many ? i < openOn.Length && openOn[i] : i == openCur, hov = Draw.Hover(it);
                if (Draw.Repaint)
                {
                    if (hov) Draw.Round(it, Look.Tone0, CSCommonDesign.RadiusTool);
                    var m = new Rect(it.x + 10f, Mathf.Round(it.center.y - mark * 0.5f), mark, mark);
                    if (many) Box(m, on);
                    else Radio(m, on);
                    float tx = m.xMax + 10f;
                    if (openDots != null && i < openDots.Length && openDots[i].HasValue) { Draw.Dot(new Vector2(tx + 4f, it.center.y), 4f, openDots[i].Value); tx += 16f; }
                    Draw.TextFit(new Rect(tx, it.y, it.xMax - tx - 10f, it.height), openItems[i], 14f, on || hov ? Pal.Strong : Pal.Light, on);
                }
                if (picked < 0 && Draw.Button(it)) picked = i;
            }
            Draw.EndScale();
            Draw.Opacity = opacity;
            if (picked >= 0)
            {
                var pick = openPick;
                if (!many) openKey = 0;
                pick?.Invoke(picked);
                return;
            }
            var e = Event.current;
            if (e.type == EventType.MouseDown && !menu.Contains(Draw.Mouse) && !openRect.Contains(Draw.Mouse)) openKey = 0;
        }

        /// <summary>하나 고르기 표시: 안 고름 = 회색 고리 · 고름 = 청록 원 + 바탕색 점.</summary>
        static void Radio(Rect m, bool on)
        {
            if (on)
            {
                Draw.Dot(m.center, m.width * 0.5f, Pal.Primary);
                Draw.Dot(m.center, 3.5f, Pal.OnPrimary);
            }
            else Draw.Ring(m.center, m.width * 0.5f - 0.75f, 1.5f, Pal.Dim);
        }

        /// <summary>여럿 고르기 표시: 안 고름 = 회색 테두리 상자 · 고름 = 청록 상자 + 바탕색 체크.</summary>
        static void Box(Rect m, bool on)
        {
            if (on)
            {
                Draw.Round(m, Pal.Primary, CSCommonDesign.RadiusChip);
                Draw.Icon(Icons.Check, new Rect(m.x + 2.5f, m.y + 2.5f, m.width - 5f, m.height - 5f), Pal.OnPrimary, 24f, 2.6f);
            }
            else Draw.RoundOutline(m, Pal.Dim, CSCommonDesign.RadiusChip, 1.5f);
        }
    }

    /// <summary>CSCommon.Stepper — − 값 +: 홈 안에 네모 단추 둘 · 가운데 고정폭 숫자, 끝이면 그쪽 단추 꺼짐(동작 null).</summary>
    public static class Stepper
    {
        public static void In(Rect r, string value, Action minus, Action plus, string minusTip = null, string plusTip = null)
        {
            if (Draw.Repaint) Draw.Round(r, Look.Field, CSCommonDesign.RadiusField);
            float s = r.height - 6f;
            if (Button.Square(new Rect(r.x + 3f, r.y + 3f, s, s), Icons.Minus, minusTip, Tone.Neutral, minus != null)) minus?.Invoke();
            if (Draw.Repaint) Draw.TextFit(new Rect(r.x + 3f + s, r.y, r.width - 6f - s * 2f, r.height), value, 14f, Pal.Txt, true, TextAnchor.MiddleCenter);
            if (Button.Square(new Rect(r.xMax - 3f - s, r.y + 3f, s, s), Icons.Plus, plusTip, Tone.Neutral, plus != null)) plus?.Invoke();
        }
    }

    /// <summary>CSCommon.Switch — 켜고 끄기(46 × 26): 켜짐 청록, 꺼짐(쓸 수 없음) 흐리게. 누르면 true.</summary>
    public static class Switch
    {
        public static bool In(Rect r, bool on, bool enabled = true)
        {
            int key = Look.Key(r, "switch");
            float t = CSCommonAnim.Toward(key, on ? 1f : 0f, 14f);
            if (Draw.Repaint)
            {
                Color track = !enabled ? new Color(1f, 1f, 1f, 0.05f) : Color.Lerp(Look.Tone1, Draw.Alpha(Pal.Primary, 0.35f), t);
                Draw.Round(r, track, r.height * 0.5f);
                float k = r.height - 6f;
                float cx = Mathf.Lerp(r.x + 3f + k * 0.5f, r.xMax - 3f - k * 0.5f, t);
                Draw.Dot(new Vector2(cx, r.center.y), k * 0.5f, !enabled ? Look.OffInk : Color.Lerp(Pal.Sub, Pal.Primary, t));
            }
            return enabled && Draw.Button(r);
        }
    }

    /// <summary>
    /// CSCommon.Slider — 끌기 막대(가이드 · F1 보드 'CSCommon 컴포넌트 · 입력'): 막대 4 · 채움 = 뜻 색 · 손잡이 16 흰 원 + 채움 색 테두리 2(그림자 없음 — 다크 · 라이트 같은 모양).
    /// 호버 18 + 옅은 고리 6, 끄는 중 고리 8 + 값 말풍선(bubble), 12단계 이하는 점 눈금(손잡이가 눈금에 붙음), 꺼짐 40%.
    /// 호버 중 ← → 키로 한 단계. 손을 떼면 released.
    /// </summary>
    public static class Slider
    {
        const float TrackH = 4f, Knob = 8f, KnobHover = 9f, Border = 2f, RingHover = 6f, RingDrag = 8f, MaxTicks = 12f;

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
            if (!Draw.Repaint) return value;

            bool drag = sliding && slideKey == key;
            float old = Draw.Opacity;
            if (!enabled) Draw.Opacity = old * 0.4f;
            float t = max > min ? Mathf.Clamp01((value - min) / (max - min)) : 0f;
            var track = new Rect(r.x, Mathf.Round(r.center.y - TrackH * 0.5f), r.width, TrackH);
            Draw.Round(track, Look.Tone1, TrackH * 0.5f);
            if (t > 0f) Draw.Round(new Rect(track.x, track.y, Mathf.Max(TrackH, track.width * t), TrackH), fill, TrackH * 0.5f);
            // 단계 눈금(12단계 이하): 채운 쪽은 흰빛, 빈 쪽은 흐린 점
            if (step > 0f && max > min && (max - min) / step <= MaxTicks)
            {
                int n = Mathf.RoundToInt((max - min) / step);
                for (int i = 0; i <= n; i++)
                {
                    float tt = i / (float)n;
                    Draw.Dot(new Vector2(track.x + track.width * tt, track.center.y), 1f, tt <= t + 0.001f ? Draw.Alpha(Color.white, 0.75f) : Look.OffText);
                }
            }
            var knob = new Vector2(Mathf.Round(track.x + track.width * t), track.center.y);
            float rad = hover || drag ? KnobHover : Knob;
            if (drag) Draw.Dot(knob, rad + RingDrag, Draw.Alpha(fill, 0.26f));
            else if (hover) Draw.Dot(knob, rad + RingHover, Draw.Alpha(fill, 0.16f));
            Draw.Dot(knob, rad, fill);
            Draw.Dot(knob, rad - Border, Color.white);
            if (drag && !string.IsNullOrEmpty(bubble))
            {
                float bw = Draw.TextWidth(bubble, 12f, true) + 16f;
                var b = new Rect(Mathf.Round(knob.x - bw * 0.5f), Mathf.Round(knob.y - rad - RingDrag - 22f), bw, 20f);
                Draw.Round(b, fill, 6f);
                Draw.Text(b, bubble, 12f, Draw.On(fill), true, TextAnchor.MiddleCenter);
            }
            Draw.Opacity = old;
            return value;
        }
    }

    /// <summary>CSCommon.Swatch — 색 칸 28: 고른 색은 Strong 고리(다크 흰 · 라이트 검정). 누르면 true.</summary>
    public static class Swatch
    {
        public static bool In(Rect r, Color color, bool on, string tip = null)
        {
            if (Draw.Repaint)
            {
                if (on)
                {
                    // 고리 = 가장 진한 글 색(다크 흰색 · 라이트 거의 검정 — 라이트 흰 바탕에 흰 고리가 안 보였음)
                    Draw.Round(new Rect(r.x - 4f, r.y - 4f, r.width + 8f, r.height + 8f), Pal.Strong, CSCommonDesign.IconBtnRadius);
                    Draw.Round(new Rect(r.x - 2f, r.y - 2f, r.width + 4f, r.height + 4f), Look.FieldFocus, 8.5f);
                }
                else if (Draw.Hover(r)) Draw.Round(new Rect(r.x - 2f, r.y - 2f, r.width + 4f, r.height + 4f), Look.Tone1, 8.5f);
                Draw.Round(r, color, CSCommonDesign.RadiusTool);
            }
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
            return Draw.Button(r);
        }
    }

    /// <summary>CSCommon.Stat — 숫자 칸(이름 · 큰 숫자 · 설명) · 숫자 줄(이름 왼쪽 · 값 오른쪽).</summary>
    public static class Stat
    {
        public static void In(Rect r, string label, string value, string sub = null, Color? color = null)
        {
            if (!Draw.Repaint) return;
            // Flatten: 회색 타일을 걷고 이름 · 값 · 설명만
            Draw.TextFit(new Rect(r.x + 14f, r.y + 10f, r.width - 28f, 16f), label, 12f, Pal.Sub);
            Draw.TextFit(new Rect(r.x + 14f, r.y + 28f, r.width - 28f, 30f), value, 24f, color ?? Pal.Txt, true);
            if (!string.IsNullOrEmpty(sub)) Draw.TextFit(new Rect(r.x + 14f, r.y + 58f, r.width - 28f, 15f), sub, CSCommonDesign.FontMin, Pal.Dim);
        }

        public static void Line(Rect r, string label, string value, Color? color = null)
        {
            if (!Draw.Repaint) return;
            Draw.Text(new Rect(r.x, r.y, r.width * 0.5f, r.height), label, 12.5f, Pal.Sub, false, TextAnchor.MiddleLeft);
            Draw.TextFit(new Rect(r.x + r.width * 0.4f, r.y, r.width * 0.6f, r.height), value, 12.5f, color ?? Pal.Txt, true, TextAnchor.MiddleRight);
        }
    }

    /// <summary>
    /// CSCommon.Metric — GLDS 2.1 지표 칸: 타일 면 없이 값 15 굵게 위 · 라벨 12 Sub 아래(설명은 그 아래 12 Dim), 왼쪽 정렬 · 들임 없음.
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
    /// CSCommon.ShareRow — 비중 줄(GLDS): 이름 14 · 값 15 굵게 · 비율 12 Sub(폭 40 오른쪽), 아래 막대 6(Tone1 홈 · 기본 팔레트, color 를 주면 그 색 + 이름 앞 점 8).
    /// r 높이 = ShareRowH. share = 0 ~ 1, ratio 를 주면 비율 글 대신 그 글.
    /// </summary>
    public static class ShareRow
    {
        public static void In(Rect r, string name, string value, float share, Color? color = null, string ratio = null)
        {
            if (!Draw.Repaint) return;
            share = Mathf.Clamp01(share);
            const float lh = 20f, pw = 40f;
            float top = r.y + CSCommonDesign.ShareRowPad, tx = r.x;
            if (color.HasValue)
            {
                Draw.Dot(new Vector2(tx + 4f, top + lh * 0.5f), 4f, color.Value);
                tx += 16f;
            }
            float vw = Mathf.Ceil(Draw.TextWidth(value, 15f, true));
            Draw.TextFit(new Rect(tx, top, r.xMax - tx - pw - vw - 12f, lh), name, 14f, Pal.Txt);
            Draw.Text(new Rect(r.xMax - pw - vw - 4f, top, vw + 2f, lh), value, 15f, Pal.Txt, true, TextAnchor.MiddleLeft);
            Draw.TextFit(new Rect(r.xMax - pw, top, pw, lh), ratio ?? Mathf.RoundToInt(share * 100f) + "%", CSCommonDesign.FontMin, Pal.Sub, false, TextAnchor.MiddleRight);
            var bar = new Rect(r.x, top + lh + 6f, r.width, 6f);
            Draw.Round(bar, Look.Tone1, 3f);
            if (share > 0f) Draw.Round(new Rect(bar.x, bar.y, Mathf.Max(6f, bar.width * share), bar.height), color ?? Pal.Primary, 3f);
        }
    }

    /// <summary>CSCommon.Progress — 막대 8: 목표 선(흰 2px), 넘으면 청록 · 못 미치면 주황(color 를 주면 그 색).</summary>
    public static class Progress
    {
        public static void In(Rect r, float t, float goal = -1f, Color? color = null)
        {
            if (!Draw.Repaint) return;
            t = Mathf.Clamp01(t);
            var bar = new Rect(r.x, Mathf.Round(r.center.y - 4f), r.width, 8f);
            Draw.Round(bar, Look.Tone0, 4f);
            Color c = color ?? (goal < 0f || t >= goal ? Pal.Teal : Pal.Warn);
            if (t > 0f) Draw.Round(new Rect(bar.x, bar.y, Mathf.Max(8f, bar.width * t), bar.height), c, 4f);
            if (goal >= 0f) Draw.Round(new Rect(bar.x + bar.width * Mathf.Clamp01(goal) - 1f, bar.y - 4f, 2f, bar.height + 8f), Pal.Strong, 1f);
        }
    }

    /// <summary>
    /// CSCommon.Spinner — 구글 머티리얼 서클 인디케이터(Google Circular Progress Indicator):
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

    /// <summary>CSCommon.Tooltip — 말풍선: 짙은 바탕 #0B0D11 · 그림자 · 꼬리, 글 12.5 · 줄바꿈 최대 280.</summary>
    public static class Tooltip
    {
        /// <summary>anchor(가리키는 칸) 아래에(화면 밖이면 위로) 말풍선을 그림.</summary>
        public static void Draw(Rect anchor, string text)
        {
            if (!CSCommon.Draw.Repaint || string.IsNullOrEmpty(text)) return;
            const float maxW = 280f, padX = 12f, padY = 8f;
            float tw = Mathf.Min(maxW, CSCommon.Draw.TextWidth(text, 12.5f) + 2f);
            float th = CSCommon.Draw.TextHeight(text, 12.5f, tw);
            float w = tw + padX * 2f, h = th + padY * 2f;
            float x = Mathf.Clamp(anchor.center.x - w * 0.5f, 8f, CSCommon.Draw.Width - w - 8f);
            bool above = anchor.yMax + 10f + h > CSCommon.Draw.Height - 8f;
            float y = above ? anchor.y - 10f - h : anchor.yMax + 10f;
            var r = new Rect(x, y, w, h);
            CSCommon.Draw.Round(new Rect(r.x - 1f, r.y + 3f, r.width + 2f, r.height + 2f), CSCommon.Draw.Alpha(Color.black, 0.35f), CSCommonDesign.RadiusField);
            CSCommon.Draw.Round(r, Look.Ink, CSCommonDesign.RadiusTile);
            float tx = Mathf.Clamp(anchor.center.x, r.x + 12f, r.xMax - 12f);
            // 꼬리: 가리키는 쪽을 향한 세모(1px 줄을 쌓아 그림)
            for (int i = 0; i < 6; i++)
                CSCommon.Draw.Fill(new Rect(tx - (i + 1), above ? r.yMax + 5f - i : r.y - 6f + i, 2f * (i + 1), 1f), Look.Ink);
            CSCommon.Draw.Text(new Rect(r.x + padX, r.y + padY, tw, th), text, 12.5f, Color.white, false, TextAnchor.UpperLeft, true);
        }
    }

    /// <summary>CSCommon.Hint — 안내 한 줄(34 알약): 굵은 머리(도구 이름) + 할 일.</summary>
    public static class Hint
    {
        public static Rect At(float centerX, float y, string text, Color? color = null)
        {
            const float h = 34f;
            string head = null, rest = text ?? "";
            int cut = rest.IndexOf("   ", StringComparison.Ordinal);
            if (cut > 0) { head = rest.Substring(0, cut); rest = rest.Substring(cut + 3); }
            float hw = head != null ? Draw.TextWidth(head, 13f, true) + 10f : 0f;
            float w = Mathf.Min(Draw.Width - 64f, hw + Draw.TextWidth(rest, 13f) + 32f);
            var r = new Rect(Mathf.Round(centerX - w * 0.5f), y, w, h);
            if (!Draw.Repaint) return r;
            Draw.Round(r, Draw.Alpha(Pal.Win, 0.94f), h * 0.5f);
            Draw.RoundOutline(r, Look.PanelLine, h * 0.5f);
            float tx = r.x + 16f;
            if (head != null) { Draw.Text(new Rect(tx, r.y, hw, h), head, 13f, Pal.Strong, true, TextAnchor.MiddleLeft); tx += hw; }
            Draw.TextFit(new Rect(tx, r.y, r.xMax - tx - 16f, h), rest, 13f, color ?? Pal.Light);
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
    /// CSCommon.Toast — 작업 알림 60(GLDS 토스트 D): 상태 원 36 · 제목 14 굵게 + 설명 12 한 줄(문구의 첫 ' — ' 뒤) · 오른쪽 작은 주 단추(되돌리기 등).
    /// 8 아래에서 올라오며 0.26초 페이드인 · 끝나기 0.18초 전부터 페이드아웃(age · life 를 넘기면).
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
            float w = CSCommonDesign.ToastPadL + CSCommonDesign.ToastCircle + CSCommonDesign.ToastGap + tw + CSCommonDesign.ToastPadR;
            if (!string.IsNullOrEmpty(action)) w += CSCommonDesign.ToastGap + Button.Width(actionIcon, action, true);
            return Mathf.Min(w, CSCommonDesign.ToastMaxW);
        }

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
            float show = CSCommonAnim.EaseOut(age / CSCommonDesign.ToastInSeconds);
            float fade = life < float.MaxValue ? Mathf.Min(show, Mathf.Clamp01((life - age) / CSCommonDesign.ToastOutSeconds)) : show;
            if (fade <= 0f) return;
            float opacity = Draw.Opacity;
            var shift = new Vector2(0f, Mathf.Round((1f - show) * CSCommonDesign.ToastRise));
            Draw.Opacity *= fade;
            Draw.Shift(shift);
            Split(text, out string title, out string desc);
            Look.Elevated(r, CSCommonDesign.CardRadius, Look.FieldFocus);
            float bw = string.IsNullOrEmpty(action) ? 0f : Button.Width(actionIcon, action, true);
            if (Draw.Repaint)
            {
                float d = CSCommonDesign.ToastCircle, isz = CSCommonDesign.ToastIcon;
                var c = new Vector2(r.x + CSCommonDesign.ToastPadL + d * 0.5f, r.center.y);
                Color ring = kind == ToastKind.Success ? Draw.Alpha(Pal.Teal, 0.14f) : kind == ToastKind.Error ? Draw.Alpha(Pal.Red, 0.16f) : Look.Tone0;
                Color ink = kind == ToastKind.Success ? Pal.Teal : kind == ToastKind.Error ? Pal.Red : Pal.Txt;
                Draw.Dot(c, d * 0.5f, ring);
                Draw.Icon(kind == ToastKind.Success ? SuccessPath : kind == ToastKind.Error ? ErrorPath : InfoPath, new Rect(c.x - isz * 0.5f, c.y - isz * 0.5f, isz, isz), ink, 24f, 2.4f);
                float tx = r.x + CSCommonDesign.ToastPadL + d + CSCommonDesign.ToastGap;
                float tw = r.xMax - tx - CSCommonDesign.ToastPadR - (bw > 0f ? bw + CSCommonDesign.ToastGap : 0f);
                if (desc == null) Draw.TextFit(new Rect(tx, r.y, tw, r.height), title, 14f, Pal.Strong, true);
                else
                {
                    // 제목 18 + 사이 3 + 설명 16 을 가운데로
                    float top = Mathf.Round(r.center.y - 18.5f);
                    Draw.TextFit(new Rect(tx, top, tw, 18f), title, 14f, Pal.Strong, true);
                    Draw.TextFit(new Rect(tx, top + 21f, tw, 16f), desc, 12f, Pal.Sub);
                }
            }
            if (bw > 0f)
                Button.In(new Rect(r.xMax - CSCommonDesign.ToastPadR - bw, r.center.y - CSCommonDesign.ActionHeightSmall * 0.5f, bw, CSCommonDesign.ActionHeightSmall), actionIcon, action, null, Tone.Primary, null, true, click, false);
            Draw.Shift(-shift);
            Draw.Opacity = opacity;
        }
    }

    /// <summary>
    /// CSCommon.Section — Flatten 섹션(GLDS 2.1): Win 바탕 · 모서리 0 · 테두리 없음, 제목 17 굵게 · 오른쪽 보조 12 Sub.
    /// 섹션 사이 · 두 열 사이는 Band(Pal.Band 띠 BandH 10), 섹션 안 줄 사이는 Hairline(Divider 1).
    /// 면은 뜻이 있을 때만(히어로 fill · 위 색 띠 stripe · 타일 · 차트 · 지도 판 · 창).
    /// </summary>
    public static class Section
    {
        public static void Frame(Rect r, Color? fill = null, Color? stripe = null)
        {
            if (!Draw.Repaint) return;
            Draw.Fill(r, fill ?? Pal.Win);
            if (stripe.HasValue) Draw.Fill(new Rect(r.x, r.y, r.width, 3f), stripe.Value);
        }

        public static void Title(Rect r, string title, string sub = null)
        {
            if (!Draw.Repaint) return;
            float sw = string.IsNullOrEmpty(sub) ? 0f : Mathf.Min(r.width * 0.45f, Draw.TextWidth(sub, CSCommonDesign.FontMin) + 8f);
            Draw.TextFit(new Rect(r.x, r.y, r.width - sw, r.height), title, CSCommonDesign.SectionTitleFont, Pal.Txt, true);
            if (sw > 0f) Draw.TextFit(new Rect(r.xMax - sw, r.y, sw, r.height), sub, CSCommonDesign.FontMin, Pal.Sub, false, TextAnchor.MiddleRight);
        }

        /// <summary>줄 사이 헤어라인(들임 inset — 글 시작선에 맞춤).</summary>
        public static void Hairline(Rect r, float inset = 0f)
        {
            if (Draw.Repaint) Draw.Fill(new Rect(r.x + inset, r.y, r.width - inset, 1f), Look.Divider);
        }
    }

    /// <summary>CSCommon.Card — 선 카드(시안 A): 바탕 #1B1F26 + 1px 선 #3A404B, 그림자 · 제목 앞 막대 없음 · 오른쪽 보조 글. 여백은 CSCommonDesign.Card*.</summary>
    public static class Card
    {
        public static void Frame(Rect r, Color? fill = null)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, fill ?? Pal.Card, CSCommonDesign.CardRadius);
            Draw.RoundOutline(r, Pal.Line2, CSCommonDesign.CardRadius);
        }

        /// <summary>제목 줄(bar 는 예전 호출 모양만 지킴 — 선 카드엔 막대 없음).</summary>
        public static void Title(Rect r, string title, string sub = null, Color? bar = null)
        {
            if (!Draw.Repaint) return;
            float tx = r.x;
            float sw = string.IsNullOrEmpty(sub) ? 0f : Mathf.Min(r.width * 0.45f, Draw.TextWidth(sub, 12f) + 8f);
            Draw.TextFit(new Rect(tx, r.y, r.xMax - tx - sw, r.height), title, 16f, Pal.Txt, true);
            if (sw > 0f) Draw.TextFit(new Rect(r.xMax - sw, r.y, sw, r.height), sub, 12f, Pal.Sub, false, TextAnchor.MiddleRight);
        }
    }

    /// <summary>
    /// CSCommon.Tile — 고를 수 있는 타일(설정 타일 · 대시 타일 · 주말 형식 · 에셋 카드 · 필터 칩): 바탕 Field · 호버 Tone0 · 고르면 떠오른 면(Look.Raised) ·
    /// 꺼짐 Off. 모서리 RadiusField. 안쪽 글 · 아이콘은 부르는 쪽이 그림.
    /// </summary>
    public static class Tile
    {
        /// <summary>바탕만 그림(안에 단추가 있는 타일 — 타일 자체는 누르지 않음).</summary>
        public static void Frame(Rect r, bool on, bool enabled = true, bool hoverable = true)
        {
            int key = Look.Key(r, "tile");
            float hv = Look.Hover(key, r, enabled && hoverable && !on);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 12f);
            if (!Draw.Repaint) return;
            Draw.Round(r, !enabled ? Look.Off : Color.Lerp(Look.Field, Look.Tone0, hv), CSCommonDesign.RadiusField);
            // UX 1.1: 고른 타일 = 주 색 옅은 면 + 주 색 2px 테두리(떠오른 면은 라이트에서 거의 안 보였음)
            if (onT > 0.01f)
            {
                Draw.Round(r, Draw.Alpha(Pal.Primary, 0.12f * onT), CSCommonDesign.RadiusField);
                Draw.RoundOutline(r, Draw.Alpha(Pal.Primary, onT), CSCommonDesign.RadiusField, 2f);
            }
        }

        /// <summary>바탕 + 누르기. 누르면 true(꺼짐이면 늘 false).</summary>
        public static bool In(Rect r, bool on, bool enabled = true, string tip = null)
        {
            Frame(r, on, enabled);
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }
    }

    /// <summary>CSCommon.ListRow — 목록 한 줄 바탕: 평소 투명 · 호버 Tone0 · 고르면 떠오른 면. 모서리 RadiusTool. 누르면 true.</summary>
    public static class ListRow
    {
        public static bool In(Rect r, bool on, bool enabled = true, string tip = null)
        {
            int key = Look.Key(r, "listrow");
            float hv = Look.Hover(key, r, enabled && !on);
            float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 12f);
            if (Draw.Repaint)
            {
                if (hv > 0.01f) Draw.Round(r, Draw.Alpha(Look.Tone0, hv), CSCommonDesign.RadiusTool);
                Look.Raised(r, CSCommonDesign.RadiusTool, onT);
            }
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }
    }

    /// <summary>CSCommon.Well — 안쪽 우물판(지도 · 미니맵 · 그림 바탕): 판 바탕 #14171C + 1px 선 #2A2F38, 모서리 RadiusTile.</summary>
    public static class Well
    {
        public static void Frame(Rect r, Color? line = null)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, Pal.Win, CSCommonDesign.RadiusTile);
            Draw.RoundOutline(r, line ?? Look.PanelLine, CSCommonDesign.RadiusTile);
        }
    }

    /// <summary>CSCommon.Panel — 위 판 · 아래 독 · 펼침 메뉴 바탕: #14171C · 그림자 두 겹 · 1px 선 · 모서리 14(메뉴 12).</summary>
    public static class Panel
    {
        public static void Frame(Rect r, float radius = 14f) => Look.Elevated(r, radius, Pal.Win);
    }

    /// <summary>CSCommon.Popover — 떠 있는 카드: 판 바탕 · 그림자 · 가리키는 꼬리(tailX, 아래쪽).</summary>
    public static class Popover
    {
        public static void Frame(Rect r, float tailX, bool tailBelow = true)
        {
            Look.Elevated(r, 12f, Look.FieldFocus);
            if (!Draw.Repaint) return;
            float tx = Mathf.Clamp(tailX, r.x + 18f, r.xMax - 18f);
            var tail = tailBelow ? new Rect(tx - 6f, r.yMax - 6f, 12f, 12f) : new Rect(tx - 6f, r.y - 6f, 12f, 12f);
            Draw.Round(tail, Look.FieldFocus, 2f);
            Draw.Fill(new Rect(tx - 7f, tailBelow ? r.yMax - 7f : r.y + 1f, 14f, 6f), Look.FieldFocus);
        }

        /// <summary>머리 줄: 이름 굵게 · 옆 작은 글 · 아래 구분선.</summary>
        public static void Head(Rect r, string title, string sub = null)
        {
            if (!Draw.Repaint) return;
            float tw = Draw.TextWidth(title, 14f, true);
            Draw.Text(new Rect(r.x, r.y, tw + 4f, r.height), title, 14f, Pal.Txt, true, TextAnchor.MiddleLeft);
            if (!string.IsNullOrEmpty(sub)) Draw.Text(new Rect(r.x + tw + 10f, r.y, r.width, r.height), sub, CSCommonDesign.FontMin, Pal.Sub, false, TextAnchor.MiddleLeft);
            Draw.Fill(new Rect(r.x - 2f, r.yMax + 3f, r.width + 4f, 1f), Look.Divider);
        }
    }

    /// <summary>CSCommon.Dialog — 확인 창 420: 뜻 아이콘 칸 · 제목 · 설명, 아래 오른쪽 [취소(보통) · 실행(주 / 지움)].</summary>
    public static class Dialog
    {
        public const float Width = 420f, Pad = 20f;

        public static float Height(string text, float width = Width)
        {
            float th = string.IsNullOrEmpty(text) ? 0f : Draw.TextHeight(text, 13.5f, width - Pad * 2f);
            return Pad + 40f + 12f + 26f + (th > 0f ? 8f + th : 0f) + 18f + CSCommonDesign.ActionHeight + Pad;
        }

        /// <summary>판을 그리고 누른 단추를 돌려줌: 1 = 실행 · 0 = 취소 · -1 = 없음.</summary>
        public static int In(Rect box, string title, string text, string ok, string cancel, bool danger, float appear = 1f)
        {
            Look.Elevated(box, 14f, Look.FieldFocus, appear);
            float x = box.x + Pad, y = box.y + Pad, inner = box.width - Pad * 2f;
            if (Draw.Repaint)
            {
                var ic = new Rect(x, y, 40f, 40f);
                Color c = danger ? Pal.Red : Pal.Primary;
                Draw.Round(ic, Draw.Alpha(c, 0.14f), CSCommonDesign.IconBtnRadius);
                Draw.Icon(danger ? Icons.Warn : Icons.Info, new Rect(ic.x + 10f, ic.y + 10f, 20f, 20f), c, 24f, 2f);
                Draw.TextFit(new Rect(x, y + 52f, inner, 26f), title, 17f, Pal.Txt, true);
                if (!string.IsNullOrEmpty(text))
                    Draw.Text(new Rect(x, y + 52f + 34f, inner, Draw.TextHeight(text, 13.5f, inner)), text, 13.5f, Pal.Sub, false, TextAnchor.UpperLeft, true);
            }
            float by = box.yMax - Pad - CSCommonDesign.ActionHeight;
            float okW = Button.Width(danger ? Icons.Trash : Icons.Check, ok), cancelW = Button.Width(null, cancel);
            var okR = new Rect(box.xMax - Pad - okW, by, okW, CSCommonDesign.ActionHeight);
            var cancelR = new Rect(okR.x - 8f - cancelW, by, cancelW, CSCommonDesign.ActionHeight);
            int hit = -1;
            // 두 단추를 늘 불러 컨트롤 순서를 지킴
            Button.In(cancelR, null, cancel, null, Tone.Neutral, null, true, () => hit = 0, false);
            Button.In(okR, danger ? Icons.Trash : Icons.Check, ok, null, danger ? Tone.Danger : Tone.Primary, null, true, () => hit = 1, false);
            return hit;
        }
    }

    /// <summary>
    /// CSCommon.Modal — 큰 창: 화면을 어둡게 덮고(scrim) 가운데 판, 머리(제목 · 설명 · 닫기) · 몸 · 발(단추 줄, #171A20 · 위 구분선).
    /// Frame 이 몸 칸을 돌려주고, 발이 있으면 Footer 칸에 단추를 그림. 열림 · 닫힘 움직임은 CSCommonAnim.Modal.
    /// </summary>
    public static class Modal
    {
        public const float HeadH = 56f, FootH = 64f;

        /// <summary>판 · 머리를 그리고 몸 칸을 돌려줌. closed = 닫기를 누름.</summary>
        public static Rect Frame(Rect r, string title, string sub, out bool closed, bool footer = false, bool scrim = true)
        {
            closed = false;
            if (scrim && Draw.Repaint) Draw.Fill(new Rect(-Draw.Width, -Draw.Height, Draw.Width * 3f, Draw.Height * 3f), new Color(5f / 255f, 6f / 255f, 8f / 255f, 0.55f));
            Look.Elevated(r, 14f, Look.FieldFocus);
            if (Draw.Repaint)
            {
                float tw = Draw.TextWidth(title, 17f, true);
                Draw.Text(new Rect(r.x + 20f, r.y, tw + 4f, HeadH), title, 17f, Pal.Txt, true, TextAnchor.MiddleLeft);
                if (!string.IsNullOrEmpty(sub)) Draw.TextFit(new Rect(r.x + 20f + tw + 12f, r.y, r.width - tw - 100f, HeadH), sub, 12.5f, Pal.Sub);
            }
            if (Button.Square(new Rect(r.xMax - 16f - 32f, r.y + (HeadH - 32f) * 0.5f, 32f, 32f), Icons.Close, "닫기(Esc)")) closed = true;
            if (footer && Draw.Repaint)
            {
                var f = new Rect(r.x, r.yMax - FootH, r.width, FootH);
                Draw.Round(f, Draw.Hex("#171A20"), CSCommonDesign.RadiusWindow);
                Draw.Fill(new Rect(f.x, f.y, f.width, 14f), Draw.Hex("#171A20"));
                Draw.Fill(new Rect(f.x, f.y, f.width, 1f), Look.Divider);
            }
            return new Rect(r.x + 20f, r.y + HeadH, r.width - 40f, r.height - HeadH - (footer ? FootH : 0f) - 12f);
        }

        /// <summary>발 칸(단추 줄 가운데 y) — 오른쪽부터 단추를 놓을 때 씀.</summary>
        public static float FooterCy(Rect r) => r.yMax - FootH * 0.5f;
    }

    /// <summary>
    /// CSCommon.Phone — 휴대폰(아이폰 프로 맥스 440×956 · 다이나믹 아일랜드 · 리퀴드 글라스 면). 크기는 고정(CSCommonDesign.PhoneW · PhoneH).
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
            CSCommon.UI.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>하단 툴바(탭 바): 홈 막대 위에 뜬 유리 알약. 칸마다 아이콘 + 이름, 고른 칸은 강조색으로 물든 알약. content = Frame 이 돌려준 쓸 자리.</summary>
        public static void TabBar(Rect content, string[] icons, string[] labels, int cur, Action<int> pick)
        {
            // .GlassToolBar(iOS 27 리퀴드 글래스) — 모드 창 하단 툴바와 같은 부품
            int hit = GlassToolBar.Bar(content, labels.Length, cur, (i, cell, sel, hover) =>
                GlassToolBar.Cell(cell, icons != null && i < icons.Length ? icons[i] : null, labels[i], sel, hover));
            if (hit >= 0) pick(hit);
        }
    }
}
