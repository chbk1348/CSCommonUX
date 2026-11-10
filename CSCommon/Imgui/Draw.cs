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
    internal static class Draw
    {
        /// <summary>논리 → 화소 배율. OnGUI 시작 때 Begin()이 정함.</summary>
        public static float S = 1f;

        static Vector2 offset;
        static readonly Stack<Vector2> offsets = new();
        static readonly Dictionary<int, GUIStyle> textStyles = new();
        static readonly Dictionary<int, GUIStyle> roundStyles = new();
        static readonly Dictionary<long, GUIStyle> outlineStyles = new();
        static readonly Dictionary<long, Texture2D> rings = new();
        static Texture2D disc;
        static readonly GUIContent content = new();

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
            // 새로 누를 때마다 지난 누름은 버림(다른 데를 눌렀다 옛 버튼 위에서 떼도 눌리지 않게)
            var e = Event.current;
            if (e != null && e.type == EventType.MouseDown) held = false;
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

        /// <summary>앞 판 칸(지금 좌표)을 남김 — 판 밖 위젯이 이 칸 안 마우스를 못 받음. 모든 이벤트에서 부를 것.</summary>
        public static void Block(Rect r)
        {
            RollBlocks();
            var a = ToRoot(r);
            if (a.width > 0f && a.height > 0f && !blocks.Contains(a)) blocks.Add(a);
        }

        /// <summary>앞 판 그리기 시작 · 끝(짝): 안쪽 위젯은 Block 칸에 막히지 않음.</summary>
        public static void BeginOverlay() => overlayDepth++;

        /// <summary>앞 판 칸을 모두 풂 — 모달 덮개(Modal.Scrim)가 창을 덮으면 모달 단추가 툴바 칸에 막히지 않게.</summary>
        public static void ClearBlocks()
        {
            RollBlocks();
            blocks.Clear();
            lastBlocks.Clear();
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
        }

        /// <summary>마우스가 앞 판 밑인가(앞 판 안 위젯이면 늘 false). 창 몸통 휠 스크롤도 Hover 로 보면 같이 막힘.</summary>
        public static bool UnderOverlay()
        {
            var e = Event.current;
            if (overlayDepth > 0 || e == null) return false;
            RollBlocks();
            var m = e.mousePosition / S + root;
            foreach (var b in blocks) if (b.Contains(m)) return true;
            foreach (var b in lastBlocks) if (b.Contains(m)) return true;
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
            GUI.EndGroup();
        }

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

        // ---------------- 채우기 · 테두리

        public static void Fill(Rect r, Color c)
        {
            if (!Repaint || c.a <= 0f) return;
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(r), Texture2D.whiteTexture);
            GUI.color = old;
        }

        public static void Border(Rect r, Color c, float w = 1f)
        {
            Fill(new Rect(r.x, r.y, r.width, w), c);
            Fill(new Rect(r.x, r.yMax - w, r.width, w), c);
            Fill(new Rect(r.x, r.y + w, w, r.height - 2f * w), c);
            Fill(new Rect(r.xMax - w, r.y + w, w, r.height - 2f * w), c);
        }

        /// <summary>
        /// 9-slice 모서리 화소: 화면 칸 짧은 변의 절반까지(넘지 않게). 알약처럼 radius = 높이/2 일 때
        /// 반올림으로 모서리가 칸 절반보다 커지면 IMGUI 가 모서리를 찌그러뜨려 팔각형 · 가운데 겹친 상자가 보이고,
        /// 절반보다 작게 자르면 스위치 · 알약 끝이 반원이 아니게 됨.
        /// </summary>
        static int SlicePx(Rect r, float radius)
        {
            int fit = Mathf.FloorToInt(Mathf.Min(r.width, r.height) * S * 0.5f);
            return Mathf.Max(1, Mathf.Min(Mathf.RoundToInt(radius * S), fit));
        }

        /// <summary>둥근 모서리 채우기. radius는 논리 단위.</summary>
        public static void Round(Rect r, Color c, float radius)
        {
            if (!Repaint || c.a <= 0f) return;
            int px = SlicePx(r, radius);
            if (!roundStyles.TryGetValue(px, out var style))
            {
                style = NineSlice(MakeRounded(px, 0), px);
                roundStyles[px] = style;
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            style.Draw(Snap(ToScreen(r)), false, false, false, false);
            GUI.color = old;
        }

        /// <summary>
        /// 아래 두 모서리만 둥근 채우기(창 몸통 바닥 띠 — 위는 머리 · 탭 줄에 붙음). 네모 Fill 로 창 바닥까지 칠하면 둥근 창 아래 모서리가 직각으로 보였음.
        /// 겹쳐 칠하지 않음(열기 애니 투명도에서 띠가 진해지지 않게): 위 = 네모, 아래 radius 띠 = 2배 높이 둥근 면의 아래 절반만.
        /// </summary>
        public static void RoundBottom(Rect r, Color c, float radius)
        {
            if (!Repaint || c.a <= 0f) return;
            radius = Mathf.Min(radius, r.height * 0.5f);
            Fill(new Rect(r.x, r.y, r.width, r.height - radius), c);
            BeginClip(new Rect(r.x, r.yMax - radius, r.width, radius), Vector2.zero);
            Round(new Rect(0f, -radius, r.width, radius * 2f), c, radius);
            EndClip();
        }

        /// <summary>화면 픽셀 경계로 맞춤 — 칸 끝이 소수 픽셀이면 9칸 그림이 번져 테두리가 변마다 두께 · 진하기가 달라 보였음.</summary>
        static Rect Snap(Rect s)
        {
            float x0 = Mathf.Round(s.x), y0 = Mathf.Round(s.y);
            return new Rect(x0, y0, Mathf.Round(s.xMax) - x0, Mathf.Round(s.yMax) - y0);
        }

        /// <summary>둥근 모서리 테두리.</summary>
        public static void RoundOutline(Rect r, Color c, float radius, float width = 1f)
        {
            if (!Repaint || c.a <= 0f) return;
            int px = SlicePx(r, radius);
            int wpx = Mathf.Max(1, Mathf.RoundToInt(width * S));
            long key = ((long)px << 16) | (uint)wpx;
            if (!outlineStyles.TryGetValue(key, out var style))
            {
                style = NineSlice(MakeRounded(Mathf.Max(px, wpx), wpx), Mathf.Max(px, wpx));
                outlineStyles[key] = style;
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            style.Draw(Snap(ToScreen(r)), false, false, false, false);
            GUI.color = old;
        }

        /// <summary>채우고 테두리까지(카드 · 버튼).</summary>
        public static void Box(Rect r, Color fill, Color line, float radius, float lineWidth = 1f)
        {
            Round(r, fill, radius);
            RoundOutline(r, line, radius, lineWidth);
        }

        static GUIStyle NineSlice(Texture2D tex, int border)
        {
            var style = new GUIStyle();
            style.normal.background = tex;
            style.border = new RectOffset(border, border, border, border);
            return style;
        }

        /// <summary>radius 화소 둥근 사각 텍스처. lineWidth > 0이면 테두리만.</summary>
        static Texture2D MakeRounded(int radius, int lineWidth)
        {
            int size = radius * 2 + 2;
            var tex = new Texture2D(size, size, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
            var px = new Color32[size * size];
            float r = radius, ir = Mathf.Max(0f, r - lineWidth);
            // 화소마다 8×8 표본으로 실제로 덮인 넓이를 셈 — 거리 근사는 모서리 호가 두 화소에 반씩 번져 곧은 변보다 얇고 얼룩져 보였음.
            const int N = 8;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                int hit = 0;
                for (int sy = 0; sy < N; sy++)
                for (int sx = 0; sx < N; sx++)
                {
                    float u = x + (sx + 0.5f) / N, v = y + (sy + 0.5f) / N;
                    if (InRounded(u, v, 0f, size, r) && !(lineWidth > 0 && InRounded(u, v, lineWidth, size - lineWidth, ir))) hit++;
                }
                px[y * size + x] = new Color32(255, 255, 255, (byte)(hit * 255 / (N * N)));
            }
            tex.SetPixels32(px);
            tex.Apply();
            return tex;
        }

        /// <summary>점 (u, v)가 [lo, hi] 정사각의 반지름 rad 둥근 사각 안인가(rad 0 = 각진 사각).</summary>
        static bool InRounded(float u, float v, float lo, float hi, float rad)
        {
            if (u < lo || u > hi || v < lo || v > hi) return false;
            float cx = Mathf.Clamp(u, lo + rad, hi - rad), cy = Mathf.Clamp(v, lo + rad, hi - rad);
            return (u - cx) * (u - cx) + (v - cy) * (v - cy) <= rad * rad;
        }

        // ---------------- 원

        static Texture2D Disc
        {
            get
            {
                if (disc != null) return disc;
                const int size = 128;
                disc = new Texture2D(size, size, TextureFormat.ARGB32, true) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Trilinear, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[size * size];
                float r = size * 0.5f;
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float d = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r));
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(r - d) * 255f));
                }
                disc.SetPixels32(px);
                disc.Apply(true);
                return disc;
            }
        }

        public static void Dot(Vector2 center, float radius, Color c)
        {
            if (!Repaint || c.a <= 0f) return;
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(new Rect(center.x - radius, center.y - radius, radius * 2f, radius * 2f)), Disc);
            GUI.color = old;
        }

        public static void Ring(Vector2 center, float radius, float width, Color c)
        {
            if (!Repaint || c.a <= 0f) return;
            int d = Mathf.Max(2, Mathf.RoundToInt(radius * 2f * S));
            int w = Mathf.Max(1, Mathf.RoundToInt(width * S));
            long key = ((long)d << 16) | (uint)w;
            if (!rings.TryGetValue(key, out var tex))
            {
                tex = new Texture2D(d, d, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[d * d];
                float r = d * 0.5f;
                for (int y = 0; y < d; y++)
                for (int x = 0; x < d; x++)
                {
                    float dist = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r));
                    float a = Mathf.Clamp01(r - dist + 0.5f) * Mathf.Clamp01(dist - (r - w) + 0.5f);
                    px[y * d + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
                tex.SetPixels32(px);
                tex.Apply();
                rings[key] = tex;
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(new Rect(center.x - radius, center.y - radius, radius * 2f, radius * 2f)), tex);
            GUI.color = old;
        }

        // ---------------- 선

        static Texture2D lineTex;

        /// <summary>선 띠: 세로 32칸, 위아래 끝 몇 칸만 옅어짐(늘여도 가장자리만 부드러움).</summary>
        static Texture2D LineTexture
        {
            get
            {
                if (lineTex != null) return lineTex;
                const int h = 32;
                lineTex = new Texture2D(1, h, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[h];
                for (int y = 0; y < h; y++)
                {
                    float edge = Mathf.Min(y + 0.5f, h - y - 0.5f); // 끝에서 몇 칸
                    px[y] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(edge / 2.5f) * 255f));
                }
                lineTex.SetPixels32(px);
                lineTex.Apply(false);
                return lineTex;
            }
        }

        public static void Line(Vector2 a, Vector2 b, float width, Color c)
        {
            if (!Repaint || c.a <= 0f) return;
            Vector2 sa = ToScreen(a), sb = ToScreen(b);
            Vector2 d = sb - sa;
            float len = d.magnitude;
            if (len < 0.01f) return;
            float w = Mathf.Max(1f, width * S);
            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
            Matrix4x4 old = GUI.matrix;
            Color oldColor = GUI.color;
            GUI.color = Op(c);
            GUIUtility.RotateAroundPivot(angle, sa);
            // 가장자리를 부드럽게(위아래 1화소 남짓 옅어지는 띠 그림) — 꺾인 선 · 지도 서킷이 계단처럼 보이지 않게.
            // 아주 가는 선(2화소 미만)은 옅어지면 흐려 보여 그대로 흰 사각.
            float soft = w >= 2f ? 1f : 0f;
            GUI.DrawTexture(new Rect(sa.x, sa.y - w * 0.5f - soft, len, w + soft * 2f), w >= 2f ? LineTexture : Texture2D.whiteTexture);
            GUI.matrix = old;
            GUI.color = oldColor;
        }

        /// <summary>굵은 선은 끝을 둥글게(stroke-linecap: round).</summary>
        public static void RoundLine(Vector2 a, Vector2 b, float width, Color c)
        {
            Line(a, b, width, c);
            if (width * S >= 3f)
            {
                Dot(a, width * 0.5f, c);
                Dot(b, width * 0.5f, c);
            }
        }

        /// <summary>
        /// 꺾은선. 완만한 이음새(30° 미만)는 앞뒤 선을 반폭 × tan(꺾인 각/2)만큼 늘여 바깥 쐐기를 메우고, 둥근 점은 크게 꺾인 곳 · 열린 끝에만 —
        /// 점마다 원을 찍으면 원과 선 띠의 굵기가 1화소 남짓 달라 촘촘한 곡선 가장자리가 울퉁불퉁해짐.
        /// </summary>
        public static void Polyline(List<Vector2> pts, float width, Color c, bool closed = false, bool roundJoins = true)
        {
            if (!Repaint || pts == null || pts.Count < 2) return;
            int count = pts.Count, n = closed ? count : count - 1;
            bool round = roundJoins && width * S >= 3f;
            for (int i = 0; i < n; i++)
            {
                Vector2 a = pts[i], b = pts[(i + 1) % count];
                Vector2 d = b - a;
                float len = d.magnitude;
                if (len < 0.01f) continue;
                d /= len;
                Line(a - d * JoinExtend(pts, i, closed, width), b + d * JoinExtend(pts, (i + 1) % count, closed, width), width, c);
            }
            if (!round) return;
            for (int i = 0; i < count; i++)
            {
                bool end = !closed && (i == 0 || i == count - 1);
                if (end || JoinAngle(pts, i, closed) >= 30f) Dot(pts[i], width * 0.5f, c);
            }
        }

        /// <summary>점 i에서 꺾인 각(도, 0 = 곧게). 열린 끝 · 겹친 점이면 0.</summary>
        static float JoinAngle(List<Vector2> pts, int i, bool closed)
        {
            int count = pts.Count;
            if (!closed && (i == 0 || i == count - 1)) return 0f;
            Vector2 a = pts[(i - 1 + count) % count], p = pts[i], b = pts[(i + 1) % count];
            Vector2 d0 = p - a, d1 = b - p;
            if (d0.sqrMagnitude < 0.0001f || d1.sqrMagnitude < 0.0001f) return 0f;
            return Vector2.Angle(d0, d1);
        }

        /// <summary>이음새 i에서 선을 늘일 길이: 완만하면 반폭 × tan(각/2)(바깥 쐐기 메움), 크게 꺾이면 0(둥근 점이 메움).</summary>
        static float JoinExtend(List<Vector2> pts, int i, bool closed, float width)
        {
            // 열린 끝은 그대로, 이음새는 늘 1화소 남짓 겹침 — 맞닿기만 하면 양쪽 가장자리가 반투명으로 겹쳐 틈으로 바탕이 비침
            // (인트로 서킷 선에 흰 눈금처럼 보였음)
            if (!closed && (i == 0 || i == pts.Count - 1)) return 0f;
            float seam = 1f / Mathf.Max(0.5f, S);
            float angle = JoinAngle(pts, i, closed);
            if (angle <= 0f || angle >= 30f) return seam;
            return seam + width * 0.5f * Mathf.Tan(angle * 0.5f * Mathf.Deg2Rad);
        }

        public static void Dashed(Vector2 a, Vector2 b, float width, Color c, float dash, float gap)
        {
            float len = Vector2.Distance(a, b);
            if (len < 0.01f) return;
            Vector2 dir = (b - a) / len;
            for (float t = 0f; t < len; t += dash + gap)
                Line(a + dir * t, a + dir * Mathf.Min(len, t + dash), width, c);
        }

        // ---------------- 그림

        /// <summary>
        /// 그림 그리기. 읽을 수 있는 Texture2D(로고 · 이미지)를 그릴 크기보다 큰 원본에서 줄여 그릴 때는 늘 미리 줄인 선명한 그림(ImageSharp)으로 그림 —
        /// GUI.DrawTexture 의 바로 줄이기는 밉맵 없이 건너뛰어 계단 · 깨짐이 생김. 영상(RenderTexture) · 자르기 · 확대는 그대로.
        /// </summary>
        public static void Image(Rect r, Texture tex, float alpha = 1f, ScaleMode mode = ScaleMode.ScaleToFit)
        {
            if (!Repaint || tex == null) return;
            if (mode == ScaleMode.ScaleToFit && tex is Texture2D t2 && !sharpFailed.Contains(t2.GetInstanceID()))
            {
                Rect sr = ToScreen(r);
                if (sr.width < t2.width || sr.height < t2.height) { ImageSharp(r, t2, alpha); return; }
            }
            ImageRaw(r, tex, alpha, mode);
        }

        static void ImageRaw(Rect r, Texture tex, float alpha, ScaleMode mode)
        {
            Color old = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, alpha * old.a * Opacity);
            GUI.DrawTexture(ToScreen(r), tex, mode, true);
            GUI.color = old;
        }

        /// <summary>미리 줄이지 못하는 그림(읽기 불가 등) — 다시 시도하지 않고 바로 그림.</summary>
        static readonly HashSet<int> sharpFailed = new();

        public static void Image(Rect r, Texture tex, Color tint, ScaleMode mode = ScaleMode.ScaleToFit)
        {
            if (!Repaint || tex == null) return;
            Color old = GUI.color;
            GUI.color = Op(tint);
            GUI.DrawTexture(ToScreen(r), tex, mode, true);
            GUI.color = old;
        }

        // ---------------- 글

        // 글꼴: 게임 UI 글꼴 그대로(보통 = 기본 UI 글꼴 · 굵게 = OpenSans-Bold) — Font Changer 같은 글꼴 모드가 바꾼 글꼴도 따라감.
        // 게임 UI 가 아직 없으면(로딩 중) 기본 IMGUI 글꼴로 그리고 다음 글에서 다시 찾음.
        static Font faceRegular, faceBold;
        static bool facesFound;

        /// <summary>
        /// 브랜드 글꼴(이 PC 에 설치된 글꼴 이름 머리, 예 F1 = "Formula1 Display") — 모드가 시작할 때 한 번. 글꼴 파일은 git 에 올리지 않으므로
        /// 설치된 글꼴을 불러 쓰고(굵게 · 보통 · 넓게 = 이름 + " Bold" · " Regular" · " Wide"), 없으면 게임 글꼴. 한글은 맑은 고딕으로 대신 그림.
        /// </summary>
        public static string BrandFont;
        static Font brandBold, brandRegular, brandWide;
        static bool brandTried;

        static void EnsureBrand()
        {
            if (brandTried || string.IsNullOrEmpty(BrandFont)) return;
            brandTried = true;
            try
            {
                var installed = new HashSet<string>(Font.GetOSInstalledFontNames());
                Font Load(string suffix)
                {
                    string name = BrandFont + suffix;
                    return installed.Contains(name) ? Font.CreateDynamicFontFromOSFont(new[] { name, "Malgun Gothic" }, 16) : null;
                }
                brandBold = Load(" Bold");
                brandRegular = Load(" Regular");
                brandWide = Load(" Wide");
                CSCommonLog.Info(brandBold != null ? $"글꼴: 브랜드 글꼴 {BrandFont} 씀(굵게 · 보통{(brandWide != null ? " · 넓게" : "")})" : $"글꼴: 브랜드 글꼴 {BrandFont} 가 이 PC 에 없어 게임 글꼴로");
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "브랜드 글꼴 찾기");
            }
        }

        static void EnsureFaces()
        {
            if (facesFound) return;
            try
            {
                var ui = UIView.GetAView()?.defaultFont as UIDynamicFont;
                if (ui == null || ui.baseFont == null) return;
                faceRegular = ui.baseFont;
                foreach (var f in Resources.FindObjectsOfTypeAll<UIDynamicFont>())
                    if (f != null && f.baseFont != null && f.name == "OpenSans-Bold") { faceBold = f.baseFont; break; }
                facesFound = true;
                // 찾기 전에 만든 글 모양(기본 글꼴)은 버림
                textStyles.Clear();
                widthCache.Clear();
                CSCommonLog.Info($"글꼴: 게임 UI 글꼴 씀(보통 {faceRegular.name} · 굵게 {(faceBold != null ? faceBold.name : "엔진 굵게")})");
            }
            catch (Exception e)
            {
                facesFound = true;
                CSCommonLog.Exception(e, "게임 글꼴 찾기");
            }
        }

        /// <summary>얼굴: 0 보통 · 1 굵게 · 2 넓게(방송 제목 — 브랜드 넓게, 없으면 굵게) · 3 브랜드 굵게 · 4 브랜드 보통(없으면 게임 굵게 · 보통). 게임 굵은 글꼴이 없으면 엔진 굵게.</summary>
        static GUIStyle TextStyle(int px, bool bold, TextAnchor anchor, bool wrap) => TextStyle(px, bold ? 1 : 0, anchor, wrap);

        static GUIStyle TextStyle(int px, int face, TextAnchor anchor, bool wrap)
        {
            int key = px * 256 + face * 32 + (wrap ? 16 : 0) + (int)anchor;
            if (!textStyles.TryGetValue(key, out var style))
            {
                EnsureFaces();
                EnsureBrand();
                Font brand = face switch { 2 => brandWide ?? brandBold, 3 => brandBold, 4 => brandRegular, _ => null };
                bool regular = face == 0 || face == 4 && brand == null;
                Font font = brand ?? (regular ? faceRegular : faceBold ?? faceRegular);
                style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = px,
                    // 게임 굵은 글꼴 · 브랜드 글꼴은 이미 제 굵기라 엔진 굵게(덧칠)를 또 걸지 않음
                    fontStyle = brand == null && !regular && faceBold == null ? FontStyle.Bold : FontStyle.Normal,
                    alignment = anchor,
                    wordWrap = wrap,
                    // 최소 글자 크기를 쓰는 모드(기상청)는 낮은 칸에서 글자가 위아래로 잘리지 않게 넘쳐 그림
                    clipping = MinText > 0f ? TextClipping.Overflow : TextClipping.Clip,
                    richText = false,
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                };
                if (font != null) style.font = font;
                textStyles[key] = style;
            }
            return style;
        }

        /// <summary>최소 글자 크기(논리 px, 0 = 없음). 모드가 시작할 때 한 번 — 기상청 13(시인성), F1 0. 글 폭 재기도 같은 크기를 씀.</summary>
        public static float MinText;

        /// <summary>글자 크기 배율(1 = 그대로). 모드가 시작할 때 한 번 — 기상청 1.08(시인성), F1 1. 게임 UI 글(CSCommonKit.Scale)도 같이 씀.</summary>
        public static float TextBoost = 1f;

        static int Px(float size) => Mathf.Max(6, Mathf.RoundToInt(Mathf.Max(size * TextBoost, MinText) * S));

        /// <summary>글자를 키운 모드(TextBoost · MinText)는 칸보다 긴 한 줄 글을 칸에 맞게 줄여 그림 — 범례 · 칩 · 요약 줄이 글자가 커져도 넘치지 않게.</summary>
        static bool Fitting => TextBoost > 1f || MinText > 0f;

        /// <summary>칸 폭에 맞춘 글자 크기(화면 px). 원래 크기의 82% 밑으로는 줄이지 않음(그래도 길면 넘쳐 그림).</summary>
        static int FitPx(string text, float size, int face, float width)
        {
            int px = Px(size);
            if (!Fitting || width <= 1f) return px;
            var style = TextStyle(px, face, TextAnchor.MiddleLeft, false);
            content.text = text;
            float w = style.CalcSize(content).x, limit = width * S;
            if (w <= limit) return px;
            return Mathf.Max(Mathf.Max(6, Mathf.RoundToInt(px * 0.82f)), Mathf.FloorToInt(px * limit / w));
        }

        /// <summary>글 한 줄 높이(논리 px): 글자 배율 · 최소 크기를 넣은 실제 크기 기준 — 글이 커지면 줄 칸도 같이 커짐.</summary>
        public static float LineHeight(float size) => Mathf.Ceil(Px(size) / S * 1.3f);

        /// <summary>방송 제목 글(ROUND · 큰 제목) — 게임 굵은 글꼴.</summary>
        public static void TextWide(Rect r, string text, float size, Color c, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f) return;
            if (HasClockColon(text)) { ClockText(r, text, size, c, 2, anchor); return; } // 큰 시계(홈 '21:41')도 콜론을 점 둘로
            var style = TextStyle(FitPx(text, size, 2, r.width), 2, anchor, false);
            style.normal.textColor = Op(c);
            content.text = text;
            style.Draw(ToScreen(r), content, false, false, false, false);
        }

        /// <summary>글꼴(0 보통 · 1 굵게 · 2 넓게) 글 폭(논리 단위).</summary>
        static float FaceWidth(string text, float size, int face)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            var style = TextStyle(Px(size), face, TextAnchor.MiddleLeft, false);
            content.text = text;
            return style.CalcSize(content).x / S;
        }

        /// <summary>브랜드 글꼴 글(방송 타워 · 숫자 · 약어) — bold = 굵게 · 아니면 보통. 브랜드 글꼴이 없으면 게임 글꼴.</summary>
        public static void TextBrand(Rect r, string text, float size, Color c, bool bold = true, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f) return;
            int face = bold ? 3 : 4;
            var style = TextStyle(FitPx(text, size, face, r.width), face, anchor, false);
            style.normal.textColor = Op(c);
            content.text = text;
            style.Draw(ToScreen(r), content, false, false, false, false);
        }

        public static float TextBrandWidth(string text, float size, bool bold = true) => FaceWidth(text, size, bold ? 3 : 4);

        public static float TextWideWidth(string text, float size)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            var style = TextStyle(Px(size), 2, TextAnchor.MiddleLeft, false);
            content.text = text;
            return style.CalcSize(content).x / S;
        }

        public static void Text(Rect r, string text, float size, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleLeft, bool wrap = false)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f) return;
            if (!wrap && HasClockColon(text)) { ClockText(r, text, size, c, bold ? 1 : 0, anchor); return; }
            var style = TextStyle(wrap ? Px(size) : FitPx(text, size, bold ? 1 : 0, r.width), bold, anchor, wrap);
            style.normal.textColor = Op(c);
            content.text = text;
            style.Draw(ToScreen(r), content, false, false, false, false);
        }

        /// <summary>숫자 사이 콜론(시계 '02:24' · '1:31.902')이 있나.</summary>
        static bool HasClockColon(string text)
        {
            for (int i = 1; i + 1 < text.Length; i++)
                if (text[i] == ':' && char.IsDigit(text[i - 1]) && char.IsDigit(text[i + 1])) return true;
            return false;
        }

        /// <summary>
        /// 시계 글: 게임 글꼴에서 콜론 위 점이 빠져 '02.24'처럼 보여(퀄리파잉 시계 · 랩 시계) 숫자 사이 콜론만 점 둘로 직접 그림.
        /// 콜론 자리 폭은 글꼴의 ':' 폭 그대로, 정렬은 원래 글 전체 폭 기준.
        /// </summary>
        static void ClockText(Rect r, string text, float size, Color c, int face, TextAnchor anchor)
        {
            var style = TextStyle(Px(size), face, anchor, false);
            style.normal.textColor = Op(c);
            float colonW = FaceWidth(":", size, face);
            float total = FaceWidth(text, size, face);
            float x = anchor is TextAnchor.UpperCenter or TextAnchor.MiddleCenter or TextAnchor.LowerCenter ? r.center.x - total * 0.5f
                : anchor is TextAnchor.UpperRight or TextAnchor.MiddleRight or TextAnchor.LowerRight ? r.xMax - total : r.x;
            var left = anchor is TextAnchor.UpperLeft or TextAnchor.UpperCenter or TextAnchor.UpperRight ? TextAnchor.UpperLeft
                : anchor is TextAnchor.LowerLeft or TextAnchor.LowerCenter or TextAnchor.LowerRight ? TextAnchor.LowerLeft : TextAnchor.MiddleLeft;
            var seg = TextStyle(Px(size), face, left, false);
            seg.normal.textColor = Op(c);
            float cy = left == TextAnchor.UpperLeft ? r.y + size * 0.62f : left == TextAnchor.LowerLeft ? r.yMax - size * 0.5f : r.center.y + size * 0.04f;
            float dot = Mathf.Max(0.9f, size * 0.085f);
            int start = 0;
            for (int i = 0; i <= text.Length; i++)
            {
                bool colon = i < text.Length && text[i] == ':' && i > 0 && i + 1 < text.Length && char.IsDigit(text[i - 1]) && char.IsDigit(text[i + 1]);
                if (!colon && i < text.Length) continue;
                if (i > start)
                {
                    string part = text.Substring(start, i - start);
                    float w = FaceWidth(part, size, face);
                    content.text = part;
                    seg.Draw(ToScreen(new Rect(x, r.y, w + 2f, r.height)), content, false, false, false, false);
                    x += w;
                }
                if (colon)
                {
                    float mx = x + colonW * 0.5f;
                    Dot(new Vector2(mx, cy - size * 0.2f), dot, c);
                    Dot(new Vector2(mx, cy + size * 0.2f), dot, c);
                    x += colonW;
                }
                start = i + 1;
            }
        }

        /// <summary>
        /// 크기가 움직이는 글: 글꼴 크기는 size 그대로 두고 그림만 scale배(r 가운데 기준).
        /// 글꼴 크기를 매 프레임 바꾸면 크기마다 글자를 새로 요청해 글꼴 텍스처가 매번 다시 지어짐(게임 UI 전체 다시 그리기 · 스택 넘침).
        /// </summary>
        public static void TextScaled(Rect r, string text, float size, float scale, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f || scale <= 0f) return;
            if (Mathf.Abs(scale - 1f) < 0.001f) { Text(r, text, size, c, bold, anchor); return; }
            Matrix4x4 old = GUI.matrix;
            GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), ToScreen(r).center);
            var inner = new Rect(r.center.x - r.width * 0.5f / scale, r.center.y - r.height * 0.5f / scale, r.width / scale, r.height / scale);
            Text(inner, text, size, c, bold, anchor);
            GUI.matrix = old;
        }

        /// <summary>넘치면 끝을 '…'로 줄여 한 줄로(글자를 키운 모드는 '…' 없이 글자를 줄여 맞춤).</summary>
        public static void TextFit(Rect r, string text, float size, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            if (!Repaint || string.IsNullOrEmpty(text)) return;
            Text(r, Fitting ? text : Ellipsis(text, size, bold, r.width), size, c, bold, anchor);
        }

        /// <summary>고정폭 알약 글이 넘칠 때 줄이는 글 크기 한 단계(논리 px).</summary>
        public const float PillFontStep = 1.5f;

        /// <summary>고정폭 알약 글(단추 · 배지 · 고르기 칸 · 탭 칸): 넘치면 먼저 글 크기를 한 단계 줄이고, 그래도 넘칠 때만 '…'.</summary>
        public static void TextPill(Rect r, string text, float size, Color c, bool bold = false, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            if (!Repaint || string.IsNullOrEmpty(text)) return;
            if (!Fitting && TextWidth(text, size, bold) > r.width + FitSlack) size -= PillFontStep;
            TextFit(r, text, size, c, bold, anchor);
        }

        /// <summary>바탕색 위에서 읽히는 글자색: 밝은 바탕 = 어두운 글(Pal.OnLight), 어두운 바탕 = 흰 글(테마와 상관없이 바탕 밝기로).</summary>
        public static Color On(Color bg) => 0.299f * bg.r + 0.587f * bg.g + 0.114f * bg.b > 0.62f ? Pal.OnLight : Color.white;

        /// <summary>캐시 키(글 · 정수 셋): 문자열을 이어 붙이지 않아 찾을 때 쓰레기가 없음. 글은 값으로 비교(참조가 같으면 바로).</summary>
        struct TextKey
        {
            public string T;
            public int A, B, C;
            public TextKey(string t, int a, int b, int c) { T = t; A = a; B = b; C = c; }
        }

        sealed class TextKeyComparer : IEqualityComparer<TextKey>
        {
            public static readonly TextKeyComparer I = new();
            public bool Equals(TextKey x, TextKey y) => x.A == y.A && x.B == y.B && x.C == y.C && string.Equals(x.T, y.T);
            public int GetHashCode(TextKey k) { unchecked { return (((k.T?.GetHashCode() ?? 0) * 397 ^ k.A) * 397 ^ k.B) * 397 ^ k.C; } }
        }

        static readonly Dictionary<TextKey, string> ellipsisCache = new(TextKeyComparer.I);
        // 글 폭 캐시: (글, 화소 크기, 굵기) → CalcSize 화소 폭. 글꼴을 찾으면(EnsureFaces) 비움, 배율은 화소 크기에 들어 있음
        static readonly Dictionary<TextKey, float> widthCache = new(TextKeyComparer.I);

        /// <summary>
        /// 말줄임 여유(논리 px): 칸 폭을 '글 폭 + 여백'으로 잡아도 칸 x 가 소수(비율로 나눈 카드)면 right − x 가 글 폭보다 아주 조금(1e-5) 작아져,
        /// 1080p(배율 1 — 글 폭이 정수)에서 마지막 글자가 '…'로 잘렸음('라이더 순위 변…'). 반 화소 밑 차이는 넘쳐도 안 보임.
        /// </summary>
        public const float FitSlack = 0.5f;

        public static string Ellipsis(string text, float size, bool bold, float width)
        {
            width += FitSlack;
            if (TextWidth(text, size, bold) <= width) return text;
            var key = new TextKey(text, Px(size), bold ? 1 : 0, Mathf.RoundToInt(width));
            if (ellipsisCache.TryGetValue(key, out string cut)) return cut;
            int lo = 0, hi = text.Length;
            while (lo < hi)
            {
                int mid = (lo + hi + 1) / 2;
                if (TextWidth(text.Substring(0, mid) + "…", size, bold) <= width) lo = mid;
                else hi = mid - 1;
            }
            cut = text.Substring(0, lo) + "…";
            if (ellipsisCache.Count > 2000) ellipsisCache.Clear();
            ellipsisCache[key] = cut;
            return cut;
        }

        public static float TextWidth(string text, float size, bool bold = false)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            int px = Px(size);
            var key = new TextKey(text, px, bold ? 1 : 0, 0);
            if (!widthCache.TryGetValue(key, out float w))
            {
                var style = TextStyle(px, bold, TextAnchor.MiddleLeft, false);
                content.text = text;
                w = style.CalcSize(content).x;
                // 굵게 잰 폭 = 굵게 · 보통 중 넓은 쪽: 알약은 굵게 재고 안 고른 칸은 보통으로 그리는데, 한글은 대체 글꼴이라 보통이 더 넓을 수 있음('테스트 모…')
                if (bold) w = Mathf.Max(w, TextStyle(px, false, TextAnchor.MiddleLeft, false).CalcSize(content).x);
                if (widthCache.Count > 4000) widthCache.Clear();
                widthCache[key] = w;
            }
            return w / S;
        }

        public static float TextHeight(string text, float size, float width, bool bold = false)
        {
            if (string.IsNullOrEmpty(text)) return 0f;
            var style = TextStyle(Px(size), bold, TextAnchor.UpperLeft, true);
            content.text = text;
            return style.CalcHeight(content, width * S) / S;
        }

        // ---------------- 입력

        // 누른 버튼: 화면 자리(절대 화소)로 기억 — IMGUI 컨트롤 번호는 앞에 그린 것(타워 줄 · 알림 · 무전 카드 · 지도 이름표)이
        // 프레임마다 늘고 줄면 밀려서, 누를 때와 뗄 때 번호가 달라 클릭이 사라졌음(확인 상자 · 창 버튼이 안 눌림).
        static bool held;
        static int heldKey;
        static int releaseFrame = -1;

        /// <summary>IMGUI 버튼을 누르고 있는 중(지도 도구가 클릭을 가져가지 않게).</summary>
        public static bool Holding => held && Input.GetMouseButton(0);

        static int ButtonKey(Rect r)
        {
            Rect s = ToScreen(r);
            Vector2 p = GUIUtility.GUIToScreenPoint(s.position);
            unchecked
            {
                int h = Mathf.RoundToInt(p.x);
                h = h * 397 ^ Mathf.RoundToInt(p.y);
                h = h * 397 ^ Mathf.RoundToInt(s.width);
                h = h * 397 ^ Mathf.RoundToInt(s.height);
                return h;
            }
        }

        /// <summary>눌렀다 같은 칸에서 떼면 true. 칸의 화면 자리로 누른 버튼을 알아봄(그리는 순서 · 개수와 상관없음).</summary>
        public static bool Button(Rect r)
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && Hover(r))
                    {
                        held = true;
                        heldKey = ButtonKey(r);
                        e.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (held && heldKey == ButtonKey(r)) e.Use();
                    break;
                case EventType.MouseUp:
                    if (held && e.button == 0 && heldKey == ButtonKey(r))
                    {
                        held = false;
                        e.Use();
                        return Hover(r);
                    }
                    break;
            }
            return false;
        }

        /// <summary>마우스를 떼면(어느 버튼에도 안 걸려도) 누른 상태를 풂 — OnGUI 끝에 한 번.</summary>
        public static void EndInput()
        {
            var e = Event.current;
            if (e != null && e.rawType == EventType.MouseUp && e.button == 0) releaseFrame = Time.frameCount;
        }

        /// <summary>누르고 있는 동안 true(꾹 누르기).</summary>
        public static bool Pressed(Rect r) => (held || GUIUtility.hotControl != 0) && Input.GetMouseButton(0) && Hover(r);

        /// <summary>글 입력 칸. 배율 맞춘 GUI.TextField.</summary>
        // 글 입력 칸 모양: (글자 화소, 여백 화소)마다 하나. 글색은 테마가 바뀔 수 있어 부를 때마다 넣음
        static readonly Dictionary<long, GUIStyle> fieldStyles = new();

        public static string Field(Rect r, string value, float size, int maxLength)
        {
            int fpx = Px(size), pad = Px(10);
            long key = ((long)fpx << 32) | (uint)pad;
            if (!fieldStyles.TryGetValue(key, out var style))
            {
                style = new GUIStyle(GUI.skin.textField) { fontSize = fpx, alignment = TextAnchor.MiddleLeft, padding = new RectOffset(pad, pad, 0, 0) };
                style.normal.background = null;
                style.focused.background = null;
                style.hover.background = null;
                fieldStyles[key] = style;
            }
            style.normal.textColor = style.focused.textColor = style.hover.textColor = Pal.Txt;
            return GUI.TextField(ToScreen(r), value ?? "", maxLength, style);
        }

        // ---------------- SVG 경로(탭 아이콘 · 설계 그림) — M L H V C S Q T A Z 지원, 결과는 꺾은선 목록

        static readonly Dictionary<string, List<List<Vector2>>> pathCache = new();

        static void Cubic(List<Vector2> into, Vector2 p, Vector2 c1, Vector2 c2, Vector2 e)
        {
            if (into == null) return;
            for (int k = 1; k <= 12; k++)
            {
                float t = k / 12f, u = 1f - t;
                into.Add(u * u * u * p + 3f * u * u * t * c1 + 3f * u * t * t * c2 + t * t * t * e);
            }
        }

        static void Quad(List<Vector2> into, Vector2 p, Vector2 c1, Vector2 e)
        {
            if (into == null) return;
            for (int k = 1; k <= 10; k++)
            {
                float t = k / 10f, u = 1f - t;
                into.Add(u * u * p + 2f * u * t * c1 + t * t * e);
            }
        }

        public static List<List<Vector2>> ParsePath(string d)
        {
            if (pathCache.TryGetValue(d, out var cached)) return cached;
            var result = new List<List<Vector2>>();
            List<Vector2> cur = null;
            Vector2 p = Vector2.zero, start = Vector2.zero;
            // S · T가 반사할 앞 곡선의 마지막 제어점(앞 명령이 C/S · Q/T일 때만)
            Vector2 lastCubic = Vector2.zero, lastQuad = Vector2.zero;
            char prev = ' ';
            int i = 0;
            char cmd = 'M';
            while (i < d.Length)
            {
                SkipSpace(d, ref i);
                if (i >= d.Length) break;
                if (char.IsLetter(d[i])) cmd = d[i++];
                bool rel = char.IsLower(cmd);
                char upper = char.ToUpperInvariant(cmd);
                char last = prev;
                prev = upper;
                switch (upper)
                {
                    case 'M':
                    {
                        var q = new Vector2(Num(d, ref i), Num(d, ref i));
                        p = rel ? p + q : q;
                        start = p;
                        cur = new List<Vector2> { p };
                        result.Add(cur);
                        cmd = rel ? 'l' : 'L';
                        break;
                    }
                    case 'L':
                    {
                        var q = new Vector2(Num(d, ref i), Num(d, ref i));
                        p = rel ? p + q : q;
                        cur?.Add(p);
                        break;
                    }
                    case 'H':
                    {
                        float x = Num(d, ref i);
                        p = new Vector2(rel ? p.x + x : x, p.y);
                        cur?.Add(p);
                        break;
                    }
                    case 'V':
                    {
                        float y = Num(d, ref i);
                        p = new Vector2(p.x, rel ? p.y + y : y);
                        cur?.Add(p);
                        break;
                    }
                    case 'C':
                    {
                        Vector2 c1 = new(Num(d, ref i), Num(d, ref i)), c2 = new(Num(d, ref i), Num(d, ref i)), e = new(Num(d, ref i), Num(d, ref i));
                        if (rel) { c1 += p; c2 += p; e += p; }
                        Cubic(cur, p, c1, c2, e);
                        lastCubic = c2;
                        p = e;
                        break;
                    }
                    case 'S':
                    {
                        // 첫 제어점 = 앞 C/S의 둘째 제어점을 지금 점에 대해 반사(앞이 곡선이 아니면 지금 점)
                        Vector2 c2 = new(Num(d, ref i), Num(d, ref i)), e = new(Num(d, ref i), Num(d, ref i));
                        if (rel) { c2 += p; e += p; }
                        Vector2 c1 = last is 'C' or 'S' ? 2f * p - lastCubic : p;
                        Cubic(cur, p, c1, c2, e);
                        lastCubic = c2;
                        p = e;
                        break;
                    }
                    case 'Q':
                    {
                        Vector2 c1 = new(Num(d, ref i), Num(d, ref i)), e = new(Num(d, ref i), Num(d, ref i));
                        if (rel) { c1 += p; e += p; }
                        Quad(cur, p, c1, e);
                        lastQuad = c1;
                        p = e;
                        break;
                    }
                    case 'T':
                    {
                        var e = new Vector2(Num(d, ref i), Num(d, ref i));
                        if (rel) e += p;
                        Vector2 c1 = last is 'Q' or 'T' ? 2f * p - lastQuad : p;
                        Quad(cur, p, c1, e);
                        lastQuad = c1;
                        p = e;
                        break;
                    }
                    case 'A':
                    {
                        float rx = Num(d, ref i), ry = Num(d, ref i), rot = Num(d, ref i);
                        bool large = Num(d, ref i) != 0f, sweep = Num(d, ref i) != 0f;
                        var e = new Vector2(Num(d, ref i), Num(d, ref i));
                        if (rel) e += p;
                        Arc(cur, p, e, rx, ry, rot, large, sweep);
                        p = e;
                        break;
                    }
                    case 'Z':
                        p = start;
                        cur?.Add(start);
                        break;
                    default:
                        i++;
                        break;
                }
            }
            pathCache[d] = result;
            return result;
        }

        static void Arc(List<Vector2> into, Vector2 p0, Vector2 p1, float rx, float ry, float rotDeg, bool large, bool sweep)
        {
            if (into == null) return;
            if (rx == 0f || ry == 0f) { into.Add(p1); return; }
            float phi = rotDeg * Mathf.Deg2Rad, cos = Mathf.Cos(phi), sin = Mathf.Sin(phi);
            Vector2 h = (p0 - p1) * 0.5f;
            float x1 = cos * h.x + sin * h.y, y1 = -sin * h.x + cos * h.y;
            rx = Mathf.Abs(rx); ry = Mathf.Abs(ry);
            float lambda = x1 * x1 / (rx * rx) + y1 * y1 / (ry * ry);
            if (lambda > 1f) { float s = Mathf.Sqrt(lambda); rx *= s; ry *= s; }
            float num = rx * rx * ry * ry - rx * rx * y1 * y1 - ry * ry * x1 * x1;
            float den = rx * rx * y1 * y1 + ry * ry * x1 * x1;
            float coef = (large == sweep ? -1f : 1f) * Mathf.Sqrt(Mathf.Max(0f, num / den));
            float cx1 = coef * rx * y1 / ry, cy1 = -coef * ry * x1 / rx;
            Vector2 mid = (p0 + p1) * 0.5f;
            float cx = cos * cx1 - sin * cy1 + mid.x, cy = sin * cx1 + cos * cy1 + mid.y;
            float a1 = Mathf.Atan2((y1 - cy1) / ry, (x1 - cx1) / rx);
            float a2 = Mathf.Atan2((-y1 - cy1) / ry, (-x1 - cx1) / rx);
            float da = a2 - a1;
            if (sweep && da < 0f) da += 2f * Mathf.PI;
            if (!sweep && da > 0f) da -= 2f * Mathf.PI;
            int steps = Mathf.Max(6, Mathf.CeilToInt(Mathf.Abs(da) / (Mathf.PI / 12f)));
            for (int k = 1; k <= steps; k++)
            {
                float a = a1 + da * k / steps;
                float ex = rx * Mathf.Cos(a), ey = ry * Mathf.Sin(a);
                into.Add(new Vector2(cos * ex - sin * ey + cx, sin * ex + cos * ey + cy));
            }
        }

        static void SkipSpace(string d, ref int i)
        {
            while (i < d.Length && (d[i] == ' ' || d[i] == ',' || d[i] == '\n' || d[i] == '\t')) i++;
        }

        static float Num(string d, ref int i)
        {
            SkipSpace(d, ref i);
            int start = i;
            if (i < d.Length && (d[i] == '-' || d[i] == '+')) i++;
            bool dot = false;
            while (i < d.Length && (char.IsDigit(d[i]) || (d[i] == '.' && !dot)))
            {
                if (d[i] == '.') dot = true;
                i++;
            }
            if (i < d.Length && (d[i] == 'e' || d[i] == 'E'))
            {
                i++;
                if (i < d.Length && (d[i] == '-' || d[i] == '+')) i++;
                while (i < d.Length && char.IsDigit(d[i])) i++;
            }
            if (start == i) { i++; return 0f; }
            return float.Parse(d.Substring(start, i - start), NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        /// <summary>SVG 경로를 origin에서 scale 배로 선 그리기.</summary>
        public static void Path(string d, Vector2 origin, float scale, float width, Color c)
        {
            if (!Repaint) return;
            foreach (var poly in ParsePath(d))
            {
                for (int k = 0; k + 1 < poly.Count; k++)
                    Line(origin + poly[k] * scale, origin + poly[k + 1] * scale, width, c);
                if (width * S >= 2.5f)
                    foreach (var q in poly) Dot(origin + q * scale, width * 0.5f, c);
            }
        }

        // ---------------- 선명한 아이콘 · 그림

        // 키 = (경로 글, 화소, 선 굵기 · viewBox 를 1/10000 단위 정수로) — 예전 문자열 이어 붙인 키와 같은 구분, 찾을 때 쓰레기 없음
        static readonly Dictionary<TextKey, Texture2D> iconCache = new(TextKeyComparer.I);

        /// <summary>
        /// 아웃라인 아이콘(SVG 경로, viewBox 0 ~ viewBox 정사각): 화면 화소 크기에 맞춰 선까지 거리로 한 번 구운 텍스처(둥근 끝 · 둥근 이음)를
        /// 정수 화소 자리에 그대로 찍음 — 짧은 선 띠 · 점을 이어 그리면 작은 크기에서 뭉개졌음. stroke = viewBox 단위 선 굵기.
        /// </summary>
        public static void Icon(string d, Rect r, Color c, float viewBox = 24f, float stroke = 1.75f)
        {
            if (!Repaint || c.a <= 0f || string.IsNullOrEmpty(d)) return;
            Rect sr = ToScreen(r);
            int px = Mathf.Max(4, Mathf.RoundToInt(sr.width));
            var key = new TextKey(d, px, Mathf.RoundToInt(stroke * 10000f), Mathf.RoundToInt(viewBox * 10000f));
            if (!iconCache.TryGetValue(key, out var tex) || tex == null)
            {
                tex = BakeIcon(d, px, viewBox, stroke);
                iconCache[key] = tex;
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(new Rect(Mathf.Round(sr.x), Mathf.Round(sr.y), px, px), tex);
            GUI.color = old;
        }

        static Texture2D BakeIcon(string d, int px, float viewBox, float stroke)
        {
            float k = px / viewBox, half = stroke * k * 0.5f;
            var segs = new List<Vector4>();
            foreach (var poly in ParsePath(d))
            {
                if (poly.Count == 1) segs.Add(new Vector4(poly[0].x * k, poly[0].y * k, poly[0].x * k, poly[0].y * k));
                for (int i = 0; i + 1 < poly.Count; i++)
                    segs.Add(new Vector4(poly[i].x * k, poly[i].y * k, poly[i + 1].x * k, poly[i + 1].y * k));
            }
            var tex = new Texture2D(px, px, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point, hideFlags = HideFlags.HideAndDontSave };
            var pxs = new Color32[px * px];
            for (int y = 0; y < px; y++)
            for (int x = 0; x < px; x++)
            {
                // 텍스처 0행 = 아래 → 아이콘 y는 위에서 아래로
                float qx = x + 0.5f, qy = px - y - 0.5f, best = float.MaxValue;
                foreach (var s in segs)
                {
                    float ax = s.x, ay = s.y, dx = s.z - ax, dy = s.w - ay;
                    float len2 = dx * dx + dy * dy;
                    float t = len2 > 1e-6f ? Mathf.Clamp01(((qx - ax) * dx + (qy - ay) * dy) / len2) : 0f;
                    float ex = ax + dx * t - qx, ey = ay + dy * t - qy;
                    float dist2 = ex * ex + ey * ey;
                    if (dist2 < best) best = dist2;
                }
                float a = Mathf.Clamp01(half - Mathf.Sqrt(best) + 0.5f);
                pxs[y * px + x] = new Color32(255, 255, 255, (byte)(a * 255f));
            }
            tex.SetPixels32(pxs);
            tex.Apply(false);
            return tex;
        }

        static readonly Dictionary<long, Texture2D> sharpCache = new();

        /// <summary>
        /// 그림을 화면 화소 크기로 미리 줄여(넓이 평균, 투명도 곱해 섞음) 정수 화소 자리에 찍음 — 큰 로고를 작게 그릴 때 흐려지지 않게.
        /// 원본을 못 읽으면(읽기 막힌 텍스처) 보통 Image.
        /// </summary>
        public static void ImageSharp(Rect r, Texture2D src, float alpha = 1f)
        {
            if (!Repaint || src == null) return;
            Rect sr = ToScreen(r);
            // 비율 유지(ScaleToFit)
            float aspect = src.width / (float)Mathf.Max(1, src.height);
            float w = sr.width, h = sr.height;
            if (w / h > aspect) w = h * aspect; else h = w / aspect;
            int pw = Mathf.Max(1, Mathf.RoundToInt(w)), ph = Mathf.Max(1, Mathf.RoundToInt(h));
            long key = ((long)src.GetInstanceID() << 24) ^ ((long)pw << 12) ^ ph;
            if (!sharpCache.TryGetValue(key, out var tex) || tex == null)
            {
                tex = Downscale(src, pw, ph);
                if (tex == null) { sharpFailed.Add(src.GetInstanceID()); ImageRaw(r, src, alpha, ScaleMode.ScaleToFit); return; }
                // 크기가 계속 바뀌는 그림(움직임)이 줄인 그림을 끝없이 쌓지 않게
                if (sharpCache.Count > 200)
                {
                    foreach (var t in sharpCache.Values)
                        if (t != null) UnityEngine.Object.Destroy(t);
                    sharpCache.Clear();
                }
                sharpCache[key] = tex;
            }
            Color old = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, alpha * old.a * Opacity);
            GUI.DrawTexture(new Rect(Mathf.Round(sr.x + (sr.width - pw) * 0.5f), Mathf.Round(sr.y + (sr.height - ph) * 0.5f), pw, ph), tex);
            GUI.color = old;
        }

        static Texture2D Downscale(Texture2D src, int w, int h)
        {
            Color32[] from;
            try { from = src.GetPixels32(); }
            catch { return null; }
            int sw = src.width, sh = src.height;
            var to = new Color32[w * h];
            for (int y = 0; y < h; y++)
            {
                int y0 = y * sh / h, y1 = Mathf.Max(y0 + 1, (y + 1) * sh / h);
                for (int x = 0; x < w; x++)
                {
                    int x0 = x * sw / w, x1 = Mathf.Max(x0 + 1, (x + 1) * sw / w);
                    float r = 0f, g = 0f, b = 0f, a = 0f;
                    for (int yy = y0; yy < y1; yy++)
                    for (int xx = x0; xx < x1; xx++)
                    {
                        var p = from[yy * sw + xx];
                        float pa = p.a / 255f;
                        r += p.r * pa; g += p.g * pa; b += p.b * pa; a += pa;
                    }
                    float n = (x1 - x0) * (y1 - y0);
                    to[y * w + x] = a > 0f
                        ? new Color32((byte)(r / a), (byte)(g / a), (byte)(b / a), (byte)(a / n * 255f))
                        : new Color32(0, 0, 0, 0);
                }
            }
            var tex = new Texture2D(w, h, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point, hideFlags = HideFlags.HideAndDontSave };
            tex.SetPixels32(to);
            tex.Apply(false);
            return tex;
        }

        // ---------------- 호 · 빛 번짐(계기판)

        static readonly Dictionary<long, BakedArc> arcCache = new();
        static Texture2D glowTex;

        /// <summary>
        /// 호 띠(고리 한 조각): center 중심, radius = 띠 가운데 반지름, width = 굵기(논리 단위). 각도는 도 — 0 = 오른쪽, 시계 방향 +(화면 y가 아래라 90 = 아래).
        /// feather > 0이면 가장자리가 그만큼(논리 단위) 부드럽게 퍼짐(빛 번짐), 0이면 1화소 안티앨리어싱만.
        /// 화면 화소 크기로 한 번 구운 텍스처(호를 감싼 사각만)를 정수 화소 자리에 찍음 — 각도는 0.5° 단위로 맞춰 움직이는 호(값 막대)도 텍스처가 끝없이 늘지 않음.
        /// </summary>
        public static void Arc(Vector2 center, float radius, float width, float fromDeg, float toDeg, Color c, float feather = 0f)
        {
            if (!Repaint || c.a <= 0f || width <= 0f) return;
            if (toDeg < fromDeg) { float t = fromDeg; fromDeg = toDeg; toDeg = t; }
            int a0 = Mathf.RoundToInt(fromDeg * 2f), a1 = Mathf.RoundToInt(toDeg * 2f);
            if (a1 - a0 > 720) a1 = a0 + 720;
            if (a1 <= a0) return;
            // 각도는 −720 ~ 720도 안으로(열쇠 칸 12비트)
            int shift = Mathf.FloorToInt(a0 / 720f) * 720;
            a0 -= shift;
            a1 -= shift;
            int rq = Mathf.Clamp(Mathf.RoundToInt(radius * S * 4f), 1, 16383);
            int wq = Mathf.Clamp(Mathf.RoundToInt(width * S * 4f), 1, 4095);
            int fq = Mathf.Clamp(Mathf.RoundToInt(feather * S * 2f), 0, 511);
            long key = ((long)rq << 42) | ((long)wq << 30) | ((long)fq << 21) | ((long)a0 << 11) | (uint)a1;
            if (!arcCache.TryGetValue(key, out var baked) || baked.Tex == null)
            {
                if (arcCache.Count > 400)
                {
                    foreach (var t in arcCache.Values)
                        if (t.Tex != null) UnityEngine.Object.Destroy(t.Tex);
                    arcCache.Clear();
                }
                var tex = BakeArc(rq / 4f, wq / 4f, fq / 2f, a0 * 0.5f, a1 * 0.5f, out int ox, out int oy);
                baked = new BakedArc { Tex = tex, Ox = ox, Oy = oy };
                arcCache[key] = baked;
            }
            Vector2 sc = ToScreen(center);
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(new Rect(Mathf.Round(sc.x) - baked.Ox, Mathf.Round(sc.y) - baked.Oy, baked.Tex.width, baked.Tex.height), baked.Tex);
            GUI.color = old;
        }

        /// <summary>구운 호: 텍스처와 그 안 원 중심 자리(왼쪽 위에서 화소).</summary>
        struct BakedArc
        {
            public Texture2D Tex;
            public int Ox, Oy;
        }

        /// <summary>호 텍스처(화소 단위). ox · oy = 텍스처 왼쪽 위에서 원 중심까지(정수 화소 모서리).</summary>
        static Texture2D BakeArc(float r, float w, float f, float from, float to, out int ox, out int oy)
        {
            float half = w * 0.5f, inner = Mathf.Max(0f, r - half), outer = r + half;
            float pad = f + 2f;
            // 감싼 사각: 두 끝의 안 · 밖 점 + 범위 안 축 방향(0 · 90 · 180 · 270)의 바깥 점
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            void Take(float deg, float rad)
            {
                float x = Mathf.Cos(deg * Mathf.Deg2Rad) * rad, y = Mathf.Sin(deg * Mathf.Deg2Rad) * rad;
                minX = Mathf.Min(minX, x); maxX = Mathf.Max(maxX, x);
                minY = Mathf.Min(minY, y); maxY = Mathf.Max(maxY, y);
            }
            Take(from, inner); Take(from, outer); Take(to, inner); Take(to, outer);
            for (float axis = Mathf.Ceil(from / 90f) * 90f; axis <= to; axis += 90f) Take(axis, outer);
            ox = Mathf.CeilToInt(-minX + pad);
            oy = Mathf.CeilToInt(-minY + pad);
            int tw = Mathf.Max(1, ox + Mathf.CeilToInt(maxX + pad)), th = Mathf.Max(1, oy + Mathf.CeilToInt(maxY + pad));
            var tex = new Texture2D(tw, th, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point, hideFlags = HideFlags.HideAndDontSave };
            var px = new Color32[tw * th];
            float mid = (from + to) * 0.5f, halfSpan = (to - from) * 0.5f;
            bool full = to - from >= 359.9f;
            for (int y = 0; y < th; y++)
            for (int x = 0; x < tw; x++)
            {
                // 텍스처 0행 = 아래 → 화면 y는 위에서 아래로
                float dx = x + 0.5f - ox, dy = th - y - 0.5f - oy;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                float d = Mathf.Abs(dist - r) - half;
                if (!full)
                {
                    float ex = Mathf.Abs(Mathf.DeltaAngle(mid, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg)) - halfSpan;
                    float da = ex <= 0f ? ex * Mathf.Deg2Rad * dist : ex < 90f ? dist * Mathf.Sin(ex * Mathf.Deg2Rad) : dist;
                    d = Mathf.Max(d, da);
                }
                float a;
                if (f > 0.01f)
                {
                    a = Mathf.Clamp01((f - d) / (2f * f));
                    a = a * a * (3f - 2f * a);
                }
                else a = Mathf.Clamp01(0.5f - d);
                px[y * tw + x] = new Color32(255, 255, 255, (byte)(a * 255f));
            }
            tex.SetPixels32(px);
            tex.Apply(false);
            return tex;
        }

        // ---------------- 구글 머티리얼 서클 인디케이터(Spinner)

        static Texture2D spinnerTex;
        static Color32[] spinnerPx;
        static int spinnerSize;
        static int spinnerLastFrame = -1;
        static float spinnerLastR = -1f, spinnerLastW = -1f;

        /// <summary>
        /// 구글 머티리얼 서클 인디케이터(Google Circular Progress Indicator):
        /// 1.333초 주기로 호의 머리(head)가 먼저 270° 치고 나가며 늘어나고(16° → 286°), 뒤이어 꼬리(tail)가 따라붙으며 줄어듦(286° → 16°).
        /// 동시에 전체 고리가 끊임없이 회전하며, 양 끝은 둥근 마감(stroke-linecap: round).
        /// 한 장의 재사용 텍스처를 써서 매 프레임 새 Texture2D · GC 할당이 없고, GUI.matrix 회전을 쓰지 않아 BeginClip 안에서도 잘리지 않음.
        /// </summary>
        public static void Spinner(Vector2 center, float radius, float width, Color c)
        {
            if (!Repaint || c.a <= 0f || radius <= 0f || width <= 0f) return;

            const float Cycle = 1.333f;
            float t = Time.unscaledTime;
            float tCycle = t / Cycle;
            int cycleIdx = Mathf.FloorToInt(tCycle);
            float u = tCycle - cycleIdx;

            static float EaseInOut(float x)
            {
                x = Mathf.Clamp01(x);
                return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) * 0.5f;
            }

            float head = u < 0.5f ? EaseInOut(u * 2f) * 270f : 270f;
            float tail = u < 0.5f ? 0f : EaseInOut((u - 0.5f) * 2f) * 270f;
            float baseDeg = -90f + cycleIdx * 270f + t * 156f;
            float from = baseDeg + tail;
            float to = baseDeg + head + 16f;

            float rPx = Mathf.Max(2f, radius * S);
            float half = Mathf.Max(0.75f, width * S * 0.5f);
            int d = Mathf.Clamp(Mathf.CeilToInt((rPx + half + 2f) * 2f), 8, 256);

            if (spinnerTex == null || spinnerSize != d)
            {
                if (spinnerTex != null) UnityEngine.Object.Destroy(spinnerTex);
                spinnerSize = d;
                spinnerTex = new Texture2D(d, d, TextureFormat.ARGB32, false)
                {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear,
                    hideFlags = HideFlags.HideAndDontSave
                };
                spinnerPx = new Color32[d * d];
                spinnerLastFrame = -1;
            }

            if (spinnerLastFrame != Time.frameCount || Mathf.Abs(spinnerLastR - rPx) > 0.1f || Mathf.Abs(spinnerLastW - half) > 0.1f)
            {
                spinnerLastFrame = Time.frameCount;
                spinnerLastR = rPx;
                spinnerLastW = half;

                float mid = (from + to) * 0.5f;
                float halfSpan = (to - from) * 0.5f;
                float rad0 = from * Mathf.Deg2Rad, rad1 = to * Mathf.Deg2Rad;
                float x0 = Mathf.Cos(rad0) * rPx, y0 = Mathf.Sin(rad0) * rPx;
                float x1 = Mathf.Cos(rad1) * rPx, y1 = Mathf.Sin(rad1) * rPx;
                float cMid = d * 0.5f;

                for (int y = 0; y < d; y++)
                {
                    float dy = d - y - 0.5f - cMid;
                    int row = y * d;
                    for (int x = 0; x < d; x++)
                    {
                        float dx = x + 0.5f - cMid;
                        float dist = Mathf.Sqrt(dx * dx + dy * dy);
                        float ang = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
                        float distToArc;
                        if (Mathf.Abs(Mathf.DeltaAngle(mid, ang)) <= halfSpan)
                        {
                            distToArc = Mathf.Abs(dist - rPx);
                        }
                        else
                        {
                            float d0 = (dx - x0) * (dx - x0) + (dy - y0) * (dy - y0);
                            float d1 = (dx - x1) * (dx - x1) + (dy - y1) * (dy - y1);
                            distToArc = Mathf.Sqrt(d0 < d1 ? d0 : d1);
                        }
                        float a = Mathf.Clamp01(half + 0.5f - distToArc);
                        spinnerPx[row + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                    }
                }
                spinnerTex.SetPixels32(spinnerPx);
                spinnerTex.Apply(false);
            }

            Vector2 sc = ToScreen(center);
            float halfD = d * 0.5f;
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(new Rect(Mathf.Round(sc.x - halfD), Mathf.Round(sc.y - halfD), d, d), spinnerTex);
            GUI.color = old;
        }

        /// <summary>부드러운 빛 번짐(가운데 진하고 가장자리로 0 — 타원으로 늘려 찍음). 계기판 뒤 후광 · 페달 빛.</summary>
        public static void Glow(Rect r, Color c)
        {
            if (!Repaint || c.a <= 0f) return;
            if (glowTex == null)
            {
                const int size = 128;
                glowTex = new Texture2D(size, size, TextureFormat.ARGB32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.HideAndDontSave };
                var px = new Color32[size * size];
                float h = size * 0.5f;
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float d = Mathf.Sqrt((x + 0.5f - h) * (x + 0.5f - h) + (y + 0.5f - h) * (y + 0.5f - h)) / h;
                    float a = Mathf.Clamp01(1f - d);
                    a = a * a * (3f - 2f * a);
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
                glowTex.SetPixels32(px);
                glowTex.Apply(false);
            }
            Color old = GUI.color;
            GUI.color = Op(c);
            GUI.DrawTexture(ToScreen(r), glowTex);
            GUI.color = old;
        }

        /// <summary>r 가운데를 축으로 deg도 돌린 글(시계 방향 +). 글꼴 크기는 그대로라 글자 텍스처를 다시 짓지 않음.</summary>
        public static void TextRotated(Rect r, string text, float size, Color c, float deg, bool bold = false, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            if (!Repaint || string.IsNullOrEmpty(text) || c.a <= 0f) return;
            if (Mathf.Abs(deg) < 0.01f) { Text(r, text, size, c, bold, anchor); return; }
            Matrix4x4 old = GUI.matrix;
            GUIUtility.RotateAroundPivot(deg, ToScreen(r).center);
            Text(r, text, size, c, bold, anchor);
            GUI.matrix = old;
        }

        /// <summary>설계 색 글("#RRGGBB" · "#RRGGBBAA").</summary>
        public static Color Hex(string hex)
        {
            hex = hex.TrimStart('#');
            uint v = Convert.ToUInt32(hex, 16);
            if (hex.Length == 8)
                return new Color32((byte)(v >> 24), (byte)(v >> 16), (byte)(v >> 8), (byte)v);
            return new Color32((byte)(v >> 16), (byte)(v >> 8), (byte)v, 255);
        }

        public static Color Alpha(Color c, float a) => new(c.r, c.g, c.b, c.a * a);

        public static Color Shade(Color c, float f) => new(c.r * f, c.g * f, c.b * f, c.a);

        /// <summary>화면 픽셀 좌표(Camera.WorldToScreenPoint) 자리에 게임 UI(패널 · 창 등)가 덮여 있는지.</summary>
        public static bool IsOccluded(Vector3 screenPoint)
        {
            var view = UIView.GetAView();
            if (view == null || view.uiCamera == null) return false;
            Vector2 res = view.GetScreenResolution();
            Ray ray = view.uiCamera.ScreenPointToRay(new Vector3(screenPoint.x, screenPoint.y, 0f));
            using var hits = view.RaycastAll(ray);
            for (int i = 0; i < hits.Count; i++)
            {
                var c = hits[i].component;
                if (c == null || !c.isVisible || c.opacity <= 0.01f) continue;
                if (c.size.x >= res.x - 10f && c.size.y >= res.y - 10f)
                {
                    if (!UIView.HasModalInput()) continue;
                }
                return true;
            }
            return false;
        }
    }
}
