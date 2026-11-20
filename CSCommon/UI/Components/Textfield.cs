using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.Textfield — 알약 글 칸(시안 'CSCommonUX.Textfield'): 높이 40 · 반지름 = 높이 절반 · 안쪽 왼쪽 16 · 오른쪽 8 · 요소 사이 4 · 그림자 없음.
    /// 바탕 Look.Pill, 호버 = 안쪽 1px Look.PillHoverLine, 초점 = 바탕 Look.FieldFocus + 안쪽 1.5px 파스텔 테두리(Look.PastelRing, 커서도 같은 색),
    /// 잘못 = 안쪽 1.5px 빨강(초점이어도 유지), 꺼짐 = Look.Off 바탕 · 글 · 단위 OffText · 호버 · 오류 테두리 없음. 호버 · 초점 전환 0.12초.
    /// 앞 아이콘 16(Dim, 오른쪽 4) · 앞뒤 단위(값이 있을 때만 · 굵은 Sub) · 지우기 ✕(지름 28 원 · 아이콘 14 · 초점이고 값이 있을 때만 · 누름 영역은 칸 오른쪽 끝).
    /// </summary>
    public static class Textfield
    {
        /// <summary>안쪽 왼쪽 · 오른쪽 · 요소 사이 · 앞 아이콘 · 지우기 원 · 지우기 아이콘 · 호버 선 · 초점/오류 선 · 전환 초 · Draw.Field 안쪽 여백(글 시작 맞춤) · 도움말 줄 높이.</summary>
        public const float PadL = 16f, PadR = 8f, Gap = 4f, Icon = 16f, ClearD = 28f, ClearIcon = 14f, HoverW = 1f, RingW = 1.5f, FadeSeconds = 0.12f, FieldInset = 10f, HelpH = 18f;

        /// <summary>글 칸(name = 초점 이름, 없으면 칸 자리로). 바뀐 글을 돌려줌.</summary>
        public static string In(Rect r, string value, int maxLength, string placeholder = null, string icon = null, bool error = false, bool enabled = true, string name = null, float font = 14.5f,
            string prefix = null, string suffix = null, bool clearable = false)
        {
            name ??= "csf" + Look.Key(r, "field");
            value ??= "";
            bool focus = GUI.GetNameOfFocusedControl() == name;
            bool has = value.Length > 0;
            int key = CSCommonAnim.Key("textfield", name);
            float hv = 0f; // 호버 효과 없음
            float fo = CSCommonAnim.Toward(key ^ 0x3b9ac9ff, enabled && focus ? 1f : 0f, 1f / FadeSeconds);
            float rad = r.height * 0.5f;
            if (Draw.Repaint)
            {
                Draw.Round(r, !enabled ? Look.Off : Color.Lerp(Look.Pill, Look.FieldFocus, fo), rad);
                if (!enabled) { }
                else if (error) Draw.RoundOutline(r, Pal.Red, rad, RingW);
                else
                {
                    if (hv * (1f - fo) > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Look.PillHoverLine, hv * (1f - fo)), rad, HoverW);
                    if (fo > 0.01f) Draw.RoundOutline(r, Draw.Alpha(Look.PastelRing, fo), rad, RingW);
                }
            }
            // 글 시작 x(Draw.Field 는 안쪽 FieldInset 만큼 띄워 씀)
            float tx = r.x + PadL;
            if (!string.IsNullOrEmpty(icon))
            {
                if (Draw.Repaint) Draw.Icon(icon, new Rect(tx, Mathf.Round(r.center.y - Icon * 0.5f), Icon, Icon), Pal.Dim, 24f, 2f);
                tx += Icon + Gap + Gap;
            }
            Color unit = enabled ? Pal.Sub : Look.OffText;
            // 앞 단위: 값 앞(값이 비면 숨겨 자리 글과 붙어 읽히지 않게)
            if (has && !string.IsNullOrEmpty(prefix))
            {
                float pw = Mathf.Ceil(Draw.TextWidth(prefix, font, true));
                if (Draw.Repaint) Draw.Text(new Rect(tx, r.y, pw + 2f, r.height), prefix, font, unit, true);
                tx += pw + Gap;
            }
            // 지우기 ✕: 오른쪽 끝 원(초점 · 값이 있을 때)
            bool clear = clearable && enabled && focus && has;
            var clearR = new Rect(r.xMax - PadR - ClearD, Mathf.Round(r.center.y - ClearD * 0.5f), ClearD, ClearD);
            float right = r.xMax - PadR - (clear ? ClearD + Gap : 0f);
            // 뒤 단위: 값 바로 뒤(넘치면 오른쪽 끝에 붙음)
            float sw = has && !string.IsNullOrEmpty(suffix) ? Mathf.Ceil(Draw.TextWidth(suffix, font, true)) : 0f;
            if (sw > 0f) right -= sw + Gap;
            var fr = new Rect(tx - FieldInset, r.y, Mathf.Max(0f, right - tx + FieldInset * 2f), r.height);
            if (sw > 0f && Draw.Repaint)
            {
                float vx = tx + Mathf.Ceil(Draw.TextWidth(value, font)) + Gap;
                Draw.Text(new Rect(Mathf.Min(vx, right + Gap), r.y, sw + 2f, r.height), suffix, font, unit, true);
            }
            if (!enabled)
            {
                if (Draw.Repaint) Draw.TextFit(new Rect(tx, r.y, Mathf.Max(0f, right - tx), r.height), value, font, Look.OffText);
                return value;
            }
            if (clear)
            {
                if (Draw.Repaint) Draw.Icon(Icons.Close, new Rect(Mathf.Round(clearR.center.x - ClearIcon * 0.5f), Mathf.Round(clearR.center.y - ClearIcon * 0.5f), ClearIcon, ClearIcon), Pal.Dim, 24f, 2.2f);
                // 누름 영역 = 원부터 칸 오른쪽 끝까지(칸 높이 전체), 눌림 표시 없음
                if (Draw.Button(new Rect(clearR.x, r.y, r.xMax - clearR.x, r.height))) return "";
            }
            // 조합 중엔 값이 아직 비어도 자리 글을 숨김(조합 글이 칸 맨 앞 자리 글 위에 겹쳐 보였음)
            if (!has && !(focus && Draw.Composing) && !string.IsNullOrEmpty(placeholder) && Draw.Repaint)
                Draw.TextFit(new Rect(tx, r.y, Mathf.Max(0f, right - tx), r.height), placeholder, font, Look.OffText);
            // 커서 색 = 초점 테두리 색(파스텔)
            var cursor = GUI.skin.settings.cursorColor;
            GUI.skin.settings.cursorColor = Look.PastelRing;
            GUI.SetNextControlName(name);
            string v = Draw.Field(fr, value, font, maxLength);
            GUI.skin.settings.cursorColor = cursor;
            // 칸 밖을 누르면 초점을 풂: 단추가 MouseDown 을 먼저 써서(Use) IMGUI 가 못 풀면 초점 · IME · 누름 상태가 남아 이후 클릭이 막힘
            var ev = Event.current;
            if (focus && ev.rawType == EventType.MouseDown && ev.button == 0 && !r.Contains(Draw.Mouse) && GUI.GetNameOfFocusedControl() == name)
            {
                GUI.FocusControl(null);
                GUIUtility.keyboardControl = 0;
                focus = false;
            }
            // 한글 IME 켜기 · 끄기 · 조합 끊기는 Draw.Field(조합 창 자리는 캐럿)
            return v;
        }

    }
}
