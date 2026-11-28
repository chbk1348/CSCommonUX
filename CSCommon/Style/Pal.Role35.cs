using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX 3.5 역할 색(FIA 캔버스 R5TokColor, 다크 전용). 테마(CSCommonTheme.Use)가 바꾸지 않음 — 3.0 이름(Txt · Sub · Dim · Card …)과 값이 달라 따로 둠.
    /// 값은 다른 Pal 필드를 읽지 않고 글로 적음(partial 파일 사이 정적 초기화 순서는 정해져 있지 않음).
    /// </summary>
    internal static partial class Pal
    {
        // ---- 바탕 4단계(그림자 없음 — 층은 면 단계와 1px 선으로만)
        /// <summary>창 몸 · 스크롤 영역.</summary>
        public static readonly Color GroundDeep = Draw.Hex("#0E1014");
        /// <summary>창 · 창 머리 · 서브 칸 숫자 알약 면.</summary>
        public static readonly Color GroundPage = Draw.Hex("#14171C");
        /// <summary>카드 · 모달 · KPI 칸.</summary>
        public static readonly Color GroundCard = Draw.Hex("#191C22");
        /// <summary>안쪽 칸 · 고른 칸 · 히어로 · 보조 버튼 · 칩.</summary>
        public static readonly Color GroundRaised = Draw.Hex("#20242B");

        // ---- 글 3역할(대비는 Card 면 위)
        /// <summary>제목 · 값 · 고른 글(14.7:1).</summary>
        public static readonly Color InkTxt = Draw.Hex("#ECEEF2");
        /// <summary>본문 보조 · 안 고른 글 · 줄 아이콘(6.7:1).</summary>
        public static readonly Color InkSub = Draw.Hex("#9AA3AF");
        /// <summary>캡션 · 단위 · 축 · 표 머리 · 꺼진 줄(5.0:1).</summary>
        public static readonly Color InkDim = Draw.Hex("#848C98");

        // ---- 선
        /// <summary>카드 · 모달 · 창 · 보조 버튼 테두리 1px, 묶음 사이 구분선 1px(#ECEEF2 8%).</summary>
        public static readonly Color Hairline = new(236f / 255f, 238f / 255f, 242f / 255f, 0.08f);
        /// <summary>켜짐 · 고른 칸 위쪽 1px 밝은 선(Hairline 과 같은 값).</summary>
        public static readonly Color SelectedTopLine = new(236f / 255f, 238f / 255f, 242f / 255f, 0.08f);

        // ---- 시리즈 강조(면 · 히어로 숫자 · 주 버튼 · 고른 칸, 한 화면 3곳 이하) · 그 위 글. 고르는 건 모드 — 부품은 인자로 받음
        public static readonly Color AccentF1 = Draw.Hex("#FFC9C4"), OnAccentF1 = Draw.Hex("#5C0F0B");
        public static readonly Color AccentNascar = Draw.Hex("#FFE49A"), OnAccentNascar = Draw.Hex("#4A3500");
        public static readonly Color AccentWec = Draw.Hex("#C6D8FF"), OnAccentWec = Draw.Hex("#14305C");

        // ---- 의미색(뜻이 있을 때만)
        /// <summary>좋음 · 확정됨 · 순위 오름.</summary>
        public static readonly Color SemGood = Draw.Hex("#4CC38A");
        /// <summary>주의 · 조건 모자람 · 황색 깃발 · SC 띠.</summary>
        public static readonly Color SemWarn = Draw.Hex("#E8B04A");
        /// <summary>나쁨 · 세션 진행 중 점 · 순위 내림 · 지우기 버튼 면.</summary>
        public static readonly Color SemBad = Draw.Hex("#E5484D");
        /// <summary>지우기(위험) 버튼 위 글.</summary>
        public static readonly Color OnSemBad = Draw.Hex("#1C0A0B");

        // ---- 차트: 강조 아닌 계열 회색 4단계(가까운 차례부터) · 눈금 · 커서 · 사건 띠
        public static readonly Color ChartGray1 = Draw.Hex("#9AA3AF"), ChartGray2 = Draw.Hex("#848C98"), ChartGray3 = Draw.Hex("#6B7380"), ChartGray4 = Draw.Hex("#565D68");
        /// <summary>0 기준선(#ECEEF2 12%).</summary>
        public static readonly Color ChartBase = new(236f / 255f, 238f / 255f, 242f / 255f, 0.12f);
        /// <summary>나머지 가로 눈금선(#ECEEF2 6%).</summary>
        public static readonly Color ChartGrid = new(236f / 255f, 238f / 255f, 242f / 255f, 0.06f);
        /// <summary>'지금' 세로 점선(#ECEEF2 24%).</summary>
        public static readonly Color ChartCursor = new(236f / 255f, 238f / 255f, 242f / 255f, 0.24f);
        /// <summary>사건 구간 띠(SC · VSC) = SemWarn 8%.</summary>
        public static readonly Color ChartEventBand = new(232f / 255f, 176f / 255f, 74f / 255f, 0.08f);

        /// <summary>확인 상자 · 모달 덮개(rgba(5,6,8,.62)).</summary>
        public static readonly Color Scrim35 = new(5f / 255f, 6f / 255f, 8f / 255f, 0.62f);

        // ---- 3.6 탭 부품 선 세기(#ECEEF2 위 알파)
        /// <summary>지난 막대 · 단계 끝 점선 · 남은 라운드 테(16%).</summary>
        public static readonly Color ChartMuted = new(236f / 255f, 238f / 255f, 242f / 255f, 0.16f);
        /// <summary>고른 칩 · 초점 칸 · 고른 등급 칸 안쪽 테(32%).</summary>
        public static readonly Color FocusLine = new(236f / 255f, 238f / 255f, 242f / 255f, 0.32f);
        /// <summary>기준 점선(WEC 최소 45분)(48%).</summary>
        public static readonly Color RefLine = new(236f / 255f, 238f / 255f, 242f / 255f, 0.48f);

        // ---- 비 세기 4단계(약 → 강, 레이더 · 강수 막대 · 젖은 노면)
        public static readonly Color Rain1 = Draw.Hex("#4A6290"), Rain2 = Draw.Hex("#7090C8"), Rain3 = Draw.Hex("#9BB5E8"), Rain4 = Draw.Hex("#C6D8FF");

        // ---- 서킷 그림 3.5(모드 CircuitMap 이 씀 · 다크 전용)
        /// <summary>그림 판 면(#0B0D10) · 판 안쪽 선 · 격자 · 피트 레인 점선 · 트랙 몸.</summary>
        public static readonly Color MapPlate = Draw.Hex("#0B0D10"), MapPlateLine = Draw.Hex("#1F232A"), MapGrid = Draw.Hex("#161A20"), MapPitLine = Draw.Hex("#8A929E"), MapTrack = Draw.Hex("#262A32");
        /// <summary>코너 원 면 · 선 · 번호 글.</summary>
        public static readonly Color MapCorner = Draw.Hex("#1B1E24"), MapCornerLine = Draw.Hex("#343A44"), MapCornerInk = Draw.Hex("#C9CFD8");
        /// <summary>트랙 없음 선(고르기 mini) · 매력 가운데 단계(50% 이상).</summary>
        public static readonly Color MapMissing = Draw.Hex("#4B525C"), MapAttrMid = Draw.Hex("#C1AAAE");
        /// <summary>그림 위 글 칩 면(레이더 · #0B0D10 86%).</summary>
        public static readonly Color MapChip = new(11f / 255f, 13f / 255f, 16f / 255f, 0.86f);

        // ---- 3.7
        /// <summary>게임 화면 위 런처 점 둘레 2(캔버스 없음 — 가장 가까운 바탕 GroundDeep 과 같은 값).</summary>
        public static readonly Color LauncherRing = Draw.Hex("#0E1014");
    }
}
