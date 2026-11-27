using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 표(타이밍 · 순위 · 명단): 머리 28(12 InkDim) · 줄 36(타이밍) / 44(순위) · 칸 사이 12 · 줄 사이 0 · 구분선 없음.
    /// 선두 · 고른 줄 = GroundRaised r8(좌우 8 밖 — .x8). 숫자 열은 Draw.Num 오른쪽 맞춤. 할당 0: 칸 폭 · 칸 배열은 부르는 쪽이 들고 있음.
    /// </summary>
    public static class Table35
    {
        /// <summary>
        /// 줄 row 를 칸으로 나눠 into 에 채움: widths[i] &gt; 0 = 고정 폭, ≤ 0 = 남는 폭(그런 칸이 여럿이면 같게 나눔). 칸 사이 12. into 길이 ≥ widths 길이.
        /// </summary>
        public static void Cols(Rect row, float[] widths, Rect[] into)
        {
            int n = widths.Length, flex = 0;
            float fixedW = D35.TableColGap * (n - 1);
            for (int i = 0; i < n; i++) { if (widths[i] > 0f) fixedW += widths[i]; else flex++; }
            float flexW = flex > 0 ? Mathf.Max(0f, row.width - fixedW) / flex : 0f, x = row.x;
            for (int i = 0; i < n; i++)
            {
                float w = widths[i] > 0f ? widths[i] : flexW;
                into[i] = new Rect(x, row.y, w, row.height);
                x += w + D35.TableColGap;
            }
        }

        /// <summary>표 머리 글(12 InkDim): right[i] = 그 칸 오른쪽 맞춤(숫자 열). cols = Cols 로 나눈 머리 줄 칸.</summary>
        public static void Head(Rect[] cols, string[] heads, bool[] right)
        {
            if (!Draw.Repaint || heads == null) return;
            for (int i = 0; i < heads.Length && i < cols.Length; i++)
                Draw.Text(cols[i], heads[i], D35.FontCaption, Pal.InkDim, false, right != null && i < right.Length && right[i] ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft);
        }

        /// <summary>
        /// 줄 바탕: lead = 선두 · 고른 줄 면 GroundRaised(r8 · 좌우 8 밖). clickable = 호버 면(GroundRaised 0.15초)을 그리고 눌러 떼면 true(H2H 고르기 — 캔버스 없음).
        /// 글보다 먼저 부름.
        /// </summary>
        public static bool Row(Rect r, bool lead, bool clickable = false)
        {
            float hover = clickable ? CSCommonAnim.Toward(Look.Key(r, "row35t"), Draw.Hover(r) ? 1f : 0f, 1f / D35.DurFeedback) : 0f;
            if (Draw.Repaint)
            {
                var b = RowItem.Bleed(r);
                if (lead) Draw.Round(b, Pal.GroundRaised, D35.RadiusCtrl);
                else if (hover > 0.01f) Draw.Round(b, Draw.Alpha(Pal.GroundRaised, hover), D35.RadiusCtrl);
            }
            return clickable && Draw.Button(r);
        }
    }
}
