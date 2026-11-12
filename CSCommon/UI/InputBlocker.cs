using System;
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
    /// 허브 공유(성능 3-4): 모드 허브(YukiModBox)가 Host 로 공유 표를 올려 두면 모드는 패널 대신 표에 칸만 올리고(허브가 패널 한 벌을 깖),
    /// 다른 모드 창이 위에 그려진 자리는 Draw.Hover 가 받지 않음(Covered). 표가 없으면 지금처럼 혼자.
    /// </summary>
    public static class InputBlocker
    {
        const string NoOwner = "CSCommonUX";
        static readonly List<UIPanel> panels = new();
        static readonly List<Rect> logicals = new(); // 패널마다 이번 프레임 논리 영역(At 용)
        static int used;
        static string owner = NoOwner;

        // ---- 허브 공유 표: AppDomain 데이터(어셈블리마다 CSCommonUX 형식이 따로라 BCL · UnityEngine · 게임 형식만 담음)
        // [0] 주인 → 이번 프레임 논리 칸 · [1] 주인 그린 순서(뒤일수록 위) · [2] 허브 패널 · [3] 허브 패널 칸
        const string Key = "CSCommonUX.Hub.v1";
        static Dictionary<string, List<Rect>> rects;
        static List<string> order;
        static List<UIPanel> hostPanels;
        static List<Rect> hostLogicals;
        static List<Rect> mine; // 공유 중 내 칸(표 안 목록), 혼자면 null
        static int sharedFrame = -1, coverFrame = -1;
        static bool hosting, covered;
        static Vector2 coverAt;

        /// <summary>이번 프레임 공유 표(프레임당 한 번 읽음). 허브가 없거나 모양이 다르면 false.</summary>
        static bool Shared()
        {
            int f = Time.frameCount;
            if (sharedFrame == f) return rects != null;
            sharedFrame = f;
            var h = AppDomain.CurrentDomain.GetData(Key) as object[];
            var r = h != null && h.Length >= 4 ? h[0] as Dictionary<string, List<Rect>> : null;
            var o = r != null ? h[1] as List<string> : null;
            var p = o != null ? h[2] as List<UIPanel> : null;
            var l = p != null ? h[3] as List<Rect> : null;
            if (l == null) r = null;
            if ((r != null) != (rects != null)) CSCommonLog.Info("허브 공유(가림 · 입력 차단) " + (r != null ? "켜짐" : "꺼짐"));
            rects = r;
            order = o;
            hostPanels = p;
            hostLogicals = l;
            return r != null;
        }

        /// <summary>프레임 시작(Update)에. owner = 패널 이름 머리(모드마다 다르게, 예 "F1GrandPrix") — 공유 표 열쇠 · 새 패널 이름.</summary>
        public static void Begin(string owner)
        {
            used = 0;
            InputBlocker.owner = owner;
            mine = null;
            if (!Shared()) return;
            if (!rects.TryGetValue(owner, out mine)) rects[owner] = mine = new List<Rect>();
            mine.Clear();
        }

        /// <summary>논리 좌표 영역 하나를 막음.</summary>
        public static void Add(Rect logical)
        {
            if (logical.width <= 0f || logical.height <= 0f) return;
            if (mine != null) mine.Add(logical); // 허브가 패널을 깖
            else Place(logical);
        }

        static void Place(Rect logical)
        {
            var view = UIView.GetAView();
            if (view == null) return;
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

        /// <summary>논리 좌표 점 위에 깐 패널(없으면 null) — 게임 UI를 숨겨 UIInput이 꺼진 동안 호버를 직접 정할 때. 공유 중이면 허브 패널(내 칸 안일 때만).</summary>
        public static UIComponent At(Vector2 logical)
        {
            if (mine != null && Shared())
            {
                bool inMine = false;
                foreach (var r in mine) inMine |= r.Contains(logical);
                if (!inMine) return null;
                for (int i = 0; i < hostPanels.Count && i < hostLogicals.Count; i++)
                    if (hostPanels[i] != null && hostPanels[i].isVisible && hostLogicals[i].Contains(logical)) return hostPanels[i];
                return null;
            }
            for (int i = 0; i < used && i < panels.Count; i++)
                if (panels[i] != null && logicals[i].Contains(logical)) return panels[i];
            return null;
        }

        /// <summary>프레임 끝: 이번 프레임에 쓰지 않은 패널은 숨김(공유로 바뀐 프레임에는 내 패널을 모두 숨김).</summary>
        public static void End()
        {
            if (hosting) return; // 허브 패널은 HostFrame 이 깔고 숨김
            for (int i = used; i < panels.Count; i++)
                if (panels[i] != null && panels[i].isVisible) panels[i].Hide();
        }

        public static void Release()
        {
            if (Shared())
            {
                rects.Remove(owner);
                order.Remove(owner);
            }
            if (hosting)
            {
                AppDomain.CurrentDomain.SetData(Key, null);
                hosting = false;
                sharedFrame = -1;
            }
            mine = null;
            foreach (var p in panels)
                if (p != null) UnityEngine.Object.Destroy(p.gameObject);
            panels.Clear();
            logicals.Clear();
            used = 0;
        }

        // ---- 허브 쪽(YukiModBox 만 부름)

        /// <summary>허브 본체가 붙을 때 한 번: 공유 표를 올림. 이 DLL 의 패널 목록이 허브 패널이 됨. 내릴 때는 Release.</summary>
        public static void Host()
        {
            AppDomain.CurrentDomain.SetData(Key, new object[] { new Dictionary<string, List<Rect>>(), new List<string>(), panels, logicals });
            hosting = true;
            sharedFrame = -1;
        }

        /// <summary>허브 LateUpdate(모든 모드 Update 뒤): 모든 모드 칸에 패널을 깔고 남은 패널은 숨김.</summary>
        public static void HostFrame()
        {
            if (!hosting || !Shared()) return;
            used = 0;
            foreach (var l in rects.Values)
                foreach (var r in l) Place(r);
            for (int i = used; i < panels.Count; i++)
                if (panels[i] != null && panels[i].isVisible) panels[i].Hide();
        }

        // ---- 모드끼리 가림(Draw 가 부름)

        /// <summary>Draw.Begin(Repaint)에서: 이 모드를 그린 순서 맨 뒤(맨 위)로. 같은 프레임 마지막 그리기가 순서를 정함.</summary>
        internal static void Painted()
        {
            if (owner == NoOwner || !Shared()) return;
            if (order.Count > 0 && order[order.Count - 1] == owner) return;
            order.Remove(owner);
            order.Add(owner);
        }

        /// <summary>논리 마우스 m 이 나보다 위에 그려진 다른 모드 칸 안인가(허브 없으면 늘 false). 같은 프레임 · 같은 자리면 지난 결과.</summary>
        internal static bool Covered(Vector2 m)
        {
            if (owner == NoOwner || !Shared()) return false;
            int f = Time.frameCount;
            if (coverFrame == f && coverAt == m) return covered;
            coverFrame = f;
            coverAt = m;
            covered = false;
            int me = order.IndexOf(owner);
            if (me < 0) return false;
            for (int i = me + 1; i < order.Count; i++)
                if (rects.TryGetValue(order[i], out var l))
                    foreach (var r in l)
                        if (r.Contains(m)) return covered = true;
            return false;
        }
    }
}
