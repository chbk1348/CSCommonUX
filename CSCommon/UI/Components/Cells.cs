using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Cells — 칸 막대(시안 '준비비 납부' 25칸 · '특수 남은 기간' 24칸): 칸 사이 3 · 반지름 6 · 높이 28.
    /// 앞 filled 칸 = done(없으면 Look.PastelStrong), 나머지 = rest(없으면 Look.PillNeutral), current 칸은 안쪽 2px Look.PastelStrong 테두리.
    /// </summary>
    public static class Cells
    {
        public const float H = 28f, Gap = 3f, Radius = 6f, Ring = 2f;

        public static void In(Rect r, int total, int filled, int current = -1, Color? done = null, Color? rest = null)
        {
            if (!Draw.Repaint || total <= 0) return;
            float w = (r.width - Gap * (total - 1)) / total, rad = Mathf.Min(Radius, w * 0.5f);
            for (int i = 0; i < total; i++)
            {
                var c = new Rect(r.x + i * (w + Gap), r.y, w, r.height);
                Draw.Round(c, i < filled ? done ?? Look.PastelStrong : rest ?? Look.PillNeutral, rad);
                if (i == current) Draw.RoundOutline(c, Look.PastelStrong, rad, Ring);
            }
        }
    }
}
