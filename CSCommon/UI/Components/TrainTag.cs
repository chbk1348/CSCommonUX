using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.TrainTag — 게임 화면 열차 위 카드(대중교통 캔버스 TrainOverlay): 흰 카드(MapTag) 높이 56 · 왼쪽 노선 색 띠 ·
    /// 윗줄 노선 칩 · 종별(급행/완행, 빈 글이면 없음) · 행선 · 번호, 아랫줄 상태 점 · 상태 · 다음 역 · 승객. 기둥 20 · 발 점. 말줄임 없이 글 폭에 맞춤.
    /// </summary>
    public static class TrainTag
    {
        /// <summary>카드 한 장 글(그릴 때마다 같은 값으로).</summary>
        public struct Info
        {
            public Color Bar, Chip, Dot;
            public string Line, Kind, Dest, No, State, Next, Pax;
            /// <summary>종별이 급행(아니면 완행 색).</summary>
            public bool Express;

            // 아래는 비행기 카드(캔버스 Runways) 선택 필드 — Phase 가 0 이면 모두 안 쓰고 예전 카드 그대로예요.
            /// <summary>
            /// 비행기 단계 1 지상 · 2 이륙 · 3 비행 · 4 착륙(0 = 예전 카드). 1~4 면 폭 348 세 줄 카드:
            /// 윗줄 편명 칩(Line · 면 Chip · 글 LineFg) · 행선(Dest) · 크기(No) · 오른쪽 상태 알약(State) / 단계 막대 · 라벨 / 아랫줄 Next · 순번 칩(Rank) · 승객(Pax) · 승객 막대(PaxFill).
            /// Bar · Dot · Kind · Express 는 안 써요.
            /// </summary>
            public int Phase;
            /// <summary>편명 칩 글자색(투명 = 흰색). Pal.AirlineChip 의 fg 를 그대로 넣으면 돼요.</summary>
            public Color LineFg;
            /// <summary>상태 알약 · 단계 막대를 주황(주의)으로(아니면 초록).</summary>
            public bool Warn;
            /// <summary>순번 칩 글(예 "2번째", 빈 글이면 안 그림).</summary>
            public string Rank;
            /// <summary>승객 막대 채움 0~1(음수면 막대 안 그림).</summary>
            public float PaxFill;
        }

        /// <summary>비행기 카드 단계 라벨(Phase 1~4 순서).</summary>
        public static readonly string[] Phases = { "지상", "이륙", "비행", "착륙" };

        const string Sep = "·";

        static float ChipW(string s) => Mathf.Ceil(Draw.TextWidth(s, CSCommonDesign.TagChipFont, true)) + CSCommonDesign.TagChipPadX * 2f;
        static float KindW(string s) => Mathf.Ceil(Draw.TextWidth(s, CSCommonDesign.TagKindFont, true)) + CSCommonDesign.TagKindPadX * 2f;
        static float SubW(string s) => Mathf.Ceil(Draw.TextWidth(s, CSCommonDesign.TagSubFont));

        static float Line1W(Info t)
        {
            float g = CSCommonDesign.TagItemGap;
            float w = ChipW(t.Line) + g + Mathf.Ceil(Draw.TextWidth(t.Dest, CSCommonDesign.TagNameFont, true));
            if (!string.IsNullOrEmpty(t.Kind)) w += g + KindW(t.Kind);
            if (!string.IsNullOrEmpty(t.No)) w += g + SubW(t.No);
            return w;
        }

        static float Line2W(Info t)
        {
            float g = CSCommonDesign.TagItemGap, sep = SubW(Sep);
            return CSCommonDesign.PinDot + g + SubW(t.State) + (g + sep + g + SubW(t.Next)) + (g + sep + g + SubW(t.Pax));
        }

        /// <summary>발 점 가운데가 foot(논리 좌표)일 때 카드 칸.</summary>
        public static Rect Card(Vector2 foot, Info t)
        {
            bool fl = t.Phase > 0;
            float w = fl ? FlightWidth(t) : CSCommonDesign.TagPadL + CSCommonDesign.TagBarW + CSCommonDesign.TagGap + Mathf.Max(Line1W(t), Line2W(t)) + CSCommonDesign.TagPadR;
            float h = fl ? CSCommonDesign.FlightH : CSCommonDesign.TagH;
            float bottom = foot.y - CSCommonDesign.TagFoot * 0.5f - CSCommonDesign.TagStemH;
            return new Rect(Mathf.Round(foot.x - w * 0.5f), Mathf.Round(bottom - h), w, h);
        }

        // 비행기 카드 칸 폭
        static float CodeW(string s) => Mathf.Ceil(Draw.TextWidth(s, CSCommonDesign.FlightCodeFont, true)) + CSCommonDesign.FlightCodePadX * 2f;
        static float PillW(string s) => CSCommonDesign.FlightPillPadL + CSCommonDesign.PinDot + CSCommonDesign.PinDotGap + Mathf.Ceil(Draw.TextWidth(s, CSCommonDesign.FlightPillFont, true)) + CSCommonDesign.FlightPillPadR;
        static float RankW(string s) => Mathf.Ceil(Draw.TextWidth(s, CSCommonDesign.TagSubFont, true)) + CSCommonDesign.FlightRankPadX * 2f;
        static float DestW(string s) => Mathf.Ceil(Draw.TextWidth(s, CSCommonDesign.TagNameFont, true));

        /// <summary>비행기 카드 폭: 348, 글이 길면 한 줄에 다 들어가게 넓힘(말줄임 없음).</summary>
        static float FlightWidth(Info t)
        {
            float g = CSCommonDesign.FlightItemGap;
            float w1 = CodeW(t.Line) + g + DestW(t.Dest) + (string.IsNullOrEmpty(t.No) ? 0f : g + SubW(t.No)) + g + PillW(t.State);
            float w3 = SubW(t.Next) + (string.IsNullOrEmpty(t.Rank) ? 0f : g + RankW(t.Rank)) + g + SubW(t.Pax) + (t.PaxFill < 0f ? 0f : g + CSCommonDesign.FlightPaxW);
            return Mathf.Max(CSCommonDesign.FlightW, Mathf.Max(w1, w3) + CSCommonDesign.FlightPadX * 2f);
        }

        /// <summary>비행기 카드 안(판 · 그림자는 In 이 그림): 윗줄 · 단계 막대 · 라벨 · 아랫줄.</summary>
        static void Flight(Rect card, Info t)
        {
            float g = CSCommonDesign.FlightItemGap, x = card.x + CSCommonDesign.FlightPadX, right = card.xMax - CSCommonDesign.FlightPadX, inner = right - x;
            Color tone = t.Warn ? Pal.MapPinWarnDot : Pal.MapPinOkDot;
            // 윗줄: 편명 칩 · 행선 · 크기 … 상태 알약
            float y = card.y + CSCommonDesign.FlightPadY, lh = CSCommonDesign.FlightLine1H;
            var chip = new Rect(x, y, CodeW(t.Line), lh);
            Draw.Round(chip, t.Chip, CSCommonDesign.FlightCodeRadius);
            Draw.TextFit(chip, t.Line, CSCommonDesign.FlightCodeFont, t.LineFg.a > 0f ? t.LineFg : Pal.MapTag, true, TextAnchor.MiddleCenter);
            float pw = PillW(t.State), ph = CSCommonDesign.FlightPillH;
            var pill = new Rect(right - pw, y + (lh - ph) * 0.5f, pw, ph);
            Draw.Round(pill, t.Warn ? Pal.FlightWarnBg : Pal.FlightOkBg, ph * 0.5f);
            float px = pill.x + CSCommonDesign.FlightPillPadL;
            Draw.Dot(new Vector2(px + CSCommonDesign.PinDot * 0.5f, pill.center.y), CSCommonDesign.PinDot * 0.5f, tone);
            px += CSCommonDesign.PinDot + CSCommonDesign.PinDotGap;
            Draw.TextFit(new Rect(px, pill.y, pill.xMax - CSCommonDesign.FlightPillPadR - px, ph), t.State, CSCommonDesign.FlightPillFont, t.Warn ? Pal.FlightWarnInk : Pal.FlightOkInk, true);
            float cx = chip.xMax + g, dw = DestW(t.Dest);
            Draw.TextFit(new Rect(cx, y, dw, lh), t.Dest, CSCommonDesign.TagNameFont, Pal.MapTagInk, true);
            cx += dw + g;
            if (!string.IsNullOrEmpty(t.No)) Draw.TextFit(new Rect(cx, y, SubW(t.No), lh), t.No, CSCommonDesign.TagSubFont, Pal.TagIntercity);
            // 단계 막대 네 칸(지금 단계까지 채움) · 아래 라벨(지금 단계 굵게)
            y += lh + CSCommonDesign.FlightRowGap;
            float tg = CSCommonDesign.FlightTrackGap, cell = (inner - tg * (Phases.Length - 1)) / Phases.Length;
            float ly = y + CSCommonDesign.FlightTrackH + CSCommonDesign.FlightLabelGap;
            for (int i = 0; i < Phases.Length; i++)
            {
                float sx = x + (cell + tg) * i;
                Draw.Round(new Rect(sx, y, cell, CSCommonDesign.FlightTrackH), i < t.Phase ? tone : Pal.FlightTrack, CSCommonDesign.FlightTrackRadius);
                bool now = i == t.Phase - 1;
                Draw.TextFit(new Rect(sx, ly, cell, CSCommonDesign.FlightLabelH), Phases[i], CSCommonDesign.FlightLabelFont, now ? Pal.MapTagInk : Pal.FlightLabel, now);
            }
            // 아랫줄: 활주로 등 · 순번 칩 … 승객 글 · 승객 막대
            y = ly + CSCommonDesign.FlightLabelH + CSCommonDesign.FlightRowGap;
            float h3 = CSCommonDesign.FlightLine3H, nw = SubW(t.Next);
            Draw.TextFit(new Rect(x, y, nw, h3), t.Next, CSCommonDesign.TagSubFont, Pal.TagIntercity);
            if (!string.IsNullOrEmpty(t.Rank))
            {
                var rank = new Rect(x + nw + g, y, RankW(t.Rank), h3);
                Draw.Round(rank, Pal.FlightRankBg, h3 * 0.5f);
                Draw.TextFit(rank, t.Rank, CSCommonDesign.TagSubFont, Pal.MapTagInk, true, TextAnchor.MiddleCenter);
            }
            float rx = right;
            if (t.PaxFill >= 0f)
            {
                rx -= CSCommonDesign.FlightPaxW;
                var bar = new Rect(rx, Mathf.Round(y + (h3 - CSCommonDesign.FlightPaxH) * 0.5f), CSCommonDesign.FlightPaxW, CSCommonDesign.FlightPaxH);
                Draw.Round(bar, Pal.FlightTrack, CSCommonDesign.FlightPaxH * 0.5f);
                float fw = Mathf.Round(CSCommonDesign.FlightPaxW * Mathf.Clamp01(t.PaxFill));
                if (fw > 0f) Draw.Round(new Rect(bar.x, bar.y, fw, bar.height), Pal.TagIntercity, CSCommonDesign.FlightPaxH * 0.5f);
                rx -= g;
            }
            float aw = SubW(t.Pax);
            Draw.TextFit(new Rect(rx - aw, y, aw, h3), t.Pax, CSCommonDesign.TagSubFont, Pal.TagIntercity);
        }

        /// <summary>카드 그리기(card = Card 값). 눌렀다 떼면 true.</summary>
        public static bool In(Rect card, Vector2 foot, Info t)
        {
            if (Draw.Repaint)
            {
                float rad = CSCommonDesign.TagRadius;
                Draw.Fill(new Rect(Mathf.Round(foot.x - CSCommonDesign.PinStemW * 0.5f), card.yMax, CSCommonDesign.PinStemW, foot.y - card.yMax), Pal.MapPinStem);
                Draw.Dot(foot, CSCommonDesign.TagFoot * 0.5f, Pal.MapTag);
                if (!Draw.Shadow(card, rad, CSCommonDesign.PinShadow)) Draw.Round(new Rect(card.x, card.y + CSCommonDesign.PinShadowY, card.width, card.height), Pal.MapTagShadow, rad);
                Draw.Round(card, Pal.MapTag, rad);
                Draw.RoundOutline(card, Pal.MapPinLine, rad);
                if (t.Phase > 0)
                {
                    Flight(card, t);
                    return Draw.Button(card);
                }
                float x = card.x + CSCommonDesign.TagPadL;
                Draw.Round(new Rect(x, Mathf.Round(card.center.y - CSCommonDesign.TagBarH * 0.5f), CSCommonDesign.TagBarW, CSCommonDesign.TagBarH), t.Bar, CSCommonDesign.TagBarW * 0.5f);
                x += CSCommonDesign.TagBarW + CSCommonDesign.TagGap;
                float g = CSCommonDesign.TagItemGap;
                float y1 = Mathf.Round(card.center.y - (CSCommonDesign.TagLine1H + CSCommonDesign.TagRowGap + CSCommonDesign.TagLine2H) * 0.5f);
                float y2 = y1 + CSCommonDesign.TagLine1H + CSCommonDesign.TagRowGap;
                // 윗줄: 칩 · 종별 · 행선 · 번호
                float cx = x, cw = ChipW(t.Line);
                var chip = new Rect(cx, y1 + (CSCommonDesign.TagLine1H - CSCommonDesign.TagChipH) * 0.5f, cw, CSCommonDesign.TagChipH);
                Draw.Round(chip, t.Chip, CSCommonDesign.TagChipH * 0.5f);
                Draw.TextFit(chip, t.Line, CSCommonDesign.TagChipFont, Pal.MapTag, true, TextAnchor.MiddleCenter);
                cx += cw + g;
                if (!string.IsNullOrEmpty(t.Kind))
                {
                    float kw = KindW(t.Kind);
                    var kind = new Rect(cx, y1 + (CSCommonDesign.TagLine1H - CSCommonDesign.TagKindH) * 0.5f, kw, CSCommonDesign.TagKindH);
                    Draw.Round(kind, t.Express ? Pal.TagExpressBg : Pal.TagLocalBg, CSCommonDesign.TagKindH * 0.5f);
                    Draw.TextFit(kind, t.Kind, CSCommonDesign.TagKindFont, t.Express ? Pal.TagExpressInk : Pal.TagLocalInk, true, TextAnchor.MiddleCenter);
                    cx += kw + g;
                }
                float dw = Mathf.Ceil(Draw.TextWidth(t.Dest, CSCommonDesign.TagNameFont, true));
                Draw.TextFit(new Rect(cx, y1, dw, CSCommonDesign.TagLine1H), t.Dest, CSCommonDesign.TagNameFont, Pal.MapTagInk, true);
                cx += dw + g;
                if (!string.IsNullOrEmpty(t.No)) Draw.TextFit(new Rect(cx, y1, SubW(t.No), CSCommonDesign.TagLine1H), t.No, CSCommonDesign.TagSubFont, Pal.MapTagSub);
                // 아랫줄: 점 · 상태 · 다음 역 · 승객
                cx = x;
                Draw.Dot(new Vector2(cx + CSCommonDesign.PinDot * 0.5f, y2 + CSCommonDesign.TagLine2H * 0.5f), CSCommonDesign.PinDot * 0.5f, t.Dot);
                cx += CSCommonDesign.PinDot + g;
                foreach (string part in new[] { t.State, Sep, t.Next, Sep, t.Pax })
                {
                    float pw = SubW(part);
                    Draw.TextFit(new Rect(cx, y2, pw, CSCommonDesign.TagLine2H), part, CSCommonDesign.TagSubFont, part == Sep ? Pal.TagSep : Pal.MapTagSub);
                    cx += pw + g;
                }
            }
            return Draw.Button(card);
        }
    }
}
