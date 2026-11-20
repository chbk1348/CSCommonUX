using UnityEngine;

namespace CSCommonUX
{
    /// <summary>컴포넌트가 같이 쓰는 색 · 알파 토큰(CSCommonTheme.Use · UsePastel 이 테마 · 팔레트에 맞춰 채움). 눌림 · 그림자 동작은 UI/Components/Look.cs.</summary>
    public static partial class Look
    {
        public static Color Field = Draw.Hex("#1B1F26"), FieldFocus = Draw.Hex("#14171C");
        /// <summary>입력 칸 · 고르기 칸 · 펼친 목록 테두리 — 칸 바탕 · 창 바탕 모두와 3:1 이상(UI 경계 기준). 라이트 #7A8089 · 다크 #737B88.</summary>
        public static Color FieldLine = Draw.Hex("#737B88");
        public static Color Tone0 = Draw.Hex("#262B33"), Tone1 = Draw.Hex("#30363F"), Tone2 = Draw.Hex("#22262E");
        public static Color OffInk = Draw.Hex("#4A505B"), OffText = Draw.Hex("#5B6370");
        public static Color Ink = Draw.Hex("#0B0D11"), PanelLine = Draw.Hex("#2A2F38"), Divider = Draw.Hex("#22262E");
        public static Color CountBg = new(1f, 169f / 255f, 77f / 255f, 0.18f);
        public static Color Off = new(1f, 1f, 1f, 0.03f);
        /// <summary>떠 있는 판 그림자 한 겹 알파(다크 0.045 / 라이트 0.03).</summary>
        public static float ShadowAlpha = 0.045f;

        /// <summary>
        /// C안 알약(하단 툴바 · 서브 알약 · 독 · FAB): 판 바탕 · 1px 선 · 안 고른 칸 아이콘/글 · 모드 파스텔(고른 칸 채움 · 글) · 서브 파스텔(채움 · 글) ·
        /// FAB 되돌리기(파랑) · 취소(빨강). CSCommonTheme.Use 가 테마 · 팔레트에 맞춰 채움(기본값 = 다크 청록).
        /// </summary>
        public static Color BarSurface = Draw.Hex("#1B1E24"), BarLine = new(1f, 1f, 1f, 0.08f), BarOff = Draw.Hex("#A7ADB8");
        public static Color PastelFill = Draw.Hex("#B8EFE6"), PastelInk = Draw.Hex("#0A3D37"), PastelSubFill = new(184f / 255f, 239f / 255f, 230f / 255f, 0.18f), PastelSubInk = Draw.Hex("#D3F5EF");
        public static Color FabUndoFill = Draw.Hex("#BCD4FF"), FabUndoInk = Draw.Hex("#0F2A5C"), FabCancelFill = Draw.Hex("#FFC9C4"), FabCancelInk = Draw.Hex("#5C0F0B");
        /// <summary>독 모드 나가기 FAB 아이콘(빨강 — 면은 판 바탕) · 독 안내 알약 머리 글.</summary>
        public static Color FabExitInk = Draw.Hex("#FF6B5E"), BarInk = Color.white;
        /// <summary>CSCommonUX.Button · IconButton(시안 'CSCommonUX.Button'): 주 · 지움 · 보통 · 글 톤 호버 · 누름 면, 꺼진 단추 면 · 글, 꺼진 아이콘 단추 아이콘.</summary>
        public static Color PastelHover = Draw.Hex("#A8E8DE"), PastelPress = Draw.Hex("#98E0D5"), DangerHover = Draw.Hex("#FFBAB4"), DangerPress = Draw.Hex("#FFAAA3");
        public static Color NeutralHover = Draw.Hex("#30363F"), NeutralPress = Draw.Hex("#22262E"), TextHover = new(184f / 255f, 239f / 255f, 230f / 255f, 0.10f), TextPress = new(184f / 255f, 239f / 255f, 230f / 255f, 0.18f);
        public static Color BtnOff = new(1f, 1f, 1f, 0.03f), BtnOffInk = Draw.Hex("#5B6370"), IconOffInk = Draw.Hex("#4A505B");

        /// <summary>
        /// 모달 · 알약 글 칸(시안 'CSCommonUX.Modal' · 'CSCommonUX.Textfield'): 덮개 · 발/머리 띠 면 · 발 위 선 · 알약 바탕(글 칸 · 탭 홈 · 타일) · 회색 알약(보통 단추 · 닫기 원) ·
        /// 회색 알약 글 · 알약 호버 선 · 밑줄 제목 선 · 파스텔 테두리(초점 · 커서 · 밑줄) · 파스텔 글 단추 · 손잡이 점 투명도 · 모달 그림자(알파 · 아래 · 흐림).
        /// </summary>
        public static Color Scrim = new(5f / 255f, 6f / 255f, 8f / 255f, 0.55f), Foot = Draw.Hex("#171A20"), FootLine = Draw.Hex("#22262E");
        public static Color Pill = Draw.Hex("#1B1F26"), PillNeutral = Draw.Hex("#262B33"), PillNeutralInk = Draw.Hex("#C9CFD8"), PillHoverLine = Draw.Hex("#30363F"), TitleLine = Draw.Hex("#3A404B");
        public static Color PastelRing = Draw.Hex("#B8EFE6"), PastelLink = Draw.Hex("#B8EFE6");
        /// <summary>알약 홈(Look.Pill) 안 원 단추 면(스테퍼 − +): 홈과 한 단 다르게. 라이트 = PillNeutral 값 · 다크 = 한 단 밝게.</summary>
        public static Color PillRaised = Draw.Hex("#30363F");
        public static float GripAlpha = 0.6f, ModalShadow = 0.5f, ModalShadowY = 10f, ModalShadowBlur = 26f;

        /// <summary>
        /// 컨트롤 시안(CtlA · B · C): 분절 고른 칸 면 · 보조 글(서브라벨 · 타일 · 줄 메모 · 숫자 이름) · 알약 1px 선(칩 · 우물 · 카드 · 토스트 · 슬라이더 빈 막대) ·
        /// 펼친 목록 면 · 선 · 줄 호버 · 손잡이(슬라이더 · 채운 눈금) · 꺼진 손잡이(스위치 · 빈 눈금) · 파스텔 위 보조 글 · 색 칸 선 · 진한 파스텔(슬라이더 · 목표 넘은 막대) ·
        /// 못 미친 막대 · 말풍선 면 · 글 · 상태 알약 주황 면 · 글. CSCommonTheme.UsePastel 이 채움(기본값 = 다크 청록).
        /// </summary>
        public static Color SegmentOn = Draw.Hex("#3A404B"), PillSub = Draw.Hex("#7F8894"), PillLine = Draw.Hex("#30363F");
        public static Color MenuSurface = Draw.Hex("#1F232B"), MenuLine = new(1f, 1f, 1f, 0.14f), MenuHover = Draw.Hex("#2A3039");
        public static Color Knob = Draw.Hex("#FCFCFC"), KnobOff = Draw.Hex("#8A919B"), PastelNote = Draw.Hex("#0A3D37"), SwatchLine = new(1f, 1f, 1f, 0.12f);
        public static Color PastelStrong = Draw.Hex("#B8EFE6"), ProgressLow = Draw.Hex("#FFA94D"), TipFill = Draw.Hex("#0B0D11"), TipInk = Draw.Hex("#ECEDF2");
        public static Color WarnFill = Draw.Hex("#FFD9A8"), WarnInk = Draw.Hex("#5A3000");
        /// <summary>히어로 면(Section.Hero — 모드 파스텔을 아주 옅게) · 잘됨 알약 면 · 글(청록 파스텔, 팔레트와 상관없음). CSCommonTheme.UsePastel 이 채움.</summary>
        public static Color HeroFill = new(184f / 255f, 239f / 255f, 230f / 255f, 0.08f), OkFill = Draw.Hex("#B8EFE6"), OkInk = Draw.Hex("#0A3D37");
        /// <summary>히어로 1px 선(모드 파스텔을 옅게 — HeroFill 과 짝). CSCommonTheme.UsePastel 이 채움.</summary>
        public static Color HeroLine = new(184f / 255f, 239f / 255f, 230f / 255f, 0.14f);
        /// <summary>
        /// 팔레트와 상관없는 노랑 · 파랑 파스텔(모드가 고르는 두 번째 · 세 번째 색 — 예: 시리즈 표시 칩 · 히어로): 채움 · 글(채움 위 4.5:1 이상) · 히어로 면 · 히어로 1px 선.
        /// 노랑 = CSCommonPalette.Gold 파스텔과 같은 값. CSCommonTheme.UsePastel 이 채움.
        /// </summary>
        public static Color SeriesYellowFill = Draw.Hex("#FFE49A"), SeriesYellowInk = Draw.Hex("#4A3500"), SeriesYellowHero = Draw.Hex("#22201A"), SeriesYellowHeroLine = new(1f, 228f / 255f, 154f / 255f, 0.16f);
        public static Color SeriesBlueFill = Draw.Hex("#C6D8FF"), SeriesBlueInk = Draw.Hex("#14305C"), SeriesBlueHero = Draw.Hex("#1A1F2A"), SeriesBlueHeroLine = new(198f / 255f, 216f / 255f, 1f, 0.16f);
        /// <summary>'잘됨' 판 옅은 면 · 1px 선(청록, 팔레트 · SeparateOk 와 상관없음 — 예: '열 수 있음' 상태 판).</summary>
        public static Color OkSubFill = new(127f / 255f, 214f / 255f, 200f / 255f, 0.06f), OkSubLine = new(127f / 255f, 214f / 255f, 200f / 255f, 0.18f);
        /// <summary>하늘색 옅은 면(뜻: 가까움 · 범위 안 — 예: 노선도 400m 안 역 칸).</summary>
        public static Color SkySubFill = new(143f / 255f, 168f / 255f, 214f / 255f, 0.14f);
        /// <summary>하늘 히어로 면(SkySubFill 보다 옅게 — 선은 SkySubFill, 예: 레이더 도착 예측 판). CSCommonTheme.UsePastel 이 채움.</summary>
        public static Color SkyHeroFill = new(143f / 255f, 168f / 255f, 214f / 255f, 0.07f);
        /// <summary>경고 옅은 판 면 · 1px 선(주황, 팔레트와 상관없음 — 예: 코션 판). CSCommonTheme.UsePastel 이 채움.</summary>
        public static Color WarnSubFill = new(1f, 217f / 255f, 168f / 255f, 0.08f), WarnSubLine = new(1f, 217f / 255f, 168f / 255f, 0.25f);
        /// <summary>파스텔 줄 강조 면(모드 파스텔을 옅게 — 예: 타이밍 표 선두 줄). CSCommonTheme.UsePastel 이 채움.</summary>
        public static Color PastelRowFill = new(184f / 255f, 239f / 255f, 230f / 255f, 0.08f);
        /// <summary>파스텔 고른 칸 면 · 1px 선(모드 파스텔 — 예: 대체 출전 좌석 칸). CSCommonTheme.UsePastel 이 채움.</summary>
        public static Color PastelPickFill = new(184f / 255f, 239f / 255f, 230f / 255f, 0.12f), PastelPickLine = new(184f / 255f, 239f / 255f, 230f / 255f, 0.35f);
        /// <summary>값 막대 홈(Look.Pill 칸 안 — 예: 설정 값 막대). CSCommonTheme.UsePastel 이 채움.</summary>
        public static Color MeterTrack = Draw.Hex("#343A44");
    }
}
