using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX 3.5 판(FIA 캔버스 R5Surface 6종). 그림자 · 파스텔 제목 칩 · 상태 테두리 · 카드 안 카드 없음(안쪽 칸으로).
    /// 쓰는 법: var c = Surface.Frame(r, kind) → float y = Surface.Head(c, kind, 제목, meta) → 몸 → Surface.Foot(c, kind) 칸에 버튼.
    /// 몸: 묶음 사이 16 · 줄 사이 0 · 구분선은 묶음 사이에만 1px Hairline.
    /// </summary>
    public static class Surface
    {
        /// <summary>
        /// Card 카드(탭 안 묶음) · Hero 히어로(한 화면 하나, 중립 면) · Inner 안쪽 칸(값 · 설정 칸 · 타일) · Slot 빈 자리(투명 + 선) ·
        /// Modal 확인 상자 · 일정 모달(폭 560 / 720, 덮개 Pal.Scrim35) · Ground 바탕 묶음(면 없음).
        /// </summary>
        public enum Kind { Card, Hero, Inner, Slot, Modal, Ground }

        /// <summary>판 모서리: 카드 · 히어로 · 빈 자리 16, 안쪽 칸 8, 모달 20, 바탕 0.</summary>
        public static float Radius(Kind kind) => kind switch
        {
            Kind.Inner => D35.RadiusCtrl,
            Kind.Modal => D35.RadiusWindow,
            Kind.Ground => 0f,
            _ => D35.RadiusCard,
        };

        /// <summary>판 안쪽 칸: 카드 · 빈 자리 20, 히어로 24, 안쪽 칸 위아래 12 · 좌우 16, 모달 좌우 24(위아래 0 — 머리 · 바닥이 높이를 가짐), 바탕 0.</summary>
        public static Rect Inset(Rect r, Kind kind)
        {
            float px, py;
            switch (kind)
            {
                case Kind.Hero: px = py = D35.HeroPad; break;
                case Kind.Inner: px = D35.InnerPadX; py = D35.InnerPadY; break;
                case Kind.Modal: px = D35.ModalPad; py = 0f; break;
                case Kind.Ground: px = py = 0f; break;
                default: px = py = D35.CardPad; break;
            }
            return new Rect(r.x + px, r.y + py, Mathf.Max(0f, r.width - px * 2f), Mathf.Max(0f, r.height - py * 2f));
        }

        /// <summary>놓인 면보다 한 단 밝은 면(칩 · 단계 막대 홈 · 안쪽 칸): 카드 · 모달 위 = Raised, 그 밖(히어로 · 안쪽 칸 · 창 몸) 위 = Card.</summary>
        public static Color Raise(Kind on) => on is Kind.Card or Kind.Modal ? Pal.GroundRaised : Pal.GroundCard;

        /// <summary>판 면 색(빈 자리 · 바탕 = 투명). 안쪽 칸은 놓인 면(on)보다 한 단.</summary>
        public static Color Face(Kind kind, Kind on = Kind.Card) => kind switch
        {
            Kind.Card or Kind.Modal => Pal.GroundCard,
            Kind.Hero => Pal.GroundRaised,
            Kind.Inner => Raise(on),
            _ => Color.clear,
        };

        /// <summary>판을 그리고 안쪽 칸(Inset)을 돌려줌. 카드 · 빈 자리 · 모달은 Hairline 1px(안쪽), 히어로 · 안쪽 칸은 선 없음. on = 놓인 면(안쪽 칸만 봄).</summary>
        public static Rect Frame(Rect r, Kind kind, Kind on = Kind.Card)
        {
            if (Draw.Repaint && kind != Kind.Ground)
            {
                float rad = Radius(kind);
                Draw.Round(r, Face(kind, on), rad);
                if (kind is Kind.Card or Kind.Slot or Kind.Modal) Draw.RoundOutline(r, Pal.Hairline, rad, 1f);
            }
            return Inset(r, kind);
        }

        /// <summary>머리 높이: 카드 · 히어로 · 빈 자리 · 바탕 32, 안쪽 칸 16(라벨), 모달 56.</summary>
        public static float HeadH(Kind kind) => kind switch
        {
            Kind.Inner => D35.LineCaption,
            Kind.Modal => D35.ModalHeadH,
            _ => D35.CardHeadH,
        };

        /// <summary>머리 → 몸 사이: 카드 등 12, 안쪽 칸 4(라벨 → 값), 모달 0(머리 56 안에 여백).</summary>
        public static float HeadGap(Kind kind) => kind switch
        {
            Kind.Inner => D35.KpiGap,
            Kind.Modal => 0f,
            _ => D35.HeadGap,
        };

        /// <summary>
        /// 머리(제목 줄)를 그리고 몸이 시작하는 y 를 돌려줌. content = Frame 이 돌려준 칸.
        /// 카드 · 히어로 · 바탕: 제목 16 InkTxt + 오른쪽 meta 12 InkDim(숫자 자리 폭 같음). 빈 자리: 제목 InkSub. 안쪽 칸: 라벨 12 InkDim. 모달: 제목 20 InkTxt.
        /// </summary>
        public static float Head(Rect content, Kind kind, string title, string meta = null)
        {
            float h = HeadH(kind);
            var row = new Rect(content.x, content.y, content.width, h);
            if (Draw.Repaint)
            {
                float metaW = string.IsNullOrEmpty(meta) || kind == Kind.Inner ? 0f : Draw.NumWidth(meta, D35.FontCaption) + D35.IconTextGap;
                var titleRow = new Rect(row.x, row.y, row.width - metaW, h);
                switch (kind)
                {
                    case Kind.Inner: Draw.Text(titleRow, title, D35.FontCaption, Pal.InkDim); break;
                    case Kind.Modal: Draw.Text(titleRow, title, D35.FontHeading, Pal.InkTxt); break;
                    default: Draw.Text(titleRow, title, D35.FontTitle, kind == Kind.Slot ? Pal.InkSub : Pal.InkTxt); break;
                }
                if (metaW > 0f) Draw.Num(row, meta, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleRight);
            }
            return row.y + h + HeadGap(kind);
        }

        // ---- 3.6 머리 확장: 제목 앞 점 · 오른쪽 작은 버튼 · 점 붙은 meta 조각

        /// <summary>머리 줄 칸(content 위, 높이 HeadH) — 오른쪽에 고르기 칸 · 버튼을 직접 놓을 때.</summary>
        public static Rect HeadRow(Rect content, Kind kind) => new(content.x, content.y, content.width, HeadH(kind));

        /// <summary>머리 — 제목 앞 점 8 → 8 → 제목(코션 · 개최 상태 · 트랙 카드). 오른쪽 meta 는 Head 와 같음. 몸이 시작하는 y 를 돌려줌.</summary>
        public static float Head(Rect content, Kind kind, string title, string meta, Color dot, bool live = false)
        {
            var row = HeadRow(content, kind);
            if (Draw.Repaint) StatusDot.Paint(new Vector2(row.x + D35.Dot * 0.5f, row.center.y), dot, live);
            float inset = D35.Dot + D35.IconTextGap;
            float metaW = string.IsNullOrEmpty(meta) ? 0f : Draw.NumWidth(meta, D35.FontCaption) + D35.IconTextGap;
            Head(new Rect(content.x + inset, content.y, Mathf.Max(0f, content.width - inset - metaW), content.height), kind, title, null);
            if (metaW > 0f && Draw.Repaint) Draw.Num(row, meta, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleRight);
            return row.y + row.height + HeadGap(kind);
        }

        /// <summary>
        /// 머리 — 오른쪽에 작은 보조 버튼 하나(meta 대신, '전체 순위' · '전체 22명 보기'), dot 이 있으면 제목 앞 점. 버튼을 눌러 떼면 clicked. 몸이 시작하는 y 를 돌려줌.
        /// </summary>
        public static float Head(Rect content, Kind kind, string title, string action, out bool clicked, Color? dot = null)
        {
            var row = HeadRow(content, kind);
            float right = row.xMax;
            clicked = Button35.AtRight(ref right, row.center.y, action, Tone35.Secondary, default, default, null, true, true, null, kind);
            var left = new Rect(content.x, content.y, Mathf.Max(0f, right - content.x), content.height);
            return dot.HasValue ? Head(left, kind, title, null, dot.Value) : Head(left, kind, title, null);
        }

        /// <summary>
        /// 점 붙은 meta 조각('●개인 최고' · '●5 / 5 충족'): 오른쪽 끝 right · 세로 가운데 cy 에 [점 8 → 8] 글 12 InkDim(숫자 자리 폭 같음)을 그리고
        /// 다음 조각이 끝날 x(= 조각 왼쪽 − 8)를 돌려줌. 오른쪽 조각부터 차례로 부름. Head 는 meta 없이 부르고 그 머리 줄(HeadRow)에 놓음.
        /// </summary>
        public static float Meta(float right, float cy, string text, Color? dot = null)
        {
            float w = Mathf.Ceil(Draw.NumWidth(text ?? "", D35.FontCaption));
            float x = right - w;
            if (Draw.Repaint) Draw.Num(new Rect(x, Mathf.Round(cy - D35.LineCaption * 0.5f), w, D35.LineCaption), text, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleRight);
            if (dot.HasValue)
            {
                x -= D35.IconTextGap + D35.Dot;
                StatusDot.Paint(new Vector2(x + D35.Dot * 0.5f, cy), dot.Value);
            }
            return x - D35.IconTextGap;
        }

        /// <summary>꼬리 높이(위 여백 포함): 카드 · 히어로 16 + 40, 안쪽 칸 12 + 32, 모달 64, 빈 자리 · 바탕 0(꼬리 없음).</summary>
        public static float FootH(Kind kind) => kind switch
        {
            Kind.Card or Kind.Hero => D35.CardFootGap + D35.BtnH,
            Kind.Inner => D35.InnerFootGap + D35.BtnSmallH,
            Kind.Modal => D35.ModalFootH,
            _ => 0f,
        };

        /// <summary>꼬리 버튼 줄 칸(content 바닥에 붙음, 버튼 높이만큼): 버튼은 오른쪽 맞춤 · 사이 8 · 주 버튼 하나. 꼬리 없는 종류는 높이 0.</summary>
        public static Rect Foot(Rect content, Kind kind)
        {
            float btn = kind switch
            {
                Kind.Card or Kind.Hero or Kind.Modal => D35.BtnH,
                Kind.Inner => D35.BtnSmallH,
                _ => 0f,
            };
            // 모달 바닥 64 안 가운데, 나머지는 content 바닥
            float bottom = kind == Kind.Modal ? content.yMax - (D35.ModalFootH - btn) * 0.5f : content.yMax;
            return new Rect(content.x, bottom - btn, content.width, btn);
        }

        /// <summary>판 높이 = 위아래 여백 + 머리(+ 사이) + 몸 + 꼬리.</summary>
        public static float Height(Kind kind, float body, bool head = true, bool foot = false)
        {
            float pad = Inset(new Rect(0f, 0f, 100f, 100f), kind).y * 2f;
            return pad + (head ? HeadH(kind) + HeadGap(kind) : 0f) + body + (foot ? FootH(kind) : 0f);
        }

        /// <summary>몸 글 색: 안쪽 칸 InkTxt, meta · 꼬리 없는 카드의 가운데 글 InkDim, 그 밖 InkSub. 빈 자리 본문은 InkDim 가운데.</summary>
        public static Color BodyInk(Kind kind, bool centered) => kind == Kind.Inner ? Pal.InkTxt : centered || kind == Kind.Slot ? Pal.InkDim : Pal.InkSub;
    }
}
