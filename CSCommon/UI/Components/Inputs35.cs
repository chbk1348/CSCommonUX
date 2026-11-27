using System;
using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 고르기 칸(FIA 캔버스 .seg — 중계 · 컨트롤 기록 · 일정 보기 · 모달 시리즈 · 주말 형식 · 팀 시즌 · 대체 기간).
    /// 보통: 판 40 · 안쪽 4 · r8 · Hairline 테 · 면 없음 · 칸 32 · 좌우 12 · 최소 48 · r4 · 글 14.
    /// 작은(small): 판 32 · 칸 24 · 좌우 8 · 글 12 숫자 · 판 면 GroundPage + Hairline(팀 시즌 · 대체 기간).
    /// 고른 칸 = GroundRaised + 위 1px SelectedTopLine · InkTxt(면 0.15초), 안 고른 칸 InkSub, 꺼진 칸 InkDim · 누름 없음. 판이 넓으면 칸을 같은 몫으로 늘림.
    /// </summary>
    public static class Segmented35
    {
        static float CellW(string label, bool small) =>
            Mathf.Max(D35.SegCellMinW, Mathf.Ceil(small ? Draw.NumWidth(label ?? "", D35.FontCaption) : Draw.TextWidth(label ?? "", D35.FontBody)) + (small ? D35.SegSmallCellPadX : D35.SegCellPadX) * 2f);

        /// <summary>내용 폭(판 안쪽 4 · 칸 사이 4 포함).</summary>
        public static float Width(string[] labels, bool small = false)
        {
            if (labels == null || labels.Length == 0) return 0f;
            float w = D35.SegPad * 2f + D35.SegCellGap * (labels.Length - 1);
            for (int i = 0; i < labels.Length; i++) w += CellW(labels[i], small);
            return w;
        }

        /// <summary>판 r 안 i 번째 칸(넓으면 남는 폭을 칸마다 같게 더함).</summary>
        public static Rect Cell(Rect r, string[] labels, int i, bool small = false)
        {
            int n = labels.Length;
            float extra = Mathf.Max(0f, r.width - Width(labels, small)) / n, x = r.x + D35.SegPad;
            for (int k = 0; k < i; k++) x += CellW(labels[k], small) + extra + D35.SegCellGap;
            return new Rect(x, r.y + D35.SegPad, CellW(labels[i], small) + extra, r.height - D35.SegPad * 2f);
        }

        /// <summary>고르기 칸을 r(높이 40 · 작은 32)에 그림. 안 고른 칸을 눌러 떼면 picked = 그 번호(없으면 −1). off[i] = 그 칸 꺼짐(잠김).</summary>
        public static void Paint(Rect r, string[] labels, int cur, out int picked, bool small = false, bool[] off = null, string[] tips = null)
        {
            picked = -1;
            if (labels == null || labels.Length == 0) return;
            if (Draw.Repaint)
            {
                if (small) Draw.Round(r, Pal.GroundPage, D35.RadiusCtrl);
                Draw.RoundOutline(r, Pal.Hairline, D35.RadiusCtrl, 1f);
            }
            for (int i = 0; i < labels.Length; i++)
            {
                var c = Cell(r, labels, i, small);
                bool on = i == cur, dead = off != null && i < off.Length && off[i];
                float t = CSCommonAnim.Toward(Look.Key(c, "seg35", labels[i]), on ? 1f : 0f, 1f / D35.DurFeedback);
                if (Draw.Repaint)
                {
                    Selected(c, D35.RadiusTag, t);
                    Color ink = dead ? Pal.InkDim : Color.Lerp(Pal.InkSub, Pal.InkTxt, t);
                    if (small) Draw.Num(c, labels[i], D35.FontCaption, ink, TextAnchor.MiddleCenter);
                    else Draw.Text(c, labels[i], D35.FontBody, ink, false, TextAnchor.MiddleCenter);
                }
                if (tips != null && i < tips.Length) CSCommonToolbar.Tip35(c, tips[i]);
                if (!dead && Draw.Button(c) && !on) picked = i;
            }
        }

        /// <summary>고른 칸 면(3.5 공통): GroundRaised + 위 1px SelectedTopLine(모서리 안쪽만). t = 나타남 0 ~ 1.</summary>
        public static void Selected(Rect c, float radius, float t = 1f)
        {
            if (!Draw.Repaint || t <= 0.01f) return;
            Draw.Round(c, Draw.Alpha(Pal.GroundRaised, t), radius);
            Draw.Fill(new Rect(c.x + radius, c.y, c.width - radius * 2f, 1f), Draw.Alpha(Pal.SelectedTopLine, t));
        }

        /// <summary>
        /// 등급 칸(FIA 그레이드): 높이 32 · r8 · 사이 4 · 폭 = weights 몫 · 글 14 가운데 · 면 모두 GroundRaised. 지금 칸 cur = InkTxt + 안쪽 1px FocusLine(.32), 나머지 InkDim.
        /// 눌러 떼면 그 번호(지금 칸 포함 — 등급을 바꾸는 쪽이 판단), 없으면 −1.
        /// </summary>
        public static int Tiles(Rect r, string[] labels, float[] weights, int cur, string[] tips = null)
        {
            int hit = -1;
            if (labels == null) return hit;
            float sum = 0f;
            for (int i = 0; i < labels.Length; i++) sum += Weight(weights, i);
            float room = r.width - D35.TileGap * (labels.Length - 1), x = r.x;
            for (int i = 0; i < labels.Length; i++)
            {
                var c = new Rect(x, r.y, sum > 0f ? room * Weight(weights, i) / sum : 0f, r.height);
                x += c.width + D35.TileGap;
                if (Draw.Repaint)
                {
                    Draw.Round(c, Pal.GroundRaised, D35.RadiusCtrl);
                    if (i == cur) Draw.RoundOutline(c, Pal.FocusLine, D35.RadiusCtrl, 1f);
                    Draw.Text(c, labels[i], D35.FontBody, i == cur ? Pal.InkTxt : Pal.InkDim, false, TextAnchor.MiddleCenter);
                }
                if (tips != null && i < tips.Length) CSCommonToolbar.Tip35(c, tips[i]);
                if (Draw.Button(c)) hit = i;
            }
            return hit;
        }

        internal static float Weight(float[] w, int i) => w == null || i >= w.Length ? 1f : Mathf.Max(0f, w[i]);
    }

    /// <summary>
    /// 3.5 스위치(FIA 캔버스 .sw · 정본 규격 켜짐 = 강조 면): 40 × 24 · r12 · 손잡이 16(r8) · 위 4 · 꺼짐 왼쪽 4 · 켜짐 왼쪽 20 · 움직임 0.15초(Ease).
    /// 꺼짐: GroundRaised + Hairline · 손잡이 InkDim. 켜짐: 강조 면 · 손잡이 강조 위 글.
    /// 꺼짐(못 바꿈, 캔버스 없음): 켜짐이면 InkDim 면 · GroundPage 손잡이, 꺼짐이면 손잡이 ChartGray4 · 누름 없음. 누르면 true(값은 부르는 쪽이 뒤집음).
    /// </summary>
    public static class Switch35
    {
        public static bool Paint(Rect r, bool on, Color accent, Color onAccent, bool enabled = true, string tip = null)
        {
            int key = Look.Key(r, "sw35");
            float t = D35.Ease(CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / D35.DurFeedback));
            if (Draw.Repaint)
            {
                Color offFace = Pal.GroundRaised, onFace = enabled ? accent : Pal.InkDim;
                Color offKnob = enabled ? Pal.InkDim : Pal.ChartGray4, onKnob = enabled ? onAccent : Pal.GroundPage;
                float rad = r.height * 0.5f;
                Draw.Round(r, Color.Lerp(offFace, onFace, t), rad);
                if (t < 0.99f) Draw.RoundOutline(r, Draw.Alpha(Pal.Hairline, 1f - t), rad, 1f);
                float k = D35.SwitchKnob * 0.5f;
                float cx = Mathf.Lerp(r.x + D35.SwitchInset + k, r.xMax - D35.SwitchInset - k, t);
                Draw.Dot(new Vector2(cx, r.center.y), k, Color.Lerp(offKnob, onKnob, t));
            }
            CSCommonToolbar.Tip35(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>설정 줄 오른쪽 스위치 칸: 줄 row 오른쪽 끝 · 세로 가운데.</summary>
        public static Rect At(Rect row) => new(row.xMax - D35.SwitchW, Mathf.Round(row.center.y - D35.SwitchH * 0.5f), D35.SwitchW, D35.SwitchH);
    }

    /// <summary>
    /// 3.5 체크 상자(일정 시리즈): 18 · r4. 켜짐 = 면 InkTxt + Done 16 GroundDeep. 꺼짐(캔버스 없음) = 놓인 면 한 단 + Hairline.
    /// 이름은 자가 검사 Check35 와 겹치지 않게 CheckBox35. 누르면 true.
    /// </summary>
    public static class CheckBox35
    {
        public static bool Paint(Rect box, bool on, Surface.Kind kind = Surface.Kind.Card, bool enabled = true, string tip = null)
        {
            if (Draw.Repaint)
            {
                if (on)
                {
                    Draw.Round(box, enabled ? Pal.InkTxt : Pal.InkDim, D35.RadiusTag);
                    Draw.Icon(Icons.Done, new Rect(box.center.x - D35.IconS * 0.5f, box.center.y - D35.IconS * 0.5f, D35.IconS, D35.IconS), Pal.GroundDeep, D35.IconBox, D35.IconLine);
                }
                else Draw.Box(box, Surface.Raise(kind), Pal.Hairline, D35.RadiusTag);
            }
            CSCommonToolbar.Tip35(box, tip);
            return enabled && Draw.Button(box);
        }

        /// <summary>줄 왼쪽 x · 세로 가운데 cy 의 상자 칸(18).</summary>
        public static Rect At(float x, float cy) => new(x, Mathf.Round(cy - D35.CheckBox * 0.5f), D35.CheckBox, D35.CheckBox);
    }

    /// <summary>
    /// 3.5 폼 칸(일정 모달 · 커리어 리저브): 폼 줄 56 · 라벨 96 14 InkSub → 12 → 칸.
    /// 값 칸(Value) 40 · r8 · 놓인 면 한 단 · 좌우 12 · 글 14 숫자 InkTxt → 끝 아이콘 16 InkDim · 초점(그 값을 고르는 중) 안쪽 1px FocusLine. 누르면 true(무엇을 열지는 부르는 쪽).
    /// 입력 칸(Input) 32 · r8 · GroundPage + Hairline · 좌우 12 · 글 14 · 자리 글 InkDim · 초점 FocusLine.
    /// </summary>
    public static class Field35
    {
        /// <summary>폼 줄 r(높이 56)에 라벨을 그리고 칸 자리(높이 h, 세로 가운데)를 돌려줌.</summary>
        public static Rect Row(Rect r, string label, float h = D35.FieldH)
        {
            if (Draw.Repaint) Draw.Text(new Rect(r.x, r.y, D35.FormLabelW, r.height), label, D35.FontBody, Pal.InkSub);
            float x = r.x + D35.FormLabelW + D35.FormLabelGap;
            return new Rect(x, Mathf.Round(r.center.y - h * 0.5f), Mathf.Max(0f, r.xMax - x), h);
        }

        public static bool Value(Rect r, string text, string icon = null, bool focus = false, bool enabled = true, Surface.Kind on = Surface.Kind.Modal, string tip = null)
        {
            if (Draw.Repaint)
            {
                Draw.Round(r, Surface.Raise(on), D35.RadiusCtrl);
                if (focus) Draw.RoundOutline(r, Pal.FocusLine, D35.RadiusCtrl, 1f);
                float right = r.xMax - D35.FieldPadX;
                if (!string.IsNullOrEmpty(icon))
                {
                    right -= D35.IconS;
                    Draw.Icon(icon, new Rect(right, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), Pal.InkDim, D35.IconBox, D35.IconLine);
                    right -= D35.IconTextGap;
                }
                Draw.Num(new Rect(r.x + D35.FieldPadX, r.y, Mathf.Max(0f, right - r.x - D35.FieldPadX), r.height), text, D35.FontBody, enabled ? Pal.InkTxt : Pal.InkDim, TextAnchor.MiddleLeft);
            }
            CSCommonToolbar.Tip35(r, tip);
            return enabled && Draw.Button(r);
        }

        static readonly System.Collections.Generic.Dictionary<int, string> names = new();

        /// <summary>초점 이름(칸 열쇠마다 한 번 만들어 둠 — 매 틀 글 결합 없음).</summary>
        internal static string NameOf(int key)
        {
            if (!names.TryGetValue(key, out var n))
            {
                if (names.Count > 256) names.Clear();
                n = "csf35_" + key;
                names[key] = n;
            }
            return n;
        }

        /// <summary>글 입력 칸(name = 초점 이름, 없으면 칸 자리로). 바뀐 글을 돌려줌.</summary>
        public static string Input(Rect r, string value, int maxLength, string placeholder = null, string name = null, bool enabled = true)
        {
            name ??= NameOf(Look.Key(r, "field35"));
            value ??= "";
            bool focus = GUI.GetNameOfFocusedControl() == name;
            float t = CSCommonAnim.Toward(Look.Key(r, "field35f"), enabled && focus ? 1f : 0f, 1f / D35.DurFeedback);
            if (Draw.Repaint)
            {
                Draw.Box(r, Pal.GroundPage, Pal.Hairline, D35.RadiusCtrl);
                if (t > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Pal.FocusLine, t), D35.RadiusCtrl, 1f);
            }
            var tr = new Rect(r.x + D35.FieldPadX, r.y, Mathf.Max(0f, r.width - D35.FieldPadX * 2f), r.height);
            if (!enabled)
            {
                if (Draw.Repaint) Draw.Text(tr, value, D35.FontBody, Pal.InkDim);
                return value;
            }
            if (value.Length == 0 && !(focus && Draw.Composing) && !string.IsNullOrEmpty(placeholder) && Draw.Repaint)
                Draw.Text(tr, placeholder, D35.FontBody, Pal.InkDim);
            // Draw.Field 는 안쪽 10 을 띄워 씀 — 글 시작을 좌우 12 에 맞춤
            const float inset = 10f;
            GUI.SetNextControlName(name);
            string v = Draw.Field(new Rect(tr.x - inset, r.y, tr.width + inset * 2f, r.height), value, D35.FontBody, maxLength);
            // 칸 밖을 누르면 초점을 풂(3.0 Textfield 와 같은 까닭 — 단추가 MouseDown 을 먼저 쓰면 IMGUI 가 못 풂)
            var ev = Event.current;
            if (focus && ev.rawType == EventType.MouseDown && ev.button == 0 && !r.Contains(Draw.Mouse) && GUI.GetNameOfFocusedControl() == name)
            {
                GUI.FocusControl(null);
                GUIUtility.keyboardControl = 0;
            }
            return v;
        }
    }

    /// <summary>
    /// 3.5 고르기 칸(커리어 이적): 264 × 56 · r8 · GroundRaised + Hairline · 왼쪽 16 · 오른쪽 12 · 사이 12: [팀 막대 4 × 16] · 이름 14 InkTxt + 캡션 12 InkDim · Chevron 16 InkDim.
    /// 펼친 목록은 캔버스 규격이 없어 3.0 Select 목록을 씀(Select.Flush 가 그림).
    /// </summary>
    public static class Select35
    {
        public static void Paint(Rect r, Color? team, string name, string caption, string[] items, Color?[] dots, int cur, Action<int> pick, bool enabled = true, string tip = null)
        {
            // shortcut: 목록 열기 · 고르기 · 닫기는 3.0 Select 에 맡기고 그 칸은 투명으로 그림(3.5 목록 규격이 캔버스에 생기면 Menu35 로 바꿈)
            float op = Draw.Opacity;
            Draw.Opacity = 0f;
            Select.In(r, name ?? "", null, items, dots, cur, pick, enabled);
            Draw.Opacity = op;
            if (Draw.Repaint)
            {
                Draw.Box(r, Pal.GroundRaised, Pal.Hairline, D35.RadiusCtrl);
                float x = r.x + D35.SelectPadL;
                if (team.HasValue) { RowItem.TeamBar(x, r.center.y, team.Value); x += D35.TeamBarW + D35.SelectGap; }
                float cx = r.xMax - D35.SelectPadR - D35.IconS, w = Mathf.Max(0f, cx - D35.SelectGap - x);
                Color ink = enabled ? Pal.InkTxt : Pal.InkDim;
                if (string.IsNullOrEmpty(caption)) Draw.Text(new Rect(x, r.y, w, r.height), name, D35.FontBody, ink);
                else
                {
                    float top = Mathf.Round(r.center.y - (D35.LineBody + D35.LineCaption) * 0.5f);
                    Draw.Text(new Rect(x, top, w, D35.LineBody), name, D35.FontBody, ink);
                    Draw.Text(new Rect(x, top + D35.LineBody, w, D35.LineCaption), caption, D35.FontCaption, Pal.InkDim);
                }
                Draw.Icon(Icons.Chevron, new Rect(cx, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), Pal.InkDim, D35.IconBox, D35.IconLine);
            }
            CSCommonToolbar.Tip35(r, tip);
        }
    }

    /// <summary>
    /// 3.5 슬라이더(설정 소리): 폭 240 · 높이 16 · 홈 4(r2, 위 6) GroundRaised · 채움 InkSub · 손잡이 16 InkTxt. 꺼짐 = 채움 · 손잡이 InkDim.
    /// 끌기 · 키 입력은 3.0 Slider 와 같음(released = 손을 뗌). 값 글(48 오른쪽)은 부르는 쪽.
    /// </summary>
    public static class Slider35
    {
        public static float Paint(Rect r, float value, float min, float max, float step, out bool released, bool enabled = true)
        {
            // shortcut: 끌기 · 키 · 놓음 판정은 3.0 Slider 를 투명으로 불러 씀(Slider.Sliding 공유) — 3.5 모양만 아래에서 그림
            float op = Draw.Opacity;
            Draw.Opacity = 0f;
            value = Slider.In(r, value, min, max, step, Pal.InkSub, out released, null, enabled);
            Draw.Opacity = op;
            if (Draw.Repaint)
            {
                float t = max > min ? Mathf.Clamp01((value - min) / (max - min)) : 0f;
                var track = new Rect(r.x, Mathf.Round(r.center.y - D35.SliderTrackH * 0.5f), r.width, D35.SliderTrackH);
                float rad = D35.SliderTrackH * 0.5f, k = D35.SliderKnob * 0.5f;
                Draw.Round(track, Pal.GroundRaised, rad);
                if (t > 0f) Draw.Round(new Rect(track.x, track.y, Mathf.Max(D35.SliderTrackH, track.width * t), track.height), enabled ? Pal.InkSub : Pal.InkDim, rad);
                Draw.Dot(new Vector2(Mathf.Clamp(track.x + track.width * t, r.x + k, r.xMax - k), track.center.y), k, enabled ? Pal.InkTxt : Pal.InkDim);
            }
            return value;
        }
    }

    /// <summary>
    /// 3.5 스테퍼(설정 레이스 표시): 값 글 14 숫자 InkTxt(48 오른쪽 맞춤) → 8 → 보조 버튼 32 × 2(사이 8 · Minus · Plus 16 InkSub). 끝값이면 그 버튼 꺼짐.
    /// 오른쪽 끝 right · 세로 가운데 cy 에 그리고 −1 · 0 · +1 을 돌려줌.
    /// </summary>
    public static class Stepper35
    {
        public static float Width => D35.StepperValueW + D35.StepperGap + D35.StepperBtn * 2f + D35.StepperGap;

        public static int Paint(float right, float cy, string value, bool canMinus = true, bool canPlus = true, string minusTip = null, string plusTip = null, Surface.Kind on = Surface.Kind.Card)
        {
            float y = Mathf.Round(cy - D35.StepperBtn * 0.5f), b = D35.StepperBtn;
            int d = 0;
            if (IconButton35.Paint(new Rect(right - b, y, b, b), Icons.Plus, plusTip, Tone35.Secondary, D35.IconS, canPlus, on)) d = 1;
            if (IconButton35.Paint(new Rect(right - b * 2f - D35.StepperGap, y, b, b), Icons.Minus, minusTip, Tone35.Secondary, D35.IconS, canMinus, on)) d = -1;
            if (Draw.Repaint)
            {
                float vr = right - b * 2f - D35.StepperGap * 2f;
                Draw.Num(new Rect(vr - D35.StepperValueW, Mathf.Round(cy - D35.LineBody * 0.5f), D35.StepperValueW, D35.LineBody), value, D35.FontBody, Pal.InkTxt, TextAnchor.MiddleRight);
            }
            return d;
        }
    }
}
