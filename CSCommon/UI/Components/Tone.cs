using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    // CSCommon 컴포넌트(디자인 아티팩트 'CSCommon 컴포넌트' 페이지 · 입력 / 표시 · 알림 / 판 · 창 보드 그대로).
    // 공통 규칙: 톤 바탕 · 테두리 없음(초점 · 고름만 청록 1.5px) · 높이 40(작게 32) · 글 14.5 · 아이콘 18 뜻 색 · 꺼짐 #5B6370.
    // IMGUI(Draw) 위에서 그리고, 모든 모드 화면(디자인 에디터 · F1 창)이 이것만 씀.

    /// <summary>단추 톤: 주(청록 톤) · 보통(회색 톤) · 지움(회색 톤 + 빨강 아이콘) · 글(면 없음 · 청록 글, GLDS Text).</summary>
    public enum Tone
    {
        Primary,
        Neutral,
        Danger,
        Text,
    }
}
