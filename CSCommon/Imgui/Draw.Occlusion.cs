using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // 가림 판정 캐시: 화면 8px 칸마다 결과를 0.2초 둠 — 월드 표식 수백 개가 매 Repaint 마다 UI RaycastAll 을 하지 않게.
        // shortcut: 판정이 칸(8px) 안에서 어긋나거나 막 열고 닫은 창에 0.2초 늦을 수 있음, 표식이 패널 가장자리에서 깜빡이면 칸을 줄임
        static readonly Dictionary<int, bool> occluded = new(), occludedGame = new();
        static float occludedAt, occludedGameAt;

        /// <summary>화면 픽셀 좌표(Camera.WorldToScreenPoint) 자리에 게임 UI(패널 · 창 등)가 덮여 있는지(8px 칸 · 0.2초 캐시).</summary>
        public static bool IsOccluded(Vector3 screenPoint) => Occluded(occluded, ref occludedAt, screenPoint, false);

        /// <summary>
        /// IsOccluded 와 같되 InputBlocker 가 IMGUI 자리에 깐 보이지 않는 패널은 뺌 — 제 칸에 막는 패널을 까는 지도 표식(MapPin)이 제 패널에 가려지지 않게.
        /// IMGUI 끼리(모드 창 · 툴바)의 앞뒤는 GUI.depth 가 정함.
        /// </summary>
        public static bool IsOccludedByGame(Vector3 screenPoint) => Occluded(occludedGame, ref occludedGameAt, screenPoint, true);

        static bool Occluded(Dictionary<int, bool> cache, ref float at, Vector3 screenPoint, bool gameOnly)
        {
            float now = Time.realtimeSinceStartup;
            if (now - at > 0.2f || now < at || cache.Count > 4096) { cache.Clear(); at = now; }
            int key = (Mathf.FloorToInt(screenPoint.x / 8f) & 0xFFFF) | (Mathf.FloorToInt(screenPoint.y / 8f) << 16);
            if (!cache.TryGetValue(key, out bool hit)) cache[key] = hit = RaycastOccluded(screenPoint, gameOnly);
            return hit;
        }

        static bool RaycastOccluded(Vector3 screenPoint, bool gameOnly)
        {
            var view = UIView.GetAView();
            if (view == null || view.uiCamera == null) return false;
            Vector2 res = view.GetScreenResolution();
            Ray ray = view.uiCamera.ScreenPointToRay(new Vector3(screenPoint.x, screenPoint.y, 0f));
            try
            {
                using var hits = view.RaycastAll(ray);
                for (int i = 0; i < hits.Count; i++)
                {
                    var c = hits[i].component;
                    if (c == null || !c.isVisible || c.opacity <= 0.01f) continue;
                    if (gameOnly && c.name != null && c.name.EndsWith(InputBlocker.PanelSuffix, StringComparison.Ordinal)) continue;
                    if (c.size.x >= res.x - 10f && c.size.y >= res.y - 10f)
                    {
                        if (!UIView.HasModalInput()) continue;
                    }
                    return true;
                }
            }
            catch (NullReferenceException)
            {
                // 게임 UI 가 이 프레임에 지워지는 중이면 RaycastAll 이 NRE(로그 1.2.0 · 1초 안 3번) — 안 가린 것으로 보고 다음 0.2초 캐시 때 다시 잼
            }
            return false;
        }
    }
}
