using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 목록 탐색 줄(설정 분류 44 · 아이콘 18 / 채팅 채널 36 · 아이콘 16): 좌우 12 · r8 · 아이콘 → 8 → 이름 14 … 오른쪽 수 12 숫자.
    /// 고름 = GroundRaised + 위 1px SelectedTopLine · 이름 InkTxt · 아이콘 강조 · 수 InkSub(면 0.15초). 안 고름 = 투명 · InkSub · 아이콘 · 수 InkDim.
    /// 자리(새 분류 자리 · 빈 칸) = 투명 + Hairline · 모두 InkDim · 누름 없음. 호버 면 없음(캔버스).
    /// </summary>
    public static class NavRow35
    {
        /// <summary>줄을 그리고 안 고른 줄을 눌러 떼면 true. indent = 왼쪽 들여쓰기(곁가지 채널 36).</summary>
        public static bool Paint(Rect r, string icon, string label, string count, bool on, Color accent, bool small = false, bool slot = false, float indent = 0f, string tip = null)
        {
            float t = CSCommonAnim.Toward(Look.Key(r, "nav35", label), on && !slot ? 1f : 0f, 1f / D35.DurFeedback);
            if (Draw.Repaint)
            {
                if (slot) Draw.RoundOutline(r, Pal.Hairline, D35.RadiusCtrl, 1f);
                else Segmented35.Selected(r, D35.RadiusCtrl, t);
                float s = small ? D35.IconS : D35.IconM, x = r.x + D35.NavPadX + indent, right = r.xMax - D35.NavPadX;
                Color iconInk = slot ? Pal.InkDim : Color.Lerp(Pal.InkDim, accent, t);
                if (icon != null)
                {
                    Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - s * 0.5f), s, s), iconInk, D35.IconBox, D35.IconLine);
                    x += s + D35.IconTextGap;
                }
                if (!string.IsNullOrEmpty(count))
                {
                    float cw = Mathf.Ceil(Draw.NumWidth(count, D35.FontCaption));
                    Draw.Num(new Rect(right - cw, r.y, cw, r.height), count, D35.FontCaption, slot ? Pal.InkDim : Color.Lerp(Pal.InkDim, Pal.InkSub, t), TextAnchor.MiddleRight);
                    right -= cw + D35.IconTextGap;
                }
                Draw.Text(new Rect(x, r.y, Mathf.Max(0f, right - x), r.height), label, D35.FontBody, slot ? Pal.InkDim : Color.Lerp(Pal.InkSub, Pal.InkTxt, t));
            }
            CSCommonToolbar.Tip35(r, tip);
            return !slot && !on && Draw.Button(r);
        }

        /// <summary>목록 묶음 이름(채팅 '정보' · '레이스'): 칸 r(높이 16)에 캡션 12 InkDim · 좌우 12. 위 16 · 아래 8 은 부르는 쪽 간격(GroupLabelTop · Bottom).</summary>
        public static void Group(Rect r, string label)
        {
            if (Draw.Repaint) Draw.Text(new Rect(r.x + D35.NavPadX, r.y, Mathf.Max(0f, r.width - D35.NavPadX * 2f), r.height), label, D35.FontCaption, Pal.InkDim);
        }
    }

    /// <summary>
    /// 3.5 화면 머리 줄(진행 중 대시보드 · 길잡이, 카드 아님): 높이 56 · 제목 20/28 400 InkTxt → 12 → 칩(부르는 쪽) / 제목 아래 4 캡션 12 InkDim · 오른쪽 버튼(사이 16).
    /// </summary>
    public static class PageHead35
    {
        /// <summary>
        /// 제목 · 캡션을 그리고 제목 줄 오른쪽(칩이 시작할 x = 제목 끝 + 12)과 제목 줄 세로 가운데 titleCy 를 돌려줌.
        /// 오른쪽 버튼은 Button35.AtRight(ref right, titleCy …) 로(right = r.xMax, 사이는 PageHeadItemGap 16 으로 옮김).
        /// </summary>
        public static float Paint(Rect r, string title, string caption, out float titleCy)
        {
            bool cap = !string.IsNullOrEmpty(caption);
            float top = Mathf.Round(r.center.y - (D35.LineHeading + (cap ? D35.KpiGap + D35.LineCaption : 0f)) * 0.5f);
            titleCy = top + D35.LineHeading * 0.5f;
            float tw = Mathf.Ceil(Draw.TextWidth(title ?? "", D35.FontHeading));
            if (Draw.Repaint)
            {
                Draw.Text(new Rect(r.x, top, tw + 2f, D35.LineHeading), title, D35.FontHeading, Pal.InkTxt);
                if (cap) Draw.Num(new Rect(r.x, top + D35.LineHeading + D35.KpiGap, r.width, D35.LineCaption), caption, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleLeft);
            }
            return r.x + tw + D35.PageHeadGap;
        }

        /// <summary>
        /// 설명 붙은 머리(3.7 — 설정 오른쪽 머리, 카드 아님): r 위부터 높이 PageHeadDescH 52 = 제목 20/28 InkTxt → 4 → 설명 14/20 InkSub.
        /// meta('항목 12개')는 제목 줄 오른쪽 12 InkDim(숫자 자리 폭 같음) — 제목은 meta 앞 16 까지.
        /// </summary>
        public static void Desc(Rect r, string title, string desc, string meta = null)
        {
            if (!Draw.Repaint) return;
            float mw = string.IsNullOrEmpty(meta) ? 0f : Mathf.Ceil(Draw.NumWidth(meta, D35.FontCaption));
            if (mw > 0f) Draw.Num(new Rect(r.xMax - mw, r.y, mw, D35.LineHeading), meta, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleRight);
            Draw.Text(new Rect(r.x, r.y, Mathf.Max(0f, r.width - (mw > 0f ? mw + D35.PageHeadItemGap : 0f)), D35.LineHeading), title, D35.FontHeading, Pal.InkTxt);
            if (!string.IsNullOrEmpty(desc)) Draw.Text(new Rect(r.x, r.y + D35.LineHeading + D35.KpiGap, r.width, D35.LineBody), desc, D35.FontBody, Pal.InkSub);
        }
    }

    /// <summary>날 칸 꼴: 다른 달 · 이 달 · 오늘.</summary>
    public enum DayTone { Other, This, Today }

    /// <summary>일정 칩 꼴: 세션(예선 · 결승) · 기간(주간 준비 · 정리) · 끝남 · 고름(수정 모달 열림) · 빈 주말(추가).</summary>
    public enum CalChip { Session, Period, Done, Selected, Empty }

    /// <summary>
    /// 3.5 달력(일정 탭): 7열 격자 · 요일 줄 28. 작은 달력 날 줄 32(숫자 상자 24 × 20 r4 · 글 12 → 2 → 점 4) ·
    /// 큰 달력 주 줄 120(위 1px Hairline · 칸 사이 세로 Hairline · 날 상자 24 r4 위 · 왼쪽 4 · 일정 칩 24 사이 4 · 좌우 2 · 안 8 · r4 · 글 12).
    /// 오늘 = 상자 면 InkTxt · 글 GroundDeep, 이 달 InkSub, 다른 달 InkDim.
    /// </summary>
    public static class Calendar35
    {
        /// <summary>격자 grid(요일 줄 28 포함)에서 rows 줄 중 i 번째 날 칸(0 = 첫 줄 첫 칸).</summary>
        public static Rect Cell(Rect grid, int rows, int i)
        {
            rows = Mathf.Max(1, rows);
            float w = grid.width / 7f, h = (grid.height - D35.CalHeadH) / rows;
            return new Rect(grid.x + w * (i % 7), grid.y + D35.CalHeadH + h * (i / 7), w, h);
        }

        /// <summary>요일 줄(높이 28): 캡션 12 InkDim — 작은 달력 가운데, 큰 달력(left) 왼쪽 8 들임.</summary>
        public static void Weekdays(Rect grid, string[] names, bool left)
        {
            if (!Draw.Repaint || names == null) return;
            float w = grid.width / 7f;
            for (int i = 0; i < 7 && i < names.Length; i++)
            {
                var c = new Rect(grid.x + w * i + (left ? D35.Step8 : 0f), grid.y, w - (left ? D35.Step8 : 0f), D35.CalHeadH);
                Draw.Text(c, names[i], D35.FontCaption, Pal.InkDim, false, left ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter);
            }
        }

        static Color DayInk(DayTone t) => t == DayTone.Today ? Pal.GroundDeep : t == DayTone.This ? Pal.InkSub : Pal.InkDim;

        /// <summary>작은 달력 날 칸: 숫자 상자 24 × 20(칸 위 가운데) · 세션 있는 날 점 4 InkSub(상자 아래 2). 누르면 true.</summary>
        public static bool MiniDay(Rect cell, string day, DayTone tone, bool dot)
        {
            float top = Mathf.Round(cell.center.y - (D35.CalMiniBoxH + D35.CalDotGap + D35.CalDot) * 0.5f);
            var box = new Rect(Mathf.Round(cell.center.x - D35.CalMiniBoxW * 0.5f), top, D35.CalMiniBoxW, D35.CalMiniBoxH);
            if (Draw.Repaint)
            {
                if (tone == DayTone.Today) Draw.Round(box, Pal.InkTxt, D35.RadiusTag);
                Draw.Num(box, day, D35.FontCaption, DayInk(tone), TextAnchor.MiddleCenter);
                if (dot) Draw.Dot(new Vector2(box.center.x, box.yMax + D35.CalDotGap + D35.CalDot * 0.5f), D35.CalDot * 0.5f, Pal.InkSub);
            }
            return Draw.Button(cell);
        }

        /// <summary>큰 달력 선: 주마다 위 1px Hairline · 칸 사이 세로 1px Hairline(첫 열 왼쪽 없음).</summary>
        public static void Lines(Rect grid, int rows)
        {
            if (!Draw.Repaint) return;
            float w = grid.width / 7f, h = (grid.height - D35.CalHeadH) / Mathf.Max(1, rows), y0 = grid.y + D35.CalHeadH;
            for (int k = 0; k < rows; k++) Draw.Fill(new Rect(grid.x, y0 + h * k, grid.width, 1f), Pal.Hairline);
            for (int c = 1; c < 7; c++) Draw.Fill(new Rect(grid.x + w * c, y0, 1f, grid.yMax - y0), Pal.Hairline);
        }

        /// <summary>큰 달력 날 글(위 4 · 왼쪽 4): 상자 높이 24 · 최소 24 · 좌우 4 · 글 12 숫자('6월 1일' 도 그대로).</summary>
        public static void DayLabel(Rect cell, string text, DayTone tone)
        {
            if (!Draw.Repaint) return;
            float w = Mathf.Max(D35.CalDayBox, Mathf.Ceil(Draw.NumWidth(text ?? "", D35.FontCaption)) + D35.CalPad * 2f);
            var box = new Rect(cell.x + D35.CalPad, cell.y + D35.CalPad, w, D35.CalDayBox);
            if (tone == DayTone.Today) Draw.Round(box, Pal.InkTxt, D35.RadiusTag);
            Draw.Num(box, text, D35.FontCaption, DayInk(tone), TextAnchor.MiddleCenter);
        }

        /// <summary>날 칸 안 i 번째 일정 칩 자리: 날 글 아래(위 4 + 24 + 4) · 칩 24 · 사이 4 · 좌우 2.</summary>
        public static Rect ChipRect(Rect cell, int i) =>
            new(cell.x + D35.CalChipInset, cell.y + D35.CalPad + D35.CalDayBox + D35.CalPad + (D35.CalChipH + D35.CalChipGap) * i, cell.width - D35.CalChipInset * 2f, D35.CalChipH);

        /// <summary>날 칸에 들어가는 칩 수.</summary>
        public static int ChipsFit(Rect cell) =>
            Mathf.Max(0, Mathf.FloorToInt((cell.height - D35.CalPad * 3f - D35.CalDayBox + D35.CalChipGap) / (D35.CalChipH + D35.CalChipGap)));

        /// <summary>
        /// 일정 칩(r = ChipRect, 기간 칩은 여러 날에 걸친 칸을 넘겨도 됨): 안 좌우 8 · r4 · 점 8 → 8 → 글 12. 꼴은 CalChip — 세션 면 없음 · 기간 GroundRaised ·
        /// 끝남 Hairline 테 · 점 · 글 InkDim · 고름 GroundRaised + FocusLine 테 · 빈 주말 FocusLine 테 · Plus 16 · InkSub. 점 = 시리즈 강조(accent). 누르면 true.
        /// </summary>
        public static bool Chip(Rect r, string text, CalChip kind, Color accent, string tip = null)
        {
            if (Draw.Repaint)
            {
                if (kind is CalChip.Period or CalChip.Selected) Draw.Round(r, Pal.GroundRaised, D35.RadiusTag);
                if (kind == CalChip.Done) Draw.RoundOutline(r, Pal.Hairline, D35.RadiusTag, 1f);
                if (kind is CalChip.Selected or CalChip.Empty) Draw.RoundOutline(r, Pal.FocusLine, D35.RadiusTag, 1f);
                float x = r.x + D35.CalChipPadX;
                if (kind == CalChip.Empty)
                {
                    Draw.Icon(Icons.Plus, new Rect(x, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), Pal.InkSub, D35.IconBox, D35.IconLine);
                    x += D35.IconS + D35.IconTextGap;
                }
                else
                {
                    StatusDot.Paint(new Vector2(x + D35.Dot * 0.5f, r.center.y), kind == CalChip.Done ? Pal.InkDim : accent);
                    x += D35.Dot + D35.IconTextGap;
                }
                Color ink = kind == CalChip.Done ? Pal.InkDim : kind == CalChip.Empty ? Pal.InkSub : Pal.InkTxt;
                Draw.Text(new Rect(x, r.y, Mathf.Max(0f, r.xMax - D35.CalChipPadX - x), r.height), text, D35.FontCaption, ink);
            }
            CSCommonToolbar.Tip35(r, tip);
            return Draw.Button(r);
        }
    }
}
