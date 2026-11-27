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
    }
}
