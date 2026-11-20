using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Section — 시안 'Section'(라이트 · 다크): 바탕 창 띠(우물 · 반지름 24 · 안쪽 12 · 플레이트 사이 12) 안에 둥근 플레이트
    /// (반지름 24 · 히어로 28 · 안쪽 16 / 아래 12 · 면 + 안쪽 1px 선 · 그림자 없음). 제목 = 모드 파스텔 알약 칩 32(아이콘 16 + 이름 13.5 굵게, 누르지 않는 표시용),
    /// 보조 = 회색 알약 28, 줄 = 알약 행 44(호버 Field 면 · 순번 원 28), 헤어라인 없음. 몸통은 늘 펼침(접기 없음).
    /// </summary>
    public static class Section
    {
        // ---------------- 치수(시안 그대로)

        /// <summary>우물(바탕 창 띠): 반지름 · 안쪽 여백 · 플레이트 사이.</summary>
        public const float WellRadius = 24f, WellPad = 12f, Gap = 12f;
        /// <summary>플레이트: 반지름 · 히어로 반지름 · 안쪽(좌우 · 위) · 아래 · 머리 아래 간격.</summary>
        public const float Radius = 24f, HeroRadius = 28f, Pad = 16f, PadBottom = 12f, HeadGap = 14f;
        /// <summary>제목 칩: 높이 · 왼쪽(아이콘 있을 때) · 오른쪽 · 칸 사이 · 아이콘 · 글 · 선 굵기 · 두 줄 칩 위아래 안쪽.</summary>
        public const float ChipH = 32f, ChipPadL = 12f, ChipPadR = 16f, ChipGap = 8f, ChipIcon = 16f, ChipFont = 13.5f, ChipStroke = 2f, ChipWrapPadY = 7f;
        /// <summary>보조 알약: 높이 · 좌우 · 글.</summary>
        public const float SubH = 28f, SubPadX = 12f, SubFont = 12.5f;
        /// <summary>알약 행: 높이 · 왼쪽(순번 원 앞) · 오른쪽 · 칸 사이 · 글 · 순번 원 · 순번 글.</summary>
        public const float RowH = 44f, RowPadL = 8f, RowPadR = 18f, RowGap = 12f, RowFont = 14f, RankD = 28f, RankFont = 12.5f;
        /// <summary>움직임: 행 호버(선형).</summary>
        public const float HoverSeconds = 0.12f;
        /// <summary>히어로(시안 '홈 · 구역 · 선수촌 · 경기 히어로'): 안쪽 좌우 · 위아래 · 줄 사이 · 큰 제목 글 · 줄 · 뒤 단위 글 · 아래 설명 글 · 줄 · 제목 → 설명 · 숫자(Stat) 사이.</summary>
        public const float HeroPadX = 24f, HeroPadY = 22f, HeroGap = 14f, HeroTitleFont = 30f, HeroTitleH = 36f, HeroUnitFont = 20f, HeroSubFont = 13f, HeroSubH = 18f, HeroSubGap = 4f, HeroStatGap = 32f;
        /// <summary>정보 알약(히어로 머리 · 플레이트 안): 높이 · 좌우 · 글(굵게) · 앞 점 · 점 → 글 · 알약 사이.</summary>
        public const float TagH = 28f, TagPadX = 12f, TagFont = 12.5f, TagDot = 8f, TagDotGap = 6f, TagGap = 6f;
        /// <summary>나라 · 종류 코드 칸(시안 'KOR'): 폭 · 높이 · 반지름 · 글.</summary>
        public const float CodeW = 40f, CodeH = 24f, CodeRadius = 6f, CodeFont = 12f;
        /// <summary>작은 줄(주요 경기장 · 점검 · 고른 칸 줄): 높이 · 줄 앞 표시 원 · 그 안 ✓ · 이름 칸 폭 · 상태 알약 폭.</summary>
        public const float LineH = 36f, TickD = 20f, TickIcon = 12f, LabelW = 72f, StateW = 52f;
        /// <summary>줄 앞 뜻 점(메달 금 · 은 · 동) 지름.</summary>
        public const float DotD = 10f;
        /// <summary>점수 줄(시안 '유치 점수'): 높이 · 막대 폭 · 값 칸 폭.</summary>
        public const float MeterH = 40f, MeterW = 96f, MeterValueW = 64f;
        /// <summary>고른 칸 막대(Tray — 표 위 'n곳 선택됨' 줄): 높이 · 안쪽 · 칸 사이 · 구분선 높이.</summary>
        public const float TrayH = 48f, TrayPad = 8f, TrayGap = 4f, TraySepH = 18f;
        /// <summary>묶음 머리 줄(시안 '지금 경기 중 2'): 높이 · 알약 높이.</summary>
        public const float GroupH = 24f, GroupBadgeH = 20f;
        /// <summary>날짜 칸(시안 경기 날짜 띠): 높이 · 칸 사이 · 위 날짜 글 · 아래 수 글 · 두 줄 사이.</summary>
        public const float DayH = 56f, DayGap = 4f, DayFont = 12f, DayCountFont = 14f, DayLineGap = 4f;
        /// <summary>설정 분류 타일(시안 '설정 왼쪽 분류 목록'): 높이 · 타일 사이.</summary>
        public const float CategoryH = 68f, CategoryGap = 8f;
        /// <summary>창 몸 두 열 비율(시안 왼쪽 6.5 : 오른쪽 3.5) · 설정 왼쪽 분류 목록 비율(34%).</summary>
        public const float MainRatio = 0.65f, ListRatio = 0.34f;
        /// <summary>좁은 열 점수 줄(시안 3열 '유치 점수'): 막대 폭.</summary>
        public const float MeterSmallW = 64f;
        /// <summary>
        /// 가로 띠 히어로(시안 3.0 'Main' 위 띠 · 안쪽은 히어로 판 그대로): 왼쪽 묶음 → 오른쪽 사이 · 큰 숫자 글 · 줄 · 이름 글 · 줄 · 줄 사이.
        /// </summary>
        public const float BandGap = 32f, BandNumFont = 64f, BandNumH = 68f, BandNameFont = 16f, BandNameH = 22f, BandLineGap = 2f;
        /// <summary>상태 띠(시안 '주경기장 1 / 1' 알약 44): 높이 · 왼쪽 · 오른쪽 · 아이콘 · 아이콘 → 글 · 글.</summary>
        public const float BannerH = 44f, BannerPadL = 14f, BannerPadR = 16f, BannerIcon = 16f, BannerGap = 10f, BannerFont = 13.5f;
        /// <summary>수 알약(시안 결과 히어로 금 · 은 · 동 64): 높이 · 왼쪽 · 오른쪽 · 표시 원 · 원 글 · 원 → 수 · 수 글.</summary>
        public const float CountH = 64f, CountPadL = 12f, CountPadR = 20f, CountMarkD = 40f, CountMarkFont = 12f, CountGap = 12f, CountFont = 28f;

        // ---------------- 색(시안 값, 테마를 따라감 — 칩 · 행 글 · 순번은 Look 의 파스텔 · 알약 토큰)


        public static Color WellFill => CSCommonTheme.Light ? Pal.SectionWellLight : Pal.SectionWellDark;
        public static Color Plate => CSCommonTheme.Light ? Pal.SectionPlateLight : Pal.SectionPlateDark;
        public static Color PlateLine => CSCommonTheme.Light ? Pal.SectionLineLight : Pal.SectionLineDark;
        public static Color SubInk => CSCommonTheme.Light ? Pal.SectionSubInkLight : Pal.SectionSubInkDark;

        // ---------------- 판

        /// <summary>우물(바탕 창 띠): 반지름 24 면.</summary>
        public static void Well(Rect r)
        {
            if (Draw.Repaint) Draw.Round(r, WellFill, WellRadius);
        }

        /// <summary>
        /// 플레이트: 면 + 안쪽 1px 선(반지름 24). fill 이 있으면 히어로(그 면 · 반지름 28 · 선 없음, 투명이면 안 그림).
        /// stripe = 예전 위 색 띠(상태 색) — 이제 플레이트 1px 선 색으로만 남김.
        /// </summary>
        public static void Frame(Rect r, Color? fill = null, Color? stripe = null)
        {
            if (!Draw.Repaint) return;
            if (fill.HasValue)
            {
                if (fill.Value.a > 0f) Draw.Round(r, fill.Value, HeroRadius);
                return;
            }
            Draw.Round(r, Plate, Radius);
            Draw.RoundOutline(r, stripe ?? PlateLine, Radius);
        }

        // ---------------- 머리(제목 칩 · 보조 알약)

        /// <summary>제목 칩 폭(글 크기 font · 아이콘 있으면 그 자리 포함). 글 폭은 굵게 · 보통 중 넓은 값(Draw.TextWidth).</summary>
        public static float ChipWidth(string title, string icon, float font = ChipFont) => ChipLead(icon) + Mathf.Ceil(Draw.TextWidth(title ?? "", font, true)) + ChipPadR;

        /// <summary>칩 왼쪽(글 앞) 폭: 아이콘이 없으면 오른쪽과 같은 여백.</summary>
        static float ChipLead(string icon) => string.IsNullOrEmpty(icon) ? ChipPadR : ChipPadL + ChipIcon + ChipGap;

        /// <summary>폭 width 안 칩 글 크기: 한 줄에 들어가면 ChipFont, 아니면 한 단계(Draw.PillFontStep) 작게.</summary>
        static float ChipFontFor(string title, string icon, float width) => ChipWidth(title, icon) <= width + Draw.FitSlack ? ChipFont : ChipFont - Draw.PillFontStep;

        /// <summary>
        /// 제목 줄 높이(폭 width): 한 줄이면 ChipH, 한 단계 작은 글로도 넘치면 두 줄 이상 높이(말줄임 없음). 카드 높이 재기 · 제목 칸에 씀.
        /// 보조 알약은 자리가 모자라면 빠지므로(제목 우선) 높이에 넣지 않음.
        /// </summary>
        public static float HeadHeight(string title, float width, string icon = null)
        {
            if (string.IsNullOrEmpty(title)) return 0f;
            float font = ChipFontFor(title, icon, width);
            if (ChipWidth(title, icon, font) <= width + Draw.FitSlack) return ChipH;
            float room = Mathf.Max(1f, width - ChipLead(icon) - ChipPadR);
            return Mathf.Max(ChipH, Mathf.Ceil(Draw.TextHeight(title, font, room, true)) + ChipWrapPadY * 2f);
        }

        /// <summary>
        /// 제목 줄(누르지 않는 칩): 한 줄이면 r 세로 가운데, 두 줄이면 r 위에 파스텔 칩 · 오른쪽 끝에 회색 보조 알약.
        /// 자리 우선 = 제목 &gt; 보조: 제목 칩이 보조 알약 옆에 다 안 들어가면 보조 알약을 숨김. 그래도 길면 글 한 단계 → 두 줄(높이 = HeadHeight).
        /// </summary>
        public static void Title(Rect r, string title, string sub = null, string icon = null, Color? fill = null, Color? ink = null)
        {
            if (!SubFits(r.width, title, sub, icon)) sub = null;
            float sw = SubPill(r, sub);
            if (!Draw.Repaint || string.IsNullOrEmpty(title)) return;
            float room = Mathf.Max(0f, r.width - sw), font = ChipFontFor(title, icon, room), h = HeadHeight(title, room, icon);
            var c = new Rect(r.x, h > ChipH ? r.y : Mathf.Round(r.center.y - ChipH * 0.5f), Mathf.Min(ChipWidth(title, icon, font), room), h);
            Color fg = ink ?? Look.PastelInk;
            Draw.Round(c, fill ?? Look.PastelFill, ChipH * 0.5f);
            float x = c.x + ChipLead(icon);
            if (!string.IsNullOrEmpty(icon))
                Draw.Icon(icon, new Rect(c.x + ChipPadL, Mathf.Round(c.center.y - ChipIcon * 0.5f), ChipIcon, ChipIcon), fg, 24f, ChipStroke);
            var tr = new Rect(x, c.y, Mathf.Max(0f, c.xMax - ChipPadR - x), c.height);
            // 한 줄 = 칸 폭이 글 폭이라 그대로(Draw.FitSlack 가 소수 x 오차를 흡수), 두 줄 = 줄바꿈
            if (h > ChipH) Draw.Text(new Rect(tr.x, tr.y + ChipWrapPadY, tr.width, tr.height - ChipWrapPadY * 2f), title, font, fg, true, TextAnchor.MiddleLeft, true);
            else Draw.TextFit(tr, title, font, fg, true);
        }

        /// <summary>한 줄 제목 칩(히어로 머리 · 경고 판): 줄 가운데 cy · 왼쪽 x(fill · ink 를 주면 그 색 — 경고 = Look.WarnFill/Ink). 그린 폭을 돌려줌.</summary>
        public static float ChipAt(float x, float cy, string title, string icon = null, Color? fill = null, Color? ink = null, bool draw = true)
        {
            float w = ChipWidth(title, icon);
            if (draw) Title(new Rect(x, Mathf.Round(cy - ChipH * 0.5f), w, ChipH), title, null, icon, fill, ink);
            return w;
        }

        // ---------------- 히어로 · 정보 알약 · 코드 칸 · 고른 칸 막대 · 강조 줄

        /// <summary>히어로 판: Look.HeroFill 면 · 반지름 28 · 선 없음(안 배치는 HeroPad* · HeroGap).</summary>
        public static void Hero(Rect r) => Frame(r, Look.HeroFill);

        /// <summary>히어로 큰 제목 30 굵게(줄 36) + 뒤 단위 20 Look.PillSub, sub 가 있으면 아래 4 띄워 설명 13 SubInk(줄 18). r 높이 = HeroTitleHeight.</summary>
        public static void HeroTitle(Rect r, string title, string unit = null, string sub = null, Color? color = null)
        {
            if (!Draw.Repaint) return;
            float tw = Mathf.Min(r.width, Mathf.Ceil(Draw.TextWidth(title ?? "", HeroTitleFont, true)));
            Draw.TextFit(new Rect(r.x, r.y, tw + 2f, HeroTitleH), title, HeroTitleFont, color ?? Look.BarInk, true);
            if (!string.IsNullOrEmpty(unit))
                Draw.TextFit(new Rect(r.x + tw + Stat.UnitGap, r.y, Mathf.Max(0f, r.width - tw - Stat.UnitGap), HeroTitleH), unit, HeroUnitFont, Look.PillSub);
            if (!string.IsNullOrEmpty(sub)) Draw.TextFit(new Rect(r.x, r.y + HeroTitleH + HeroSubGap, r.width, HeroSubH), sub, HeroSubFont, SubInk);
        }

        public static float HeroTitleHeight(string sub) => HeroTitleH + (string.IsNullOrEmpty(sub) ? 0f : HeroSubGap + HeroSubH);

        /// <summary>
        /// 정보 알약 28(누르지 않음): 기본 = Section.Plate 면(히어로 위 흰 알약) · Look.BarOff 글 12.5 굵게. fill · ink 를 주면 그 색, dot 를 주면 글 앞 점 8(경기 중 빨강).
        /// 그린 폭을 돌려줌(draw = false 면 재기만).
        /// </summary>
        public static float Tag(float x, float cy, string text, Color? fill = null, Color? ink = null, Color? dot = null, bool draw = true)
        {
            float tw = Mathf.Ceil(Draw.TextWidth(text ?? "", TagFont, true)), dw = dot.HasValue ? TagDot + TagDotGap : 0f;
            float w = TagPadX * 2f + dw + tw;
            if (draw && Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - TagH * 0.5f), w, TagH);
                Draw.Round(r, fill ?? Plate, TagH * 0.5f);
                if (dot.HasValue) Draw.Dot(new Vector2(r.x + TagPadX + TagDot * 0.5f, r.center.y), TagDot * 0.5f, dot.Value);
                Draw.Text(new Rect(r.x + TagPadX + dw, r.y, tw + 2f, TagH), text, TagFont, ink ?? Look.BarOff, true, TextAnchor.MiddleLeft);
            }
            return w;
        }

        /// <summary>코드 칸 40 × 24(반지름 6 · 글 12 굵게 Look.BarOff): 평소 Look.Pill 면, on(강조 줄 위) = Section.Plate 면. 그린 폭(40)을 돌려줌.</summary>
        public static float Code(float x, float cy, string code, bool on = false)
        {
            // 확장점: 모드가 코드 → 국기 그림을 주면 [국기 + 코드] 칩(폭은 글자에 맞춰 늘어남)
            var flag = CodeFlag != null && !string.IsNullOrEmpty(code) ? CodeFlag(code) : null;
            float w = CodeSpan; // 국기 칩은 폭 고정(가장 넓은 코드 기준) — 줄마다 이름 시작이 같게, 국기가 없는 팀도 같은 폭
            if (Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - CodeH * 0.5f), w, CodeH);
                Draw.Round(r, on ? Plate : Look.Pill, CodeRadius);
                if (flag == null) Draw.TextPill(r, code ?? "", CodeFont, Look.BarOff, true, TextAnchor.MiddleCenter);
                else
                {
                    Draw.Image(new Rect(x + CodeFlagPadL, Mathf.Round(cy - CodeFlagH * 0.5f), CodeFlagW, CodeFlagH), flag, 1f, ScaleMode.StretchToFill);
                    float tx = x + CodeFlagPadL + CodeFlagW + CodeFlagGap;
                    Draw.TextPill(new Rect(tx, r.y, r.xMax - CodeFlagPadR - tx, r.height), code, CodeFont, Look.BarOff, true, TextAnchor.MiddleCenter);
                }
            }
            return w;
        }

        /// <summary>코드 칩이 차지하는 폭: 국기 확장점이 없으면 CodeW, 있으면 왼쪽 · 국기 · 사이 · 가장 넓은 코드("WWW") · 오른쪽.</summary>
        public static float CodeSpan => CodeFlag == null ? CodeW : CodeFlagPadL + CodeFlagW + CodeFlagGap + (codeTextW > 0f ? codeTextW : codeTextW = Mathf.Ceil(Draw.TextWidth("WWW", CodeFont, true))) + CodeFlagPadR;
        static float codeTextW;

        /// <summary>코드 칩 국기 확장점(없으면 글자만). 모드가 도시를 불러올 때 넣음 — 예: 올림픽 Flags.Get.</summary>
        public static System.Func<string, Texture> CodeFlag;

        /// <summary>국기 칩: 왼쪽 4 · 국기 18 × 12 · 사이 5 · 코드 · 오른쪽 7.</summary>
        public const float CodeFlagW = 18f, CodeFlagH = 12f, CodeFlagGap = 5f, CodeFlagPadL = 4f, CodeFlagPadR = 7f;

        /// <summary>고른 칸 막대 바탕(표 위 'n곳 선택됨' 줄): Look.Pill 알약(반지름 = 높이/2).</summary>
        public static void Tray(Rect r)
        {
            if (Draw.Repaint) Draw.Round(r, Look.Pill, r.height * 0.5f);
        }

        /// <summary>고른 칸 막대 안 세로 구분선(1 × TraySepH, Look.BarLine). 차지한 폭(1)을 돌려줌.</summary>
        public static float TraySep(float x, float cy)
        {
            if (Draw.Repaint) Draw.Fill(new Rect(x, Mathf.Round(cy - TraySepH * 0.5f), 1f, TraySepH), Look.BarLine);
            return 1f;
        }

        /// <summary>강조 줄 면(우리 소속 · 지금 고른 경기장): Look.PastelSubFill 알약.</summary>
        public static void Mark(Rect r)
        {
            if (Draw.Repaint) Draw.Round(r, Look.PastelSubFill, r.height * 0.5f);
        }

        /// <summary>줄 앞 표시 원 20: on = 잘됨 파스텔(Look.OkFill) + ✓ 12, off = Look.Pill 빈 원. 그린 폭(20)을 돌려줌.</summary>
        public static float Tick(float x, float cy, bool on)
        {
            if (Draw.Repaint)
            {
                var c = new Vector2(x + TickD * 0.5f, cy);
                Draw.Dot(c, TickD * 0.5f, on ? Look.OkFill : Look.Pill);
                if (on) Draw.Icon(Icons.Done, new Rect(Mathf.Round(c.x - TickIcon * 0.5f), Mathf.Round(c.y - TickIcon * 0.5f), TickIcon, TickIcon), Look.OkInk, 24f, Check.Stroke);
            }
            return TickD;
        }

        /// <summary>회색 보조 알약 폭(글 전체 — 말줄임 없음).</summary>
        static float SubPillWidth(Rect r, string sub) => Mathf.Ceil(Draw.TextWidth(sub, SubFont)) + SubPadX * 2f;

        /// <summary>
        /// 보조 알약이 말줄임 없이 다 들어가나: 제목 칩 + 칸 사이 + 알약 글 전체 ≤ 폭. 안 들어가면 알약을 그리지 않음
        /// (부르는 쪽은 설명을 제목 아래 본문 글로 — 예: CSCommonOptionsPanel 섹션 설명).
        /// </summary>
        public static bool SubFits(float width, string title, string sub, string icon = null)
            => !string.IsNullOrEmpty(sub) && ChipWidth(title, icon) + ChipGap + Mathf.Ceil(Draw.TextWidth(sub, SubFont)) + SubPadX * 2f <= width + Draw.FitSlack;

        /// <summary>회색 보조 알약(오른쪽 끝, 글 전체 · 말줄임 없음 — 안 들어가면 Title 이 숨김). 차지한 폭(+ 칸 사이)을 돌려줌, 없으면 0.</summary>
        static float SubPill(Rect r, string sub)
        {
            if (string.IsNullOrEmpty(sub)) return 0f;
            float w = SubPillWidth(r, sub);
            if (Draw.Repaint)
            {
                var p = new Rect(r.xMax - w, Mathf.Round(r.center.y - SubH * 0.5f), w, SubH);
                Draw.Round(p, Look.Pill, SubH * 0.5f);
                Draw.Text(new Rect(p.x + SubPadX, p.y, p.width - SubPadX * 2f + 2f, p.height), sub, SubFont, SubInk, false, TextAnchor.MiddleCenter);
            }
            return w + ChipGap;
        }

        // ---------------- 알약 행

        /// <summary>알약 행 바탕: 호버 면 없음(자리만 유지 — 호출부 호환).</summary>
        public static void Row(Rect r, bool hoverable = true)
        { /* 호버 면 없음 — 줄은 면 없이 글 · 부품만 */         }

        /// <summary>행 왼쪽 순번 원(28 · Pill 면 · 12.5 BarOff 글). 다음 글이 시작할 x 를 돌려줌.</summary>
        public static float Rank(Rect row, int n)
        {
            var c = new Rect(row.x + RowPadL, Mathf.Round(row.center.y - RankD * 0.5f), RankD, RankD);
            if (Draw.Repaint)
            {
                Draw.Round(c, Look.Pill, RankD * 0.5f);
                Draw.Text(c, n.ToString(), RankFont, Look.BarOff, false, TextAnchor.MiddleCenter);
            }
            return c.xMax + RowGap;
        }

        /// <summary>
        /// 상태 띠 44(시안 준비 중 '주경기장 1 / 1 · …' · '주경기장이 없어요'): fill 알약 · 아이콘 16 · 글 13.5 굵게 ink · 오른쪽 hint 12.
        /// 글은 hint 앞까지 한 줄(TextFit). click 이 있으면 누를 수 있고 눌렀으면 true.
        /// </summary>
        public static bool Banner(Rect r, string icon, string text, string hint, Color fill, Color ink, bool click = false)
        {
            if (Draw.Repaint)
            {
                Draw.Round(r, fill, r.height * 0.5f);
                float x = r.x + BannerPadL, right = r.xMax - BannerPadR;
                if (!string.IsNullOrEmpty(icon))
                {
                    Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - BannerIcon * 0.5f), BannerIcon, BannerIcon), ink, 24f, ChipStroke);
                    x += BannerIcon + BannerGap;
                }
                if (!string.IsNullOrEmpty(hint))
                {
                    float hw = Mathf.Ceil(Draw.TextWidth(hint, CSCommonDesign.FontMin));
                    Draw.Text(new Rect(right - hw - 2f, r.y, hw + 2f, r.height), hint, CSCommonDesign.FontMin, ink, false, TextAnchor.MiddleRight);
                    right -= hw + BannerGap;
                }
                Draw.TextFit(new Rect(x, r.y, Mathf.Max(0f, right - x), r.height), text, BannerFont, ink, true);
            }
            return click && Draw.Button(r);
        }

        /// <summary>수 알약 64(시안 결과 '금 12'): Section.Plate 알약 · 왼쪽 표시 원 40(markColor · 흰 글 mark) · 수 28 굵게. mark 가 없으면 value 만 가운데 13 SubInk(시안 '메달 31개').</summary>
        public static void Count(Rect r, string mark, string value, Color markColor)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, Plate, r.height * 0.5f);
            if (string.IsNullOrEmpty(mark))
            {
                Draw.TextFit(new Rect(r.x + CountPadR, r.y, r.width - CountPadR * 2f, r.height), value, CSCommonDesign.FontDesc, SubInk, false, TextAnchor.MiddleCenter);
                return;
            }
            var c = new Rect(r.x + CountPadL, Mathf.Round(r.center.y - CountMarkD * 0.5f), CountMarkD, CountMarkD);
            Draw.Round(c, markColor, CountMarkD * 0.5f);
            Draw.TextPill(c, mark, CountMarkFont, Draw.On(markColor), true);
            float x = c.xMax + CountGap;
            Draw.TextFit(new Rect(x, r.y, Mathf.Max(0f, r.xMax - CountPadR - x), r.height), value, CountFont, Look.BarInk, true);
        }

        /// <summary>
        /// 가는 구분선(Look.Divider 1px, 들임 inset). 새 Section 안 줄 사이엔 쓰지 않음(헤어라인 없음) —
        /// 섹션이 아닌 곳(대화 날짜 줄 · 떠 있는 판 발 · 표 칸)의 예전 호출이 남아 있어 둠.
        /// </summary>
        public static void Hairline(Rect r, float inset = 0f)
        {
            if (Draw.Repaint) Draw.Fill(new Rect(r.x + inset, r.y, r.width - inset, 1f), Look.Divider);
        }
    }
}
