using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 목록 행(FIA 캔버스 R5DashLive '빠른 실행' .qk): 줄 44 · 아이콘 18 InkSub → 12 → 제목 14 InkTxt (+ 아래 캡션 12 InkDim, 두 줄).
    /// 호버 = GroundRaised 면 r8(줄 좌우 8 밖으로), 면 색만 0.15초. 꺼짐 = 글 · 아이콘 InkDim, 호버 · 누름 없음.
    /// </summary>
    public static class RowItem
    {
        /// <summary>행을 그리고 누르면 true. icon = Icons 경로(없으면 null — 글이 왼쪽 끝에서 시작).</summary>
        public static bool Paint(Rect r, string icon, string title, string caption = null, bool enabled = true)
        {
            int key = Look.Key(r, "row35");
            float hover = CSCommonAnim.Toward(key, enabled && Draw.Hover(r) ? 1f : 0f, 1f / D35.DurFeedback);
            if (Draw.Repaint)
            {
                if (hover > 0.01f) Draw.Round(new Rect(r.x - D35.Step8, r.y, r.width + D35.Step16, r.height), Draw.Alpha(Pal.GroundRaised, hover), D35.RadiusCtrl);
                float x = r.x;
                if (icon != null)
                {
                    Draw.Icon(icon, new Rect(x, r.center.y - D35.IconM * 0.5f, D35.IconM, D35.IconM), enabled ? Pal.InkSub : Pal.InkDim, D35.IconBox, D35.IconLine);
                    x += D35.IconM + D35.RowIconGap;
                }
                float w = r.xMax - x;
                Color ink = enabled ? Pal.InkTxt : Pal.InkDim;
                if (string.IsNullOrEmpty(caption)) Draw.Text(new Rect(x, r.y, w, r.height), title, D35.FontBody, ink);
                else
                {
                    float top = r.y + (r.height - D35.LineBody - D35.LineCaption) * 0.5f;
                    Draw.Text(new Rect(x, top, w, D35.LineBody), title, D35.FontBody, ink);
                    Draw.Text(new Rect(x, top + D35.LineBody, w, D35.LineCaption), caption, D35.FontCaption, Pal.InkDim);
                }
            }
            return enabled && Draw.Button(r);
        }

        /// <summary>줄 앞 팀 색 막대 4 × 16 · 반지름 2(왼쪽 x, 세로 가운데 centerY). 팀 색은 자료.</summary>
        public static void TeamBar(float x, float centerY, Color team) =>
            Draw.Round(new Rect(x, centerY - D35.TeamBarH * 0.5f, D35.TeamBarW, D35.TeamBarH), team, D35.TeamBarR);

        // ---- 3.6 줄 꼴: 한 줄 목록 · 두 줄 메시지 · 단계 · 상태 · 값 줄

        static float Hover(Rect r, string id, bool enabled, Color face)
        {
            float h = CSCommonAnim.Toward(Look.Key(r, id), enabled && Draw.Hover(r) ? 1f : 0f, 1f / D35.DurFeedback);
            if (Draw.Repaint && h > 0.01f) Draw.Round(Bleed(r), Draw.Alpha(face, h), D35.RadiusCtrl);
            return h;
        }

        /// <summary>줄 좌우 8 밖으로 넓힌 칸(.x8 — 호버 · 고른 · 선두 줄 면).</summary>
        public static Rect Bleed(Rect r) => new(r.x - D35.RowBleed, r.y, r.width + D35.RowBleed * 2f, r.height);

        /// <summary>
        /// 한 줄 목록 행(R5Main 빠른 실행 · 경로 목록): 44 · 아이콘 18 InkSub → 12 → 제목 14 InkTxt … 오른쪽 캡션 12 InkDim → 8 → 꺾쇠 Right 16 InkDim.
        /// 호버 = GroundRaised r8(좌우 8 밖) 0.15초. 꺼짐 = 글 · 아이콘 InkDim, 호버 · 누름 없음. 누르면 true.
        /// </summary>
        public static bool Line(Rect r, string icon, string title, string caption = null, bool chevron = true, bool enabled = true)
        {
            Hover(r, "line35", enabled, Pal.GroundRaised);
            if (Draw.Repaint)
            {
                float x = r.x, right = r.xMax;
                Color ink = enabled ? Pal.InkTxt : Pal.InkDim;
                if (icon != null)
                {
                    Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - D35.IconM * 0.5f), D35.IconM, D35.IconM), enabled ? Pal.InkSub : Pal.InkDim, D35.IconBox, D35.IconLine);
                    x += D35.IconM + D35.RowIconGap;
                }
                if (chevron)
                {
                    right -= D35.IconS;
                    Draw.Icon(Icons.Right, new Rect(right, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), Pal.InkDim, D35.IconBox, D35.IconLine);
                    right -= D35.IconTextGap;
                }
                if (!string.IsNullOrEmpty(caption))
                {
                    float cw = Mathf.Ceil(Draw.NumWidth(caption, D35.FontCaption));
                    Draw.Num(new Rect(right - cw, r.y, cw, r.height), caption, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleRight);
                    right -= cw + D35.IconTextGap;
                }
                Draw.Text(new Rect(x, r.y, Mathf.Max(0f, right - x), r.height), title, D35.FontBody, ink);
            }
            return enabled && Draw.Button(r);
        }

        /// <summary>
        /// 두 줄 메시지 줄(레이스 컨트롤): 56 · 위 캡션 줄(점 8 → 8 → '황색 · 15:10' 12 InkDim) → 4 → 본문 14 InkTxt. 점 색 = 메시지 종류(부르는 쪽).
        /// </summary>
        public static void Message(Rect r, Color dot, string caption, string body)
        {
            if (!Draw.Repaint) return;
            float top = Mathf.Round(r.center.y - (D35.LineCaption + D35.KpiGap + D35.LineBody) * 0.5f);
            StatusDot.Paint(new Vector2(r.x + D35.Dot * 0.5f, top + D35.LineCaption * 0.5f), dot);
            float tx = r.x + D35.Dot + D35.IconTextGap;
            Draw.Num(new Rect(tx, top, Mathf.Max(0f, r.xMax - tx), D35.LineCaption), caption, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleLeft);
            Draw.Text(new Rect(r.x, top + D35.LineCaption + D35.KpiGap, r.width, D35.LineBody), body, D35.FontBody, Pal.InkTxt);
        }

        /// <summary>
        /// 단계 줄(길잡이 준비 순서): 44 · 좌우 8 밖 r8 · 번호 칸 28(12 InkDim) · 아이콘 16 → 12 → 제목 14 · 오른쪽 캡션 12 InkDim.
        /// 됨 = 아이콘 SemGood · 제목 InkSub, 지금 = 줄 면 GroundRaised · 아이콘 · 제목 InkTxt, 남음 = 아이콘 InkDim · 제목 InkSub. 누르면 true.
        /// </summary>
        public static bool Step(Rect r, string number, string icon, string title, string caption, StepState state)
        {
            if (Draw.Repaint)
            {
                if (state == StepState.Now) Draw.Round(Bleed(r), Pal.GroundRaised, D35.RadiusCtrl);
                Draw.Num(new Rect(r.x, r.y, D35.StepNumW, r.height), number, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleLeft);
                float x = r.x + D35.StepNumW;
                Color ic = state == StepState.Done ? Pal.SemGood : state == StepState.Now ? Pal.InkTxt : Pal.InkDim;
                if (icon != null) Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), ic, D35.IconBox, D35.IconLine);
                x += D35.IconS + D35.RowIconGap;
                float cw = string.IsNullOrEmpty(caption) ? 0f : Mathf.Ceil(Draw.NumWidth(caption, D35.FontCaption));
                if (cw > 0f) Draw.Num(new Rect(r.xMax - cw, r.y, cw, r.height), caption, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleRight);
                Draw.Text(new Rect(x, r.y, Mathf.Max(0f, r.xMax - cw - D35.IconTextGap - x), r.height), title, D35.FontBody, state == StepState.Now ? Pal.InkTxt : Pal.InkSub);
            }
            return Draw.Button(r);
        }

        /// <summary>
        /// 상태 줄(시설 개최 상태 · 참고할 점): 56 · 아이콘 16(뜻 색 — 됨 SemGood · 참고 SemWarn · 할 일 InkDim) → 8 → 제목 14 InkTxt + 캡션 12(captionInk — InkDim 또는 SemWarn).
        /// 오른쪽 칸(폭 side — 작은 버튼 56 · 값 글)을 돌려줌 — 부르는 쪽이 버튼 · 값을 그 안에 그림.
        /// </summary>
        public static Rect Status(Rect r, string icon, Color iconInk, string title, string caption, Color captionInk, float side = D35.StatusSideW)
        {
            var sideR = new Rect(r.xMax - side, r.y, side, r.height);
            if (Draw.Repaint)
            {
                float top = Mathf.Round(r.center.y - (D35.LineBody + D35.LineCaption) * 0.5f);
                if (icon != null) Draw.Icon(icon, new Rect(r.x, top + (D35.LineBody - D35.IconS) * 0.5f, D35.IconS, D35.IconS), iconInk, D35.IconBox, D35.IconLine);
                float x = r.x + D35.IconS + D35.IconTextGap, w = Mathf.Max(0f, sideR.x - D35.IconTextGap - x);
                Draw.Text(new Rect(x, top, w, D35.LineBody), title, D35.FontBody, Pal.InkTxt);
                if (!string.IsNullOrEmpty(caption)) Draw.Num(new Rect(x, top + D35.LineBody, w, D35.LineCaption), caption, D35.FontCaption, captionInk, TextAnchor.MiddleLeft);
            }
            return sideR;
        }

        /// <summary>
        /// 값 줄(이름 · 캡션 · 값 — 높이 RowValueXs 24 · RowValueS 28 · RowValueM 32 · RowTiming 36 · RowList 44): 이름 14 InkSub → 8 → 캡션 12 InkDim 숫자 … 값 14 오른쪽.
        /// figure = 값을 숫자 600(Draw.Num) 으로, 아니면 글 400. valueInk 기본 InkTxt. line = 줄 아래 1px Hairline(서킷 히어로 · 시설 도시).
        /// </summary>
        public static void Value(Rect r, string name, string caption, string value, bool figure = true, bool line = false, Color? valueInk = null)
        {
            if (!Draw.Repaint) return;
            Color vi = valueInk ?? Pal.InkTxt;
            float vw = Mathf.Ceil(figure ? Draw.NumWidth(value ?? "", D35.FontBody) : Draw.TextWidth(value ?? "", D35.FontBody));
            if (figure) Draw.Num(r, value, D35.FontBody, vi, TextAnchor.MiddleRight);
            else Draw.Text(r, value, D35.FontBody, vi, false, TextAnchor.MiddleRight);
            float nw = Mathf.Ceil(Draw.TextWidth(name ?? "", D35.FontBody));
            Draw.Text(new Rect(r.x, r.y, nw + 2f, r.height), name, D35.FontBody, Pal.InkSub);
            if (!string.IsNullOrEmpty(caption))
            {
                float cx = r.x + nw + D35.IconTextGap;
                Draw.Num(new Rect(cx, r.y, Mathf.Max(0f, r.xMax - vw - D35.IconTextGap - cx), r.height), caption, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleLeft);
            }
            if (line) Draw.Fill(new Rect(r.x, r.yMax - 1f, r.width, 1f), Pal.Hairline);
        }
    }

    /// <summary>단계 줄 상태: 됨 · 지금 · 남음.</summary>
    public enum StepState { Done, Now, Todo }
}
