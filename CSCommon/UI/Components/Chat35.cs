using UnityEngine;
using D35 = CSCommonUX.CSCommonDesign35;

namespace CSCommonUX
{
    /// <summary>
    /// 3.5 채팅 조각(팬 › 채팅 R5FanChat 가운데 칸): 날짜 줄 28 · 봇 공지 · 메시지 · 반응 태그 · 스레드 생김 36 · 관리봇 · 지운 글 28 · 입력 중 20.
    /// 들여쓰기 44(= 아바타 32 + 12). 메시지 사이 4 · 메시지 위아래 8 · 칸 위아래 12 는 부르는 쪽 간격. 문단(본문)은 줄바꿈 허용.
    /// </summary>
    public static class Chat35
    {
        /// <summary>날짜 줄(28): 선 1px Hairline · 12 · 캡션 12 InkDim · 12 · 선.</summary>
        public static void DateLine(Rect r, string text)
        {
            if (!Draw.Repaint) return;
            float tw = Mathf.Ceil(Draw.NumWidth(text ?? "", D35.FontCaption)), cx = r.center.x, y = Mathf.Round(r.center.y);
            float l = cx - tw * 0.5f - D35.Step12, rr = cx + tw * 0.5f + D35.Step12;
            Draw.Fill(new Rect(r.x, y, Mathf.Max(0f, l - r.x), 1f), Pal.Hairline);
            Draw.Fill(new Rect(rr, y, Mathf.Max(0f, r.xMax - rr), 1f), Pal.Hairline);
            Draw.Num(r, text, D35.FontCaption, Pal.InkDim, TextAnchor.MiddleCenter);
        }

        /// <summary>본문 높이(폭 w, 글 14 · 줄바꿈).</summary>
        public static float BodyH(string body, float w) => Mathf.Max(D35.LineBody, Mathf.Ceil(Draw.TextHeight(body ?? "", D35.FontBody, w)));

        /// <summary>메시지 높이(반응 줄 있으면 + 4 + 24): 위아래 8 + 캡션 16 + 4 + 본문.</summary>
        public static float MessageH(string body, float width, bool reactions) =>
            D35.Step8 * 2f + D35.LineCaption + D35.KpiGap + BodyH(body, width - D35.ChatIndent) + (reactions ? D35.KpiGap + D35.ReactionH : 0f);

        /// <summary>
        /// 보통 메시지: 아바타 32(원 · GroundRaised · 글 14 InkSub) · 12 · 캡션 줄(이름 InkSub · [역할 12 InkDim] · 시각 12 InkDim, 사이 8) → 4 → 본문 14 InkTxt.
        /// 반응 줄이 들어갈 왼쪽 x · y 를 돌려줌(본문 아래 4) — Reaction 을 그 자리부터 사이 4 로.
        /// </summary>
        public static Vector2 Message(Rect r, string initials, string name, string role, string time, string body)
        {
            float top = r.y + D35.Step8, x = r.x + D35.ChatIndent, w = Mathf.Max(0f, r.xMax - x);
            float bh = BodyH(body, w);
            if (Draw.Repaint)
            {
                Avatar35.Paint(new Vector2(r.x + D35.AvatarL * 0.5f, top + D35.AvatarL * 0.5f), D35.AvatarL, initials, Pal.GroundRaised);
                float cx = x;
                cx = Seg(cx, top, name, D35.FontBody, Pal.InkSub, false);
                if (!string.IsNullOrEmpty(role)) cx = Seg(cx, top, role, D35.FontCaption, Pal.InkDim, false);
                Seg(cx, top, time, D35.FontCaption, Pal.InkDim, true);
                Draw.Text(new Rect(x, top + D35.LineCaption + D35.KpiGap, w, bh), body, D35.FontBody, Pal.InkTxt, false, TextAnchor.UpperLeft, true);
            }
            return new Vector2(x, top + D35.LineCaption + D35.KpiGap + bh + D35.KpiGap);
        }

        static float Seg(float x, float top, string text, float size, Color ink, bool num)
        {
            if (string.IsNullOrEmpty(text)) return x;
            float w = Mathf.Ceil(num ? Draw.NumWidth(text, size) : Draw.TextWidth(text, size));
            var r = new Rect(x, top, w + 2f, D35.LineCaption);
            if (num) Draw.Num(r, text, size, ink, TextAnchor.MiddleLeft);
            else Draw.Text(r, text, size, ink);
            return x + w + D35.IconTextGap;
        }

        /// <summary>
        /// 반응 태그(왼쪽 x · 위 y): 24 · 좌우 8 · r4 · 아이콘 16 → 4 → 수 12. 내 반응 = GroundRaised + 위 SelectedTopLine · InkTxt, 남의 반응 = Hairline 테 · InkDim.
        /// 폭을 돌려주고 눌러 떼면 clicked(다음 태그는 + 4).
        /// </summary>
        public static float Reaction(float x, float y, string icon, string count, bool mine, out bool clicked)
        {
            float w = D35.ReactionPadX * 2f + D35.IconS + D35.LinkGap + Mathf.Ceil(Draw.NumWidth(count ?? "", D35.FontCaption));
            var r = new Rect(x, y, w, D35.ReactionH);
            if (Draw.Repaint)
            {
                if (mine) Segmented35.Selected(r, D35.RadiusTag);
                else Draw.RoundOutline(r, Pal.Hairline, D35.RadiusTag, 1f);
                Color ink = mine ? Pal.InkTxt : Pal.InkDim;
                float ix = r.x + D35.ReactionPadX;
                Draw.Icon(icon, new Rect(ix, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), ink, D35.IconBox, D35.IconLine);
                Draw.Num(new Rect(ix + D35.IconS + D35.LinkGap, r.y, w, r.height), count, D35.FontCaption, ink, TextAnchor.MiddleLeft);
            }
            clicked = Draw.Button(r);
            return w;
        }

        /// <summary>
        /// 봇 공지 · 관리봇(안쪽 칸 GroundRaised r8 · 위아래 12 · 좌우 16 · 가로 사이 12): 봇 칸 32(GroundCard · 아이콘 16 InkDim) /
        /// 캡션 줄(점 8 · 이름 InkSub · 시각 12 InkDim) → 4 → 글 14(관리봇은 InkSub). r 높이 = 12 + 16 + 4 + 본문 + 12.
        /// </summary>
        public static void Bot(Rect r, string icon, Color? dot, string name, string time, string body, bool muted = false)
        {
            var c = Surface.Frame(r, Surface.Kind.Inner, Surface.Kind.Card);
            if (!Draw.Repaint) return;
            Avatar35.Bot(new Rect(c.x, Mathf.Round(c.center.y - D35.AvatarL * 0.5f), D35.AvatarL, D35.AvatarL), icon, Pal.GroundCard);
            float x = c.x + D35.AvatarL + D35.Step12, w = Mathf.Max(0f, c.xMax - x);
            float cx = x;
            if (dot.HasValue)
            {
                StatusDot.Paint(new Vector2(cx + D35.Dot * 0.5f, c.y + D35.LineCaption * 0.5f), dot.Value);
                cx += D35.Dot + D35.IconTextGap;
            }
            cx = Seg(cx, c.y, name, D35.FontCaption, Pal.InkSub, false);
            Seg(cx, c.y, time, D35.FontCaption, Pal.InkDim, true);
            Draw.Text(new Rect(x, c.y + D35.LineCaption + D35.KpiGap, w, BodyH(body, w)), body, D35.FontBody, muted ? Pal.InkSub : Pal.InkTxt, false, TextAnchor.UpperLeft, true);
        }

        /// <summary>관리봇 한 줄 높이 52: 바깥 위아래 4 + 안쪽 칸(위아래 12 + 글 20).</summary>
        public static float BotLineH => D35.Step4 * 2f + D35.InnerPadY * 2f + D35.LineBody;

        /// <summary>
        /// 관리봇 한 줄(3.7 · Bot 변형): 메시지 칸 r(높이 BotLineH) 안 바깥 위아래 4 · 왼쪽 44 → 안쪽 칸(GroundRaised r8 · 위아래 12 · 좌우 16) 안 가로 사이 8:
        /// Shield 16 InkDim · 이름 14 InkSub · 글 14 InkSub(한 줄 — 넘치면 문구를 줄임).
        /// </summary>
        public static void BotLine(Rect r, string name, string body)
        {
            var box = new Rect(r.x + D35.ChatIndent, r.y + D35.Step4, Mathf.Max(0f, r.width - D35.ChatIndent), Mathf.Max(0f, r.height - D35.Step4 * 2f));
            var c = Surface.Frame(box, Surface.Kind.Inner, Surface.Kind.Card);
            if (!Draw.Repaint) return;
            float x = c.x, top = Mathf.Round(c.center.y - D35.LineBody * 0.5f);
            Draw.Icon(Icons.Shield, new Rect(x, Mathf.Round(c.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), Pal.InkDim, D35.IconBox, D35.IconLine);
            x += D35.IconS + D35.IconTextGap;
            if (!string.IsNullOrEmpty(name))
            {
                float nw = Mathf.Ceil(Draw.TextWidth(name, D35.FontBody));
                Draw.Text(new Rect(x, top, nw + 2f, D35.LineBody), name, D35.FontBody, Pal.InkSub);
                x += nw + D35.IconTextGap;
            }
            Draw.Text(new Rect(x, top, Mathf.Max(0f, c.xMax - x), D35.LineBody), body, D35.FontBody, Pal.InkSub);
        }

        /// <summary>한 줄 알림(들여쓰기 44 · 아이콘 16 → 8 → 글 14): 스레드 생김 36(Branch InkDim · 글 InkSub, 스레드 이름 name 만 InkTxt) · 지운 글 28(Trash · InkDim).</summary>
        public static void Note(Rect r, string icon, string lead, string name, string tail, Color ink)
        {
            if (!Draw.Repaint) return;
            float x = r.x + D35.ChatIndent;
            Draw.Icon(icon, new Rect(x, Mathf.Round(r.center.y - D35.IconS * 0.5f), D35.IconS, D35.IconS), Pal.InkDim, D35.IconBox, D35.IconLine);
            x += D35.IconS + D35.IconTextGap;
            float top = Mathf.Round(r.center.y - D35.LineBody * 0.5f);
            x = Piece(x, top, lead, ink);
            x = Piece(x, top, name, Pal.InkTxt);
            Piece(x, top, tail, ink);
        }

        static float Piece(float x, float top, string text, Color ink)
        {
            if (string.IsNullOrEmpty(text)) return x;
            float w = Mathf.Ceil(Draw.TextWidth(text, D35.FontBody));
            Draw.Text(new Rect(x, top, w + 2f, D35.LineBody), text, D35.FontBody, ink);
            return x + w + D35.LinkGap;
        }

        /// <summary>입력 중 줄(20 · 들여쓰기 44): 점 4 × 3(사이 4, InkDim, 멈춤) → 8 → 캡션 '3명이 입력 중'.</summary>
        public static void Typing(Rect r, string text)
        {
            if (!Draw.Repaint) return;
            float x = r.x + D35.ChatIndent, cy = r.center.y, d = D35.CalDot;
            for (int i = 0; i < 3; i++) Draw.Dot(new Vector2(x + d * 0.5f + (d + D35.Step4) * i, cy), d * 0.5f, Pal.InkDim);
            x += d * 3f + D35.Step4 * 2f + D35.IconTextGap;
            Draw.Text(new Rect(x, r.y, Mathf.Max(0f, r.xMax - x), r.height), text, D35.FontCaption, Pal.InkDim);
        }
    }

    /// <summary>
    /// 3.5 채팅 입력 줄: 48 · r8 · GroundRaised + Hairline · 왼쪽 16 · 오른쪽 8 · 사이 4: 글 칸(자리 글 14 InkDim) · 고스트 아이콘 버튼 32(Smile 18, '반응') · 주 아이콘 버튼 32(ArrowRight 18, '보내기').
    /// 보내기를 누르거나 글 칸에서 Enter → send. 바뀐 글을 돌려줌(보낸 뒤 비우기는 부르는 쪽).
    /// </summary>
    public static class ChatInput35
    {
        public static string Paint(Rect r, string value, int maxLength, string placeholder, Color accent, Color onAccent, string reactTip, string sendTip, out bool send, out bool react, string name = null) =>
            Core(r, value, maxLength, placeholder, accent, onAccent, true, reactTip, sendTip, out send, out react, name);

        /// <summary>반응 버튼 없는 꼴(3.7 — 반응 동작이 없는 화면): 글 칸 · 주 아이콘 버튼 32('보내기')만. 글 칸은 보내기 버튼 앞 4 까지.</summary>
        public static string Paint(Rect r, string value, int maxLength, string placeholder, Color accent, Color onAccent, string sendTip, out bool send, string name = null) =>
            Core(r, value, maxLength, placeholder, accent, onAccent, false, null, sendTip, out send, out _, name);

        static string Core(Rect r, string value, int maxLength, string placeholder, Color accent, Color onAccent, bool reactions, string reactTip, string sendTip, out bool send, out bool react, string name)
        {
            name ??= Field35.NameOf(Look.Key(r, "chatin35"));
            value ??= "";
            if (Draw.Repaint) Draw.Box(r, Pal.GroundRaised, Pal.Hairline, D35.RadiusCtrl);
            float b = D35.BtnSmallH, by = Mathf.Round(r.center.y - b * 0.5f);
            var sendR = new Rect(r.xMax - D35.Step8 - b, by, b, b);
            // 반응 없는 꼴은 반응 칸 폭 0 · 사이 0(글 칸이 보내기 앞 4 까지)
            var reactR = reactions ? new Rect(sendR.x - D35.Step4 - b, by, b, b) : new Rect(sendR.x, by, 0f, b);
            var ev = Event.current;
            bool focus = GUI.GetNameOfFocusedControl() == name;
            send = focus && ev.type == EventType.KeyDown && (ev.keyCode == KeyCode.Return || ev.keyCode == KeyCode.KeypadEnter) && !Draw.Composing && value.Length > 0;
            if (send) ev.Use();
            var tr = new Rect(r.x + D35.Step16, r.y, Mathf.Max(0f, reactR.x - D35.Step4 - r.x - D35.Step16), r.height);
            if (value.Length == 0 && !(focus && Draw.Composing) && !string.IsNullOrEmpty(placeholder) && Draw.Repaint)
                Draw.Text(tr, placeholder, D35.FontBody, Pal.InkDim);
            const float inset = 10f; // Draw.Field 안쪽 여백(글 시작을 16 에 맞춤)
            GUI.SetNextControlName(name);
            string v = Draw.Field(new Rect(tr.x - inset, r.y, tr.width + inset * 2f, r.height), value, D35.FontBody, maxLength);
            if (focus && ev.rawType == EventType.MouseDown && ev.button == 0 && !r.Contains(Draw.Mouse) && GUI.GetNameOfFocusedControl() == name)
            {
                GUI.FocusControl(null);
                GUIUtility.keyboardControl = 0;
            }
            react = reactions && IconButton35.Paint(reactR, Icons.Smile, reactTip, Tone35.Ghost, D35.IconM, true, Surface.Kind.Card);
            send |= Button35.Paint(sendR, null, Tone35.Primary, accent, onAccent, Icons.ArrowRight, v.Length > 0, false, sendTip, Surface.Kind.Card);
            return v;
        }
    }
}
