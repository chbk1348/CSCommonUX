using System;
using UnityEngine;

namespace CSCommonUX.UI
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

        static string tip, tipShown;
        static Rect tipAt;
        static float tipSince, tipSeen;

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
            // CSCommonUX.Panel(판 바탕 · 그림자 두 겹 · 1px 선) — 예전 틴트 줄은 없앰
            CSCommonUX.Panel.Frame(r, CSCommonDesign.RadiusWindow);
        }

        /// <summary>선택줄 안의 작은 동작 칩(아이콘 + 글).</summary>
        public static float SmallChip(float x, float cy, string icon, string label, string tipText, Color outline, bool draw, Action click)
        {
            // 공통 단추(CSCommonUX.Button): 예전 테두리 색 = 톤 · 아이콘 뜻 색
            var tone = CSCommonUX.Button.ToneOf(outline, out Color? ink);
            return CSCommonUX.Button.At(x, cy, icon, label, tipText, tone, ink, true, click, draw);
        }

        /// <summary>툴바 아래로 열리는 선택줄 바탕(Elevation 3, 모서리 12). 독 안에서는 독 판이 바탕이라 그리지 않음.</summary>
        public static void FlyoutPanel(Rect r)
        {
            if (InDock) return;
            CSCommonUX.Panel.Frame(r, 12f);
        }

        // ---------------- 위 판 묶음 탭 · 아이콘 단추 · 아래 도구 독(서킷 디자인 에디터 새 틀)

        /// <summary>Dockbar 를 그리는 중: FlyoutPanel 은 바탕 없이(독 판이 바탕).</summary>
        public static bool InDock { get; private set; }

        /// <summary>
        /// 독 도구 단추(툴바 C안 알약 칸 — Dockbar 메인 줄): 안 고른 도구 = 아이콘만(폭 44), 고른 도구 = 파스텔 채움 + 아이콘 + 이름 가운데(폭 = 24 + 8 + 글폭 + 28).
        /// 고른 도구가 바뀌면 폭이 CSCommonAnim.Smooth 로 늘고 줄며 글은 열린 만큼 나타남, 누르면 0.96배. 놓은 수(badge &gt; 0)는 오른쪽 위 작은 알약 · 툴팁.
        /// accent · idle 은 예전 호출 호환용(색은 Look.Pastel* · Look.BarOff 토큰).
        /// </summary>
        public static float DockTool(float x, float cy, string icon, string label, bool on, string tipText, bool draw, Action pick, Color? accent = null, int badge = -1, Color? idle = null)
        {
            float lw = Mathf.Ceil(Draw.TextWidth(label ?? "", CSCommonUX.PillBar.Font, true));
            float selW = CSCommonUX.PillBar.CellW(lw);
            int key = CSCommonAnim.Key(icon, label) ^ 0x6d2b79f5;
            float w = CSCommonAnim.Smooth(key ^ 0x1b873593, on ? selW : CSCommonUX.PillBar.IdleW, 18f);
            if (draw)
            {
                float h = CSCommonUX.PillBar.CellH;
                var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
                CSCommonUX.PillBar.Cell(r, icon, label, lw, CSCommonUX.PillBar.Open(w, selW), CSCommonUX.Look.Hover(key, r, !on), CSCommonUX.Look.Press(key, r));
                string count = badge > 0 ? badge.ToString() : null;
                if (count != null) CSCommonUX.Badge.Count(r.xMax - CSCommonUX.Badge.Count(0f, 0f, count, false), r.y + 9f, count, true, on);
                string head = badge > 0 ? label + " · " + badge : label;
                Tip(r, string.IsNullOrEmpty(tipText) ? head : head + " — " + tipText);
                if (Draw.Button(r)) pick?.Invoke();
            }
            return w;
        }

        /// <summary>위 판 아이콘 단추(정사각 IconBtnSize): 켜지면 Inset 바탕 + 테두리, 호버 · 누름 애니메이션. 툴팁에 이름.</summary>
        public static float IconToggle(float x, float cy, string icon, bool on, string tipText, bool draw, Action click, Color? color = null, bool enabled = true)
        {
            return CSCommonUX.IconButton.At(x, cy, icon, on, tipText, draw, click, color, enabled);
        }

        // 독 옵션 막대 크기: 지난 그림의 것으로 막대를 잡음(옵션은 그리면서 크기가 정해짐) · 숨는 동안은 마지막 크기
        static float dockOptW, dockOptH;
        static readonly int DockKey = CSCommonAnim.Key("CSCommonToolbar", "dock");

        /// <summary>
        /// CSCommonUX.Dockbar — 도구 독(시안 'Dockbar · SubDockBar'): 위에서 아래로 [메인 줄 + 오른쪽 FAB 줄] → 12 → [SubDockBar 옵션 막대] → 12 → [안내 알약].
        /// 메인 줄 = 도구 알약(높이 64 · 반지름 32 · 안쪽 8 · 칸 48 · 안 고른 칸 44 · 고른 칸 24 + 8 + 글폭 + 28 — tools(x, cy, draw) 를 재고 그림, 칸은 DockTool),
        /// 오른쪽 12 에 fabs(x, cy, draw)(CSCommonUX.Fab.At · PillBar.IconGroup 을 PillBar.FabGap 간격으로 놓고 폭을 돌려줌, 없으면 null — 모드 나가기는 맨 오른쪽 FabTone.Exit). 둘을 묶어 화면 가운데.
        /// options(centerX, top) = 고른 도구 옵션(SubDockBar 로 한 줄, 그린 사각형을 돌려줌, 없으면 null): 메인 알약 뒤에서 아래로 뜯어져 나옴(위 가운데 기준 · 숨으면 위로 68 · 가로 62% · 세로 55% · 투명,
        /// 0.46초 오버슈트 · 투명 0.22초), 없어지면 마지막 크기의 빈 막대가 거꾸로 들어감, 옵션이 있는 도구끼리 바꾸면 접히지 않고 폭만 바뀜.
        /// hint(문구의 첫 세 칸 띄움 앞 = 굵은 머리)는 옵션 막대 아래 12(없으면 메인 아래 12)로 부드럽게 옮겨 감.
        /// top &gt;= 0 이면 메인 줄을 그 높이에(위에 붙는 독), &lt; 0 이면 화면 아래에 붙이고 메인 아래 [옵션 막대 · 안내] 자리를 비워 둠(메인 줄은 옵션이 생기고 없어져도 그대로).
        /// 메인 줄 · FAB · 옵션 막대를 덮는 사각형을 돌려줌(지도 클릭 막기).
        /// </summary>
        public static Rect Dockbar(Func<float, float, bool, float> tools, Func<float, float, Rect> options, string hint, Func<float, float, bool, float> fabs = null, float top = -1f)
        {
            float screenW = Draw.Width, screenH = Draw.Height, gap = CSCommonUX.PillBar.SubGap;
            InDock = true;
            Draw.BeginOverlay();
            try
            {
                float toolsW = tools(0f, 0f, false) + CSCommonUX.PillBar.Pad * 2f;
                float fabW = fabs != null ? fabs(0f, 0f, false) : 0f;
                float groupW = toolsW + (fabW > 0f ? CSCommonUX.PillBar.FabGap + fabW : 0f);
                bool hasHint = !string.IsNullOrEmpty(hint);
                float y = top;
                if (top < 0f)
                {
                    // 아래에 붙을 때: 옵션 막대(한 줄보다 크면 그 높이)와 안내 자리를 메인 아래에 비움 — 큰 옵션 칸이면 메인이 부드럽게 올라감
                    float below = gap + Mathf.Max(CSCommonUX.PillBar.SubH, dockOptH) + (hasHint ? gap + CSCommonUX.DockHint.Height : 0f);
                    y = screenH - CSCommonDesign.DockBottomGap - CSCommonUX.PillBar.Height - CSCommonAnim.Smooth(DockKey ^ 0x5f3759df, below, 18f);
                }
                var pill = new Rect(CSCommonUX.PillBar.Left(screenW * 0.5f, groupW), Mathf.Round(y), toolsW, CSCommonUX.PillBar.Height);
                float t = CSCommonUX.PillBar.TearProgress(DockKey, options != null);

                float baseOp = Draw.Opacity;
                float intro = CSCommonAnim.Appear(DockKey ^ 0x11, 0, 0.22f);
                var slide = new Vector2(0f, (top < 0f ? 14f : -14f) * (1f - intro));
                Draw.Opacity = baseOp * intro;
                Draw.Shift(slide);

                // 옵션 막대(SubDockBar): 메인 알약 뒤 층이라 먼저 그림 · 메인 아래로 뜯어져 나옴
                var opt = new Rect(CSCommonUX.PillBar.Left(screenW * 0.5f, dockOptW), pill.yMax + gap, dockOptW, dockOptH);
                if (t > 0f || options != null)
                {
                    CSCommonUX.PillBar.BeginTear(opt, t, -(gap + CSCommonUX.PillBar.Height - CSCommonUX.PillBar.Pad));
                    CSCommonUX.PillBar.Plate(opt, Mathf.Min(CSCommonUX.PillBar.SubH, opt.height) * 0.5f);
                    if (options != null)
                    {
                        var o = options(screenW * 0.5f, opt.y);
                        dockOptW = o.width;
                        dockOptH = o.height;
                    }
                    CSCommonUX.PillBar.EndTear();
                }
                bool hasOpt = t > 0f && opt.width > 0f;

                // 안내 알약: 옵션 막대 아래 12 ↔ 메인 아래 12 (0.34초 곡선 대신 같은 빠르기의 부드러운 따라감)
                float hintOff = 0f;
                if (hasHint)
                {
                    hintOff = CSCommonAnim.Smooth(DockKey ^ 0x2545f491, (options != null ? CSCommonUX.PillBar.Height + gap + dockOptH : CSCommonUX.PillBar.Height) + gap, 18f);
                    CSCommonUX.DockHint.At(screenW * 0.5f, pill.y + hintOff, hint);
                }

                // 독 알약 + FAB 줄 · 옵션 막대(보이는 동안) 전체가 클릭 · 호버를 먹음(밑 창 몸통으로 새지 않게)
                Draw.Block(new Rect(pill.x, pill.y, groupW, pill.height));
                if (hasOpt) Draw.Block(opt);
                CSCommonUX.PillBar.Plate(pill);
                tools(pill.x + CSCommonUX.PillBar.Pad, pill.center.y, true);
                if (fabW > 0f) fabs(pill.xMax + CSCommonUX.PillBar.FabGap, pill.center.y, true);

                // 독 아래 판(DockBarPanel)이 붙을 자리: 지금 보이는 맨 아래(메인 · 옵션 막대 · 안내) + 12
                float bottom = pill.yMax;
                if (hasOpt) bottom = Mathf.Max(bottom, Mathf.Lerp(pill.yMax, opt.yMax, t));
                if (hasHint) bottom = Mathf.Max(bottom, pill.y + hintOff + CSCommonUX.DockHint.Height);
                CSCommonUX.DockBarPanel.Anchor(bottom + gap);

                Draw.Shift(-slide);
                Draw.Opacity = baseOp;
                DrawTip();
                var bounds = new Rect(pill.x, pill.y, groupW, pill.height);
                return hasOpt ? Rect.MinMaxRect(Mathf.Min(bounds.xMin, opt.xMin), bounds.yMin, Mathf.Max(bounds.xMax, opt.xMax), opt.yMax) : bounds;
            }
            finally
            {
                InDock = false;
                Draw.EndOverlay();
            }
        }

        /// <summary>
        /// CSCommonUX.SubDockBar — 독 옵션 막대 한 줄(높이 48 · 반지름 24 · 안쪽 8 · 칩 32 · 칩 사이 4): items(x, cy, draw) 를 재고(draw = false) 막대 가운데에 그림 —
        /// CSCommonUX.PillBar.SubChip · DockLabel · Sep 등. Dockbar 의 options 콜백에서 return CSCommonToolbar.SubDockBar(centerX, top, items). 막대 바탕 · 뜯어짐은 Dockbar 가 그림.
        /// </summary>
        public static Rect SubDockBar(float centerX, float top, Func<float, float, bool, float> items)
        {
            float pad = CSCommonUX.PillBar.SubPad, w = items(0f, 0f, false) + pad * 2f;
            var r = new Rect(CSCommonUX.PillBar.Left(centerX, w), top, w, CSCommonUX.PillBar.SubH);
            items(r.x + pad, r.center.y, true);
            return r;
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

        /// <summary>독 옵션 칸 줄 이름(왼쪽 DockLabelW 칸, 줄 가운데 cy — 여러 줄 칸의 이름 열): 글은 SubDockBar 이름(PillBar.SubText)과 같은 12 굵게 · Look.BarOff · 칩과 같은 32 높이 가운데. 돌려주는 값 = 칸 시작 x.</summary>
        public static float DockLabel(float x, float cy, string label)
        {
            if (Draw.Repaint && !string.IsNullOrEmpty(label))
                Draw.TextFit(new Rect(x, Mathf.Round(cy - CSCommon.PillBar.SubCellH * 0.5f), CSCommonDesign.DockLabelW - CSCommon.PillBar.SubTextPadX, CSCommon.PillBar.SubCellH), label, CSCommon.PillBar.SubTextFont, Look.BarOff, true, TextAnchor.MiddleLeft);
            return x + CSCommonDesign.DockLabelW;
        }

        /// <summary>오른쪽 일괄 열 단추(열 폭 가득 · 테두리 색 = 뜻: 청록 실행 · 빨강 지우기).</summary>
        public static void SideButton(Rect r, string icon, string label, string tipText, Color outline, bool enabled, Action click)
        {
            var tone = CSCommonUX.Button.ToneOf(outline, out Color? ink);
            CSCommonUX.Button.In(r, icon, label, tipText, tone, ink, enabled, click);
        }

        /// <summary>오른쪽 일괄 열 숫자 줄: 이름 왼쪽 · 굵은 값 오른쪽.</summary>
        public static void SideStat(Rect r, string label, string value, Color? valueColor = null)
        {
            CSCommonUX.Stat.Line(r, label, value, valueColor);
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
                // 잘라 그리는 영역(스크롤 칸 · 타일) 안의 칸이면 맨 위 논리 좌표로 — 말풍선은 영역 밖 맨 위에서 그리므로 안 그러면 스크롤 · 영역 시작점만큼 어긋남
                tipAt = Draw.ToRoot(r);
            }
        }

        /// <summary>말풍선 툴팁 그리기.</summary>
        /// <summary>지난 프레임 툴팁을 지움(툴바 밖에서 따로 그리는 판 — 설정 패널 등).</summary>
        public static void ClearTip() => tip = null;

        public static void DrawTip()
        {
            // 0.4초 머물면 뜸(시안 'CSCommonUX.Tooltip') — 다른 글로 옮기면 다시 셈
            // 한 프레임에 여러 판이 DrawTip 을 불러도(빈 판 포함) 셈이 끊기지 않게, 같은 글이 0.1초 넘게 안 보였을 때만 처음부터
            if (string.IsNullOrEmpty(tip)) return;
            float now = Time.unscaledTime;
            if (tip != tipShown || now - tipSeen > 0.1f) { tipShown = tip; tipSince = now; }
            tipSeen = now;
            if (!Draw.Repaint || now - tipSince < CSCommonUX.Tooltip.Delay) return;
            CSCommonUX.Tooltip.Draw(tipAt, tip);
        }
    }
}
