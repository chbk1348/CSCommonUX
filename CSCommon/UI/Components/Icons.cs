using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>컴포넌트 안에서 쓰는 선 아이콘(24 상자 SVG 경로).</summary>
    public static class Icons
    {
        public const string Minus = "M5 12h14";
        public const string Plus = "M12 5v14M5 12h14";
        public const string Close = "M6 6l12 12M18 6L6 18";
        public const string Check = "M5 12l5 5 9-10";
        public const string Chevron = "M6 9l6 6 6-6";
        public const string Left = "M15 6l-6 6 6 6";
        public const string Right = "M9 6l6 6-6 6";
        public const string Search = "M11 18a7 7 0 1 0 0-14 7 7 0 0 0 0 14zM21 21l-4.3-4.3";
        public const string Warn = "M12 3l10 18H2L12 3zM12 10v4M12 17h.01";
        public const string Info = "M12 22a10 10 0 1 0 0-20 10 10 0 0 0 0 20zM12 16v-5M12 8h.01";
        public const string Trash = "M3 6h18M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2";
        public const string Undo = "M3 10h10a5 5 0 0 1 5 5v2M3 10l5-5M3 10l5 5";
        /// <summary>모드 나가기(문 + 바깥 화살표) · 확정 체크(독 FAB).</summary>
        public const string Exit = "M9 21H5V3h4M16 17l5-5-5-5M21 12H9";
        public const string Done = "M5 12.5L9.5 17L19 7.5";

        /// <summary>한 글자 단추 글 → 아이콘(− + × ‹ › ✓), 없으면 null.</summary>
        public static string ForGlyph(string text) => text switch
        {
            "−" or "-" => Minus,
            "+" => Plus,
            "×" or "x" => Close,
            "‹" or "<" => Left,
            "›" or ">" => Right,
            "✓" => Check,
            _ => null,
        };
    }
}
