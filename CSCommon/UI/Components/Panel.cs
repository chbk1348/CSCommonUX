using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>CSCommonUX.Panel — 위 판 · 아래 독 · 펼침 메뉴 바탕: #14171C · 그림자 두 겹 · 1px 선 · 모서리 14(메뉴 12).</summary>
    public static class Panel
    {
        public static void Frame(Rect r, float radius = 14f) => Look.Elevated(r, radius, Pal.Win);
    }
}
