using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommonUX
{
    internal static partial class Draw
    {
        // ---------------- 입력

        // 누른 버튼: 화면 자리(절대 화소)로 기억 — IMGUI 컨트롤 번호는 앞에 그린 것(타워 줄 · 알림 · 무전 카드 · 지도 이름표)이
        // 프레임마다 늘고 줄면 밀려서, 누를 때와 뗄 때 번호가 달라 클릭이 사라졌음(확인 상자 · 창 버튼이 안 눌림).
        static bool held;
        static int heldKey;
        static int releaseFrame = -1;

        /// <summary>IMGUI 버튼을 누르고 있는 중(지도 도구가 클릭을 가져가지 않게).</summary>
        public static bool Holding => (held || ScrollBar.Dragging) && Input.GetMouseButton(0);

        static int ButtonKey(Rect r)
        {
            Rect s = ToScreen(r);
            Vector2 p = GUIUtility.GUIToScreenPoint(s.position);
            unchecked
            {
                int h = Mathf.RoundToInt(p.x);
                h = h * 397 ^ Mathf.RoundToInt(p.y);
                h = h * 397 ^ Mathf.RoundToInt(s.width);
                h = h * 397 ^ Mathf.RoundToInt(s.height);
                return h;
            }
        }

        /// <summary>눌렀다 같은 칸에서 떼면 true. 칸의 화면 자리로 누른 버튼을 알아봄(그리는 순서 · 개수와 상관없음).</summary>
        public static bool Button(Rect r)
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && Hover(r))
                    {
                        held = true;
                        heldKey = ButtonKey(r);
                        e.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (held && heldKey == ButtonKey(r)) e.Use();
                    break;
                case EventType.MouseUp:
                    if (held && e.button == 0 && heldKey == ButtonKey(r))
                    {
                        held = false;
                        e.Use();
                        return Hover(r);
                    }
                    break;
            }
            return false;
        }

        /// <summary>마우스를 떼면(어느 버튼에도 안 걸려도) 누른 상태를 풂 — OnGUI 끝에 한 번.</summary>
        public static void EndInput()
        {
            var e = Event.current;
            // Begin ~ EndInput(모드 OnGUI 한 번) — 허브 공유 전후 비교 지점. 그리기(Repaint)만 재요: 가벼운 Layout · 마우스 호출이 평균을 낮추지 않게(rawType 은 Use() 뒤에도 그대로예요)
            if (e != null && e.rawType == EventType.Repaint) guiWatch.End(guiStart);
            if (e != null && e.rawType == EventType.MouseUp && e.button == 0) releaseFrame = Time.frameCount;
            if (e != null && e.isKey && TypingFrame == Time.frameCount) e.Use(); // 글 칸에 쓰는 동안 키를 뒤 OnGUI(다른 모드 · 단축키)에 넘기지 않음
        }

        /// <summary>누르고 있는 동안 true(꾹 누르기).</summary>
        public static bool Pressed(Rect r) => (held || GUIUtility.hotControl != 0) && Input.GetMouseButton(0) && Hover(r);

        /// <summary>글 입력 칸. 배율 맞춘 GUI.TextField.</summary>
        // 글 입력 칸 모양: (글자 화소, 여백 화소)마다 하나. 글색은 테마가 바뀔 수 있어 부를 때마다 넣음
        static readonly Dictionary<long, GUIStyle> fieldStyles = new();

        public static string Field(Rect r, string value, float size, int maxLength)
        {
            int fpx = Px(size), pad = Px(10);
            long key = ((long)fpx << 32) | (uint)pad;
            if (!fieldStyles.TryGetValue(key, out var style))
            {
                style = new GUIStyle(GUI.skin.textField) { fontSize = fpx, alignment = TextAnchor.MiddleLeft, padding = new RectOffset(pad, pad, 0, 0) };
                style.normal.background = null;
                style.focused.background = null;
                style.hover.background = null;
                fieldStyles[key] = style;
            }
            style.normal.textColor = style.focused.textColor = style.hover.textColor = Pal.Txt;
            var sr = ToScreen(r);
            string v = GUI.TextField(sr, value ?? "", maxLength, style);
            // 방금 그린 칸의 편집기는 position == sr(DoTextField 가 넣음) — 이것으로 '이 칸'인지 앎
            int kc = GUIUtility.keyboardControl;
            if (kc != 0 && GUIUtility.QueryStateObject(typeof(TextEditor), kc) is TextEditor te && te.position == sr)
            {
                // 이 모드의 이 칸에 초점일 때만 '쓰는 중'(초점 이름 · keyboardControl 은 모드 전체 공용 — 다른 모드 칸 초점으로 남의 숨은 칸 · 키 써 버리기가 켜지지 않게)
                TypingFrame = Time.frameCount;
                imeField = kc;
                // GUI.HandleTextFieldEventForDesktop 은 글꼴이 HasCharacter 로 모르는 글자(기본 스킨 글꼴의 한글)를 넣지도 쓰지도 않고 넘김 —
                // 확정된 음절이 버려져 마지막 조합 글만 남던 것('유키' → '키'). 그 자리에서 Unity 가 하던 대로 넣음(넣었으면 이미 Used 라 두 번 안 들어감)
                var ke = Event.current;
                // Ctrl · Cmd 조합(AltGr = Ctrl + Alt 는 글자)은 글자가 아니라 단축키: Ctrl+A 뒤 따라오는 'a' 글자 이벤트를 넣어 고른 글이 'a' 로 바뀌던 것
                bool shortcut = (ke.control || ke.command) && !ke.alt;
                if (ke.type == EventType.KeyDown && shortcut && ke.keyCode == KeyCode.A)
                {
                    te.SelectAll();
                    ke.Use();
                }
                else if (ke.type == EventType.KeyDown && !shortcut && ke.character != '\0' && !char.IsControl(ke.character))
                {
                    te.Insert(ke.character);
                    v = maxLength >= 0 && te.text.Length > maxLength ? te.text.Substring(0, maxLength) : te.text;
                    GUI.changed = true;
                    ke.Use();
                }
                // 한글 IME 켜기(직전에 다른 칸 조합을 끊은 프레임은 한 프레임 쉼). 조합 창 자리는 TextEditor.DrawCursor 가 캐럿에 둠
                if (imeHold != Time.frameCount && Input.imeCompositionMode != IMECompositionMode.On) Input.imeCompositionMode = IMECompositionMode.On;
            }
            else if (imeField != 0 && kc != imeField && GUIUtility.QueryStateObject(typeof(TextEditor), imeField) is TextEditor old && old.position == sr)
            {
                // 이 칸이 조합 중에 초점을 잃음(밖 클릭 · 다른 칸 · FocusControl): 조합 글을 이 칸에 넣고, IME 를 한 프레임 꺼 OS 조합을 끊어 다음 칸으로 새지 않게
                imeField = 0;
                string comp = Input.compositionString;
                if (comp.Length > 0)
                {
                    v = Commit(v, comp, old.cursorIndex, old.selectIndex, maxLength);
                    Input.imeCompositionMode = IMECompositionMode.Off;
                    imeHold = Time.frameCount;
                }
                else if (kc == 0) Input.imeCompositionMode = IMECompositionMode.Auto;
            }
            return v;
        }

        // 초점 글 칸 IMGUI id(0 = 없음) · IME 를 끊은 프레임
        static int imeField, imeHold = -1;

        /// <summary>조합 글 comp 를 선택 [a, b) 자리에 넣음(maxLength 넘으면 자름, 음수 = 끝없음). 순수 — 검사용 internal.</summary>
        internal static string Commit(string v, string comp, int a, int b, int maxLength)
        {
            v ??= "";
            int lo = Mathf.Clamp(Mathf.Min(a, b), 0, v.Length), hi = Mathf.Clamp(Mathf.Max(a, b), 0, v.Length);
            string s = v.Substring(0, lo) + comp + v.Substring(hi);
            return maxLength >= 0 && s.Length > maxLength ? s.Substring(0, maxLength) : s;
        }

        /// <summary>지금 IME 조합 중인 글이 있음(자리 글을 숨길 때).</summary>
        public static bool Composing => !string.IsNullOrEmpty(Input.compositionString);

        /// <summary>글 칸에 키보드 초점이 있던 마지막 프레임: InputBlocker 가 게임 단축키를 막고, EndInput 이 남은 키 이벤트를 써 버림.</summary>
        public static int TypingFrame = -1;

        /// <summary>이번 · 지난 프레임에 글 칸에 쓰는 중(Update 는 OnGUI 앞이라 지난 프레임까지 봄).</summary>
        public static bool Typing => TypingFrame >= Time.frameCount - 1;

    }
}
