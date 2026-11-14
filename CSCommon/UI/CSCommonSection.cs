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

        static readonly Color WellLight = Draw.Hex("#EEF0F2"), WellDark = Draw.Hex("#0E1014");
        static readonly Color PlateLight = Draw.Hex("#FEFEFE"), PlateDark = Draw.Hex("#1B1F26");
        static readonly Color LineLight = Draw.Hex("#E4E6EA"), LineDark = Draw.Hex("#2A2F38");
        static readonly Color SubInkLight = Draw.Hex("#5F6368"), SubInkDark = Draw.Hex("#9AA3AF");

        public static Color WellFill => CSCommonTheme.Light ? WellLight : WellDark;
        public static Color Plate => CSCommonTheme.Light ? PlateLight : PlateDark;
        public static Color PlateLine => CSCommonTheme.Light ? LineLight : LineDark;
        public static Color SubInk => CSCommonTheme.Light ? SubInkLight : SubInkDark;

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

    /// <summary>
    /// CSCommonUX.Gauge — 반원 게이지(시안 준비 중 '예상 준비도' 440 × 250) · 고리(시안 개최 중 '운영 점수' 176).
    /// 홈 Look.PastelFill · 채움 Look.PastelStrong · 둥근 끝, 목표 선 3 · Look.BarInk + 이름 12 굵게. 가운데 값 굵게 · 아래 이름 13 SubInk.
    /// </summary>
    public static class Gauge
    {
        /// <summary>반원: 굵기 · 위(목표 이름 자리) · 아래(0 · 100 자리) · 옆 · 값 글 · 값 줄 · 이름 글 · 줄 · 목표 선 · 선 안쪽 · 바깥 · 목표 이름 거리 · 옆으로.</summary>
        public const float HalfStroke = 26f, HalfTop = 38f, HalfBottom = 44f, HalfSide = 52f, HalfValueFont = 60f, HalfValueH = 64f, LabelFont = 13f, LabelH = 18f;
        public const float TickW = 3f, TickIn = 22f, TickOut = 20f, TickLabel = 36f, TickLabelShift = 14f, EndLabelY = 30f;
        /// <summary>고리: 굵기 · 값 글 · 줄.</summary>
        public const float RingStroke = 16f, RingValueFont = 40f, RingValueH = 44f;

        /// <summary>반원 높이(폭 w): 시안 비율(반지름 = 폭/2 − 옆).</summary>
        public static float HalfHeight(float w) => Mathf.Max(0f, w * 0.5f - HalfSide) + HalfTop + HalfBottom;

        static void Band(Vector2 c, float radius, float stroke, float from, float to, Color color)
        {
            if (to - from <= 0.01f) return;
            Draw.Arc(c, radius, stroke, from, to, color);
            // 둥근 끝
            float a0 = from * Mathf.Deg2Rad, a1 = to * Mathf.Deg2Rad;
            Draw.Dot(c + new Vector2(Mathf.Cos(a0), Mathf.Sin(a0)) * radius, stroke * 0.5f, color);
            Draw.Dot(c + new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * radius, stroke * 0.5f, color);
        }

        static Vector2 At(Vector2 c, float deg, float radius) => c + new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad)) * radius;

        /// <summary>반원(왼쪽 0 → 위 → 오른쪽 1): t 채움 · goal 목표 선(goalLabel 은 선 바깥) · 가운데 value / label · 양끝 아래 min · max.</summary>
        public static void Half(Rect r, float t, float goal, string value, string label, string goalLabel, string min, string max)
        {
            if (!Draw.Repaint) return;
            float radius = Mathf.Max(HalfStroke, Mathf.Min(r.width * 0.5f - HalfSide, r.height - HalfTop - HalfBottom));
            var c = new Vector2(r.center.x, r.y + HalfTop + radius);
            Band(c, radius, HalfStroke, 180f, 360f, Look.PastelFill);
            Band(c, radius, HalfStroke, 180f, 180f + 180f * Mathf.Clamp01(t), Look.PastelStrong);
            if (goal >= 0f)
            {
                float deg = 180f + 180f * Mathf.Clamp01(goal);
                Draw.RoundLine(At(c, deg, radius - TickIn), At(c, deg, radius + TickOut), TickW, Look.BarInk);
                if (!string.IsNullOrEmpty(goalLabel))
                {
                    var p = At(c, deg, radius + TickLabel);
                    float w = Mathf.Ceil(Draw.TextWidth(goalLabel, CSCommonDesign.FontMin, true));
                    Draw.Text(new Rect(p.x + TickLabelShift - w * 0.5f - 1f, p.y - LabelH * 0.5f, w + 2f, LabelH), goalLabel, CSCommonDesign.FontMin, Look.BarInk, true, TextAnchor.MiddleCenter);
                }
            }
            Draw.TextFit(new Rect(c.x - radius + HalfStroke, c.y - LabelH - HalfValueH, (radius - HalfStroke) * 2f, HalfValueH), value, HalfValueFont, Look.BarInk, true, TextAnchor.MiddleCenter);
            Draw.TextFit(new Rect(c.x - radius + HalfStroke, c.y - LabelH, (radius - HalfStroke) * 2f, LabelH), label, LabelFont, Section.SubInk, false, TextAnchor.MiddleCenter);
            if (!string.IsNullOrEmpty(min)) Draw.Text(new Rect(c.x - radius - HalfSide, c.y + EndLabelY - LabelH * 0.5f, HalfSide * 2f, LabelH), min, CSCommonDesign.FontMin, Look.PillSub, false, TextAnchor.MiddleCenter);
            if (!string.IsNullOrEmpty(max)) Draw.Text(new Rect(c.x + radius - HalfSide, c.y + EndLabelY - LabelH * 0.5f, HalfSide * 2f, LabelH), max, CSCommonDesign.FontMin, Look.PillSub, false, TextAnchor.MiddleCenter);
        }

        /// <summary>고리(위에서 시계 방향): r 안 가장 큰 원 · t 채움 · 가운데 value 40 굵게 / label 12.</summary>
        public static void Ring(Rect r, float t, string value, string label)
        {
            if (!Draw.Repaint) return;
            float radius = Mathf.Min(r.width, r.height) * 0.5f - RingStroke * 0.5f;
            var c = r.center;
            Draw.Arc(c, radius, RingStroke, 0f, 360f, Look.PastelFill);
            Band(c, radius, RingStroke, -90f, -90f + 360f * Mathf.Clamp01(t), Look.PastelStrong);
            float inner = (radius - RingStroke) * 2f, top = Mathf.Round(c.y - (RingValueH + CSCommonDesign.LineSub) * 0.5f);
            Draw.TextFit(new Rect(c.x - inner * 0.5f, top, inner, RingValueH), value, RingValueFont, Look.BarInk, true, TextAnchor.MiddleCenter);
            Draw.TextFit(new Rect(c.x - inner * 0.5f, top + RingValueH, inner, CSCommonDesign.LineSub), label, CSCommonDesign.FontMin, Section.SubInk, false, TextAnchor.MiddleCenter);
        }
    }

    /// <summary>
    /// CSCommonUX.Cells — 칸 막대(시안 '준비비 납부' 25칸 · '특수 남은 기간' 24칸): 칸 사이 3 · 반지름 6 · 높이 28.
    /// 앞 filled 칸 = done(없으면 Look.PastelStrong), 나머지 = rest(없으면 Look.PillNeutral), current 칸은 안쪽 2px Look.PastelStrong 테두리.
    /// </summary>
    public static class Cells
    {
        public const float H = 28f, Gap = 3f, Radius = 6f, Ring = 2f;

        public static void In(Rect r, int total, int filled, int current = -1, Color? done = null, Color? rest = null)
        {
            if (!Draw.Repaint || total <= 0) return;
            float w = (r.width - Gap * (total - 1)) / total, rad = Mathf.Min(Radius, w * 0.5f);
            for (int i = 0; i < total; i++)
            {
                var c = new Rect(r.x + i * (w + Gap), r.y, w, r.height);
                Draw.Round(c, i < filled ? done ?? Look.PastelStrong : rest ?? Look.PillNeutral, rad);
                if (i == current) Draw.RoundOutline(c, Look.PastelStrong, rad, Ring);
            }
        }
    }

    /// <summary>
    /// CSCommonUX.WindowHead — 모드 창 머리줄(시안 '올림픽 3.0' 창 머리): 높이 64 · 왼쪽 16 · 오른쪽 12 · 요소 사이 12.
    /// [로고(부르는 쪽이 그림 · LogoW × LogoH)] [이름 15.5 굵게(줄 22) + 옆 작은 글 note · 아래 부제 12 SubInk(줄 17)] … [날짜 14 굵게(줄 20) · 아래 상태 알약 24] [닫기 IconButton 40].
    /// 닫기를 눌러 떼면 true. 머리줄 아래 선 없음(몸 우물이 바로 이어짐).
    /// </summary>
    public static class WindowHead
    {
        public const float H = 64f, PadL = 16f, PadR = 12f, Gap = 12f, LogoW = 45f, LogoH = 30f, NameFont = 15.5f, NameH = 22f, SubFont = 12f, SubH = 17f, LineGap = 2f, DateFont = 14f, DateH = 20f;

        public static bool In(Rect r, System.Action<Rect> logo, string name, string note, string sub, string date, string badge, Color badgeColor, string closeTip)
        {
            bool closed = false;
            float cy = r.y + H * 0.5f, x = r.x + PadL;
            logo?.Invoke(new Rect(x, Mathf.Round(cy - LogoH * 0.5f), LogoW, LogoH));
            x += LogoW + Gap;
            float right = r.xMax - PadR - IconButton.Size;
            IconButton.At(right, cy, Icons.Close, false, closeTip, true, () => closed = true);
            right -= Gap;
            float bw = string.IsNullOrEmpty(badge) ? 0f : Badge.At(0f, 0f, badge, badgeColor, null, false);
            float dw = Mathf.Ceil(Mathf.Max(Draw.TextWidth(date ?? "", DateFont, true), bw));
            if (Draw.Repaint)
            {
                float top = Mathf.Round(cy - (DateH + LineGap + Badge.H) * 0.5f);
                Draw.Text(new Rect(right - dw, top, dw, DateH), date ?? "", DateFont, Look.BarInk, true, TextAnchor.MiddleRight);
                if (bw > 0f) Badge.At(right - bw, top + DateH + LineGap + Badge.H * 0.5f, badge, badgeColor);
                float room = Mathf.Max(0f, right - dw - Gap - x), nt = Mathf.Round(cy - (NameH + LineGap + SubH) * 0.5f);
                float nw = Mathf.Min(room, Mathf.Ceil(Draw.TextWidth(name ?? "", NameFont, true)));
                Draw.TextFit(new Rect(x, nt, nw + 2f, NameH), name, NameFont, Look.BarInk, true);
                if (!string.IsNullOrEmpty(note)) Draw.TextFit(new Rect(x + nw + LineGap * 3f, nt, Mathf.Max(0f, room - nw - LineGap * 3f), NameH), note, SubFont, Section.SubInk);
                Draw.TextFit(new Rect(x, nt + NameH + LineGap, room, SubH), sub, SubFont, Section.SubInk);
            }
            return closed;
        }
    }

    /// <summary>
    /// CSCommonUX.Gantt — 시간 띠 표(시안 3.0 올림픽 'Games' 그날 경기): 머리 눈금 줄 28(시각 12 Pal.Dim) · 줄(경기장) 64 · 왼쪽 이름 칸 148 ~ 240(이름 12.5 굵게 18 · 보조 12 16 · 오른쪽 12),
    /// 눈금 선 1(Section.WellFill) · 줄 위 선 1, 지금 선 2(Pal.Red) + 시각 알약 44 × 20(12 굵게). 블록: 위아래 8 · 좌우 2 · 반지름 14 · 안쪽 왼쪽 10 / 오른쪽 8 · 글 12 두 줄(14 · 사이 2, 아래 줄 0.78),
    /// 예정 = Look.PillNeutral · 경기 중 = Section.Plate + 테두리 1.5(Pal.Dim) + 빨간 점 7 · 종료 = Section.WellFill(글 Pal.Dim) · 메달 결정전 = Pal.MedalGold(끝나면 옅은 금), 흐림 0.3.
    /// 범례 줄 20(견본 14 × 10 · 반지름 4 · 견본과 글 6 · 묶음 사이 16).
    /// </summary>
    public static class Gantt
    {
        public const int FirstHour = 9, LastHour = 22;
        public const float AxisH = 28f, RowH = 64f, LabelW = 148f, LabelMaxW = 240f, LabelPadR = 12f, NameFont = 12.5f, NameH = 18f, SubFont = 12f, SubH = 16f;
        public const float NowLine = 2f, NowW = 44f, NowH = 20f, NowFont = 12f, GridLine = 1f;
        public const float BlockInsetY = 8f, BlockInsetX = 2f, BlockRadius = 14f, BlockPadL = 10f, BlockPadR = 8f, BlockFont = 12f, BlockLineH = 14f, BlockLineGap = 2f;
        public const float LiveDot = 7f, LiveDotGap = 5f, LiveRing = 1.5f, DimAlpha = 0.3f, TimeAlpha = 0.78f;
        public const float LegendH = 20f, SwatchW = 14f, SwatchH = 10f, SwatchRadius = 4f, LegendGap = 6f, LegendGroupGap = 16f;

        public enum State { Next, Live, Done }

        static readonly Color MedalDoneLight = Draw.Hex("#F0DFA6"), MedalInkLight = Draw.Hex("#2F2200"), MedalDoneInkLight = Draw.Hex("#5C4300");

        public static Color Grid => Section.WellFill;
        /// <summary>메달 결정전 면 · 글(진행 전 · 중 / 끝남) — 히어로 '메달 결정전' 알약도 같은 색.</summary>
        public static Color MedalFill => Pal.MedalGold;
        public static Color MedalInk => MedalInkLight;
        public static Color MedalDoneFill => MedalDoneLight;
        public static Color MedalDoneInk => MedalDoneInkLight;

        public static Color Fill(State st, bool medal) => medal ? (st == State.Done ? MedalDoneFill : MedalFill) : st == State.Done ? Section.WellFill : st == State.Live ? Section.Plate : Look.PillNeutral;

        public static Color Ink(State st, bool medal) => medal ? (st == State.Done ? MedalDoneInk : MedalInk) : st == State.Done ? Pal.Dim : st == State.Live ? Pal.Txt : Pal.Light;

        /// <summary>시각 → x(area = 눈금 칸, FirstHour ~ LastHour).</summary>
        public static float X(Rect area, float hour) => area.x + (Mathf.Clamp(hour, FirstHour, LastHour) - FirstHour) * area.width / (LastHour - FirstHour);

        /// <summary>블록 한 칸(r = 시각 칸 · 줄 높이 전체, 여백은 여기서 뺌): 두 줄 글(제목 · 아래 줄 흐리게), 줄임 없이 맞춤. 누르면 true(흐린 칸은 안 눌림).</summary>
        public static bool Block(Rect r, string title, string sub, State st, bool medal, bool dim, bool bold)
        {
            var b = new Rect(r.x + BlockInsetX, r.y + BlockInsetY, Mathf.Max(0f, r.width - BlockInsetX * 2f), Mathf.Max(0f, r.height - BlockInsetY * 2f));
            if (Draw.Repaint)
            {
                float a = dim ? DimAlpha : 1f;
                Color ink = Draw.Alpha(Ink(st, medal), a);
                Draw.Round(b, Draw.Alpha(Fill(st, medal), a), BlockRadius);
                if (st == State.Live && !medal) Draw.RoundOutline(b, Draw.Alpha(Pal.Dim, a), BlockRadius, LiveRing);
                float x = b.x + BlockPadL, right = b.xMax - BlockPadR, top = Mathf.Round(b.center.y - (BlockLineH * 2f + BlockLineGap) * 0.5f);
                if (st == State.Live)
                {
                    Draw.Dot(new Vector2(x + LiveDot * 0.5f, top + BlockLineH * 0.5f), LiveDot * 0.5f, Draw.Alpha(Pal.Red, a));
                    x += LiveDot + LiveDotGap;
                }
                Draw.TextFit(new Rect(x, top, Mathf.Max(0f, right - x), BlockLineH), title, BlockFont, ink, bold);
                Draw.TextFit(new Rect(b.x + BlockPadL, top + BlockLineH + BlockLineGap, Mathf.Max(0f, right - b.x - BlockPadL), BlockLineH), sub, BlockFont, Draw.Alpha(ink, TimeAlpha));
            }
            return !dim && Draw.Button(b);
        }

        /// <summary>지금 시각 알약(가운데 cx · cy): Pal.Red 면 · 흰 글.</summary>
        public static void Now(float cx, float cy, string text)
        {
            if (!Draw.Repaint) return;
            var p = new Rect(Mathf.Round(cx - NowW * 0.5f), Mathf.Round(cy - NowH * 0.5f), NowW, NowH);
            Draw.Round(p, Pal.Red, NowH * 0.5f);
            Draw.TextPill(p, text, NowFont, Draw.On(Pal.Red), true);
        }

        /// <summary>범례 묶음 하나(견본 면 또는 경기 중 점 + 글 12 Pal.Sub). 쓴 폭을 돌려줌.</summary>
        public static float Swatch(float x, float cy, Color fill, bool dot, string label)
        {
            float lw = Mathf.Ceil(Draw.TextWidth(label, SubFont)), sw = dot ? LiveDot : SwatchW;
            if (Draw.Repaint)
            {
                if (dot) Draw.Dot(new Vector2(x + LiveDot * 0.5f, cy), LiveDot * 0.5f, fill);
                else Draw.Round(new Rect(x, Mathf.Round(cy - SwatchH * 0.5f), SwatchW, SwatchH), fill, SwatchRadius);
                Draw.Text(new Rect(x + sw + LegendGap, Mathf.Round(cy - SubH * 0.5f), lw + 2f, SubH), label, SubFont, Pal.Sub);
            }
            return sw + LegendGap + lw;
        }
    }

    /// <summary>
    /// CSCommonUX.Bracket — 대진표(시안 GameDetailBracket): 열 머리 28(12 굵게 Pal.Sub) · 경기 칸 = 팀 줄 32 × 2 + 상태 줄 16, 칸 사이 16 · 열 사이 32.
    /// 칸 = Look.Pill 면 · 반지름 12 · 안쪽 10, 팀 줄 = 코드(Section.Code) · 이름 13 · 점수 14(오른쪽), 이긴 팀 굵게 · 우리 소속 Look.PastelInk,
    /// 이 경기 = Look.PastelStrong 테두리 2. 연결선 1.5(Section.PlateLine, 꺾은 선).
    /// </summary>
    public static class Bracket
    {
        public const float HeadH = 28f, TeamH = 32f, StateH = 16f, BoxGap = 16f, ColGap = 32f, Radius = 12f, PadX = 10f, NameFont = 13f, ScoreFont = 14f, StateFont = 12f, Ring = 2f, Link = 1.5f;

        public static float BoxH => TeamH * 2f + StateH;

        /// <summary>경기 칸 하나: win = 이긴 쪽(0 = 위 · 1 = 아래 · -1 = 아직), ours = 우리 소속 줄(0 · 1 · -1).</summary>
        public static void Box(Rect r, string codeA, string nameA, string scoreA, string codeB, string nameB, string scoreB, int win, int ours, string state, bool current)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, Look.Pill, Radius);
            if (current) Draw.RoundOutline(r, Look.PastelStrong, Radius, Ring);
            Team(new Rect(r.x + PadX, r.y, r.width - PadX * 2f, TeamH), codeA, nameA, scoreA, win == 0, ours == 0);
            Team(new Rect(r.x + PadX, r.y + TeamH, r.width - PadX * 2f, TeamH), codeB, nameB, scoreB, win == 1, ours == 1);
            Draw.TextFit(new Rect(r.x + PadX, r.y + TeamH * 2f, r.width - PadX * 2f, StateH), state, StateFont, Pal.Sub);
        }

        static void Team(Rect r, string code, string name, string score, bool won, bool ours)
        {
            float cy = r.center.y, x = r.x + Section.Code(r.x, cy, code, ours) + Section.RowGap * 0.5f;
            float sw = string.IsNullOrEmpty(score) ? 0f : Mathf.Ceil(Draw.TextWidth(score, ScoreFont, true));
            var ink = ours ? Look.PastelInk : Pal.Txt;
            if (sw > 0f) Draw.Text(new Rect(r.xMax - sw - 2f, r.y, sw + 2f, r.height), score, ScoreFont, won ? ink : Pal.Sub, won, TextAnchor.MiddleRight);
            Draw.TextFit(new Rect(x, r.y, Mathf.Max(0f, r.xMax - sw - Section.RowGap * 0.5f - x), r.height), name, NameFont, ink, won || ours);
        }

        /// <summary>앞 칸 오른쪽 가운데 → 뒤 칸 왼쪽 가운데 꺾은 선.</summary>
        public static void Connect(Vector2 from, Vector2 to)
        {
            if (!Draw.Repaint) return;
            float mx = Mathf.Round((from.x + to.x) * 0.5f);
            Draw.Line(from, new Vector2(mx, from.y), Link, Section.PlateLine);
            Draw.Line(new Vector2(mx, from.y), new Vector2(mx, to.y), Link, Section.PlateLine);
            Draw.Line(new Vector2(mx, to.y), to, Link, Section.PlateLine);
        }
    }

    /// <summary>
    /// CSCommonUX.Scoreboard — 두 팀 점수판(시안 GameDetailTeam 히어로 '대한민국 2 : 1 멕시코'): 높이 80, 가운데 점수 40 굵게(48) + 아래 상태 12(16) Look.PillSub,
    /// 양쪽 = 이름 16 굵게(우리 소속 Look.PastelInk) · 코드(Section.Code, 점수 쪽), 점수와 코드 사이 24.
    /// </summary>
    public static class Scoreboard
    {
        public const float H = 80f, ScoreFont = 40f, ScoreH = 48f, NoteFont = 12f, NoteH = 16f, NameFont = 16f, Gap = 24f;

        public static void In(Rect r, string codeA, string nameA, string codeB, string nameB, string score, string note, bool oursA, bool oursB)
        {
            if (!Draw.Repaint) return;
            float sw = Mathf.Max(Mathf.Ceil(Draw.TextWidth(score ?? "", ScoreFont, true)), Mathf.Ceil(Draw.TextWidth(note ?? "", NoteFont)));
            float top = Mathf.Round(r.center.y - (ScoreH + NoteH) * 0.5f), cx = r.center.x, cy = r.center.y;
            Draw.Text(new Rect(cx - sw * 0.5f - 1f, top, sw + 2f, ScoreH), score ?? "", ScoreFont, Pal.Txt, true, TextAnchor.MiddleCenter);
            Draw.Text(new Rect(cx - sw * 0.5f - 1f, top + ScoreH, sw + 2f, NoteH), note ?? "", NoteFont, Look.PillSub, false, TextAnchor.MiddleCenter);
            float ax = cx - sw * 0.5f - Gap - Section.CodeW;
            Section.Code(ax, cy, codeA, oursA);
            Draw.TextFit(new Rect(r.x, r.y, Mathf.Max(0f, ax - Section.RowGap - r.x), r.height), nameA, NameFont, oursA ? Look.PastelInk : Pal.Txt, true, TextAnchor.MiddleRight);
            float bx = cx + sw * 0.5f + Gap;
            Section.Code(bx, cy, codeB, oursB);
            float nx = bx + Section.CodeW + Section.RowGap;
            Draw.TextFit(new Rect(nx, r.y, Mathf.Max(0f, r.xMax - nx), r.height), nameB, NameFont, oursB ? Look.PastelInk : Pal.Txt, true);
        }
    }
}
