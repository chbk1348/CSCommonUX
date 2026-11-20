using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Select — 목록에서 고르기(시안 'CSCommonUX.Select'): 알약 칸(bg 없으면 Look.Pill · 반지름 = 높이/2 · 왼쪽 16 · 오른쪽 12 · 색 점 8 + 8 · 글 14.5 Look.BarInk · ▾ 16 Section.SubInk).
    /// 누르면 칸 아래 6 에 둥근 목록(반지름 24 · 안쪽 6 · Look.MenuSurface + 1px Look.MenuLine, 그림자 없음 — 다크에서도 바탕에 안 묻힘): 줄 40 · 반지름 20 · 좌우 14 · 사이 10 ·
    /// 왼쪽 표시 18(하나 ○ 고리 1.5 + 점 8 · 여럿 □ 반지름 6 + ✓ 12) · 글 14, 고른 줄 = 모드 파스텔 + 굵은 파스텔 글 · 호버 Look.MenuHover · 누르면 0.95배.
    /// 열린 칸 = 안쪽 1.5px Look.PastelRing + ▾ 180° 돌기(0.22초). 목록은 칸 쪽에서 펼쳐짐(0.24초 Modal.Ease) + 투명 0.16초, 닫을 때 거꾸로(그동안 목록 자리 클릭은 먹음).
    /// 하나 고르기는 고르면 닫힘 · 여럿 고르기(Many)는 열어 둠 · 밖을 누르면 닫힘. Close() 는 움직임 없이 바로 닫음(창을 닫거나 탭 · 스크롤이 바뀔 때).
    /// 목록은 다른 칸보다 위에 그려야 하므로 그 프레임 끝 Flush() 에서 그림(창 · 판 OnGUI 끝에서 부름).
    /// </summary>
    public static class Select
    {
        /// <summary>칸: 왼쪽 · 오른쪽 · 점 · 점 → 글 · 글 · ▾ · ▾ 선 · 열림 테두리 · 누름 배율 · ▾ 도는 초.</summary>
        public const float PadL = 16f, PadR = 12f, Dot = 8f, DotGap = 8f, Font = 14.5f, Chevron = 16f, ChevronStroke = 2.2f, RingW = 1.5f, PressScale = 0.95f, TurnSeconds = 0.22f;
        /// <summary>목록: 칸과 사이 · 반지름 · 최소 폭 · 줄 좌우 · 줄 안 사이 · 줄 글 · 고른 점 · 상자 반지름 · ✓ · ✓ 선.</summary>
        public const float MenuGap = 6f, MenuRadius = 24f, MenuMinW = 200f, RowPadX = 14f, RowGap = 10f, RowFont = 14f, MarkDot = 8f, BoxRadius = 6f, Check = 12f, CheckStroke = 3f;

        static int openKey;
        static Rect openRect;
        static string[] openItems;
        static Color?[] openDots;
        static int openCur;
        static bool[] openOn;
        static string[] openHeads;
        static Action<int> openPick;
        static float openAt;
        /// <summary>열린 칸을 마지막으로 그린 프레임 — 칸이 안 그려지면(창을 닫음) 목록도 닫음.</summary>
        static int openSeenFrame;

        /// <summary>지난 그림의 목록 판 · 줄 자리(바탕 좌표) — 창보다 먼저 클릭을 받는 데 씀(PreInput).</summary>
        static Rect menuRect;
        static readonly List<Rect> itemRects = new();
        static int pressed = -1;

        /// <summary>닫히는 중: 닫기 시작한 때(없으면 −1) · 그때 펼친 정도 · 투명도 · 닫히는 칸 · 그 칸을 마지막으로 그린 프레임.</summary>
        static float closeAt = -1f, closeFrom, closeFade;
        static int closingKey, closingSeen;

        public static bool AnyOpen => openKey != 0;

        static bool Closing => closeAt >= 0f && Time.unscaledTime - closeAt < CSCommonDesign.MenuOpenSeconds;

        /// <summary>열린 목록을 바로 닫음(탭을 바꾸거나 스크롤할 때 — 움직임 없음).</summary>
        public static void Close()
        {
            openKey = 0;
            closeAt = -1f;
            itemRects.Clear();
            pressed = -1;
        }

        /// <summary>사용자가 닫음(고름 · 밖 누름 · 칸 다시 누름): 지금 펼친 정도에서 거꾸로 접히며 사라짐.</summary>
        static void BeginClose()
        {
            float now = Time.unscaledTime, el = now - openAt;
            closeFrom = Modal.Ease(el / CSCommonDesign.MenuOpenSeconds);
            closeFade = Mathf.Clamp01(el / CSCommonDesign.MenuFadeSeconds);
            closeAt = now;
            closingKey = openKey;
            closingSeen = Time.frameCount;
            openKey = 0;
            itemRects.Clear();
            pressed = -1;
        }

        /// <summary>
        /// GUI 차례 맨 앞(Draw.Begin)에서 열린 목록이 먼저 클릭을 받음: 목록 안 누름은 여기서 쓰고(떼면 고름), 밖 누름은 목록을 닫고 아래로 보냄.
        /// 목록은 창 위에 그리지만(Flush) 창이 먼저 그려져 — 목록 아래 카드 단추나 창의 '창 위 클릭은 여기서 끝' 처리가 누름을 먼저 써 버려
        /// 목록 줄을 눌러도 고를 수 없던 것(올림픽 구역 지구 고르기).
        /// </summary>
        internal static void PreInput()
        {
            var e = Event.current;
            if (e == null) return;
            Vector2 m = e.mousePosition / Draw.S;
            if (openKey == 0)
            {
                // 접히는 동안 목록 자리 누름 · 끌기 · 뗌은 여기서 먹음(아래 창 · 지도로 새지 않게, 위로 열린 목록도 같은 자리)
                if (Closing && menuRect.Contains(m) && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag || e.rawType == EventType.MouseUp)) e.Use();
                return;
            }
            if (itemRects.Count == 0) return;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (menuRect.Contains(m))
                {
                    pressed = itemRects.FindIndex(it => it.Contains(m));
                    if (pressed < 0) pressed = -2; // 판 여백 — 쓰기만
                    e.Use();
                }
                else if (!openRect.Contains(m)) BeginClose();
            }
            else if (e.type == EventType.MouseDrag && pressed != -1) e.Use();
            else if (e.rawType == EventType.MouseUp && pressed != -1)
            {
                int i = pressed;
                pressed = -1;
                e.Use();
                if (i >= 0 && i < itemRects.Count && itemRects[i].Contains(m))
                {
                    var pick = openPick;
                    if (openOn == null) BeginClose();
                    pick?.Invoke(i);
                }
            }
        }

        /// <summary>하나 고르기 칸(bg = 닫힌 칸 바탕, 없으면 Look.Pill). heads[i] = 줄 i 위에 둘 묶음 머리(없으면 null).</summary>
        public static void In(Rect r, string text, Color? dot, string[] items, Color?[] dots, int cur, Action<int> pick, bool enabled = true, string tip = null, Color? bg = null, string[] heads = null)
            => Field(r, text, dot, items, dots, cur, null, pick, enabled, tip, bg, heads);

        /// <summary>여럿 고르기 칸: on[i] = 고름, 누르면 toggle(i) · 목록은 열어 둠. text = 칸에 보일 요약(예: '세션 2개').</summary>
        public static void Many(Rect r, string text, string[] items, bool[] on, Action<int> toggle, bool enabled = true, string tip = null, Color? bg = null, string[] heads = null)
            => Field(r, text, null, items, null, -1, on, toggle, enabled, tip, bg, heads);

        static void Field(Rect r, string text, Color? dot, string[] items, Color?[] dots, int cur, bool[] on, Action<int> pick, bool enabled, string tip, Color? bg, string[] heads)
        {
            int key = Look.Key(r, "select", on != null ? "many" : text) | 1;
            bool open = openKey == key;
            if (open) openSeenFrame = Time.frameCount;
            if (key == closingKey) closingSeen = Time.frameCount;
            float turn = CSCommonAnim.Toward(key ^ 0x6a09e667, open ? 1f : 0f, 1f / TurnSeconds);
            float pr = Look.Press(key, r, enabled);
            if (Draw.Repaint)
            {
                float rad = r.height * 0.5f;
                Draw.BeginScale(r.center, Look.PressScale(pr, PressScale));
                Draw.Round(r, !enabled ? Look.BtnOff : bg ?? Look.Pill, rad);
                if (turn > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Look.PastelRing, turn), rad, RingW);
                float tx = r.x + PadL;
                if (dot.HasValue) { Draw.Dot(new Vector2(tx + Dot * 0.5f, r.center.y), Dot * 0.5f, enabled ? dot.Value : Look.IconOffInk); tx += Dot + DotGap; }
                var cr = new Rect(r.xMax - PadR - Chevron, Mathf.Round(r.center.y - Chevron * 0.5f), Chevron, Chevron);
                Draw.TextPill(new Rect(tx, r.y, Mathf.Max(0f, cr.x - DotGap - tx), r.height), text, Font, enabled ? Look.BarInk : Look.BtnOffInk, false, TextAnchor.MiddleLeft);
                // ▾ 돌기: 시안 곡선(열 때 Ease, 닫을 때 거꾸로)
                float deg = 180f * (open ? Modal.Ease(turn) : 1f - Modal.Ease(1f - turn));
                var m = GUI.matrix;
                if (deg > 0.5f) GUIUtility.RotateAroundPivot(deg, Draw.ToScreen(cr).center);
                Draw.Icon(Icons.Chevron, cr, enabled ? Section.SubInk : Look.BtnOffInk, 24f, ChevronStroke);
                GUI.matrix = m;
                Draw.EndScale();
            }
            CSCommonUX.CSCommonToolbar.Tip(r, tip);
            if (!enabled) return;
            bool hit = Draw.Button(r);
            if (hit && open)
            {
                BeginClose();
                return;
            }
            if (hit)
            {
                closeAt = -1f;
                openKey = key;
                openSeenFrame = Time.frameCount;
                itemRects.Clear();
                pressed = -1;
                openAt = Time.unscaledTime;
                open = true;
            }
            if (open)
            {
                // 같은 프레임 끝에 그릴 목록(자리 · 값 갱신)
                openRect = Draw.ToRoot(r);
                openItems = items;
                openDots = dots;
                openCur = cur;
                openOn = on;
                openHeads = heads;
                openPick = pick;
            }
        }

        /// <summary>열린 목록을 맨 위에 그림 · 하나 고르기는 고르면 닫음 · 밖을 누르면 닫음. 닫히는 중이면 접히는 목록만(누름 없음).</summary>
        public static void Flush()
        {
            // 열린 목록은 맨 위 판 — 툴바 · 독 칸(Draw.Block)에 막히지 않음
            Draw.BeginOverlay();
            try { FlushMenu(); }
            finally { Draw.EndOverlay(); }
        }

        static void FlushMenu()
        {
            if (openItems == null) return;
            bool live = openKey != 0;
            float now = Time.unscaledTime, shown, fade;
            if (live)
            {
                // 목록을 연 칸이 이번(또는 바로 앞) 프레임에 안 그려졌으면 그 창이 닫힌 것 — 목록만 남아 잔상으로 보이던 것(열린 채 X 로 닫기)
                if (Time.frameCount - openSeenFrame > 1) { Close(); return; }
                float el = now - openAt;
                shown = Modal.Ease(el / CSCommonDesign.MenuOpenSeconds);
                fade = Mathf.Clamp01(el / CSCommonDesign.MenuFadeSeconds);
            }
            else
            {
                // 닫히는 중: 연 칸이 안 그려지면(창이 닫힘) 접힘도 그만
                if (!Closing || Time.frameCount - closingSeen > 1) { closeAt = -1f; return; }
                float el = now - closeAt;
                shown = closeFrom * (1f - Modal.Ease(el / CSCommonDesign.MenuOpenSeconds));
                fade = closeFade * (1f - Mathf.Clamp01(el / CSCommonDesign.MenuFadeSeconds));
            }
            float row = CSCommonDesign.MenuRowH, pad = CSCommonDesign.MenuPad, mark = CSCommonDesign.MenuMark;
            bool many = openOn != null;
            // 묶음 머리: 첫 묶음은 머리 줄만, 다음 묶음부터 위 구분선 + 4
            float HeadAt(int i) => openHeads != null && i < openHeads.Length && !string.IsNullOrEmpty(openHeads[i]) ? CSCommonDesign.MenuHeadH + (i > 0 ? 4f : 0f) : 0f;
            float body = 0f;
            for (int i = 0; i < openItems.Length; i++) body += HeadAt(i) + row;
            var menu = new Rect(openRect.x, openRect.yMax + MenuGap, Mathf.Max(openRect.width, MenuMinW), pad * 2f + body);
            bool above = menu.yMax > Draw.Height - 8f;
            if (above) menu.y = openRect.y - MenuGap - menu.height;
            menuRect = menu;
            if (live) itemRects.Clear();
            // 칸 쪽에서 펼쳐짐(아래로 열면 위부터, 위로 열면 아래부터 보임) — 1px 선이 잘리지 않게 바깥 1 을 더 둠
            float sh = Mathf.Round((menu.height + 2f) * shown);
            var clip = new Rect(menu.x - 1f, above ? menu.yMax + 1f - sh : menu.y - 1f, menu.width + 2f, sh);
            float opacity = Draw.Opacity;
            Draw.Opacity *= fade;
            // 안쪽 좌표를 바깥과 같게(scroll = 자른 칸 자리) — 줄 자리(itemRects)는 그대로 바탕 좌표
            Draw.BeginClip(clip, clip.position);
            if (Draw.Repaint)
            {
                Draw.Round(menu, Look.MenuSurface, MenuRadius);
                Draw.RoundOutline(menu, Look.MenuLine, MenuRadius);
            }
            int picked = -1;
            float iy = menu.y + pad;
            for (int i = 0; i < openItems.Length; i++)
            {
                float hh = HeadAt(i);
                if (hh > 0f && Draw.Repaint)
                {
                    if (i > 0) Draw.Fill(new Rect(menu.x + pad + RowPadX, iy + 2f, menu.width - (pad + RowPadX) * 2f, 1f), Look.Divider);
                    Draw.TextFit(new Rect(menu.x + pad + RowPadX, iy + hh - CSCommonDesign.MenuHeadH, menu.width - (pad + RowPadX) * 2f, CSCommonDesign.MenuHeadH), openHeads[i], CSCommonDesign.FontMin, Section.SubInk, true);
                }
                iy += hh;
                var it = new Rect(menu.x + pad, iy, menu.width - pad * 2f, row);
                if (live) itemRects.Add(it);
                iy += row;
                bool on = many ? i < openOn.Length && openOn[i] : i == openCur;
                int k = Look.Key(it, "select.row", openItems[i]);
                float onT = CSCommonAnim.Toward(k, on ? 1f : 0f, 1f / Tabs.FadeSeconds);
                float pr = Look.Press(k ^ 0x51ed270b, it, live);
                if (Draw.Repaint)
                {
                    float rad = row * 0.5f;
                    Draw.BeginScale(it.center, Look.PressScale(pr, PressScale));
                    if (onT > 0.01f) Draw.Round(it, Draw.Alpha(Look.PastelFill, onT), rad);
                    var m = new Rect(it.x + RowPadX, Mathf.Round(it.center.y - mark * 0.5f), mark, mark);
                    if (many) Box(m, onT);
                    else Radio(m, onT);
                    float tx = m.xMax + RowGap;
                    if (openDots != null && i < openDots.Length && openDots[i].HasValue) { Draw.Dot(new Vector2(tx + Dot * 0.5f, it.center.y), Dot * 0.5f, openDots[i].Value); tx += Dot + RowGap; }
                    Draw.TextFit(new Rect(tx, it.y, it.xMax - tx - RowPadX, it.height), openItems[i], RowFont, Color.Lerp(Look.BarInk, Look.PastelInk, onT), on);
                    Draw.EndScale();
                }
                if (live && picked < 0 && Draw.Button(it)) picked = i;
            }
            Draw.EndClip();
            Draw.Opacity = opacity;
            if (!live) return;
            if (picked >= 0)
            {
                var pick = openPick;
                if (!many) BeginClose();
                pick?.Invoke(picked);
                return;
            }
            var e = Event.current;
            if (e.type == EventType.MouseDown && !menu.Contains(Draw.Mouse) && !openRect.Contains(Draw.Mouse)) BeginClose();
        }

        /// <summary>하나 고르기 표시(t = 고름 0 ~ 1): 1.5 고리(안 고름 Look.BtnOffInk → 고름 Look.PastelInk) + 가운데 점 8.</summary>
        static void Radio(Rect m, float t)
        {
            Draw.Ring(m.center, m.width * 0.5f - RingW * 0.5f, RingW, Color.Lerp(Look.BtnOffInk, Look.PastelInk, t));
            if (t > 0.01f) Draw.Dot(m.center, MarkDot * 0.5f, Draw.Alpha(Look.PastelInk, t));
        }

        /// <summary>여럿 고르기 표시(t = 고름 0 ~ 1): 반지름 6 상자 1.5 선 + ✓ 12.</summary>
        static void Box(Rect m, float t)
        {
            Draw.RoundOutline(m, Color.Lerp(Look.BtnOffInk, Look.PastelInk, t), BoxRadius, RingW);
            if (t > 0.01f) Draw.Icon(Icons.Done, new Rect(Mathf.Round(m.center.x - Check * 0.5f), Mathf.Round(m.center.y - Check * 0.5f), Check, Check), Draw.Alpha(Look.PastelInk, t), 24f, CheckStroke);
        }
    }
}
