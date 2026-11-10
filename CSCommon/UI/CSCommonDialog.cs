using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX.UI
{
    /// <summary>
    /// 알림 창(F1 창 모양): 화면 가운데 창 하나 — 제목줄 · 주 색 띠 · 가상화 스크롤 본문 · 버튼.
    /// 업데이트 내역(판마다 볼드 뱃지 · 날짜 · 신규/개선/수정 대분류 볼드 · 항목)과 일반 알림(문단 · 목록)에 씀.
    /// 게임 UILabel 수백 개를 만들지 않고 IMGUI(CSCommonUX.Draw) 가상화 스크롤로 보이는 줄만 그려 렉 없이 즉시 열림.
    /// </summary>
    public static class CSCommonDialog
    {
        const float W = CSCommonUX.Modal.Width, MaxH = 660f, Pad = CSCommonUX.Modal.PadX, HeadH = CSCommonUX.Modal.HeadH, FootH = CSCommonUX.Modal.FootH, ScrollBarW = 10f;

        static UIPanel win;
        static DialogHost host;

        /// <summary>모달 창이 열려 있는지(설정 패널이 뒤에서 사라지지 않고 배경으로 남게).</summary>
        public static bool IsOpen => win != null && host != null;

        enum CatKind { New = 0, Improved = 1, Fixed = 2 }

        static readonly string[] CatTitles = { "신규", "개선", "수정" };

        static Color CatColor(CatKind k) => k switch
        {
            CatKind.New => Pal.Teal,
            CatKind.Improved => Pal.Ok,
            _ => Pal.Warn,
        };

        enum RowKind { ReleaseHead, CategoryHead, Bullet, Paragraph, Spacer }

        struct LayoutRow
        {
            public RowKind Kind;
            public string Text, SubText;
            public CatKind Cat;
            public int Depth, Count;
            public bool Numbered, FirstRelease;
            public float Y, Height;
        }

        /// <summary>업데이트 내역: since보다 새 판만(0.0이면 전부). dontShowAgain이 있으면 '다시 보지 않기' 버튼.</summary>
        public static void Changelog(string title, CSCommonRelease[] releases, Version since, Action dontShowAgain, int maxLines = 0)
        {
            var rawRows = new List<LayoutRow>(256);
            var buckets = new List<string>[3] { new(), new(), new() };
            bool firstRel = true;

            if (releases != null)
            {
                foreach (var r in releases)
                {
                    if (r == null || (since != null && r.Version <= since)) continue;
                    buckets[0].Clear();
                    buckets[1].Clear();
                    buckets[2].Clear();

                    if (r.Lines != null)
                    {
                        foreach (string raw in r.Lines)
                        {
                            if (string.IsNullOrEmpty(raw)) continue;
                            CatKind cat = ClassifyLine(raw, out string cleaned);
                            if (!string.IsNullOrEmpty(cleaned))
                                buckets[(int)cat].Add(cleaned);
                        }
                    }

                    if (!firstRel)
                        rawRows.Add(new LayoutRow { Kind = RowKind.Spacer, Height = 12f });

                    rawRows.Add(new LayoutRow
                    {
                        Kind = RowKind.ReleaseHead,
                        Text = "v" + r.Version,
                        SubText = r.Date ?? "",
                        FirstRelease = firstRel,
                    });
                    firstRel = false;

                    for (int c = 0; c < 3; c++)
                    {
                        var list = buckets[c];
                        if (list.Count == 0) continue;
                        rawRows.Add(new LayoutRow
                        {
                            Kind = RowKind.CategoryHead,
                            Cat = (CatKind)c,
                            Text = CatTitles[c],
                            Count = list.Count,
                        });
                        for (int i = 0; i < list.Count; i++)
                        {
                            ParseBullet(list[i], out int depth, out bool numbered, out string body);
                            rawRows.Add(new LayoutRow
                            {
                                Kind = RowKind.Bullet,
                                Cat = (CatKind)c,
                                Depth = depth,
                                Numbered = numbered,
                                Text = body,
                            });
                        }
                    }
                }
            }

            Open(title + " · 업데이트 내역", rawRows, dontShowAgain);
        }

        /// <summary>일반 알림: 문단들 · 점 목록.</summary>
        public static void Notice(string title, string[] paragraphs, string[] list)
        {
            var rawRows = new List<LayoutRow>(32);
            if (paragraphs != null)
            {
                foreach (string p in paragraphs)
                {
                    if (string.IsNullOrEmpty(p)) continue;
                    rawRows.Add(new LayoutRow { Kind = RowKind.Paragraph, Text = p });
                }
            }
            if (list != null)
            {
                foreach (string item in list)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    ParseBullet(item, out int depth, out bool numbered, out string body);
                    rawRows.Add(new LayoutRow
                    {
                        Kind = RowKind.Bullet,
                        Cat = CatKind.Improved,
                        Depth = depth,
                        Numbered = numbered,
                        Text = body,
                    });
                }
            }
            Open(title, rawRows, null);
        }

        static void ParseBullet(string line, out int depth, out bool numbered, out string text)
        {
            line ??= "";
            depth = 0;
            while (line.StartsWith("  ", StringComparison.Ordinal))
            {
                line = line.Substring(2);
                depth++;
            }
            numbered = Regex.IsMatch(line, @"^\d+\.\s");
            text = line;
        }

        /// <summary>
        /// 업데이트 내역 문구를 신규 / 개선 / 수정 3대 카테고리로 자동 분류.
        /// 명시적 머리말([신규]/[개선]/[수정], 신규:/개선:/수정:)이 있으면 우선 따르고, 없으면 문맥 키워드로 판별.
        /// </summary>
        static CatKind ClassifyLine(string line, out string cleaned)
        {
            cleaned = (line ?? "").Trim();
            if (cleaned.Length == 0) return CatKind.Improved;

            if (cleaned.StartsWith("[신규]", StringComparison.Ordinal)) { cleaned = cleaned.Substring(4).TrimStart(); return CatKind.New; }
            if (cleaned.StartsWith("[개선]", StringComparison.Ordinal)) { cleaned = cleaned.Substring(4).TrimStart(); return CatKind.Improved; }
            if (cleaned.StartsWith("[수정]", StringComparison.Ordinal)) { cleaned = cleaned.Substring(4).TrimStart(); return CatKind.Fixed; }
            if (cleaned.StartsWith("신규:", StringComparison.Ordinal)) { cleaned = cleaned.Substring(3).TrimStart(); return CatKind.New; }
            if (cleaned.StartsWith("개선:", StringComparison.Ordinal)) { cleaned = cleaned.Substring(3).TrimStart(); return CatKind.Improved; }
            if (cleaned.StartsWith("수정:", StringComparison.Ordinal)) { cleaned = cleaned.Substring(3).TrimStart(); return CatKind.Fixed; }

            // 콜론(:) 또는 대시(—) 앞 머리말 추출
            int colon = cleaned.IndexOf(':');
            int dash = cleaned.IndexOf(" — ", StringComparison.Ordinal);
            int cut = colon > 0 && (dash < 0 || colon < dash) ? colon : dash;
            string head = cut > 0 ? cleaned.Substring(0, cut) : cleaned;

            // 1) 수정(Fixed): 버그 · 오류 · 결함 · 고침 · 해결 · 누락 · ~던 것/현상/문제
            if (ContainsAny(head, "고침", "수정", "해결", "바로잡음", "버그", "오류", "결함", "누락", "교착", "원복", "복구", "되돌림", "완화") ||
                ContainsAny(cleaned, "고침", "문제 수정", "현상 수정", "버그 수정", "결함 해결", "전면 해결", "전면 수정", "완전 해결", "완전 해소",
                    "교착 현상", "멈추던", "겹치던", "찌그러지던", "쏠리던", "끊기던", "뒤집히던", "안 되던", "안 보이던", "못 하던", "먹지 않던",
                    "남던 것", "나오던 것", "보이던 것", "밀리던 것", "튀던 것", "오가던 것", "사라졌음", "못 찾았음", "흐트러졌음"))
            {
                return CatKind.Fixed;
            }

            // 2) 신규(New): 신설 · 새로 · 새 탭 · 추가 · 도입 · 탑재 · 구현 · 첫 정식 판 · 릴리즈
            if (ContainsAny(head, "신설", "새로", "새 탭", "새 규격", "추가", "도입", "탑재", "구현", "첫 정식 판", "릴리즈", "수립", "개발 시작", "전격 구현", "전격 탑재", "분리 탑재", "구축") ||
                cleaned.StartsWith("새 ", StringComparison.Ordinal))
            {
                return CatKind.New;
            }

            // 3) 개선(Improved): 그 외 모든 품질 · 디자인 · 주행 · UI · 성능 · 동기화 개선
            return CatKind.Improved;
        }

        static bool ContainsAny(string text, params string[] words)
        {
            if (string.IsNullOrEmpty(text)) return false;
            for (int i = 0; i < words.Length; i++)
                if (text.IndexOf(words[i], StringComparison.Ordinal) >= 0) return true;
            return false;
        }

        static void Open(string title, List<LayoutRow> rows, Action dontShowAgain)
        {
            try
            {
                Close();
                var view = UIView.GetAView();
                if (view == null) return;
                Vector2 screen = view.GetScreenResolution();

                // 모달 입력 차단용 투명 UIPanel (실제 그리기는 DialogHost.OnGUI 에서 CSCommonUX.Draw 로 수행)
                win = (UIPanel)view.AddUIComponent(typeof(UIPanel));
                win.backgroundSprite = "";
                win.color = new Color32(0, 0, 0, 0);
                win.size = screen;
                win.relativePosition = Vector3.zero;
                win.isInteractive = true;
                win.canFocus = true;

                host = win.gameObject.AddComponent<DialogHost>();
                host.Init(title, rows, dontShowAgain);

                UIView.PushModal(win);
                win.BringToFront();
                win.Focus();
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
            var gone = win;
            win = null;
            host = null;
            if (gone != null)
            {
                gone.isInteractive = false;
                UnityEngine.Object.Destroy(gone.gameObject);
            }
        }

        /// <summary>
        /// 한글 대체 글꼴(맑은 고딕 Regular)과 영문/숫자 모두에서 확실한 볼드체가 보이도록
        /// 기본 bold 패스 + 0.65px 가로 오프셋 이중 패스로 굵게 그림.
        /// </summary>
        static void DrawBoldText(Rect r, string text, float size, Color color, TextAnchor anchor = TextAnchor.MiddleLeft, bool wrap = false)
        {
            if (!Draw.Repaint || string.IsNullOrEmpty(text)) return;
            Draw.Text(r, text, size, color, true, anchor, wrap);
            Draw.Text(new Rect(r.x + 0.65f, r.y, r.width, r.height), text, size, color, true, anchor, wrap);
        }

        /// <summary>버전 번호 볼드 뱃지(v2.1.0 등): 알약 바탕 + 테두리 + 이중 패스 볼드 텍스트.</summary>
        static float DrawBoldVersionBadge(float x, float cy, string verText, Color color)
        {
            const float font = 13f, h = 25f;
            float tw = Mathf.Ceil(Draw.TextWidth(verText, font, true)) + 2f;
            float w = Mathf.Max(60f, tw + 22f);
            if (Draw.Repaint)
            {
                var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
                Draw.Round(r, Draw.Alpha(color, 0.18f), h * 0.5f);
                Draw.RoundOutline(r, Draw.Alpha(color, 0.48f), h * 0.5f);
                DrawBoldText(r, verText, font, color, TextAnchor.MiddleCenter);
            }
            return w;
        }

        /// <summary>대분류(신규 · 개선 · 수정) 볼드 헤더: 좌측 컬러 막대 + 알약 배지(볼드) + 항목 수.</summary>
        static void DrawCategoryHeader(Rect r, CatKind cat, string label, int count)
        {
            if (!Draw.Repaint) return;
            Color c = CatColor(cat);
            float cy = r.center.y;
            const float pillH = 22f, font = 13f;
            float tw = Mathf.Ceil(Draw.TextWidth(label, font, true)) + 2f;
            float pillW = tw + 20f;

            // 좌측 포인트 바 + 볼드 대분류 알약
            Draw.Round(new Rect(r.x, Mathf.Round(cy - 8f), 3.5f, 16f), c, 1.5f);
            var pill = new Rect(r.x + 9f, Mathf.Round(cy - pillH * 0.5f), pillW, pillH);
            Draw.Round(pill, Draw.Alpha(c, 0.16f), 6f);
            Draw.RoundOutline(pill, Draw.Alpha(c, 0.40f), 6f);
            DrawBoldText(pill, label, font, c, TextAnchor.MiddleCenter);

            // 항목 수 표시 + 우측 옅은 구분선
            string cntStr = count + "건";
            float cw = Mathf.Ceil(Draw.TextWidth(cntStr, 11.5f, true)) + 4f;
            float cx = pill.xMax + 8f;
            Draw.Text(new Rect(cx, r.y, cw, r.height), cntStr, 11.5f, Pal.Sub, true, TextAnchor.MiddleLeft);
            float lineX = cx + cw + 8f;
            if (lineX < r.xMax - 8f)
                Draw.Fill(new Rect(lineX, Mathf.Round(cy), r.xMax - lineX, 1f), Draw.Alpha(c, 0.18f));
        }

        sealed class DialogHost : MonoBehaviour
        {
            string title = "";
            List<LayoutRow> rows = new();
            Action dontShowAgain;

            float measuredScale = -1f;
            float measuredBoost = -1f;
            float contentH;
            float scroll;
            float targetScroll;
            float openTime;
            bool draggingBar;
            float dragGrabOffset;

            public void Init(string dialogTitle, List<LayoutRow> dialogRows, Action onDontShowAgain)
            {
                title = dialogTitle ?? "";
                rows = dialogRows ?? new List<LayoutRow>();
                dontShowAgain = onDontShowAgain;
                measuredScale = -1f;
                scroll = 0f;
                targetScroll = 0f;
                openTime = Time.realtimeSinceStartup;
            }

            void EnsureMeasured(float bodyWidth)
            {
                if (Mathf.Approximately(measuredScale, Draw.S) && Mathf.Approximately(measuredBoost, Draw.TextBoost))
                    return;
                measuredScale = Draw.S;
                measuredBoost = Draw.TextBoost;

                float y = 4f;
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    row.Y = y;
                    switch (row.Kind)
                    {
                        case RowKind.Spacer:
                            y += row.Height;
                            break;
                        case RowKind.ReleaseHead:
                            row.Height = 32f;
                            y += row.Height + 6f;
                            break;
                        case RowKind.CategoryHead:
                            y += 4f;
                            row.Y = y;
                            row.Height = 26f;
                            y += row.Height + 5f;
                            break;
                        case RowKind.Paragraph:
                        {
                            float ph = Mathf.Max(20f, Draw.TextHeight(row.Text, 14f, bodyWidth - 4f));
                            row.Height = ph;
                            y += ph + 10f;
                            break;
                        }
                        case RowKind.Bullet:
                        {
                            // 대분류 아래 들여쓰기(8px) + 깊이별 들여쓰기
                            float indent = 8f + row.Depth * 18f;
                            float textX = row.Numbered ? indent : indent + 15f;
                            float availW = Mathf.Max(120f, bodyWidth - textX - 4f);
                            float th = Mathf.Max(19f, Draw.TextHeight(row.Text, 13f, availW));
                            row.Height = th;
                            y += th + 6f;
                            break;
                        }
                    }
                    rows[i] = row;
                }
                contentH = Mathf.Max(40f, y + 6f);
            }

            void Update()
            {
                // 부드러운 휠 관성 스크롤 보간 (드래그 중에는 즉시 추종)
                if (!draggingBar && !Mathf.Approximately(scroll, targetScroll))
                {
                    float dt = Mathf.Min(0.05f, Time.unscaledDeltaTime);
                    scroll = Mathf.Lerp(scroll, targetScroll, 1f - Mathf.Exp(-22f * dt));
                    if (Mathf.Abs(scroll - targetScroll) < 0.4f) scroll = targetScroll;
                }
            }

            void OnGUI()
            {
                if (win == null) return;
                try
                {
                    // 설정 패널 · 메인 창보다 항상 위에 그림
                    GUI.depth = -100;
                    Draw.Begin();
                    // 맨 위 대화 상자: 툴바 · 독 칸(Draw.Block)에 막히지 않음(다음 Draw.Begin 이 깊이를 0 으로)
                    Draw.BeginOverlay();

                    float bodyInnerW = W - Pad * 2f - ScrollBarW - 8f;
                    EnsureMeasured(bodyInnerW);

                    float maxBodyH = MaxH - HeadH - CSCommonUX.Modal.BodyTop - CSCommonUX.Modal.BodyBottom - FootH;
                    float bodyH = Mathf.Clamp(contentH, 60f, maxBodyH);
                    float winH = HeadH + CSCommonUX.Modal.BodyTop + bodyH + CSCommonUX.Modal.BodyBottom + FootH;
                    float maxScroll = Mathf.Max(0f, contentH - bodyH);
                    targetScroll = Mathf.Clamp(targetScroll, 0f, maxScroll);
                    scroll = Mathf.Clamp(scroll, 0f, maxScroll);

                    // 등장 애니메이션(CSCommonUX.Modal: 0.18초 · 시안 곡선 · 10 아래에서 떠오름)
                    float age = Time.realtimeSinceStartup - openTime;
                    float ease = CSCommonUX.Modal.Ease(age / CSCommonUX.Modal.OpenSeconds);
                    float oldOp = Draw.Opacity;
                    Draw.Opacity = oldOp * ease;

                    var box = new Rect(
                        Mathf.Round((Draw.Width - W) * 0.5f),
                        Mathf.Round((Draw.Height - winH) * 0.5f + (1f - ease) * CSCommonUX.Modal.Rise),
                        W,
                        winH);

                    DrawModal(box, bodyH, bodyInnerW, maxScroll);

                    Draw.Opacity = oldOp;
                    Draw.EndInput();
                }
                catch (Exception e)
                {
                    CSCommonLog.Exception(e, "알림 창 그리기");
                }
            }

            void DrawModal(Rect box, float bodyH, float bodyInnerW, float maxScroll)
            {
                Event e = Event.current;

                // 키보드 스크롤 및 Esc 닫기 처리
                if (e.type == EventType.KeyDown)
                {
                    switch (e.keyCode)
                    {
                        case KeyCode.Escape:
                            e.Use();
                            Close();
                            return;
                        case KeyCode.DownArrow:
                            targetScroll = Mathf.Clamp(targetScroll + 48f, 0f, maxScroll);
                            e.Use();
                            break;
                        case KeyCode.UpArrow:
                            targetScroll = Mathf.Clamp(targetScroll - 48f, 0f, maxScroll);
                            e.Use();
                            break;
                        case KeyCode.PageDown:
                            targetScroll = Mathf.Clamp(targetScroll + bodyH * 0.85f, 0f, maxScroll);
                            e.Use();
                            break;
                        case KeyCode.PageUp:
                            targetScroll = Mathf.Clamp(targetScroll - bodyH * 0.85f, 0f, maxScroll);
                            e.Use();
                            break;
                        case KeyCode.Home:
                            targetScroll = 0f;
                            e.Use();
                            break;
                        case KeyCode.End:
                            targetScroll = maxScroll;
                            e.Use();
                            break;
                    }
                }

                // 덮개 · 판 · 머리(제목 · 원 닫기) · 발 띠(CSCommonUX.Modal)
                CSCommonUX.Modal.Frame(box, title, null, out bool closed, true);
                if (closed)
                {
                    Close();
                    return;
                }

                // 본문 스크롤 영역
                var bodyRect = new Rect(box.x + Pad, box.y + HeadH + CSCommonUX.Modal.BodyTop, box.width - Pad * 2f, bodyH);
                var barTrack = new Rect(bodyRect.xMax - ScrollBarW, bodyRect.y, ScrollBarW, bodyH);

                // 마우스 휠 스크롤 (가속 · 부드러운 감속)
                if (e.type == EventType.ScrollWheel && maxScroll > 0f && Draw.Hover(box))
                {
                    targetScroll = Mathf.Clamp(targetScroll + e.delta.y * 52f, 0f, maxScroll);
                    e.Use();
                }

                // 스크롤바 마우스 드래그 · 트랙 클릭 이동
                if (maxScroll > 0f)
                {
                    float thumbH = Mathf.Max(32f, bodyH * bodyH / contentH);
                    float travel = Mathf.Max(1f, bodyH - thumbH);
                    float thumbY = barTrack.y + travel * (scroll / maxScroll);
                    var thumbRect = new Rect(barTrack.x, thumbY, barTrack.width, thumbH);

                    if (e.type == EventType.MouseDown && e.button == 0 && Draw.Hover(barTrack))
                    {
                        draggingBar = true;
                        if (Draw.Hover(thumbRect))
                        {
                            dragGrabOffset = Draw.Mouse.y - thumbY;
                        }
                        else
                        {
                            dragGrabOffset = thumbH * 0.5f;
                            float ratio = Mathf.Clamp01((Draw.Mouse.y - barTrack.y - dragGrabOffset) / travel);
                            scroll = targetScroll = ratio * maxScroll;
                        }
                        e.Use();
                    }
                    else if (e.type == EventType.MouseDrag && draggingBar)
                    {
                        float ratio = Mathf.Clamp01((Draw.Mouse.y - barTrack.y - dragGrabOffset) / travel);
                        scroll = targetScroll = ratio * maxScroll;
                        e.Use();
                    }
                    else if (e.rawType == EventType.MouseUp && draggingBar)
                    {
                        draggingBar = false;
                        e.Use();
                    }
                }

                // 가상화 본문 렌더링: 현재 화면(scroll ~ scroll + bodyH)에 걸치는 줄만 그림
                Draw.BeginClip(bodyRect, new Vector2(0f, Mathf.Round(scroll)));
                if (Draw.Repaint)
                {
                    float viewTop = scroll - 24f;
                    float viewBottom = scroll + bodyH + 24f;
                    for (int i = 0; i < rows.Count; i++)
                    {
                        var r = rows[i];
                        if (r.Y + r.Height < viewTop) continue;
                        if (r.Y > viewBottom) break;
                        DrawRow(r, bodyInnerW);
                    }
                }
                Draw.EndClip();

                // 스크롤바 렌더링 (호버/드래그 시 넓어지고 밝아짐)
                if (maxScroll > 0f && Draw.Repaint)
                {
                    bool barHover = draggingBar || Draw.Hover(barTrack);
                    float thumbH = Mathf.Max(32f, bodyH * bodyH / contentH);
                    float travel = Mathf.Max(1f, bodyH - thumbH);
                    float thumbY = barTrack.y + travel * (scroll / maxScroll);
                    float barW = barHover ? 6f : 4f;
                    float barX = barTrack.x + (barTrack.width - barW) * 0.5f;
                    Draw.Round(new Rect(barX, barTrack.y, barW, barTrack.height), Draw.Alpha(Look.Tone0, 0.55f), barW * 0.5f);
                    Color thumbCol = draggingBar ? Pal.Primary : barHover ? Pal.Sub : Pal.Line2;
                    Draw.Round(new Rect(barX, thumbY, barW, thumbH), thumbCol, barW * 0.5f);
                }

                // 발 단추: 오른쪽부터 [닫기 = 파스텔 알약] [다시 보지 않기 = 회색 알약]
                float right = 0f;
                if (CSCommonUX.Modal.FootButton(box, ref right, Icons.Check, "닫기", true))
                {
                    Close();
                    return;
                }
                if (dontShowAgain != null && CSCommonUX.Modal.FootButton(box, ref right, null, "다시 보지 않기"))
                {
                    var cb = dontShowAgain;
                    cb();
                    Close();
                    return;
                }

                // 모달 밖/안의 남은 마우스 이벤트를 소비해 뒤쪽 창이 눌리지 않게 보호
                if (e.type == EventType.MouseDown || e.type == EventType.MouseUp || e.type == EventType.MouseDrag || e.type == EventType.ScrollWheel)
                    e.Use();
            }

            static void DrawRow(LayoutRow row, float width)
            {
                switch (row.Kind)
                {
                    case RowKind.ReleaseHead:
                    {
                        if (!row.FirstRelease)
                            Draw.Fill(new Rect(0f, row.Y - 6f, width, 1f), Look.Divider);
                        float cy = row.Y + row.Height * 0.5f;
                        float bw = DrawBoldVersionBadge(0f, cy, row.Text, Pal.PrimaryInk);
                        if (!string.IsNullOrEmpty(row.SubText))
                        {
                            Draw.Text(new Rect(bw + 10f, row.Y, 180f, row.Height), row.SubText, 12.5f, Pal.Sub, true, TextAnchor.MiddleLeft);
                        }
                        break;
                    }
                    case RowKind.CategoryHead:
                    {
                        DrawCategoryHeader(new Rect(0f, row.Y, width, row.Height), row.Cat, row.Text, row.Count);
                        break;
                    }
                    case RowKind.Paragraph:
                    {
                        Draw.Text(new Rect(0f, row.Y, width - 4f, row.Height), row.Text, 14f, Pal.Txt, false, TextAnchor.UpperLeft, true);
                        break;
                    }
                    case RowKind.Bullet:
                    {
                        float indent = 8f + row.Depth * 18f;
                        float textX = row.Numbered ? indent : indent + 15f;
                        if (!row.Numbered)
                        {
                            Color dotCol = CatColor(row.Cat);
                            float r = 13f * 0.20f;
                            var c = new Vector2(indent + 4f, row.Y + 9.5f);
                            if (row.Depth % 3 == 1) Draw.Ring(c, r, 1.3f, dotCol);
                            else if (row.Depth % 3 == 2) Draw.Fill(new Rect(c.x - r * 0.85f, c.y - r * 0.85f, r * 1.7f, r * 1.7f), dotCol);
                            else Draw.Dot(c, r, dotCol);
                        }
                        Draw.Text(new Rect(textX, row.Y, width - textX - 4f, row.Height + 2f), row.Text, 13f, Pal.Light, false, TextAnchor.UpperLeft, true);
                        break;
                    }
                }
            }
        }
    }
}
