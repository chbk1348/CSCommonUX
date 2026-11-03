using System;
using CSCommon.UI;

namespace CSCommon
{
    /// <summary>업데이트 내역 한 판.</summary>
    public sealed class CSCommonRelease
    {
        public Version Version;
        /// <summary>yyyy-MM-dd(설정 패널 '모드 출시일').</summary>
        public string Date = "";
        public string[] Lines = new string[0];

        public CSCommonRelease(string version, string date, params string[] lines)
        {
            Version = new Version(version);
            Date = date ?? "";
            Lines = lines ?? new string[0];
        }
    }

    /// <summary>
    /// 업데이트 내역: 새 판을 처음 불러올 때 한 번 알림('다시 보지 않기' = 그 판까지 봄), 설정 패널 버튼으로 언제든 전체.
    /// 마지막으로 본 판은 설정 XML(WhatsNewVersion)에 저장.
    /// </summary>
    public static class CSCommonChangelog
    {
        public static Version LastSeen = new("0.0");

        public static string LastSeenText
        {
            get => LastSeen.ToString();
            set
            {
                try
                {
                    LastSeen = string.IsNullOrEmpty(value) ? new Version("0.0") : new Version(value);
                }
                catch (Exception)
                {
                    LastSeen = new Version("0.0");
                }
            }
        }

        /// <summary>새 판이면 알림. 도시 불러오기 끝에 UxLoading이 부름.</summary>
        public static void ShowIfNew()
        {
            var mod = CSCommonModBase.Instance;
            var list = mod?.Releases;
            if (list == null || list.Length == 0 || LastSeen >= list[0].Version) return;
            CSCommonDialog.Changelog(mod.Name, list, LastSeen, () =>
            {
                LastSeen = list[0].Version;
                mod.SaveSettings();
            });
        }

        /// <summary>전체 내역(설정 패널 · F1 창 버튼).</summary>
        public static void ShowAll()
        {
            var mod = CSCommonModBase.Instance;
            if (mod?.Releases == null) return;
            CSCommonDialog.Changelog(mod.Name, mod.Releases, new Version("0.0"), null);
        }
    }
}
