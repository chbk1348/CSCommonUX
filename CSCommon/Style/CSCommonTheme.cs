using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommon 테마(다크 · 라이트): 모드가 시작할 때 Use 로 한 번 고름(F1 = 다크 기본, 기상청 = 라이트).
    /// Pal · Look 의 같은 토큰 이름에 테마 값을 넣음 — 부품은 토큰만 읽으므로 그대로 따라감.
    /// 값은 디자인 시스템 토큰(f1-dark · light)과 같음. 지도 마커 · 차트 계열 같은 '뜻 색'은 테마와 상관없이 그대로.
    /// </summary>
    /// <summary>모드 기본 팔레트(Pal.Primary · PrimaryInk · OnPrimary): 청록(기본 — 주차장 · 농장) · F1 빨강(F1 로고 #E10600) · 하늘(스카이캐스트).</summary>
    internal enum CSCommonPalette
    {
        Teal,
        F1Red,
        Violet,
        Gold,
        Sky,
    }

    internal static class CSCommonTheme
    {
        public static bool Light { get; private set; }
        public static CSCommonPalette Palette { get; private set; }

        public static void Use(bool light, CSCommonPalette palette = CSCommonPalette.Teal)
        {
            Light = light;
            Palette = palette;
            Color H(string hex) => Draw.Hex(hex);
            if (light)
            {
                // UX 1.1 라이트(B안 선명 대비): 보조 글 8.1:1 · 흐린 글 띠 위 4.9:1 · 떠 있는 판 · 입력 칸 테두리 #B9BFC7
                Pal.Win = H("#FFFFFF"); Pal.Band = H("#E4E7EB"); Pal.Card = H("#F8F9FA"); Pal.Inset = H("#F4F5F7");
                Pal.Line = H("#D3D7DC"); Pal.Line2 = H("#B9BFC7");
                Pal.Txt = H("#14171A"); Pal.Light = H("#2E3338"); Pal.Sub = H("#4A5058"); Pal.Dim = H("#5C636B");
                Pal.Body = H("#2E3338"); Pal.Hero = H("#F1F3F5");
                Pal.Accent = H("#1A73E8"); Pal.Teal = H("#00695C"); Pal.Ok = H("#0B57D0"); Pal.Warn = H("#9A5200");
                Pal.Red = H("#B3261E"); Pal.Green = H("#146C2E"); Pal.PersonalBest = H("#146C2E"); Pal.Yellow = H("#B08800");
                Pal.SeriesF2 = H("#0B57D0"); Pal.SeriesWec = H("#00695C"); Pal.SeriesMoto = H("#8F4E00");
                Pal.AccentInk = Color.white; Pal.DoneDot = H("#E6F4EA"); Pal.DoneInk = H("#146C2E"); Pal.TodoDot = H("#FCE8E6"); Pal.WarnDot = H("#FEF0DB");
                Pal.Selected = H("#FFFFFF"); Pal.OnSelected = H("#14171A"); Pal.Strong = H("#0B0D10"); Pal.Haze = Color.black;
                Look.Field = H("#F1F3F5"); Look.FieldFocus = H("#FFFFFF"); Look.FieldLine = H("#7A8089");
                Look.Tone0 = H("#E9ECEF"); Look.Tone1 = H("#DEE2E6"); Look.Tone2 = H("#D3D7DC");
                Look.Ink = H("#14171A"); Look.PanelLine = H("#B9BFC7"); Look.Divider = H("#D3D7DC");
                Look.OffInk = H("#9AA1A9"); Look.OffText = H("#80868E");
                Look.Off = new Color(0f, 0f, 0f, 0.04f);
                Look.ShadowAlpha = 0.05f;
            }
            else
            {
                Pal.Win = H("#14171C"); Pal.Band = H("#0E1014"); Pal.Card = H("#1B1F26"); Pal.Inset = H("#262B33");
                // UX 1.1 다크(B안 선명 대비): 보조 글 9.4:1 · 흐린 글 6.6:1 · 헤어라인 #343A44 · 떠 있는 판 · 입력 칸 테두리 #4A515D
                Pal.Line = H("#343A44"); Pal.Line2 = H("#4A515D");
                Pal.Txt = H("#F2F4F7"); Pal.Light = H("#C9CFD8"); Pal.Sub = H("#B4BCC7"); Pal.Dim = H("#959EAA");
                Pal.Body = H("#DDE2E8"); Pal.Hero = H("#20252D");
                Pal.Accent = H("#E10600"); Pal.Teal = H("#2ED4BF"); Pal.Ok = H("#5CC8FF"); Pal.Warn = H("#FFA94D");
                Pal.Red = H("#FF6B5E"); Pal.Green = H("#22C55E"); Pal.PersonalBest = H("#00C761"); Pal.Yellow = H("#F5C400");
                Pal.SeriesF2 = H("#5294FA"); Pal.SeriesWec = H("#12A594"); Pal.SeriesMoto = H("#F08A00");
                Pal.AccentInk = H("#06121A"); Pal.DoneDot = H("#1F4D33"); Pal.DoneInk = H("#4CC787"); Pal.TodoDot = H("#4D1F1C"); Pal.WarnDot = H("#3A2A14");
                Pal.Selected = H("#3A404B"); Pal.OnSelected = Color.white; Pal.Strong = Color.white; Pal.Haze = Color.white;
                Look.Field = H("#1E232B"); Look.FieldFocus = H("#14171C"); Look.FieldLine = H("#737B88");
                Look.Tone0 = H("#2A3039"); Look.Tone1 = H("#343B45"); Look.Tone2 = H("#22262E");
                Look.Ink = H("#0B0D11"); Look.PanelLine = H("#4A515D"); Look.Divider = H("#343A44");
                Look.OffInk = H("#4A515D"); Look.OffText = H("#6B7380");
                Look.Off = new Color(1f, 1f, 1f, 0.03f);
                Look.ShadowAlpha = 0.045f;
            }
            Pal.RedAccent = Pal.Red;
            if (palette == CSCommonPalette.F1Red)
            {
                // 면 = 로고 빨강, 글 = 바탕 위에서 4.5:1 넘는 빨강
                Pal.Primary = H(light ? "#D10A00" : "#E10600");
                Pal.PrimaryInk = H(light ? "#B80900" : "#FF5A4E");
                Pal.OnPrimary = Color.white;
            }
            else if (palette == CSCommonPalette.Sky)
            {
                // 하늘 파랑: 라이트 면 · 글 #0B57D0(흰 바탕 6.4:1, 흰 글 6.4:1) · 다크 면 #2E9BFF(어두운 글) · 글 #6CB4FF
                Pal.Primary = H(light ? "#0B57D0" : "#2E9BFF");
                Pal.PrimaryInk = H(light ? "#0B57D0" : "#6CB4FF");
                Pal.OnPrimary = light ? Color.white : Pal.AccentInk;
            }
            else
            {
                Pal.Primary = Pal.PrimaryInk = Pal.Teal;
                if (light) Pal.PrimaryInk = H("#00574D");
                Pal.OnPrimary = light ? Color.white : Pal.AccentInk;
            }
            UsePastel(light, palette);
        }

        /// <summary>
        /// C안 알약 색(Look.Bar* · Pastel* · Fab*): 판 바탕 · 1px 선 · 안 고른 칸 아이콘/글 + 팔레트 파스텔(채움 · 글 · 서브 채움 · 서브 글 — 글은 채움 위 4.5:1 이상).
        /// 다크 서브 채움 = 다크 채움 18%. 되돌리기 FAB = 파랑 파스텔, 취소 FAB = F1 빨강 파스텔(팔레트와 상관없이 고정), 확인 FAB = 팔레트 파스텔.
        /// </summary>
        static void UsePastel(bool light, CSCommonPalette palette)
        {
            Color H(string hex) => Draw.Hex(hex);
            // 라이트 채움 · 글 · 서브 채움 · 서브 글 / 다크 채움 · 글 · 서브 글
            string[] p = palette switch
            {
                CSCommonPalette.F1Red => new[] { "#FFDEDB", "#7A1410", "#FFECEB", "#8F1A15", "#FFC9C4", "#5C0F0B", "#FFE0DD" },
                CSCommonPalette.Sky => new[] { "#D7ECFF", "#0B3A6B", "#E8F4FF", "#0F4C85", "#BFE0FF", "#0A2E55", "#DCEEFF" },
                CSCommonPalette.Gold => new[] { "#FFF0C2", "#5C4300", "#FFF7DE", "#6B4F00", "#FFE49A", "#4A3500", "#FFF0C2" },
                CSCommonPalette.Violet => new[] { "#E8E0FF", "#3B2A7A", "#F1ECFF", "#4A3A8A", "#D4C8FF", "#2E1F66", "#E8E0FF" },
                _ => new[] { "#D6F3EE", "#0B4F47", "#E6F7F4", "#0F5E55", "#B8EFE6", "#0A3D37", "#D3F5EF" },
            };
            Look.BarSurface = light ? Color.white : H("#1B1E24");
            Look.BarLine = light ? new Color(20f / 255f, 24f / 255f, 31f / 255f, 0.14f) : new Color(1f, 1f, 1f, 0.08f);
            Look.BarOff = H(light ? "#4A5260" : "#A7ADB8");
            Look.PastelFill = H(light ? p[0] : p[4]);
            Look.PastelInk = H(light ? p[1] : p[5]);
            Look.PastelSubFill = light ? H(p[2]) : Draw.Alpha(H(p[4]), 0.18f);
            Look.PastelSubInk = H(light ? p[3] : p[6]);
            Look.FabUndoFill = H(light ? "#DBE8FF" : "#BCD4FF");
            Look.FabUndoInk = H(light ? "#14306B" : "#0F2A5C");
            Look.FabCancelFill = H(light ? "#FFDEDB" : "#FFC9C4");
            Look.FabCancelInk = H(light ? "#7A1410" : "#5C0F0B");
            // 독 모드 나가기 FAB 아이콘(면은 판 바탕) · 독 안내 알약 머리 글
            Look.FabExitInk = H(light ? "#C5221F" : "#FF6B5E");
            Look.BarInk = light ? H("#14181F") : Color.white;
            // CSCommonUX.Button · IconButton 상태 면(시안 'CSCommonUX.Button' — 주 호버 · 누름은 시안 청록 값, 다른 팔레트는 채움→글 7% · 14% 섞음)
            bool teal = palette == CSCommonPalette.Teal;
            Look.PastelHover = teal ? H(light ? "#C5ECE5" : "#A8E8DE") : Color.Lerp(Look.PastelFill, Look.PastelInk, 0.07f);
            Look.PastelPress = teal ? H(light ? "#B4E4DB" : "#98E0D5") : Color.Lerp(Look.PastelFill, Look.PastelInk, 0.14f);
            Look.DangerHover = H(light ? "#FFD0CC" : "#FFBAB4");
            Look.DangerPress = H(light ? "#FFC2BD" : "#FFAAA3");
            Look.NeutralHover = H(light ? "#DFE1E5" : "#30363F");
            Look.NeutralPress = H(light ? "#D5D8DC" : "#22262E");
            Look.TextHover = light ? H(p[2]) : Draw.Alpha(H(p[4]), 0.10f);
            Look.TextPress = light ? H(p[0]) : Draw.Alpha(H(p[4]), 0.18f);
            Look.BtnOff = light ? new Color(20f / 255f, 24f / 255f, 31f / 255f, 0.04f) : new Color(1f, 1f, 1f, 0.03f);
            Look.BtnOffInk = H(light ? "#9AA0A6" : "#5B6370");
            Look.IconOffInk = H(light ? "#BDC1C6" : "#4A505B");
            // CSCommonUX.Modal · Textfield 알약(시안 라이트 / 다크): 파스텔 테두리 = 라이트 파스텔 글 · 다크 파스텔 채움(어두운 바탕 위 밝게)
            Look.Scrim = new Color(5f / 255f, 6f / 255f, 8f / 255f, light ? 0.35f : 0.55f);
            Look.Foot = H(light ? "#F6F7F9" : "#171A20");
            Look.FootLine = H(light ? "#ECEDF1" : "#22262E");
            // 다크 알약 면 = 플레이트(Section.Plate #1B1F26)보다 한 단 밝게(Pal.Inset 단계): 같은 값이라 플레이트 위 고르기 · 글 칸 · 홈이 안 보였음(라이트는 플레이트보다 한 단 어둡게)
            Look.Pill = H(light ? "#F1F3F4" : "#262B33");
            Look.PillRaised = H(light ? "#E8EAED" : "#30363F");
            Look.PillNeutral = H(light ? "#E8EAED" : "#262B33");
            Look.PillNeutralInk = H(light ? "#3C4043" : "#C9CFD8");
            Look.PillHoverLine = H(light ? "#C9CDD2" : "#30363F");
            Look.TitleLine = H(light ? "#C9CDD2" : "#3A404B");
            Look.PastelRing = H(light ? p[1] : p[4]);
            Look.PastelLink = H(light ? p[3] : p[4]);
            Look.GripAlpha = light ? 0.5f : 0.6f;
            Look.ModalShadow = light ? 0.12f : 0.5f;
            Look.ModalShadowY = light ? 8f : 10f;
            Look.ModalShadowBlur = light ? 22f : 26f;
            // 컨트롤 시안(CtlA · B · C 라이트 / 다크)
            Look.SegmentOn = H(light ? "#FDFDFD" : "#3A404B");
            Look.PillSub = H(light ? "#6E747B" : "#7F8894");
            Look.PillLine = H(light ? "#DFE1E5" : "#30363F");
            Look.MenuSurface = light ? Color.white : H("#1F232B");
            Look.MenuLine = light ? new Color(20f / 255f, 24f / 255f, 31f / 255f, 0.14f) : new Color(1f, 1f, 1f, 0.14f);
            Look.MenuHover = H(light ? "#F1F3F4" : "#2A3039");
            Look.KnobOff = H(light ? "#9BA1A7" : "#8A919B");
            Look.PastelNote = H(light ? p[3] : p[5]);
            Look.SwatchLine = light ? new Color(20f / 255f, 24f / 255f, 31f / 255f, 0.12f) : new Color(1f, 1f, 1f, 0.12f);
            // 진한 파스텔(슬라이더 · 목표 넘은 막대): 라이트 청록은 시안 값, 다른 팔레트 라이트는 채움→글 40%(흰 바탕 위에서 보이게), 다크는 채움
            Look.PastelStrong = !light ? Look.PastelFill : teal ? H("#4FBCAE") : Color.Lerp(Look.PastelFill, Look.PastelInk, 0.4f);
            Look.ProgressLow = H(light ? "#F0A24A" : "#FFA94D");
            Look.TipFill = H(light ? "#202124" : "#0B0D11");
            Look.TipInk = H("#ECEDF2");
            Look.WarnFill = H(light ? "#FFE8CC" : "#FFD9A8");
            Look.WarnInk = H(light ? "#8A4A00" : "#5A3000");
            Look.CountBg = new Color(1f, 169f / 255f, 77f / 255f, 0.22f);
        }
    }
}
