namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX 3.6 탭 배치 토큰: 창 몸 내용 폭 1140(= 창 1180 − 몸 안쪽 20 × 2) 안 열 폭 · 칸 폭 · 그림 크기 · 줄 높이.
    /// 열 사이는 GroupGap 16. 이름은 쓰는 보드 · 자리(FIA 캔버스 R5*) — 같은 값이라도 자리가 다르면 이름을 따로 둠.
    /// </summary>
    internal static partial class CSCommonDesign35
    {
        /// <summary>창 몸 내용 폭 1140.</summary>
        public const float ContentW = 1140f;

        // ---- 두 열(사이 16)
        /// <summary>대시보드 · 서킷 · 시설 · 도시 빈 자리: 넓은 752 · 좁은 372.</summary>
        public const float ColWide = 752f, ColNarrow = 372f;
        /// <summary>레이스 · 팬 · 팀 · 커리어: 왼쪽 748 · 오른쪽 376.</summary>
        public const float ColMain = 748f, ColSide = 376f;
        /// <summary>진행 중 대시보드 간격 차트 740 · 실시간 순위 384.</summary>
        public const float ColChart = 740f, ColChartSide = 384f;
        /// <summary>설정 · 일정: 왼쪽 280 · 오른쪽 844. 설정 값 · 목록 카드 반 폭 414 · 설정 2열 칸 394.</summary>
        public const float ColNav = 280f, ColDetail = 844f, ColDetailHalf = 414f, SettingColW = 394f;
        /// <summary>시설 왼쪽 열 안 두 카드 368 · 368.</summary>
        public const float ColHalf = 368f;
        /// <summary>길잡이 아래 줄: 빈 자리 724 · 지난 레이스 400.</summary>
        public const float ColGuideSlot = 724f, ColGuideLast = 400f;

        // ---- 세 열(사이 16)
        /// <summary>챔피언십 548 · 280 · 280(진행 중 · 길잡이 · 시설 빈 자리).</summary>
        public const float ColStandings = 548f, ColThird = 280f;
        /// <summary>NASCAR 진행 중: 챔피언십 452 · 스테이지 결과 320 · 오른쪽 336.</summary>
        public const float ColNascarStand = 452f, ColNascarStage = 320f, ColNascarSide = 336f;
        /// <summary>도시: 팬 존 460 · 관람 300 · 2km 안 시설 348.</summary>
        public const float ColFanZone = 460f, ColVolunteer = 300f, ColNearby = 348f;

        // ---- 칸 폭
        /// <summary>히어로 오른쪽 칸: 기본 200 · 다음 주간(R5Main) 340 · 뉴스 히어로 300.</summary>
        public const float HeroSideW = 200f, HeroSideWideW = 340f, NewsHeroSideW = 300f;
        /// <summary>서킷 고르기 상태 칸 240 · 도시 안쪽 칸 220 · 포디움 칸 112 · 폼 라벨 · H2H 값 96.</summary>
        public const float PickStatusW = 240f, CityCellW = 220f, PodiumW = 112f, PairValueW = 96f;

        // ---- 그림
        /// <summary>간격 차트(대시보드 · 레이스) 700 × 236 · 레이스 탭 그림 708 × 236.</summary>
        public const float GapChartW = 700f, GapChartH = 236f, RaceChartW = 708f;
        /// <summary>순위 변화(팀) 1100 × 208 · WEC 운전 시간 1100 × 164.</summary>
        public const float WideChartW = 1100f, RankChartH = 208f, DriveChartH = 164f;
        /// <summary>서킷 그림: 히어로 588 × 368 · 시설 704 × 408 · 레이스 트랙 카드 336 × 196 · 디렉터 708 × 410 · 고르기 mini 329 × 120 · 줄 mini 80 × 48.</summary>
        public const float MapHeroW = 588f, MapHeroH = 368f, MapFacilityW = 704f, MapFacilityH = 408f, MapCardW = 336f, MapCardH = 196f,
            MapDirectorW = 708f, MapDirectorH = 410f, MapMiniW = 329f, MapMiniH = 120f, MapThumbW = 80f, MapThumbH = 48f;
        /// <summary>시설 '매력을 주는 것' 설명 그림 328 × 112.</summary>
        public const float ExplainW = 328f, ExplainH = 112f;

        // ---- 줄 높이(배치)
        /// <summary>트랙 줄 64 · 노선 줄 88 · 빈 자리 줄 190 · 레이스 히어로 156 · 빈 상태 가로 카드 192.</summary>
        public const float RowTrack = 64f, RowRoute = 88f, RowSlotBand = 190f, RaceHeroH = 156f, EmptyRowH = 192f;

        // ---- 3.7 표 · 목록 열 폭(쓰임으로 이름 — 같은 쓰임은 탭이 달라도 하나). 칸 사이는 TableColGap 12
        /// <summary>순위 숫자 칸 28(목록 줄 앞 — 챔피언십 줄 · 인기 줄 · 명단).</summary>
        public const float ColRank = 28f;
        /// <summary>숫자 열: 작은 32(더할 점수 · 피트 횟수 · 점유율 비율) · 좁은 44(진행 중 점수) · 보통 48(차이 · 점수 · 우승 · 포디움 · 스틴트 · 차 번호).</summary>
        public const float ColNumXs = 32f, ColNumS = 44f, ColNum = 48f;
        /// <summary>순위 변화 화살표 열 36.</summary>
        public const float ColMove = 36f;
        /// <summary>짧은 값 열 40(표 순위 · 시각 · 타이어 · 랩 · 연료 — 두세 글자).</summary>
        public const float ColShort = 40f;
        /// <summary>차 번호 + 클래스 열 56(WEC) · 스테이지 열 64 · 간격 열 72 · 랩 시간 열 80.</summary>
        public const float ColCarWide = 56f, ColStage = 64f, ColGap = 72f, ColLapTime = 80f;
        /// <summary>약어 · 짧은 이름 칸 72(리저브 약어 · 이적 모달 이름).</summary>
        public const float ColCode = 72f;
        /// <summary>이름표 열 88(통계 줄 이름 · 페널티 팀 이름).</summary>
        public const float ColLabel = 88f;
        /// <summary>이름 열: 짧은 80 · 좁은 112(번호 칸이 붙는 순위 줄 · 그레이드 이름표) · 보통 120 · 넓은 128 · 긴 152(라디오 누구 · 시즌 보기 서킷 이름).</summary>
        public const float ColNameS = 80f, ColNameM = 112f, ColName = 120f, ColNameL = 128f, ColNameXl = 152f;
        /// <summary>시각 칸 56(일정 주 · 일 보기 왼쪽).</summary>
        public const float ColGutter = 56f;

        // ---- 3.7 칸 · 그림 크기
        /// <summary>목록 줄 오른쪽 칸 120(트랙 줄 '고른 트랙' · 작은 버튼) · 카드 안 오른쪽 칸 240(챔피언십 드라이버 줄).</summary>
        public const float RowSideW = 120f, CardSideW = 240f;
        /// <summary>차트 넓은 범례 칸 120(간격 차트 이름표).</summary>
        public const float ChartLegendWideW = 120f;
        /// <summary>뉴스 히어로 높이 296 · 팩트 띠 첫 칸(우승) 236.</summary>
        public const float NewsHeroH = 296f, FactLeadW = 236f;
        /// <summary>포디움 띠 폭 420 · 칸 높이 1위 160 · 2위 132 · 3위 116.</summary>
        public const float PodiumRowW = 420f, PodiumFirstH = 160f, PodiumSecondH = 132f, PodiumThirdH = 116f;
        /// <summary>시즌 보기 라운드 점 지름 20.</summary>
        public const float RoundDot = 20f;
    }
}
