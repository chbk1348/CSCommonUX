using System;
using UnityEngine;

namespace CSCommon.UI
{
    /// <summary>
    /// 주차장 그리기 모드(Parking Lot Tool) 및 F1 서킷 툴킷 표준 머티리얼 상단 플로팅 툴바 엔진:
    /// 2-Pass 동적 측정(Measure & Layout) 구조를 통해 모든 버튼 요소가 툴바 패널을 벗어나는 현상을 100% 원천 방지하며,
    /// Elevation Level 3 다층 섀도우, 도구 칩 버튼, 액션 버튼, 구분선, 말풍선 툴팁 및 하단 상태 알약 바를 제공합니다.
    /// </summary>
    public static class CSCommonToolbar
    {
        public const float DefaultY = CSCommonDesign.BarTop;
        public const float DefaultHeight = CSCommonDesign.BarHeight;
        public const float DefaultBtnH = CSCommonDesign.BtnHeight;
        public const float Gap = CSCommonDesign.Gap;

        static string tip;
        static Rect tipAt;

        /// <summary>한 줄이 화면보다 넓을 때 켜짐: 라벨 버튼은 아이콘만, 점수 칩은 점수만, 로고는 그림만.</summary>
        public static bool Compact { get; private set; }

        /// <summary>
        /// 2-Pass 동적 측정 기반 툴바 렌더링 프레임워크.
        /// drawPass가 false일 때 아이템 너비를 누적하고, 계산된 너비로 패널을 생성한 후 drawPass가 true일 때 정밀 렌더링합니다.
        /// </summary>
        static readonly int BarKey = CSCommonAnim.Key("CSCommonToolbar", "bar"), FlyKey = CSCommonAnim.Key("CSCommonToolbar", "flyout");

        public static Rect Render(
            Func<float, float, bool, float> renderItems,
            float topY = DefaultY,
            float height = DefaultHeight,
            float padding = CSCommonDesign.BarPad,
            Color? highlightTint = null,
            string statusPillMsg = null,
            Color? statusPillColor = null,
            Func<float, float, Rect> flyout = null,
            int flyoutKey = 0)
        {
            tip = null;
            float screenW = Draw.Width;

            // 1. Measure Pass: draw = false로 호출하여 모든 버튼 및 위젯의 실제 필요 너비를 정확히 사전 계산
            Compact = false;
            float contentW = renderItems(0f, 0f, false);
            if (contentW + padding * 2f > screenW - 24f)
            {
                Compact = true;
                contentW = renderItems(0f, 0f, false);
            }
            float barW = Mathf.Min(screenW - 24f, contentW + padding * 2f);
            float barX = Mathf.Round((screenW - barW) * 0.5f);
            var barRect = new Rect(barX, topY, barW, height);

            // 전환: 툴바가 처음 보이면 위에서 내려오며 나타남, 선택줄은 열리거나 바뀔 때 살짝 내려오며 나타남
            float baseOp = Draw.Opacity;
            float intro = CSCommonAnim.Appear(BarKey, 0, 0.22f);
            var barSlide = new Vector2(0f, -12f * (1f - intro));
            Draw.Opacity = baseOp * intro;
            Draw.Shift(barSlide);

            // 2. 머티리얼 패널 & 다층 드롭 섀도우 (Elevation Level 3)
            Panel(barRect, highlightTint);

            // 3. Layout & Draw Pass: draw = true로 호출하여 계산된 바 내부 중앙에 완벽하게 정렬하여 렌더링
            renderItems(barRect.x + padding, barRect.center.y, true);

            // 4. 툴바 아래로 열리는 선택줄(플라이아웃)
            Rect fly = default;
            if (flyout != null)
            {
                float open = CSCommonAnim.Appear(FlyKey, flyoutKey, 0.16f);
                var flySlide = new Vector2(0f, -8f * (1f - open));
                Draw.Opacity = baseOp * intro * open;
                Draw.Shift(flySlide);
                fly = flyout(barRect.center.x, barRect.yMax + 8f);
                Draw.Shift(-flySlide);
                Draw.Opacity = baseOp * intro;
            }
            bool hasFly = fly.width > 0f;
            if (!hasFly) CSCommonAnim.Forget(FlyKey);
            Rect stack = hasFly ? Rect.MinMaxRect(barRect.xMin, barRect.yMin, barRect.xMax, fly.yMax) : barRect;

            // 5. 하단 미니 상태 알약 바 (선택줄이 열려 있으면 그 아래)
            if (!string.IsNullOrEmpty(statusPillMsg))
            {
                DrawStatusPill(stack, screenW, statusPillMsg, statusPillColor);
            }
            Draw.Shift(-barSlide);
            Draw.Opacity = baseOp;

            // 6. 말풍선 툴팁 출력
            DrawTip();

            return hasFly
                ? Rect.MinMaxRect(Mathf.Min(barRect.xMin, fly.xMin), barRect.yMin, Mathf.Max(barRect.xMax, fly.xMax), fly.yMax)
                : barRect;
        }

        /// <summary>주차장 툴 표준 머티리얼 패널 (다층 섀도우 + 짙은 표면 + 상단 하이라이트 선).</summary>
        public static void Panel(Rect r, Color? highlightTint = null)
        {
            // CSCommon.Panel(판 바탕 · 그림자 두 겹 · 1px 선) — 예전 틴트 줄은 없앰
            CSCommon.Panel.Frame(r, CSCommonDesign.RadiusWindow);
        }

        /// <summary>누름 정도 0 ~ 1: 누르는 동안 빠르게 들어가고 떼면 돌아옴(버튼마다 열쇠).</summary>
        static float PressAnim(int key, Rect r, bool hov) => CSCommonAnim.Toward(key ^ 0x27d4eb2f, hov && Draw.Pressed(r) ? 1f : 0f, 22f);

        /// <summary>누름 그늘(어둡게) — 내용은 Draw.Shift 로 1px 내려 그림.</summary>
        static void PressShade(Rect r, float p, float radius)
        {
            if (p > 0.01f) Draw.Round(r, Draw.Alpha(Color.black, 0.16f * p), radius);
        }

        /// <summary>도구 칩 버튼 (선택 토글, 호버 피드백, 아웃라인 아이콘, 툴팁 지원).</summary>
        public static float ToolButton(
            float x, float cy,
            string icon, string label,
            bool on, string tipText,
            bool draw, Action pick,
            Color? accent = null,
            bool isPrimary = false,
            float btnH = DefaultBtnH)
        {
            float iw = !string.IsNullOrEmpty(icon) ? 14f : 0f;
            float tw = !string.IsNullOrEmpty(label) ? Draw.TextWidth(label, CSCommonDesign.FontMin, on || isPrimary) : 0f;
            float g = iw > 0f && tw > 0f ? 5f : 0f;
            float totalW = 10f + iw + g + tw + 10f;

            if (draw)
            {
                var r = new Rect(x, Mathf.Round(cy - btnH * 0.5f), totalW, btnH);
                bool hov = Draw.Hover(r);
                int key = CSCommonAnim.Key(icon, label) ^ 0x0a3f71c3 ^ Mathf.RoundToInt(cy);
                float hv = CSCommonAnim.Toward(key, hov ? 1f : 0f, 10f);
                float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 9f);
                float pr = PressAnim(key, r, hov);

                if (Draw.Repaint)
                {
                    Color ac = accent ?? Pal.Primary;
                    Color bgCol;
                    Color borderCol;
                    Color fgCol;

                    if (isPrimary)
                    {
                        bgCol = Draw.Alpha(ac, Mathf.Lerp(0.18f, 0.32f, hv));
                        borderCol = ac;
                        fgCol = ac;
                    }
                    else
                    {
                        // 꺼짐 ↔ 켜짐을 서서히(호버는 그 위에)
                        bgCol = Color.Lerp(Color.Lerp(Pal.Card, Pal.Line2, hv), Draw.Alpha(ac, Mathf.Lerp(0.25f, 0.38f, hv)), onT);
                        borderCol = Color.Lerp(Color.Lerp(Pal.Line, Pal.Light, hv), ac, onT);
                        fgCol = Color.Lerp(Color.Lerp(Pal.Txt, Color.white, hv), Color.white, onT);
                    }

                    Draw.Round(r, bgCol, CSCommonDesign.RadiusTool);
                    Draw.RoundOutline(r, borderCol, CSCommonDesign.RadiusTool);
                    PressShade(r, pr, CSCommonDesign.RadiusTool);
                    Draw.Shift(new Vector2(0f, pr));

                    float ix = r.x + 10f;
                    if (iw > 0f)
                    {
                        Draw.Icon(icon, new Rect(ix, Mathf.Round(r.center.y - 7f), 14f, 14f), fgCol, 24f, 1.8f);
                        ix += iw + g;
                    }
                    if (tw > 0f)
                    {
                        Draw.Text(new Rect(ix, r.y, tw + 2f, r.height), label, CSCommonDesign.FontMin, fgCol, on || isPrimary, TextAnchor.MiddleLeft);
                    }
                    Draw.Shift(new Vector2(0f, -pr));
                }

                Tip(r, tipText);
                if (Draw.Button(r)) pick?.Invoke();
            }

            return totalW;
        }

        /// <summary>액션/완료 버튼 (채움 배경 강조 버튼).</summary>
        public static float ActionButton(
            float x, float cy,
            string icon, string label,
            Color bg, Color fg,
            string tipText,
            bool draw, Action click,
            float btnH = DefaultBtnH)
        {
            float iw = !string.IsNullOrEmpty(icon) ? 14f : 0f;
            float tw = !string.IsNullOrEmpty(label) ? Draw.TextWidth(label, CSCommonDesign.FontMin, true) : 0f;
            float g = iw > 0f && tw > 0f ? 5f : 0f;
            float totalW = 12f + iw + g + tw + 12f;

            if (draw)
            {
                var r = new Rect(x, Mathf.Round(cy - btnH * 0.5f), totalW, btnH);
                bool hov = Draw.Hover(r);

                int key = CSCommonAnim.Key(icon, label) ^ 0x2c1b3c6d;
                float hv = CSCommonAnim.Toward(key, hov ? 1f : 0f, 10f);
                float pr = PressAnim(key, r, hov);
                if (Draw.Repaint)
                {
                    Color fill = Color.Lerp(bg, Color.white, 0.18f * hv);
                    Draw.Round(r, fill, CSCommonDesign.RadiusTool);
                    Draw.RoundOutline(r, Draw.Alpha(fg, 0.4f), CSCommonDesign.RadiusTool);
                    PressShade(r, pr, 7f);
                    Draw.Shift(new Vector2(0f, pr));

                    float ix = r.x + 12f;
                    if (iw > 0f)
                    {
                        Draw.Icon(icon, new Rect(ix, Mathf.Round(r.center.y - 7f), 14f, 14f), fg, 24f, 2.0f);
                        ix += iw + g;
                    }
                    if (tw > 0f)
                    {
                        Draw.Text(new Rect(ix, r.y, tw + 2f, r.height), label, CSCommonDesign.FontMin, fg, true, TextAnchor.MiddleLeft);
                    }
                    Draw.Shift(new Vector2(0f, -pr));
                }

                Tip(r, tipText);
                if (Draw.Button(r)) click?.Invoke();
            }

            return totalW;
        }

        /// <summary>단독 아이콘 버튼 (닫기, 옵션 등).</summary>
        public static float IconButton(
            float x, float cy,
            string icon,
            string tipText,
            bool draw, Action click,
            Color? hoverColor = null,
            float width = 30f,
            float btnH = DefaultBtnH)
        {
            if (draw)
            {
                var r = new Rect(x, Mathf.Round(cy - btnH * 0.5f), width, btnH);
                bool hov = Draw.Hover(r);
                int key = CSCommonAnim.Key(icon, tipText) ^ 0x68e31da4 ^ Mathf.RoundToInt(cy);
                float hv = CSCommonAnim.Toward(key, hov ? 1f : 0f, 10f);
                float pr = PressAnim(key, r, hov);

                if (Draw.Repaint)
                {
                    Color hCol = hoverColor ?? Pal.Red;
                    if (hv > 0.01f) Draw.Round(r, Draw.Alpha(hCol, 0.22f * hv), CSCommonDesign.RadiusTool);
                    PressShade(r, pr, 7f);
                    Color fg = Color.Lerp(Pal.Sub, hCol, hv);
                    Draw.Icon(icon, new Rect(r.center.x - 7f, r.center.y - 7f + pr, 14f, 14f), fg, 24f, 1.8f);
                }

                Tip(r, tipText);
                if (Draw.Button(r)) click?.Invoke();
            }

            return width;
        }

        /// <summary>로고 & 모드 타이틀 칩.</summary>
        public static float LogoChip(
            float x, float cy,
            string icon, string headerText, string subText,
            Color logoBg,
            bool draw,
            float iconSize = 16f)
        {
            const float lw = 28f;
            float tw1 = Draw.TextWidth(headerText, CSCommonDesign.FontMin, true);
            float tw2 = !string.IsNullOrEmpty(subText) ? Draw.TextWidth(subText, CSCommonDesign.FontMin) : 0f;
            float tw = Mathf.Clamp(Mathf.Max(tw1, tw2), 60f, 150f);
            float totalW = lw + 8f + tw;

            if (draw && Draw.Repaint)
            {
                var lg = new Rect(x, Mathf.Round(cy - 14f), lw, 28f);
                Draw.Round(lg, logoBg, CSCommonDesign.RadiusTile);
                Draw.Icon(icon, new Rect(lg.x + (lw - iconSize) * 0.5f, lg.y + (28f - iconSize) * 0.5f, iconSize, iconSize), Color.white, 24f, 1.8f);

                float tx = x + lw + 8f;
                Draw.Text(new Rect(tx, cy - 13f, tw, 15f), headerText, CSCommonDesign.FontMin, Color.white, true, TextAnchor.MiddleLeft);
                if (!string.IsNullOrEmpty(subText))
                {
                    Draw.TextFit(new Rect(tx, cy + 1f, tw, 13f), subText, CSCommonDesign.FontMin, Pal.Sub, false, TextAnchor.MiddleLeft);
                }
            }

            return totalW;
        }

        /// <summary>상태/점수 뱃지 칩 (클릭 시 액션 지원).</summary>
        public static float BadgeChip(
            float x, float cy,
            string icon, string label,
            Color color,
            string tipText,
            bool draw, Action click = null,
            float btnH = DefaultBtnH)
        {
            float tw = Draw.TextWidth(label, CSCommonDesign.FontMin, true);
            float totalW = 8f + 14f + 6f + tw + 10f;

            if (draw)
            {
                var r = new Rect(x, Mathf.Round(cy - btnH * 0.5f), totalW, btnH);
                bool hov = Draw.Hover(r);

                int key = CSCommonAnim.Key(icon, label) ^ 0x3c6ef372 ^ Mathf.RoundToInt(cy);
                float hv = CSCommonAnim.Toward(key, hov ? 1f : 0f, 10f);
                float pr = click != null ? PressAnim(key, r, hov) : 0f;
                if (Draw.Repaint)
                {
                    Draw.Round(r, Draw.Alpha(color, Mathf.Lerp(0.14f, 0.24f, hv)), CSCommonDesign.RadiusTool);
                    Draw.RoundOutline(r, Draw.Alpha(color, 0.45f), CSCommonDesign.RadiusTool);
                    PressShade(r, pr, 7f);
                    Draw.Icon(icon, new Rect(r.x + 8f, r.center.y - 6f + pr, 13f, 13f), color, 24f, 2.0f);
                    Draw.Text(new Rect(r.x + 24f, r.y + pr, tw + 2f, r.height), label, CSCommonDesign.FontMin, color, true, TextAnchor.MiddleLeft);
                }

                Tip(r, tipText);
                if (click != null && Draw.Button(r)) click();
            }

            return totalW;
        }

        /// <summary>
        /// 툴바 도구 버튼(디자인 시스템 C안 + 이름): 높이 52 · 아이콘 위 · 이름 아래(너비는 이름에 맞춤, 최소 48) · 모서리 10, 설명은 툴팁(이름 — 설명).
        /// 켜지면 강조색으로 꽉 채우고 아이콘은 어두운 잉크, badge &gt; 0 이면 오른쪽 위에 강조색 점(개수는 툴팁).
        /// </summary>
        public static float LabelButton(
            float x, float cy,
            string icon, string label,
            bool on, string tipText,
            bool draw, Action pick,
            Color? accent = null,
            int badge = -1,
            Color? idle = null)
        {
            if (InDock) return DockTool(x, cy, icon, label, on, tipText, draw, pick, accent, badge, idle);
            float h = CSCommonDesign.LabelBtnHeight;
            float w = Mathf.Max(CSCommonDesign.LabelBtnMinW, Mathf.Ceil(Draw.TextWidth(label ?? "", CSCommonDesign.LabelBtnFont, on)) + CSCommonDesign.LabelBtnPadX * 2f);

            if (draw)
            {
                var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
                bool hov = Draw.Hover(r);

                int key = CSCommonAnim.Key(icon, label);
                float hv = CSCommonAnim.Toward(key, hov ? 1f : 0f, 10f);
                float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 9f);
                float pr = PressAnim(key, r, hov);

                if (Draw.Repaint)
                {
                    Color ac = accent ?? Pal.Primary;
                    Color idleFg = idle ?? Color.Lerp(Pal.Light, Color.white, hv);
                    Color fg = Color.Lerp(idleFg, Pal.AccentInk, onT);
                    // 꺼짐: 흰 아웃라인(호버면 짙게) · 켜짐: 강조색으로 채워짐(서서히)
                    Draw.Round(r, Draw.Alpha(Color.white, 0.07f * hv * (1f - onT)), CSCommonDesign.IconBtnRadius);
                    if (onT > 0f) Draw.Round(r, Draw.Alpha(Color.Lerp(ac, Color.white, 0.12f * hv), onT), CSCommonDesign.IconBtnRadius);
                    Draw.RoundOutline(r, Draw.Alpha(Color.white, (0.14f + 0.18f * hv) * (1f - onT)), CSCommonDesign.IconBtnRadius);
                    PressShade(r, pr, CSCommonDesign.IconBtnRadius);

                    // 아이콘 위 · 이름 아래(누르는 동안 1px 내려감)
                    float dy = pr;
                    Draw.Icon(icon, new Rect(Mathf.Round(r.center.x - 9f), r.y + 8f + dy, 18f, 18f), fg, 24f, 1.8f);
                    if (!string.IsNullOrEmpty(label))
                        Draw.Text(new Rect(r.x + 2f, r.y + 30f + dy, r.width - 4f, 14f), label, CSCommonDesign.LabelBtnFont, fg, on, TextAnchor.MiddleCenter);

                    if (badge > 0)
                    {
                        var dot = new Vector2(r.xMax - 8.5f, r.y + 8.5f);
                        Draw.Dot(dot, 5.5f, Pal.Win);
                        Draw.Dot(dot, 3.5f, on ? Pal.AccentInk : ac);
                    }
                }

                string head = badge > 0 ? label + " · " + badge : label;
                Tip(r, string.IsNullOrEmpty(tipText) ? head : head + " — " + tipText);
                if (Draw.Button(r)) pick?.Invoke();
            }

            return w;
        }

        /// <summary>도구 버튼 묶음: 틀 없이 버튼만 2px 간격으로(묶음 사이는 Sep 세로선). inner(x, cy, draw) 는 전체 너비를 돌려줌.</summary>
        public static float Group(float x, float cy, bool draw, Func<float, float, bool, float> inner)
        {
            float w = inner(0f, 0f, false);
            if (draw) inner(x, cy, true);
            return w;
        }

        /// <summary>점수 칩: 점수 네모 + 두 줄 글(제목 · 상태). 색만으로 알리지 않도록 상태 글과 아이콘을 함께 둠.</summary>
        public static float ScoreChip(
            float x, float cy,
            string number, string title, string status,
            Color color, string icon,
            string tipText,
            bool draw, Action click = null)
        {
            float tw = Mathf.Max(Draw.TextWidth(title, CSCommonDesign.FontMin, true), Draw.TextWidth(status, CSCommonDesign.FontMin, true) + 13f);
            float w = Compact ? 36f : 4f + 28f + 8f + tw + 12f;

            if (draw)
            {
                var r = new Rect(x, Mathf.Round(cy - 18f), w, 36f);
                bool hov = Draw.Hover(r);

                int key = CSCommonAnim.Key(title, number) ^ 0x1f83d9ab;
                float hv = CSCommonAnim.Toward(key, hov ? 1f : 0f, 10f);
                float pr = click != null ? PressAnim(key, r, hov) : 0f;
                if (Draw.Repaint)
                {
                    Draw.Round(r, Color.Lerp(Pal.Card, Pal.Inset, hv), CSCommonDesign.IconBtnRadius);
                    Draw.RoundOutline(r, Pal.Line, CSCommonDesign.IconBtnRadius);
                    PressShade(r, pr, 10f);

                    var nb = new Rect(r.x + 4f, r.y + 4f, 28f, 28f);
                    Draw.Round(nb, Draw.Alpha(color, 0.16f), CSCommonDesign.RadiusTile);
                    Draw.RoundOutline(nb, Draw.Alpha(color, 0.55f), CSCommonDesign.RadiusTile);
                    Draw.Text(nb, number, 14f, color, true, TextAnchor.MiddleCenter);

                    if (!Compact)
                    {
                        Draw.Text(new Rect(r.x + 40f, r.y + 3f, tw, 15f), title, CSCommonDesign.FontMin, Pal.Txt, true, TextAnchor.MiddleLeft);
                        Draw.Icon(icon, new Rect(r.x + 40f, r.y + 21f, 10f, 10f), color, 24f, 3f);
                        Draw.Text(new Rect(r.x + 53f, r.y + 18f, tw - 13f, 14f), status, CSCommonDesign.FontMin, color, true, TextAnchor.MiddleLeft);
                    }
                }

                Tip(r, Compact ? title + " " + status + " — " + tipText : tipText);
                if (click != null && Draw.Button(r)) click();
            }

            return w;
        }

        /// <summary>왼쪽 로고 마크: F1 로고 그림 + 가는 구분선 + 두 줄 글(제목 · 부제). 그림이 없으면 글자 F1.</summary>
        public static float LogoMark(float x, float cy, Texture logo, string headerText, string subText, bool draw)
        {
            float tw1 = Draw.TextWidth(headerText, CSCommonDesign.FontMin, true);
            float tw2 = !string.IsNullOrEmpty(subText) ? Draw.TextWidth(subText, CSCommonDesign.FontMin) : 0f;
            float tw = Compact ? 0f : Mathf.Clamp(Mathf.Max(tw1, tw2), 60f, 170f);
            float total = Compact ? CSCommonDesign.LogoWidth : CSCommonDesign.LogoWidth + 9f + 1f + 9f + tw;

            if (draw && Draw.Repaint)
            {
                var lr = new Rect(x, Mathf.Round(cy - CSCommonDesign.LogoHeight * 0.5f), CSCommonDesign.LogoWidth, CSCommonDesign.LogoHeight);
                if (logo != null) Draw.Image(lr, logo, 1f, ScaleMode.ScaleToFit);
                else Draw.Text(lr, "F1", 16f, Pal.Accent, true, TextAnchor.MiddleLeft);

                if (Compact) return total;
                Draw.Fill(new Rect(x + CSCommonDesign.LogoWidth + 9f, cy - 14f, 1f, 28f), Pal.Line2);

                float tx = x + CSCommonDesign.LogoWidth + 19f;
                Draw.Text(new Rect(tx, cy - 14f, tw, 14f), headerText, CSCommonDesign.FontMin, Color.white, true, TextAnchor.MiddleLeft);
                if (!string.IsNullOrEmpty(subText))
                    Draw.TextFit(new Rect(tx, cy + 1f, tw, 13f), subText, CSCommonDesign.FontMin, Pal.Sub, false, TextAnchor.MiddleLeft);
            }

            return total;
        }

        /// <summary>선택줄 안의 분절 선택(양쪽 | 좌측만 | 우측만). dots[i] 가 있으면 글 앞에 색 점.</summary>
        public static float Segmented(float x, float cy, string[] labels, int cur, string[] tips, Color?[] dots, bool draw, Action<int> pick)
        {
            return CSCommon.Segmented.At(x, cy, labels, cur, tips, dots, draw, pick);
        }

        /// <summary>선택줄 안의 작은 동작 칩(아이콘 + 글).</summary>
        public static float SmallChip(float x, float cy, string icon, string label, string tipText, Color outline, bool draw, Action click)
        {
            // 공통 단추(CSCommon.Button): 예전 테두리 색 = 톤 · 아이콘 뜻 색
            var tone = CSCommon.Button.ToneOf(outline, out Color? ink);
            return CSCommon.Button.At(x, cy, icon, label, tipText, tone, ink, true, click, draw);
        }

        /// <summary>끌기 막대를 끄는 중(지도 도구가 클릭을 가져가지 않게).</summary>
        public static bool Sliding => CSCommon.Slider.Sliding;

        /// <summary>
        /// 끌기 막대: 홈 · 채움(fill) · 흰 손잡이. 누르거나 끌면 값(min ~ max, step 단위)이 바뀜. 손을 떼면 released.
        /// 칸 자리로 끄는 막대를 알아봄(Draw.Button 과 같은 까닭 — 그리는 순서 · 개수와 상관없게).
        /// </summary>
        public static float Slider(Rect r, float value, float min, float max, float step, Color fill, out bool released)
        {
            return CSCommon.Slider.In(r, value, min, max, step, fill, out released);
        }

        /// <summary>툴바 아래로 열리는 선택줄 바탕(Elevation 3, 모서리 12). 독 안에서는 독 판이 바탕이라 그리지 않음.</summary>
        public static void FlyoutPanel(Rect r)
        {
            if (InDock) return;
            CSCommon.Panel.Frame(r, 12f);
        }

        // ---------------- 위 판 묶음 탭 · 아이콘 단추 · 아래 도구 독(서킷 디자인 에디터 새 틀)

        /// <summary>독(아래 판)을 그리는 중: LabelButton 은 가로형, FlyoutPanel 은 바탕 없이.</summary>
        public static bool InDock { get; private set; }

        /// <summary>
        /// 독 도구 단추(가로형): 높이 DockToolHeight · 아이콘 18 왼쪽 · 굵은 이름 12 · 놓은 수 알약(badge &gt; 0).
        /// 켜지면 떠오른 면(Look.Raised, 서서히) · 굵은 이름 · 아이콘 강조색, 호버 · 누름 애니메이션.
        /// </summary>
        public static float DockTool(float x, float cy, string icon, string label, bool on, string tipText, bool draw, Action pick, Color? accent = null, int badge = -1, Color? idle = null)
        {
            float h = CSCommonDesign.DockToolHeight;
            const float font = 13.5f;
            float tw = Mathf.Ceil(Draw.TextWidth(label ?? "", font, true));
            string count = badge > 0 ? badge.ToString() : null;
            float pw = count != null ? CSCommon.Badge.Count(0f, 0f, count, false) : 0f;
            float w = 12f + 16f + 8f + tw + (pw > 0f ? 8f + pw : 0f) + 12f;
            if (draw)
            {
                var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
                bool hov = Draw.Hover(r);
                int key = CSCommonAnim.Key(icon, label) ^ 0x6d2b79f5;
                float hv = CSCommonAnim.Toward(key, hov ? 1f : 0f, 10f);
                float onT = CSCommonAnim.Toward(key ^ 0x5bd1e995, on ? 1f : 0f, 9f);
                float pr = PressAnim(key, r, hov);
                if (Draw.Repaint)
                {
                    Color ac = accent ?? Pal.Primary;
                    Draw.Round(r, Draw.Alpha(Pal.Haze, 0.05f * hv * (1f - onT)), CSCommonDesign.RadiusField);
                    CSCommon.Look.Raised(r, 9f, onT);
                    float ix = r.x + 12f, iy = r.center.y + pr;
                    Color fg = Color.Lerp(idle ?? Color.Lerp(Pal.Sub, Pal.Txt, hv), Pal.OnSelected, onT);
                    Draw.Icon(icon, new Rect(ix, Mathf.Round(iy - 8f), 16f, 16f), on ? ac : fg, 24f, 1.9f);
                    Draw.Text(new Rect(ix + 24f, r.y + pr, tw + 2f, r.height), label, font, fg, on, TextAnchor.MiddleLeft);
                    if (count != null) CSCommon.Badge.Count(ix + 24f + tw + 8f, iy, count, true, on);
                }
                string head = badge > 0 ? label + " · " + badge : label;
                Tip(r, string.IsNullOrEmpty(tipText) ? head : head + " — " + tipText);
                if (Draw.Button(r)) pick?.Invoke();
            }
            return w;
        }

        /// <summary>
        /// 위 판 묶음 탭: 어두운 홈 안에 탭들, 고른 탭은 떠오른 면(Look.Raised, 고른 탭이 바뀌면 미끄러져 옮겨 감) · 굵은 이름.
        /// badges[i] 가 있으면 이름 뒤 작은 알약(badgeColors 색).
        /// </summary>
        public static float Tabs(float x, float cy, string[] icons, string[] labels, int cur, string[] badges, Color[] badgeColors, Color[] accents, string[] tips, bool draw, Action<int> pick)
        {
            return CSCommon.Tabs.At(x, cy, icons, labels, cur, badges, badgeColors, accents, tips, draw, pick);
        }

        /// <summary>위 판 아이콘 단추(정사각 IconBtnSize): 켜지면 Inset 바탕 + 테두리, 호버 · 누름 애니메이션. 툴팁에 이름.</summary>
        public static float IconToggle(float x, float cy, string icon, bool on, string tipText, bool draw, Action click, Color? color = null, bool enabled = true)
        {
            return CSCommon.IconButton.At(x, cy, icon, on, tipText, draw, click, color, enabled);
        }

        // 독 옵션 칸 크기: 지난 그림의 것으로 판을 잡음(옵션은 그리면서 크기가 정해짐) · 바뀌면 부드럽게
        static float dockOptW, dockOptH;
        static int dockOptKey = int.MinValue;
        static readonly int DockKey = CSCommonAnim.Key("CSCommonToolbar", "dock");

        /// <summary>
        /// 아래 도구 독: 화면 아래 가운데에 떠 있는 판 — 위 줄 = 도구(tools, 측정 후 그림 · LabelButton 은 가로형), 아래 = 고른 도구 옵션
        /// (options(centerX, top) 이 그린 사각형을 돌려줌, 없으면 접힘). 판 바로 위에 안내 한 줄(hint). 그린 판 사각형을 돌려줌.
        /// </summary>
        public static Rect RenderDock(Func<float, float, bool, float> tools, Func<float, float, Rect> options, int optionsKey, string hint, Color? hintColor = null)
        {
            float screenW = Draw.Width, screenH = Draw.Height;
            InDock = true;
            try
            {
                float toolsW = tools(0f, 0f, false) + CSCommonDesign.BarPad * 2f;
                if (options == null) { dockOptW = dockOptH = 0f; dockOptKey = int.MinValue; }
                else if (optionsKey != dockOptKey) dockOptKey = optionsKey;
                float optH = CSCommonAnim.Smooth(DockKey, options != null ? dockOptH : 0f, 20f);
                float optW = CSCommonAnim.Smooth(DockKey ^ 0x3e1f7a9b, options != null ? dockOptW : 0f, 20f);
                float w = Mathf.Min(screenW - 24f, Mathf.Max(toolsW, optW));
                float h = CSCommonDesign.DockToolsHeight + optH;
                var plate = new Rect(Mathf.Round((screenW - w) * 0.5f), Mathf.Round(screenH - CSCommonDesign.DockBottomGap - h), w, h);

                float baseOp = Draw.Opacity;
                float intro = CSCommonAnim.Appear(DockKey ^ 0x11, 0, 0.22f);
                var slide = new Vector2(0f, 14f * (1f - intro));
                Draw.Opacity = baseOp * intro;
                Draw.Shift(slide);

                if (!string.IsNullOrEmpty(hint))
                    CSCommon.Hint.At(screenW * 0.5f, plate.y - 10f - CSCommonDesign.HintHeight, hint, hintColor);

                Panel(plate, Pal.Primary);
                tools(plate.x + CSCommonDesign.BarPad, plate.y + CSCommonDesign.DockToolsHeight * 0.5f, true);
                if (options != null)
                {
                    if (Draw.Repaint && optH > 1f) Draw.Fill(new Rect(plate.x + 1f, plate.y + CSCommonDesign.DockToolsHeight, plate.width - 2f, 1f), Pal.Line);
                    // 옵션은 칸이 다 펴진 뒤만 또렷하게(접히는 동안은 흐리게)
                    Draw.Opacity = baseOp * intro * Mathf.Clamp01(dockOptH > 0f ? optH / dockOptH : 1f);
                    var o = options(plate.center.x, plate.y + CSCommonDesign.DockToolsHeight);
                    Draw.Opacity = baseOp * intro;
                    if (o.width > 0f) { dockOptW = o.width; dockOptH = o.height; }
                    else dockOptW = dockOptH = 0f;
                }
                Draw.Shift(-slide);
                Draw.Opacity = baseOp;
                DrawTip();
                return plate;
            }
            finally { InDock = false; }
        }

        // ---------------- 독 옵션 칸 부품(시안 EditorDocks: 왼쪽 이름 줄 · 오른쪽 일괄 열)

        /// <summary>
        /// 독 옵션 칸 틀: 왼쪽 leftW × leftH(이름 줄들), side 가 있으면 세로 구분선 뒤 오른쪽 열(DockSideW × sideH).
        /// 판(독) 가운데 centerX · 위 top 에서 시작, 그린 칸을 돌려줌(독이 그 크기로 펴짐). 칸 = 왼쪽 · 오른쪽 그리는 함수가 받음.
        /// </summary>
        public static Rect DockOptions(float centerX, float top, float leftW, float leftH, Action<Rect> left, float sideH = 0f, Action<Rect> side = null)
        {
            float pad = CSCommonDesign.DockPad, gap = CSCommonDesign.DockColGap;
            bool hasSide = side != null;
            float w = pad * 2f + leftW + (hasSide ? gap * 2f + 1f + CSCommonDesign.DockSideW : 0f);
            float h = pad * 2f + Mathf.Max(leftH, hasSide ? sideH : 0f);
            var r = new Rect(Mathf.Round(centerX - w * 0.5f), top, w, h);
            var l = new Rect(r.x + pad, r.y + pad, leftW, h - pad * 2f);
            left?.Invoke(l);
            if (hasSide)
            {
                float sx = l.xMax + gap;
                if (Draw.Repaint) Draw.Fill(new Rect(sx, r.y + pad, 1f, h - pad * 2f), Pal.Line);
                side(new Rect(sx + 1f + gap, r.y + pad, CSCommonDesign.DockSideW, h - pad * 2f));
            }
            return r;
        }

        /// <summary>독 옵션 줄 이름(왼쪽 DockLabelW 칸, 줄 가운데 cy) — 돌려주는 값 = 칸 시작 x.</summary>
        public static float DockLabel(float x, float cy, string label)
        {
            if (Draw.Repaint && !string.IsNullOrEmpty(label))
                Draw.Text(new Rect(x, cy - 8f, CSCommonDesign.DockLabelW - 6f, 16f), label, CSCommonDesign.FontMin, Pal.Sub, false, TextAnchor.MiddleLeft);
            return x + CSCommonDesign.DockLabelW;
        }

        /// <summary>오른쪽 일괄 열 단추(열 폭 가득 · 테두리 색 = 뜻: 청록 실행 · 빨강 지우기).</summary>
        public static void SideButton(Rect r, string icon, string label, string tipText, Color outline, bool enabled, Action click)
        {
            var tone = CSCommon.Button.ToneOf(outline, out Color? ink);
            CSCommon.Button.In(r, icon, label, tipText, tone, ink, enabled, click, true);
        }

        /// <summary>오른쪽 일괄 열 숫자 줄: 이름 왼쪽 · 굵은 값 오른쪽.</summary>
        public static void SideStat(Rect r, string label, string value, Color? valueColor = null)
        {
            CSCommon.Stat.Line(r, label, value, valueColor);
        }

        /// <summary>수직 구분선.</summary>
        public static float Sep(float x, float cy, bool draw)
        {
            if (draw && Draw.Repaint)
            {
                Draw.Line(new Vector2(x + 5f, cy - 12f), new Vector2(x + 5f, cy + 12f), 1f, Pal.Line2);
            }
            return CSCommonDesign.SepWidth;
        }

        /// <summary>툴바 하단 미니 가이드 알약 바.</summary>
        public static void DrawStatusPill(Rect barRect, float screenW, string message, Color? textColor = null)
        {
            if (!Draw.Repaint || string.IsNullOrEmpty(message)) return;

            float pillW = Mathf.Clamp(Draw.TextWidth(message, CSCommonDesign.FontMin) + 24f, 320f, screenW - 64f);
            float pillX = Mathf.Round((screenW - pillW) * 0.5f);
            var pillRect = new Rect(pillX, barRect.yMax + 8f, pillW, 22f);

            Draw.Round(new Rect(pillRect.x - 1f, pillRect.y + 1f, pillRect.width + 2f, pillRect.height + 2f), Draw.Alpha(Color.black, 0.12f), CSCommonDesign.RadiusTile);
            Draw.Round(pillRect, Draw.Alpha(Pal.Win, 0.92f), CSCommonDesign.RadiusTool);
            Draw.RoundOutline(pillRect, Draw.Alpha(Pal.Line2, 0.8f), CSCommonDesign.RadiusTool);
            Draw.Text(pillRect, message, CSCommonDesign.FontMin, textColor ?? Pal.Sub, false, TextAnchor.MiddleCenter);
        }

        /// <summary>말풍선 툴팁 등록.</summary>
        public static void Tip(Rect r, string text)
        {
            if (!string.IsNullOrEmpty(text) && Draw.Hover(r) && !Draw.Holding)
            {
                tip = text;
                tipAt = r;
            }
        }

        /// <summary>말풍선 툴팁 그리기.</summary>
        /// <summary>지난 프레임 툴팁을 지움(툴바 밖에서 따로 그리는 판 — 설정 패널 등).</summary>
        public static void ClearTip() => tip = null;

        public static void DrawTip()
        {
            if (string.IsNullOrEmpty(tip) || !Draw.Repaint) return;
            CSCommon.Tooltip.Draw(tipAt, tip);
        }
    }
}
