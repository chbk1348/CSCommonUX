using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CitiesHarmony.API;
using HarmonyLib;

namespace CSCommon
{
    /// <summary>먼저 따로 붙이고, 다른 패치가 실패해도 떼지 않음(세이브를 읽는 데 꼭 필요한 것 — 예: 길 프리팹 올리기). 모드를 끌 때만 뗌.</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CSCommonCorePatchAttribute : Attribute
    {
    }

    /// <summary>실패해도 모드 전체를 쉬게 하지 않고 이 패치만 건너뜀(다른 모드 연동 · 진단처럼 없어도 되는 것).</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CSCommonOptionalPatchAttribute : Attribute
    {
    }

    /// <summary>
    /// Harmony 패치 적용 · 해제. 이 어셈블리의 [HarmonyPatch] 클래스를 셋으로 나눠 붙임:
    /// [CSCommonCorePatch] 먼저 · 따로 ID(다른 것이 실패해도 남음) → 보통 패치(모두 붙거나 하나도 안 붙음, 점검 API 58) → [CSCommonOptionalPatch](하나씩, 실패하면 그것만 건너뜀).
    /// 추가 패치는 OnPatchAll에서. CitiesHarmony가 준비된 뒤에만 부름(CSCommonMod가 DoOnHarmonyReady로).
    /// 해제는 늘 — 붙이려 한 적이 있으면 Patched와 상관없이 이 ID의 패치를 모두 뗌.
    /// </summary>
    public class CSCommonPatcher
    {
        public string HarmonyID { get; internal set; } = "";

        string CoreID => HarmonyID + ".core";

        /// <summary>보통 패치가 모두 붙어 있음. 도시 불러오기 때 이것으로 Harmony 문제를 판단.</summary>
        public bool Patched { get; protected set; }

        /// <summary>붙이기를 시작한 적이 있음(일부만 붙었을 수 있음) — UnpatchAll이 이것으로 뗄지 정함.</summary>
        bool attempted, coreAttempted;

        public void PatchAll()
        {
            if (Patched) return;
            if (!HarmonyHelper.IsHarmonyInstalled)
            {
                CSCommonLog.Warn("Harmony가 아직 준비되지 않아 패치를 미룸");
                return;
            }
            if (!coreAttempted)
            {
                coreAttempted = true;
                try
                {
                    ApplyCore();
                }
                catch (Exception e)
                {
                    CSCommonLog.Exception(e, "Harmony 핵심 패치 붙이기 실패");
                }
            }
            attempted = true;
            try
            {
                Apply();
                Patched = true;
                CSCommonLog.Detail("Harmony 패치 붙임");
            }
            catch (Exception e)
            {
                // 일부만 붙은 채로 두면 모드는 쉬는데 몇몇 덮어쓰기가 남음 → 붙은 것까지 모두 뗌(Patched=false 그대로). 핵심 패치는 남김
                CSCommonLog.Exception(e, "Harmony 패치 붙이기 실패 · 붙은 것까지 모두 뗌");
                RemoveMain();
            }
            if (Patched) ApplyOptional();
        }

        /// <summary>모드를 끌 때: 핵심 패치까지 모두 뗌.</summary>
        public void UnpatchAll()
        {
            RemoveMain();
            if (!coreAttempted || string.IsNullOrEmpty(HarmonyID)) return;
            try
            {
                Remove(CoreID);
                coreAttempted = false;
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "Harmony 핵심 패치 떼기");
            }
        }

        void RemoveMain()
        {
            bool was = Patched;
            Patched = false;
            if (!attempted || string.IsNullOrEmpty(HarmonyID)) return;
            try
            {
                Remove(HarmonyID);
                attempted = false;
                CSCommonLog.Detail(was ? "Harmony 패치 뗌" : "일부만 붙은 Harmony 패치 뗌");
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "Harmony 패치 떼기");
            }
        }

        protected virtual void OnPatchAll(Harmony harmony)
        {
        }

        // Harmony 형식은 아래에서만 직접 씀(인라인 안 함) — 위 메서드가 Harmony 없이 불려도 형식 읽기에서 멈추지 않게.

        [MethodImpl(MethodImplOptions.NoInlining)]
        List<Type> PatchTypes(Type mark, bool with)
        {
            var list = new List<Type>();
            foreach (var t in AccessTools.GetTypesFromAssembly(GetType().Assembly))
            {
                if (!Attribute.IsDefined(t, typeof(HarmonyPatch), false)) continue;
                if (Attribute.IsDefined(t, mark, false) == with) list.Add(t);
            }
            return list;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void ApplyCore()
        {
            var harmony = new Harmony(CoreID);
            foreach (var t in PatchTypes(typeof(CSCommonCorePatchAttribute), true)) harmony.CreateClassProcessor(t).Patch();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void Apply()
        {
            var harmony = new Harmony(HarmonyID);
            foreach (var t in PatchTypes(typeof(CSCommonCorePatchAttribute), false))
                if (!Attribute.IsDefined(t, typeof(CSCommonOptionalPatchAttribute), false)) harmony.CreateClassProcessor(t).Patch();
            OnPatchAll(harmony);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void ApplyOptional()
        {
            var harmony = new Harmony(HarmonyID);
            foreach (var t in PatchTypes(typeof(CSCommonOptionalPatchAttribute), true))
            {
                try
                {
                    harmony.CreateClassProcessor(t).Patch();
                }
                catch (Exception e)
                {
                    CSCommonLog.Exception(e, $"선택 패치 {t.Name} 건너뜀(게임 · 다른 모드 판이 다름)");
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Remove(string id) => new Harmony(id).UnpatchAll(id);
    }

    /// <summary>패처 하나(모드마다). HarmonyID는 CSCommonMod가 OnEnabled에서 정함.</summary>
    public static class CSCommonPatcher<T> where T : CSCommonPatcher, new()
    {
        static T instance;

        public static string HarmonyID = "";

        public static T Instance => instance ??= new T { HarmonyID = HarmonyID };
    }
}
