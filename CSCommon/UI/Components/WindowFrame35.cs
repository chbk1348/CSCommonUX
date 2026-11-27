using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 창 틀(FIA 캔버스 R5Window · R5PartWindow): 창 1180 × 920(CSCommonDesign.WindowW · WindowH) · r20 · 면 GroundPage + 바깥 1px Hairline · 그림자 없음.
    /// 머리 64(좌우 16 · 사이 12): [시리즈 칸] [제목 16/24 + 아래 meta 12/16 InkDim] … [상태 칩] [날짜 14 InkSub · 숫자 자리 폭 같음] [닫기 40]. 헤더에 뒤로 없음(상세 뒤로는 툴바 왼쪽 FAB).
    /// 몸 = 머리 아래 ~ 창 바닥, 면 GroundDeep. 몸 스크롤 영역은 창 바닥까지, 안쪽 위 · 좌우 WindowBodyPad 20 · 아래 BodyBottom(88, 서브 툴바가 있으면 148).
    /// 하단 툴바 · 서브 툴바 · FAB 은 3.1 그대로(Toolbar · SubToolBar).
    /// </summary>
    public static class WindowFrame35
    {
        /// <summary>몸 칸: 머리 아래 ~ 창 바닥.</summary>
        public static Rect Body(Rect window) =>
            new(window.x, window.y + D35.WindowHeadH, window.width, Mathf.Max(0f, window.height - D35.WindowHeadH));

        /// <summary>몸 스크롤 안쪽 아래 여백 = 툴바 안전 영역(sub = 서브 툴바가 있음).</summary>
        public static float BodyBottom(bool sub) => sub ? SubToolBar.SafeH : Toolbar.SafeH;

        /// <summary>창 판(GroundPage · r20) · 몸(GroundDeep, 아래 모서리만 둥글게) · 바깥 Hairline 을 그리고 몸 칸을 돌려줌.</summary>
        public static Rect Frame(Rect window)
        {
            var body = Body(window);
            if (Draw.Repaint)
            {
                Draw.Round(window, Pal.GroundPage, D35.RadiusWindow);
                Draw.RoundBottom(body, Pal.GroundDeep, D35.RadiusWindow);
                Draw.RoundOutline(window, Pal.Hairline, D35.RadiusWindow, 1f);
            }
            return body;
        }

        static float CellW(string label) => Mathf.Ceil(Draw.TextWidth(label ?? "", D35.FontBody)) + D35.SeriesCellPadX * 2f;

        /// <summary>시리즈 칸 판 폭(안쪽 4 · 칸 = 글 + 좌우 12 · 칸 사이 4).</summary>
        public static float SeriesWidth(string[] labels)
        {
            if (labels == null || labels.Length == 0) return 0f;
            float w = D35.SeriesSegPad * 2f + D35.SeriesCellGap * (labels.Length - 1);
            for (int i = 0; i < labels.Length; i++) w += CellW(labels[i]);
            return w;
        }

        /// <summary>
        /// 머리 왼쪽 시리즈 전환 칸(왼쪽 x · 세로 가운데 cy): 판 40 GroundCard + Hairline r8 · 안쪽 4 · 칸 32 r4 · 사이 4 · 글 14.
        /// 고른 칸 = 강조 면 · 강조 위 글(면 색 0.15초), 안 고른 칸 = InkSub 글 · 누르면 0.97배. tips = 칸 툴팁 · 이름.
        /// 판 자리를 돌려주고(창 끌기에서 뺄 칸), 안 고른 칸을 눌러 떼면 picked = 그 번호(없으면 -1).
        /// </summary>
        public static Rect Series(float x, float cy, string[] labels, int cur, Color accent, Color onAccent, string[] tips, out int picked)
        {
            picked = -1;
            var frame = new Rect(x, Mathf.Round(cy - D35.SeriesSegH * 0.5f), SeriesWidth(labels), D35.SeriesSegH);
            if (frame.width <= 0f) return frame;
            if (Draw.Repaint)
            {
                Draw.Round(frame, Pal.GroundCard, D35.RadiusCtrl);
                Draw.RoundOutline(frame, Pal.Hairline, D35.RadiusCtrl, 1f);
            }
            float cx = frame.x + D35.SeriesSegPad;
            for (int i = 0; i < labels.Length; i++)
            {
                var c = new Rect(cx, frame.y + D35.SeriesSegPad, CellW(labels[i]), D35.SeriesCellH);
                bool on = i == cur;
                int key = Look.Key(c, "series35", labels[i]);
                float onT = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / D35.DurFeedback);
                float pr = Look.Press(key ^ 0x6a09e667, c, !on);
                if (Draw.Repaint)
                {
                    Draw.BeginScale(c.center, Look.PressScale(pr, D35.PressScale));
                    if (onT > 0.01f) Draw.Round(c, Draw.Alpha(accent, onT), D35.RadiusTag);
                    Draw.Text(c, labels[i], D35.FontBody, Color.Lerp(Pal.InkSub, onAccent, onT), false, TextAnchor.MiddleCenter);
                    Draw.EndScale();
                }
                if (tips != null && i < tips.Length) CSCommonToolbar.Tip(c, tips[i]);
                if (Draw.Button(c) && !on) picked = i;
                cx += c.width + D35.SeriesCellGap;
            }
            return frame;
        }

        /// <summary>창 닫기 40 정사각(r8 · 투명 면) · Close 18 InkSub. tip = 툴팁 · 이름. 눌러 떼면 true.</summary>
        public static bool Close(Rect r, string tip)
        {
            if (Draw.Repaint)
                Draw.Icon(Icons.Close, new Rect(Mathf.Round(r.center.x - D35.IconM * 0.5f), Mathf.Round(r.center.y - D35.IconM * 0.5f), D35.IconM, D35.IconM), Pal.InkSub, D35.IconBox, D35.IconLine);
            CSCommonToolbar.Tip(r, tip);
            return Draw.Button(r);
        }

        /// <summary>
        /// 머리 나머지(left = 제목이 시작하는 x, 보통 시리즈 칸 오른쪽 + 12): 오른쪽부터 닫기 40 → 12 → 날짜 → 12 → 상태 칩 → 12, 남은 폭에 제목 16/24 + 아래 meta 12/16 InkDim(meta 가 없으면 제목만 가운데).
        /// 상태 칩(status 가 비면 없음)은 날짜 왼쪽 같은 줄 · 면 GroundRaised · 점 = dot(진행 중이면 live — SemBad 를 넘김). 제목 자리가 모자라면 칩을 숨김(제목 우선).
        /// 닫기를 눌러 떼면 true.
        /// </summary>
        public static bool Head(Rect window, float left, string title, string meta, string status, Color dot, bool live, string date, string closeTip)
        {
            float cy = window.y + D35.WindowHeadH * 0.5f;
            float right = window.xMax - D35.WindowHeadPadX - D35.CloseBtn;
            bool closed = Close(new Rect(right, Mathf.Round(cy - D35.CloseBtn * 0.5f), D35.CloseBtn, D35.CloseBtn), closeTip);
            right -= D35.WindowHeadGap;
            if (!string.IsNullOrEmpty(date))
            {
                float dw = Mathf.Ceil(Draw.NumWidth(date, D35.FontBody));
                Draw.Num(new Rect(right - dw, Mathf.Round(cy - D35.LineBody * 0.5f), dw, D35.LineBody), date, D35.FontBody, Pal.InkSub, TextAnchor.MiddleRight);
                right -= dw + D35.WindowHeadGap;
            }
            bool hasMeta = !string.IsNullOrEmpty(meta);
            float nameW = Mathf.Ceil(Mathf.Max(Draw.TextWidth(title ?? "", D35.FontTitle), hasMeta ? Draw.TextWidth(meta, D35.FontCaption) : 0f));
            float cw = string.IsNullOrEmpty(status) ? 0f : Chip35.Width(status, true);
            if (cw > 0f && right - cw - D35.WindowHeadGap - left >= nameW)
            {
                Chip35.Status(right - cw, cy, status, dot, live, Pal.GroundRaised);
                right -= cw + D35.WindowHeadGap;
            }
            if (!Draw.Repaint) return closed;
            float room = Mathf.Max(0f, right - left);
            float top = Mathf.Round(cy - (D35.LineTitle + (hasMeta ? D35.LineCaption : 0f)) * 0.5f);
            Draw.TextFit(new Rect(left, top, room, D35.LineTitle), title, D35.FontTitle, Pal.InkTxt);
            if (hasMeta) Draw.TextFit(new Rect(left, top + D35.LineTitle, room, D35.LineCaption), meta, D35.FontCaption, Pal.InkDim);
            return closed;
        }
    }
}
