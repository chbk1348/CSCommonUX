using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>3.5 부품 순수 함수 자가 검사(게임 밖 하니스 · 모드 진단에서 부름). check(통과, 이름).</summary>
    internal static class Check35
    {
        public static void SelfCheck(Action<bool, string> check)
        {
            static bool Near(float a, float b) => Mathf.Abs(a - b) < 0.01f;
            static bool NearV(Vector2 a, Vector2 b) => Near(a.x, b.x) && Near(a.y, b.y);
            int steps = CSCommonDesign.CurveSteps;

            // 곡선: 모든 점을 지남 · Catmull-Rom 조절점(/6) · 끝 밖 점은 끝점 · 조절점 y 는 영역 안
            var pts = new List<Vector2> { new(0f, 0f), new(10f, 10f), new(20f, 0f), new(30f, 10f) };
            var line = new List<Vector2>();
            Draw.SmoothInto(line, pts, -100f, 100f);
            check(line.Count == 1 + 3 * steps && NearV(line[steps], pts[1]) && NearV(line[2 * steps], pts[2]) && NearV(line[line.Count - 1], pts[3]), "smooth passes points");
            Draw.CatmullControls(pts, 1, -100f, 100f, out var c1, out var c2);
            check(NearV(c1, new Vector2(10f + 20f / 6f, 10f)) && NearV(c2, new Vector2(20f - 20f / 6f, 0f)), "smooth catmull controls");
            Draw.CatmullControls(pts, 0, -100f, 100f, out c1, out _);
            check(NearV(c1, new Vector2(10f / 6f, 10f / 6f)), "smooth end uses itself");
            var spike = new List<Vector2> { new(0f, 0f), new(10f, 10f), new(20f, 10f), new(30f, 0f) };
            Draw.CatmullControls(spike, 1, 0f, 10f, out c1, out c2);
            check(c1.y <= 10f && c2.y <= 10f && Near(c1.y, 10f), "smooth controls clamped");

            // 차트 축 · 막대
            var plot = new Rect(0f, 10f, 100f, 200f);
            check(Near(Chart35.Y(plot, 0f, 0f, 12f), 10f) && Near(Chart35.Y(plot, 6f, 0f, 12f), 110f) && Near(Chart35.Y(plot, 99f, 0f, 12f), 210f), "chart y map");
            check(Near(Chart35.TickY(plot, 4, 0), 10f) && Near(Chart35.TickY(plot, 4, 3), 210f), "chart ticks span plot");
            var bar = Chart35.Bar(plot, 10f, 8f, 50f, 100f);
            check(Near(bar.yMax, plot.yMax) && Near(bar.height, 100f), "chart bar from floor");
            check(Near(Chart35.Bar(plot, 0f, 8f, 150f, 100f).height, 200f) && Near(Chart35.Bar(plot, 0f, 8f, -5f, 100f).height, 0f), "chart bar clamped");
            check(Chart35.Gray(0) == Pal.ChartGray1 && Chart35.Gray(3) == Pal.ChartGray4 && Chart35.Gray(9) == Pal.ChartGray4, "chart gray steps");
            var area = Chart35.Plot(new Rect(0f, 0f, 700f, 236f));
            check(Near(area.x, 44f) && Near(area.y, 16f) && Near(area.yMax, 208f) && Near(area.xMax, 612f), "chart plot area");

            // 숫자 자리 폭: 1 이 좁은 글꼴에서도 같은 꼴 글의 끝이 같음
            var w = new float[] { 10f, 6f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f };
            float cell = Draw.DigitCell(w, out bool same);
            check(Near(cell, 10f) && !same, "num cell widest digit");
            float Total(string t)
            {
                float x = 0f;
                foreach (char ch in t) x += Draw.Advance(ch, cell, ch >= '0' && ch <= '9' ? w[ch - '0'] : 3f);
                return x;
            }
            check(Near(Total("1:11.111"), Total("0:00.000")) && Near(Total("+1.1"), Total("+9.0")), "num tabular advance");
            Draw.DigitCell(new float[] { 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f, 7f }, out same);
            check(same, "num cell same font");

            // 점수 · 단계 막대 · KPI
            ScoreBar.Split(30f, 20f, 100f, 200f, out float nowW, out float addW);
            check(Near(nowW, 60f) && Near(addW, 40f), "score split");
            ScoreBar.Split(90f, 50f, 100f, 200f, out nowW, out addW);
            check(Near(nowW + addW, 200f), "score clipped at end");
            ScoreBar.Split(10f, -5f, 0f, 200f, out nowW, out addW);
            check(nowW == 0f && addW == 0f, "score empty max");
            var track = new Rect(0f, 0f, 168f, 8f);
            check(Near(StageTrack.Cell(track, 3, 2).xMax, 168f) && Near(StageTrack.Cell(track, 3, 1).x - StageTrack.Cell(track, 3, 0).xMax, 4f), "track cells gap 4");
            var row = new Rect(0f, 0f, 1000f, 108f);
            check(Near(Kpi.Col(row, 3).xMax, 1000f) && Near(Kpi.Col(row, 1).x - Kpi.Col(row, 0).xMax, 16f) && Near(Kpi.H, 108f), "kpi columns");

            // 상태 점 · 숫자 알약
            check(Near(StatusDot.LiveAlpha(0f), 1f) && Near(StatusDot.LiveAlpha(1.2f), 0.4f) && Near(StatusDot.LiveAlpha(2.4f), 1f) && StatusDot.LiveAlpha(0.6f) < 1f && StatusDot.LiveAlpha(0.6f) > 0.4f, "dot live blink");
            check(NearV(StatusDot.CornerCenter(new Rect(0f, 0f, 48f, 48f), false), new Vector2(38f, 10f)) && NearV(StatusDot.CornerCenter(new Rect(0f, 0f, 64f, 32f), true), new Vector2(54f, 8f)), "dot corner inset");
            check(StatusDot.CountText(5) == "5" && StatusDot.CountText(99) == "99" && StatusDot.CountText(100) == "99+" && StatusDot.CountText(0) == null, "count text");

            // 곡선 Ease · 아이콘 단계 · 빈 상태 · 판
            bool rising = true;
            for (int i = 1; i <= 20; i++) rising &= CSCommonDesign35.Ease(i / 20f) >= CSCommonDesign35.Ease((i - 1) / 20f);
            check(Near(CSCommonDesign35.Ease(0f), 0f) && Near(CSCommonDesign35.Ease(1f), 1f) && CSCommonDesign35.Ease(0.5f) > 0.8f && rising, "ease out curve");
            check(Icons.Stage(12f) == 12f && Icons.Stage(16f) == 16f && Icons.Stage(20f) == 18f && Icons.Stage(24f) == 24f && Icons.Stage(13.9f) == 12f, "icon stage");
            var seen = new HashSet<string>();
            int consts = 0;
            foreach (var f in typeof(Icons).GetFields(BindingFlags.Public | BindingFlags.Static))
                if (f.IsLiteral && f.FieldType == typeof(string)) { consts++; seen.Add((string)f.GetValue(null)); }
            check(consts == 74 && seen.Count == 74, "icons 74 unique");
            check(Near(EmptyState.Height(false, false), 72f) && Near(EmptyState.Height(true, true), 152f), "empty height");
            check(Near(Surface.Height(Surface.Kind.Card, 100f, true, true), 240f) && Near(Surface.Inset(new Rect(0f, 0f, 560f, 300f), Surface.Kind.Modal).height, 300f), "surface height");
            check(Surface.Raise(Surface.Kind.Card) == Pal.GroundRaised && Surface.Raise(Surface.Kind.Hero) == Pal.GroundCard, "surface raise");

            // 창 틀: 몸 = 머리 64 아래 ~ 창 바닥 · 스크롤 안쪽 아래 88 / 서브 148
            var body = WindowFrame35.Body(new Rect(10f, 20f, CSCommonDesign.WindowW, CSCommonDesign.WindowH));
            check(Near(body.y, 84f) && Near(body.yMax, 940f) && Near(body.x, 10f) && Near(body.width, 1180f)
                && Near(WindowFrame35.BodyBottom(false), 88f) && Near(WindowFrame35.BodyBottom(true), 148f), "window frame body");
            // 호버 면 = 놓인 면보다 한 단: 닫기(머리 GroundPage 위) Card · 시리즈 칸(판 GroundCard 위) Raised
            check(WindowFrame35.CloseHover == Pal.GroundCard && WindowFrame35.CellHover == Pal.GroundRaised, "window hover faces");

            // ---- 3.6 탭 부품
            // 단계 막대 칸 폭 비율(7 : 8 : 15, 폭 308 · 사이 4) · 비율 없으면 같은 폭
            var trk = new Rect(0f, 0f, 308f, 8f);
            var wts = new float[] { 7f, 8f, 15f };
            var k0 = StageTrack.Cell(trk, 3, wts, 0); var k1 = StageTrack.Cell(trk, 3, wts, 1); var k2 = StageTrack.Cell(trk, 3, wts, 2);
            check(Near(k0.width, 70f) && Near(k1.x, 74f) && Near(k1.width, 80f) && Near(k2.x - k1.xMax, 4f) && Near(k2.xMax, 308f), "track weighted cells");
            check(Near(StageTrack.Cell(trk, 3, null, 2).x, StageTrack.Cell(trk, 3, 2).x), "track weights null = even");
            check(StageTrack.SectorColor(SectorMark.Pending, Color.red) == Pal.Hairline && StageTrack.SectorColor(SectorMark.Done, Color.red) == Pal.ChartGray4
                && StageTrack.SectorColor(SectorMark.Personal, Color.red) == Pal.SemGood && StageTrack.SectorColor(SectorMark.Overall, Color.red) == Color.red, "sector colors");

            // 쌓인 막대 조각: 사이 2 · 마지막 채운 조각은 끝까지 · 0 조각 뒤엔 틈 없음 · max 로 나눔
            var bar100 = new Rect(0f, 0f, 100f, 12f);
            var sv = new float[] { 50f, 30f, 20f };
            check(Near(StackBar35.Piece(bar100, sv, 3, 0f, 0).width, 48f) && Near(StackBar35.Piece(bar100, sv, 3, 0f, 1).x, 50f)
                && Near(StackBar35.Piece(bar100, sv, 3, 0f, 2).xMax, 100f), "stack pieces gap 2");
            check(Near(StackBar35.Piece(bar100, new float[] { 50f, 0f, 0f }, 3, 0f, 0).width, 100f), "stack last piece no gap");
            check(Near(StackBar35.Piece(bar100, new float[] { 50f, 50f }, 2, 200f, 1).xMax, 50f) && Near(StackBar35.Piece(bar100, sv, 3, 0f, 7).width, 0f), "stack max and range");

            // 가로 막대: 0분 = 112 · 150분 = 1012 에서 45분 = 382(캔버스 WEC 기준선 자리)
            var hp = new Rect(112f, 24f, 900f, 112f);
            check(Near(HBar35.Bar(hp, 40f, 45f, 150f).xMax, 382f) && Near(HBar35.Bar(hp, 40f, 45f, 150f).height, 20f) && Near(HBar35.Bar(hp, 40f, 999f, 150f).xMax, 1012f), "hbar length");

            // 표 칸: 고정 폭 + 남는 칸 하나 · 사이 12
            var cols = new Rect[4];
            Table35.Cols(new Rect(0f, 0f, 700f, 36f), new float[] { 40f, -1f, 80f, 72f }, cols);
            check(Near(cols[1].x, 52f) && Near(cols[1].width, 472f) && Near(cols[3].xMax, 700f) && Near(cols[2].x - cols[1].xMax, 12f), "table columns");

            // 달력: 7열 · 요일 줄 28 · 주 120 · 칩 자리(날 글 아래) · 칸에 칩 3개
            var grid = new Rect(0f, 0f, 840f, 28f + 120f * 6f);
            var day = Calendar35.Cell(grid, 6, 8);
            check(Near(day.x, 120f) && Near(day.y, 148f) && Near(day.width, 120f) && Near(day.height, 120f), "calendar cell");
            check(Near(Calendar35.ChipRect(day, 0).y, 180f) && Near(Calendar35.ChipRect(day, 1).y, 208f) && Near(Calendar35.ChipRect(day, 0).x, 122f) && Calendar35.ChipsFit(day) == 3, "calendar chips");

            // 모달: 높이 = 56 + 몸 + 64 · 바닥 버튼 줄 = 바닥 64 가운데 · 좌우 24
            var modal = new Rect(0f, 0f, CSCommonDesign35.ModalW, CSCommonDesign35.ModalFixedH);
            var foot = Modal35.Foot(modal);
            check(Near(Modal35.Height(280f), CSCommonDesign35.ModalFixedH) && Near(foot.y, 348f) && Near(foot.x, 24f) && Near(foot.width, 512f), "modal foot");
            check(Near(Modal35.Place(new Rect(0f, 0f, 1180f, 920f), 400f).x, 310f) && Near(Modal35.Place(new Rect(0f, 0f, 1180f, 920f), 400f).y, 260f), "modal place");

            // 막대 묶음 영역(축 48 + 8 · 위 16 · 아래 28) · 막대 자리 사이 12
            var bp = Chart35.BarPlot(new Rect(0f, 0f, 700f, 236f));
            check(Near(bp.x, 56f) && Near(bp.y, 16f) && Near(bp.yMax, 208f) && Near(bp.xMax, 700f), "bar plot area");
            check(Near(Chart35.BarSlot(bp, 4, 3).xMax, bp.xMax) && Near(Chart35.BarSlot(bp, 4, 1).x - Chart35.BarSlot(bp, 4, 0).xMax, 12f), "bar slots");

            // 스위치: 꺼짐 손잡이 왼쪽 4 · 켜짐 왼쪽 20 · 높이 안 가운데(24 = 4 + 16 + 4)
            check(Near(CSCommonDesign35.SwitchW - CSCommonDesign35.SwitchInset - CSCommonDesign35.SwitchKnob, 20f)
                && Near(CSCommonDesign35.SwitchH, CSCommonDesign35.SwitchKnob + CSCommonDesign35.SwitchInset * 2f), "switch knob travel");
            // 채팅 띠 = 창 몸 보이는 높이 856 − 위 20 − 서브 안전 148 · 세 칸 + 선 둘(양옆 16)이 내용 폭 1140
            check(Near(CSCommonDesign35.ChatBandH, CSCommonDesign.WindowH - CSCommonDesign35.WindowHeadH - CSCommonDesign35.WindowBodyPad - WindowFrame35.BodyBottom(true))
                && Near(CSCommonDesign35.ChatServerW + CSCommonDesign35.ChatSideW + 574f + (16f * 2f + 1f) * 2f, CSCommonDesign35.ContentW), "chat band fits body");
            // 배치: 두 열 · 세 열이 내용 폭 1140 에 맞음
            const float G = CSCommonDesign35.GroupGap, W = CSCommonDesign35.ContentW;
            check(Near(CSCommonDesign35.ColWide + G + CSCommonDesign35.ColNarrow, W) && Near(CSCommonDesign35.ColMain + G + CSCommonDesign35.ColSide, W)
                && Near(CSCommonDesign35.ColChart + G + CSCommonDesign35.ColChartSide, W) && Near(CSCommonDesign35.ColNav + G + CSCommonDesign35.ColDetail, W)
                && Near(CSCommonDesign35.ColStandings + CSCommonDesign35.ColThird * 2f + G * 2f, W)
                && Near(CSCommonDesign35.ColNascarStand + CSCommonDesign35.ColNascarStage + CSCommonDesign35.ColNascarSide + G * 2f, W)
                && Near(CSCommonDesign35.ColFanZone + CSCommonDesign35.ColVolunteer + CSCommonDesign35.ColNearby + G * 2f, W)
                && Near(CSCommonDesign35.ColGuideSlot + G + CSCommonDesign35.ColGuideLast, W) && Near(CSCommonDesign35.ColHalf * 2f + G, CSCommonDesign35.ColWide)
                && Near(CSCommonDesign35.ColDetailHalf * 2f + G, CSCommonDesign35.ColDetail), "layout columns fill 1140");

            // ---- 3.7 탭 토큰 · 꼴
            // 토스트 비켜 가기: 겹치면 그 칸 위 8 · 올린 자리에서 다른 칸과 또 겹치면 그 위 · 안 겹치면 그대로 · 빈 칸 무시
            var dock = new Rect(400f, 900f, 400f, 60f);
            var hint = new Rect(500f, 820f, 200f, 60f);
            check(Near(Toast35.AvoidY(450f, 932f, 300f, 52f, dock, default), 840f) && Near(Toast35.AvoidY(450f, 932f, 300f, 52f, dock, hint), 760f)
                && Near(Toast35.AvoidY(0f, 932f, 300f, 52f, dock, hint), 932f) && Near(Toast35.AvoidY(450f, 932f, 300f, 52f, new Rect(400f, 900f, 0f, 60f), default), 932f), "toast avoid");
            // 빈 상태 작은 버튼 32 · 관리봇 한 줄 52 · 설명 머리 52
            check(Near(EmptyState.Height(true, true, true), 144f) && Near(EmptyState.Height(true, true), 152f), "empty small button");
            check(Near(Chat35.BotLineH, 52f) && Near(CSCommonDesign35.PageHeadDescH, CSCommonDesign35.LineHeading + CSCommonDesign35.KpiGap + CSCommonDesign35.LineBody), "bot line · desc head 52");
            // 열 폭 단계가 오름(이름 · 숫자) · 비 세기 투명도 오름 · 지도 띠 옅게 < 보통
            check(CSCommonDesign35.ColNameS < CSCommonDesign35.ColLabel && CSCommonDesign35.ColLabel < CSCommonDesign35.ColNameM && CSCommonDesign35.ColNameM < CSCommonDesign35.ColName
                && CSCommonDesign35.ColName < CSCommonDesign35.ColNameL && CSCommonDesign35.ColNameL < CSCommonDesign35.ColNameXl
                && CSCommonDesign35.ColRank < CSCommonDesign35.ColNumXs && CSCommonDesign35.ColNumXs < CSCommonDesign35.ColShort && CSCommonDesign35.ColShort < CSCommonDesign35.ColNumS
                && CSCommonDesign35.ColNumS < CSCommonDesign35.ColNum, "column ladders rise");
            check(CSCommonDesign35.RainAlpha1 < CSCommonDesign35.RainAlpha2 && CSCommonDesign35.RainAlpha2 < CSCommonDesign35.RainAlpha3 && CSCommonDesign35.RainAlpha3 < CSCommonDesign35.RainAlpha4
                && CSCommonDesign35.RainAlpha4 < 1f && CSCommonDesign35.MapBandSoftAlpha < CSCommonDesign35.MapBandAlpha, "alpha steps rise");
            // 포디움: 1위 > 2위 > 3위 · 강수 막대 위 100 이 그림 140 안 · 런처 둘레 = 바탕 GroundDeep
            check(CSCommonDesign35.PodiumFirstH > CSCommonDesign35.PodiumSecondH && CSCommonDesign35.PodiumSecondH > CSCommonDesign35.PodiumThirdH
                && CSCommonDesign35.RainPlotH < CSCommonDesign35.RainChartH && Pal.LauncherRing == Pal.GroundDeep, "podium · rain chart · launcher ring");

            // ---- 3.8 방송 카드
            // 배틀 252 = 안쪽 20 × 2 + 머리 32 + 12 + 두 차 줄 44 + 12 + 간격 추이 64 + 12 + 타이어 줄 36 · 추이 그림 폭 220 = 400 − 40 − 32 − 라벨 96 − 12
            const float CP = CSCommonDesign35.CardPad, CH = CSCommonDesign35.CardHeadH, HG = CSCommonDesign35.HeadGap, CW = CSCommonDesign35.CastCardW;
            check(Near(CP * 2f + CH + HG * 3f + CSCommonDesign35.RowList + CSCommonDesign35.CastSparkH + CSCommonDesign35.RowTiming, CSCommonDesign35.CastBattleH)
                && Near(CW - CP * 2f - CSCommonDesign35.InnerPadX * 2f - CSCommonDesign35.CastSparkLabelW - HG, 220f)
                && CSCommonDesign35.CastSideW * 2f < CW - CP * 2f, "cast battle card 252");
            // 피트 212 = 20 × 2 + 32 + 12 + 드라이버 줄 44 + 12 + 안쪽 칸 72(12 + 라벨 16 + 4 + 값 28 + 12) · 칸 셋 120 + 112 × 2 + 8 × 2 = 360
            check(Near(CP * 2f + CH + HG * 2f + CSCommonDesign35.RowList + CSCommonDesign35.InnerPadY * 2f + CSCommonDesign35.LineCaption + CSCommonDesign35.KpiGap + CSCommonDesign35.LineStat, CSCommonDesign35.CastPitH)
                && Near(CSCommonDesign35.CastStopW + CSCommonDesign35.CastCellW * 2f + CSCommonDesign35.BtnGap * 2f, CW - CP * 2f), "cast pit card 212");
            // 델타 바 76 = 12 + 줄 28 + 8 + 막대 줄 16 + 12 · 막대 204 = 320 − 16 × 2 − 섹터 칸 72 − 12
            check(Near(CSCommonDesign35.InnerPadY * 2f + CSCommonDesign35.LineHeading + CSCommonDesign35.LabelGap + CSCommonDesign35.SpanBarH, CSCommonDesign35.DeltaBarH)
                && Near(CSCommonDesign35.DeltaBarW - CSCommonDesign35.InnerPadX * 2f - CSCommonDesign35.SectorCellsW - HG, 204f), "delta bar 320 × 76");
            // ± 막대: −0.25 / 0.5 = 왼쪽 반의 반 · +1 은 오른쪽 끝까지 · 0 · 범위 0 은 폭 0
            var sTrack = new Rect(0f, 4f, 204f, 8f);
            var sNeg = Meter35.SignedFill(sTrack, -0.25f, CSCommonDesign35.DeltaRange);
            var sPos = Meter35.SignedFill(sTrack, 1f, CSCommonDesign35.DeltaRange);
            check(Near(sNeg.x, 51f) && Near(sNeg.xMax, 102f) && Near(sPos.x, 102f) && Near(sPos.xMax, 204f)
                && Near(Meter35.SignedFill(sTrack, 0f, 0.5f).width, 0f) && Near(Meter35.SignedFill(sTrack, 0.3f, 0f).width, 0f), "signed meter fill");
            // 순위 변화 칸: 오름 Up SemGood · 내림 Chevron SemBad · 그대로 Minus InkDim · 아이콘 12 + 2 + 두 자리 수가 칸 36 안
            check(Move35.Icon(2) == Icons.Up && Move35.Icon(-1) == Icons.Chevron && Move35.Icon(0) == Icons.Minus
                && Move35.Ink(1) == Pal.SemGood && Move35.Ink(-3) == Pal.SemBad && Move35.Ink(0) == Pal.InkDim
                && CSCommonDesign35.IconXs + CSCommonDesign35.MoveGap + CSCommonDesign35.FontCaption < CSCommonDesign35.ColMove, "move cell");
            // 포디움 칸: 2 · 1 · 3위 차례 · 폭 360 사이 24 · 바닥 760 맞춤(R5BcPostPodium)
            const float PX = CSCommonDesign35.CastPost.PodiumX, PB = CSCommonDesign35.CastPost.PodiumBottom, PW = CSCommonDesign35.CastPost.PodiumW, PG = CSCommonDesign35.CastPost.PodiumGap;
            const float P1 = CSCommonDesign35.CastPost.PodiumFirstH, P2 = CSCommonDesign35.CastPost.PodiumSecondH, P3 = CSCommonDesign35.CastPost.PodiumThirdH;
            var pod1 = Podium35.Slot(PX, PB, 0, PW, PG, P1, P2, P3);
            var pod2 = Podium35.Slot(PX, PB, 1, PW, PG, P1, P2, P3);
            var pod3 = Podium35.Slot(PX, PB, 2, PW, PG, P1, P2, P3);
            check(Near(pod2.x, 236f) && Near(pod1.x, 620f) && Near(pod3.x, 1004f) && Near(pod1.y, 400f) && Near(pod2.y, 460f) && Near(pod3.yMax, 760f)
                && Podium35.Column(0) == 1 && Podium35.Column(1) == 0 && Podium35.Column(2) == 2, "podium slots");
            // 결승 뒤 장면(보드 1600 × 900): 시상대 셋 가운데 800 · 표 열(P 68 · 띠 6 포함) 합이 줄 폭 안 · 10줄(간격 58 · 줄 50)이 보드 안 · 머리 줄이 첫 줄 위 · 띠 칸 차례
            check(Near(pod2.x + (pod3.xMax - pod2.x) * 0.5f, 800f)
                && 68f + CSCommonDesign35.CastPost.ChangeW + 6f + CSCommonDesign35.CastPost.TeamW + CSCommonDesign35.CastPost.RoundW + CSCommonDesign35.CastPost.PtsW < CSCommonDesign35.CastPost.RowW
                && CSCommonDesign35.CastPost.RowTop + 58f * 9f + 50f <= 900f && CSCommonDesign35.CastPost.HeadY + CSCommonDesign35.CastPost.HeadH <= CSCommonDesign35.CastPost.RowTop
                && CSCommonDesign35.CastPost.Div1X < CSCommonDesign35.CastPost.Col2X && CSCommonDesign35.CastPost.Col2X < CSCommonDesign35.CastPost.Div2X
                && CSCommonDesign35.CastPost.Div2X < CSCommonDesign35.CastPost.Col3X && CSCommonDesign35.CastPost.Col3X < CSCommonDesign35.CastPost.RowW
                && CSCommonDesign35.CastPost.SkipTopBand < CSCommonDesign35.CastPost.SkipTopPlate, "post scene fits board");

            // 4px 격자: 2 이하(선 · 맞춤) · 시간 · 배율 · 글 크기 · 캔버스 값 그대로 둔 이름 말고는 모두 4 의 배수
            var offGrid = new HashSet<string> { "DotMainTop", "DotMainRight", "DotSubRight", "ScoreBarH", "ScoreBarR", "ChartXLabelY", "LegendBarW", "IconM", "CheckBox",
                "ToastPadRAction", "LauncherDotOn", "RainValueGap", "MapCarR", "MapNameHalo", "MapNameRise", "ColDetailHalf", "SettingColW", "MapDirectorH", "MapMiniW", "RowSlotBand",
                "MapTrackW", "MapTrackMiniW", "MapLineW", "MapStartW", "MapPitDash", "MapPitGap", "MapPinH", "MapPinR", "MapLegendW", "MapLegendGap" };
            string stray = null;
            foreach (var f in typeof(CSCommonDesign35).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (!f.IsLiteral || f.FieldType != typeof(float)) continue;
                string n = f.Name;
                float v = (float)f.GetValue(null);
                if (v <= 2f || offGrid.Contains(n) || n.StartsWith("Font") || n.Contains("Alpha") || n.Contains("Dur") || n.Contains("Scale") || n.Contains("Blink") || n.Contains("Delay")) continue;
                if (Mathf.Abs(v / 4f - Mathf.Round(v / 4f)) > 1e-4f) { stray = n; break; }
            }
            check(stray == null, "tokens on 4px grid" + (stray == null ? "" : " — " + stray));
        }
    }
}
