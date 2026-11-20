using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Phone — 휴대폰(아이폰 프로 맥스 440×956 · 다이나믹 아일랜드 · 리퀴드 글라스 면). 크기는 고정(CSCommonDesign.PhoneW · PhoneH).
    /// Frame 이 틀(티타늄 테두리 · 화면 바탕 · 상태 줄 · 아일랜드 · 홈 막대)을 그리고 안쪽 쓸 자리를 돌려줌. 그 안은 Glass · GlassButton 으로 채움.
    /// 게임 화면은 흐려 비출 수 없어, 반투명 면 + 위 반짝임 + 가는 테두리로 유리 느낌을 냄.
    /// </summary>
    public static class Phone
    {
        /// <summary>화면이 낮으면 틀 전체를 줄이는 배율(1 = 440×956 그대로). 크기 규격은 그대로이고 보이는 크기만 줄어듦.</summary>
        public static float Scale => Mathf.Clamp((Draw.Height - CSCommonDesign.PhoneMargin * 2f) / CSCommonDesign.PhoneH, 0.4f, 1f);

        /// <summary>화면 오른쪽 가운데에 놓은 틀 자리. 크기는 논리 440×956(그리는 좌표) — 낮은 화면에서는 BeginScale 로 줄여 그림.</summary>
        public static Rect Place()
        {
            float m = CSCommonDesign.PhoneMargin, k = Scale;
            return new Rect(Mathf.Max(m, Draw.Width - CSCommonDesign.PhoneW * k - m), Mathf.Max(m, (Draw.Height - CSCommonDesign.PhoneH * k) * 0.5f), CSCommonDesign.PhoneW, CSCommonDesign.PhoneH);
        }

        /// <summary>화면에 보이는 틀 자리(배율 적용 — 클릭 막기 · 마우스 검사용).</summary>
        public static Rect Visual(Rect frame) => new(frame.x, frame.y, frame.width * Scale, frame.height * Scale);

        static Matrix4x4 savedMatrix;

        /// <summary>틀을 그리기 전에: 낮은 화면이면 틀 왼쪽 위를 기준으로 GUI 를 줄임. 그린 뒤 EndScale.</summary>
        public static void BeginScale(Rect frame)
        {
            savedMatrix = GUI.matrix;
            float k = Scale;
            if (k < 0.999f) GUIUtility.ScaleAroundPivot(new Vector2(k, k), new Vector2(frame.x, frame.y) * Draw.S);
        }

        public static void EndScale() => GUI.matrix = savedMatrix;

        /// <summary>틀 그리기. time = 왼쪽 상태 글(시각) · status = 오른쪽 상태 글 · island = 아일랜드에 펼칠 한 줄(없으면 알약만). 안쪽 쓸 자리를 돌려줌.</summary>
        public static Rect Frame(Rect r, string time, string status, string island = null)
        {
            var screen = new Rect(r.x + CSCommonDesign.PhoneBezel, r.y + CSCommonDesign.PhoneBezel, r.width - CSCommonDesign.PhoneBezel * 2f, r.height - CSCommonDesign.PhoneBezel * 2f);
            if (Draw.Repaint)
            {
                // 티타늄 테두리(바깥 선 · 안쪽 검은 틈) + 화면 바탕(iOS 에는 그림자가 없음)
                Draw.Round(r, Pal.PhoneBezel, CSCommonDesign.PhoneRadius);
                Draw.RoundOutline(r, Pal.PhoneBezelEdge, CSCommonDesign.PhoneRadius, 1.5f);
                Draw.Round(new Rect(r.x + 3f, r.y + 3f, r.width - 6f, r.height - 6f), Color.black, CSCommonDesign.PhoneRadius - 3f);
                Draw.Round(screen, Pal.PhoneWall, CSCommonDesign.ScreenRadius);
                // 바탕 무늬: 안쪽에 번지는 색 덩어리 셋(유리가 비출 것)
                // 상태 줄 · 홈 막대
                float sy = screen.y + CSCommonDesign.IslandTop, sh = CSCommonDesign.IslandH;
                Draw.Text(new Rect(screen.x + 34f, sy, 90f, sh), time, 16f, Pal.PhoneInk, true, TextAnchor.MiddleLeft);
                Draw.Text(new Rect(screen.xMax - 34f - 140f, sy, 140f, sh), status, 14f, Pal.PhoneInk, true, TextAnchor.MiddleRight);
                Draw.Round(new Rect(screen.center.x - CSCommonDesign.HomeBarW * 0.5f, screen.yMax - 14f, CSCommonDesign.HomeBarW, 5f), Draw.Alpha(Pal.PhoneInk, 0.85f), 2.5f);
                Island(screen, island);
            }
            return new Rect(screen.x + CSCommonDesign.GlassPad, screen.y + CSCommonDesign.StatusH, screen.width - CSCommonDesign.GlassPad * 2f, screen.height - CSCommonDesign.StatusH - CSCommonDesign.HomeH);
        }

        /// <summary>색이 번지는 덩어리(겹친 원 여럿).</summary>
        static void Glow(Vector2 c, float radius, Color color)
        {
            for (int i = 0; i < 7; i++)
                Draw.Dot(c, radius * (1f - i * 0.12f), Draw.Alpha(color, color.a * 0.35f));
        }

        /// <summary>다이나믹 아일랜드: 검은 알약, 글이 있으면 옆으로 펼쳐 글을 보임(부드럽게 늘었다 줄어듦).</summary>
        static void Island(Rect screen, string text)
        {
            bool open = !string.IsNullOrEmpty(text);
            float want = open ? Mathf.Min(CSCommonDesign.IslandMaxW, Draw.TextWidth(text, 13f, true) + 52f) : CSCommonDesign.IslandW;
            float w = CSCommonAnim.Toward(0x1514d1, Mathf.Max(want, CSCommonDesign.IslandW), 14f, CSCommonDesign.IslandW);
            var pill = new Rect(screen.center.x - w * 0.5f, screen.y + CSCommonDesign.IslandTop, w, CSCommonDesign.IslandH);
            Draw.Round(pill, Color.black, CSCommonDesign.IslandH * 0.5f);
            if (open && w > want - 6f) Draw.TextFit(new Rect(pill.x + 20f, pill.y, pill.width - 40f, pill.height), text, 13f, Color.white, true, TextAnchor.MiddleCenter);
        }

        /// <summary>유리 면(카드 · 줄 바탕): 반투명 바탕 + 위 반짝임 + 가는 테두리(그림자 없음). strength 로 진하기.</summary>
        public static void Glass(Rect r, float radius = -1f, float strength = 1f)
        {
            if (!Draw.Repaint) return;
            if (radius < 0f) radius = CSCommonDesign.GlassRadius;
            Draw.Round(r, Draw.Alpha(Pal.GlassFill, strength), radius);
            Draw.Round(new Rect(r.x + 2f, r.y + 2f, r.width - 4f, r.height * 0.46f), Draw.Alpha(Pal.GlassSheen, strength), Mathf.Max(2f, radius - 2f));
            Draw.RoundOutline(r, Draw.Alpha(Pal.GlassEdge, strength), radius, CSCommonDesign.GlassStroke);
            Draw.Fill(new Rect(r.x + radius, r.y + 1f, Mathf.Max(0f, r.width - radius * 2f), 1f), Draw.Alpha(Pal.GlassShine, strength));
        }

        /// <summary>유리 단추: on 이면 강조색으로 물든 유리. 누르면 true.</summary>
        public static bool GlassButton(Rect r, string label, bool on, string tip = null, bool enabled = true)
        {
            int key = Look.Key(r, "glassbtn", label);
            float hv = Look.Hover(key, r, enabled);
            if (Draw.Repaint)
            {
                Glass(r, CSCommonDesign.GlassRadius - 6f, 0.8f + 0.5f * hv);
                if (on) Draw.Round(r, Pal.Accent, CSCommonDesign.GlassRadius - 6f);
                Draw.TextFit(new Rect(r.x + 12f, r.y, r.width - 24f, r.height), label, 15f, on ? Color.white : enabled ? Pal.PhoneInk : Draw.Alpha(Pal.PhoneInk, 0.4f), true, TextAnchor.MiddleCenter);
            }
            CSCommonUX.UI.CSCommonToolbar.Tip(r, tip);
            return enabled && Draw.Button(r);
        }

        /// <summary>하단 툴바(탭 바): CSCommonUX.Toolbar 와 같은 알약(설정 FAB 없음). content = Frame 이 돌려준 쓸 자리.</summary>
        public static void TabBar(Rect content, string[] icons, string[] labels, int cur, Action<int> pick)
        {
            int hit = Toolbar.Bar(content, labels.Length, cur, i => icons != null && i < icons.Length ? icons[i] : null, i => labels[i], null, null, false, out _);
            if (hit >= 0) pick(hit);
        }
    }
}
