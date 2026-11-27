using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 빈 · 오류 · 잠김 상태(FIA 캔버스 R5PartStates, 사용자 결정 2026-11-27): 칸 가운데에 원 40(GroundRaised) 안 아이콘 18(잠김 16, InkDim) →
    /// 12 → 제목 14 InkTxt → 4 → 설명 14 InkSub → 16 → 보조 버튼(부르는 쪽). 문구 예 '아직 메시지가 없어요' · '서킷을 확정하면 열려요'.
    /// </summary>
    public static class EmptyState
    {
        /// <summary>내용 높이: 원 40 + 12 + 제목 20 (+ 4 + 설명 20) (+ 16 + 버튼 40).</summary>
        public static float Height(bool desc, bool button) =>
            D35.EmptyCircle + D35.EmptyIconGap + D35.LineBody + (desc ? D35.EmptyDescGap + D35.LineBody : 0f) + (button ? D35.EmptyBtnGap + D35.BtnH : 0f);

        /// <summary>
        /// r 가운데에 그림. button = 버튼 자리를 둠 — 그 줄 칸(폭 r, 높이 40)을 돌려주니 버튼은 그 가운데에. button 이 아니면 높이 0 칸.
        /// icon 은 Icons 경로(Lock · Warn · Chat …).
        /// </summary>
        public static Rect Paint(Rect r, string icon, string title, string desc = null, bool button = false, bool locked = false)
        {
            bool hasDesc = !string.IsNullOrEmpty(desc);
            float y = Mathf.Round(r.center.y - Height(hasDesc, button) * 0.5f);
            if (Draw.Repaint)
            {
                float cx = r.center.x, rad = D35.EmptyCircle * 0.5f, s = locked ? D35.IconS : D35.IconM;
                Draw.Dot(new Vector2(cx, y + rad), rad, Pal.GroundRaised);
                Draw.Icon(icon, new Rect(cx - s * 0.5f, y + rad - s * 0.5f, s, s), Pal.InkDim, D35.IconBox, D35.IconLine);
                float ty = y + D35.EmptyCircle + D35.EmptyIconGap;
                Draw.Text(new Rect(r.x, ty, r.width, D35.LineBody), title, D35.FontBody, Pal.InkTxt, false, TextAnchor.MiddleCenter);
                if (hasDesc) Draw.Text(new Rect(r.x, ty + D35.LineBody + D35.EmptyDescGap, r.width, D35.LineBody), desc, D35.FontBody, Pal.InkSub, false, TextAnchor.MiddleCenter);
            }
            if (!button) return new Rect(r.x, y + Height(hasDesc, false), r.width, 0f);
            return new Rect(r.x, y + Height(hasDesc, false) + D35.EmptyBtnGap, r.width, D35.BtnH);
        }
    }
}
