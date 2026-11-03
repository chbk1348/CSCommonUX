using UnityEngine;

namespace CSCommon
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
                Pal.AccentInk = Color.white; Pal.DoneDot = H("#E6F4EA"); Pal.DoneInk = H("#146C2E"); Pal.TodoDot = H("#FCE8E6"); Pal.WarnDot = H("#FEF0DB");
                Pal.Selected = H("#FFFFFF"); Pal.OnSelected = H("#14171A"); Pal.Strong = H("#0B0D10"); Pal.Haze = Color.black;
                Look.Field = H("#F1F3F5"); Look.FieldFocus = H("#FFFFFF");
                Look.Tone0 = H("#E9ECEF"); Look.Tone1 = H("#DEE2E6"); Look.Tone2 = H("#D3D7DC");
                Look.Ink = H("#14171A"); Look.PanelLine = H("#B9BFC7"); Look.Divider = H("#D3D7DC");
                Look.OffInk = H("#9AA1A9"); Look.OffText = H("#80868E");
                Look.Off = new Color(0f, 0f, 0f, 0.04f); Look.CountBg = new Color(154f / 255f, 82f / 255f, 0f, 0.14f);
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
                Pal.AccentInk = H("#06121A"); Pal.DoneDot = H("#1F4D33"); Pal.DoneInk = H("#4CC787"); Pal.TodoDot = H("#4D1F1C"); Pal.WarnDot = H("#3A2A14");
                Pal.Selected = H("#3A404B"); Pal.OnSelected = Color.white; Pal.Strong = Color.white; Pal.Haze = Color.white;
                Look.Field = H("#1E232B"); Look.FieldFocus = H("#14171C");
                Look.Tone0 = H("#2A3039"); Look.Tone1 = H("#343B45"); Look.Tone2 = H("#22262E");
                Look.Ink = H("#0B0D11"); Look.PanelLine = H("#4A515D"); Look.Divider = H("#343A44");
                Look.OffInk = H("#4A515D"); Look.OffText = H("#6B7380");
                Look.Off = new Color(1f, 1f, 1f, 0.03f); Look.CountBg = new Color(1f, 169f / 255f, 77f / 255f, 0.18f);
                Look.ShadowAlpha = 0.045f;
            }
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
        }
    }
}
