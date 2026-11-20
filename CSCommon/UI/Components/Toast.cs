using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>토스트 상태: 완료(청록 ✓) · 안내(회색 i) · 오류(빨강 !).</summary>
    public enum ToastKind
    {
        Success,
        Info,
        Error,
    }

    /// <summary>
    /// CSCommonUX.Toast — 작업 알림(시안 'CSCommonUX.Toast'): 알약 60(반지름 30 · Section.Plate 면 + 1px Look.PillLine, 그림자 없음) · 왼쪽 12 ·
    /// 상태 원 36(완료 = 모드 파스텔 · 안내 = Look.PillNeutral · 오류 = Look.FabCancel*) · 12 · 제목 14 굵게 Look.BarInk + 3 + 설명 12 Section.SubInk(문구의 첫 ' — ' 뒤) ·
    /// 오른쪽 작은 주 단추 34(되돌리기 등, 앞 18) · 오른쪽 안쪽 단추 있으면 12 · 없으면 28.
    /// 8 아래에서 올라오며 0.26초(Modal.Ease) 나타남 · 끝나기 0.18초 전부터 사라짐(age · life 를 넘기면).
    /// </summary>
    public static class Toast
    {
        const string SuccessPath = "M5 12.5L9.5 17L19 7.5", InfoPath = "M12 11V18M12 6.5h.01", ErrorPath = "M12 5.5V13.5M12 18h.01";

        /// <summary>문구의 첫 ' — ' 앞 = 제목, 뒤 = 설명(없으면 null).</summary>
        static void Split(string text, out string title, out string desc)
        {
            text ??= "";
            int cut = text.IndexOf(" — ", StringComparison.Ordinal);
            title = cut > 0 ? text.Substring(0, cut) : text;
            desc = cut > 0 ? text.Substring(cut + 3) : null;
        }

        public static float Width(string text, string action = null, string actionIcon = null)
        {
            Split(text, out string title, out string desc);
            float tw = Mathf.Max(Mathf.Ceil(Draw.TextWidth(title, 14f, true)), desc != null ? Mathf.Ceil(Draw.TextWidth(desc, 12f)) : 0f);
            float w = CSCommonDesign.ToastPadL + CSCommonDesign.ToastCircle + CSCommonDesign.ToastGap + tw + PadR(action);
            if (!string.IsNullOrEmpty(action)) w += CSCommonDesign.ToastGap + CSCommonDesign.ToastActionGap + Button.Width(actionIcon, action, true);
            return Mathf.Min(w, CSCommonDesign.ToastMaxW);
        }

        /// <summary>오른쪽 안쪽 여백: 단추가 있으면 12 · 없으면 28(시안 — 글만 있는 토스트가 오른쪽에 붙어 보이지 않게).</summary>
        static float PadR(string action) => string.IsNullOrEmpty(action) ? CSCommonDesign.ToastPadR : CSCommonDesign.ToastPadRAction;

        // ---- 한 번에 하나(GLDS 토스트 자리): Show 로 띄우면 바꿔 끼우고 시간을 다시 잼, Flush 가 화면 아래 16 · 겹치는 떠 있는 것(Avoid) 위 8 에 그림
        static string showText;
        static ToastKind showKind;
        static string showAction, showIcon;
        static Action showClick;
        static float showAt = -100f, showLife;
        static readonly List<Rect> avoid = new();

        /// <summary>지금 그린 토스트 자리(그 위 클릭은 지도로 보내지 않음). 없으면 빈 칸.</summary>
        public static Rect Bounds { get; private set; }

        /// <summary>토스트를 띄움(앞의 것은 바꿔 끼움). life 0 = 기본(단추가 있으면 ToastLifeAction · 없으면 ToastLife).</summary>
        public static void Show(string text, ToastKind kind = ToastKind.Info, string action = null, string actionIcon = null, Action click = null, float life = 0f)
        {
            showText = text ?? "";
            showKind = kind;
            showAction = action;
            showIcon = actionIcon;
            showClick = click;
            showAt = Time.realtimeSinceStartup;
            showLife = life > 0f ? life : !string.IsNullOrEmpty(action) ? CSCommonDesign.ToastLifeAction : CSCommonDesign.ToastLife;
        }

        public static void Hide() => showAt = -100f;

        /// <summary>이번 프레임 토스트가 비켜 갈 떠 있는 것(독 · 안내 한 줄 · 판). Flush 전에 부름.</summary>
        public static void Avoid(Rect r)
        {
            if (r.width > 0f && r.height > 0f) avoid.Add(r);
        }

        /// <summary>지금 토스트를 그림(OnGUI 끝 · 다른 판보다 뒤에서 부름).</summary>
        public static void Flush()
        {
            Bounds = default;
            float age = Time.realtimeSinceStartup - showAt;
            if (string.IsNullOrEmpty(showText) || age >= showLife)
            {
                avoid.Clear();
                return;
            }
            float w = Width(showText, showAction, showIcon), h = CSCommonDesign.ToastHeight;
            float x = Mathf.Round((Draw.Width - w) * 0.5f), y = Draw.Height - CSCommonDesign.ToastSafeGap - h;
            // 겹치면 그 위로 — 올린 자리에서 다시 봐서 쌓인 것들 중 가장 높은 것 위
            for (int pass = 0; pass < 8; pass++)
            {
                bool moved = false;
                foreach (var a in avoid)
                    if (a.xMax > x && a.x < x + w && a.yMax > y && a.y < y + h)
                    {
                        y = a.y - CSCommonDesign.FloatingGap - h;
                        moved = true;
                    }
                if (!moved) break;
            }
            avoid.Clear();
            var r = new Rect(x, Mathf.Round(y), w, h);
            Bounds = r;
            var click = showClick;
            In(r, showText, showKind, showAction, showIcon, () =>
            {
                showAt = -100f;
                click?.Invoke();
            }, age, showLife);
        }

        /// <summary>r 높이 = ToastHeight. age = 뜬 뒤 초 · life = 보일 초(넘기지 않으면 움직임 없이 그대로).</summary>
        public static void In(Rect r, string text, ToastKind kind, string action = null, string actionIcon = null, Action click = null, float age = float.MaxValue, float life = float.MaxValue)
        {
            float show = Modal.Ease(age / CSCommonDesign.ToastInSeconds);
            float fade = life < float.MaxValue ? Mathf.Min(show, Mathf.Clamp01((life - age) / CSCommonDesign.ToastOutSeconds)) : show;
            if (fade <= 0f) return;
            float opacity = Draw.Opacity;
            var shift = new Vector2(0f, Mathf.Round((1f - show) * CSCommonDesign.ToastRise));
            Draw.Opacity *= fade;
            Draw.Shift(shift);
            Split(text, out string title, out string desc);
            float bw = string.IsNullOrEmpty(action) ? 0f : Button.Width(actionIcon, action, true), padR = PadR(action);
            if (Draw.Repaint)
            {
                float rad = r.height * 0.5f;
                Draw.Round(r, Section.Plate, rad);
                Draw.RoundOutline(r, Look.PillLine, rad);
                float d = CSCommonDesign.ToastCircle, isz = CSCommonDesign.ToastIcon;
                var c = new Vector2(r.x + CSCommonDesign.ToastPadL + d * 0.5f, r.center.y);
                Color fill = kind == ToastKind.Success ? Look.PastelFill : kind == ToastKind.Error ? Look.FabCancelFill : Look.PillNeutral;
                Color ink = kind == ToastKind.Success ? Look.PastelInk : kind == ToastKind.Error ? Look.FabCancelInk : Look.PillNeutralInk;
                Draw.Dot(c, d * 0.5f, fill);
                Draw.Icon(kind == ToastKind.Success ? SuccessPath : kind == ToastKind.Error ? ErrorPath : InfoPath, new Rect(c.x - isz * 0.5f, c.y - isz * 0.5f, isz, isz), ink, 24f, 2.4f);
                float tx = r.x + CSCommonDesign.ToastPadL + d + CSCommonDesign.ToastGap;
                float tw = r.xMax - tx - padR - (bw > 0f ? bw + CSCommonDesign.ToastGap + CSCommonDesign.ToastActionGap : 0f);
                if (desc == null) Draw.TextFit(new Rect(tx, r.y, tw, r.height), title, 14f, Look.BarInk, true);
                else
                {
                    // 제목 18 + 사이 3 + 설명 16 을 가운데로
                    float top = Mathf.Round(r.center.y - 18.5f);
                    Draw.TextFit(new Rect(tx, top, tw, 18f), title, 14f, Look.BarInk, true);
                    Draw.TextFit(new Rect(tx, top + 21f, tw, 16f), desc, 12f, Section.SubInk);
                }
            }
            if (bw > 0f)
                Button.In(new Rect(r.xMax - padR - bw, Mathf.Round(r.center.y - CSCommonDesign.ToastActionH * 0.5f), bw, CSCommonDesign.ToastActionH), actionIcon, action, null, Tone.Primary, null, true, click);
            Draw.Shift(-shift);
            Draw.Opacity = opacity;
        }
    }
}
