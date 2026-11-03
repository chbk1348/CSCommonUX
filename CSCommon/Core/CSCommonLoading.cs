using System;
using CSCommon.UI;
using ICities;

namespace CSCommon
{
    /// <summary>
    /// 도시를 불러오고 내릴 때(LoadingExtension 쪽). 게임 모드에서만 켬.
    /// Harmony 패치가 없으면 모드만 쉬고 알림(도시 불러오기는 계속), 이어받은 쪽 검사(CreatedChecksPassed)가 실패해도 쉼.
    /// 불러오기가 끝나면 LoadedActions → 설정 패널 고리 다시 걸기 → 새 판이면 업데이트 내역 알림.
    /// </summary>
    public abstract class CSCommonLoading<TPanel, TPatcher> : LoadingExtensionBase
        where TPanel : CSCommonOptionsPanel
        where TPatcher : CSCommonPatcher, new()
    {
        static bool created;
        bool active, harmonyMissing;

        public static bool IsLoaded { get; private set; }

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (created)
            {
                CSCommonLog.Detail("OnCreated 두 번째 부름 · 건너뜀");
                return;
            }
            active = false;
            harmonyMissing = false;
            if (loading.currentMode != AppMode.Game)
            {
                CSCommonLog.Detail($"게임 모드가 아님({loading.currentMode}) · 쉼");
                return;
            }
            if (!CSCommonPatcher<TPatcher>.Instance.Patched)
            {
                harmonyMissing = true;
                CSCommonLog.Error("Harmony 패치가 붙지 않아 모드를 쉼");
                return;
            }
            if (!CreatedChecksPassed())
            {
                CSCommonLog.Info("불러오기 검사 실패 · 모드를 쉼");
                return;
            }
            active = created = true;
            CreatedActions(loading);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            if (harmonyMissing)
            {
                CSCommonDialog.Notice(CSCommonModBase.Instance?.Name ?? "",
                    new[]
                    {
                        "Harmony 패치를 붙이지 못해 이 모드를 쉽니다. 도시는 그대로 불러왔습니다.",
                        "흔한 원인:",
                    },
                    new[]
                    {
                        "Harmony 모드(창작마당 2040656402)를 구독하지 않았거나 꺼짐",
                        "Harmony 모드가 제대로 읽히지 않음(게임을 다시 시작)",
                        "다른 모드가 Harmony를 망가뜨림",
                    });
                return;
            }
            if (!active) return;
            // 단계마다 따로 감쌈(점검 API 57): 본체 붙이기가 실패하면 모드만 쉬고, 패널 고리 · 내역 알림 실패는 본체와 상관없이 넘어감
            try
            {
                LoadedActions(mode);
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "도시 불러온 뒤 모드 붙이기 · 모드를 쉼");
                return;
            }
            try
            {
                CSCommonOptionsHost<TPanel>.Hook();
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "설정 패널 고리 걸기");
            }
            try
            {
                CSCommonChangelog.ShowIfNew();
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "업데이트 내역 알림");
            }
            IsLoaded = true;
            CSCommonLog.Info("도시 불러오기 끝");
        }

        public override void OnLevelUnloading()
        {
            IsLoaded = false;
            base.OnLevelUnloading();
        }

        public override void OnReleased()
        {
            created = false;
            base.OnReleased();
        }

        /// <summary>도시를 만들기 전 검사(예: DLC). false면 모드를 쉼.</summary>
        protected virtual bool CreatedChecksPassed() => true;

        protected virtual void CreatedActions(ILoading loading)
        {
        }

        protected virtual void LoadedActions(LoadMode mode)
        {
        }
    }
}
