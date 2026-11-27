using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 공통 아이콘 새 45개(기존 14 와 합쳐 59, FIA 캔버스 R5TokIcon) + 3.6 탭 화면 15개(74). 상자 24 · 획 2 하나 · 끝 · 이음 둥글게 · 채운 아이콘 없음.
    /// 경로 글은 원본 글자 그대로(공백 하나도 바꾸지 않음 — 옮긴 모드의 화소 · 굽기 캐시 키가 같게). 그릴 때는 Paint.
    /// </summary>
    public static partial class Icons
    {
        // ---- 동작
        /// <summary>다시 실행.</summary>
        public const string Redo = "M21 10H11a5 5 0 0 0-5 5v2M21 10l-5-5M21 10l-5 5";
        /// <summary>고치기.</summary>
        public const string Edit = "M4 20h4L19 9l-4-4L4 16zM13.5 6.5l4 4";
        /// <summary>더 보기.</summary>
        public const string More = "M4.5 12a1 1 0 1 0 2 0a1 1 0 1 0-2 0 M11 12a1 1 0 1 0 2 0a1 1 0 1 0-2 0 M17.5 12a1 1 0 1 0 2 0a1 1 0 1 0-2 0";
        /// <summary>재생.</summary>
        public const string Play = "M7.5 5.5v13l11-6.5z";
        /// <summary>멈춤.</summary>
        public const string Pause = "M8.5 5.5v13 M15.5 5.5v13";
        /// <summary>정지.</summary>
        public const string Stop = "M7 7h10v10H7z";
        /// <summary>클릭.</summary>
        public const string Pointer = "M9 9l11 4-5 2-2 5zM4 4l2 2M10 3v3M3 10h3";

        // ---- 탐색
        /// <summary>위 · 오름.</summary>
        public const string Up = "M6 15l6-6 6 6";
        /// <summary>왼쪽으로.</summary>
        public const string ArrowLeft = "M19 12H5M11 6l-6 6 6 6";
        /// <summary>다음 · 이동.</summary>
        public const string ArrowRight = "M5 12h14M13 6l6 6-6 6";
        /// <summary>홈.</summary>
        public const string Home = "M3 10.5 12 4l9 6.5V20H3z";
        /// <summary>격자.</summary>
        public const string Grid = "M4 4h7v7H4zM13 4h7v7h-7zM4 13h7v7H4zM13 13h7v7h-7z";
        /// <summary>점 목록.</summary>
        public const string List = "M8 6h13M8 12h13M8 18h13M3 6h.01M3 12h.01M3 18h.01";
        /// <summary>글 목록.</summary>
        public const string Lines = "M4 6h16M4 12h16M4 18h10";
        /// <summary>설정.</summary>
        public const string Sliders = "M5 7h9M18 7h1M5 17h1M10 17h9M14 5v4M8 15v4";
        /// <summary>창.</summary>
        public const string Window = "M3 5h18v14H3zM3 9h18";

        // ---- 상태
        /// <summary>잠김.</summary>
        public const string Lock = "M7 11V8a5 5 0 0 1 10 0v3 M5 11h14v10H5z";
        /// <summary>알림.</summary>
        public const string Bell = "M6 16V11a6 6 0 0 1 12 0v5l2 2H4zM10 21h4";
        /// <summary>진단.</summary>
        public const string Bug = "M8 8a4 4 0 0 1 8 0v8a4 4 0 0 1-8 0zM4 12h4M16 12h4M5 7l3 2M19 7l-3 2M5 17l3-2M19 17l-3-2";
        /// <summary>보기.</summary>
        public const string Eye = "M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12z M9 12a3 3 0 1 0 6 0a3 3 0 1 0-6 0";
        /// <summary>실시간.</summary>
        public const string Pulse = "M2.5 12h4l2.5-6 4.5 12 2.5-6h5.5";
        /// <summary>오름세.</summary>
        public const string Trend = "M4 17l6-6 4 4 6-6M14 9h6v6";
        /// <summary>통계.</summary>
        public const string Bars = "M4 20V10M10 20V4M16 20v-8M22 20H2";

        // ---- 사물
        /// <summary>달력.</summary>
        public const string Calendar = "M5 4.5h14a2 2 0 0 1 2 2v13a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-13a2 2 0 0 1 2-2zM3 10h18M8 2.5v4M16 2.5v4";
        /// <summary>시간.</summary>
        public const string Clock = "M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18zM12 7v5l3 2";
        /// <summary>위치.</summary>
        public const string Pin = "M12 21s-6.5-5.8-6.5-11.2a6.5 6.5 0 0 1 13 0C18.5 15.2 12 21 12 21zM12 12.3a2.5 2.5 0 1 0 0-5 2.5 2.5 0 0 0 0 5z";
        /// <summary>지도.</summary>
        public const string Map = "M14.1 5.55a2 2 0 0 0 1.79 0l3.66-1.83A1 1 0 0 1 21 4.62v12.76a1 1 0 0 1-.55.9l-4.56 2.27a2 2 0 0 1-1.78 0l-4.22-2.1a2 2 0 0 0-1.78 0l-3.66 1.83A1 1 0 0 1 3 19.38V6.62a1 1 0 0 1 .55-.9l4.56-2.27a2 2 0 0 1 1.78 0z M15 5.76v15 M9 3.24v15";
        /// <summary>도로 · 트랙.</summary>
        public const string Road = "M8 3L5 21M16 3l3 18M12 5v3M12 11v3M12 17v3";
        /// <summary>버스.</summary>
        public const string Bus = "M6 3h12a2 2 0 0 1 2 2v12H4V5a2 2 0 0 1 2-2zM4 11h16M7 17v3M17 17v3";
        /// <summary>사람.</summary>
        public const string User = "M12 12a4 4 0 1 0 0-8 4 4 0 0 0 0 8z M5 21a7 7 0 0 1 14 0";
        /// <summary>팀.</summary>
        public const string Users = "M9 11a3 3 0 1 0 0-6 3 3 0 0 0 0 6zM3 20v-1a4 4 0 0 1 4-4h4a4 4 0 0 1 4 4v1M17 8a3 3 0 0 1 0 6";
        /// <summary>깃발.</summary>
        public const string Flag = "M6 21V4M6 5h11l-2 4 2 4H6";
        /// <summary>우승.</summary>
        public const string Trophy = "M8 21h8M12 17v4M7 4h10v5a5 5 0 0 1-10 0zM17 6h3v2a3 3 0 0 1-3 3M7 6H4v2a3 3 0 0 0 3 3";
        /// <summary>즐겨찾기.</summary>
        public const string Star = "M12 3l2.6 5.6 6 .7-4.5 4.1 1.2 6L12 16.4 6.7 19.4l1.2-6L3.4 9.3l6-.7z";
        /// <summary>안전.</summary>
        public const string Shield = "M12 3l8 3v6c0 5-3.5 8-8 9-4.5-1-8-4-8-9V6z";
        /// <summary>전기 · 빠름.</summary>
        public const string Bolt = "M13 3L5 13h6l-1 8 8-10h-6z";
        /// <summary>팁.</summary>
        public const string Bulb = "M9 18h6M10 21h4M12 3a6 6 0 0 0-3.5 10.9c.6.5 1 1.2 1 2.1h5c0-.9.4-1.6 1-2.1A6 6 0 0 0 12 3z";
        /// <summary>소리.</summary>
        public const string Volume = "M4 9h4l5-4v14l-5-4H4zM16 9a4 4 0 0 1 0 6M19 6a8 8 0 0 1 0 12";
        /// <summary>음악.</summary>
        public const string Music = "M9 18V5l12-2v13M6 21a3 3 0 1 0 0-6 3 3 0 0 0 0 6zM18 19a3 3 0 1 0 0-6 3 3 0 0 0 0 6z";
        /// <summary>메시지.</summary>
        public const string Mail = "M4 5h16v14H4zM4 7l8 6 8-6";
        /// <summary>말풍선.</summary>
        public const string Chat = "M3 4h18v12H11l-5 5v-5H3z";
        /// <summary>입장권.</summary>
        public const string Ticket = "M3 9a2 2 0 0 0 0 6v3h18v-3a2 2 0 0 1 0-6V6H3zM14 6v12";
        /// <summary>돈 · 예산.</summary>
        public const string Coin = "M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18zM9 9h6M9 15h6M12 6v12";
        /// <summary>인기.</summary>
        public const string Flame = "M12 3c1 4 5 5 5 10a5 5 0 0 1-10 0c0-3 2-4 2-7 2 1 3 2 3 4 1-2 1-4 0-7z";
        /// <summary>좋아요.</summary>
        public const string Heart = "M12 20s-7-4.4-7-9.6A3.9 3.9 0 0 1 12 8a3.9 3.9 0 0 1 7 2.4C19 15.6 12 20 12 20z";

        // ---- 3.6 탭 화면(대시보드 · 서킷 · 레이스 · 팬 · 팀 · 설정 보드 경로 그대로)
        /// <summary>오류(원 + 느낌표 — 지우기 모달 · 오류 토스트).</summary>
        public const string Alert = "M12 21a9 9 0 1 0 0-18 9 9 0 0 0 0 18zM12 8v5M12 16h.01";
        /// <summary>기사.</summary>
        public const string Article = "M6 3h9l4 4v14H6zM14 3v5h5M9 13h7M9 17h5";
        /// <summary>문서(경기 규칙).</summary>
        public const string Doc = "M6 3h9l3 3v15H6zM9 9h6M9 13h6M9 17h4";
        /// <summary>채널(해시).</summary>
        public const string Hash = "M5 9h14M5 15h14M10 4L8 20M16 4l-2 16";
        /// <summary>갈래(스레드).</summary>
        public const string Branch = "M6 4v8a4 4 0 0 0 4 4h8M14 12l4 4-4 4";
        /// <summary>웃는 얼굴(반응).</summary>
        public const string Smile = "M12 21a9 9 0 1 0 0-18 9 9 0 0 0 0 18zM8.5 14a4 4 0 0 0 7 0M9 9.5h.01M15 9.5h.01";
        /// <summary>맞바꿈(이적).</summary>
        public const string Swap = "M7 7h12l-3-3M17 17H5l3 3";
        /// <summary>서킷 툴킷(런처).</summary>
        public const string Toolkit = "M7 21L10 3M17 21L14 3M12 7v2M12 13v2";
        /// <summary>카메라(선두 따라가기).</summary>
        public const string Camera = "M3 7.5h11.5v9H3zM14.5 10.5 20.5 7v10l-6-3.5";
        /// <summary>구름(날씨).</summary>
        public const string Cloud = "M7 18a4.5 4.5 0 1 1 .9-8.9A6 6 0 0 1 19.5 11 3.5 3.5 0 0 1 18 18z";
        /// <summary>체커 깃발(지난 레이스).</summary>
        public const string Chequer = "M4 21V4M4 4h6v6H4M10 4h6v6h-6M10 10h6v6h-6M4 10h6v6H4";
        /// <summary>빈 원(할 일 — 길잡이 · 시설 하나로, 반지름 8).</summary>
        public const string Circle = "M12 20a8 8 0 1 0 0-16 8 8 0 0 0 0 16z";
        /// <summary>참고(원 + 느낌표).</summary>
        public const string NoteCircle = "M12 8v5M12 16v.5M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18z";
        /// <summary>겹침(두 원).</summary>
        public const string Rings = "M8 12a4 4 0 1 0 8 0 4 4 0 0 0-8 0M4 12a8 8 0 1 0 16 0 8 8 0 0 0-16 0";
        /// <summary>버스 앞모습(도시 셔틀).</summary>
        public const string BusFront = "M6 17h12M7 17V6a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v11M7 11h10M8 20l1-3M16 20l-1-3";

        /// <summary>
        /// 3.5 아이콘 그리기: 칸 폭으로 크기 단계(14 미만 12 · 17 미만 16 · 21 미만 18 · 그 밖 24)를 골라 칸 가운데에 획 2(상자 24 기준, 화면 획 = 크기 ÷ 12)로 그림.
        /// 색은 옆 글 역할(InkTxt · InkSub · InkDim)을 넘김. 아이콘은 누름 · 진입 배율을 받지 않게 BeginScale 밖에서 부를 것.
        /// </summary>
        public static void Paint(string d, Rect r, Color c)
        {
            float s = Stage(r.width);
            Draw.Icon(d, new Rect(r.x + (r.width - s) * 0.5f, r.y + (r.height - s) * 0.5f, s, s), c, CSCommonDesign35.IconBox, CSCommonDesign35.IconLine);
        }

        /// <summary>칸 폭 → 아이콘 크기 단계(12 · 16 · 18 · 24).</summary>
        public static float Stage(float width) =>
            width < 14f ? CSCommonDesign35.IconXs : width < 17f ? CSCommonDesign35.IconS : width < 21f ? CSCommonDesign35.IconM : CSCommonDesign35.IconL;
    }
}
