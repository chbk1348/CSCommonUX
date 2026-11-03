using System;
using ColossalFramework.UI;
using UnityEngine;

using CSCommon;

namespace CSCommon.UI
{
    /// <summary>버튼 톤(CSCommon.Button 과 같음): 보통(회색 톤) · 주(청록 톤) · 지움(회색 톤 + 빨강 글).</summary>
    public enum CSCommonButtonKind
    {
        Outline,
        Primary,
        Danger,
    }

    /// <summary>
    /// 게임 UI(ColossalFramework) 부품을 CSCommon 컴포넌트(디자인 아티팩트 'CSCommon 컴포넌트')와 같은 모양으로 만드는 도우미 —
    /// 톤 바탕 · 테두리 없음 · 켜짐 청록.
    /// 크기 · 색은 F1 창(Draw · Pal)과 같음: 카드 #1B1F26 · 선 #2A2F38 · 강조 #E10600 · 글 #ECEEF2 / #9AA3AF / #7F8894.
    /// 글 크기는 화소(기본 글꼴 16px = 배율 1).
    /// </summary>
    internal static class CSCommonKit
    {
        public static Color32 C(Color c) => c;

        public static float Scale(float px) => px * Draw.TextBoost / 16f;

        /// <summary>게임 UI 는 색 알파를 버리고 불투명으로 그림(UIComponent.ApplyOpacity) → 반투명 색은 바탕(기본 카드)에 미리 섞어 씀.</summary>
        public static Color Over(Color c, Color? under = null)
        {
            var o = Color.Lerp(under ?? Pal.Card, c, c.a);
            o.a = 1f;
            return o;
        }

        static UIFont boldFont;

        /// <summary>게임 굵은 UI 글꼴(OpenSans-Bold, 없으면 Semibold · 그마저도 없으면 기본 글꼴) — 글꼴 모드가 바꾼 글꼴도 따라감.</summary>
        public static UIFont BoldFont => boldFont != null ? boldFont : boldFont = FindGameBoldFont();

        static UIFont FindGameBoldFont()
        {
            UIFont semibold = null;
            foreach (var f in Resources.FindObjectsOfTypeAll<UIFont>())
            {
                if (f.name == "OpenSans-Bold") return f;
                if (f.name == "OpenSans-Semibold") semibold = f;
            }
            return semibold;
        }

        public static UIPanel Panel(UIComponent parent, string sprite, Color color, float x, float y, float w, float h)
        {
            var p = parent.AddUIComponent<UIPanel>();
            p.atlas = CSCommonAtlas.Atlas;
            p.backgroundSprite = sprite;
            p.color = color;
            p.relativePosition = new Vector3(x, y);
            p.size = new Vector2(w, h);
            return p;
        }

        public static UIPanel Line(UIComponent parent, float x, float y, float w, float h, Color color)
        {
            var l = Panel(parent, CSCommonAtlas.White, color, x, y, w, h);
            l.isInteractive = false;
            return l;
        }

        public static UILabel Label(UIComponent parent, string text, float px, Color color, float x, float y, float width = -1f, bool bold = false)
        {
            var l = parent.AddUIComponent<UILabel>();
            l.textScale = Scale(px);
            l.textColor = color;
            l.disabledTextColor = Pal.Dim;
            if (bold && BoldFont != null) l.font = BoldFont;
            if (width > 0f)
            {
                l.autoSize = false;
                l.autoHeight = true;
                l.wordWrap = true;
                l.width = width;
            }
            else l.autoSize = true;
            l.relativePosition = new Vector3(x, y);
            l.text = text ?? "";
            l.PerformLayout();
            return l;
        }

        /// <summary>둥근 배지(판 · 상태): 색 바탕 · 흰 굵은 글.</summary>
        public static UILabel Badge(UIComponent parent, string text, Color bg)
        {
            // CSCommon.Badge: 그 색 16% 바탕 · 그 색 굵은 글
            var l = parent.AddUIComponent<UILabel>();
            l.atlas = CSCommonAtlas.Atlas;
            l.backgroundSprite = CSCommonAtlas.BadgePill;
            l.color = Over(Draw.Alpha(bg, 0.16f));
            l.textScale = Scale(12f);
            l.textColor = bg == Pal.Line2 ? Pal.Sub : bg;
            if (BoldFont != null) l.font = BoldFont;
            l.verticalAlignment = UIVerticalAlignment.Middle;
            l.text = text ?? "";
            // 높이 24 고정(알약 그림과 같은 높이) · 폭은 실제 글자 잉크 + 양옆 10, 글은 정가운데(단추와 같은 CenterText 규칙)
            l.autoSize = false;
            l.height = 24f;
            if (Ink(l.font, l.textScale, l.text, out float inkL, out float ink))
            {
                l.width = Mathf.Max(Mathf.Ceil(ink + 20f), 24f);
                l.textAlignment = UIHorizontalAlignment.Left;
                l.padding = new RectOffset(Mathf.RoundToInt((l.width - ink) * 0.5f - inkL), 0, 2, 0);
            }
            else
            {
                l.textAlignment = UIHorizontalAlignment.Center;
                l.padding = new RectOffset(10, 10, 2, 0);
                l.autoSize = true;
                l.PerformLayout();
                float w = l.width;
                l.autoSize = false;
                l.size = new Vector2(Mathf.Max(w, 24f), 24f);
            }
            return l;
        }

        /// <summary>F1 창 버튼: 테두리(기본) · 빨강 채움(Primary) · 빨강 테두리(Danger). 꺼지면 흐림.</summary>
        public static UIButton Button(UIComponent parent, string text, float x, float y, float w, float h, CSCommonButtonKind kind, Action click)
        {
            // CSCommon.Button: 톤 바탕 · 테두리 없음 · 글 13.5(보통 굵기), 주 = 청록 톤 · 지움 = 빨강 글
            var b = parent.AddUIComponent<UIButton>();
            b.atlas = CSCommonAtlas.Atlas;
            b.normalBgSprite = b.hoveredBgSprite = b.pressedBgSprite = b.focusedBgSprite = b.disabledBgSprite = CSCommonAtlas.Round6;
            bool primary = kind == CSCommonButtonKind.Primary;
            // UX 1.1: 주 단추는 주 색으로 꽉 채움 + OnPrimary 글
            b.color = primary ? Pal.Primary : Look.Tone0;
            b.hoveredColor = primary ? Color.Lerp(Pal.Primary, Pal.Haze, 0.10f) : Look.Tone1;
            b.pressedColor = primary ? Color.Lerp(Pal.Primary, Pal.Haze, 0.18f) : Look.Tone2;
            b.focusedColor = b.color;
            b.disabledColor = Over(Look.Off);
            Color ink = kind == CSCommonButtonKind.Danger ? Pal.Red : primary ? Pal.OnPrimary : Pal.Txt;
            b.textColor = ink;
            b.hoveredTextColor = kind == CSCommonButtonKind.Danger ? Pal.Red : primary ? Pal.OnPrimary : Pal.Strong;
            b.pressedTextColor = ink;
            b.focusedTextColor = ink;
            b.disabledTextColor = Look.OffText;
            b.textScale = Scale(13.5f);
            // 게임 기본 글꼴은 한글 폭을 잘못 재 가운데가 왼쪽으로 쏠림 → 라벨과 같은 글꼴
            if (BoldFont != null) b.font = BoldFont;
            b.textVerticalAlignment = UIVerticalAlignment.Middle;
            b.text = text ?? "";
            b.relativePosition = new Vector3(x, y);
            b.size = new Vector2(w, h);
            CenterText(b, 0f, false);
            b.canFocus = false;
            if (click != null) b.eventClick += (c, e) => click();
            return b;
        }

        /// <summary>
        /// 단추 글을 칸 정가운데에(UX 1.1). 게임 글꼴(OpenSans + 한글 대체 글꼴)은 잰 폭과 그린 글자 자리가 달라, 엔진 가운데 정렬로는
        /// 긴 한글일수록 왼쪽으로 쏠렸음('이전 내역 더 보기' 약 14px). 그래서 렌더러(DynamicFontRenderer)와 같은 규칙 — 글자마다 Ceil(maxX) 씩,
        /// 빈칸 묶음은 advance 합의 올림 — 으로 실제 잉크 왼쪽 · 오른쪽 끝을 재고, 왼쪽 정렬 + 왼쪽 여백으로 그 잉크를 가운데에 둠.
        /// 세로는 위 2px 여백(예전 아래 2px 은 글이 2 ~ 3px 위로 떠 보였음). fit = 칩처럼 폭을 글에 맞춤(양옆 padX).
        /// </summary>
        static void CenterText(UIButton b, float padX, bool fit)
        {
            b.autoSize = false;
            b.textPadding = new RectOffset(0, 0, 2, 0);
            b.textHorizontalAlignment = UIHorizontalAlignment.Center;
            if (!Ink(b.font, b.textScale, b.text, out float inkL, out float ink))
            {
                if (fit) b.width = 48f;
                return;
            }
            if (fit) b.width = Mathf.Ceil(ink + padX * 2f);
            b.textHorizontalAlignment = UIHorizontalAlignment.Left;
            b.textPadding = new RectOffset(Mathf.RoundToInt((b.width - ink) * 0.5f - inkL), 0, 2, 0);
        }

        /// <summary>렌더러 규칙으로 잰 글자 잉크의 왼쪽 끝(inkL) · 폭(ink). 동적 글꼴이 아니거나 글자 정보가 없으면 false.</summary>
        static bool Ink(UIFont font, float textScale, string s, out float inkL, out float ink)
        {
            inkL = ink = 0f;
            if (!(font is UIDynamicFont df) || df.baseFont == null || string.IsNullOrEmpty(s)) return false;
            int size = Mathf.CeilToInt(df.size * textScale);
            var f = df.baseFont;
            f.RequestCharactersInTexture(s, size, FontStyle.Normal);
            float pen = 0f, left = float.MaxValue, inkR = float.MinValue;
            int i = 0;
            while (i < s.Length)
            {
                if (s[i] == ' ')
                {
                    float adv = 0f;
                    for (; i < s.Length && s[i] == ' '; i++)
                        if (f.GetCharacterInfo(' ', out var sp, size, FontStyle.Normal)) adv += sp.advance;
                    pen += Mathf.Ceil(adv);
                    continue;
                }
                float start = pen, p = pen, sum = 0f;
                for (; i < s.Length && s[i] != ' '; i++)
                {
                    if (!f.GetCharacterInfo(s[i], out var info, size, FontStyle.Normal)) continue;
                    left = Mathf.Min(left, p + info.minX);
                    inkR = Mathf.Max(inkR, p + info.minX + info.glyphWidth);
                    p += Mathf.Ceil(info.maxX);
                    sum += info.maxX;
                }
                pen = start + Mathf.Ceil(sum);
            }
            if (inkR < left) return false; // 글자 정보가 아직 없음 — 엔진 가운데 정렬 그대로
            inkL = left;
            ink = inkR - left;
            return true;
        }

        /// <summary>
        /// 글 속 목록 한 줄(GLDS): 앞 공백 두 칸마다 한 단 들임 · 점은 글자 • 대신 도형(지름 = 글 크기 × 0.38, 깊이 0 ● → 1 ○ → 2 ■ → 다시 ●, 글과 같은 색).
        /// 「1. 」로 시작하면 번호를 적은 그대로 두고 점 없음. 쓴 높이를 돌려줌.
        /// </summary>
        public static float ListItem(UIComponent parent, string line, float px, Color color, float x, float y, float width)
        {
            line ??= "";
            int depth = 0;
            while (line.StartsWith("  ")) { line = line.Substring(2); depth++; }
            float indent = x + depth * 18f;
            bool numbered = System.Text.RegularExpressions.Regex.IsMatch(line, @"^\d+\.\s");
            float textX = indent + 15f;
            if (!numbered)
            {
                float d = px * 0.38f, cy = y + px * 0.72f;
                if (depth % 3 == 2) d *= 0.86f;
                string sprite = depth % 3 == 1 ? CSCommonAtlas.Ring : depth % 3 == 2 ? CSCommonAtlas.White : CSCommonAtlas.Disc;
                Panel(parent, sprite, color, indent + 4f - d * 0.5f, cy - d * 0.5f, d, d).isInteractive = false;
            }
            var l = Label(parent, line, px, color, numbered ? indent : textX, y, width - (numbered ? indent : textX) + x);
            l.PerformLayout();
            return Mathf.Max(l.height, px * 1.4f);
        }

        /// <summary>세로 스크롤 막대(가는 회색 손잡이, F1 창과 같음).</summary>
        public static UIScrollbar Scrollbar(UIComponent parent, UIScrollablePanel scroll)
        {
            var bar = parent.AddUIComponent<UIScrollbar>();
            bar.orientation = UIOrientation.Vertical;
            bar.size = new Vector2(10f, scroll.height);
            bar.relativePosition = new Vector3(scroll.relativePosition.x + scroll.width + 2f, scroll.relativePosition.y);
            bar.minValue = 0f;
            bar.value = 0f;
            bar.incrementAmount = 40f;
            bar.autoHide = true;
            var track = bar.AddUIComponent<UISlicedSprite>();
            track.atlas = CSCommonAtlas.Atlas;
            track.spriteName = CSCommonAtlas.White;
            track.color = Pal.Win;  // 알파를 버리므로 투명 대신 창 바탕색
            track.size = bar.size;
            track.relativePosition = Vector3.zero;
            bar.trackObject = track;
            var thumb = track.AddUIComponent<UISlicedSprite>();
            thumb.atlas = CSCommonAtlas.Atlas;
            thumb.spriteName = CSCommonAtlas.Thin;
            thumb.color = Pal.Line2;
            thumb.width = 4f;
            thumb.relativePosition = new Vector3(3f, 0f);
            bar.thumbObject = thumb;
            scroll.verticalScrollbar = bar;
            return bar;
        }
    }
}
