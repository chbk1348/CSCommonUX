using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.8 순위 변화 칸(FIA 캔버스 R5BcPit 순위 칸 · R5Teams 변화 열 · R5BcPostStand): 폭 ColMove 36 · 아이콘 12 → 2 → 수 12(400).
    /// 오름 Up SemGood · 내림 Chevron SemBad · 그대로 Minus InkDim(수 없음). 수 글은 부르는 쪽이 값이 바뀔 때만 지어 넘김(할당 0).
    /// 방송 판(다른 글꼴 · 크기)은 Icon · Ink 만 골라 씀.
    /// </summary>
    public static class Move35
    {
        /// <summary>변화 방향(양수 = 오름)의 아이콘.</summary>
        public static string Icon(int change) => change > 0 ? Icons.Up : change < 0 ? Icons.Chevron : Icons.Minus;

        /// <summary>변화 방향의 색.</summary>
        public static Color Ink(int change) => change > 0 ? Pal.SemGood : change < 0 ? Pal.SemBad : Pal.InkDim;

        /// <summary>칸 r(왼쪽 맞춤 · 세로 가운데)에 아이콘 12 + 수 count(그대로면 안 씀).</summary>
        public static void Paint(Rect r, int change, string count)
        {
            if (!Draw.Repaint) return;
            Color ink = Ink(change);
            float cy = r.center.y;
            Draw.Icon(Icon(change), new Rect(r.x, Mathf.Round(cy - D35.IconXs * 0.5f), D35.IconXs, D35.IconXs), ink, D35.IconBox, D35.IconLine);
            if (change == 0 || string.IsNullOrEmpty(count)) return;
            float x = r.x + D35.IconXs + D35.MoveGap;
            Draw.Text(new Rect(x, r.y, Mathf.Max(0f, r.xMax - x), r.height), count, D35.FontCaption, ink);
        }
    }

    /// <summary>
    /// 3.8 포디움 칸 자리(R5Main 지난 레이스 · R5BcPostPodium): 칸 셋을 2 · 1 · 3위 차례로 왼쪽부터 · 폭 같음 · 사이 gap · 바닥 맞춤.
    /// 자리만 셈 — 판 · 글은 부르는 쪽 룩(3.5 안쪽 칸 · 방송 판)으로 그림.
    /// </summary>
    public static class Podium35
    {
        /// <summary>place(0 = 1위 · 1 = 2위 · 2 = 3위)가 놓이는 열(0 왼쪽 ~ 2 오른쪽).</summary>
        public static int Column(int place) => place == 0 ? 1 : place == 1 ? 0 : 2;

        /// <summary>바닥 bottom 에 맞춘 place 칸: x = left + 열 × (w + gap) · 높이 first / second / third.</summary>
        public static Rect Slot(float left, float bottom, int place, float w, float gap, float first, float second, float third)
        {
            float h = place == 0 ? first : place == 1 ? second : third;
            return new Rect(left + Column(place) * (w + gap), bottom - h, w, h);
        }
    }
}
