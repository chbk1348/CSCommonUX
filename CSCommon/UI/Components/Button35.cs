using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>3.5 버튼 꼴: 주(강조 면 · 강조 위 글, 한 카드에 하나) · 보조(놓인 면 한 단 + Hairline · InkTxt) · 고스트(투명 · InkSub) · 위험(SemBad · OnSemBad).</summary>
    public enum Tone35 { Primary, Secondary, Ghost, Danger }

    /// <summary>
    /// 3.5 버튼(FIA 캔버스 R5PartRows · R5PartCard · R5Circuit): 보통 40 · 좌우 16 / 작은 32 · 좌우 12 · r8 · 글 14 가운데 · 아이콘 18 → 8 → 글.
    /// 누름 0.97배(판 · 글만, 아이콘 크기 그대로) · 면 색 0.15초. 꺼짐 = 놓인 면 한 단 · InkDim · 누름 없음. 호버 면은 고스트만(놓인 면 한 단).
    /// 초점 링(focus) = 강조 2px · 버튼 바깥 2. 강조 · 강조 위 글은 모드가 고른 시리즈 값(Pal.AccentF1 · OnAccentF1 …)을 넘김.
    /// </summary>
    public static class Button35
    {
        /// <summary>버튼 폭: 좌우 여백 + [아이콘 18 (+ 8)] + 글 폭. label 이 비면 아이콘만(정사각 = 높이).</summary>
        public static float Width(string icon, string label, bool small = false)
        {
            float h = small ? D35.BtnSmallH : D35.BtnH;
            if (string.IsNullOrEmpty(label)) return h;
            float pad = small ? D35.BtnSmallPadX : D35.BtnPadX;
            return pad * 2f + Mathf.Ceil(Draw.TextWidth(label, D35.FontBody)) + (string.IsNullOrEmpty(icon) ? 0f : D35.IconM + D35.IconTextGap);
        }

        /// <summary>면 · 글 색(꺼짐 · 호버 반영 전): on = 놓인 면(보조 · 꺼짐 면 = Surface.Raise(on)).</summary>
        public static void Colors(Tone35 tone, bool enabled, Color accent, Color onAccent, Surface.Kind on, out Color face, out Color ink, out bool line)
        {
            line = false;
            if (!enabled) { face = Surface.Raise(on); ink = Pal.InkDim; return; }
            switch (tone)
            {
                case Tone35.Primary: face = accent; ink = onAccent; break;
                case Tone35.Danger: face = Pal.SemBad; ink = Pal.OnSemBad; break;
                case Tone35.Ghost: face = Color.clear; ink = Pal.InkSub; break;
                default: face = Surface.Raise(on); ink = Pal.InkTxt; line = true; break;
            }
        }

        /// <summary>
        /// 버튼을 r 에 그리고 눌러 떼면 true. label 이 비면 아이콘만(아이콘 크기 iconSize — 32 칸 16 · 18, 40 칸 18).
        /// tip = 툴팁 · 접근성 이름(아이콘만 버튼은 꼭 넘김). on = 놓인 면(카드 · 모달 위 Card, 히어로 · 창 몸 위 Hero · Ground).
        /// </summary>
        public static bool Paint(Rect r, string label, Tone35 tone, Color accent, Color onAccent, string icon = null, bool enabled = true,
            bool focus = false, string tip = null, Surface.Kind on = Surface.Kind.Card, float iconSize = D35.IconM, Color? iconInk = null)
        {
            int key = Look.Key(r, "btn35", label ?? icon);
            float pr = Look.Press(key, r, enabled);
            float hover = tone == Tone35.Ghost ? CSCommonAnim.Toward(key ^ 0x3c6ef372, enabled && Draw.Hover(r) ? 1f : 0f, 1f / D35.DurFeedback) : 0f;
            if (Draw.Repaint)
            {
                Colors(tone, enabled, accent, onAccent, on, out Color face, out Color ink, out bool line);
                if (hover > 0.01f) face = Color.Lerp(face, Surface.Raise(on), hover);
                if (focus && enabled)
                {
                    float g = D35.FocusRingGap + D35.FocusRingW;
                    Draw.RoundOutline(new Rect(r.x - g, r.y - g, r.width + g * 2f, r.height + g * 2f), accent, D35.RadiusCtrl + g, D35.FocusRingW);
                }
                Draw.BeginScale(r.center, Look.PressScale(pr, D35.PressScale));
                Draw.Round(r, face, D35.RadiusCtrl);
                if (line) Draw.RoundOutline(r, Pal.Hairline, D35.RadiusCtrl, 1f);
                bool hasLabel = !string.IsNullOrEmpty(label), hasIcon = !string.IsNullOrEmpty(icon);
                float iw = hasIcon ? (hasLabel ? D35.IconM : iconSize) : 0f;
                float tw = hasLabel ? Mathf.Ceil(Draw.TextWidth(label, D35.FontBody)) : 0f;
                float x = Mathf.Round(r.center.x - (iw + (hasIcon && hasLabel ? D35.IconTextGap : 0f) + tw) * 0.5f);
                if (hasLabel) Draw.Text(new Rect(x + iw + (hasIcon ? D35.IconTextGap : 0f), r.y, tw + 2f, r.height), label, D35.FontBody, ink);
                Draw.EndScale();
                // 아이콘은 누름 배율 밖(크기 그대로)
                // 아이콘만 보조 버튼(±)은 InkSub — 재생처럼 다른 색은 iconInk 로
                Color ic = iconInk.HasValue && enabled ? iconInk.Value : !hasLabel && tone == Tone35.Secondary && enabled ? Pal.InkSub : ink;
                if (hasIcon) Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - iw * 0.5f), iw, iw), ic, D35.IconBox, D35.IconLine);
            }
            CSCommonToolbar.Tip35(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>오른쪽 끝 right 에서 왼쪽으로 버튼을 놓고(세로 가운데 cy) right 를 버튼 왼쪽 − 8 로 옮김. 꼬리 · 머리 버튼 줄(오른쪽 맞춤 · 사이 8).</summary>
        public static bool AtRight(ref float right, float cy, string label, Tone35 tone, Color accent, Color onAccent, string icon = null, bool small = false,
            bool enabled = true, string tip = null, Surface.Kind on = Surface.Kind.Card)
        {
            float h = small ? D35.BtnSmallH : D35.BtnH, w = Width(icon, label, small);
            var r = new Rect(right - w, Mathf.Round(cy - h * 0.5f), w, h);
            right = r.x - D35.BtnGap;
            return Paint(r, label, tone, accent, onAccent, icon, enabled, false, tip, on);
        }
    }

    /// <summary>
    /// 3.5 아이콘만 버튼: 32(아이콘 16 또는 18) · 40(재생 · 모달 닫기 18) 정사각 · r8. 기본 고스트(투명 · InkSub · 호버 = 놓인 면 한 단 — 모달 위에서는 GroundRaised),
    /// 면 있는 꼴(± · 재생)은 Tone35.Secondary(아이콘 InkSub, 재생은 iconInk = InkTxt). tip = 툴팁 · 접근성 이름('이전 달' · '낮추기' · '닫기').
    /// </summary>
    public static class IconButton35
    {
        public static bool Paint(Rect r, string icon, string tip, Tone35 tone = Tone35.Ghost, float iconSize = D35.IconM, bool enabled = true,
            Surface.Kind on = Surface.Kind.Card, Color? iconInk = null, Color accent = default, Color onAccent = default) =>
            Button35.Paint(r, null, tone, accent, onAccent, icon, enabled, false, tip, on, iconSize, iconInk);
    }

    /// <summary>3.5 글 링크(R5Circuit 'FIA 탭 열기'): 글 14 InkSub → 4 → Right 16, 호버 InkTxt(0.15초). 왼쪽 x · 세로 가운데 cy, 누르면 true.</summary>
    public static class TextLink35
    {
        public static float Width(string label) => Mathf.Ceil(Draw.TextWidth(label ?? "", D35.FontBody)) + D35.LinkGap + D35.IconS;

        public static bool Paint(float x, float cy, string label, string tip = null)
        {
            var r = new Rect(x, Mathf.Round(cy - D35.LineBody * 0.5f), Width(label), D35.LineBody);
            int key = Look.Key(r, "link35", label);
            float hover = CSCommonAnim.Toward(key, Draw.Hover(r) ? 1f : 0f, 1f / D35.DurFeedback);
            if (Draw.Repaint)
            {
                Color ink = Color.Lerp(Pal.InkSub, Pal.InkTxt, hover);
                float tw = r.width - D35.LinkGap - D35.IconS;
                Draw.Text(new Rect(r.x, r.y, tw + 2f, r.height), label, D35.FontBody, ink);
                Draw.Icon(Icons.Right, new Rect(r.x + tw + D35.LinkGap, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), ink, D35.IconBox, D35.IconLine);
            }
            CSCommonToolbar.Tip35(r, tip);
            return Draw.Button(r);
        }
    }
}
