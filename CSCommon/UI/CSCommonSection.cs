using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Section — 시안 'Section'(라이트 · 다크): 바탕 창 띠(우물 · 반지름 24 · 안쪽 12 · 플레이트 사이 12) 안에 둥근 플레이트
    /// (반지름 24 · 히어로 28 · 안쪽 16 / 아래 12 · 면 + 안쪽 1px 선 · 그림자 없음). 제목 = 모드 파스텔 알약 칩 32(아이콘 16 + 이름 13.5 굵게 + ▾),
    /// 보조 = 회색 알약 28, 줄 = 알약 행 44(호버 Field 면 · 순번 원 28), 헤어라인 없음. 제목을 누르면 접힘(0.32초 cubic-bezier(.2,.8,.2,1) · 쉐브론 −90° · 투명 0.2초).
    /// 접힘 상태는 열쇠(보통 제목)마다 이 클래스가 기억함 — 다시 지어도(카드 재생성) 그대로.
    /// </summary>
    public static class Section
    {
        // ---------------- 치수(시안 그대로)

        /// <summary>우물(바탕 창 띠): 반지름 · 안쪽 여백 · 플레이트 사이.</summary>
        public const float WellRadius = 24f, WellPad = 12f, Gap = 12f;
        /// <summary>플레이트: 반지름 · 히어로 반지름 · 안쪽(좌우 · 위) · 아래 · 머리 아래 간격.</summary>
        public const float Radius = 24f, HeroRadius = 28f, Pad = 16f, PadBottom = 12f, HeadGap = 6f;
        /// <summary>제목 칩: 높이 · 왼쪽(아이콘 있을 때) · 오른쪽 · 칸 사이 · 아이콘 · 글 · 쉐브론 · 선 굵기.</summary>
        public const float ChipH = 32f, ChipPadL = 12f, ChipPadR = 14f, ChipGap = 8f, ChipIcon = 16f, ChipFont = 13.5f, Chevron = 14f, ChipStroke = 2f, ChevronStroke = 2.4f;
        /// <summary>보조 알약: 높이 · 좌우 · 글.</summary>
        public const float SubH = 28f, SubPadX = 12f, SubFont = 12.5f;
        /// <summary>알약 행: 높이 · 왼쪽(순번 원 앞) · 오른쪽 · 칸 사이 · 글 · 순번 원 · 순번 글.</summary>
        public const float RowH = 44f, RowPadL = 8f, RowPadR = 18f, RowGap = 12f, RowFont = 14f, RankD = 28f, RankFont = 12.5f;
        /// <summary>움직임: 접기 · 투명 · 행 호버(선형) · 눌림 배율.</summary>
        public const float FoldSeconds = 0.32f, FadeSeconds = 0.2f, HoverSeconds = 0.12f, PressScale = 0.97f;

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

        /// <summary>제목 칩 폭(아이콘 · 쉐브론 포함 여부에 따라).</summary>
        public static float ChipWidth(string title, string icon, bool chevron)
        {
            float w = (string.IsNullOrEmpty(icon) ? ChipPadR : ChipPadL + ChipIcon + ChipGap) + Mathf.Ceil(Draw.TextWidth(title ?? "", ChipFont, true)) + ChipPadR;
            return chevron ? w + ChipGap + Chevron : w;
        }

        /// <summary>제목 줄(누르지 않는 칩): r 세로 가운데에 파스텔 칩, 오른쪽 끝에 회색 보조 알약. 접기가 없는 곳(설명 판 · 떠 있는 판).</summary>
        public static void Title(Rect r, string title, string sub = null, string icon = null)
        {
            float sw = SubPill(r, sub);
            if (Draw.Repaint) Chip(ChipRect(r, title, icon, false, sw), title, icon, -1f);
        }

        /// <summary>
        /// 접히는 제목 줄: 칩(▾ 가 접힘에 따라 0 → −90°)을 누르면 key 의 접힘을 뒤집음. 보조 알약은 오른쪽 끝. 뒤집었으면 true.
        /// 펼침 정도는 Openness(key) 로 읽어 몸통 높이 · 투명도에 씀.
        /// </summary>
        public static bool Head(Rect r, string key, string title, string sub = null, string icon = null)
        {
            float sw = SubPill(r, sub);
            var chip = ChipRect(r, title, icon, true, sw);
            int k = FoldKey(key);
            float pr = Look.Press(k, chip);
            if (Draw.Repaint)
            {
                Draw.BeginScale(chip.center, Look.PressScale(pr, PressScale));
                Chip(chip, title, icon, Openness(key));
                Draw.EndScale();
            }
            if (!Draw.Button(chip)) return false;
            Toggle(key);
            return true;
        }

        static Rect ChipRect(Rect r, string title, string icon, bool chevron, float subW)
        {
            float w = Mathf.Min(ChipWidth(title, icon, chevron), Mathf.Max(0f, r.width - subW));
            return new Rect(r.x, Mathf.Round(r.center.y - ChipH * 0.5f), w, ChipH);
        }

        /// <summary>칩 그리기. open &lt; 0 이면 쉐브론 없음, 아니면 펼침 정도(1 = ▾, 0 = ▸).</summary>
        static void Chip(Rect c, string title, string icon, float open)
        {
            Draw.Round(c, Look.PastelFill, ChipH * 0.5f);
            float x = c.x + (string.IsNullOrEmpty(icon) ? ChipPadR : ChipPadL);
            if (!string.IsNullOrEmpty(icon))
            {
                Draw.Icon(icon, new Rect(x, Mathf.Round(c.center.y - ChipIcon * 0.5f), ChipIcon, ChipIcon), Look.PastelInk, 24f, ChipStroke);
                x += ChipIcon + ChipGap;
            }
            float right = c.xMax - ChipPadR - (open >= 0f ? Chevron + ChipGap : 0f);
            Draw.TextFit(new Rect(x, c.y, Mathf.Max(0f, right - x), c.height), title ?? "", ChipFont, Look.PastelInk, true);
            if (open < 0f) return;
            var cr = new Rect(c.xMax - ChipPadR - Chevron, Mathf.Round(c.center.y - Chevron * 0.5f), Chevron, Chevron);
            var m = GUI.matrix;
            if (open < 0.999f) GUIUtility.RotateAroundPivot(-90f * (1f - open), Draw.ToScreen(cr).center);
            Draw.Icon(Icons.Chevron, cr, Look.PastelInk, 24f, ChevronStroke);
            GUI.matrix = m;
        }

        /// <summary>회색 보조 알약(오른쪽 끝, r 의 45% 까지). 차지한 폭(+ 칸 사이)을 돌려줌, 없으면 0.</summary>
        static float SubPill(Rect r, string sub)
        {
            if (string.IsNullOrEmpty(sub)) return 0f;
            float w = Mathf.Min(r.width * 0.45f, Mathf.Ceil(Draw.TextWidth(sub, SubFont)) + SubPadX * 2f);
            if (Draw.Repaint)
            {
                var p = new Rect(r.xMax - w, Mathf.Round(r.center.y - SubH * 0.5f), w, SubH);
                Draw.Round(p, Look.Pill, SubH * 0.5f);
                Draw.TextFit(new Rect(p.x + SubPadX, p.y, p.width - SubPadX * 2f, p.height), sub, SubFont, SubInk, false, TextAnchor.MiddleCenter);
            }
            return w + ChipGap;
        }

        // ---------------- 접기

        struct Run
        {
            public bool Open;
            public float From, FadeFrom, At;
        }

        static readonly Dictionary<int, Run> runs = new();
        static float lastToggle = -100f;

        static int FoldKey(string key) => CSCommonAnim.Key("section.fold", key);

        /// <summary>접고 펴는 중(마지막 뒤집기 뒤 0.32초) — 내용 높이가 바뀌므로 배치를 다시 잴 것.</summary>
        public static bool Folding => Time.unscaledTime - lastToggle < FoldSeconds;

        public static bool IsOpen(string key) => !runs.TryGetValue(FoldKey(key), out var s) || s.Open;

        public static void Toggle(string key)
        {
            int k = FoldKey(key);
            float now = Time.unscaledTime;
            var s = runs.TryGetValue(k, out var old) ? old : new Run { Open = true, From = 1f, FadeFrom = 1f, At = now - FoldSeconds };
            s.From = Value(s, now, out s.FadeFrom);
            s.Open = !s.Open;
            s.At = now;
            runs[k] = s;
            lastToggle = now;
        }

        /// <summary>펼침 정도 0 ~ 1(곡선 cubic-bezier(.2,.8,.2,1)). 처음 보는 열쇠는 펼침.</summary>
        public static float Openness(string key) => Openness(key, out _);

        /// <summary>펼침 정도 + fade = 몸통 투명도(0.2초 선형).</summary>
        public static float Openness(string key, out float fade)
        {
            if (!runs.TryGetValue(FoldKey(key), out var s))
            {
                fade = 1f;
                return 1f;
            }
            return Value(s, Time.unscaledTime, out fade);
        }

        static float Value(Run s, float now, out float fade)
        {
            float el = now - s.At, to = s.Open ? 1f : 0f;
            fade = Mathf.Lerp(s.FadeFrom, to, Mathf.Clamp01(el / FadeSeconds));
            return Mathf.Lerp(s.From, to, Modal.Ease(el / FoldSeconds));
        }

        /// <summary>
        /// 몸통 그리기 시작: body 위쪽부터 body.height × open 만 보이게 자르고 투명도 × fade. 안쪽 좌표는 body 왼쪽 위 기준.
        /// 다 접혔으면(open ≈ 0) false — 그리지 말고 EndBody 도 부르지 말 것(잘린 줄이 누름을 받지 않게).
        /// </summary>
        public static bool BeginBody(Rect body, float open, float fade)
        {
            if (open <= 0.001f) return false;
            opacities.Push(Draw.Opacity);
            Draw.Opacity *= fade;
            Draw.BeginClip(new Rect(body.x, body.y, body.width, Mathf.Round(body.height * open)), Vector2.zero);
            return true;
        }

        public static void EndBody()
        {
            Draw.EndClip();
            if (opacities.Count > 0) Draw.Opacity = opacities.Pop();
        }

        static readonly Stack<float> opacities = new();

        // ---------------- 알약 행

        /// <summary>알약 행 바탕: 평소 투명 · 호버 Field 면(0.12초 선형), 반지름 = 높이 반(최대 22).</summary>
        public static void Row(Rect r, bool hoverable = true)
        {
            int key = Look.Key(r, "section.row");
            float hv = CSCommonAnim.Toward(key, hoverable && Draw.Hover(r) ? 1f : 0f, 1f / HoverSeconds);
            if (Draw.Repaint && hv > 0.01f) Draw.Round(r, Draw.Alpha(Look.Field, hv), Mathf.Min(RowH * 0.5f, r.height * 0.5f));
        }

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
