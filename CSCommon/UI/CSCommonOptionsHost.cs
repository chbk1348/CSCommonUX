using System;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace CSCommonUX.UI
{
    /// <summary>
    /// 게임 설정 화면(OptionsPanel)의 이 모드 칸에 패널을 걸고 뗌.
    /// 설정 화면이 보일 때 새로 짓고(다른 곳에서 바꾼 값도 바로 보임) 닫힐 때 설정을 저장하고 지움.
    /// </summary>
    public static class CSCommonOptionsHost<TPanel> where TPanel : CSCommonOptionsPanel
    {
        const float Margin = 10f;

        static UIScrollablePanel parent;
        static UIPanel gameOptions;
        static GameObject host;
        static TPanel panel;

        public static float Width => parent != null ? parent.width - Margin * 2f : 744f;

        public static float Height => parent != null ? parent.height - 15f : 753f;

        public static bool IsOpen => panel != null;

        /// <summary>지금 열린 패널(없으면 null).</summary>
        public static TPanel Panel => panel;

        /// <summary>OnSettingsUI: 게임이 만든 이 모드 칸.</summary>
        public static void Setup(UIHelperBase helper)
        {
            var next = (helper as UIHelper)?.self as UIScrollablePanel;
            if (next == null) return;
            next.autoLayout = false;
            // 설정 화면 왼쪽 모드 목록에서 이 모드를 누를 때 칸이 (새로) 옴 → 이미 열린 화면이면 그 칸에 바로 지음
            if (next != parent)
            {
                if (parent != null) parent.eventVisibilityChanged -= OnParentVisibility;
                Close();
                next.eventVisibilityChanged += OnParentVisibility;
            }
            parent = next;
            if (gameOptions == null)
            {
                Hook();
                // 모드 진입(CSCommonMod)을 안 거치는 모드: 게임 설정 화면이 아직 없으면 첫 화면 뒤 다시 걺
                if (gameOptions == null && LoadingManager.exists)
                {
                    LoadingManager.instance.m_introLoaded -= Hook;
                    LoadingManager.instance.m_introLoaded += Hook;
                }
            }
            if ((gameOptions != null && gameOptions.isVisible) || parent.isVisible) Create();
        }

        // 이 모드 칸이 보이게 되면 지음(게임 설정 화면 보임 고리를 못 건 때 대비)
        static void OnParentVisibility(UIComponent c, bool visible)
        {
            if (!visible) return;
            if (gameOptions == null) Hook();
            Create();
        }

        public static void Hook()
        {
            var found = UIView.library?.Get<UIPanel>("OptionsPanel");
            if (found == null)
            {
                CSCommonLog.Warn("게임 설정 화면(OptionsPanel)을 찾지 못함");
                return;
            }
            if (found == gameOptions) return;
            Unhook();
            gameOptions = found;
            gameOptions.eventVisibilityChanged += OnVisibility;
        }

        public static void Unhook()
        {
            if (gameOptions != null) gameOptions.eventVisibilityChanged -= OnVisibility;
            gameOptions = null;
            Close();
        }

        static void OnVisibility(UIComponent c, bool visible)
        {
            if (visible) Create();
            else Close();
        }

        /// <summary>패널 안에서 다시 지을 때(값에 따라 줄 모양이 바뀜).</summary>
        public static void Rebuild()
        {
            if (panel == null) return;
            Close();
            Create();
        }

        static void Create()
        {
            if (host != null || parent == null) return;
            try
            {
                host = new GameObject(typeof(TPanel).Name);
                host.transform.parent = parent.transform;
                panel = host.AddComponent<TPanel>();
                panel.relativePosition = new Vector2(Margin, Margin);
                panel.Setup(Width, Height, Rebuild);
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "설정 패널 짓기");
            }
        }

        static void Close()
        {
            if (host == null) return;
            CSCommonModBase.Instance?.SaveSettings();
            UnityEngine.Object.Destroy(host);
            host = null;
            panel = null;
        }
    }
}
