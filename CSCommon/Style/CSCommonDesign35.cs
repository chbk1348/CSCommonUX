using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX 3.5 치수 · 글 · 움직임 토큰(FIA 캔버스 '5.0 리뉴얼' R5Tok* 보드). 3.0 값(CSCommonDesign)은 그대로 두고 이름을 새로 둠.
    /// 4px 격자 — 예외는 1px 선 · 2px 맞춤 · 가운데 맞춤 홀수(점수 막대 6 / 3, 범례 막대 3 / 1.5)뿐. 다크 전용.
    /// </summary>
    internal static partial class CSCommonDesign35
    {
        // ---- 간격 단계(이 밖 값은 쓰지 않음)
        public const float Step4 = 4f, Step8 = 8f, Step12 = 12f, Step16 = 16f, Step20 = 20f, Step24 = 24f, Step32 = 32f, Step40 = 40f, Step48 = 48f, Step56 = 56f, Step64 = 64f;

        // ---- 간격 역할
        /// <summary>컨트롤 안쪽 여백 최소 · 최대.</summary>
        public const float CtrlPadMin = 4f, CtrlPadMax = 12f;
        /// <summary>아이콘과 글 사이 — 칩 · 버튼 8, 목록 행 12(사용자 결정 2026-11-27).</summary>
        public const float IconTextGap = 8f, RowIconGap = 12f;
        /// <summary>라벨 ↔ 컨트롤 8 · 제목 ↔ 본문 12 · 묶음 · 카드 사이 16.</summary>
        public const float LabelGap = 8f, HeadGap = 12f, GroupGap = 16f;
        /// <summary>카드 안쪽 20 · 히어로 · 모달 24 · 안쪽 칸 위아래 12 · 좌우 16 · 창 몸 20.</summary>
        public const float CardPad = 20f, HeroPad = 24f, ModalPad = 24f, InnerPadY = 12f, InnerPadX = 16f, WindowBodyPad = 20f;

        // ---- 모서리 4단계(스위치만 알약). 중첩: 안쪽 = 바깥 − 여백, 최소 4, 여백이 더 크면 8
        public const float RadiusTag = 4f, RadiusCtrl = 8f, RadiusCard = 16f, RadiusWindow = 20f;

        // ---- 줄 높이(같은 종류는 하나, 줄 사이 0 · 구분선 없음)
        public const float RowHead = 28f, RowTiming = 36f, RowList = 44f, RowSetting = 56f, RowSettingDesc = 64f;
        /// <summary>카드 머리 32 · 모달 머리 56 · 모달 바닥 64.</summary>
        public const float CardHeadH = 32f, ModalHeadH = 56f, ModalFootH = 64f;
        /// <summary>버튼 줄: 40 · 작은 버튼 32 · 버튼 사이 8. 카드 꼬리 위 16, 안쪽 칸 꼬리 위 12.</summary>
        public const float BtnH = 40f, BtnSmallH = 32f, BtnGap = 8f, CardFootGap = 16f, InnerFootGap = 12f;

        // ---- 창 틀(R5Window · R5PartWindow) — 창 1180 × 920 은 CSCommonDesign.WindowW · WindowH, 모서리 RadiusWindow
        /// <summary>창 머리 64 · 좌우 16 · 요소 사이 12. 몸 스크롤 안쪽 위 · 좌우 = WindowBodyPad, 아래 = 툴바 안전 영역(Toolbar.SafeH 88 · SubToolBar.SafeH 148).</summary>
        public const float WindowHeadH = 64f, WindowHeadPadX = 16f, WindowHeadGap = 12f;
        /// <summary>창 닫기 정사각 40(r8 · 투명 면, 아이콘 IconM 18).</summary>
        public const float CloseBtn = 40f;
        /// <summary>창 머리 시리즈 전환 칸: 판 40 · 안쪽 4(r8) · 칸 32 · 좌우 12(r4) · 칸 사이 4.</summary>
        public const float SeriesSegH = 40f, SeriesSegPad = 4f, SeriesCellH = 32f, SeriesCellPadX = 12f, SeriesCellGap = 4f;
        /// <summary>칩 · 상태 칩(Chip35): 높이 32 · 좌우 12 · r8 · 점 8 → 글 IconTextGap 8.</summary>
        public const float ChipH = 32f, ChipPadX = 12f;

        // ---- 점 · 막대 · 알약
        /// <summary>상태 점 지름 8 · 툴바 점 둘레 2(알약 바탕색).</summary>
        public const float Dot = 8f, DotRing = 2f;
        /// <summary>툴바 칸 구석 점 자리: 메인 칸 위 6 · 오른쪽 6, 서브 칸 위 4 · 오른쪽 6.</summary>
        public const float DotMainTop = 6f, DotMainRight = 6f, DotSubTop = 4f, DotSubRight = 6f;
        /// <summary>서브 칸 안 읽은 수 알약: 높이 20 · 좌우 8 · 글 뒤 8(반지름 = 높이 / 2).</summary>
        public const float CountH = 20f, CountPadX = 8f, CountGap = 8f;
        /// <summary>줄 앞 팀 색 막대 4 × 16 · 반지름 2.</summary>
        public const float TeamBarW = 4f, TeamBarH = 16f, TeamBarR = 2f;
        /// <summary>단계 · 섹터 막대: 높이 8 · 반지름 4 · 칸 사이 4 · 라벨은 아래 8.</summary>
        public const float TrackH = 8f, TrackR = 4f, TrackGap = 4f, TrackLabelGap = 8f;
        /// <summary>점수 막대 높이 6 · 반지름 3(캔버스 값 그대로).</summary>
        public const float ScoreBarH = 6f, ScoreBarR = 3f;
        /// <summary>점수 막대 '이대로 끝나면 더해질 몫' = 강조색 56%.</summary>
        public const float AccentAddAlpha = 0.56f;

        // ---- KPI 칸(4열 · 사이 16, 높이는 내용 = Kpi.H)
        public const float KpiPadY = 16f, KpiPadX = 20f, KpiGap = 4f, KpiRadius = 16f, KpiColGap = 16f;
        public const int KpiCols = 4;

        // ---- 빈 · 오류 · 잠김 상태: 원 40 안 아이콘 18(잠김 16) · 원 → 제목 12 · 제목 → 설명 4 · 설명 → 버튼 16
        public const float EmptyCircle = 40f, EmptyIconGap = 12f, EmptyDescGap = 4f, EmptyBtnGap = 16f;

        // ---- 차트(R5DashLive '선두와의 간격' 본보기)
        /// <summary>그림 영역 = 칸에서 왼쪽 축 숫자 44 · 위 16 · 아래 x 축 글 28 · 오른쪽 범례 88 을 뺀 곳.</summary>
        public const float ChartAxisW = 44f, ChartTop = 16f, ChartAxisH = 28f, ChartLegendW = 88f;
        /// <summary>선 굵기 2 · 강조 끝 원 반지름 4 · 테두리 2 · 세로 막대 끝 반지름 4(바닥 0).</summary>
        public const float ChartLine = 2f, ChartEndR = 4f, ChartEndRing = 2f, BarEndR = 4f;
        /// <summary>y 축 글 오른쪽 끝 = 영역 왼쪽 − 8 · x 축 글 가운데 = 영역 바닥 + 18 · 사건 띠 이름 가운데 = 영역 위 − 8.</summary>
        public const float ChartYLabelGap = 8f, ChartXLabelY = 18f, ChartBandLabelY = 8f;
        /// <summary>'지금' 세로 점선: 굵기 1 · 대시 2 · 틈 4.</summary>
        public const float ChartCursorW = 1f, ChartDash = 2f, ChartDashGap = 4f;
        /// <summary>선 끝 이름표: 팀 색 막대 3 × 12 · 반지름 1.5(사용자 결정 2026-11-27) · 막대 x = 영역 오른쪽 + 8 · 이름 x = + 16.</summary>
        public const float LegendBarW = 3f, LegendBarH = 12f, LegendBarR = 1.5f, LegendBarX = 8f, LegendNameX = 16f;

        // ---- 아이콘(상자 24 · 획 2 하나 — 화면 획 = 크기 ÷ 12)
        public const float IconXs = 12f, IconS = 16f, IconM = 18f, IconL = 24f, IconBox = 24f, IconLine = 2f, IconGap = 8f;

        // ---- 글(여섯 크기 밖은 쓰지 않음). 굵기: 400 기본, 600 은 숫자(Draw.Num) · 창 제목만, 700 없음
        public const float FontCaption = 12f, LineCaption = 16f;
        public const float FontBody = 14f, LineBody = 20f;
        public const float FontTitle = 16f, LineTitle = 24f;
        public const float FontHeading = 20f, LineHeading = 28f;
        /// <summary>카드 · KPI 숫자 28 / 줄 36(사용자 결정 2026-11-27).</summary>
        public const float FontNumber = 28f, LineNumber = 36f;
        public const float FontHero = 48f, LineHero = 56f;

        /// <summary>카드 숫자(.fig · .n28) 28 / 줄 32 — KPI 칸만 LineNumber 36(3.6, 탭 규격).</summary>
        public const float LineFigure = 32f;
        /// <summary>작은 숫자(.n20) 20 / 28 · 600(Draw.Num) — 피트 칸 · 팬층 · 통계 · 시설 값. 글(400)은 FontHeading.</summary>
        public const float FontStat = 20f, LineStat = 28f;

        // ---- 움직임(곡선 하나 = Ease)
        /// <summary>호버 · 누름 · 토글 · 면 색 0.15초 · 메뉴 · 툴팁 0.2초 · 진입 0.3초(올라옴 8).</summary>
        public const float DurFeedback = 0.15f, DurMenu = 0.2f, DurEnter = 0.3f, EnterRise = 8f;
        /// <summary>누름 0.97배(판 · 글만, 아이콘은 크기 그대로).</summary>
        public const float PressScale = 0.97f;
        /// <summary>'세션 진행 중' 점만: 1.2초마다 투명도 1 ↔ 0.4 오감.</summary>
        public const float LiveBlink = 1.2f, LiveAlphaMin = 0.4f;

        /// <summary>3.5 움직임 곡선 cubic-bezier(0.22, 1, 0.36, 1) — 끝에서 느려짐. t 0 ~ 1 → 0 ~ 1.</summary>
        public static float Ease(float t)
        {
            t = Mathf.Clamp01(t);
            // x(u) = t 를 뉴턴법으로 풂(x1 · x2 > 0 이라 x(u) 는 늘 오름 — 기울기 0 은 끝점 근처뿐)
            float u = t;
            for (int i = 0; i < 8; i++)
            {
                float dx = Bezier(u, 0.22f, 0.36f) - t;
                if (Mathf.Abs(dx) < 1e-5f) break;
                float slope = BezierSlope(u, 0.22f, 0.36f);
                if (slope < 1e-5f) break;
                u = Mathf.Clamp01(u - dx / slope);
            }
            return Bezier(u, 1f, 1f);
        }

        static float Bezier(float u, float a, float b)
        {
            float v = 1f - u;
            return 3f * v * v * u * a + 3f * v * u * u * b + u * u * u;
        }

        static float BezierSlope(float u, float a, float b)
        {
            float v = 1f - u;
            return 3f * v * v * a + 6f * v * u * (b - a) + 3f * u * u * (1f - b);
        }
    }
}
