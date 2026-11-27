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

        // ---- 3.6: 칸 폭 비율 · 칸 안 부분 채움 · 라벨 둘 · 한 칸 진행 막대 · 섹터 칸 색

        /// <summary>막대 칸 r 을 count 칸(사이 4)으로 나누되 폭은 weights 몫(null 이면 같은 폭 — Cell 과 같음). i 번째 칸.</summary>
        public static Rect Cell(Rect r, int count, float[] weights, int i)
        {
            if (weights == null) return Cell(r, count, i);
            count = Mathf.Max(1, count);
            float sum = 0f, before = 0f;
            for (int k = 0; k < count; k++)
            {
                float w = Segmented35.Weight(weights, k);
                if (k < i) before += w;
                sum += w;
            }
            float room = r.width - D35.TrackGap * (count - 1);
            float x = r.x + (sum > 0f ? room * before / sum : 0f) + D35.TrackGap * i;
            return new Rect(x, r.y, sum > 0f ? room * Segmented35.Weight(weights, i) / sum : 0f, r.height);
        }

        /// <summary>
        /// 칸 폭 비율 · 칸마다 부분 채움(fills[i] 0 ~ 1 — 레이스 히어로 스테이지 7 : 8 : 15 · 100% 88% 0%): 홈 home(놓인 면 한 단) · 채움 fill(레이스 InkSub · 대시보드 강조).
        /// 채움 끝은 칸 모서리를 따라 왼쪽만 둥글고 자른 쪽은 각짐.
        /// </summary>
        public static void Paint(Rect r, int count, float[] weights, float[] fills, Color fill, Color home)
        {
            if (!Draw.Repaint) return;
            for (int i = 0; i < count; i++)
            {
                var c = Cell(r, count, weights, i);
                Draw.Round(c, home, D35.TrackR);
                float f = fills == null || i >= fills.Length ? 0f : Mathf.Clamp01(fills[i]);
                if (f >= 1f) Draw.Round(c, fill, D35.TrackR);
                else if (f > 0f) Draw.RoundPart(new Rect(c.x, c.y, c.width * f, c.height), c, fill, D35.TrackR);
            }
        }

        /// <summary>칸 폭 비율 라벨(막대 아래 8, 칸 왼쪽): 지금 칸 current InkTxt, 지난 칸 past(캔버스 레이스 · NASCAR InkDim, 대시보드 InkSub), 남은 칸 InkDim.</summary>
        public static void Labels(Rect track, string[] labels, int current, float[] weights, Color past)
        {
            if (!Draw.Repaint || labels == null) return;
            for (int i = 0; i < labels.Length; i++)
            {
                var c = Cell(track, labels.Length, weights, i);
                Color ink = i == current ? Pal.InkTxt : i < current ? past : Pal.InkDim;
                Draw.Text(new Rect(c.x, track.yMax + D35.TrackLabelGap, c.width, D35.LineCaption), labels[i], D35.FontCaption, ink);
            }
        }

        /// <summary>칸 하나 아래 8 에 라벨 둘: 왼쪽 이름 12(ink) · 오른쪽 값 12 숫자 InkDim(대시보드 단계 이름 · 시각).</summary>
        public static void LabelPair(Rect cell, string name, string value, Color ink)
        {
            if (!Draw.Repaint) return;
            var row = new Rect(cell.x, cell.yMax + D35.TrackLabelGap, cell.width, D35.LineCaption);
            Draw.Text(row, name, D35.FontCaption, ink);
            Draw.Num(row, value, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleRight);
        }

        /// <summary>한 칸 진행 막대(높이 8 · r4): 홈 home · 채움 t(0 ~ 1) fill. Meter35 와 같음.</summary>
        public static void Progress(Rect r, float t, Color fill, Color home) => Meter35.Paint(r, t, fill, home);

        /// <summary>섹터 칸 색: 아직 Hairline · 지남 ChartGray4 · 개인 최고 SemGood · 전체 최고 강조(지운 기록 색은 캔버스 없음).</summary>
        public static Color SectorColor(SectorMark m, Color accent) => m switch
        {
            SectorMark.Done => Pal.ChartGray4,
            SectorMark.Personal => Pal.SemGood,
            SectorMark.Overall => accent,
            _ => Pal.Hairline,
        };

        /// <summary>이번 랩 섹터 칸 셋(레이스 표 — 폭 72 · 높이 8 · r4 · 사이 4, 칸마다 색).</summary>
        public static void SectorCells(Rect r, SectorMark s1, SectorMark s2, SectorMark s3, Color accent)
        {
            if (!Draw.Repaint) return;
            Draw.Round(Cell(r, 3, 0), SectorColor(s1, accent), D35.TrackR);
            Draw.Round(Cell(r, 3, 1), SectorColor(s2, accent), D35.TrackR);
            Draw.Round(Cell(r, 3, 2), SectorColor(s3, accent), D35.TrackR);
        }
    }

    /// <summary>섹터 칸 상태: 아직 안 지남 · 지남 · 개인 최고 · 전체 최고.</summary>
    public enum SectorMark { Pending, Done, Personal, Overall }

    /// <summary>
    /// 3.5 가는 막대(WEC 연료 40 × 8 · 레이더 섹터 비 · 팬층 · 인기 4): 높이 = r 높이(8 은 TrackH, 4 는 ThinBarH) · 반지름 = 높이 ÷ 2.
    /// 홈 home(놓인 면 한 단, 보통 GroundRaised) · 채움 t(0 ~ 1) fill(부르는 쪽 — InkSub · SemWarn · Pal.Rain3 · 1위 InkTxt · 나머지 InkDim).
    /// </summary>
    public static class Meter35
    {
        public static void Paint(Rect r, float t, Color fill, Color home)
        {
            if (!Draw.Repaint) return;
            float rad = r.height * 0.5f;
            Draw.Round(r, home, rad);
            t = Mathf.Clamp01(t);
            if (t > 0f) Draw.Round(new Rect(r.x, r.y, Mathf.Max(r.height, r.width * t), r.height), fill, rad);
        }
    }

    /// <summary>
    /// 3.5 쌓인 막대(팀 포인트 점유율 12 · 바깥 r4 · 조각 사이 2 · 1위 강조 · 다음 Chart35.Gray) · 몫 막대(NASCAR 스테이지 · 결승 몫 — 앞 조각 각짐 · 뒤 끝 r4).
    /// 조각은 바깥 끝만 둥글고 가운데는 각짐. 할당 0: 값 · 색 배열은 부르는 쪽이 들고 있음.
    /// </summary>
    public static class StackBar35
    {
        /// <summary>i 번째 조각 칸: 폭 = r 폭 × 값 ÷ max(max ≤ 0 이면 n 개 합), 뒤에 채운 조각이 있으면 오른쪽 2 를 비움. 값 0 이하는 폭 0.</summary>
        public static Rect Piece(Rect r, float[] values, int n, float max, int i)
        {
            n = Mathf.Min(n, values == null ? 0 : values.Length);
            if (max <= 0f) for (int k = 0; k < n; k++) max += Mathf.Max(0f, values[k]);
            if (max <= 0f || i < 0 || i >= n) return new Rect(r.x, r.y, 0f, r.height);
            float x = r.x;
            for (int k = 0; k < i; k++) x += r.width * Mathf.Max(0f, values[k]) / max;
            float w = Mathf.Max(0f, Mathf.Min(r.xMax - x, r.width * Mathf.Max(0f, values[i]) / max));
            bool last = true;
            for (int k = i + 1; k < n; k++) if (values[k] > 0f) { last = false; break; }
            return new Rect(x, r.y, Mathf.Max(0f, w - (last ? 0f : D35.StackGap)), r.height);
        }

        /// <summary>
        /// 막대를 그림: home 이 투명이 아니면 홈(r 전체, 바깥 r4) 먼저(F1 점유율은 홈 없음 · NASCAR · WEC GroundRaised).
        /// roundStart = 왼쪽 끝을 둥글게(점유율 true · 몫 막대 false). 오른쪽은 채운 끝이 둥긂.
        /// </summary>
        public static void Paint(Rect r, float[] values, int n, float max, Color[] colors, Color home, bool roundStart = true)
        {
            if (!Draw.Repaint) return;
            if (home.a > 0f) Draw.Round(r, home, D35.RadiusTag);
            int cnt = Mathf.Min(n, values == null ? 0 : values.Length);
            float end = r.x;
            for (int i = 0; i < cnt; i++) { var p = Piece(r, values, cnt, max, i); if (p.width > 0f) end = Mathf.Max(end, p.xMax); }
            float x0 = roundStart ? r.x : r.x - D35.RadiusTag;
            var whole = new Rect(x0, r.y, end - x0, r.height);
            for (int i = 0; i < cnt; i++)
            {
                var p = Piece(r, values, cnt, max, i);
                if (p.width > 0f && colors != null && i < colors.Length) Draw.RoundPart(p, whole, colors[i], D35.RadiusTag);
            }
        }

        /// <summary>범례 색 칸 12 × 8 · r2(왼쪽 x · 세로 가운데 cy).</summary>
        public static void Swatch(float x, float cy, Color c)
        {
            if (Draw.Repaint) Draw.Round(new Rect(x, Mathf.Round(cy - D35.SwatchH * 0.5f), D35.SwatchW, D35.SwatchH), c, D35.SwatchR);
        }
    }

    /// <summary>
    /// 3.5 가로 막대 차트(NASCAR 스테이지 결과 · WEC 운전 시간): 막대 20 · 끝(오른쪽) r4 · 시작 각짐 · 강조 하나 · 나머지 ChartGray3.
    /// 세로 눈금선(첫 줄 ChartBase · 나머지 ChartGrid) · x 축 글 영역 바닥 + 16 · 기준 점선 RefLine(.48 · 4 / 4) · 값 글 막대 끝 + 8(테 4).
    /// </summary>
    public static class HBar35
    {
        /// <summary>값 v(0 ~ max) 막대 칸: 영역 왼쪽에서 시작, 세로 가운데 cy, 높이 20. 범위 밖은 자름.</summary>
        public static Rect Bar(Rect plot, float cy, float v, float max)
        {
            float w = max <= 0f ? 0f : plot.width * Mathf.Clamp01(v / max);
            return new Rect(plot.x, Mathf.Round(cy - D35.HBarH * 0.5f), w, D35.HBarH);
        }

        /// <summary>막대 그리기(끝 r4 · 시작 각짐).</summary>
        public static void PaintBar(Rect bar, Color c) => Draw.RoundRight(bar, c, D35.BarEndR);

        /// <summary>세로 눈금선 labels.Length 줄(같은 간격, 영역 위 ~ 바닥) · x 축 글 12 InkDim 가운데(영역 바닥 + 16).</summary>
        public static void Grid(Rect plot, string[] labels)
        {
            if (!Draw.Repaint || labels == null) return;
            int n = labels.Length;
            float w = D35.ChartAxisW * 2f;
            for (int i = 0; i < n; i++)
            {
                float x = n <= 1 ? plot.x : plot.x + plot.width * i / (n - 1);
                Draw.Fill(new Rect(x - 0.5f, plot.y, 1f, plot.height), i == 0 ? Pal.ChartBase : Pal.ChartGrid);
                Draw.Text(new Rect(x - w * 0.5f, plot.yMax + D35.HBarAxisY - D35.LineCaption * 0.5f, w, D35.LineCaption), labels[i], D35.FontCaption, Pal.InkDim, false, TextAnchor.MiddleCenter);
            }
        }

        /// <summary>줄 이름 14(오른쪽 끝 = 영역 왼쪽 − 12, 세로 가운데 cy). 지금 운전 InkTxt · 나머지 InkSub.</summary>
        public static void Name(Rect plot, float cy, string name, Color ink)
        {
            if (!Draw.Repaint) return;
            float right = plot.x - D35.HBarNameGap;
            Draw.Text(new Rect(right - D35.FormLabelW, Mathf.Round(cy - D35.LineBody * 0.5f), D35.FormLabelW, D35.LineBody), name, D35.FontBody, ink, false, TextAnchor.MiddleRight);
        }

        /// <summary>기준 점선(최소 45분): 1px RefLine · 대시 4 · 틈 4 · 영역 위 ~ 바닥, 이름 12 InkSub 가운데(영역 위 − 12).</summary>
        public static void RefLine(Rect plot, float x, string label)
        {
            if (!Draw.Repaint) return;
            Draw.Dashed(new Vector2(x, plot.y), new Vector2(x, plot.yMax), 1f, Pal.RefLine, D35.HBarDash, D35.HBarDashGap);
            float w = D35.ChartAxisW * 2f;
            Draw.Text(new Rect(x - w * 0.5f, plot.y - D35.HBarRefLabelY - D35.LineCaption * 0.5f, w, D35.LineCaption), label, D35.FontCaption, Pal.InkSub, false, TextAnchor.MiddleCenter);
        }

        /// <summary>값 글 14 숫자(왼쪽 = 막대 끝 + 8, 세로 가운데 = 막대): 뒤에 halo(놓인 면 — 보통 GroundCard) 테 4 로 눈금선 · 점선을 가림. 폭을 돌려줌.</summary>
        public static float Value(Rect bar, string text, Color ink, Color halo)
        {
            float w = Mathf.Ceil(Draw.NumWidth(text ?? "", D35.FontBody));
            if (!Draw.Repaint || w <= 0f) return w;
            var r = new Rect(bar.xMax + D35.HBarValueGap, Mathf.Round(bar.center.y - D35.LineBody * 0.5f), w, D35.LineBody);
            Draw.Round(new Rect(r.x - D35.HaloPad, r.y - D35.HaloPad, r.width + D35.HaloPad * 2f, r.height + D35.HaloPad * 2f), halo, D35.RadiusTag);
            Draw.Num(r, text, D35.FontBody, ink, TextAnchor.MiddleLeft);
            return w;
        }
    }
}
