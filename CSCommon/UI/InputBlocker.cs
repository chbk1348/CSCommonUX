using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// IMGUI 창 위 클릭 · 휠이 게임 도구 · 카메라로 새지 않게:
    /// 게임 UI(UIView)에 보이지 않는 패널을 IMGUI 영역마다 깔아, 게임이 "UI 위"로 알게 함.
    /// 매 프레임(모드 컨트롤러 Update) Begin(모드 이름) → Add(영역마다) → End, 쓰지 않은 패널은 숨김. 모드를 내릴 때 Release.
    /// 모드마다 자기 DLL에 한 벌씩 들어가므로(정적 상태도 모드별) 패널 이름만 모드 이름으로 나눔.
    /// </summary>
    public static class InputBlocker
    {
        static readonly List<UIPanel> panels = new();
        static readonly List<Rect> logicals = new(); // 패널마다 이번 프레임 논리 영역(At 용)
        static int used;
        static string owner = "CSCommonUX";

        /// <summary>프레임 시작(Update)에. owner = 패널 이름 머리(모드마다 다르게, 예 "F1GrandPrix") — 새 패널을 만들 때만 씀.</summary>
        public static void Begin(string owner)
        {
            used = 0;
            InputBlocker.owner = owner;
        }

        /// <summary>논리 좌표 영역 하나를 막음.</summary>
        public static void Add(Rect logical)
        {
            var view = UIView.GetAView();
            if (view == null || logical.width <= 0f || logical.height <= 0f) return;
            UIPanel p;
            if (used < panels.Count) p = panels[used];
            else
            {
                p = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
                p.name = owner + "InputBlocker";
                p.backgroundSprite = null;
                p.isInteractive = true;
                p.canFocus = false;
                panels.Add(p);
                logicals.Add(default);
            }
            logicals[used] = logical;
            used++;
            // 논리(1080 기준) → 화소 → 게임 UI 좌표
            float px = Draw.S;
            float ratio = view.GetScreenResolution().y / Screen.height;
            // 값이 바뀔 때만 대입 — 게임 UI는 대입마다 다시 그리기를 잡아 매 프레임 대입하면 UI 전체가 매 프레임 무효화됨
            var pos = new Vector3(logical.x * px * ratio, logical.y * px * ratio);
            var size = new Vector2(logical.width * px * ratio, logical.height * px * ratio);
            if ((p.absolutePosition - pos).sqrMagnitude > 0.25f) p.absolutePosition = pos;
            if ((p.size - size).sqrMagnitude > 0.25f) p.size = size;
            bool shown = false;
            if (!p.isVisible)
            {
                p.Show();
                shown = true;
            }
            // 맨 앞으로: 처음 보일 때와 30틀마다(다른 게임 창이 위로 올라와도 다시 덮게)
            if (shown || Time.frameCount % 30 == 0) p.BringToFront();
        }

        /// <summary>논리 좌표 점 위에 이번 프레임에 깐 패널(없으면 null) — 게임 UI를 숨겨 UIInput이 꺼진 동안 호버를 직접 정할 때.</summary>
        public static UIComponent At(Vector2 logical)
        {
            for (int i = 0; i < used && i < panels.Count; i++)
                if (panels[i] != null && logicals[i].Contains(logical)) return panels[i];
            return null;
        }

        /// <summary>프레임 끝: 이번 프레임에 쓰지 않은 패널은 숨김.</summary>
        public static void End()
        {
            for (int i = used; i < panels.Count; i++)
                if (panels[i] != null && panels[i].isVisible) panels[i].Hide();
        }

        public static void Release()
        {
            foreach (var p in panels)
                if (p != null) Object.Destroy(p.gameObject);
            panels.Clear();
            logicals.Clear();
            used = 0;
        }
    }
}
