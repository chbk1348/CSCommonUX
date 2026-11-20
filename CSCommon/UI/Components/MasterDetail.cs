using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.MasterDetail — 작업 화면(시안 3.0 올림픽 'Zone'): 위 머리 줄 56(판 · 왼쪽 20 · 오른쪽 8 · 묶음 사이 16 · 이름 18 굵게 · 정보 12.5)
    /// → 필터 · 선택 막대(Section.Tray 48) → 본문 왼쪽 표(Ratio) | 오른쪽 고정 인스펙터. 표 머리 28 · 줄 앞 그림 원 32(그림 18) · 칸 사이 10.
    /// 접는 줄 44(펼침 화살표 16). 판 하나를 가운데 둘 때(빈 화면) 폭 = CenterRatio.
    /// </summary>
    public static class MasterDetail
    {
        public const float Ratio = 0.6f, CenterRatio = 0.6f, HeadH = 56f, HeadPadL = 20f, HeadPadR = 8f, HeadGap = 16f, NameFont = 18f, InfoFont = 12.5f;
        /// <summary>머리 줄 왼쪽 묶음에서 이름이 가질 수 있는 몫 · 본문 최소 높이(작은 화면에서는 창이 스크롤).</summary>
        public const float NameShare = 0.45f, MinBodyH = 240f;
        public const float TableHeadH = 28f, IconD = 32f, Icon = 18f, ColGap = 10f, NameMinW = 120f;
        public const float FoldH = 44f, FoldIcon = 16f;
    }
}
