using UnityEngine;

namespace CSCommonUX
{
    /// <summary>F1 UX 색(설계 색). IMGUI(Draw)와 게임 UI 부품(CSCommonStyle)이 같이 씀.</summary>
    internal static class Pal
    {
        public static Color Win = Draw.Hex("#14171C");
        /// <summary>고른 칸(떠오른 면) 바탕 · 그 위 글 · 아이콘(다크 #3A404B · 흰 글 / 라이트 흰 바탕 · 짙은 글).</summary>
        public static Color Selected = Draw.Hex("#3A404B"), OnSelected = Color.white;
        /// <summary>휴대폰 틀(CSCommonUX.Phone): 티타늄 테두리 · 화면 바탕 · 번지는 색, 리퀴드 글라스 면(바탕 · 위 반짝임 · 테두리 · 위 1px 빛).</summary>
        public static Color PhoneBezel = Draw.Hex("#2E3035"), PhoneBezelEdge = Draw.Hex("#5A5E66"), PhoneWall = Color.white;
        public static Color PhoneGlowA = new(0.18f, 0.42f, 1f, 0.5f), PhoneGlowB = new(0.62f, 0.25f, 0.95f, 0.45f);
        public static Color GlassFill = new(0.93f, 0.95f, 0.98f, 0.85f), GlassSheen = new(1f, 1f, 1f, 0.6f), GlassEdge = new(0f, 0f, 0f, 0.10f), GlassShine = new(1f, 1f, 1f, 1f);
        /// <summary>휴대폰 화면(흰색 고정) 위 글: 짙은 글 · 옅은 글.</summary>
        public static Color PhoneInk = Draw.Hex("#1C1C1E"), PhoneInkSub = new(0.11f, 0.11f, 0.12f, 0.6f);
        /// <summary>가장 진한 글(호버 · 강조 — 다크 흰색 / 라이트 거의 검정).</summary>
        public static Color Strong = Color.white;
        /// <summary>호버 · 판 가장자리 살짝 덮는 색(알파는 쓰는 곳에서 — 다크 흰색 / 라이트 검정).</summary>
        public static Color Haze = Color.white;
        /// <summary>강조색으로 꽉 채운 버튼(켜진 도구 · 완료) 위 아이콘 · 글.</summary>
        public static Color AccentInk = Draw.Hex("#06121A");
        public static Color Card = Draw.Hex("#1B1F26");
        /// <summary>Flatten 띠 — 섹션 사이 · 두 열 사이 바닥(Win 보다 한 단 어둡게, 라이트는 한 단 회색).</summary>
        public static Color Band = Draw.Hex("#0E1014");
        public static Color Inset = Draw.Hex("#262B33");
        public static Color Line = Draw.Hex("#2A2F38");
        public static Color Line2 = Draw.Hex("#3A404B");
        public static Color Txt = Draw.Hex("#ECEEF2");
        public static Color Sub = Draw.Hex("#9AA3AF");
        public static Color Dim = Draw.Hex("#7F8894");
        public static Color Ok = Draw.Hex("#5CC8FF");
        public static Color Warn = Draw.Hex("#FFA94D");
        public static Color Red = Draw.Hex("#FF6B5E");
        /// <summary>강조로 쓰는 빨강 자리(값은 Red 와 같음 — 시리즈 테마가 시리즈 색으로 바꿈). 위험 · 오류 뜻은 Red.</summary>
        public static Color RedAccent = Red;
        /// <summary>시리즈 테마가 바꾸지 않는 로고 빨강(레드 플래그 띠 · 소프트 타이어 · 과열 · 나침반 북쪽 · 폴).</summary>
        public static Color FixedRed = Draw.Hex("#E10600");
        public static Color Accent = Draw.Hex("#E10600");
        /// <summary>시리즈 대표색(F1 은 Accent): F2 파랑 · WEC 청록 · 모토GP 주황(옛) · NASCAR 노랑. 바탕 위 글로도, 면으로도(그 위 글 = AccentInk) 4.5:1. 라이트 값은 CSCommonTheme.</summary>
        public static Color SeriesF2 = Draw.Hex("#5294FA"), SeriesWec = Draw.Hex("#12A594"), SeriesMoto = Draw.Hex("#F08A00");
        /// <summary>NASCAR 노랑(브랜드 노랑 계열): 다크 #FFD100(바탕 #14171C 위 12:1 · AccentInk 글 13:1), 라이트 #7A5C00(흰 바탕 · 흰 글 6.2:1).</summary>
        public static Color SeriesNascar = Draw.Hex("#FFD100");
        /// <summary>WEC 방송 타워: 짙은 남색 판 · 상황 띠 빨강 그라데이션(왼쪽 → 오른쪽 끝). 화면 위 방송 그래픽이라 라이트 테마에서도 같음.</summary>
        public static Color WecTowerBg = Draw.Hex("#0B1B3A"), WecBand = Draw.Hex("#E4162B"), WecBandEnd = Draw.Hex("#7C0A1A");
        /// <summary>NASCAR 방송 타워: 검정 판 · 짝수 줄 · 순위 칸 · 시리즈 이름 띠(회색) · 바깥 테두리(밝은 초록). 라이트 테마에서도 같음.</summary>
        public static Color NascarTowerBg = Draw.Hex("#101012"), NascarRowAlt = Draw.Hex("#1A1A1D"), NascarPosBg = Draw.Hex("#2B2C30"), NascarBand = Draw.Hex("#3A3C42"), NascarBorder = Draw.Hex("#3DDC4A");
        /// <summary>
        /// 방송 연출(결승 전 인트로 — 게임 화면 위 그래픽이라 테마와 상관없이 고정): 섹터 1 · 2 · 3(빨강 · 파랑 · 노랑) · 오버테이크 초록 · 글(흰색),
        /// 판 남색(정보 판 · 표지) · 짙은 판(지구 장면 목록 카드) · 트랙 몸(남색) · 그리드 장면 바탕 · 가운데 번짐.
        /// </summary>
        public static Color FixedSectorRed = Draw.Hex("#FF2A2A"), FixedSectorBlue = Draw.Hex("#33A8FF"), FixedSectorYellow = Draw.Hex("#FFE600"), FixedOvertake = Draw.Hex("#3CFF6E"), FixedBroadcastInk = Draw.Hex("#FFFFFF");
        public static Color FixedBroadcastPanel = Draw.Hex("#1D1B28"), FixedBroadcastCard = Draw.Hex("#0E0E18"), FixedBroadcastTrack = Draw.Hex("#1C2140"), FixedBroadcastGrid = Draw.Hex("#121318"), FixedBroadcastGridGlow = Draw.Hex("#2A2B33");
        /// <summary>방송 지구(테마와 상관없이 고정): 깊은 바다 · 밝은 바다 · 땅 · 우주 바탕 · 우주 번짐 · 지구 둘레 빛.</summary>
        public static Color FixedGlobeSeaDeep = Draw.Hex("#061F55"), FixedGlobeSea = Draw.Hex("#3D93E6"), FixedGlobeLand = Draw.Hex("#5FA548"), FixedGlobeSpace = Draw.Hex("#090C16"), FixedGlobeSpaceGlow = Draw.Hex("#123A7A"), FixedGlobeHalo = Draw.Hex("#3B8CFF");
        /// <summary>분류 색 넷(빨강 · 보라 · 파랑 · 청록 — 예: 팬 뉴스 분류 점 · 범례). 라이트 값은 CSCommonTheme(흰 바탕 위 3:1 이상).</summary>
        public static Color CatRed = Draw.Hex("#FF8A80"), CatViolet = Draw.Hex("#C9A6FF"), CatBlue = Draw.Hex("#8FB0FF"), CatTeal = Draw.Hex("#7FD6C8");
        /// <summary>시리즈 옅은 글(Pink 자리) = 시리즈 색과 Strong 을 섞는 비율.</summary>
        /// <summary>시리즈 옅은 글(Pink 자리) = 시리즈 색과 Strong 을 섞는 비율.</summary>
        public const float SeriesTintMix = 0.5f;
        public static Color Yellow = Draw.Hex("#F5C400");
        public static Color Green = Draw.Hex("#22C55E");
        public static Color Teal = Draw.Hex("#2ED4BF");
        /// <summary>주차장 그림 뜻 색: 화단 잔디 · 연석(테마와 상관없음).</summary>
        public static Color Grass = Draw.Hex("#4CA64C"), Curb = Draw.Hex("#CCCCCC");
        /// <summary>
        /// 모드 기본 팔레트 색 — 주 톤 바탕 · 초점 테두리 · 켜짐 · 고른 표시(면 · 테두리 · 아이콘). 모드가 CSCommonTheme.Use 의 CSCommonPalette 로 고름(기본 청록, F1 빨강).
        /// 청록(Teal)은 '잘됨 · 완료' 뜻 색으로 그대로 남음(토스트 완료 · 목표 넘은 막대 · 정상 뱃지).
        /// </summary>
        public static Color Primary = Draw.Hex("#2ED4BF");
        /// <summary>기본 팔레트 색 글(바탕 위 4.5:1 — 글 단추 등) · 그 색으로 채운 면 위 글 · 아이콘.</summary>
        public static Color PrimaryInk = Draw.Hex("#2ED4BF"), OnPrimary = Draw.Hex("#06121A");
        public static Color Purple = Draw.Hex("#B038DE");
        public static Color PersonalBest = Draw.Hex("#00C761");
        public static Color NoTime = Draw.Hex("#52565F");
        public static Color Penalty = Draw.Hex("#E8590C");
        public static Color Track = Draw.Hex("#15151E");
        public static Color PitGrey = Draw.Hex("#4A4A55");
        public static Color Light = Draw.Hex("#C9CFD8");
        public static Color Pink = Draw.Hex("#FFB3AE");
        public static Color DoneDot = Draw.Hex("#1F4D33");
        public static Color DoneInk = Draw.Hex("#4CC787");
        public static Color TodoDot = Draw.Hex("#4D1F1C");

        // 도로 가장자리선 · 시설 표시 (디자인 시스템 토큰 edge-left · edge-right · gravel · tyre)
        public static Color EdgeLeft = Draw.Hex("#00E5FF");
        public static Color EdgeRight = Draw.Hex("#FFB300");
        public static Color Gravel = Draw.Hex("#E76F51");
        public static Color Tyre = Draw.Hex("#343A40");

        // 지도 마커(서킷 라인 도구 · 디자인 툴킷). 상태 색(선택 · 위험 · 정상)은 다른 뜻으로 쓰지 않음
        /// <summary>모든 마커를 두르는 어두운 테두리(잔디 · 아스팔트 · 눈 · 야간 어디서든 대비).</summary>
        public static Color MapHalo = new(11f / 255f, 13f / 255f, 18f / 255f, 0.82f);
        /// <summary>지도 위 흰 표식(올림픽 버스 위 표식 · 아래 쉐브론): 판 · 굵은 글 · 보조 글 · 그림자. 라이트 · 다크 같음(게임 화면 위).</summary>
        public static Color MapTag = Draw.Hex("#FFFFFF"), MapTagInk = Draw.Hex("#14181F"), MapTagSub = Draw.Hex("#5F6368"), MapTagShadow = new(0f, 0f, 0f, 0.28f);
        /// <summary>지도 표식(CSCommonUX.MapPin): 원 면 · 아이콘 청록 · 주의 주황, 상태 점 정상 · 주의, 알약 1px 선 · 기둥 · 발 고리. 라이트 · 다크 같음(게임 화면 위, 판 · 글은 MapTag*).</summary>
        public static Color MapPinOkFill = Draw.Hex("#D6F3EE"), MapPinOkInk = Draw.Hex("#0B4F47"), MapPinWarnFill = Draw.Hex("#FFE8CC"), MapPinWarnInk = Draw.Hex("#8A4A00");
        /// <summary>지도 표식 종류별 원(면 · 아이콘) — 같은 지도에 여러 종류가 섞일 때 가르기: 기차(하늘) · 버스(초록) · 비행기(보라).</summary>
        public static Color MapPinTrainFill = Draw.Hex("#D7E9FF"), MapPinTrainInk = Draw.Hex("#0B3354"), MapPinBusFill = Draw.Hex("#D6F3DD"), MapPinBusInk = Draw.Hex("#0E5A2B"), MapPinPlaneFill = Draw.Hex("#E8E0FF"), MapPinPlaneInk = Draw.Hex("#3A2A78");
        /// <summary>열차 표식(CSCommonUX.TrainTag): 신호 대기 점 · 시외 칩 · 칸 사이 점 · 급행 · 완행 종별(바탕 · 글). 정시 · 지연 점은 MapPinOkDot · MapPinWarnDot.</summary>
        /// <summary>도로 표지판(CSCommonUX.RoadSign): 고속도로 초록 · 흰 글자 · 테 · ETC 전용 보라 · 일반 차로 진한 초록.</summary>
        public static Color RoadSignGreen = Draw.Hex("#0D6A3A"), RoadSignInk = Draw.Hex("#FFFFFF"), RoadSignEtc = Draw.Hex("#6D3FA0"), RoadSignGeneral = Draw.Hex("#0A5A30");
        /// <summary>활주로 표지(CSCommonUX.RunwaySign · ICAO 의무 안내 표지): 빨강 바탕 · 흰 글자 · 검은 테.</summary>
        public static Color RunwaySignRed = Draw.Hex("#C8102E"), RunwaySignInk = Draw.Hex("#FFFFFF"), RunwaySignEdge = Draw.Hex("#111111");
        public static Color MapPinHoldDot = Draw.Hex("#D93A32"), TagIntercity = Draw.Hex("#5F6873"), TagSep = Draw.Hex("#B0B5BC"), TagExpressBg = new(1f, 107f / 255f, 94f / 255f, 0.16f), TagExpressInk = Draw.Hex("#C2392D"), TagLocalBg = new(26f / 255f, 143f / 255f, 92f / 255f, 0.14f), TagLocalInk = Draw.Hex("#127048");
        public static Color MapPinOkDot = Draw.Hex("#1A8F5C"), MapPinWarnDot = Draw.Hex("#C2790A"), MapPinLine = new(20f / 255f, 24f / 255f, 31f / 255f, 0.14f), MapPinStem = new(1f, 1f, 1f, 0.9f), MapPinFootRing = new(11f / 255f, 79f / 255f, 71f / 255f, 0.35f);
        /// <summary>
        /// 비행기 카드(CSCommonUX.TrainTag 의 Phase 모양 · 대중교통 캔버스 Runways): 상태 알약 정상(면 · 글) · 주의(면 · 글), 단계 막대 · 승객 막대 빈 칸, 단계 라벨, 순번 칩 면.
        /// 점 · 채운 칸은 MapPinOkDot · MapPinWarnDot, 굵은 글은 MapTagInk, 보조 글 · 승객 채움은 TagIntercity(#5F6873).
        /// </summary>
        public static Color FlightOkBg = Draw.Hex("#E1F3EA"), FlightOkInk = Draw.Hex("#0B5B3B"), FlightWarnBg = Draw.Hex("#FBECD0"), FlightWarnInk = Draw.Hex("#7A4A00");
        public static Color FlightTrack = Draw.Hex("#E3E7EC"), FlightLabel = Draw.Hex("#8A929D"), FlightRankBg = Draw.Hex("#EEF1F5");

        // 항공사 대표색(편명 칩 면). JL · NH · KE · OZ · 7C 는 시안 값, 나머지는 근사값 · 공식 가이드 미확인.
        // 흰 글자와 대비 4.5:1 이상인 색만 흰 글자, 밝은 색(OZ · 7C · GK · HD · 5J)은 어두운 글자(MapTagInk)와 대비 5.8:1 이상.
        static readonly System.Collections.Generic.Dictionary<string, Color> airline = new()
        {
            ["JL"] = Draw.Hex("#C8102E"), ["NH"] = Draw.Hex("#13448F"), ["KE"] = Draw.Hex("#00256C"), ["OZ"] = Draw.Hex("#C9A227"), ["7C"] = Draw.Hex("#F26B21"),
            ["BC"] = Draw.Hex("#1E3C8C"), ["MM"] = Draw.Hex("#C8007A"), ["GK"] = Draw.Hex("#F96B00"), ["HD"] = Draw.Hex("#2DA8E0"), ["NU"] = Draw.Hex("#C8102E"),
            ["BX"] = Draw.Hex("#1C3F94"), ["CA"] = Draw.Hex("#C8161D"), ["MU"] = Draw.Hex("#00387B"), ["FM"] = Draw.Hex("#B5121B"), ["CZ"] = Draw.Hex("#00599F"),
            ["CX"] = Draw.Hex("#006564"), ["UO"] = Draw.Hex("#5E2A84"), ["CI"] = Draw.Hex("#1B3A6B"), ["BR"] = Draw.Hex("#006A4E"), ["SQ"] = Draw.Hex("#00266B"),
            ["TG"] = Draw.Hex("#51247A"), ["PR"] = Draw.Hex("#003F87"), ["5J"] = Draw.Hex("#FFD200"), ["AA"] = Draw.Hex("#0061AB"), ["UA"] = Draw.Hex("#002244"),
            ["DL"] = Draw.Hex("#003A70"), ["HA"] = Draw.Hex("#413691"), ["BA"] = Draw.Hex("#075AAA"), ["AF"] = Draw.Hex("#002157"), ["LH"] = Draw.Hex("#05164D"),
            ["EK"] = Draw.Hex("#D71921"), ["QF"] = Draw.Hex("#CC0000"),
        };
        /// <summary>표에 없는 항공사 칩 면(흰 글자 대비 5.65:1).</summary>
        public static Color AirlineDefault = Draw.Hex("#5F6873");

        /// <summary>항공사 코드(예 "JL") → 편명 칩 면 · 글자색(fg = 흰색 또는 MapTagInk). 표에 없으면 AirlineDefault + 흰 글자. 도시 항공사 색은 모드가 직접 넘겨요.</summary>
        public static Color AirlineChip(string code, out Color fg)
        {
            fg = MapTag;
            if (code == null || !airline.TryGetValue(code, out var bg)) return AirlineDefault;
            if (code == "OZ" || code == "7C" || code == "GK" || code == "HD" || code == "5J") fg = MapTagInk;
            return bg;
        }
        /// <summary>선택 · 끄는 중 · 깜빡임(예약 — 다른 마커에 쓰지 않음).</summary>
        public static Color MapSelect = Draw.Hex("#FFD166");
        /// <summary>철거 · 실패 · 보행로 미연결(예약).</summary>
        public static Color MapDanger = Draw.Hex("#FF4D4F");
        /// <summary>연결됨 · 추천 · 마샬 초록 깃발.</summary>
        public static Color MapOk = Draw.Hex("#22C55E");
        /// <summary>라인 그리기: 지금 누를 수 있는 다음 노드.</summary>
        public static Color MapNext = Draw.Hex("#3BA7FF");
        /// <summary>손잡이 · 마지막 노드 · 커서.</summary>
        public static Color MapHandle = Draw.Hex("#FFFFFF");
        /// <summary>중계 카메라(피트 청록과 구분).</summary>
        public static Color MapCamera = Draw.Hex("#A78BFA");
        /// <summary>노드 옮기기의 일반 노드.</summary>
        public static Color MapNode = Draw.Hex("#C8D1E0");
        /// <summary>피트 노드.</summary>
        public static Color MapPit = Draw.Hex("#2DD4BF");
        /// <summary>제동판 150 · 100(50 은 MapDanger) — 선택 금색과 겹치지 않게 흰 · 회색.</summary>
        public static Color MapBrake150 = Draw.Hex("#F1F3F5");
        public static Color MapBrake100 = Draw.Hex("#ADB5BD");
        /// <summary>지도 도로: 레이스 도로 · 피트 도로 · 다른 도로(라인 도구 밑그림).</summary>
        public static Color MapRoadRace = Draw.Hex("#6B7480"), MapRoadPit = Draw.Hex("#2B6E66"), MapRoadOther = Draw.Hex("#56606B");
        /// <summary>끊긴 라인 조각(지도 빨강).</summary>
        public static Color MapBroken = Draw.Hex("#FF3B30");
        /// <summary>디자인 에디터 지도 뱃지: 바닐라 가드레일 · 마샬 깃발(SC 주황 · 노랑 · 빨강) · 아스팔트 런오프 · X-MODE · 관중 구역(초록 · 주황) · 팬 센터.</summary>
        public static Color MapArmco = Draw.Hex("#8D99AE"), MapMarshalSc = Draw.Hex("#F4A261"), MapMarshalYellow = Draw.Hex("#FACC15"), MapMarshalRed = Draw.Hex("#EF476F");
        public static Color MapAsphalt = Draw.Hex("#118AB2"), MapXMode = Draw.Hex("#39FF14"), MapFanGreen = Draw.Hex("#7BD389"), MapFanCenter = Draw.Hex("#00D4AA");

        // F1 창 글 · 바탕
        /// <summary>본문 글(기사 · 설명 큰 글, Txt 보다 한 단계 부드럽게).</summary>
        public static Color Body = Draw.Hex("#DDE2E8");
        /// <summary>히어로 · 펼친 기사 바탕(카드보다 한 단계 밝게).</summary>
        public static Color Hero = Draw.Hex("#20252D");
        /// <summary>주의 단계 점 바탕(DoneDot · TodoDot 과 짝).</summary>
        public static Color WarnDot = Draw.Hex("#3A2A14");
        /// <summary>챔피언십 선두 · 섹터 2 파랑.</summary>
        public static Color Blue = Draw.Hex("#2E9BFF");
        /// <summary>레드 플래그 · 가장 나쁨(차트 띠).</summary>
        public static Color Flag = Draw.Hex("#FF5A4E");
        /// <summary>일정: 스프린트(주황) · 퀄리파잉(하늘) 글 · 스프린트 막대 · 밝은 알약 위 글.</summary>
        public static Color SprintInk = Draw.Hex("#FFB060"), QualiInk = Draw.Hex("#7FD3FF"), Sprint = Draw.Hex("#FF8000"), OnLight = Draw.Hex("#14141A");
        /// <summary>시즌 차트 계열 넷(점수 · 승 · 포디움 · 폴).</summary>
        public static readonly Color[] Series = { Draw.Hex("#6366F1"), Draw.Hex("#3B82F6"), Draw.Hex("#1E3A8A"), Draw.Hex("#EC4899") };
        /// <summary>올림픽: 오륜 파랑(대표색 없는 팀 · 난민 선수단) · 나라 스포츠 대표색(선수단 버스).</summary>
        public static Color OlympicBlue = Draw.Hex("#0081C8");
        /// <summary>메달 점(금 · 은 · 동 — 메달 순위 머리 · 우리 메달 줄). 테마와 상관없는 뜻 색.</summary>
        public static Color MedalGold = Draw.Hex("#D9A92B"), MedalSilver = Draw.Hex("#A8AFB8"), MedalBronze = Draw.Hex("#C2834A");
        /// <summary>
        /// 프로야구 리그 팀 색(시안 팀 막대 · 테마와 상관없는 뜻 색): 펠리컨스 하늘 · 팰컨스 빨강 · 볼츠 노랑 · 레이븐스 보라 · 코메츠 청록 · 머스탱스 주황 · 웨일스 검정 ·
        /// 오터스 초록 · 스톰즈 갈색 · 아이언스 회색 · 돌핀스 남색(시안 홈팀 설정 '남색') · 그리폰스 자주(NPB 12팀만). BallTeams = 팀 번호 차례.
        /// </summary>
        public static Color BallPelicans = Draw.Hex("#4FA3E0"), BallFalcons = Draw.Hex("#E0564F"), BallBolts = Draw.Hex("#F2B33D"), BallRavens = Draw.Hex("#8E7CC3");
        public static Color BallComets = Draw.Hex("#3FBFA8"), BallMustangs = Draw.Hex("#D9774A"), BallWhales = Draw.Hex("#2B2F36"), BallOtters = Draw.Hex("#6FA86B");
        public static Color BallStorms = Draw.Hex("#A0764A"), BallIrons = Draw.Hex("#5F6368"), BallDolphins = Draw.Hex("#2F4F8F"), BallGriffons = Draw.Hex("#B5478A");
        public static readonly Color[] BallTeams = { BallPelicans, BallFalcons, BallBolts, BallRavens, BallComets, BallMustangs, BallWhales, BallOtters, BallStorms, BallIrons, BallDolphins, BallGriffons };
        /// <summary>프로야구 볼카운트 점(시안 GameLive): 스트라이크 노랑 · 아웃 빨강(볼 = Pal.Primary 초록).</summary>
        public static Color BallStrike = Draw.Hex("#E3A008"), BallOut = Draw.Hex("#D93A2F");
        /// <summary>팬챗(시민 커뮤니티): 보라(인플루언서 인증) · 분홍 · 하늘 · 라임 — 시민 아바타 색은 ChatAvatars 에서 이름 해시로 고름.</summary>
        public static Color ChatViolet = Draw.Hex("#A78BFA"), ChatPink = Draw.Hex("#F472B6"), ChatSky = Draw.Hex("#7DD3FC"), ChatLime = Draw.Hex("#A3E635");
        public static readonly Color[] ChatAvatars = { Teal, Ok, ChatViolet, Warn, ChatPink, Yellow, Green, ChatSky, ChatLime };

        // ---- 전광판(대중교통 Extension 캔버스): 테마와 상관없이 고정. 실제 LED · LCD 판 색
        /// <summary>LED 판 바탕 · 테두리 · 도트 격자 그림자(3px 칸 사이).</summary>
        public static Color BoardLed = Draw.Hex("#060708"), BoardLedLine = Draw.Hex("#1D2026"), BoardDotShade = new(6f / 255f, 7f / 255f, 8f / 255f, 0.6f);
        /// <summary>기차역 LED 호박색(글 · 머리 · 강조 글).</summary>
        public static Color BoardAmber = Draw.Hex("#FFB21E"), BoardAmberDim = Draw.Hex("#BF8A26"), BoardAmberHot = Draw.Hex("#FFF1D2");
        /// <summary>버스터미널 LED 초록(글 · 머리 · 강조 글).</summary>
        public static Color BoardGreen = Draw.Hex("#62E08C"), BoardGreenDim = Draw.Hex("#46A866"), BoardGreenHot = Draw.Hex("#E6FFEE");
        /// <summary>일본식 판: 발차 시각 주황 · 행선 크림 · 타는 위치 · 흐르는 글 초록 · 특급 빨강.</summary>
        /// <summary>일본형 버스터미널 판(버스타 신주쿠식 남색 LED): 판 · 글 · 영어 · 지연 · 승차홈 칸 · 노선 칩 기본 · 줄 선.</summary>
        public static Color BoardJpBusBg = Draw.Hex("#070B16"), BoardJpBusInk = Draw.Hex("#F4F6FB"), BoardJpBusSub = Draw.Hex("#8D97AD"), BoardJpBusLate = Draw.Hex("#FFB02E"), BoardJpBusBay = Draw.Hex("#2F6FDC"), BoardJpBusChip = Draw.Hex("#5B6577"), BoardJpBusRule = new(150f / 255f, 170f / 255f, 210f / 255f, 0.22f);
        public static Color BoardJpTime = Draw.Hex("#FF9A1E"), BoardJpDest = Draw.Hex("#FFE9B0"), BoardJpGreen = Draw.Hex("#9BE35A"), BoardJpLtd = Draw.Hex("#FF5A4F");
        /// <summary>인천공항식 판: 바탕 남색 · 머리 띠 · 띠 글 · 줄 교차 · 편명 노랑 · 보조 글.</summary>
        public static Color BoardAirBg = Draw.Hex("#060B14"), BoardAirBar = Draw.Hex("#0F2A55"), BoardAirBarInk = Draw.Hex("#CFE0FF"), BoardAirRowA = Draw.Hex("#0A1426"), BoardAirRowB = Draw.Hex("#070E1B"), BoardAirFlight = Draw.Hex("#FFD23F"), BoardAirSub = Draw.Hex("#8FA6C8");
        /// <summary>나리타공항식 판: 바탕 검정 · 머리 띠 · 줄 교차 · 편명 하늘색.</summary>
        public static Color BoardNrtBg = Draw.Hex("#050505"), BoardNrtBar = Draw.Hex("#1D2026"), BoardNrtRowA = Draw.Hex("#0C0C0D"), BoardNrtFlight = Draw.Hex("#8FD0FF");
        /// <summary>판 위 상태: 탑승 중 · 도착 초록, 흰 글.</summary>
        public static Color BoardOk = Draw.Hex("#3DDC84"), BoardInk = Draw.Hex("#F2F5FA");
        /// <summary>선로 관제 판(Board · 대중교통 캔버스 TrackCtc): 바탕 · 빈 구간 · 점유 · 진로 · 구간 경계 금 · 역 이름 테 · 역 이름 글.</summary>
        public static Color BoardCtcBg = Draw.Hex("#05070A"), BoardCtcTrack = Draw.Hex("#D9DEE6"), BoardCtcBusy = Draw.Hex("#FF4A3F"), BoardCtcRoute = Draw.Hex("#3DDC84"), BoardCtcTick = Draw.Hex("#5B6672"), BoardCtcStation = Draw.Hex("#5AA9E6"), BoardCtcStationInk = Draw.Hex("#BFE0FF");
        /// <summary>기차 신호기(Board.Signal · 대중교통 캔버스 TrainSignals): 녹 · 황 · 적 등 · 꺼진 등 · 몸통 · 몸통 1px 테 · 기둥 · 발.</summary>
        public static Color SignalGreen = Draw.Hex("#3DDC84"), SignalYellow = Draw.Hex("#FFC233"), SignalRed = Draw.Hex("#FF5A52"), SignalOff = Draw.Hex("#262A32"), SignalHousing = Draw.Hex("#0A0B0D"), SignalRing = new(1f, 1f, 1f, 0.22f), SignalMast = Draw.Hex("#C9CFD8");
        /// <summary>공연 장면(Concert · 콘서트 확장판 캔버스): 밤 무대 바탕 · 트러스 · 조명 기구 · LED 화면 · 무대 바닥 · 관객 실루엣 · 휴대폰 불빛 · 레코드판 · 홈.</summary>
        public static Color ConcertScene = Draw.Hex("#07080C"), ConcertTruss = Draw.Hex("#1C2029"), ConcertLamp = Draw.Hex("#2A2F38"), ConcertLed = Draw.Hex("#15111A"), ConcertFloor = Draw.Hex("#14171E"), ConcertCrowd = Draw.Hex("#000000"), ConcertPhone = Draw.Hex("#FFF5C2"), ConcertVinyl = Draw.Hex("#0D0E12"), ConcertGroove = Draw.Hex("#1D2028"), ConcertCurtain = Draw.Hex("#3A0D14"), ConcertCurtainLight = Draw.Hex("#5A1520"), ConcertPosterSub = Draw.Hex("#D6DBE2");
        /// <summary>가상 예매 사이트 화면(콘서트 예매 오픈 전 · 대기줄): 브라우저 머리 · 주소 칸 · 사이트 바탕 · 카드 · 선 · 글 · 보조 글 · 흐린 글 · 사이트 강조 · 안내 바탕 · 안내 글 · 막힌 단추.</summary>
        public static Color SiteChrome = Draw.Hex("#1C2029"), SiteAddress = Draw.Hex("#11141A"), SiteBg = Draw.Hex("#F5F6F8"), SiteCard = Draw.Hex("#FFFFFF"), SiteLine = Draw.Hex("#E4E6EA"), SiteInk = Draw.Hex("#14171A"), SiteSub = Draw.Hex("#5F6368"), SiteMuted = Draw.Hex("#80868E"), SiteAccent = Draw.Hex("#FF4F6D"), SiteNoteBg = Draw.Hex("#FFF7E6"), SiteNoteInk = Draw.Hex("#8A5A00"), SiteOff = Draw.Hex("#E4E6EA");
        /// <summary>공연 숫자: 인기 오름 · 내림 · 좌석 등급 VIP · R · S · 일반 · 팔린 자리.</summary>
        public static Color ConcertUp =Draw.Hex("#7BE0B0"), ConcertDown = Draw.Hex("#FF8A7A"), ConcertSeatVip = Draw.Hex("#C9A6FF"), ConcertSeatR = Draw.Hex("#6FB7FF"), ConcertSeatS = Draw.Hex("#7BE0B0"), ConcertSeatA = Draw.Hex("#FFC857"), ConcertSold = Draw.Hex("#2A2F38");
        /// <summary>공연개최의향서(Concert.Letter · 캔버스 Letter): 문서 종류(0 공문 · 1 서한 · 2 레터헤드 · 3 메모)마다 종이 · 글 · 보조 글 · 칸 바탕 · 선.</summary>
        public static readonly Color[] LetterPaper = { Draw.Hex("#FBFAF6"), Draw.Hex("#F6F0E2"), Draw.Hex("#FFFFFF"), Draw.Hex("#EFEDE7") },
            LetterInk = { Draw.Hex("#1E1B16"), Draw.Hex("#2A2116"), Draw.Hex("#14171A"), Draw.Hex("#1D1D1B") },
            LetterSub = { Draw.Hex("#6B665C"), Draw.Hex("#7A6A52"), Draw.Hex("#5F6368"), Draw.Hex("#5C5A55") },
            LetterCell = { Draw.Hex("#EFECE4"), Draw.Hex("#EDE3CC"), Draw.Hex("#F3F5F8"), Draw.Hex("#E2DFD7") },
            LetterLine = { Draw.Hex("#BDB7AA"), Draw.Hex("#A8957A"), Draw.Hex("#E4E6EA"), Draw.Hex("#1D1D1B") };
        /// <summary>의향서 인장 · 레터헤드: 붉은 인주 · 머리 남색 · 머리 보조 글 · 금박 · 금박 밝은 금 · 금박 어두운 리본.</summary>
        public static Color LetterSeal = Draw.Hex("#C8261D"), LetterHead = Draw.Hex("#0F1B2D"), LetterHeadSub = Draw.Hex("#9FB0C8"), LetterGold = Draw.Hex("#C9A24A"), LetterGoldLight = Draw.Hex("#F3E2B0"), LetterGoldDark = Draw.Hex("#A8842F");

        static readonly System.Collections.Generic.Dictionary<string, Color> teams = new()
        {
            { "KOR", Draw.Hex("#CD2E3A") }, { "NED", Draw.Hex("#FF6200") }, { "ITA", Draw.Hex("#0066B3") }, { "AUS", Draw.Hex("#FFCD00") },
            { "NZL", Draw.Hex("#111111") }, { "GER", Draw.Hex("#1A1A1A") }, { "BRA", Draw.Hex("#FEDD00") }, { "SWE", Draw.Hex("#FECC02") },
            { "UKR", Draw.Hex("#0057B7") }, { "IND", Draw.Hex("#FF9933") }, { "RSA", Draw.Hex("#007749") }, { "JAM", Draw.Hex("#009B3A") },
            { "USA", Draw.Hex("#0A3161") }, { "GBR", Draw.Hex("#012169") }, { "IRL", Draw.Hex("#169B62") }, { "MEX", Draw.Hex("#006847") },
            { "COL", Draw.Hex("#FCD116") }, { "BEL", Draw.Hex("#E30613") }, { "POR", Draw.Hex("#DA291C") },
        };

        /// <summary>국기에서 가장 눈에 띄는 색(흰 · 검은 바탕보다 채도 높은 색) — 스포츠 대표색이 없는 나라.</summary>
        static readonly System.Collections.Generic.Dictionary<string, Color> flags = new()
        {
            { "CHN", Draw.Hex("#DE2910") },
            { "JPN", Draw.Hex("#BC002D") },
            { "FRA", Draw.Hex("#0055A4") },
            { "CAN", Draw.Hex("#D80621") },
            { "UZB", Draw.Hex("#0099B5") },
            { "HUN", Draw.Hex("#CE2939") },
            { "ESP", Draw.Hex("#AA151B") },
            { "KEN", Draw.Hex("#006600") },
            { "NOR", Draw.Hex("#BA0C2F") },
            { "IRI", Draw.Hex("#239F40") },
            { "ROU", Draw.Hex("#002B7F") },
            { "GEO", Draw.Hex("#E8112D") },
            { "BUL", Draw.Hex("#00966E") },
            { "SRB", Draw.Hex("#C6363C") },
            { "CZE", Draw.Hex("#11457E") },
            { "DEN", Draw.Hex("#C8102E") },
            { "CRO", Draw.Hex("#E8112D") },
            { "CUB", Draw.Hex("#002A8F") },
            { "AZE", Draw.Hex("#00B5E2") },
            { "POL", Draw.Hex("#DC143C") },
            { "TUR", Draw.Hex("#E30A17") },
            { "THA", Draw.Hex("#241D4F") },
            { "EGY", Draw.Hex("#CE1126") },
            { "ARG", Draw.Hex("#74ACDF") },
            { "SUI", Draw.Hex("#DA291C") },
            { "AUT", Draw.Hex("#ED2939") },
            { "GRE", Draw.Hex("#0D5EAF") },
            { "KAZ", Draw.Hex("#00AFCA") },
            { "ISR", Draw.Hex("#0038B8") },
            { "HKG", Draw.Hex("#DE2910") },
            { "TPE", Draw.Hex("#0F3F8F") },
            { "INA", Draw.Hex("#CE1126") },
            { "PHI", Draw.Hex("#0038A8") },
            { "PUR", Draw.Hex("#ED0000") },
            { "ECU", Draw.Hex("#FFDD00") },
            { "DOM", Draw.Hex("#002D62") },
            { "SLO", Draw.Hex("#005DA4") },
            { "LTU", Draw.Hex("#FDB913") },
            { "ALG", Draw.Hex("#006233") },
            { "MAR", Draw.Hex("#C1272D") },
            { "ETH", Draw.Hex("#078930") },
            { "UGA", Draw.Hex("#FCDC04") },
            { "ARM", Draw.Hex("#D90012") },
            { "MGL", Draw.Hex("#C4272F") },
            { "KOS", Draw.Hex("#244AA5") },
            { "PER", Draw.Hex("#D91023") },
            { "CHI", Draw.Hex("#D52B1E") },
            { "SGP", Draw.Hex("#EF3340") },
            { "MAS", Draw.Hex("#010066") },
            { "PAK", Draw.Hex("#01411C") },
            { "GRN", Draw.Hex("#CE1126") },
            { "LCA", Draw.Hex("#66CCFF") },
            { "DMA", Draw.Hex("#006B3F") },
            { "BOT", Draw.Hex("#75AADB") },
            { "CPV", Draw.Hex("#003893") },
            { "ALB", Draw.Hex("#E41E20") },
            { "TJK", Draw.Hex("#CC0000") },
            { "KGZ", Draw.Hex("#E8112D") },
            { "BRN", Draw.Hex("#CE1126") },
            { "QAT", Draw.Hex("#8A1538") },
            { "ZAM", Draw.Hex("#198A00") },
            { "CIV", Draw.Hex("#F77F00") },
            { "FIJ", Draw.Hex("#68BFE5") },
            { "TUN", Draw.Hex("#E70013") },
            { "JOR", Draw.Hex("#007A3D") },
            { "SVK", Draw.Hex("#0B4EA2") },
            { "LAT", Draw.Hex("#9E3039") },
            { "EST", Draw.Hex("#0072CE") },
            { "FIN", Draw.Hex("#002F6C") },
            { "CYP", Draw.Hex("#D57800") },
            { "VEN", Draw.Hex("#FCE300") },
            { "URU", Draw.Hex("#0038A8") },
            { "NGR", Draw.Hex("#008751") },
            { "GHA", Draw.Hex("#006B3F") },
            { "CMR", Draw.Hex("#007A5E") },
            { "SEN", Draw.Hex("#00853F") },
            { "VIE", Draw.Hex("#DA251D") },
            { "KSA", Draw.Hex("#006C35") },
            { "UAE", Draw.Hex("#00732F") },
            { "KUW", Draw.Hex("#007A3D") },
            { "IRQ", Draw.Hex("#CE1126") },
            { "LBN", Draw.Hex("#EE161F") },
            { "SYR", Draw.Hex("#CE1126") },
            { "PLE", Draw.Hex("#009736") },
            { "OMA", Draw.Hex("#DB161B") },
            { "YEM", Draw.Hex("#CE1126") },
            { "AFG", Draw.Hex("#D32011") },
            { "SRI", Draw.Hex("#8D153A") },
            { "BAN", Draw.Hex("#006A4E") },
            { "NEP", Draw.Hex("#DC143C") },
            { "BHU", Draw.Hex("#FF4E12") },
            { "MDV", Draw.Hex("#D21034") },
            { "MYA", Draw.Hex("#FECB00") },
            { "LAO", Draw.Hex("#CE1126") },
            { "CAM", Draw.Hex("#032EA1") },
            { "BRU", Draw.Hex("#F7E017") },
            { "TLS", Draw.Hex("#DC241F") },
            { "PRK", Draw.Hex("#ED1C27") },
            { "TKM", Draw.Hex("#00843D") },
            { "ISL", Draw.Hex("#02529C") },
            { "LUX", Draw.Hex("#00A1DE") },
            { "LIE", Draw.Hex("#002B7F") },
            { "MON", Draw.Hex("#CE1126") },
            { "AND", Draw.Hex("#10069F") },
            { "SMR", Draw.Hex("#5EB6E4") },
            { "MLT", Draw.Hex("#CF142B") },
            { "MNE", Draw.Hex("#C40308") },
            { "BIH", Draw.Hex("#002395") },
            { "MKD", Draw.Hex("#D20000") },
            { "MDA", Draw.Hex("#0046AE") },
            { "PAN", Draw.Hex("#DA121A") },
            { "CRC", Draw.Hex("#002B7F") },
            { "GUA", Draw.Hex("#4997D0") },
            { "HON", Draw.Hex("#00BCE4") },
            { "ESA", Draw.Hex("#0F47AF") },
            { "NCA", Draw.Hex("#0067C6") },
            { "BIZ", Draw.Hex("#003F87") },
            { "HAI", Draw.Hex("#00209F") },
            { "BAH", Draw.Hex("#00778B") },
            { "BAR", Draw.Hex("#00267F") },
            { "TTO", Draw.Hex("#DA1A35") },
            { "GUY", Draw.Hex("#009E49") },
            { "SUR", Draw.Hex("#377E3F") },
            { "PAR", Draw.Hex("#D52B1E") },
            { "BOL", Draw.Hex("#007934") },
            { "ANT", Draw.Hex("#CE1126") },
            { "SKN", Draw.Hex("#009E49") },
            { "VIN", Draw.Hex("#009E60") },
            { "BER", Draw.Hex("#C8102E") },
            { "CAY", Draw.Hex("#012169") },
            { "IVB", Draw.Hex("#012169") },
            { "ISV", Draw.Hex("#FFC72C") },
            { "ARU", Draw.Hex("#418FDE") },
            { "ASA", Draw.Hex("#002868") },
            { "GUM", Draw.Hex("#00297B") },
            { "SAM", Draw.Hex("#CE1126") },
            { "TGA", Draw.Hex("#C10000") },
            { "PNG", Draw.Hex("#CE1126") },
            { "SOL", Draw.Hex("#0051BA") },
            { "VAN", Draw.Hex("#009543") },
            { "KIR", Draw.Hex("#CE1126") },
            { "TUV", Draw.Hex("#5B97B1") },
            { "NRU", Draw.Hex("#002B7F") },
            { "PLW", Draw.Hex("#4AADD6") },
            { "FSM", Draw.Hex("#75B2DD") },
            { "MHL", Draw.Hex("#003893") },
            { "COK", Draw.Hex("#012169") },
            { "LBA", Draw.Hex("#239E46") },
            { "SUD", Draw.Hex("#007229") },
            { "SSD", Draw.Hex("#0F47AF") },
            { "ERI", Draw.Hex("#EA0437") },
            { "DJI", Draw.Hex("#6AB2E7") },
            { "SOM", Draw.Hex("#4189DD") },
            { "TAN", Draw.Hex("#1EB53A") },
            { "RWA", Draw.Hex("#00A1DE") },
            { "BDI", Draw.Hex("#CE1126") },
            { "MOZ", Draw.Hex("#007168") },
            { "MAW", Draw.Hex("#CE1126") },
            { "ZIM", Draw.Hex("#319208") },
            { "NAM", Draw.Hex("#003580") },
            { "LES", Draw.Hex("#00209F") },
            { "SWZ", Draw.Hex("#3E5EB9") },
            { "MAD", Draw.Hex("#FC3D32") },
            { "MRI", Draw.Hex("#EA2839") },
            { "SEY", Draw.Hex("#003F87") },
            { "COM", Draw.Hex("#3A75C4") },
            { "ANG", Draw.Hex("#CC092F") },
            { "COD", Draw.Hex("#007FFF") },
            { "CGO", Draw.Hex("#009543") },
            { "GAB", Draw.Hex("#009E60") },
            { "GEQ", Draw.Hex("#3E9A00") },
            { "CAF", Draw.Hex("#003082") },
            { "CHA", Draw.Hex("#002664") },
            { "NIG", Draw.Hex("#E05206") },
            { "MLI", Draw.Hex("#14B53A") },
            { "BUR", Draw.Hex("#EF2B2D") },
            { "BEN", Draw.Hex("#008751") },
            { "TOG", Draw.Hex("#006A4E") },
            { "GUI", Draw.Hex("#CE1126") },
            { "GBS", Draw.Hex("#CE1126") },
            { "SLE", Draw.Hex("#1EB53A") },
            { "LBR", Draw.Hex("#BF0A30") },
            { "GAM", Draw.Hex("#CE1126") },
            { "MTN", Draw.Hex("#00A95C") },
            { "STP", Draw.Hex("#12AD2B") },
        };

        /// <summary>나라색: 스포츠 대표색(19개국) → 국기에서 가장 눈에 띄는 색 → 국기 없는 팀(개인 중립 · 난민)은 오륜 파랑.</summary>
        public static Color Team(string code)
        {
            if (code == null) return OlympicBlue;
            if (teams.TryGetValue(code, out var c)) return c;
            return flags.TryGetValue(code, out c) ? c : OlympicBlue;
        }

        /// <summary>레이더 밝은 지도(기상청 식): 글 · 보조 글 · 실황 · 경고 · 줄무늬.</summary>
        public static Color RadarInk = Draw.Hex("#1E2228"), RadarSub = Draw.Hex("#5A6068"), RadarLive = Draw.Hex("#0A7CC4"), RadarWarn = Draw.Hex("#D9480F"), RadarStreak = Draw.Hex("#2A2E34");
        /// <summary>정치 시위 군중 팻말(3D 재질 색 · 테마와 상관없음): 판 = 흰 종이 · 막대 = 나무 · 글자 = 먹.</summary>
        public static readonly Color SignPaper = new(0.94f, 0.93f, 0.89f, 1f), SignWood = new(0.52f, 0.37f, 0.22f, 1f), SignInk = Color.black;
    }
}
