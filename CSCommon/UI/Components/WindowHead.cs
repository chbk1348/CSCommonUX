using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.WindowHead — 모드 창 머리줄(시안 '올림픽 3.0' 창 머리): 높이 64 · 왼쪽 16 · 오른쪽 12 · 요소 사이 12.
    /// [로고(부르는 쪽이 그림 · LogoW × LogoH)] [이름 15.5 굵게(줄 22) + 옆 작은 글 note · 아래 부제 12 SubInk(줄 17)] … [날짜 14 굵게(줄 20) · 아래 상태 알약 24] [닫기 IconButton 40].
    /// 닫기를 눌러 떼면 true. 머리줄 아래 선 없음(몸 우물이 바로 이어짐).
    /// </summary>
    public static class WindowHead
    {
        public const float H = 64f, PadL = 16f, PadR = 12f, Gap = 12f, LogoW = 45f, LogoH = 30f, NameFont = 15.5f, NameH = 22f, SubFont = 12f, SubH = 17f, LineGap = 2f, DateFont = 14f, DateH = 20f;

        public static bool In(Rect r, System.Action<Rect> logo, string name, string note, string sub, string date, string badge, Color badgeColor, string closeTip)
        {
            bool closed = false;
            float cy = r.y + H * 0.5f, x = r.x + PadL;
            logo?.Invoke(new Rect(x, Mathf.Round(cy - LogoH * 0.5f), LogoW, LogoH));
            x += LogoW + Gap;
            float right = r.xMax - PadR - IconButton.Size;
            IconButton.At(right, cy, Icons.Close, false, closeTip, true, () => closed = true);
            right -= Gap;
            float bw = string.IsNullOrEmpty(badge) ? 0f : Badge.At(0f, 0f, badge, badgeColor, null, false);
            float dw = Mathf.Ceil(Mathf.Max(Draw.TextWidth(date ?? "", DateFont, true), bw));
            if (Draw.Repaint)
            {
                float top = Mathf.Round(cy - (DateH + LineGap + Badge.H) * 0.5f);
                Draw.Text(new Rect(right - dw, top, dw, DateH), date ?? "", DateFont, Look.BarInk, true, TextAnchor.MiddleRight);
                if (bw > 0f) Badge.At(right - bw, top + DateH + LineGap + Badge.H * 0.5f, badge, badgeColor);
                float room = Mathf.Max(0f, right - dw - Gap - x), nt = Mathf.Round(cy - (NameH + LineGap + SubH) * 0.5f);
                float nw = Mathf.Min(room, Mathf.Ceil(Draw.TextWidth(name ?? "", NameFont, true)));
                Draw.TextFit(new Rect(x, nt, nw + 2f, NameH), name, NameFont, Look.BarInk, true);
                if (!string.IsNullOrEmpty(note)) Draw.TextFit(new Rect(x + nw + LineGap * 3f, nt, Mathf.Max(0f, room - nw - LineGap * 3f), NameH), note, SubFont, Section.SubInk);
                Draw.TextFit(new Rect(x, nt + NameH + LineGap, room, SubH), sub, SubFont, Section.SubInk);
            }
            return closed;
        }
    }
}
