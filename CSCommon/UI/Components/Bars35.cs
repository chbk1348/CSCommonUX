using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 점수 막대(FIA 캔버스 R5DashLive .pbt): 높이 6 · 반지름 3 · 홈 GroundRaised · 지금 점수 InkSub · 이대로 끝나면 더해질 몫 = 강조 56%.
    /// 줄 44 안 가운데에 두고 오른쪽 숫자 44 · 더할 수 32 는 부르는 쪽(Draw.Num). 뜻은 아래 캡션으로 적음.
    /// </summary>
    public static class ScoreBar
    {
        /// <summary>막대 칸 r(높이 ScoreBarH)에 now · add 를 max 기준으로 그림.</summary>
        public static void Paint(Rect r, float now, float add, float max, Color accent)
        {
            if (!Draw.Repaint) return;
            Split(now, add, max, r.width, out float nowW, out float addW);
            Draw.Round(r, Pal.GroundRaised, D35.ScoreBarR);
            if (nowW + addW > 0f) Draw.Round(new Rect(r.x, r.y, nowW + addW, r.height), Draw.Alpha(accent, D35.AccentAddAlpha), D35.ScoreBarR);
            if (nowW > 0f) Draw.Round(new Rect(r.x, r.y, nowW, r.height), Pal.InkSub, D35.ScoreBarR);
        }

        /// <summary>폭 나눔: now · add 를 0 이상으로, 합이 max 를 넘으면 막대 끝에서 자름. max ≤ 0 이면 0.</summary>
        public static void Split(float now, float add, float max, float width, out float nowW, out float addW)
        {
            if (max <= 0f || width <= 0f) { nowW = addW = 0f; return; }
            nowW = width * Mathf.Clamp01(now / max);
            addW = Mathf.Min(width - nowW, width * Mathf.Clamp01(Mathf.Max(0f, add) / max));
        }
    }

    /// <summary>
    /// 3.5 단계 막대 · 섹터 3분할 막대(FIA 캔버스 R5DashLive .trk · R5PartRows): 높이 8 · 반지름 4 · 칸 사이 4.
    /// 채운 칸 = 강조, 나머지 = 홈(놓인 면보다 한 단 — Surface.Raise). 진행 · 랩 · 단계 표시에만 씀.
    /// </summary>
    public static class StageTrack
    {
        /// <summary>막대 칸 r 을 count 칸(사이 4)으로 나눈 i 번째 칸.</summary>
        public static Rect Cell(Rect r, int count, int i)
        {
            count = Mathf.Max(1, count);
            float w = (r.width - D35.TrackGap * (count - 1)) / count;
            return new Rect(r.x + (w + D35.TrackGap) * i, r.y, w, r.height);
        }

        /// <summary>count 칸 중 앞 filled 칸을 강조로, 나머지를 home 으로.</summary>
        public static void Paint(Rect r, int count, int filled, Color accent, Color home)
        {
            if (!Draw.Repaint) return;
            for (int i = 0; i < count; i++) Draw.Round(Cell(r, count, i), i < filled ? accent : home, D35.TrackR);
        }

        /// <summary>섹터 3분할(시그니처): 지난 섹터 done 칸 강조 · 남은 섹터 GroundRaised.</summary>
        public static void Sectors(Rect r, int done, Color accent) => Paint(r, 3, done, accent, Pal.GroundRaised);

        /// <summary>막대 아래 8 에 칸마다 라벨 12(칸 왼쪽 맞춤): 지금 단계 current 는 InkTxt, 지난 단계 InkSub, 남은 단계 InkDim(캡션 기본색).</summary>
        public static void Labels(Rect track, string[] labels, int current)
        {
            if (!Draw.Repaint || labels == null) return;
            for (int i = 0; i < labels.Length; i++)
            {
                var c = Cell(track, labels.Length, i);
                Color ink = i == current ? Pal.InkTxt : i < current ? Pal.InkSub : Pal.InkDim;
                Draw.Text(new Rect(c.x, track.yMax + D35.TrackLabelGap, c.width, D35.LineCaption), labels[i], D35.FontCaption, ink);
            }
        }
    }
}
