using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// 전광판 부품(대중교통 Extension 캔버스): LED · LCD 판 바탕 · 도트 무늬 · 한영 교차 글자 · 테두리 칸 · 흐르는 글 · 깜박임.
    /// 색은 Pal.Board* 만 씀. 움직임은 실제 판처럼 바로 바뀜(흐려지지 않음).
    /// </summary>
    public static class Board
    {
        /// <summary>판 모서리 · 도트 칸 · 도트 사이 선 굵기 · 테두리 칸 모서리 · 선 굵기 · 흐르는 글 속도(논리 px/초).</summary>
        public const float Radius = 16f, DotStep = 4f, DotLine = 1f, BoxRadius = 8f, BoxLine = 1.5f, TickSpeed = 60f;

        /// <summary>판 배치(캔버스 Main · Connections · ModularFlow): 역 묶기 1단계 판 폭 · 왼쪽 역 목록 폭 · 오른쪽 상세 폭 · 일본식 방면 안내판 폭 · 판 안 여백 · 제목 줄 · 칸 머리 줄 · 줄 · 흐르는 글 줄 · 칸 사이 · 작은 판(도착) 줄 · 칸 머리.</summary>
        public const float FlowW = 520f, RailW = 300f, SideW = 440f, SignW = 200f, Pad = 24f, HeadH = 64f, ColH = 32f, RowH = 48f, TickH = 40f, ColGap = 12f, SmallRowH = 40f, SmallHeadH = 48f, SmallColH = 28f;

        /// <summary>글 크기: 판 제목 · 줄 · 칸 머리 · 흐르는 글 · 작은 판 제목 · 작은 판 줄.</summary>
        public const float FontTitle = 26f, FontRow = 20f, FontHead = 13f, FontTick = 18f, FontSmallTitle = 18f, FontSmallRow = 16f;

        /// <summary>역 화면 틀(캔버스 Main): 왼쪽 목록 줄 · 목록 아이콘 · 머리 판 아이콘 칸 · 머리 판 아이콘 · 카드 안 여백 · 카드 모서리 · 노선 줄 · 노선 점.</summary>
        public const float RailRowH = 56f, RailIcon = 20f, Tile = 48f, TileIcon = 24f, CardPad = 14f, CardRadius = 16f, LineRowH = 60f, LineDot = 10f;

        /// <summary>대시보드 노선도(캔버스 Dashboard): 노선 줄 높이 · 왼쪽 노선 이름 칸 · 선 굵기 · 정류장 · 갈아타는 역 · 차량 칩 높이 · 줄 안 선 위치 · 정류장 이름 위치 · 현황 카드 줄 수 · 막대 굵기.</summary>
        public const float MapRowH = 88f, MapLabelW = 176f, MapTrack = 6f, MapStop = 16f, MapXfer = 20f, MapVehH = 16f, MapTrackY = 36f, MapNameY = 54f, MapVehY = 12f, BarH = 6f;
        public const int CardsPerRow = 4;

        /// <summary>테두리 칸 최소 폭(번호 한 자리).</summary>
        public const float BoxMinW = 36f;

        /// <summary>한글 · 영어를 7초씩 번갈아 · 깜박임 주기(초).</summary>
        public const float SwapSeconds = 7f, BlinkSeconds = 1f;

        /// <summary>지금 영어를 보일 차례(모든 판이 같은 박자).</summary>
        public static bool English => Mathf.FloorToInt(Time.realtimeSinceStartup / SwapSeconds) % 2 == 1;

        /// <summary>깜박임 켜진 차례.</summary>
        public static bool BlinkOn => Mathf.FloorToInt(Time.realtimeSinceStartup / (BlinkSeconds * 0.5f)) % 2 == 0;

        /// <summary>판 바탕(둥근 칸 + 1px 테두리).</summary>
        public static void Panel(Rect r, Color fill)
        {
            if (Draw.Repaint) Draw.Box(r, fill, Pal.BoardLedLine, Radius);
        }

        /// <summary>LED 도트 무늬: 3px 칸 사이에 어두운 격자(판 글을 그린 뒤에 덮음).</summary>
        public static void Dots(Rect r)
        {
            if (!Draw.Repaint) return;
            var shade = Pal.BoardDotShade;
            // 둥근 모서리 밖으로 나가지 않게 모서리 반지름 절반만큼 안쪽에만
            float m = Radius * 0.5f;
            var a = new Rect(r.x + m, r.y + m, r.width - m * 2f, r.height - m * 2f);
            for (float x = a.x + DotStep; x < a.xMax; x += DotStep) Draw.Fill(new Rect(x, a.y, DotLine, a.height), shade);
            for (float y = a.y + DotStep; y < a.yMax; y += DotStep) Draw.Fill(new Rect(a.x, y, a.width, DotLine), shade);
        }

        /// <summary>한영 교차 글자: en 이 비었으면 한글만.</summary>
        public static void Text(Rect r, string ko, string en, float size, Color c, bool bold = true, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Draw.Repaint) return;
            Draw.TextFit(r, English && !string.IsNullOrEmpty(en) ? en : ko, size, c, bold, anchor);
        }

        /// <summary>테두리 칸(타는곳 · 승차홈 · 탑승구 번호): 가운데 글 + 둥근 테두리.</summary>
        public static void Box(Rect r, string text, float size, Color c)
        {
            if (!Draw.Repaint) return;
            Draw.RoundOutline(r, c, BoxRadius, BoxLine);
            Draw.TextFit(r, text, size, c, true, TextAnchor.MiddleCenter);
        }

        /// <summary>흐르는 글: 오른쪽 끝에서 들어와 왼쪽으로 빠짐(r 안에서만 보임).</summary>
        public static void Ticker(Rect r, string text, float size, Color c)
        {
            if (!Draw.Repaint || string.IsNullOrEmpty(text)) return;
            float w = Draw.TextWidth(text, size, true);
            float travel = r.width + w;
            float x = r.width - Time.realtimeSinceStartup * TickSpeed % travel;
            Draw.BeginClip(r, Vector2.zero);
            Draw.Text(new Rect(x, 0f, w + DotStep, r.height), text, size, c, true, TextAnchor.MiddleLeft);
            Draw.EndClip();
        }
    }
}
