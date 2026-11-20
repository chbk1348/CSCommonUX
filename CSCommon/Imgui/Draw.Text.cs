using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 글

        // 글꼴: 게임 UI 글꼴 그대로(보통 = 기본 UI 글꼴 · 굵게 = OpenSans-Bold) — Font Changer 같은 글꼴 모드가 바꾼 글꼴도 따라감.
        // 게임 UI 가 아직 없으면(로딩 중) 기본 IMGUI 글꼴로 그리고 다음 글에서 다시 찾음.
        static Font faceRegular, faceBold;
        static bool facesFound;

        /// <summary>
        /// 브랜드 글꼴(이 PC 에 설치된 글꼴 이름 머리, 예 F1 = "Formula1 Display") — 모드가 시작할 때 한 번. 글꼴 파일은 git 에 올리지 않으므로
        /// 설치된 글꼴을 불러 쓰고(굵게 · 보통 · 넓게 = 이름 + " Bold" · " Regular" · " Wide"), 없으면 게임 글꼴. 한글은 맑은 고딕으로 대신 그림.
        /// </summary>
        #pragma warning disable 0649 // 모드가 값을 넣는 확장점이라 정본 안에서는 대입이 없음
        public static string BrandFont;
        #pragma warning restore 0649
        static Font brandBold, brandRegular, brandWide;
        static bool brandTried;

        static void EnsureBrand()
        {
            if (brandTried || string.IsNullOrEmpty(BrandFont)) return;
            brandTried = true;
            try
            {
                var installed = new HashSet<string>(Font.GetOSInstalledFontNames());
                Font Load(string suffix)
                {
                    string name = BrandFont + suffix;
                    return installed.Contains(name) ? Font.CreateDynamicFontFromOSFont(new[] { name, "Malgun Gothic" }, 16) : null;
                }
                brandBold = Load(" Bold");
                brandRegular = Load(" Regular");
                brandWide = Load(" Wide");
                CSCommonLog.Info(brandBold != null ? $"글꼴: 브랜드 글꼴 {BrandFont} 씀(굵게 · 보통{(brandWide != null ? " · 넓게" : "")})" : $"글꼴: 브랜드 글꼴 {BrandFont} 가 이 PC 에 없어 게임 글꼴로");
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "브랜드 글꼴 찾기");
            }
        }

        static void EnsureFaces()
        {
            if (facesFound) return;
            try
            {
                var ui = UIView.GetAView()?.defaultFont as UIDynamicFont;
                if (ui == null || ui.baseFont == null) return;
                faceRegular = ui.baseFont;
                foreach (var f in Resources.FindObjectsOfTypeAll<UIDynamicFont>())
                    if (f != null && f.baseFont != null && f.name == "OpenSans-Bold") { faceBold = f.baseFont; break; }
                facesFound = true;
                // 찾기 전에 만든 글 모양(기본 글꼴)은 버림
                textStyles.Clear();
                widthCache.Clear();
                heightCache.Clear();
                CSCommonLog.Info($"글꼴: 게임 UI 글꼴 씀(보통 {faceRegular.name} · 굵게 {(faceBold != null ? faceBold.name : "엔진 굵게")})");
            }
            catch (Exception e)
            {
                facesFound = true;
                CSCommonLog.Exception(e, "게임 글꼴 찾기");
            }
        }

        /// <summary>얼굴: 0 보통 · 1 굵게 · 2 넓게(방송 제목 — 브랜드 넓게, 없으면 굵게) · 3 브랜드 굵게 · 4 브랜드 보통(없으면 게임 굵게 · 보통). 게임 굵은 글꼴이 없으면 엔진 굵게.</summary>
        static GUIStyle TextStyle(int px, bool bold, TextAnchor anchor, bool wrap) => TextStyle(px, bold ? 1 : 0, anchor, wrap);

        static GUIStyle TextStyle(int px, int face, TextAnchor anchor, bool wrap)
        {
            int key = px * 256 + face * 32 + (wrap ? 16 : 0) + (int)anchor;
            if (!textStyles.TryGetValue(key, out var style))
            {
                EnsureFaces();
                EnsureBrand();
                Font brand = face switch { 2 => brandWide ?? brandBold, 3 => brandBold, 4 => brandRegular, _ => null };
                bool regular = face == 0 || face == 4 && brand == null;
                Font font = brand ?? (regular ? faceRegular : faceBold ?? faceRegular);
                style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = px,
                    // 게임 굵은 글꼴 · 브랜드 글꼴은 이미 제 굵기라 엔진 굵게(덧칠)를 또 걸지 않음
                    fontStyle = brand == null && !regular && faceBold == null ? FontStyle.Bold : FontStyle.Normal,
                    alignment = anchor,
                    wordWrap = wrap,
                    // 최소 글자 크기를 쓰는 모드(기상청)는 낮은 칸에서 글자가 위아래로 잘리지 않게 넘쳐 그림
                    clipping = MinText > 0f ? TextClipping.Overflow : TextClipping.Clip,
                    richText = false,
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                };
                if (font != null) style.font = font;
                textStyles[key] = style;
            }
            return style;
        }

        /// <summary>최소 글자 크기(논리 px, 0 = 없음). 모드가 시작할 때 한 번 — 기상청 13(시인성), F1 0. 글 폭 재기도 같은 크기를 씀.</summary>
        #pragma warning disable 0649 // 모드가 값을 넣는 확장점이라 정본 안에서는 대입이 없음
        public static float MinText;
        #pragma warning restore 0649

        /// <summary>글자 크기 배율(1 = 그대로). 모드가 시작할 때 한 번 — 기상청 1.08(시인성), F1 1. 게임 UI 글(CSCommonKit.Scale)도 같이 씀.</summary>
        public static float TextBoost = 1f;

        static int Px(float size) => Mathf.Max(6, Mathf.RoundToInt(Mathf.Max(size * TextBoost, MinText) * S));

        /// <summary>칸 폭에 맞춘 글자 크기(화면 px): 한 줄 글이 칸보다 길면 칸 밖으로 넘치거나 잘리지 않게 줄임(SqueezePx).</summary>
        static int FitPx(string text, float size, int face, float width)
        {
            int px = Px(size);
            return width <= 1f ? px : SqueezePx(text, px, face, width);
        }

        /// <summary>글이 칸 폭(논리)에 들어가는 화면 px — 넘칠 때만 줄이고, CSCommonDesign.FontSqueezeMin(글자 배율 · 최소 크기와 상관없이) 밑으로는 안 줄임.</summary>
        static int SqueezePx(string text, int px, int face, float width)
        {
            float limit = (width + FitSlack) * S, w = FaceWidthPx(text, px, face);
            if (w <= limit) return px;
            int min = Mathf.Min(px, Mathf.Max(6, Mathf.RoundToInt(CSCommonDesign.FontSqueezeMin * S)));
            int fit = Mathf.Clamp(Mathf.FloorToInt(px * limit / w), min, px);
            // 글 폭은 크기에 딱 비례하지 않음(화소 반올림) — 한 단계씩 더 줄여 확인
            while (fit > min && FaceWidthPx(text, fit, face) > limit) fit--;
            return fit;
        }

        /// <summary>글 한 줄 높이(논리 px): 글자 배율 · 최소 크기를 넣은 실제 크기 기준 — 글이 커지면 줄 칸도 같이 커짐.</summary>
        public static float LineHeight(float size) => Mathf.Ceil(Px(size) / S * 1.3f);

        /// <summary>방송 제목 글(ROUND · 큰 제목) — 게임 굵은 글꼴.</summary>
        public static void TextWide(Rect r, string text, float size, Color c, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f || TextOff(r, size, MinText > 0f)) return;
            if (HasClockColon(text)) { ClockText(r, text, size, c, 2, anchor); return; } // 큰 시계(홈 '21:41')도 콜론을 점 둘로
            var style = TextStyle(FitPx(text, size, 2, r.width), 2, anchor, false);
            style.normal.textColor = Op(c);
            content.text = text;
            style.Draw(ToScreen(r), content, false, false, false, false);
        }

        /// <summary>글꼴(0 보통 · 1 굵게 · 2 넓게) 글 폭(논리 단위).</summary>
        static float FaceWidth(string text, float size, int face)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            return FaceWidthPx(text, Px(size), face) / S;
        }

        /// <summary>글꼴 face 글 폭(화면 px), widthCache 에 둠 — 열쇠 C = 1 로 TextWidth(굵게 · 보통 중 넓은 쪽, C = 0)와 나눔.</summary>
        static float FaceWidthPx(string text, int px, int face)
        {
            var key = new TextKey(text, px, face, 1);
            if (!widthCache.TryGetValue(key, out float w))
            {
                var style = TextStyle(px, face, TextAnchor.MiddleLeft, false);
                content.text = text;
                w = style.CalcSize(content).x;
                if (widthCache.Count > 4000) widthCache.Clear();
                widthCache[key] = w;
            }
            return w;
        }

        /// <summary>브랜드 글꼴 글(방송 타워 · 숫자 · 약어) — bold = 굵게 · 아니면 보통. 브랜드 글꼴이 없으면 게임 글꼴.</summary>
        public static void TextBrand(Rect r, string text, float size, Color c, bool bold = true, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f || TextOff(r, size, MinText > 0f)) return;
            int face = bold ? 3 : 4;
            var style = TextStyle(FitPx(text, size, face, r.width), face, anchor, false);
            style.normal.textColor = Op(c);
            content.text = text;
            style.Draw(ToScreen(r), content, false, false, false, false);
        }

        public static float TextBrandWidth(string text, float size, bool bold = true) => FaceWidth(text, size, bold ? 3 : 4);

        public static float TextWideWidth(string text, float size)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            var style = TextStyle(Px(size), 2, TextAnchor.MiddleLeft, false);
            content.text = text;
            return style.CalcSize(content).x / S;
        }

        public static void Text(Rect r, string text, float size, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleLeft, bool wrap = false)
        {
            // 넘쳐 그리는 줄바꿈 글(MinText 모드)은 아래로 몇 줄이든 번질 수 있어 거르지 않음
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f || (wrap ? MinText <= 0f && TextOff(r, size, false) : TextOff(r, size, MinText > 0f))) return;
            if (!wrap && HasClockColon(text)) { ClockText(r, text, size, c, bold ? 1 : 0, anchor); return; }
            var style = TextStyle(wrap ? Px(size) : FitPx(text, size, bold ? 1 : 0, r.width), bold, anchor, wrap);
            style.normal.textColor = Op(c);
            content.text = text;
            style.Draw(ToScreen(r), content, false, false, false, false);
        }

        /// <summary>숫자 사이 콜론(시계 '02:24' · '1:31.902')이 있나.</summary>
        static bool HasClockColon(string text)
        {
            for (int i = 1; i + 1 < text.Length; i++)
                if (text[i] == ':' && char.IsDigit(text[i - 1]) && char.IsDigit(text[i + 1])) return true;
            return false;
        }

        /// <summary>
        /// 시계 글: 게임 글꼴에서 콜론 위 점이 빠져 '02.24'처럼 보여(퀄리파잉 시계 · 랩 시계) 숫자 사이 콜론만 점 둘로 직접 그림.
        /// 콜론 자리 폭은 글꼴의 ':' 폭 그대로, 정렬은 원래 글 전체 폭 기준.
        /// </summary>
        static void ClockText(Rect r, string text, float size, Color c, int face, TextAnchor anchor)
        {
            var style = TextStyle(Px(size), face, anchor, false);
            style.normal.textColor = Op(c);
            float colonW = FaceWidth(":", size, face);
            float total = FaceWidth(text, size, face);
            float x = anchor is TextAnchor.UpperCenter or TextAnchor.MiddleCenter or TextAnchor.LowerCenter ? r.center.x - total * 0.5f
                : anchor is TextAnchor.UpperRight or TextAnchor.MiddleRight or TextAnchor.LowerRight ? r.xMax - total : r.x;
            var left = anchor is TextAnchor.UpperLeft or TextAnchor.UpperCenter or TextAnchor.UpperRight ? TextAnchor.UpperLeft
                : anchor is TextAnchor.LowerLeft or TextAnchor.LowerCenter or TextAnchor.LowerRight ? TextAnchor.LowerLeft : TextAnchor.MiddleLeft;
            var seg = TextStyle(Px(size), face, left, false);
            seg.normal.textColor = Op(c);
            float cy = left == TextAnchor.UpperLeft ? r.y + size * 0.62f : left == TextAnchor.LowerLeft ? r.yMax - size * 0.5f : r.center.y + size * 0.04f;
            float dot = Mathf.Max(0.9f, size * 0.085f);
            int start = 0;
            for (int i = 0; i <= text.Length; i++)
            {
                bool colon = i < text.Length && text[i] == ':' && i > 0 && i + 1 < text.Length && char.IsDigit(text[i - 1]) && char.IsDigit(text[i + 1]);
                if (!colon && i < text.Length) continue;
                if (i > start)
                {
                    string part = text.Substring(start, i - start);
                    float w = FaceWidth(part, size, face);
                    content.text = part;
                    seg.Draw(ToScreen(new Rect(x, r.y, w + 2f, r.height)), content, false, false, false, false);
                    x += w;
                }
                if (colon)
                {
                    float mx = x + colonW * 0.5f;
                    Dot(new Vector2(mx, cy - size * 0.2f), dot, c);
                    Dot(new Vector2(mx, cy + size * 0.2f), dot, c);
                    x += colonW;
                }
                start = i + 1;
            }
        }

        /// <summary>
        /// 크기가 움직이는 글: 글꼴 크기는 size 그대로 두고 그림만 scale배(r 가운데 기준).
        /// 글꼴 크기를 매 프레임 바꾸면 크기마다 글자를 새로 요청해 글꼴 텍스처가 매번 다시 지어짐(게임 UI 전체 다시 그리기 · 스택 넘침).
        /// </summary>
        public static void TextScaled(Rect r, string text, float size, float scale, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f || scale <= 0f) return;
            if (Mathf.Abs(scale - 1f) < 0.001f) { Text(r, text, size, c, bold, anchor); return; }
            Matrix4x4 old = GUI.matrix;
            GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), ToScreen(r).center);
            var inner = new Rect(r.center.x - r.width * 0.5f / scale, r.center.y - r.height * 0.5f / scale, r.width / scale, r.height / scale);
            Text(inner, text, size, c, bold, anchor);
            GUI.matrix = old;
        }

        /// <summary>
        /// 칸에 맞춘 글(말줄임표 없음): 넘치면 글 크기를 하한(CSCommonDesign.FontMin, 원래 더 작으면 원래 크기)까지 줄여 한 줄로.
        /// 하한에서도 넘치면 칸 밖으로 삐져나가지 않게 FontSqueezeMin 까지 더 줄여 한 줄로 그림 — 칸 · 문구를 고치라고 자세한 로그에 한 번 알림.
        /// </summary>
        public static void TextFit(Rect r, string text, float size, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f || TextOff(r, size, true)) return;
            float floor = Mathf.Min(size, CSCommonDesign.FontMin), limit = r.width + FitSlack;
            float s = FitSize(TextWidth(text, size, bold), size, floor, r.width);
            // 글 폭은 크기에 딱 비례하지 않음(화소 반올림) — 한 단계씩 더 줄여 확인
            while (s > floor && TextWidth(text, s, bold) > limit) s = Mathf.Max(floor, s - FitStep);
            if (TextWidth(text, s, bold) <= limit) { Text(r, text, s, c, bold, anchor); return; }
            if (overflowSeen.Count > 2000) overflowSeen.Clear();
            if (overflowSeen.Add(text)) CSCommonLog.Detail($"경고: 글이 칸보다 넓음(칸 {r.width:0} · 글 '{text}') — 칸 폭을 늘리거나 문구를 줄이세요");
            Text(r, text, floor, c, bold, anchor); // Text 가 칸 폭까지 더 줄임(SqueezePx)
        }

        /// <summary>넘쳐 알린 글(한 번만 알림).</summary>
        static readonly HashSet<string> overflowSeen = new();

        /// <summary>맞춤 글 크기를 찾을 때 줄이는 한 단계(논리 px).</summary>
        public const float FitStep = 0.5f;

        /// <summary>폭 textW(크기 size 에서 잰 글 폭)인 글이 칸 폭 width 에 들어가는 글 크기(비례 어림, floor 밑으로는 안 줄임). 들어가면 size 그대로.</summary>
        public static float FitSize(float textW, float size, float floor, float width)
        {
            floor = Mathf.Min(size, floor);
            if (textW <= width + FitSlack || textW <= 0f) return size;
            return Mathf.Clamp(size * width / textW, floor, size);
        }

        /// <summary>고정폭 알약 글이 넘칠 때 줄이는 글 크기 한 단계(논리 px).</summary>
        public const float PillFontStep = 1.5f;

        /// <summary>고정폭 알약 글(단추 · 배지 · 고르기 칸 · 탭 칸): 넘치면 글 크기를 하한까지 줄임(TextFit, 말줄임표 없음).</summary>
        public static void TextPill(Rect r, string text, float size, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleCenter)
            => TextFit(r, text, size, c, bold, anchor);

        /// <summary>바탕색 위에서 읽히는 글자색: 밝은 바탕 = 어두운 글(Pal.OnLight), 어두운 바탕 = 흰 글(테마와 상관없이 바탕 밝기로).</summary>
        public static Color On(Color bg) => 0.299f * bg.r + 0.587f * bg.g + 0.114f * bg.b > 0.62f ? Pal.OnLight : Color.white;

        /// <summary>캐시 키(글 · 정수 셋): 문자열을 이어 붙이지 않아 찾을 때 쓰레기가 없음. 글은 값으로 비교(참조가 같으면 바로).</summary>
        struct TextKey
        {
            public string T;
            public int A, B, C;
            public TextKey(string t, int a, int b, int c) { T = t; A = a; B = b; C = c; }
        }

        sealed class TextKeyComparer : IEqualityComparer<TextKey>
        {
            public static readonly TextKeyComparer I = new();
            public bool Equals(TextKey x, TextKey y) => x.A == y.A && x.B == y.B && x.C == y.C && string.Equals(x.T, y.T);
            public int GetHashCode(TextKey k) { unchecked { return (((k.T?.GetHashCode() ?? 0) * 397 ^ k.A) * 397 ^ k.B) * 397 ^ k.C; } }
        }

        // 글 폭 캐시: (글, 화소 크기, 굵기) → CalcSize 화소 폭. 글꼴을 찾으면(EnsureFaces) 비움, 배율은 화소 크기에 들어 있음
        static readonly Dictionary<TextKey, float> widthCache = new(TextKeyComparer.I);
        // 글 높이 캐시: (글, 화소 크기, 굵기, 화소 폭) → CalcHeight 화소 높이. widthCache 와 같이 비움
        static readonly Dictionary<TextKey, float> heightCache = new(TextKeyComparer.I);

        /// <summary>
        /// 맞춤 여유(논리 px): 칸 폭을 '글 폭 + 여백'으로 잡아도 칸 x 가 소수(비율로 나눈 카드)면 right − x 가 글 폭보다 아주 조금(1e-5) 작아져,
        /// 1080p(배율 1 — 글 폭이 정수)에서 글이 괜히 줄었음. 반 화소 밑 차이는 넘쳐도 안 보임.
        /// </summary>
        public const float FitSlack = 0.5f;

        public static float TextWidth(string text, float size, bool bold = false)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            int px = Px(size);
            var key = new TextKey(text, px, bold ? 1 : 0, 0);
            if (!widthCache.TryGetValue(key, out float w))
            {
                var style = TextStyle(px, bold, TextAnchor.MiddleLeft, false);
                content.text = text;
                w = style.CalcSize(content).x;
                // 굵게 잰 폭 = 굵게 · 보통 중 넓은 쪽: 알약은 굵게 재고 안 고른 칸은 보통으로 그리는데, 한글은 대체 글꼴이라 보통이 더 넓을 수 있음
                if (bold) w = Mathf.Max(w, TextStyle(px, false, TextAnchor.MiddleLeft, false).CalcSize(content).x);
                if (widthCache.Count > 4000) widthCache.Clear();
                widthCache[key] = w;
            }
            return w / S;
        }

        public static float TextHeight(string text, float size, float width, bool bold = false)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            int px = Px(size);
            float wpx = width * S;
            var key = new TextKey(text, px, bold ? 1 : 0, Mathf.RoundToInt(wpx));
            if (!heightCache.TryGetValue(key, out float h))
            {
                var style = TextStyle(px, bold, TextAnchor.UpperLeft, true);
                content.text = text;
                h = style.CalcHeight(content, wpx);
                if (heightCache.Count > 4000) heightCache.Clear();
                heightCache[key] = h;
            }
            return h / S;
        }

        /// <summary>r 가운데를 축으로 deg도 돌린 글(시계 방향 +). 글꼴 크기는 그대로라 글자 텍스처를 다시 짓지 않음.</summary>
        public static void TextRotated(Rect r, string text, float size, Color c, float deg, bool bold = false, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f) return;
            if (Mathf.Abs(deg) < 0.01f) { Text(r, text, size, c, bold, anchor); return; }
            Matrix4x4 old = GUI.matrix;
            GUIUtility.RotateAroundPivot(deg, ToScreen(r).center);
            Text(r, text, size, c, bold, anchor);
            GUI.matrix = old;
        }

        /// <summary>설계 색 글("#RRGGBB" · "#RRGGBBAA").</summary>
        public static Color Hex(string hex)
        {
            hex = hex.TrimStart('#');
            uint v = Convert.ToUInt32(hex, 16);
            if (hex.Length == 8)
                return new Color32((byte)(v >> 24), (byte)(v >> 16), (byte)(v >> 8), (byte)v);
            return new Color32((byte)(v >> 16), (byte)(v >> 8), (byte)v, 255);
        }

        public static Color Alpha(Color c, float a) => new(c.r, c.g, c.b, c.a * a);

        public static Color Shade(Color c, float f) => new(c.r * f, c.g * f, c.b * f, c.a);

    }
}
