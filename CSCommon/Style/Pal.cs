using UnityEngine;

namespace CSCommon
{
    /// <summary>F1 UX 색(설계 색). IMGUI(Draw)와 게임 UI 부품(CSCommonStyle)이 같이 씀.</summary>
    internal static class Pal
    {
        public static Color Win = Draw.Hex("#14171C");
        /// <summary>고른 칸(떠오른 면) 바탕 · 그 위 글 · 아이콘(다크 #3A404B · 흰 글 / 라이트 흰 바탕 · 짙은 글).</summary>
        public static Color Selected = Draw.Hex("#3A404B"), OnSelected = Color.white;
        /// <summary>휴대폰 틀(CSCommon.Phone): 티타늄 테두리 · 화면 바탕 · 번지는 색, 리퀴드 글라스 면(바탕 · 위 반짝임 · 테두리 · 위 1px 빛).</summary>
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
        public static Color Accent = Draw.Hex("#E10600");
        /// <summary>시리즈 대표색(F1 창 제목줄 아래 띠 · F1 은 Accent): F2 파랑 · WEC 청록 · 모토GP 주황.</summary>
        public static Color SeriesF2 = Draw.Hex("#1F6FEB"), SeriesWec = Draw.Hex("#12A594"), SeriesMoto = Draw.Hex("#F08A00");
        public static Color Yellow = Draw.Hex("#F5C400");
        public static Color Green = Draw.Hex("#22C55E");
        public static Color Teal = Draw.Hex("#2ED4BF");
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
        /// <summary>팬챗(시민 커뮤니티): 보라(인플루언서 인증) · 분홍 · 하늘 · 라임 — 시민 아바타 색은 ChatAvatars 에서 이름 해시로 고름.</summary>
        public static Color ChatViolet = Draw.Hex("#A78BFA"), ChatPink = Draw.Hex("#F472B6"), ChatSky = Draw.Hex("#7DD3FC"), ChatLime = Draw.Hex("#A3E635");
        public static readonly Color[] ChatAvatars = { Teal, Ok, ChatViolet, Warn, ChatPink, Yellow, Green, ChatSky, ChatLime };

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
    }
}
