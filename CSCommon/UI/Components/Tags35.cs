using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 태그(FIA 캔버스 .tag · 번호 칸 · 글 태그). 모두 r4 · 글 가운데 · 숫자 자리 폭 같음(Draw.Num, 600).
    /// 면은 놓인 면보다 한 단(Surface.Raise — 카드 위 GroundRaised · 안쪽 칸 위 GroundCard)을 부르는 쪽이 넘김.
    /// </summary>
    public static class Tag35
    {
        /// <summary>번호 · 약어 태그 폭: 좌우 4 + 글, 최소 minW(32 — 명단 · 이적은 TagMinWide 40).</summary>
        public static float Width(string text, float minW = D35.TagMinW) =>
            Mathf.Max(minW, Mathf.Ceil(Draw.NumWidth(text ?? "", D35.FontCaption)) + D35.TagPadX * 2f);

        /// <summary>번호 · 약어 태그(높이 24 · 글 12 InkTxt)를 왼쪽 x · 세로 가운데 cy 에 그리고 폭을 돌려줌. line = Hairline 테(레이스 표 · 교대 기록).</summary>
        public static float Paint(float x, float cy, string text, Color face, bool line = false, float minW = D35.TagMinW)
        {
            float w = Width(text, minW);
            if (!Draw.Repaint) return w;
            var r = new Rect(x, Mathf.Round(cy - D35.TagH * 0.5f), w, D35.TagH);
            Draw.Round(r, face, D35.RadiusTag);
            if (line) Draw.RoundOutline(r, Pal.Hairline, D35.RadiusTag, 1f);
            Draw.Num(r, text, D35.FontCaption, Pal.InkTxt, TextAnchor.MiddleCenter);
            return w;
        }

        /// <summary>
        /// 글 태그(노선 '2호선' · 시설 매력 %): 높이 24 · 좌우 8 · 글 12 600 · 폭 = 글(w &gt; 0 이면 고정 — 시설 퍼센트 TagPercentW 48).
        /// face · ink 는 부르는 쪽(도시 GroundCard · InkTxt, 시설 단계 색 — 50% 밑은 InkDim 면 · GroundDeep 글). 폭을 돌려줌.
        /// </summary>
        public static float Label(float x, float cy, string text, Color face, Color ink, float w = 0f)
        {
            if (w <= 0f) w = Mathf.Ceil(Draw.NumWidth(text ?? "", D35.FontCaption)) + D35.TagLabelPadX * 2f;
            if (!Draw.Repaint) return w;
            var r = new Rect(x, Mathf.Round(cy - D35.TagH * 0.5f), w, D35.TagH);
            Draw.Round(r, face, D35.RadiusTag);
            Draw.Num(r, text, D35.FontCaption, ink, TextAnchor.MiddleCenter);
            return w;
        }

        /// <summary>
        /// 3.8 아이콘 글 태그(R5BcBattle '오버테이크 모드'): 높이 24 · 좌우 8 · r4 · 아이콘 12(iconInk) → 4 → 글 12 InkTxt(400). centerX = 태그 가운데 · 폭을 돌려줌.
        /// </summary>
        public static float IconLabel(float centerX, float cy, string icon, Color iconInk, string text, Color face)
        {
            float w = D35.TagLabelPadX * 2f + D35.IconXs + D35.LinkGap + Mathf.Ceil(Draw.TextWidth(text ?? "", D35.FontCaption));
            if (!Draw.Repaint) return w;
            var r = new Rect(Mathf.Round(centerX - w * 0.5f), Mathf.Round(cy - D35.TagH * 0.5f), w, D35.TagH);
            Draw.Round(r, face, D35.RadiusTag);
            float x = r.x + D35.TagLabelPadX;
            Draw.Icon(icon, new Rect(x, Mathf.Round(cy - D35.IconXs * 0.5f), D35.IconXs, D35.IconXs), iconInk, D35.IconBox, D35.IconLine);
            x += D35.IconXs + D35.LinkGap;
            Draw.Text(new Rect(x, r.y, r.xMax - x, r.height), text, D35.FontCaption, Pal.InkTxt);
            return w;
        }

        /// <summary>차 번호 칸 32 × 20 · 글 12 600 InkTxt. 면: NASCAR GroundRaised · WEC 팀 색 40%(TeamFace).</summary>
        public static void Car(float x, float cy, string number, Color face)
        {
            if (!Draw.Repaint) return;
            var r = new Rect(x, Mathf.Round(cy - D35.CarTagH * 0.5f), D35.CarTagW, D35.CarTagH);
            Draw.Round(r, face, D35.RadiusTag);
            Draw.Num(r, number, D35.FontCaption, Pal.InkTxt, TextAnchor.MiddleCenter);
        }

        /// <summary>팀 색 면(팀 색 40%).</summary>
        public static Color TeamFace(Color team) => Draw.Alpha(team, D35.TeamFaceAlpha);
    }

    /// <summary>
    /// 3.5 타이어 표시(레이스 표): 원 20 · 테 1.5 · 글자 12 가운데 · 테 · 글 = 타이어 색(소프트 SemBad · 미디엄 SemWarn · 하드 InkTxt — 인터 · 웨트 색은 캔버스 없음, 모드가 고름).
    /// </summary>
    public static class TyreBadge35
    {
        public static void Paint(Vector2 center, string letter, Color ink)
        {
            if (!Draw.Repaint) return;
            Draw.Ring(center, D35.TyreD * 0.5f, D35.TyreRing, ink);
            Draw.Num(new Rect(center.x - D35.TyreD * 0.5f, center.y - D35.TyreD * 0.5f, D35.TyreD, D35.TyreD), letter, D35.FontCaption, ink, TextAnchor.MiddleCenter);
        }
    }

    /// <summary>
    /// 3.5 아바타(뉴스 반응 · 채팅): 원 20 · 24 · 28 · 32(글 12, 32 만 14) · 면 = 놓인 면 한 단 · 글 InkSub. 말하는 중 = 둘레 2 SemGood.
    /// 상태 점 8(온라인 SemGood · 제재 SemBad · 오프라인 InkDim)은 오른쪽 −2 · 아래 −2 · 둘레 2(놓인 바탕색).
    /// </summary>
    public static class Avatar35
    {
        public static void Paint(Vector2 center, float d, string initials, Color face, bool speaking = false, Color? dot = null, Color dotRing = default)
        {
            if (!Draw.Repaint) return;
            float r = d * 0.5f;
            Draw.Dot(center, r, face);
            if (speaking) Draw.Ring(center, r, D35.DotRing, Pal.SemGood);
            var box = new Rect(center.x - r, center.y - r, d, d);
            Draw.Text(box, initials, d >= D35.AvatarL ? D35.FontBody : D35.FontCaption, Pal.InkSub, false, TextAnchor.MiddleCenter);
            if (!dot.HasValue) return;
            var p = new Vector2(box.xMax + D35.AvatarDotOut - D35.Dot * 0.5f, box.yMax + D35.AvatarDotOut - D35.Dot * 0.5f);
            Draw.Dot(p, D35.Dot * 0.5f + D35.DotRing, dotRing);
            Draw.Dot(p, D35.Dot * 0.5f, dot.Value);
        }

        /// <summary>봇 칸: 32 정사각 · r8 · 면 face(GroundCard) · 아이콘 16 InkDim(레이스 봇 Flag).</summary>
        public static void Bot(Rect r, string icon, Color face)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, face, D35.RadiusCtrl);
            Draw.Icon(icon, new Rect(r.center.x - D35.IconS * 0.5f, r.center.y - D35.IconS * 0.5f, D35.IconS, D35.IconS), Pal.InkDim, D35.IconBox, D35.IconLine);
        }
    }
}
