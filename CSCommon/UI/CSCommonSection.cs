using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Section — 시안 'Section'(라이트 · 다크): 바탕 창 띠(우물 · 반지름 24 · 안쪽 12 · 플레이트 사이 12) 안에 둥근 플레이트
    /// (반지름 24 · 히어로 28 · 안쪽 16 / 아래 12 · 면 + 안쪽 1px 선 · 그림자 없음). 제목 = 모드 파스텔 알약 칩 32(아이콘 16 + 이름 13.5 굵게, 누르지 않는 표시용),
    /// 보조 = 회색 알약 28, 줄 = 알약 행 44(호버 Field 면 · 순번 원 28), 헤어라인 없음. 몸통은 늘 펼침(접기 없음).
    /// </summary>
    public static class Section
    {
        // ---------------- 치수(시안 그대로)

        /// <summary>우물(바탕 창 띠): 반지름 · 안쪽 여백 · 플레이트 사이.</summary>
        public const float WellRadius = 24f, WellPad = 12f, Gap = 12f;
        /// <summary>플레이트: 반지름 · 히어로 반지름 · 안쪽(좌우 · 위) · 아래 · 머리 아래 간격.</summary>
        public const float Radius = 24f, HeroRadius = 28f, Pad = 16f, PadBottom = 12f, HeadGap = 8f;
        /// <summary>제목 칩: 높이 · 왼쪽(아이콘 있을 때) · 오른쪽 · 칸 사이 · 아이콘 · 글 · 선 굵기 · 두 줄 칩 위아래 안쪽.</summary>
        public const float ChipH = 32f, ChipPadL = 12f, ChipPadR = 16f, ChipGap = 8f, ChipIcon = 16f, ChipFont = 13.5f, ChipStroke = 2f, ChipWrapPadY = 7f;
        /// <summary>보조 알약: 높이 · 좌우 · 글.</summary>
        public const float SubH = 28f, SubPadX = 12f, SubFont = 12.5f;
        /// <summary>알약 행: 높이 · 왼쪽(순번 원 앞) · 오른쪽 · 칸 사이 · 글 · 순번 원 · 순번 글.</summary>
        public const float RowH = 44f, RowPadL = 8f, RowPadR = 18f, RowGap = 12f, RowFont = 14f, RankD = 28f, RankFont = 12.5f;
        /// <summary>움직임: 행 호버(선형).</summary>
        public const float HoverSeconds = 0.12f;

        // ---------------- 색(시안 값, 테마를 따라감 — 칩 · 행 글 · 순번은 Look 의 파스텔 · 알약 토큰)

        static readonly Color WellLight = Draw.Hex("#EEF0F2"), WellDark = Draw.Hex("#0E1014");
        static readonly Color PlateLight = Draw.Hex("#FEFEFE"), PlateDark = Draw.Hex("#1B1F26");
        static readonly Color LineLight = Draw.Hex("#E4E6EA"), LineDark = Draw.Hex("#2A2F38");
        static readonly Color SubInkLight = Draw.Hex("#5F6368"), SubInkDark = Draw.Hex("#9AA3AF");

        public static Color WellFill => CSCommonTheme.Light ? WellLight : WellDark;
        public static Color Plate => CSCommonTheme.Light ? PlateLight : PlateDark;
        public static Color PlateLine => CSCommonTheme.Light ? LineLight : LineDark;
        public static Color SubInk => CSCommonTheme.Light ? SubInkLight : SubInkDark;

        // ---------------- 판

        /// <summary>우물(바탕 창 띠): 반지름 24 면.</summary>
        public static void Well(Rect r)
        {
            if (Draw.Repaint) Draw.Round(r, WellFill, WellRadius);
        }

        /// <summary>
        /// 플레이트: 면 + 안쪽 1px 선(반지름 24). fill 이 있으면 히어로(그 면 · 반지름 28 · 선 없음, 투명이면 안 그림).
        /// stripe = 예전 위 색 띠(상태 색) — 이제 플레이트 1px 선 색으로만 남김.
        /// </summary>
        public static void Frame(Rect r, Color? fill = null, Color? stripe = null)
        {
            if (!Draw.Repaint) return;
            if (fill.HasValue)
            {
                if (fill.Value.a > 0f) Draw.Round(r, fill.Value, HeroRadius);
                return;
            }
            Draw.Round(r, Plate, Radius);
            Draw.RoundOutline(r, stripe ?? PlateLine, Radius);
        }

        // ---------------- 머리(제목 칩 · 보조 알약)

        /// <summary>제목 칩 폭(글 크기 font · 아이콘 있으면 그 자리 포함). 글 폭은 굵게 · 보통 중 넓은 값(Draw.TextWidth).</summary>
        public static float ChipWidth(string title, string icon, float font = ChipFont) => ChipLead(icon) + Mathf.Ceil(Draw.TextWidth(title ?? "", font, true)) + ChipPadR;

        /// <summary>칩 왼쪽(글 앞) 폭: 아이콘이 없으면 오른쪽과 같은 여백.</summary>
        static float ChipLead(string icon) => string.IsNullOrEmpty(icon) ? ChipPadR : ChipPadL + ChipIcon + ChipGap;

        /// <summary>폭 width 안 칩 글 크기: 한 줄에 들어가면 ChipFont, 아니면 한 단계(Draw.PillFontStep) 작게.</summary>
        static float ChipFontFor(string title, string icon, float width) => ChipWidth(title, icon) <= width + Draw.FitSlack ? ChipFont : ChipFont - Draw.PillFontStep;

        /// <summary>
        /// 제목 줄 높이(폭 width): 한 줄이면 ChipH, 한 단계 작은 글로도 넘치면 두 줄 이상 높이(말줄임 없음). 카드 높이 재기 · 제목 칸에 씀.
        /// 보조 알약은 자리가 모자라면 빠지므로(제목 우선) 높이에 넣지 않음.
        /// </summary>
        public static float HeadHeight(string title, float width, string icon = null)
        {
            if (string.IsNullOrEmpty(title)) return 0f;
            float font = ChipFontFor(title, icon, width);
            if (ChipWidth(title, icon, font) <= width + Draw.FitSlack) return ChipH;
            float room = Mathf.Max(1f, width - ChipLead(icon) - ChipPadR);
            return Mathf.Max(ChipH, Mathf.Ceil(Draw.TextHeight(title, font, room, true)) + ChipWrapPadY * 2f);
        }

        /// <summary>
        /// 제목 줄(누르지 않는 칩): 한 줄이면 r 세로 가운데, 두 줄이면 r 위에 파스텔 칩 · 오른쪽 끝에 회색 보조 알약.
        /// 자리 우선 = 제목 &gt; 보조: 제목 칩이 보조 알약 옆에 다 안 들어가면 보조 알약을 숨김. 그래도 길면 글 한 단계 → 두 줄(높이 = HeadHeight).
        /// </summary>
        public static void Title(Rect r, string title, string sub = null, string icon = null)
        {
            if (!string.IsNullOrEmpty(sub) && ChipWidth(title, icon) > r.width - SubPillWidth(r, sub) - ChipGap + Draw.FitSlack) sub = null;
            float sw = SubPill(r, sub);
            if (!Draw.Repaint || string.IsNullOrEmpty(title)) return;
            float room = Mathf.Max(0f, r.width - sw), font = ChipFontFor(title, icon, room), h = HeadHeight(title, room, icon);
            var c = new Rect(r.x, h > ChipH ? r.y : Mathf.Round(r.center.y - ChipH * 0.5f), Mathf.Min(ChipWidth(title, icon, font), room), h);
            Draw.Round(c, Look.PastelFill, ChipH * 0.5f);
            float x = c.x + ChipLead(icon);
            if (!string.IsNullOrEmpty(icon))
                Draw.Icon(icon, new Rect(c.x + ChipPadL, Mathf.Round(c.center.y - ChipIcon * 0.5f), ChipIcon, ChipIcon), Look.PastelInk, 24f, ChipStroke);
            var tr = new Rect(x, c.y, Mathf.Max(0f, c.xMax - ChipPadR - x), c.height);
            // 한 줄 = 칸 폭이 글 폭이라 그대로(Draw.FitSlack 가 소수 x 오차를 흡수), 두 줄 = 줄바꿈
            if (h > ChipH) Draw.Text(new Rect(tr.x, tr.y + ChipWrapPadY, tr.width, tr.height - ChipWrapPadY * 2f), title, font, Look.PastelInk, true, TextAnchor.MiddleLeft, true);
            else Draw.TextFit(tr, title, font, Look.PastelInk, true);
        }

        /// <summary>회색 보조 알약 폭(r 의 45% 까지).</summary>
        static float SubPillWidth(Rect r, string sub) => Mathf.Min(r.width * 0.45f, Mathf.Ceil(Draw.TextWidth(sub, SubFont)) + SubPadX * 2f);

        /// <summary>회색 보조 알약(오른쪽 끝, r 의 45% 까지). 차지한 폭(+ 칸 사이)을 돌려줌, 없으면 0.</summary>
        static float SubPill(Rect r, string sub)
        {
            if (string.IsNullOrEmpty(sub)) return 0f;
            float w = SubPillWidth(r, sub);
            if (Draw.Repaint)
            {
                var p = new Rect(r.xMax - w, Mathf.Round(r.center.y - SubH * 0.5f), w, SubH);
                Draw.Round(p, Look.Pill, SubH * 0.5f);
                Draw.TextFit(new Rect(p.x + SubPadX, p.y, p.width - SubPadX * 2f, p.height), sub, SubFont, SubInk, false, TextAnchor.MiddleCenter);
            }
            return w + ChipGap;
        }

        // ---------------- 알약 행

        /// <summary>알약 행 바탕: 호버 면 없음(자리만 유지 — 호출부 호환).</summary>
        public static void Row(Rect r, bool hoverable = true)
        { /* 호버 면 없음 — 줄은 면 없이 글 · 부품만 */         }

        /// <summary>행 왼쪽 순번 원(28 · Pill 면 · 12.5 BarOff 글). 다음 글이 시작할 x 를 돌려줌.</summary>
        public static float Rank(Rect row, int n)
        {
            var c = new Rect(row.x + RowPadL, Mathf.Round(row.center.y - RankD * 0.5f), RankD, RankD);
            if (Draw.Repaint)
            {
                Draw.Round(c, Look.Pill, RankD * 0.5f);
                Draw.Text(c, n.ToString(), RankFont, Look.BarOff, false, TextAnchor.MiddleCenter);
            }
            return c.xMax + RowGap;
        }

        /// <summary>
        /// 가는 구분선(Look.Divider 1px, 들임 inset). 새 Section 안 줄 사이엔 쓰지 않음(헤어라인 없음) —
        /// 섹션이 아닌 곳(대화 날짜 줄 · 떠 있는 판 발 · 표 칸)의 예전 호출이 남아 있어 둠.
        /// </summary>
        public static void Hairline(Rect r, float inset = 0f)
        {
            if (Draw.Repaint) Draw.Fill(new Rect(r.x + inset, r.y, r.width - inset, 1f), Look.Divider);
        }
    }
}
