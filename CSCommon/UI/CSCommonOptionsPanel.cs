using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX.UI
{
    /// <summary>
    /// 게임 설정 화면 안 모드 패널(디자인 아티팩트 '게임 설정 메뉴 · F1 모드 옵션' 보드).
    /// 게임 UI 패널은 자리만 잡고(투명 · 클릭 받기), 내용은 그 화면 자리에 IMGUI CSCommon 컴포넌트로 그림 —
    /// F1 창과 같은 부품이라 모양이 늘 같음(게임 UI 는 색 알파를 버려 따로 흉내 내면 어긋났음).
    /// 머리(로고 · 이름 · 판 · 상태 · 업데이트 내역) → CSCommonUX.Tabs → 탭마다 스크롤되는 카드 묶음.
    /// 이어받는 쪽은 Build 에서 AddPage · page.Section · section.Toggle/Steps/Slider/Button/Info/Note 로 짓고, 값은 그릴 때마다 읽음.
    /// </summary>
    public abstract class CSCommonOptionsPanel : UIPanel
    {
        const float Pad = CSCommonDesign.PanelPad, HeadH = CSCommonDesign.PanelHeadH, TabBarH = CSCommonUX.Tabs.H + CSCommonUX.Tabs.Pad * 2f, Gap = CSCommonDesign.PanelGap;

        readonly List<CSCommonPage> pages = new();
        int cur;
        Action rebuild;

        /// <summary>머리 왼쪽 로고(없으면 LogoIcon, 둘 다 없으면 비움).</summary>
        protected virtual Texture Logo => null;

        /// <summary>Logo 가 없을 때 머리 왼쪽에 그릴 아이콘(SVG path, viewBox 24). 둘 다 없으면 자리를 비움.</summary>
        protected virtual string LogoIcon => null;

        protected virtual string Title => CSCommonModBase.Instance?.BaseName ?? "";

        protected abstract void Build();

        internal void Setup(float width, float height, Action rebuildPanel)
        {
            rebuild = rebuildPanel;
            size = new Vector2(width, height);
            autoLayout = false;
            isInteractive = true;
            try
            {
                Build();
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "설정 패널 내용 짓기");
            }
        }

        /// <summary>탭 하나(스크롤되는 카드 묶음).</summary>
        protected CSCommonPage AddPage(string name)
        {
            var page = new CSCommonPage(name);
            pages.Add(page);
            return page;
        }

        /// <summary>패널을 새로 지음(값이 바뀌어 줄 모양이 달라질 때).</summary>
        protected void RebuildPanel() => rebuild?.Invoke();

        /// <summary>게임 UI 자리 → IMGUI 논리 좌표(화면 높이 1080 기준).</summary>
        Rect ScreenRect()
        {
            var cam = GetUIView()?.uiCamera;
            if (cam == null) return default;
            // 네 귀퉁이 차례가 게임마다 달라(3 = 왼쪽 아래였음 → 폭 0) 넷 다 보고 가장 바깥을 씀
            float x0 = float.MaxValue, x1 = float.MinValue, sy0 = float.MaxValue, sy1 = float.MinValue;
            foreach (var corner in GetCorners())
            {
                Vector3 p = cam.WorldToScreenPoint(corner);
                x0 = Mathf.Min(x0, p.x); x1 = Mathf.Max(x1, p.x);
                sy0 = Mathf.Min(sy0, p.y); sy1 = Mathf.Max(sy1, p.y);
            }
            float y0 = Screen.height - sy1, y1 = Screen.height - sy0;
            return new Rect(x0 / Draw.S, y0 / Draw.S, (x1 - x0) / Draw.S, (y1 - y0) / Draw.S);
        }

        /// <summary>맨 위 모달이 설정 화면(이 패널의 조상)일 때만 — 알림 창 · 확인 창이 뜨면 가리지 않게 쉼.</summary>
        bool OnTop()
        {
            var modal = UIView.GetModalComponent();
            if (modal == null) return true;
            for (UIComponent p = this; p != null; p = p.parent)
                if (p == modal) return true;
            return false;
        }

        void OnGUI()
        {
            bool top = OnTop();
            if (!isVisible || pages.Count == 0 || (!top && !CSCommonDialog.IsOpen)) return;
            if (!top && Event.current != null && Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout) return;
            try
            {
                Draw.Begin();
                var r = ScreenRect();
                if (r.width < 100f || r.height < 100f) return;
                CSCommonToolbar.ClearTip();
                DrawPanel(r);
                CSCommonUX.Select.Flush();
                CSCommonToolbar.DrawTip();
                Draw.EndInput();
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, "설정 패널 그리기");
            }
        }

        void DrawPanel(Rect r)
        {
            if (Draw.Repaint)
            {
                Draw.Round(r, Pal.Win, CSCommonDesign.RadiusWindow);
            }
            DrawHead(new Rect(r.x, r.y, r.width, HeadH));

            float ty = r.y + HeadH + 3f + 12f;
            var labels = new string[pages.Count];
            for (int i = 0; i < pages.Count; i++) labels[i] = pages[i].Name;
            CSCommonUX.Tabs.At(r.x + Pad, ty + TabBarH * 0.5f, null, labels, cur, null, null, null, true, i =>
            {
                cur = i;
                CSCommonUX.Select.Close();
            });

            float top = ty + TabBarH + 12f;
            // CSCommonUX.Section: 좌우 · 아래 PanelPad 안쪽에 우물(바탕 창 띠) — 그 안에 플레이트
            var view = new Rect(r.x + Pad, top, r.width - Pad * 2f, r.yMax - Pad - top);
            pages[cur].Draw(view);
        }

        void DrawHead(Rect h)
        {
            var mod = CSCommonModBase.Instance;
            float cy = h.center.y, x = h.x + 20f;
            if (Draw.Repaint)
            {
                var logo = Logo;
                if (logo != null)
                {
                    float lh = logo.width > 0 ? Mathf.Min(24f, 80f * logo.height / logo.width) : 20f;
                    Draw.Image(new Rect(x, cy - lh * 0.5f, 80f, lh), logo);
                }
                else if (!string.IsNullOrEmpty(LogoIcon))
                {
                    var tile = new Rect(x, cy - CSCommonDesign.PanelMarkSize * 0.5f, CSCommonDesign.PanelMarkSize, CSCommonDesign.PanelMarkSize);
                    Draw.Round(tile, Draw.Alpha(Pal.Primary, 0.14f), CSCommonDesign.PanelMarkRadius);
                    float ip = (CSCommonDesign.PanelMarkSize - CSCommonDesign.PanelMarkIcon) * 0.5f;
                    Draw.Icon(LogoIcon, new Rect(tile.x + ip, tile.y + ip, CSCommonDesign.PanelMarkIcon, CSCommonDesign.PanelMarkIcon), Pal.Primary, 24f, 2f);
                }
                // UX 1.1: 머리 띠 = 모드 주 색(라이트 모드엔 Accent 가 파랑이라 청록 모드와 안 맞았음)
                Draw.Round(new Rect(h.x, h.yMax, h.width, 3f), Pal.Primary, 0f);
            }
            // 로고 없는 모드는 자리를 비우지 않음(예전엔 모든 모드에 'F1' 글이 찍혔음)
            if (Logo != null) x += 80f + 12f;
            else if (!string.IsNullOrEmpty(LogoIcon)) x += CSCommonDesign.PanelMarkSize + 12f;
            float tw = Mathf.Ceil(Draw.TextWidth(Title, 17f, true));
            if (Draw.Repaint) Draw.Text(new Rect(x, h.y, tw + 2f, h.height), Title, 17f, Pal.Txt, true);
            x += tw + 12f;
            if (mod != null)
            {
                bool dev = mod.Channel != "안정";
                x += CSCommonUX.Badge.At(x, cy, mod.VersionText + " · " + mod.Channel, dev ? Pal.Warn : Pal.DoneInk) + 8f;
                bool ok = mod.Patched;
                CSCommonUX.Badge.At(x, cy, ok ? "정상" : "Harmony 없음", ok ? Pal.Teal : Pal.Warn);
                float bw = CSCommonUX.Button.Width(Icons.Info, "업데이트 내역", true);
                CSCommonUX.Button.In(new Rect(h.xMax - 16f - bw, cy - CSCommonDesign.ActionHeightSmall * 0.5f, bw, CSCommonDesign.ActionHeightSmall),
                    Icons.Info, "업데이트 내역", null, Tone.Neutral, null, mod.Releases != null, CSCommonChangelog.ShowAll);
            }
        }
    }

    /// <summary>탭 하나: 우물(CSCommonUX.Section 바탕 창 띠) 안에 안내 글 · 플레이트를 위에서 아래로 쌓고 스크롤.</summary>
    public sealed class CSCommonPage
    {
        public readonly string Name;
        readonly List<object> items = new();
        float scroll, contentH;

        internal CSCommonPage(string name) => Name = name;

        /// <summary>플레이트 하나(제목 칩 · 설명 · 줄). top = 예전 제목 앞 막대 색(새 디자인엔 없음 — 호출 모양만 지킴).</summary>
        public CSCommonSection Section(string title, string desc = null, Color? top = null)
        {
            var s = new CSCommonSection(title, desc, top);
            items.Add(s);
            return s;
        }

        /// <summary>플레이트 밖 안내 글.</summary>
        public void Note(string text) => items.Add(text);

        internal void Draw(Rect view)
        {
            float pad = CSCommonUX.Section.WellPad, gap = CSCommonUX.Section.Gap;
            var inner = new Rect(view.x + pad, view.y + pad, view.width - pad * 2f, view.height - pad * 2f);
            float w = inner.width;
            var e = Event.current;
            if (e.type == EventType.ScrollWheel && CSCommonUX.Draw.Hover(view))
            {
                scroll += e.delta.y * 20f;
                CSCommonUX.Select.Close();
                e.Use();
            }
            scroll = Mathf.Clamp(scroll, 0f, Mathf.Max(0f, contentH - inner.height));

            CSCommonUX.Section.Well(view);
            CSCommonUX.Draw.BeginClip(inner, new Vector2(0f, scroll));
            float y = 0f;
            foreach (var it in items)
            {
                if (it is string note)
                {
                    float nh = CSCommonUX.Draw.TextHeight(note, 12f, w - CSCommonUX.Section.Pad * 2f);
                    if (CSCommonUX.Draw.Repaint)
                        CSCommonUX.Draw.Text(new Rect(CSCommonUX.Section.Pad, y, w - CSCommonUX.Section.Pad * 2f, nh), note, 12f, Pal.Sub, false, TextAnchor.UpperLeft, true);
                    y += nh + gap;
                }
                else if (it is CSCommonSection s)
                {
                    float sh = s.Measure(w);
                    // 보이는 칸만 그림(재기는 늘)
                    if (y + sh >= scroll - 40f && y <= scroll + inner.height + 40f) s.Draw(new Rect(0f, y, w, sh));
                    y += sh + gap;
                }
            }
            contentH = Mathf.Max(0f, y - gap);
            CSCommonUX.Draw.EndClip();

            // 가는 스크롤 손잡이(넘칠 때만)
            if (contentH > inner.height && CSCommonUX.Draw.Repaint)
            {
                float th = Mathf.Max(30f, inner.height * inner.height / contentH);
                float t = scroll / (contentH - inner.height);
                CSCommonUX.Draw.Round(new Rect(view.xMax - 6f, inner.y + (inner.height - th) * t, 4f, th), Pal.Line2, 2f);
            }
        }
    }

    /// <summary>
    /// 섹션 하나(CSCommonUX.Section 플레이트): 제목 = 파스텔 칩(표시용) · 설명 · 줄. 줄 = 알약 행(최소 44 · 호버 Field 면 · 헤어라인 없음),
    /// 왼쪽 = 이름(14) · 설명(12, 흐림), 오른쪽 = 부품.
    /// </summary>
    public sealed class CSCommonSection
    {
        const float Pad = CSCommonUX.Section.Pad, RowPad = CSCommonDesign.CardRowPad, RowIn = CSCommonUX.Section.RowPadR;

        enum Kind { Toggle, Steps, Slider, Button, Info, Note }

        sealed class Row
        {
            public Kind Kind;
            public string Label, Desc;
            public Func<bool> GetBool;
            public Action<bool> SetBool;
            public string[] Steps;
            public Func<int> GetStep;
            public Action<int> SetStep;
            public float Min, Max, Step;
            public Func<float> GetFloat;
            public Action<float> SetFloat;
            public Func<float, string> Format;
            public string Text;
            public Action Act;
            public CSCommonButtonKind ButtonKind;
            public Func<bool> Enabled;
            public string Badge;
            public Color BadgeColor, NoteColor;
        }

        readonly string title, desc;
        readonly List<Row> rows = new();

        // bar = 예전 제목 앞 막대 색(새 플레이트엔 없음 — 호출 모양만 지킴)
        internal CSCommonSection(string title, string desc, Color? bar)
        {
            this.title = title;
            this.desc = desc;
        }

        public void Toggle(string label, string desc, Func<bool> get, Action<bool> set) =>
            rows.Add(new Row { Kind = Kind.Toggle, Label = label, Desc = desc, GetBool = get, SetBool = set });

        /// <summary>정해진 단계 중 하나(CSCommonUX.Select).</summary>
        public void Steps(string label, string desc, string[] steps, Func<int> get, Action<int> set) =>
            rows.Add(new Row { Kind = Kind.Steps, Label = label, Desc = desc, Steps = steps, GetStep = get, SetStep = set });

        /// <summary>끌기 막대(오른쪽 막대 · 굵은 값).</summary>
        public void Slider(string label, string desc, float min, float max, float step, Func<float> get, Func<float, string> format, Action<float> set) =>
            rows.Add(new Row { Kind = Kind.Slider, Label = label, Desc = desc, Min = min, Max = max, Step = step, GetFloat = get, Format = format, SetFloat = set });

        public void Button(string label, string desc, string text, Action act, CSCommonButtonKind kind = CSCommonButtonKind.Outline, Func<bool> enabled = null) =>
            rows.Add(new Row { Kind = Kind.Button, Label = label, Desc = desc, Text = text, Act = act, ButtonKind = kind, Enabled = enabled });

        /// <summary>이름 · 오른쪽 값(· 색 배지).</summary>
        public void Info(string label, string value, string badge = null, Color? badgeColor = null) =>
            rows.Add(new Row { Kind = Kind.Info, Label = label, Text = value, Badge = badge, BadgeColor = badgeColor ?? Pal.Sub });

        public void Note(string text, Color? color = null) =>
            rows.Add(new Row { Kind = Kind.Note, Desc = text, NoteColor = color ?? Pal.Sub });

        static float RightWidth(Row r) => r.Kind switch
        {
            Kind.Toggle => 46f,
            Kind.Steps => 220f,
            Kind.Slider => 300f,
            Kind.Button => CSCommonUX.Button.Width(null, r.Text),
            Kind.Info => 220f,
            _ => 0f,
        };

        static float ControlHeight(Row r) => r.Kind switch
        {
            Kind.Toggle => 26f,
            Kind.Info => 24f,
            Kind.Note => 0f,
            _ => CSCommonDesign.ActionHeight,
        };

        // inner = 행 폭(알약) — 글 · 부품은 그 안 좌우 RowIn 안쪽
        float TextWidth(Row r, float inner)
        {
            float rw = RightWidth(r), w = inner - RowIn * 2f;
            return rw > 0f ? w - rw - 16f : w;
        }

        float TextHeight(Row r, float inner)
        {
            float lw = TextWidth(r, inner), h = 0f;
            if (r.Kind == Kind.Note) return CSCommonUX.Draw.TextHeight(r.Desc, 12f, lw);
            if (!string.IsNullOrEmpty(r.Label)) h = CSCommonUX.Draw.TextHeight(r.Label, 14f, lw);
            if (!string.IsNullOrEmpty(r.Desc)) h += 2f + CSCommonUX.Draw.TextHeight(r.Desc, 12f, lw);
            return h;
        }

        float RowHeight(Row r, float inner) => Mathf.Max(CSCommonUX.Section.RowH, Mathf.Max(TextHeight(r, inner), ControlHeight(r)) + RowPad * 2f);

        bool HasTitle => !string.IsNullOrEmpty(title);

        /// <summary>몸통(설명 + 줄) 높이.</summary>
        float BodyHeight(float inner)
        {
            float h = 0f;
            if (!string.IsNullOrEmpty(desc)) h += CSCommonUX.Draw.TextHeight(desc, 12f, inner - RowIn * 2f) + CSCommonUX.Section.HeadGap;
            foreach (var r in rows) h += RowHeight(r, inner);
            return h;
        }

        // 제목 칩 높이: 한 줄 32, 긴 제목은 글 한 단계 → 두 줄(Section.HeadHeight — 말줄임 없음)
        float Head(float inner) => HasTitle ? CSCommonUX.Section.HeadHeight(title, inner) + CSCommonUX.Section.HeadGap : 0f;

        internal float Measure(float width)
        {
            float inner = width - Pad * 2f;
            return Pad + Head(inner) + BodyHeight(inner) + CSCommonUX.Section.PadBottom;
        }

        internal void Draw(Rect card)
        {
            float inner = card.width - Pad * 2f, x = card.x + Pad;
            CSCommonUX.Section.Frame(card);
            float by = card.y + Pad;
            if (HasTitle)
            {
                CSCommonUX.Section.Title(new Rect(x, by, inner, CSCommonUX.Section.HeadHeight(title, inner)), title);
                by += Head(inner);
            }
            if (!string.IsNullOrEmpty(desc))
            {
                float dh = CSCommonUX.Draw.TextHeight(desc, 12f, inner - RowIn * 2f);
                if (CSCommonUX.Draw.Repaint) CSCommonUX.Draw.Text(new Rect(x + RowIn, by, inner - RowIn * 2f, dh), desc, 12f, Pal.Sub, false, TextAnchor.UpperLeft, true);
                by += dh + CSCommonUX.Section.HeadGap;
            }
            foreach (var r in rows)
            {
                float rh = RowHeight(r, inner);
                DrawRow(r, new Rect(x, by, inner, rh), inner);
                by += rh;
            }
        }

        void DrawRow(Row r, Rect pill, float inner)
        {
            // 알약 행(호버 면) — 글 · 부품은 좌우 RowIn 안쪽
            CSCommonUX.Section.Row(pill, r.Kind != Kind.Note && r.Kind != Kind.Info);
            var row = new Rect(pill.x + RowIn, pill.y, pill.width - RowIn * 2f, pill.height);
            float top = row.y + RowPad, body = row.height - RowPad * 2f, cy = top + body * 0.5f;
            float lw = TextWidth(r, inner);

            if (CSCommonUX.Draw.Repaint)
            {
                if (r.Kind == Kind.Note)
                    CSCommonUX.Draw.Text(new Rect(row.x, top, lw, body), r.Desc, 12f, r.NoteColor, false, TextAnchor.UpperLeft, true);
                else
                {
                    float th = TextHeight(r, inner), ty = cy - th * 0.5f;
                    if (!string.IsNullOrEmpty(r.Label))
                    {
                        float lh = CSCommonUX.Draw.TextHeight(r.Label, 14f, lw);
                        CSCommonUX.Draw.Text(new Rect(row.x, ty, lw, lh), r.Label, 14f, Pal.Txt, false, TextAnchor.UpperLeft, true);
                        ty += lh + 2f;
                    }
                    if (!string.IsNullOrEmpty(r.Desc))
                        CSCommonUX.Draw.Text(new Rect(row.x, ty, lw, CSCommonUX.Draw.TextHeight(r.Desc, 12f, lw)), r.Desc, 12f, Pal.Dim, false, TextAnchor.UpperLeft, true);
                }
            }

            float rw = RightWidth(r), rx = row.xMax - rw;
            switch (r.Kind)
            {
                case Kind.Toggle:
                {
                    bool on = r.GetBool();
                    if (CSCommonUX.Switch.In(new Rect(rx, Mathf.Round(cy - CSCommonDesign.SwitchH * 0.5f), CSCommonDesign.SwitchW, CSCommonDesign.SwitchH), on)) Safe(() => r.SetBool(!on), r.Label);
                    break;
                }
                case Kind.Steps:
                {
                    int cur = Mathf.Clamp(r.GetStep(), 0, r.Steps.Length - 1);
                    CSCommonUX.Select.In(new Rect(rx, Mathf.Round(cy - 20f), rw, 40f), r.Steps[cur], null, r.Steps, null, cur,
                        i => Safe(() => r.SetStep(i), r.Label));
                    break;
                }
                case Kind.Slider:
                {
                    // 시안 'CSCommonUX.Slider': 막대 + 오른쪽 값 알약(폭 64 · 높이 28 · Look.Pill · 12.5)
                    const float valueW = 64f, valueH = 28f, valueFont = 12.5f;
                    float v = r.GetFloat();
                    float nv = CSCommonUX.Slider.In(new Rect(rx, Mathf.Round(cy - 9f), rw - valueW - 12f, 18f), v, r.Min, r.Max, r.Step, Look.PastelStrong, out _, r.Format(v));
                    if (!Mathf.Approximately(nv, v)) Safe(() => r.SetFloat(nv), r.Label);
                    if (CSCommonUX.Draw.Repaint)
                    {
                        var vr = new Rect(row.xMax - valueW, Mathf.Round(cy - valueH * 0.5f), valueW, valueH);
                        CSCommonUX.Draw.Round(vr, Look.Pill, valueH * 0.5f);
                        CSCommonUX.Draw.TextFit(vr, r.Format(r.GetFloat()), valueFont, Look.BarInk, true, TextAnchor.MiddleCenter);
                    }
                    break;
                }
                case Kind.Button:
                {
                    bool enabled = r.Act != null && (r.Enabled == null || r.Enabled());
                    var tone = r.ButtonKind == CSCommonButtonKind.Danger ? Tone.Danger : r.ButtonKind == CSCommonButtonKind.Primary ? Tone.Primary : Tone.Neutral;
                    CSCommonUX.Button.In(new Rect(rx, Mathf.Round(cy - 20f), rw, 40f), null, r.Text, null, tone, null, enabled,
                        () => Safe(r.Act, r.Text));
                    break;
                }
                case Kind.Info:
                {
                    float right = row.xMax;
                    if (!string.IsNullOrEmpty(r.Badge))
                    {
                        float bw = CSCommonUX.Badge.At(0f, cy, r.Badge, r.BadgeColor, null, false);
                        right -= bw;
                        CSCommonUX.Badge.At(right, cy, r.Badge, r.BadgeColor);
                        right -= 8f;
                    }
                    if (!string.IsNullOrEmpty(r.Text) && CSCommonUX.Draw.Repaint)
                        CSCommonUX.Draw.TextFit(new Rect(right - 200f, cy - 12f, 200f, 24f), r.Text, 14f, Pal.Txt, true, TextAnchor.MiddleRight);
                    break;
                }
            }
        }

        static void Safe(Action act, string what)
        {
            try
            {
                act?.Invoke();
            }
            catch (Exception e)
            {
                CSCommonLog.Exception(e, what);
            }
        }
    }
}
