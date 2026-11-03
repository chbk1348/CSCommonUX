using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommon.UI
{
    /// <summary>
    /// 알림 창(F1 창 모양): 화면 가운데 창 하나 — 제목줄 · 빨강 띠 · 스크롤 본문 · 버튼.
    /// 업데이트 내역(판마다 제목 · 날짜 · 항목)과 일반 알림(문단 · 목록)에 씀. 닫으면 지움.
    /// </summary>
    public static class CSCommonDialog
    {
        const float W = 640f, MaxH = 620f, Pad = 20f, HeadH = 54f, FootH = 64f;

        static UIPanel win;

        /// <summary>업데이트 내역: since보다 새 판만(0.0이면 전부). dontShowAgain이 있으면 '다시 보지 않기' 버튼.</summary>
        /// <param name="maxLines">처음 만들 줄 수 — 넘으면 아래 「이전 내역 더 보기」(줄마다 게임 UI 글 칸 · 줄바꿈 계산이라 수백 줄을 한 번에 만들면 창이 열릴 때 멈칫했음).</param>
        public static void Changelog(string title, CSCommonRelease[] releases, Version since, Action dontShowAgain, int maxLines = 40)
        {
            Open(title + " · " + "업데이트 내역", body =>
            {
                float y = 4f;
                int lines = 0;
                bool more = false;
                foreach (var r in releases)
                {
                    if (r.Version <= since) continue;
                    if (lines >= maxLines) { more = true; break; }
                    string verStr = "v" + r.Version;
                    var badge = CSCommonKit.Badge(body, verStr, Pal.Accent);
                    badge.relativePosition = new Vector3(0f, y);
                    badge.PerformLayout();
                    float badgeW = Mathf.Max(badge.width, 56f);
                    if (r.Date.Length > 0)
                    {
                        var d = CSCommonKit.Label(body, r.Date, 12f, Pal.Dim, badgeW + 8f, y);
                        d.PerformLayout();
                        // 날짜를 뱃지 높이 가운데에
                        d.relativePosition = new Vector3(d.relativePosition.x, y + Mathf.Round((badge.height - d.height) * 0.5f));
                    }
                    float headH = Mathf.Max(badge.height, 22f);
                    y += headH + 8f;
                    foreach (string line in r.Lines)
                    {
                        if (lines >= maxLines) { more = true; break; }
                        lines++;
                        y += CSCommonKit.ListItem(body, line, 13f, Pal.Light, 0f, y, body.width - 9f) + 6f;
                    }
                    y += 14f;
                }
                if (more)
                {
                    int next = maxLines + 60;
                    CSCommonKit.Button(body, "이전 내역 더 보기", 0f, y, 200f, 32f, CSCommonButtonKind.Outline, () =>
                    {
                        Close();
                        Changelog(title, releases, since, dontShowAgain, next);
                    });
                    y += 40f;
                }
                return y + 6f;
            }, dontShowAgain);
        }

        /// <summary>일반 알림: 문단들 · 점 목록.</summary>
        public static void Notice(string title, string[] paragraphs, string[] list)
        {
            Open(title, body =>
            {
                float y = 4f;
                foreach (string p in paragraphs ?? new string[0])
                {
                    var l = CSCommonKit.Label(body, p, 14f, Pal.Txt, 0f, y, body.width - 4f);
                    l.PerformLayout();
                    y += l.height + 10f;
                }
                foreach (string item in list ?? new string[0])
                {
                    y += CSCommonKit.ListItem(body, item, 13f, Pal.Light, 0f, y, body.width - 9f) + 6f;
                }
                return y + 6f;
            }, null);
        }

        static void Open(string title, Func<UIScrollablePanel, float> fill, Action dontShowAgain)
        {
            try
            {
                Close();
                var view = UIView.GetAView();
                if (view == null) return;
                Vector2 screen = view.GetScreenResolution();

                win = (UIPanel)view.AddUIComponent(typeof(UIPanel));
                win.atlas = CSCommonAtlas.Atlas;
                win.backgroundSprite = CSCommonAtlas.Round10;
                win.color = Pal.Win;
                win.size = new Vector2(W, 200f);
                var edge = CSCommonKit.Panel(win, CSCommonAtlas.RoundLine10, Pal.Line, 0f, 0f, W, 200f);
                edge.isInteractive = false;
                edge.anchor = UIAnchorStyle.All;

                // 제목줄: 빨강 F1 · 제목 · 닫기
                CSCommonKit.Label(win, "F1", 20f, Pal.Accent, Pad, 15f, -1f, true);
                CSCommonKit.Label(win, title, 17f, Pal.Txt, Pad + 44f, 16f, W - Pad * 2f - 100f, true);
                var x = CSCommonKit.Button(win, "✕", W - HeadH, 0f, HeadH, HeadH, CSCommonButtonKind.Outline, Close);
                x.normalBgSprite = x.disabledBgSprite = "";
                x.textScale = CSCommonKit.Scale(18f);
                x.textColor = Pal.Sub;
                CSCommonKit.Line(win, 0f, HeadH, W, 3f, Pal.Accent);

                var body = win.AddUIComponent<UIScrollablePanel>();
                body.relativePosition = new Vector3(Pad, HeadH + 3f + 16f);
                body.width = W - Pad * 2f - 12f;
                body.height = 10f;
                body.clipChildren = true;
                body.autoLayout = false;
                body.scrollWheelDirection = UIOrientation.Vertical;
                body.scrollWheelAmount = 40;
                float contentH = fill(body);
                float bodyH = Mathf.Clamp(contentH, 40f, MaxH - HeadH - 3f - 16f - FootH);
                body.height = bodyH;
                if (contentH > bodyH) CSCommonKit.Scrollbar(win, body);

                float h = HeadH + 3f + 16f + bodyH + FootH;
                win.height = h;
                float by = h - FootH + 14f;
                CSCommonKit.Button(win, "닫기", W - Pad - 140f, by, 140f, 36f, CSCommonButtonKind.Primary, Close);
                if (dontShowAgain != null)
                    CSCommonKit.Button(win, "다시 보지 않기", W - Pad - 140f - 10f - 160f, by, 160f, 36f, CSCommonButtonKind.Outline, () =>
                    {
                        dontShowAgain();
                        Close();
                    });
                win.relativePosition = new Vector3((screen.x - W) * 0.5f, (screen.y - h) * 0.5f);
                UIView.PushModal(win);
                win.BringToFront();
                // 나타남: 투명 → 불투명(0.18초) · 12px 아래에서 떠오름
                var shown = win;
                Vector3 at = win.relativePosition;
                win.opacity = 0f;
                ValueAnimator.Animate(v =>
                {
                    if (shown == null) return;
                    shown.opacity = v;
                    shown.relativePosition = at + new Vector3(0f, (1f - v) * 12f);
                }, new AnimatedFloat(0f, 1f, 0.18f, EasingType.CubicEaseOut));
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "알림 창");
                Close();
            }
        }

        public static void Close()
        {
            if (win == null) return;
            try
            {
                if (UIView.GetModalComponent() == win) UIView.PopModal();
                // 반투명 배경(게임 ModalEffect)이 창 위로 남지 않게: 게임 UnlockingPanel.Hide와 같이 남은 모달 바로 아래로, 없으면 숨김
                var fx = UIView.GetAView()?.panelsLibraryModalEffect;
                if (fx != null)
                {
                    if (UIView.HasModalInput()) fx.zOrder = UIView.GetModalComponent().zOrder - 1;
                    else fx.Hide();
                }
            }
            catch (Exception)
            {
                // 이미 내려감
            }
            // 사라짐: 입력을 끊고 0.14초 동안 흐려진 뒤 지움(바로 다음 창을 열어도 겹치지 않게 win 은 먼저 비움)
            var gone = win;
            win = null;
            gone.isInteractive = false;
            float from = gone.opacity;
            ValueAnimator.Animate(v => { if (gone != null) gone.opacity = v; }, new AnimatedFloat(from, 0f, 0.14f, EasingType.CubicEaseIn),
                () => { if (gone != null) UnityEngine.Object.Destroy(gone.gameObject); });
        }
    }
}
