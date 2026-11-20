using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// IMGUI 그리기 바탕. 모든 좌표는 논리 좌표(화면 높이 1080 기준)로 받고 여기서 실제 화소로 바꿈 —
    /// GUI.matrix로 늘리지 않아 글자가 흐려지지 않음(글꼴 크기 자체를 배율만큼).
    /// 그리기는 Repaint 때만, 입력(버튼)은 모든 이벤트에서.
    /// </summary>
    internal static partial class Draw
    {
        /// <summary>논리 → 화소 배율. OnGUI 시작 때 Begin()이 정함.</summary>
        public static float S = 1f;

        static Vector2 offset;
        static readonly Stack<Vector2> offsets = new();
        static readonly Dictionary<int, GUIStyle> textStyles = new();
        static readonly Dictionary<int, GUIStyle> roundStyles = new();
        static readonly Dictionary<long, GUIStyle> outlineStyles = new();
        static readonly TexCache<long, Texture2D> rings = new(256, t => t);
        static Texture2D disc;
        static readonly GUIContent content = new();
        static readonly PerfWatch guiWatch = new("OnGUI");
        static long guiStart;

        public static bool Repaint => Event.current.type == EventType.Repaint;

        /// <summary>OnGUI 처음에 한 번. 배율 = 화면 높이 / 1080.</summary>
        public static void Begin()
        {
            S = Mathf.Max(0.5f, Screen.height / 1080f);
            offset = Vector2.zero;
            offsets.Clear();
            overlayDepth = 0;
            root = Vector2.zero;
            roots.Clear();
            clipBase = Vector2.zero;
            clipBases.Clear();
            visible = new Rect(0f, 0f, Width, Height);
            visibles.Clear();
            view = visible;
            views.Clear();
            // 새로 누를 때마다 지난 누름은 버림(다른 데를 눌렀다 옛 버튼 위에서 떼도 눌리지 않게)
            var e = Event.current;
            guiStart = PerfWatch.Now;
            if (e != null && e.type == EventType.Repaint) InputBlocker.Painted(); // 허브 공유: 그린 순서(모드끼리 가림)
            if (e != null && e.type == EventType.MouseDown) held = false;
            // 스크롤 막대를 끄는 동안 다른 누름 · 휠은 아무 데도 안 감(뗌 · 끌기는 막대가 받음)
            if (e != null && ScrollBar.Dragging && (e.type == EventType.MouseDown || e.type == EventType.ScrollWheel)) e.Use();
            // 뗀 다음 프레임에 풂(같은 프레임의 다른 OnGUI 가 먼저 풀면 창 단추가 뗌을 못 봄)
            if (releaseFrame >= 0 && Time.frameCount != releaseFrame) { held = false; releaseFrame = -1; }
            // 열린 고르기 목록이 창보다 먼저 클릭을 받음
            BeginOverlay(); // 열린 고르기 목록은 맨 위 판 — 툴바 칸에 막히지 않음
            Select.PreInput();
            EndOverlay();
        }

        /// <summary>논리 화면 크기.</summary>
        public static float Width => Screen.width / S;
        public static float Height => Screen.height / S;

        public static Rect ToScreen(Rect r) => new((r.x + offset.x) * S, (r.y + offset.y) * S, r.width * S, r.height * S);
        public static Vector2 ToScreen(Vector2 p) => new((p.x + offset.x) * S, (p.y + offset.y) * S);

        /// <summary>논리 마우스 자리(지금 잘라낸 영역 기준).</summary>
        public static Vector2 Mouse => Event.current.mousePosition / S - offset;

        /// <summary>
        /// 마우스가 칸 안이고 지금 잘라낸 영역(Visible) 안일 때만 참. 잘라낸 밖(스크롤로 머리 · 툴바 밑으로 들어간 칸)은 눌림 · 호버를 가로채지 않음.
        /// </summary>
        public static bool Hover(Rect r) { var m = Mouse; return r.Contains(m) && visible.Contains(m) && !UnderOverlay(); }

        // ---- 앞 판(하단 툴바 · 서브 툴바 · FAB · 독 · 독 아래 판)이 덮은 칸
        // 창 몸통은 툴바보다 먼저 그려져(먼저 입력을 받아) 툴바 칸 · 칸 사이 · 여백을 누르면 그 밑 단추가 눌렸음.
        // 앞 판은 Block(판 칸) 으로 이번 · 지난 프레임 자리를 남기고 BeginOverlay ~ EndOverlay 안에서 제 칸을 그림 —
        // 판 밖(overlay 깊이 0)의 Hover · Button 은 그 칸 안 마우스를 못 받음(같은 프레임의 Layout 이 먼저 돌아 MouseDown 전에 남음).
        static readonly List<Rect> blocks = new(), lastBlocks = new();
        static int blockFrame = -1, overlayDepth;
        // 판 칸이 바뀔 때마다(새 프레임 · 칸 더함 · 비움) 올림 — UnderOverlay 가 같은 판 칸 · 같은 마우스면 지난 결과를 씀
        static int blockVer, underVer = -1;
        static Vector2 underAt;
        static bool under;

        /// <summary>앞 판 칸(지금 좌표)을 남김 — 판 밖 위젯이 이 칸 안 마우스를 못 받음. 모든 이벤트에서 부를 것.</summary>
        public static void Block(Rect r)
        {
            RollBlocks();
            var a = ToRoot(r);
            if (a.width > 0f && a.height > 0f && !blocks.Contains(a)) { blocks.Add(a); blockVer++; }
        }

        /// <summary>앞 판 그리기 시작 · 끝(짝): 안쪽 위젯은 Block 칸에 막히지 않음.</summary>
        public static void BeginOverlay() => overlayDepth++;

        /// <summary>앞 판 칸을 모두 풂 — 모달 덮개(Modal.Scrim)가 창을 덮으면 모달 단추가 툴바 칸에 막히지 않게.</summary>
        public static void ClearBlocks()
        {
            RollBlocks();
            blocks.Clear();
            lastBlocks.Clear();
            blockVer++;
        }
        public static void EndOverlay() { if (overlayDepth > 0) overlayDepth--; }

        static void RollBlocks()
        {
            int f = Time.frameCount;
            if (blockFrame == f) return;
            lastBlocks.Clear();
            if (blockFrame == f - 1) lastBlocks.AddRange(blocks);
            blocks.Clear();
            blockFrame = f;
            blockVer++;
        }

        /// <summary>마우스가 앞 판 밑인가(앞 판 안 위젯이면 false — 위에 그려진 다른 모드 창 밑이면 늘 true, 허브 있을 때만). 창 몸통 휠 스크롤도 Hover 로 보면 같이 막힘.</summary>
        public static bool UnderOverlay()
        {
            var e = Event.current;
            if (e == null) return false;
            var m = e.mousePosition / S + root;
            if (InputBlocker.Covered(m)) return true; // 다른 모드 창이 위(허브 있을 때만) — 앞 판 안 위젯도 막힘
            if (overlayDepth > 0) return false;
            RollBlocks();
            if (underVer == blockVer && underAt == m) return under;
            underVer = blockVer;
            underAt = m;
            under = false;
            foreach (var b in blocks) if (b.Contains(m)) return under = true;
            foreach (var b in lastBlocks) if (b.Contains(m)) return under = true;
            return false;
        }

        /// <summary>전환 애니메이션용: 이 값(0 ~ 1)을 모든 그리기 색 알파에 곱함. 쓰고 나면 되돌릴 것.</summary>
        public static float Opacity = 1f;

        static Color Op(Color c)
        {
            c.a *= Opacity;
            return c;
        }

        /// <summary>전환 애니메이션용: 뒤에 그리는 것을 d 만큼 밀어 그림(마우스 판정도 같이). Shift(-d) 로 되돌림.</summary>
        public static void Shift(Vector2 d) => offset += d;

        /// <summary>
        /// 글 속 목록 점(시안 '글 속 목록 점'): 글자 • 대신 도형 — 지름 = 글 크기 × 0.38(13.5 → 5), 깊이 0 채운 원 → 1 빈 원(선 × 0.24) → 2 채운 네모(× 0.8 → 4) → 다시 채운 원.
        /// center = 첫 줄 높이의 가운데, 색 = 글과 같은 색.
        /// </summary>
        public static void Bullet(Vector2 center, float fontSize, int depth, Color c)
        {
            float d = fontSize * 0.38f;
            switch (depth % 3)
            {
                case 1: Ring(center, d * 0.5f - d * 0.12f, d * 0.24f, c); break;
                case 2: Fill(new Rect(center.x - d * 0.4f, center.y - d * 0.4f, d * 0.8f, d * 0.8f), c); break;
                default: Dot(center, d * 0.5f, c); break;
            }
        }

        static readonly Stack<Matrix4x4> matrices = new();

        /// <summary>
        /// 눌림 · 펼침 움직임: 뒤에 그리는 것을 pivot(논리 좌표) 기준 s배로(글꼴 크기는 그대로 — TextScaled 와 같은 까닭). 판정 칸은 그대로. EndScale 과 짝.
        /// </summary>
        public static void BeginScale(Vector2 pivot, float s)
        {
            matrices.Push(GUI.matrix);
            if (Mathf.Abs(s - 1f) > 0.001f) GUIUtility.ScaleAroundPivot(new Vector2(s, s), ToScreen(pivot));
        }

        public static void EndScale()
        {
            if (matrices.Count > 0) GUI.matrix = matrices.Pop();
        }

        /// <summary>잘라 그리기 시작. 안쪽 좌표는 r 왼쪽 위 기준, scroll만큼 밀림.</summary>
        public static void BeginClip(Rect r, Vector2 scroll)
        {
            GUI.BeginGroup(ToScreen(r));
            // 그리기 거름 칸(그룹 안 좌표, Shift 반영) = 바깥 거름 칸 ∩ 그룹 칸
            views.Push(view);
            float gx = r.x + offset.x, gy = r.y + offset.y;
            float vx0 = Mathf.Max(view.x, gx), vy0 = Mathf.Max(view.y, gy);
            view = new Rect(vx0 - gx, vy0 - gy, Mathf.Max(0f, Mathf.Min(view.xMax, gx + r.width) - vx0), Mathf.Max(0f, Mathf.Min(view.yMax, gy + r.height) - vy0));
            roots.Push(root);
            // 맨 바깥 영역(창 몸통 — 창을 끌면 움직임)은 빼고 안쪽 영역 자리만 더함: 끄는 동안 열쇠가 바뀌어 애니 칸이 쌓이지 않게
            if (clipBases.Count > 0) clipBase += r.position;
            clipBases.Push(clipBase);
            root += r.position + offset;
            offsets.Push(offset);
            offset = -scroll;
            // 보이는 영역(안쪽 좌표) = 바깥 보이는 영역 ∩ r, 안쪽 좌표로 옮김
            visibles.Push(visible);
            float x0 = Mathf.Max(visible.x, r.x), y0 = Mathf.Max(visible.y, r.y);
            float x1 = Mathf.Min(visible.xMax, r.xMax), y1 = Mathf.Min(visible.yMax, r.yMax);
            visible = new Rect(x0 - r.x + scroll.x, y0 - r.y + scroll.y, Mathf.Max(0f, x1 - x0), Mathf.Max(0f, y1 - y0));
        }

        public static void EndClip()
        {
            offset = offsets.Pop();
            if (roots.Count > 0) root = roots.Pop();
            if (clipBases.Count > 0) clipBase = clipBases.Pop();
            if (visibles.Count > 0) visible = visibles.Pop();
            if (views.Count > 0) view = views.Pop();
            GUI.EndGroup();
        }

        // 그리기 거름 칸: 지금 그룹 안 좌표(r + offset)로 본 보이는 곳. Begin 전에도 다 그리게 처음엔 아주 넓게
        static Rect view = new(-1e6f, -1e6f, 2e6f, 2e6f);
        static readonly Stack<Rect> views = new();

        /// <summary>글 거름에서 가로는 보지 않음(넘쳐 그리는 글).</summary>
        const float NoX = 1e6f;

        /// <summary>
        /// 화면 밖 칸이면 true(그리지 않음). pad = 칸 밖으로 번지는 만큼(논리). GUI.matrix 가 걸려 있으면(확대 · 회전 · 기울임) 거르지 않음.
        /// 입력(Hover · Button)은 거르지 않음.
        /// </summary>
        static bool Off(Rect r, float padX, float padY)
        {
            float x = r.x + offset.x, y = r.y + offset.y;
            if (x + r.width + padX > view.x && x - padX < view.xMax && y + r.height + padY > view.y && y - padY < view.yMax) return false;
            var m = GUI.matrix;
            return m.m00 == 1f && m.m11 == 1f && m.m01 == 0f && m.m10 == 0f && m.m03 == 0f && m.m13 == 0f;
        }

        /// <summary>글 거름: 잘라 그리는 글은 칸 안, 넘쳐 그리는 글(MinText 모드 · 맞춤 실패)은 세로만 글 크기 2배 여유.</summary>
        static bool TextOff(Rect r, float size, bool overflow) => Off(r, overflow ? NoX : 1f, overflow ? Mathf.Max(size, MinText) * 2f : 1f);

        // 잘라 그리는 영역(BeginClip)들의 화면 기준 시작점 — 영역 안 좌표를 맨 위(영역 밖) 좌표로 바꿀 때
        static Vector2 root;
        static readonly Stack<Vector2> roots = new();

        /// <summary>
        /// 잘라 그리는 영역들의 자리 합(스크롤 · 밀림 빼고 — 스크롤해도 안 바뀜). 영역마다 안쪽 좌표가 0 부터라
        /// 다른 영역의 같은 자리 칸(설정 패널 두 섹션의 2번째 행 등)이 같은 애니 열쇠를 나눠 쓰지 않게 Look.Key 가 더함.
        /// </summary>
        public static Vector2 ClipBase => clipBase;
        static Vector2 clipBase;
        static readonly Stack<Vector2> clipBases = new();

        /// <summary>지금 좌표(잘라 그리는 영역 안일 수 있음)의 칸을 맨 위 논리 좌표로(영역 밖 맨 위에 뜨는 목록 · 말풍선 자리).</summary>
        public static Rect ToRoot(Rect r) => new(r.x + offset.x + root.x, r.y + offset.y + root.y, r.width, r.height);

        static Rect visible;
        static readonly Stack<Rect> visibles = new();

        /// <summary>
        /// 지금 잘라낸 영역에서 실제로 보이는 곳(논리 좌표). 돌려 그리는 선(Line · Polyline)은 GUI.matrix 회전 때문에
        /// BeginClip 잘라내기가 먹지 않아 — 영역 밖으로 나갈 수 있는 선은 이걸로 직접 잘라 그림.
        /// </summary>
        public static Rect Visible => visible;
    }
}
