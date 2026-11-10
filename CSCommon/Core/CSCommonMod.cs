using System;
using CitiesHarmony.API;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using CSCommonUX.UI;
using ICities;

namespace CSCommonUX
{
    /// <summary>모드 공통 정보(설정 패널 · 알림 · 로그가 봄).</summary>
    public abstract class CSCommonModBase
    {
        public static CSCommonModBase Instance { get; private set; }

        protected CSCommonModBase()
        {
            Instance = this;
        }

        /// <summary>화면에 보이는 모드 이름(판 없이).</summary>
        public abstract string BaseName { get; }

        /// <summary>모드 목록 이름 = 이름 + 판.</summary>
        public virtual string Name => BaseName + " " + VersionText;

        public virtual string LogName => "[" + BaseName + "]";

        public abstract string HarmonyID { get; }

        /// <summary>업데이트 내역(새 판이 맨 앞). 첫 판이 지금 판 · 출시일.</summary>
        public virtual CSCommonRelease[] Releases => null;

        /// <summary>판 꼬리표(설정 패널 판 배지): 예 "개발 중" · "안정".</summary>
        public virtual string Channel => "개발 중";

        public string VersionText
        {
            get
            {
                var r = Releases;
                if (r != null && r.Length > 0) return r[0].Version.ToString();
                var v = GetType().Assembly.GetName().Version;
                return v.Build > 0 ? v.ToString(3) : v.ToString(2);
            }
        }

        public string ReleaseDate
        {
            get
            {
                var r = Releases;
                return r != null && r.Length > 0 ? r[0].Date : "";
            }
        }

        /// <summary>이 어셈블리가 든 모드 폴더(없으면 null).</summary>
        public static string Folder()
        {
            try
            {
                var asm = typeof(CSCommonModBase).Assembly;
                foreach (var plugin in PluginManager.instance.GetPluginsInfo())
                    if (plugin.ContainsAssembly(asm)) return plugin.modPath;
            }
            catch (Exception e)
            {
                CSCommonLog.Warn($"모드 폴더를 찾지 못함: {e.Message}");
            }
            return null;
        }

        /// <summary>Harmony 패치가 붙었나(설정 패널 '모드 상태').</summary>
        public abstract bool Patched { get; }

        public abstract void LoadSettings();

        public abstract void SaveSettings();
    }

    /// <summary>
    /// 모드 진입(IUserMod 쪽): 켜질 때 설정 읽기 → 게임 설정 화면에 패널 걸기 → CitiesHarmony가 준비되면 패치.
    /// 꺼질 때 패치 · 패널 고리를 뗌. 모드 클래스는 이것을 이어받고 IUserMod(Name · Description)를 붙임.
    /// </summary>
    public abstract class CSCommonMod<TPanel, TPatcher> : CSCommonModBase
        where TPanel : CSCommonOptionsPanel
        where TPatcher : CSCommonPatcher, new()
    {
        public override bool Patched => CSCommonPatcher<TPatcher>.Instance.Patched;

        public virtual void OnEnabled()
        {
            CSCommonLog.Name = LogName;
            CSCommonPatcher<TPatcher>.HarmonyID = HarmonyID;
            LoadSettings();
            if (UIView.GetAView() != null) CSCommonOptionsHost<TPanel>.Hook();
            else LoadingManager.instance.m_introLoaded += CSCommonOptionsHost<TPanel>.Hook;
            HarmonyHelper.DoOnHarmonyReady(() =>
            {
                try
                {
                    CSCommonPatcher<TPatcher>.Instance.PatchAll();
                }
                catch (Exception e)
                {
                    CSCommonLog.Exception(e, "Harmony 패치");
                }
            });
            CSCommonLog.Info($"{VersionText} 켜짐");
        }

        /// <summary>정리 단계마다 따로 감쌈(점검 API 57): 하나가 던져도 나머지(특히 패치 떼기)는 함.</summary>
        public virtual void OnDisabled()
        {
            try
            {
                LoadingManager.instance.m_introLoaded -= CSCommonOptionsHost<TPanel>.Hook;
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "설정 패널 고리 떼기(intro)");
            }
            try
            {
                CSCommonOptionsHost<TPanel>.Unhook();
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "설정 패널 고리 떼기");
            }
            try
            {
                if (HarmonyHelper.IsHarmonyInstalled) CSCommonPatcher<TPatcher>.Instance.UnpatchAll();
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "Harmony 패치 떼기");
            }
        }

        /// <summary>게임이 모드 설정 화면을 열 때 부름(반사로). 패널은 화면이 보일 때 UxOptionsHost가 지음.</summary>
        public void OnSettingsUI(UIHelperBase helper) => CSCommonOptionsHost<TPanel>.Setup(helper);
    }
}
