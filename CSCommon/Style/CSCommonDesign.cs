namespace CSCommon
{
    /// <summary>
    /// 세 모드(기상청 · 주차장 · F1)가 같이 쓰는 디자인 시스템 치수.
    /// 이름은 디자인 시스템 토큰(space-* · radius-* · bar-* · fab-*)과 같고, 색은 Pal 에 있음.
    /// 규격을 바꾸려면 여기 한 곳만 고침.
    /// </summary>
    internal static class CSCommonDesign
    {
        /// <summary>디자인 규격 이름 · 판(공개 저장소 CSCommonUX 와 같음).</summary>
        public const string Ux = "CSCommon UX 1.1";

        public const float BarTop = 14f;
        public const float BarHeight = 68f;
        public const float BtnHeight = 32f;
        /// <summary>툴바 도구 버튼(아이콘 위 · 이름 아래) 높이 · 최소 너비 · 이름 양옆 여백 · 이름 글자 크기 · 모서리 · 버튼 사이.</summary>
        public const float LabelBtnHeight = 52f;
        // 서킷 디자인 에디터 새 틀: 위 판(묶음 탭 · 아이콘 단추) · 아래 도구 독 · 안내 한 줄
        public const float TopBarHeight = 56f;
        public const float TabHeight = 36f;
        public const float TabPadX = 14f;
        public const float IconBtnSize = 38f;
        public const float DockToolsHeight = 56f;
        public const float DockToolHeight = 40f;
        public const float DockBottomGap = 16f;
        public const float HintHeight = 34f;
        // 독 옵션 칸(시안 EditorDocks): 왼쪽 이름 줄들 · 오른쪽 세로선 뒤 일괄 열
        public const float DockPad = 14f;
        public const float DockLabelW = 56f;
        public const float DockRowH = 44f;
        public const float DockRowGap = 6f;
        public const float DockSideW = 220f;
        public const float DockColGap = 16f;
        public const float DockSideBtnH = 40f;
        // 공통 단추(CSCommonButton · 시안 '단추 새 방향' G): 톤 바탕 · 뜻 색 아이콘 · 테두리 없음
        public const float ActionHeight = 40f;
        public const float ActionFont = 14.5f;
        public const float ActionIcon = 18f;
        public const float ActionGap = 8f;
        public const float ActionPadL = 12f;
        public const float ActionPadR = 15f;
        public const float ActionRadius = 7f;
        public const float ActionHeightSmall = 32f;
        public const float ActionFontSmall = 12.5f;
        public const float ActionIconSmall = 15f;
        public const float LabelBtnMinW = 48f;
        public const float LabelBtnPadX = 6f;
        public const float LabelBtnFont = FontMin;
        public const float IconBtnRadius = 10f;
        public const float IconBtnGap = 2f;
        public const float LogoWidth = 72f;
        public const float LogoHeight = 18f;
        public const float BarPad = 12f;
        public const float BtnPad = 10f;
        public const float Gap = 4f;
        public const float Icon = 14f;
        public const float IconGap = 5f;
        public const float SepWidth = 11f;
        public const float PillHeight = 22f;
        /// <summary>CSCommon.Switch 알약 크기(F1 줄 · 설정 패널 · 기상청 설정이 같은 크기).</summary>
        public const float SwitchW = 46f, SwitchH = 26f;
        public const float FabSize = 40f;
        public const float FabMargin = 8f;

        /// <summary>지도 마커 화면 크기(px, 줌과 상관없이 같음) S · M · L · 어두운 테두리 두께 · 링 두께.</summary>
        public const float MarkS = 10f, MarkM = 14f, MarkL = 22f, MarkHalo = 2f, MarkRing = 2.5f;
        /// <summary>도구를 쓰는 동안 그 도구와 상관없는 시설 표시의 불투명도.</summary>
        public const float MarkDim = 0.35f;

        /// <summary>카드(CSCommon.Card, 시안 A 선 카드): 모서리 · 안쪽 여백 · 제목 줄 높이 · 제목→설명 · 머리→내용 간격.</summary>
        public const float CardRadius = 12f, CardPad = 16f, CardTitleH = 28f, CardDescGap = 2f, CardHeadGap = 8f;

        /// <summary>
        /// Flatten 섹션(GLDS 2.1 → CSCommon, 카드 대신): 좌우 · 위 · 아래 여백 · 제목 줄 높이 · 제목 글 · 제목 → 내용 · 섹션 사이 띠 두께.
        /// 섹션은 Win 바탕 · 모서리 0 · 테두리 없음, 섹션 사이 · 두 열 사이는 Pal.Band 띠.
        /// </summary>
        public const float SectionPadX = 20f, SectionTop = 22f, SectionBottom = 20f, SectionTitleH = 28f, SectionTitleFont = 17f, SectionHeadGap = 14f, BandH = 10f;
        /// <summary>창 · 패널(F1 창 · 설정 패널): 머리 높이 · 바깥 여백(창 끝 → 카드) · 카드 사이 간격.</summary>
        public const float PanelHeadH = 54f, PanelPad = 16f, PanelGap = 14f;
        /// <summary>떠 있는 판(메뉴 · 점수 판 · 줄 판) 안쪽 여백.</summary>
        public const float FloatPad = 14f;
        /// <summary>카드 안: 두 칸 사이 · 타일 격자 간격 · 줄 위아래 여백.</summary>
        public const float CardColGap = 16f, InnerGap = 10f, CardRowPad = 12f;
        // GLDS 2.1 에서 옮겨 옴(아티팩트 'CSCommon 컴포넌트' 입력 · 표시 보드): 움직임 · 목록 · 토스트 · 글 칸 단위
        /// <summary>단추를 누르면 이 배로 줄어듦.</summary>
        public const float PressScale = 0.97f;
        /// <summary>펼친 목록(CSCommon.Select): 줄 높이 · 위아래 여백 · 왼쪽 표시(○ · □) 크기 · 펼칠 때 시작 배율 · 걸리는 초.</summary>
        public const float MenuRowH = 40f, MenuPad = 6f, MenuMark = 18f, MenuOpenScale = 0.92f, MenuOpenSeconds = 0.18f;
        /// <summary>토스트: 높이 · 상태 원 · 원 안 그림 · 왼쪽 · 오른쪽 여백 · 요소 사이 · 최대 폭 · 올라오는 거리 · 들어오는 초 · 사라지는 초.</summary>
        public const float ToastHeight = 60f, ToastCircle = 36f, ToastIcon = 18f, ToastPadL = 12f, ToastPadR = 16f, ToastGap = 12f, ToastMaxW = 480f, ToastRise = 8f, ToastInSeconds = 0.26f, ToastOutSeconds = 0.18f;
        /// <summary>토스트 자리: 화면 아래 여백 · 떠 있는 것끼리 간격 · 보일 초(되돌리기 같은 단추가 있으면 더 길게).</summary>
        public const float ToastSafeGap = 16f, FloatingGap = 8f, ToastLife = 2.2f, ToastLifeAction = 5f;
        /// <summary>칩(CSCommon.Chip, GLDS 칩 D): 높이 · 좌우 · 글 · ▾ ✕ 크기 · 글 → 아이콘 · 누르면 줄어드는 배.</summary>
        public const float ChipHeight = 32f, ChipPadX = 14f, ChipFont = 13f, ChipIcon = 14f, ChipIconGap = 4f, ChipPressScale = 0.95f;
        /// <summary>비중 줄(CSCommon.ShareRow): 위아래 여백 · 높이(여백 8 + 글 20 + 6 + 막대 6 + 여백 8).</summary>
        public const float ShareRowPad = 8f, ShareRowH = 48f;
        /// <summary>서브라벨이 있는 고르기 칸 높이 · 펼친 목록 묶음 머리 높이.</summary>
        public const float SegmentTwoH = 52f, MenuHeadH = 28f;
        /// <summary>CSCommon 부품 글자 하한(방송 그래픽 · 지도 표시 밖) — 이보다 작은 글은 쓰지 않음.</summary>
        public const float FontMin = 12f;

        // GLDS 2.1 목록 · 지표 · 글(가이드 '가독성 최소 글자' · '지표' · '목록 · 비중 줄')
        /// <summary>글: 본문 · 줄 제목 14 · 값 15 굵게 · 부제 · 안내 13(보조 · 라벨은 FontMin 12). 줄 높이(한 줄 글 칸) 본문 · 값 20 · 보조 16.</summary>
        public const float FontBody = 14f, FontValue = 15f, FontDesc = 13f, LineBody = 20f, LineSub = 16f;
        /// <summary>목록 줄: 위아래 12 · 요소 사이 12 · 한 줄 높이(12 + 20 + 12). 섹션 끝 → 띠 보이는 간격 = SectionBottom − 마지막 줄 여백.</summary>
        public const float RowPadY = 12f, RowGap = 12f, RowH = 44f;
        /// <summary>지표 칸(CSCommon.Metric): 값 15 굵게 줄 20 · 라벨 12 줄 16 · 칸 사이 행 간격 16 · 열 사이 16.</summary>
        public const float MetricValueH = 20f, MetricLabelH = 16f, MetricRowGap = 16f, MetricColGap = 16f;
        /// <summary>게임 정보 창 옆에 붙는 떠 있는 판 폭.</summary>
        public const float SidePanelW = 340f;
        /// <summary>글 칸 앞뒤 단위와 값 사이 · 지우기 ✕ 크기.</summary>
        public const float FieldAffixGap = 4f, FieldClear = 16f;
        /// <summary>입력 칸 · 분절 홈 모서리.</summary>
        public const float RadiusField = 9f;
        /// <summary>입력 칸 · 고르기 칸 · 펼친 목록 테두리 두께(상하좌우 같음, 1080p 에서 2px) · 초점 · 오류 · 열림 두께.</summary>
        public const float FieldLineW = 1.5f, FieldFocusW = 2f;

        public const float RadiusWindow = 14f;
        public const float RadiusTile = 8f;
        public const float RadiusTool = 7f;
        public const float RadiusTip = 6f;
        public const float RadiusChip = 5f;
    }
}
