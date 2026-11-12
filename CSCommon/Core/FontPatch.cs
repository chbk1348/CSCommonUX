using System;
using System.Reflection;
using ColossalFramework.UI;
using HarmonyLib;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// 게임 전체에 닿는 글꼴 패치 2개(F1에서 옮김). 모드마다 같은 클래스가 자기 DLL에 들어가므로,
    /// 다른 모드의 같은 패치(같은 형식 이름)가 이미 붙어 있으면 Prepare에서 건너뜀 — 여러 모드가 켜져도 한 번만 붙음.
    /// shortcut: 먼저 붙인 모드를 게임 중에 끄면 그 패치도 떨어지고 남은 모드는 다시 붙이지 않음, 필요해지면 끌 때 남은 모드가 다시 붙이게.
    /// </summary>
    static class FontPatchOnce
    {
        public static bool Free(MethodBase target, Type self)
        {
            if (target == null) return false;
            var info = Harmony.GetPatchInfo(target);
            if (info == null) return true;
            foreach (var p in info.Prefixes)
                if (p.PatchMethod?.DeclaringType?.FullName == self.FullName)
                {
                    CSCommonLog.Detail($"글꼴 패치 {self.Name}: 다른 모드({p.owner})가 이미 붙임 · 건너뜀");
                    return false;
                }
            return true;
        }
    }

    /// <summary>
    /// 글꼴 처리기 묶기. 게임 UITextComponent는 어떤 글꼴 텍스처가 다시 지어져도(Font.textureRebuilt) 모든 글 칸을 다시 그림.
    /// 모드 창(IMGUI)이 한글 글자를 새로 요청할 때마다 그 글꼴이 다시 지어져, 글이 많이 바뀌는 때 게임 UI 전체가 매번 다시 그려져 멈칫(프리징)함.
    /// → 다시 지어진 글꼴이 자기 글꼴일 때만 반응하게 묶음.
    /// </summary>
    [CSCommonOptionalPatch]
    [HarmonyPatch(typeof(UITextComponent), "OnFontTextureRebuilt")]
    static class FontRebuildPatch
    {
        static bool Prepare(MethodBase original) => original == null || FontPatchOnce.Free(original, typeof(FontRebuildPatch));

        static bool Prefix(UITextComponent __instance, Font font)
        {
            UIFont own = __instance.font;
            if (own == null) return true;
            Font baseFont = own.baseFont;
            return baseFont == null || baseFont == font;
        }
    }

    /// <summary>
    /// 글꼴 텍스처가 다시 지어졌을 때 알리기(Font.textureRebuilt)를 차례로 부름.
    /// 게임의 옛 Mono는 묶인 처리기를 하나 부를 때마다 앞 것을 안에서 부르는(재귀) 방식이라,
    /// 모드가 많아 글 칸(UITextComponent)이 수천 개면 한 번 알릴 때 스택이 넘침(StackOverflowException).
    /// 넘치면 뒤 처리기들이 못 불려 글이 깨짐.
    /// → 목록을 꺼내 하나씩(반복문으로) 부르고, 하나가 던져도 나머지는 부름. 하는 일 · 순서는 원래와 같음.
    /// </summary>
    [CSCommonOptionalPatch]
    [HarmonyPatch(typeof(Font), "InvokeTextureRebuilt_Internal")]
    static class FontRebuildInvokePatch
    {
        static readonly FieldInfo Event = AccessTools.Field(typeof(Font), "textureRebuilt");
        static readonly FieldInfo Callback = AccessTools.Field(typeof(Font), "m_FontTextureRebuildCallback");

        static bool Prepare(MethodBase original) =>
            Event != null && Callback != null && (original == null || FontPatchOnce.Free(original, typeof(FontRebuildInvokePatch)));

        // 처리기 목록은 묶음(대리자)이 바뀔 때만 다시 꺼냄 — 대리자는 바뀌면 새 객체라 참조가 같으면 목록도 같음(재빌드마다 수천 칸 배열 할당 안 함)
        static Action<Font> lastAll;
        static Delegate[] lastList;
        static readonly PerfWatch watch = new("글꼴 재빌드 알림");

        static bool Prefix(Font font)
        {
            watch.Begin();
            if (Event.GetValue(null) is Action<Font> all)
            {
                if (!ReferenceEquals(all, lastAll))
                {
                    lastList = all.GetInvocationList();
                    lastAll = all;
                }
                var list = lastList;
                for (int i = 0; i < list.Length; i++)
                {
                    try { ((Action<Font>)list[i])(font); }
                    catch (Exception e) { Debug.LogException(e); }
                }
            }
            if (font != null && Callback.GetValue(font) is Font.FontTextureRebuildCallback own)
            {
                try { own(); }
                catch (Exception e) { Debug.LogException(e); }
            }
            watch.End();
            return false;
        }
    }
}
