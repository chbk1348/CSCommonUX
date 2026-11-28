using System;
using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 모달(일정 추가 · 수정 · 지우기 · 이적 확인): 덮개 = 창 전체(r20) Pal.Scrim35 · 덮개 아래 툴바 막힘 · 판은 창 가운데.
    /// 판 폭 560(넓게 720) · r20 · GroundCard + Hairline(Surface.Kind.Modal). 머리 56: 왼쪽 24 · 제목 20/28 InkTxt → 12 → 부제 12 InkDim … 닫기 40(고스트, 오른쪽 12).
    /// 몸 좌우 24 · 바닥 64(버튼 40 · 사이 8 · 오른쪽 맞춤 — Foot). 열고 닫기 0.3초 · 8 아래에서 올라옴(곡선 Ease). 접근성: dialog · 이름 = 제목.
    /// </summary>
    public static class Modal35
    {
        /// <summary>보일 정도 0 ~ 1(열면 0.3초에 1, 닫으면 0.3초에 0) — 0 이면 그리지 말 것. key = 모달마다 고정 값.</summary>
        public static float Show(int key, bool open) => CSCommonAnim.Toward(key, open ? 1f : 0f, 1f / D35.DurEnter, 0f);

        static float opacity, shift;

        /// <summary>그리기 시작: 투명도 · 8 아래에서 올라옴(Ease). End 와 짝.</summary>
        public static void Begin(float t)
        {
            float k = D35.Ease(t);
            opacity = Draw.Opacity;
            shift = Mathf.Round((1f - k) * D35.EnterRise);
            Draw.Opacity *= k;
            Draw.Shift(new Vector2(0f, shift));
        }

        public static void End()
        {
            Draw.Shift(new Vector2(0f, -shift));
            Draw.Opacity = opacity;
        }

        /// <summary>덮개(창 칸 window · r20). 덮개가 깔리면 그 위 모달이 맨 위(툴바 칸이 모달 단추를 막지 않게 — 3.0 Modal.Scrim 과 같음).</summary>
        public static void Scrim(Rect window, float t = 1f)
        {
            Draw.ClearBlocks();
            if (Draw.Repaint) Draw.Round(window, Draw.Alpha(Pal.Scrim35, D35.Ease(t)), D35.RadiusWindow);
        }

        /// <summary>창 window 가운데 판 자리(폭 w · 높이 h — 일정 모달은 ModalFixedH 400 고정).</summary>
        public static Rect Place(Rect window, float h, float w = D35.ModalW) =>
            new(Mathf.Round(window.center.x - w * 0.5f), Mathf.Round(window.center.y - h * 0.5f), w, h);

        /// <summary>판 높이 = 머리 56 + 몸 + 바닥 64.</summary>
        public static float Height(float body) => D35.ModalHeadH + body + D35.ModalFootH;

        /// <summary>판 · 머리를 그리고 몸 칸(좌우 24, 머리 아래 ~ 바닥 위)을 돌려줌. 닫기를 눌러 떼면 closed. closeTip = 닫기 이름('닫기').</summary>
        public static Rect Frame(Rect r, string title, string sub, string closeTip, out bool closed)
        {
            Surface.Frame(r, Surface.Kind.Modal);
            var close = new Rect(r.xMax - D35.ModalCloseRight - D35.BtnH, Mathf.Round(r.y + (D35.ModalHeadH - D35.BtnH) * 0.5f), D35.BtnH, D35.BtnH);
            if (Draw.Repaint)
            {
                float x = r.x + D35.ModalPad, cy = r.y + D35.ModalHeadH * 0.5f;
                float tw = Mathf.Min(Mathf.Ceil(Draw.TextWidth(title ?? "", D35.FontHeading)), Mathf.Max(0f, close.x - D35.WindowHeadGap - x));
                Draw.Text(new Rect(x, Mathf.Round(cy - D35.LineHeading * 0.5f), tw + 2f, D35.LineHeading), title, D35.FontHeading, Pal.InkTxt);
                float sx = x + tw + D35.WindowHeadGap;
                if (!string.IsNullOrEmpty(sub) && close.x - D35.WindowHeadGap > sx)
                    Draw.Num(new Rect(sx, Mathf.Round(cy - D35.LineCaption * 0.5f), close.x - D35.WindowHeadGap - sx, D35.LineCaption), sub, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleLeft);
            }
            closed = IconButton35.Paint(close, Icons.Close, closeTip, Tone35.Ghost, D35.IconM, true, Surface.Kind.Modal);
            return new Rect(r.x + D35.ModalPad, r.y + D35.ModalHeadH, r.width - D35.ModalPad * 2f, Mathf.Max(0f, r.height - D35.ModalHeadH - D35.ModalFootH));
        }

        /// <summary>바닥 버튼 줄(높이 40, 바닥 64 가운데 · 좌우 24). 버튼은 Button35.AtRight(ref right = 줄 xMax, 줄 가운데 y …)로 오른쪽부터, 왼쪽 버튼('라운드 지우기')은 줄 x 에.</summary>
        public static Rect Foot(Rect r) =>
            new(r.x + D35.ModalPad, r.yMax - D35.ModalFootH + (D35.ModalFootH - D35.BtnH) * 0.5f, r.width - D35.ModalPad * 2f, D35.BtnH);
    }

    /// <summary>3.5 확인 줄(일정 모달 · 이적 안내): 44 · r8 · 놓인 면 한 단 · 좌우 16 · 점 8 → 8 → 글 14 InkTxt(한 줄 — 넘치면 문구를 줄임). 점 = SemGood · SemWarn.</summary>
    public static class Notice35
    {
        public static void Paint(Rect r, Color dot, string text, Surface.Kind on = Surface.Kind.Modal)
        {
            if (!Draw.Repaint) return;
            Draw.Round(r, Surface.Raise(on), D35.RadiusCtrl);
            float x = r.x + D35.NoticePadX;
            StatusDot.Paint(new Vector2(x + D35.Dot * 0.5f, r.center.y), dot);
            x += D35.Dot + D35.IconTextGap;
            Draw.Text(new Rect(x, r.y, Mathf.Max(0f, r.xMax - D35.NoticePadX - x), r.height), text, D35.FontBody, Pal.InkTxt);
        }
    }

    /// <summary>토스트 3.5 종류: 정보(InkSub Info) · 성공(SemGood Done) · 경고(SemWarn Warn) · 오류(SemBad Alert).</summary>
    public enum Toast35Kind { Info, Success, Warn, Error }

    /// <summary>
    /// 3.5 토스트(FIA 캔버스 R5Toast): 높이 52 · r16 · GroundCard + Hairline · 그림자 없음 · 폭 = 내용 · 왼쪽 8 · 오른쪽 16(버튼 있으면 10) · 사이 12.
    /// 상태 칸 36 · r8 · GroundRaised · 아이콘 18 = 뜻 색 · 제목 14 InkTxt + 설명 12 InkSub(숫자 자리 폭 같음) · 작은 주 버튼 32('되돌리기').
    /// 한 번에 하나(Show 가 바꿔 끼움) · 화면 아래 가운데 · 0.3초 8 아래에서 올라옴 · 끝 0.15초 동안 사라짐. Flush 를 OnGUI 끝에서 부름.
    /// </summary>
    public static class Toast35
    {
        static string sTitle, sDesc, sAction;
        static Toast35Kind sKind;
        static Action sClick;
        static Color sAccent, sOnAccent;
        static float sAt = -100f, sLife;

        /// <summary>지금 그린 토스트 자리(그 위 클릭은 지도로 보내지 않음).</summary>
        public static Rect Bounds { get; private set; }

        /// <summary>토스트를 띄움. action · click = 작은 주 버튼(강조 accent · onAccent). life ≤ 0 = 기본(버튼 있으면 3.0 ToastLifeAction, 없으면 ToastLife).</summary>
        public static void Show(string title, string desc, Toast35Kind kind = Toast35Kind.Info, string action = null, Action click = null, Color accent = default, Color onAccent = default, float life = 0f)
        {
            sTitle = title ?? "";
            sDesc = desc;
            sKind = kind;
            sAction = action;
            sClick = click;
            sAccent = accent;
            sOnAccent = onAccent;
            sAt = Time.realtimeSinceStartup;
            sLife = life > 0f ? life : !string.IsNullOrEmpty(action) ? CSCommonDesign.ToastLifeAction : CSCommonDesign.ToastLife;
        }

        public static void Hide() => sAt = -100f;

        public static float Width(string title, string desc, string action)
        {
            float tw = Mathf.Max(Mathf.Ceil(Draw.TextWidth(title ?? "", D35.FontBody)), string.IsNullOrEmpty(desc) ? 0f : Mathf.Ceil(Draw.NumWidth(desc, D35.FontCaption)));
            float w = D35.ToastPadL + D35.ToastIconBox + D35.ToastGap + tw;
            return string.IsNullOrEmpty(action) ? w + D35.ToastPadR : w + D35.ToastGap + Button35.Width(null, action, true) + D35.ToastPadRAction;
        }

        public static void Flush() => Flush(default, default);

        /// <summary>
        /// 비켜 가기(3.7): 화면 아래 자리가 avoidA · avoidB(아래 독 · 안내 칸 등, 빈 칸이면 무시)와 겹치면 겹친 칸 위 8 로 올려 그림.
        /// Bounds 는 올린 자리. 겹침 판정은 AvoidY. 그 자리는 앞 판(Draw.Block) — 먼저 그린 창 단추가 토스트 밑 클릭 · 호버를 받지 않음(3.8.1).
        /// </summary>
        public static void Flush(Rect avoidA, Rect avoidB)
        {
            Bounds = default;
            float age = Time.realtimeSinceStartup - sAt;
            if (string.IsNullOrEmpty(sTitle) || age >= sLife) return;
            float w = Width(sTitle, sDesc, sAction), x = Mathf.Round((Draw.Width - w) * 0.5f);
            float y = AvoidY(x, Mathf.Round(Draw.Height - CSCommonDesign.ToastSafeGap - D35.ToastH), w, D35.ToastH, avoidA, avoidB);
            var r = new Rect(x, y, w, D35.ToastH);
            Bounds = r;
            Draw.Block(r);
            Draw.BeginOverlay();
            bool hit = In(r, sTitle, sDesc, sKind, sAction, sAccent, sOnAccent, age, sLife);
            Draw.EndOverlay();
            if (hit)
            {
                var click = sClick;
                sAt = -100f;
                click?.Invoke();
            }
        }

        /// <summary>토스트 위 y: (x, y, w, h) 가 a · b 와 겹치면 겹친 칸 위 8 로 — 올린 자리에서 한 번 더 봐 둘 중 높은 것 위(3.0 Toast.Flush 와 같은 규칙).</summary>
        public static float AvoidY(float x, float y, float w, float h, Rect a, Rect b)
        {
            for (int pass = 0; pass < 2; pass++)
            {
                if (Overlaps(a, x, y, w, h)) y = a.y - D35.ToastAvoidGap - h;
                if (Overlaps(b, x, y, w, h)) y = b.y - D35.ToastAvoidGap - h;
            }
            return Mathf.Round(y);
        }

        static bool Overlaps(Rect a, float x, float y, float w, float h) =>
            a.width > 0f && a.height > 0f && a.xMax > x && a.x < x + w && a.yMax > y && a.y < y + h;

        /// <summary>r(높이 52)에 그림 — 버튼을 눌러 떼면 true. age · life 를 넘기면 들어오고 사라지는 움직임.</summary>
        public static bool In(Rect r, string title, string desc, Toast35Kind kind, string action, Color accent, Color onAccent, float age = float.MaxValue, float life = float.MaxValue)
        {
            float show = D35.Ease(age / D35.DurEnter);
            float fade = life < float.MaxValue ? Mathf.Min(show, Mathf.Clamp01((life - age) / D35.DurFeedback)) : show;
            if (fade <= 0f) return false;
            float op = Draw.Opacity;
            var sh = new Vector2(0f, Mathf.Round((1f - show) * D35.EnterRise));
            Draw.Opacity *= fade;
            Draw.Shift(sh);
            bool hasBtn = !string.IsNullOrEmpty(action);
            float bw = hasBtn ? Button35.Width(null, action, true) : 0f;
            if (Draw.Repaint)
            {
                Draw.Box(r, Pal.GroundCard, Pal.Hairline, D35.RadiusCard);
                var box = new Rect(r.x + D35.ToastPadL, Mathf.Round(r.center.y - D35.ToastIconBox * 0.5f), D35.ToastIconBox, D35.ToastIconBox);
                Draw.Round(box, Pal.GroundRaised, D35.RadiusCtrl);
                Color ink = kind switch { Toast35Kind.Success => Pal.SemGood, Toast35Kind.Warn => Pal.SemWarn, Toast35Kind.Error => Pal.SemBad, _ => Pal.InkSub };
                string icon = kind switch { Toast35Kind.Success => Icons.Done, Toast35Kind.Warn => Icons.Warn, Toast35Kind.Error => Icons.Alert, _ => Icons.Info };
                Draw.Icon(icon, new Rect(box.center.x - D35.IconM * 0.5f, box.center.y - D35.IconM * 0.5f, D35.IconM, D35.IconM), ink, D35.IconBox, D35.IconLine);
                float tx = box.xMax + D35.ToastGap, tw = r.xMax - (hasBtn ? D35.ToastPadRAction + bw + D35.ToastGap : D35.ToastPadR) - tx;
                if (string.IsNullOrEmpty(desc)) Draw.Text(new Rect(tx, r.y, tw, r.height), title, D35.FontBody, Pal.InkTxt);
                else
                {
                    float top = Mathf.Round(r.center.y - (D35.LineBody + D35.LineCaption) * 0.5f);
                    Draw.Text(new Rect(tx, top, tw, D35.LineBody), title, D35.FontBody, Pal.InkTxt);
                    Draw.Num(new Rect(tx, top + D35.LineBody, tw, D35.LineCaption), desc, D35.FontCaption, Pal.InkSub, TextAnchor.MiddleLeft);
                }
            }
            bool hit = hasBtn && Button35.Paint(new Rect(r.xMax - D35.ToastPadRAction - bw, Mathf.Round(r.center.y - D35.BtnSmallH * 0.5f), bw, D35.BtnSmallH),
                action, Tone35.Primary, accent, onAccent);
            Draw.Shift(-sh);
            Draw.Opacity = op;
            return hit;
        }
    }

    /// <summary>
    /// 3.5 툴팁(런처 · 3.5 부품): 한 줄 · 높이 32 · 좌우 12 · r8 · GroundRaised + Hairline · 글 12/16 InkTxt · 가리키는 칸 아래 8(화면 밖이면 위). 그림자 · 꼬리 없음.
    /// 등록은 CSCommonToolbar.Tip35, 그리기 · 0.4초 지연은 CSCommonToolbar.DrawTip(3.0 툴팁과 같은 자리).
    /// </summary>
    public static class Tooltip35
    {
        public static void Draw(Rect anchor, string text)
        {
            if (!CSCommonUX.Draw.Repaint || string.IsNullOrEmpty(text)) return;
            float w = Mathf.Ceil(CSCommonUX.Draw.TextWidth(text, D35.FontCaption)) + D35.TipPadX * 2f, h = D35.TipH;
            float x = Mathf.Clamp(Mathf.Round(anchor.center.x - w * 0.5f), D35.Step8, CSCommonUX.Draw.Width - w - D35.Step8);
            bool above = anchor.yMax + D35.TipGap + h > CSCommonUX.Draw.Height - D35.Step8;
            var r = new Rect(x, above ? anchor.y - D35.TipGap - h : anchor.yMax + D35.TipGap, w, h);
            CSCommonUX.Draw.Box(r, Pal.GroundRaised, Pal.Hairline, D35.RadiusCtrl);
            CSCommonUX.Draw.Text(r, text, D35.FontCaption, Pal.InkTxt, false, TextAnchor.MiddleCenter);
        }

        static string floatTip;
        static float floatSince, floatSeen;

        /// <summary>
        /// 창 밖 툴팁(3.7 — 게임 화면 위 런처처럼 툴바 DrawTip 이 그리지 않는 곳): anchor 위에 0.4초 머물면 그 자리에서 Draw.
        /// anchor 를 그린 뒤 OnGUI 맨 끝 쪽에서 부름. show = false(끌기 중 등)면 그리지 않음. 다른 글로 옮기거나 0.1초 넘게 안 보이면 다시 셈(DrawTip 과 같음).
        /// </summary>
        public static void Float(Rect anchor, string text, bool show = true)
        {
            if (!show || string.IsNullOrEmpty(text) || !CSCommonUX.Draw.Hover(anchor) || CSCommonUX.Draw.Holding) return;
            float now = Time.unscaledTime;
            if (text != floatTip || now - floatSeen > 0.1f) { floatTip = text; floatSince = now; }
            floatSeen = now;
            if (now - floatSince >= D35.TipDelay) Draw(anchor, text);
        }
    }

    /// <summary>
    /// 3.5 런처 원(관제 런처 'F1' · 서킷 툴킷 런처 아이콘): 원 40 · 그림자 없음 · 면 0.15초 · 누르면 0.97배.
    /// 꺼짐 = GroundCard + Hairline · InkSub, 호버(hover 켬 — 툴킷만) = GroundRaised + Hairline · InkTxt, 켜짐 = GroundRaised + 위 SelectedTopLine + Hairline · 강조.
    /// 점: 진행 중 8 SemBad(위 0 · 오른쪽 0, 멈춤) · 툴킷 켜짐 6 강조(위 1 · 오른쪽 1). 둘레 2 = ring(게임 화면 위 둘레 색은 캔버스 없음 — 모드가 넘김).
    /// </summary>
    public static class Launcher35
    {
        /// <summary>label('F1' 14 · 600) 또는 icon(18) 중 하나. dot = 점 색(없으면 null), smallDot = 켜짐 점 6. 눌러 떼면 true. tip = 툴팁 · 이름.</summary>
        public static bool Paint(Rect r, string label, string icon, bool on, bool hover, Color accent, string tip, Color? dot = null, bool smallDot = false, Color ring = default)
        {
            int key = Look.Key(r, "launch35", label ?? icon);
            float t = CSCommonAnim.Toward(key, on ? 1f : 0f, 1f / D35.DurFeedback);
            float hv = CSCommonAnim.Toward(key ^ 0x3c6ef372, hover && !on && Draw.Hover(r) ? 1f : 0f, 1f / D35.DurFeedback);
            float pr = Look.Press(key ^ 0x6a09e667, r);
            if (Draw.Repaint)
            {
                float rad = r.width * 0.5f;
                Color face = Color.Lerp(Pal.GroundCard, Pal.GroundRaised, Mathf.Max(t, hv));
                Color ink = Color.Lerp(Color.Lerp(Pal.InkSub, Pal.InkTxt, hv), accent, t);
                Draw.BeginScale(r.center, Look.PressScale(pr, D35.PressScale));
                Draw.Round(r, face, rad);
                Draw.RoundOutline(r, Pal.Hairline, rad, 1f);
                // 위 1px 밝은 선: 원 안쪽 1px 깊이의 현(弦) 길이만큼
                float chord = 2f * Mathf.Sqrt(Mathf.Max(0f, 2f * rad - 1f));
                if (t > 0.01f) Draw.Fill(new Rect(r.center.x - chord * 0.5f, r.y + 1f, chord, 1f), Draw.Alpha(Pal.SelectedTopLine, t));
                if (!string.IsNullOrEmpty(label)) Draw.Num(r, label, D35.FontBody, ink, TextAnchor.MiddleCenter);
                Draw.EndScale();
                if (!string.IsNullOrEmpty(icon)) Draw.Icon(icon, new Rect(r.center.x - D35.IconM * 0.5f, r.center.y - D35.IconM * 0.5f, D35.IconM, D35.IconM), ink, D35.IconBox, D35.IconLine);
                if (dot.HasValue)
                {
                    float d = smallDot ? D35.LauncherDotOn : D35.Dot, inset = smallDot ? D35.LauncherDotInset : 0f;
                    var p = new Vector2(r.xMax - inset - d * 0.5f, r.y + inset + d * 0.5f);
                    Draw.Dot(p, d * 0.5f + D35.DotRing, ring);
                    Draw.Dot(p, d * 0.5f, dot.Value);
                }
            }
            CSCommonToolbar.Tip35(r, tip);
            return Draw.Button(r);
        }
    }
}
