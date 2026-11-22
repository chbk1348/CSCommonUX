using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// 공연 그림 부품(콘서트 확장판 캔버스): 밤 무대 장면(트러스 · 조명 · LED 화면 · 바닥 · 관객) · 레코드판.
    /// 색은 Pal.Concert* 와 밴드 색(게임 공연 색)만 씀. 장면 좌표는 캔버스 무대 그림(824 × 340)을 칸 크기에 맞춰 늘림.
    /// </summary>
    public static class Concert
    {
        /// <summary>무대 장면 칸 높이 · 모서리 · 인기 순위 줄 높이 · 레코드판 반지름 · 좌석 점 · 점 사이.</summary>
        public const float StageW = 824f, StageH = 340f, StageRadius = 20f, RankRowH = 64f, VinylR = 88f, SeatDot = 11f, SeatGap = 4f;

        /// <summary>다음 공연 표 높이 · 표 왼쪽 밴드 색 띠 · 표 오른쪽 떼는 칸 폭 · 앨범 칸(밴드 머리글자) · 날짜 칸 · 지난 공연 줄 높이.</summary>
        public const float TicketH = 132f, Strip = 8f, StubW = 96f, Tile = 40f, DateW = 52f, DateH = 56f, PastRowH = 72f;

        /// <summary>큰 숫자 · 순위 숫자 · 1위 카드 그림 높이 · 인기 추이 칸 높이 · 명예의 전당 출입증 높이 · 관객 막대 굵기.</summary>
        public const float BigFont = 22f, RankFont = 24f, NameFont = 30f, HeroArtH = 200f, TrendH = 300f, PassH = 150f, BarH = 8f;

        /// <summary>예매 사이트 화면: 브라우저 머리 · 사이트 머리 · 포스터 · 카운트다운 칸 · 큰 단추 · 대기 카드 폭 · 사이트 로고 글 · 큰 대기 순번 글.</summary>
        public const float SiteChromeH = 44f, SiteHeadH = 60f, PosterW = 400f, PosterH = 560f, CountBox = 52f, SiteButtonH = 52f, WaitCardW = 640f, LogoFont = 22f, WaitFont = 48f;

        /// <summary>캔버스 무대 그림 크기(좌표 기준).</summary>
        const float W = StageW, H = StageH;

        /// <summary>조명 빛 · LED 덮개 · 바닥 띠 · 흰 빛의 투명도.</summary>
        const float BeamAlpha = 0.22f, WhiteBeamAlpha = 0.1f, LedTint = 0.08f, StripAlpha = 0.5f;

        /// <summary>사다리꼴 채우기(조명 빛 · 무대 바닥): 위 · 아래 가로 범위를 줄마다 이어 칠함.</summary>
        static void Trap(Rect r, float sx, float sy, float topY, float topL, float topR, float botY, float botL, float botR, Color c)
        {
            const float step = 3f;
            for (float y = topY; y < botY; y += step)
            {
                float t = (y - topY) / (botY - topY);
                float l = Mathf.Lerp(topL, botL, t), rr = Mathf.Lerp(topR, botR, t);
                float x0 = Mathf.Max(r.x, r.x + l * sx), x1 = Mathf.Min(r.xMax, r.x + rr * sx);
                if (x1 > x0) Draw.Fill(new Rect(x0, r.y + y * sy, x1 - x0, step * sy + 0.5f), c);
            }
        }

        static Rect At(Rect r, float sx, float sy, float x, float y, float w, float h) => new(r.x + x * sx, r.y + y * sy, w * sx, h * sy);

        /// <summary>밤 무대: 밴드 색 조명 둘 · 흰 조명 둘 · LED 화면에 밴드 이름 · 아래 한 줄 · 바닥 · 관객 실루엣과 휴대폰 불빛.</summary>
        public static void Stage(Rect r, Color band, string name, string sub)
        {
            if (!Draw.Repaint) return;
            float sx = r.width / W, sy = r.height / H;
            Draw.Round(r, Pal.ConcertScene, StageRadius);
            Draw.Fill(At(r, sx, sy, 96f, 22f, 632f, 10f), Pal.ConcertTruss);
            foreach (float x in new[] { 150f, 290f, 520f, 660f }) Draw.Round(At(r, sx, sy, x, 32f, 14f, 12f), Pal.ConcertLamp, 3f * sx);
            var beam = Draw.Alpha(band, BeamAlpha);
            Trap(r, sx, sy, 44f, 155f, 159f, 290f, 60f, 260f, beam);
            Trap(r, sx, sy, 44f, 665f, 669f, 290f, 560f, 770f, beam);
            var white = Draw.Alpha(Color.white, WhiteBeamAlpha);
            Trap(r, sx, sy, 44f, 295f, 299f, 290f, 250f, 420f, white);
            Trap(r, sx, sy, 44f, 525f, 529f, 290f, 410f, 580f, white);
            var led = At(r, sx, sy, 262f, 62f, 300f, 132f);
            Draw.Round(led, Pal.ConcertLed, 6f * sx);
            Draw.Round(led, Draw.Alpha(band, LedTint), 6f * sx);
            Draw.RoundOutline(led, Pal.ConcertLamp, 6f * sx);
            Draw.TextFit(At(r, sx, sy, 262f, 84f, 300f, 64f), name, 48f * sy, band, true, TextAnchor.MiddleCenter);
            Draw.TextFit(At(r, sx, sy, 262f, 152f, 300f, 28f), sub, 13f * sy, Pal.Light, false, TextAnchor.MiddleCenter);
            Trap(r, sx, sy, 228f, 120f, 704f, 256f, 80f, 744f, Pal.ConcertFloor);
            Draw.Fill(At(r, sx, sy, 80f, 256f, 664f, 6f), Draw.Alpha(band, StripAlpha));
            for (int i = 0; i < 18; i++)
            {
                float x = 40f + i * 44.5f, y = i % 2 == 0 ? 300f : 296f, rad = i % 3 == 2 ? 13f : 15f;
                Draw.Dot(new Vector2(r.x + x * sx, r.y + y * sy), rad * sy, Pal.ConcertCrowd);
            }
            Draw.Fill(new Rect(r.x, r.y + 306f * sy, r.width, r.yMax - (r.y + 306f * sy)), Pal.ConcertCrowd);
            foreach (var p in new[] { new Vector2(104f, 244f), new Vector2(374f, 238f), new Vector2(602f, 248f), new Vector2(230f, 268f), new Vector2(470f, 266f), new Vector2(700f, 270f) })
                Draw.Dot(new Vector2(r.x + p.x * sx, r.y + p.y * sy), 2.5f * sy, Pal.ConcertPhone);
        }

        /// <summary>레코드판: 검은 판 · 홈 다섯 줄 · 밴드 색 라벨에 글자 · 가운데 구멍.</summary>
        public static void Vinyl(Vector2 c, float radius, Color band, string label)
        {
            if (!Draw.Repaint) return;
            Draw.Dot(c, radius, Pal.ConcertVinyl);
            for (int i = 0; i < 5; i++) Draw.Ring(c, radius * (0.91f - i * 0.09f), 1f, Pal.ConcertGroove);
            float lr = radius * 0.39f;
            Draw.Dot(c, lr, band);
            Draw.TextFit(new Rect(c.x - lr, c.y - lr * 0.5f, lr * 2f, lr), label, lr * 0.6f, Draw.On(band), true, TextAnchor.MiddleCenter);
            Draw.Dot(c, radius * 0.045f, Pal.ConcertVinyl);
        }

        /// <summary>등급 색(0 VIP · 1 R · 2 S · 3 일반).</summary>
        public static Color Seat(int grade) => grade switch { 0 => Pal.ConcertSeatVip, 1 => Pal.ConcertSeatR, 2 => Pal.ConcertSeatS, _ => Pal.ConcertSeatA };

        /// <summary>포스터 종류 이름(캔버스 공연 포스터 10종).</summary>
        public static readonly string[] PosterKinds = { "조명", "레코드", "해 질 녘", "사선 띠", "하프톤", "음파", "커튼", "입장권", "울림", "글자 반복" };

        /// <summary>포스터 그림 좌표 기준(캔버스 300 × 420).</summary>
        const float PW = 300f, PH = 420f;

        /// <summary>
        /// 공연 포스터 10종(kind 0 ~ 9): 밤 바탕에 종류별 그림 · 위에 LIVE IN 도시 · 밴드 이름 · 날짜 · 공연장 · 아래 종류 이름.
        /// 공연을 잡을 때 모드가 무작위로 하나 골라 세이브에 남김.
        /// </summary>
        public static void Poster(Rect r, int kind, Color band, string name, string date, string venue, string city)
        {
            kind = Mathf.Clamp(kind, 0, PosterKinds.Length - 1);
            float sx = r.width / PW, sy = r.height / PH;
            Vector2 P(float x, float y) => new(r.x + x * sx, r.y + y * sy);
            if (Draw.Repaint)
            {
                Draw.Round(r, Pal.ConcertScene, StageRadius * 0.6f);
                switch (kind)
                {
                    case 0:
                        Trap(r, sx, sy, 0f, 58f, 62f, PH, -10f, 150f, Draw.Alpha(band, 0.35f));
                        Trap(r, sx, sy, 0f, 238f, 242f, PH, 150f, 310f, Draw.Alpha(band, 0.35f));
                        for (int i = 0; i < 5; i++) Draw.Dot(P(30f + i * 60f, i % 2 == 0 ? 400f : 394f), (i % 2 == 0 ? 20f : 22f) * sy, Pal.ConcertCrowd);
                        break;
                    case 1:
                        Vinyl(P(150f, 270f), 120f * sx, band, "");
                        break;
                    case 2:
                        for (float y = 200f; y <= 398f; y += 18f) Draw.Fill(new Rect(r.x, r.y + y * sy, r.width, 1.2f * sy), Draw.Alpha(band, 0.55f));
                        Draw.Dot(P(150f, 300f), 70f * sx, band);
                        foreach (var s in new[] { new Vector2(300f, 6f), new Vector2(316f, 5f), new Vector2(330f, 4f), new Vector2(342f, 3f) })
                            Draw.Fill(new Rect(r.x + 70f * sx, r.y + s.x * sy, 160f * sx, s.y * sy), Pal.ConcertScene);
                        break;
                    case 3:
                        Draw.Line(P(-40f, 260f), P(340f, 140f), 34f * sy, band);
                        Draw.Line(P(-40f, 310f), P(340f, 190f), 16f * sy, Draw.Alpha(band, 0.6f));
                        Draw.Line(P(-40f, 342f), P(340f, 222f), 8f * sy, Draw.Alpha(band, 0.35f));
                        break;
                    case 4:
                        for (int y = 0; y < 8; y++)
                            for (int x = 0; x < 10; x++)
                            {
                                float rad = Mathf.Max(0.6f, 11f - y * 1.4f - Mathf.Abs(x - 4.5f) * 0.5f);
                                Draw.Dot(P(15f + x * 30f, 196f + y * 28f), rad * sx, band);
                            }
                        break;
                    case 5:
                        for (int i = 0; i < 30; i++)
                        {
                            float h = 20f + Mathf.Abs(Mathf.Sin(i * 0.7f) * 90f + Mathf.Sin(i * 1.9f) * 30f);
                            Draw.Round(new Rect(r.x + (12f + i * 9.4f) * sx, r.y + (300f - h * 0.5f) * sy, 6f * sx, h * sy), band, 3f * sx);
                        }
                        break;
                    case 6:
                        Trap(r, sx, sy, 0f, 0f, 70f, PH, 0f, 90f, Pal.ConcertCurtain);
                        Trap(r, sx, sy, 0f, 230f, 300f, PH, 210f, 300f, Pal.ConcertCurtain);
                        Trap(r, sx, sy, 0f, 0f, 40f, PH, 0f, 52f, Pal.ConcertCurtainLight);
                        Trap(r, sx, sy, 0f, 260f, 300f, PH, 248f, 300f, Pal.ConcertCurtainLight);
                        Draw.Fill(new Rect(r.x, r.y, r.width, 26f * sy), Pal.ConcertCurtainLight);
                        Draw.Round(new Rect(r.x + 60f * sx, r.y + 386f * sy, 180f * sx, 28f * sy), Draw.Alpha(band, 0.5f), 14f * sy);
                        break;
                    case 7:
                        var t = new Rect(r.x + 34f * sx, r.y + 200f * sy, 232f * sx, 150f * sy);
                        Draw.RoundOutline(t, band, 14f * sx, 3f * sx);
                        Draw.Dot(P(34f, 275f), 14f * sx, Pal.ConcertScene);
                        Draw.Dot(P(266f, 275f), 14f * sx, Pal.ConcertScene);
                        Draw.Dashed(P(200f, 214f), P(200f, 336f), 2f * sx, band, 6f * sy, 6f * sy);
                        break;
                    case 8:
                        for (int i = 4; i >= 0; i--) Draw.Ring(P(150f, 290f), (40f + i * 30f) * sx, 2f * sx, Draw.Alpha(band, 0.8f - i * 0.17f));
                        Draw.Dot(P(150f, 290f), 14f * sx, band);
                        break;
                }
            }
            if (kind == 7) Draw.TextRotated(new Rect(r.x + 200f * sx, r.y + 214f * sy, 66f * sx, 122f * sy), "ADMIT ONE", 13f * sy, band, 90f, true);
            if (kind == 9)
                for (int i = 0; i < 5; i++) Draw.TextFit(new Rect(r.x - 6f * sx, r.y + (196f + i * 46f) * sy, r.width + 12f * sx, 46f * sy), name, 46f * sy, Draw.Alpha(band, 0.9f - i * 0.2f), true);
            float x0 = r.x + 28f * sx, w = r.width - 56f * sx, y0 = r.y + 40f * sy;
            Draw.TextFit(new Rect(x0, y0, w, 18f * sy), "LIVE IN " + city, 13f * sy, Pal.ConcertPosterSub, false);
            y0 += 24f * sy;
            float size = (name.Length > 12 ? 34f : name.Length > 8 ? 44f : 60f) * sy;
            Draw.TextFit(new Rect(x0, y0, w, size * 1.15f), name, size, band, true);
            y0 += size * 1.2f;
            Draw.TextFit(new Rect(x0, y0, w, 22f * sy), date, 16f * sy, Pal.Txt, true);
            y0 += 26f * sy;
            Draw.TextFit(new Rect(x0, y0, w, 18f * sy), venue, 13f * sy, Pal.ConcertPosterSub, false);
            Draw.TextFit(new Rect(x0, r.yMax - 32f * sy, w, 16f * sy), $"POSTER {kind + 1} · {PosterKinds[kind]}", 11f * sy, Pal.ConcertPosterSub, false);
        }

        /// <summary>의향서 문서 크기(캔버스 Letter 600 × 780, 좌표 기준) · 목록 줄 높이.</summary>
        public const float LetterW = 600f, LetterH = 780f, LetterRowH = 84f;

        /// <summary>의향서 내용(모드가 채움): 종류 0 공문 · 1 서한 · 2 레터헤드 · 3 메모, 밴드 색, 기획사 이름 · 영문, 문서 번호 · 날짜 · 받는 사람 · 제목 · 본문 줄 · 조건 이름 · 값 · 서명 · 도장 글(4자) · 해.</summary>
        public sealed class Doc
        {
            public int Kind;
            public Color Band;
            public string Agency = "", AgencyEn = "", City = "", No = "", Date = "", To = "", Subject = "", Sign = "", Seal = "", Year = "";
            public string[] Body = new string[0], Keys = new string[0], Vals = new string[0];
        }

        /// <summary>공연개최의향서 4종(캔버스 Letter): 0 한국 공문 · 둥근 직인 / 1 일본 서한 · 네모 각인 / 2 해외 레터헤드 · 금박 인장 / 3 인디 메모 · 고무 도장.</summary>
        public static void Letter(Rect r, Doc d)
        {
            int k = Mathf.Clamp(d.Kind, 0, 3);
            float s = r.width / LetterW;
            Color ink = Pal.LetterInk[k], sub = Pal.LetterSub[k], cell = Pal.LetterCell[k], line = Pal.LetterLine[k];
            Rect R(float x, float y, float w, float h) => new(r.x + x * s, r.y + y * s, w * s, h * s);
            void T(float x, float y, float w, float h, string t, float size, Color c, bool bold = false, TextAnchor a = TextAnchor.MiddleLeft) => Draw.TextFit(R(x, y, w, h), t ?? "", size * s, c, bold, a);
            void Fill(float x, float y, float w, float h, Color c) { if (Draw.Repaint) Draw.Fill(R(x, y, w, h), c); }
            Vector2 P(float x, float y) => new(r.x + x * s, r.y + y * s);
            int n = Mathf.Min(d.Keys.Length, d.Vals.Length);
            if (Draw.Repaint) Draw.Round(r, Pal.LetterPaper[k], 6f * s);
            float y;
            switch (k)
            {
                case 0:
                    if (Draw.Repaint) Draw.Round(R(48f, 44f, 40f, 40f), d.Band, 8f * s);
                    T(100f, 42f, 300f, 26f, d.Agency, 18f, ink, true);
                    T(100f, 66f, 300f, 18f, d.AgencyEn, 11f, sub);
                    Fill(48f, 96f, 504f, 3f, ink);
                    string[] heads = { "문서번호", "시행일자", "수  신", "참  조", "제  목" }, cols = { d.No, d.Date, d.To, "문화관광과장", d.Subject };
                    for (int i = 0; i < heads.Length; i++)
                    {
                        T(48f, 112f + i * 22f, 72f, 20f, heads[i], 13f, sub);
                        T(132f, 112f + i * 22f, 420f, 20f, cols[i], 13f, ink, i == 4);
                    }
                    Fill(48f, 228f, 504f, 1f, ink);
                    y = 240f;
                    foreach (var b in d.Body) { T(48f, y, 504f, 26f, b, 14f, ink); y += 26f; }
                    T(48f, y + 6f, 504f, 24f, "- 아 래 -", 14f, ink, true, TextAnchor.MiddleCenter);
                    y += 40f;
                    for (int i = 0; i < n; i++)
                    {
                        var row = R(48f, y, 504f, 36f);
                        if (Draw.Repaint)
                        {
                            Draw.Fill(R(48f, y, 110f, 36f), cell);
                            Draw.Border(row, line);
                            Draw.Fill(R(158f, y, 1f, 36f), line);
                        }
                        T(58f, y, 96f, 36f, d.Keys[i], 13f, ink, true);
                        T(168f, y, 380f, 36f, d.Vals[i], 13f, ink);
                        y += 36f;
                    }
                    T(48f, y + 10f, 504f, 22f, "붙임 · 공연 기획안 1부. 끝.", 13f, sub);
                    T(0f, 676f, 600f, 34f, d.Agency + " 대표이사", 24f, ink, true, TextAnchor.MiddleCenter);
                    if (Draw.Repaint)
                    {
                        var c = P(452f, 692f);
                        Draw.Ring(c, 48f * s, 4f * s, Pal.LetterSeal);
                        Draw.Ring(c, 38f * s, 1.5f * s, Pal.LetterSeal);
                    }
                    T(404f, 662f, 96f, 14f, d.Seal, 9f, Pal.LetterSeal, true, TextAnchor.MiddleCenter);
                    T(404f, 676f, 96f, 34f, "印", 26f, Pal.LetterSeal, true, TextAnchor.MiddleCenter);
                    T(404f, 708f, 96f, 14f, "대표이사인", 9f, Pal.LetterSeal, true, TextAnchor.MiddleCenter);
                    break;
                case 1:
                    T(52f, 44f, 250f, 18f, d.No, 12f, sub);
                    T(298f, 44f, 250f, 18f, d.Date, 12f, sub, false, TextAnchor.MiddleRight);
                    T(52f, 76f, 496f, 20f, d.To + " 귀하", 13f, ink);
                    T(52f, 110f, 496f, 38f, "公演開催意向書", 26f, ink, true, TextAnchor.MiddleCenter);
                    T(52f, 150f, 496f, 20f, "공연 개최 의향서", 14f, sub, false, TextAnchor.MiddleCenter);
                    y = 192f;
                    foreach (var b in d.Body) { T(52f, y, 496f, 26f, b, 14f, ink); y += 26f; }
                    y += 14f;
                    Fill(52f, y, 496f, 2f, ink);
                    for (int i = 0; i < n; i++)
                    {
                        T(56f, y + 2f, 110f, 38f, d.Keys[i], 13f, sub);
                        T(166f, y + 2f, 380f, 38f, d.Vals[i], 13f, ink, true);
                        y += 40f;
                        if (Draw.Repaint && i < n - 1) Draw.Dashed(P(52f, y), P(548f, y), 1f * s, line, 2f * s, 3f * s);
                    }
                    Fill(52f, y, 496f, 2f, ink);
                    T(52f, y + 12f, 496f, 22f, "敬具", 14f, ink, false, TextAnchor.MiddleRight);
                    T(200f, 642f, 240f, 26f, d.Agency, 18f, ink, true, TextAnchor.MiddleRight);
                    T(200f, 668f, 240f, 18f, "代表取締役 · 대표", 12f, sub, false, TextAnchor.MiddleRight);
                    if (Draw.Repaint)
                    {
                        Draw.RoundOutline(R(456f, 636f, 92f, 92f), Pal.LetterSeal, 6f * s, 5f * s);
                        Draw.Border(R(464f, 644f, 76f, 76f), Pal.LetterSeal, 1.5f * s);
                    }
                    for (int i = 0; i < 4 && i < d.Seal.Length; i++)
                        T(468f + (i % 2) * 34f, 648f + (i / 2) * 34f, 34f, 34f, d.Seal.Substring(i, 1), 24f, Pal.LetterSeal, true, TextAnchor.MiddleCenter);
                    break;
                case 2:
                    if (Draw.Repaint)
                    {
                        Draw.Round(R(0f, 0f, 600f, 96f), Pal.LetterHead, 6f * s);
                        Draw.Fill(R(0f, 88f, 600f, 8f), Pal.LetterHead);
                        Draw.Ring(P(64f, 48f), 18f * s, 3f * s, d.Band);
                        Draw.Line(P(52f, 54f), P(64f, 38f), 3f * s, d.Band);
                        Draw.Line(P(64f, 38f), P(76f, 54f), 3f * s, d.Band);
                    }
                    T(100f, 26f, 460f, 28f, d.AgencyEn, 20f, Pal.LetterPaper[2], true);
                    T(100f, 54f, 460f, 18f, "LIVE · TOURING · MANAGEMENT", 11f, Pal.LetterHeadSub);
                    T(44f, 128f, 512f, 18f, d.Date + " · Ref. " + d.No, 12f, sub);
                    T(44f, 154f, 512f, 20f, "To the Mayor of " + d.City, 13f, ink);
                    T(44f, 174f, 512f, 20f, d.To + "님께", 13f, ink);
                    T(44f, 204f, 512f, 32f, d.Subject, 22f, ink, true);
                    y = 244f;
                    foreach (var b in d.Body) { T(44f, y, 512f, 24f, b, 14f, sub); y += 24f; }
                    y += 12f;
                    for (int i = 0; i < n; i++)
                    {
                        float cx = 44f + (i % 2) * 261f, cy = y + (i / 2) * 68f;
                        if (Draw.Repaint) Draw.Round(R(cx, cy, 251f, 58f), cell, 10f * s);
                        T(cx + 14f, cy + 10f, 223f, 16f, d.Keys[i], 11f, sub);
                        T(cx + 14f, cy + 28f, 223f, 22f, d.Vals[i], 14f, ink, true);
                    }
                    T(44f, 640f, 260f, 40f, d.Sign, 30f, Pal.LetterHead);
                    Fill(44f, 690f, 200f, 1f, ink);
                    T(44f, 698f, 300f, 18f, "Head of Touring, " + d.AgencyEn, 12f, sub);
                    if (Draw.Repaint)
                    {
                        var c = P(490f, 670f);
                        Draw.Line(P(470f, 712f), P(460f, 742f), 12f * s, Pal.LetterGoldDark);
                        Draw.Line(P(510f, 712f), P(520f, 742f), 12f * s, Pal.LetterGoldDark);
                        Draw.Dot(c, 50f * s, Pal.LetterGold);
                        Draw.Ring(c, 42f * s, 1.5f * s, Pal.LetterGoldLight);
                        Draw.Ring(c, 34f * s, 1.5f * s, Pal.LetterGoldLight);
                        var star = new System.Collections.Generic.List<Vector2>();
                        for (int i = 0; i < 10; i++)
                        {
                            float a = Mathf.PI * (-0.5f + i * 0.2f), rad = (i % 2 == 0 ? 22f : 9f) * s;
                            star.Add(new Vector2(c.x + Mathf.Cos(a) * rad, c.y + Mathf.Sin(a) * rad));
                        }
                        Draw.Polyline(star, 3f * s, Pal.LetterGoldLight, true);
                    }
                    break;
                default:
                    T(52f, 44f, 340f, 40f, d.Agency.ToLowerInvariant(), 30f, ink, true);
                    T(392f, 52f, 156f, 30f, "independent label", 12f, ink, false, TextAnchor.MiddleRight);
                    Fill(52f, 92f, 496f, 2f, ink);
                    string[] memo = { "TO: " + d.To + "님", "FROM: " + d.Agency, "RE: " + d.Subject, "DATE: " + d.Date };
                    for (int i = 0; i < memo.Length; i++) T(52f, 108f + i * 22f, 496f, 22f, memo[i], 13f, ink);
                    y = 210f;
                    foreach (var b in d.Body) { T(52f, y, 496f, 24f, b, 13.5f, ink); y += 24f; }
                    y += 10f;
                    for (int i = 0; i < n; i++)
                    {
                        T(52f, y, 120f, 22f, "[" + d.Keys[i] + "]", 13f, ink);
                        T(172f, y, 376f, 22f, d.Vals[i], 13f, ink, true);
                        y += 30f;
                    }
                    T(52f, y + 8f, 496f, 22f, d.Sign, 13f, ink);
                    var stamp = R(378f, 618f, 170f, 100f);
                    if (Draw.Repaint)
                    {
                        Draw.RoundOutline(stamp, d.Band, 10f * s, 5f * s);
                        Draw.Fill(R(420f, 615f, 6f, 8f), Pal.LetterPaper[3]);
                        Draw.Fill(R(500f, 713f, 8f, 8f), Pal.LetterPaper[3]);
                    }
                    T(378f, 636f, 170f, 36f, d.Agency.ToLowerInvariant(), 24f, d.Band, true, TextAnchor.MiddleCenter);
                    T(378f, 672f, 170f, 22f, "OFFICIAL · " + d.Year, 13f, d.Band, true, TextAnchor.MiddleCenter);
                    break;
            }
        }
    }
}
