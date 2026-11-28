namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX 3.6 탭 부품 치수 · 배치 토큰(FIA 캔버스 대시보드 · 서킷 · 레이스 · 팬 · 팀 · 설정 보드, Docs 8b 탭 규격 문서).
    /// 4px 격자 — 예외는 캔버스 값 그대로 둔 것만(쌓인 막대 조각 사이 2 · 타이어 테 1.5 · 랩 띠 2 · 팀 범례 3 / 1.5).
    /// 배치(열 폭 · 그림 크기)도 여기 둠 — 모드 코드에는 치수 숫자를 쓰지 않음(AGENTS 5절).
    /// </summary>
    internal static partial class CSCommonDesign35
    {
        // ---- 버튼 3.5(Button35 · IconButton35): 보통 40 · 좌우 16, 작은 32 · 좌우 12, 아이콘만 = 높이 정사각
        public const float BtnPadX = 16f, BtnSmallPadX = 12f;
        /// <summary>초점 링: 굵기 2 · 버튼 바깥 2(강조색).</summary>
        public const float FocusRingW = 2f, FocusRingGap = 2f;
        /// <summary>글 링크: 글 14 → 4 → 꺾쇠 Right 16.</summary>
        public const float LinkGap = 4f;

        // ---- 고르기(Segmented35): 판 40 · 안쪽 4 · 칸 32 · 좌우 12 · 최소 48 / 작은 판 32 · 칸 24 · 좌우 8 · 글 12. 칸 사이 4
        public const float SegH = 40f, SegSmallH = 32f, SegPad = 4f, SegCellGap = 4f, SegCellMinW = 48f, SegCellPadX = 12f, SegSmallCellPadX = 8f;
        /// <summary>등급 칸(Segmented35.Tiles): 높이 32 · r8 · 칸 사이 4.</summary>
        public const float TileH = 32f, TileGap = 4f;

        // ---- 스위치 · 체크 상자
        /// <summary>스위치 40 × 24 · 손잡이 16 · 안쪽 4(꺼짐 왼쪽 4 · 켜짐 왼쪽 20).</summary>
        public const float SwitchW = 40f, SwitchH = 24f, SwitchKnob = 16f, SwitchInset = 4f;
        /// <summary>체크 상자 18 · r4 · Done 16.</summary>
        public const float CheckBox = 18f;

        // ---- 폼 · 입력(Field35 · Select35 · Slider35 · Stepper35)
        /// <summary>폼 줄 56 · 라벨 96 → 12 → 칸. 값 칸 40 · 작은 입력 칸 32 · 좌우 12 · 끝 아이콘 16.</summary>
        public const float FormRowH = 56f, FormLabelW = 96f, FormLabelGap = 12f, FieldH = 40f, FieldSmallH = 32f, FieldPadX = 12f;
        /// <summary>고르기 칸 264 × 56 · 왼쪽 16 · 오른쪽 12 · 사이 12.</summary>
        public const float SelectW = 264f, SelectH = 56f, SelectPadL = 16f, SelectPadR = 12f, SelectGap = 12f;
        /// <summary>펼친 목록: 줄 40 · 판 안쪽 4 · 칸과 사이 4(캔버스 없음 — 3.5 컨트롤 값으로 맞춤).</summary>
        public const float MenuRowH = 40f, MenuPad = 4f, MenuGap = 4f;
        /// <summary>슬라이더 폭 240 · 높이 16 · 홈 4(r2) · 손잡이 16.</summary>
        public const float SliderW = 240f, SliderH = 16f, SliderTrackH = 4f, SliderKnob = 16f;
        /// <summary>스테퍼 버튼 32 × 2 · 사이 8 · 값 칸 48(오른쪽 맞춤, 왼쪽 따로).</summary>
        public const float StepperBtn = 32f, StepperGap = 8f, StepperValueW = 48f;

        // ---- 태그 · 칩
        /// <summary>태그(번호 · 약어 · 글): 높이 24 · 좌우 4(글 태그 8) · 최소 폭 32(명단 · 이적 40) · r4.</summary>
        public const float TagH = 24f, TagPadX = 4f, TagLabelPadX = 8f, TagMinW = 32f, TagMinWide = 40f;
        /// <summary>차 번호 칸 32 × 20 · r4 · 글 12 600.</summary>
        public const float CarTagW = 32f, CarTagH = 20f;
        /// <summary>시설 퍼센트 글 태그 폭 48.</summary>
        public const float TagPercentW = 48f;
        /// <summary>팀 색 면 투명도(WEC 차 번호 칸 = 팀 색 40%).</summary>
        public const float TeamFaceAlpha = 0.4f;
        /// <summary>고정 폭 칩(페널티 칩) 120.</summary>
        public const float ChipFixedW = 120f;
        /// <summary>타이어 표시: 원 20 · 테 1.5(캔버스 값).</summary>
        public const float TyreD = 20f, TyreRing = 1.5f;
        /// <summary>아바타 지름 20 · 24 · 28 · 32(글 12 · 32 만 14) · 봇 칸 32 r8 · 상태 점 오른쪽 −2 · 아래 −2 · 둘레 2.</summary>
        public const float AvatarXs = 20f, AvatarS = 24f, AvatarM = 28f, AvatarL = 32f, AvatarDotOut = 2f;

        // ---- 막대
        /// <summary>가는 막대(팬층 · 인기) 4 · r2. 가는 막대 8 은 TrackH.</summary>
        public const float ThinBarH = 4f;
        /// <summary>쌓인 점유율 · 몫 막대 12 · 바깥 r4 · 조각 사이 2 · 몫 막대 칸 160 · 범례 색 칸 12 × 8 r2.</summary>
        public const float StackBarH = 12f, StackGap = 2f, SplitBarW = 160f, SwatchW = 12f, SwatchH = 8f, SwatchR = 2f;
        /// <summary>가로 막대 차트: 막대 20 · 끝 r4 · 기준 점선 대시 4 · 틈 4 · 값 글 막대 끝 + 8 · 값 글 테 4.</summary>
        public const float HBarH = 20f, HBarDash = 4f, HBarDashGap = 4f, HBarValueGap = 8f, HaloPad = 4f;
        /// <summary>가로 막대 차트 축: 이름 오른쪽 끝 = 영역 왼쪽 − 12 · x 축 글 가운데 = 영역 바닥 + 16 · 기준선 이름 가운데 = 영역 위 − 12.</summary>
        public const float HBarNameGap = 12f, HBarAxisY = 16f, HBarRefLabelY = 12f;
        /// <summary>단계 끝 세로 점선(Chart35.StageLine): 대시 4 · 틈 4 · 이름은 영역 위 − 8.</summary>
        public const float StageDash = 4f, StageDashGap = 4f;
        /// <summary>세로 막대 묶음(관중 차트): 축 글 칸 48 · 축 글 ↔ 영역 8 · 막대 사이 12 · 막대 위 값 4 · 범례 표 8 r2 · 남은 라운드 높이 8.</summary>
        public const float BarAxisW = 48f, BarGap = 12f, BarValueGap = 4f, BarLegend = 8f, BarFutureH = 8f;

        // ---- 줄
        /// <summary>두 줄 메시지 줄(레이스 컨트롤) · 상태 줄(시설) 56.</summary>
        public const float RowMessage = 56f, RowStatus = 56f;
        /// <summary>값 줄 높이(캔버스 보드마다): 24(서킷 고르기 값) · 28(시설 조건 · 짧은 표) · 32(서킷 히어로, 아래 선) · 36(도시 · 표) · 44(목록).</summary>
        public const float RowValueXs = 24f, RowValueS = 28f, RowValueM = 32f;
        /// <summary>단계 줄(길잡이) 44 · 번호 칸 28 · 아이콘 16 · 사이 12. 상태 줄 오른쪽 칸 56.</summary>
        public const float StepNumW = 28f, StatusSideW = 56f;
        /// <summary>표: 칸 사이 12 · 줄 좌우 8 넓힘(.x8 — 선두 · 호버 면).</summary>
        public const float TableColGap = 12f, RowBleed = 8f;
        /// <summary>섹터 칸(이번 랩) 폭 72 · 칸 3.</summary>
        public const float SectorCellsW = 72f;
        /// <summary>화면 머리 줄(PageHead35) 56 · 제목 ↔ 칩 12 · 제목 → 캡션 4 · 오른쪽 항목 사이 16.</summary>
        public const float PageHeadH = 56f, PageHeadGap = 12f, PageHeadItemGap = 16f;
        /// <summary>목록 탐색 줄: 설정 분류 44(아이콘 18) · 채널 36(아이콘 16) · 좌우 12 · 사이 8.</summary>
        public const float NavRowH = 44f, NavRowSmallH = 36f, NavPadX = 12f;
        /// <summary>목록 묶음 이름: 높이 16 · 위 16 · 아래 8.</summary>
        public const float GroupLabelTop = 16f, GroupLabelBottom = 8f;

        // ---- 모달 3.5(Modal35): 머리 56 안 왼쪽 24 · 오른쪽 12(닫기 40) · 사이 12, 판 높이 400(일정 모달)
        public const float ModalCloseRight = 12f, ModalFixedH = 400f;
        /// <summary>모달 폭 560 · 넓게 720(3.0 Modal.Width · WideWidth 와 같은 값).</summary>
        public const float ModalW = 560f, ModalWideW = 720f;
        /// <summary>확인 줄(Notice35) 44 · 좌우 16.</summary>
        public const float NoticeH = 44f, NoticePadX = 16f;

        // ---- 토스트 3.5(Toast35): 52 · r16 · 왼쪽 8 · 오른쪽 16(버튼 10) · 사이 12 · 상태 칸 36 r8
        public const float ToastH = 52f, ToastPadL = 8f, ToastPadR = 16f, ToastPadRAction = 10f, ToastGap = 12f, ToastIconBox = 36f;
        /// <summary>툴팁 3.5: 높이 32 · 좌우 12 · 앵커 아래 8 · 0.4초 머물면.</summary>
        public const float TipH = 32f, TipPadX = 12f, TipGap = 8f, TipDelay = 0.4f;
        /// <summary>런처 원 40 · 진행 중 점 8(위 0 · 오른쪽 0) · 툴킷 켜짐 점 6(위 1 · 오른쪽 1).</summary>
        public const float LauncherD = 40f, LauncherDotOn = 6f, LauncherDotInset = 1f;

        // ---- 달력(Calendar35)
        /// <summary>작은 달력: 요일 줄 28 · 날 줄 32 · 숫자 상자 24 × 20 · 상자 → 점 2 · 점 4.</summary>
        public const float CalHeadH = 28f, CalMiniRowH = 32f, CalMiniBoxW = 24f, CalMiniBoxH = 20f, CalDotGap = 2f, CalDot = 4f;
        /// <summary>큰 달력: 주 줄 120 · 날 상자 24 · 안쪽 4 · 일정 칩 24 · 칩 사이 4 · 칩 좌우 띄움 2 · 칩 안 좌우 8.</summary>
        public const float CalWeekH = 120f, CalDayBox = 24f, CalPad = 4f, CalChipH = 24f, CalChipGap = 4f, CalChipInset = 2f, CalChipPadX = 8f;

        // ---- 채팅(Chat35)
        /// <summary>채팅 띠 688(창 몸 856 − 위 20 − 서브 안전 148) · 서버 칸 240 · 스레드 칸 260 · 칸 사이 선 양옆 16.</summary>
        public const float ChatBandH = 688f, ChatServerW = 240f, ChatSideW = 260f;
        /// <summary>날짜 줄 28 · 스레드 줄 36 · 지운 글 28 · 입력 중 20 · 메시지 들여쓰기 44 · 입력 줄 48 · 반응 태그 24 · 좌우 8.</summary>
        public const float ChatDateH = 28f, ChatThreadH = 36f, ChatDeletedH = 28f, ChatTypingH = 20f, ChatIndent = 44f, ChatInputH = 48f, ReactionH = 24f, ReactionPadX = 8f;
        /// <summary>서버 머리 56 · 서버 칸 40 · 채널 줄 들여쓰기(곁가지 · 참가자) 36.</summary>
        public const float ChatServerHeadH = 56f, ChatServerBox = 40f, ChatSubIndent = 36f;

        // ---- 레이더(F1 전용 그림은 모드 — 치수만 이름)
        /// <summary>재생 장 칸 높이 28 · 랩 띠 16(사이 2 · r2) · 구간 막대 16 · 지금 표시 2 · 강수 막대 축 칸 52 · 막대 폭 32 · 값 글 막대 위 6 · 예보 투명도 .5.</summary>
        public const float FrameCellH = 28f, LapStripH = 16f, LapStripGap = 2f, LapStripR = 2f, SpanBarH = 16f, NowMarkW = 2f, RainAxisW = 52f, RainBarW = 32f, RainValueGap = 6f, ForecastAlpha = 0.5f;
        /// <summary>맵 위 차 점: 반지름 약 5.2 · 테 약 1.7 · 이름 테 약 3.4 · 점 위 약 10(캔버스 viewBox 0.431 배 값).</summary>
        public const float MapCarR = 5.2f, MapCarRing = 1.7f, MapNameHalo = 3.4f, MapNameRise = 10f;

        // ---- 3.7 탭 토큰(8b ③ 탭 구현 뒤 모음 — Docs 8b 규격 '정본에 더할 것')
        /// <summary>1px 선 굵기(Hairline 구분선 · 테 · 눈금선).</summary>
        public const float HairW = 1f;
        /// <summary>강수 막대 그림 높이 140 · 막대 위 높이 100(4 mm/h × 25) · 섹터 줄 64 · 섹터 이름 칸 24.</summary>
        public const float RainChartH = 140f, RainPlotH = 100f, RowSector = 64f, SectorNameW = 24f;
        /// <summary>비 세기 4단계 투명도(Rain1 ~ 4 위, 약 → 강 .28 · .38 · .5 · .56 — 캔버스 값).</summary>
        public const float RainAlpha1 = 0.28f, RainAlpha2 = 0.38f, RainAlpha3 = 0.5f, RainAlpha4 = 0.56f;
        /// <summary>지도 위 강조 띠 투명도: 보통 .22(트랙 카드) · 옅게 .16(큰 지도).</summary>
        public const float MapBandAlpha = 0.22f, MapBandSoftAlpha = 0.16f;
        /// <summary>
        /// 서킷 그림(모드 CircuitMap 이 씀 — 캔버스 값, 격자 밖 홀수 그대로): 트랙 몸 18 · mini 6 · 가운데 선 5 · mini 2 · 출발선 3 · 코너 원 20 ·
        /// 피트 선 2 · 대시 6 · 틈 6 · 격자 가로 선 사이 80 · 세로 선 사이 100 · mini 판 r12.
        /// </summary>
        public const float MapTrackW = 18f, MapTrackMiniW = 6f, MapLineW = 5f, MapLineMiniW = 2f, MapStartW = 3f, MapCornerD = 20f,
            MapPitW = 2f, MapPitDash = 6f, MapPitGap = 6f, MapGridRow = 80f, MapGridCol = 100f, MapMiniR = 12f;
        /// <summary>매력 핀 44 × 22 · r11 · 그림 머리 이름 글 15(서킷 그림 안만 — 3.5 글 여섯 크기 밖 캔버스 값) · 범례 막대 14 × 4 · 막대 → 이름 6.</summary>
        public const float MapPinW = 44f, MapPinH = 22f, MapPinR = 11f, FontMapHead = 15f, MapLegendW = 14f, MapLegendH = 4f, MapLegendGap = 6f;
        /// <summary>순위 변화 차트: 눈금 글 칸 64(영역 왼쪽) · 위 12 · 선 끝 이름표 영역 오른쪽 + 16(겹치면 + 24).</summary>
        public const float BumpAxisW = 64f, BumpTop = 12f, BumpLegendX = 16f, BumpLegendXWide = 24f;
        /// <summary>차트 아래 캡션 줄 20(위 4 + 캡션 16 — '랩 뒤라 빠졌어요', 켜져도 높이 그대로).</summary>
        public const float ChartNoteH = 20f;
        /// <summary>기사 링크 줄(관련 뉴스) 64 · 참가자 줄(음성 채널) 32.</summary>
        public const float RowArticle = 64f, RowMember = 32f;
        /// <summary>설명 붙은 머리(PageHead35.Desc) 52 = 제목 28 + 4 + 설명 20.</summary>
        public const float PageHeadDescH = 52f;
        /// <summary>값 줄 이름 칸이 이보다 좁으면 조작을 작은 꼴로(고르기 칸 · 슬라이더 → 스테퍼) 240 · 목록 값 글이 줄 폭에서 차지할 몫 0.4.</summary>
        public const float RowLabelMinW = 240f, RowValueShare = 0.4f;
        /// <summary>토스트 비켜 가기: 겹친 칸 위 8(3.0 FloatingGap 과 같은 값).</summary>
        public const float ToastAvoidGap = 8f;

        // ---- 3.8 방송 카드(FIA 캔버스 R5BcLayout · R5BcBattle · R5BcPit · R5BcDelta — 게임 화면 1920 × 1080 논리 px)
        /// <summary>방송 카드 폭 400 · 화면 오른쪽 여백 24 · 화면 아래 여백 120(카드 아래 맞춤).</summary>
        public const float CastCardW = 400f, CastEdge = 24f, CastBottom = 120f;
        /// <summary>카드 높이: 배틀 252 · 피트 스톱 212. 카드 바꿈 이동 8(앞 카드 내려가며 나감 · 다음 카드 올라오며 들어옴, 각 DurEnter).</summary>
        public const float CastBattleH = 252f, CastPitH = 212f, CastSwapShift = 8f;
        /// <summary>배틀 카드: 두 차 줄 · 타이어 줄 양쪽 칸 120 · 번호 태그 폭 24 · 간격 추이 칸 64 · 그 안 라벨 칸 96.</summary>
        public const float CastSideW = 120f, CastTagW = 24f, CastSparkH = 64f, CastSparkLabelW = 96f;
        /// <summary>피트 스톱 카드 안쪽 칸: 정지 120 · 타이어 · 순위 112(사이 BtnGap 8).</summary>
        public const float CastStopW = 120f, CastCellW = 112f;
        /// <summary>퀄리 델타 바 320 × 76(r16 · 안쪽 위아래 12 · 좌우 16) · 랩 패널과 사이 8 · 델타 글 칸 80 · 막대 끝 값 ±0.5초.</summary>
        public const float DeltaBarW = 320f, DeltaBarH = 76f, DeltaGap = 8f, DeltaValueW = 80f, DeltaRange = 0.5f;
        /// <summary>순위 변화 칸(Move35): 아이콘 → 수 2.</summary>
        public const float MoveGap = 2f;

        /// <summary>
        /// 3.8 B5 결승 뒤 장면(FIA 캔버스 R5BcPost · PostWait · PostPodium · PostStand): 방송 연출 보드 1600 × 900 px 그대로(화면에는 k = min(W / 1600, H / 900) 배).
        /// 인트로 방송 룩(700 · 900 · 모서리 0)이라 3.5 글 여섯 크기 · 4px 격자 밖 — 그래서 격자 검사를 받는 바깥 토큰과 따로 둠.
        /// 띠 머리 · 띠 몸 높이 · 판 머리 · 줄 높이 50 · 간격 58 · P 칸 68 처럼 인트로(모드 Intro.D)에 이미 있는 값은 거기 것을 씀.
        /// </summary>
        public static class CastPost
        {
            /// <summary>아래 정보 띠(체커 · 결과 기다림) 몸 칸: 둘째 · 셋째 칸 x 560 · 860 · 세로 구분선 x 520 · 820(띠 왼쪽 기준).</summary>
            public const float Col2X = 560f, Col3X = 860f, Div1X = 520f, Div2X = 820f;
            /// <summary>체커기 40(칸 10) · 띠 왼쪽 22 · 몸 위 27 · 우승 팀 막대 6 × 30 → 14 → 이름 · 결과 기다림 글 24 · 기다린 시간 막대 높이 4.</summary>
            public const float FlagX = 22f, FlagY = 27f, FlagD = 40f, WinnerBarW = 6f, WinnerBarH = 30f, WinnerBarGap = 14f, WaitFont = 24f, ProgressH = 4f;
            /// <summary>건너뛰기 글: 오른쪽 44 · 위 40(띠 장면) / 128(전체 화면 판) · 글 16 / 20 · 투명도 .72.</summary>
            public const float SkipRight = 44f, SkipTopBand = 40f, SkipTopPlate = 128f, SkipFont = 16f, SkipH = 20f, SkipAlpha = 0.72f;
            /// <summary>챔피언십 변동 표: 줄 x 240 · 폭 1120 · 첫 줄 148 · 머리 줄 y 120 · 높이 20 · 글 14.</summary>
            public const float RowX = 240f, RowW = 1120f, RowTop = 148f, HeadY = 120f, HeadH = 20f, HeadFont = 14f;
            /// <summary>변화 칸 60(아이콘 16 · 획 2.5 → 2 → 수 18) · 이름 앞 18 · 팀 160 · 이번 라운드 96(글 22) · 점수 120(오른쪽 24 · 글 26).</summary>
            public const float ChangeW = 60f, ChangeIcon = 16f, ChangeStroke = 2.5f, ChangeGap = 2f, ChangeFont = 18f, NamePad = 18f, TeamW = 160f, RoundW = 96f, RoundFont = 22f, PtsW = 120f, PtsPadR = 24f, PtsFont = 26f;
            /// <summary>시상대: 칸 셋 왼쪽 236 · 폭 360 · 사이 24 · 높이 1위 360 · 2위 300 · 3위 260 · 바닥 760 · 위 팀 색 띠 6.</summary>
            public const float PodiumX = 236f, PodiumW = 360f, PodiumGap = 24f, PodiumFirstH = 360f, PodiumSecondH = 300f, PodiumThirdH = 260f, PodiumBottom = 760f, PodiumStripeH = 6f;
            /// <summary>시상대 칸 안: 위 22 · 좌우 26 · P 칸 → 18 → 차이 30 / 38 · 이름 위 18 · 32 / 40 · 팀 위 4 · 20 / 28.</summary>
            public const float PodiumPadTop = 22f, PodiumPadX = 26f, PodiumChipGap = 18f, GapFont = 30f, GapH = 38f, NameTop = 18f, NameFont = 32f, NameH = 40f, TeamTop = 4f, TeamFont = 20f, TeamH = 28f;
        }
    }
}
