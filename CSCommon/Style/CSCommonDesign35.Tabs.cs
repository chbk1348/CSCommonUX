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
    }
}
