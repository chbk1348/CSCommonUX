# CSCommon UX 3.0

Cities: Skylines 모드가 같이 쓰는 UI 공유 폴더입니다.

- `CSCommon/Style/Pal.cs` — 색 토큰(다크 기본값)
- `CSCommon/Style/CSCommonTheme.cs` — 다크 · 라이트 테마 값과 모드 팔레트(청록 · F1 빨강 · 하늘 · 보라 · 금색)와 툴바 C안 파스텔
- `CSCommon/Style/CSCommonDesign.cs` — 치수 토큰(높이 · 간격 · 모서리 · 글자)
- `CSCommon/UI/Components.cs` — IMGUI 컴포넌트(Button · Textfield · Select · Switch · Slider · Toast · Dialog …)
- `CSCommon/UI/CSCommonKit.cs` · `CSCommonToolbar.cs` — 게임 UI(ColossalFramework) 부품 · 위 판 툴바 · 독(Dockbar)
- `CSCommon/UI/CSCommonUXToolbar.cs` — 모드 창 하단 Toolbar · SubToolBar
- `CSCommon/DesignCheck.targets` — 모드 코드에 색 값 · 치수 숫자 · 직접 그린 IMGUI 부품이 늘면 빌드 오류(CSC001)

## 사용하는 모드

- F1 그랑프리 — 개인 사용 / 비공개
- 환경음 조절기 — 개인 사용 / 비공개
- 기상청 — 개인 사용 / 비공개
- 주차장 — 개인 사용 / 비공개
- 농장 — 개인 사용 / 비공개

## UX 3.0 변경 (2026-11-11)

- **성능:** `Draw.TextWidth` · `TextHeight` 캐시(`widthCache` · `heightCache`), 아이콘 · 말줄임 캐시 키를 `TextKey` 로, `Draw.Field` GUIStyle 캐시(`fieldStyles`). 모드 사본 9곳에 같이 반영.
- **툴바:** `DockTool` 툴팁 문자열을 마우스를 올렸을 때만 조립. `Fab` · `SubChip` · `IconGroup` 애니메이션 키를 자리와 무관하게 바꿔 `slots` · `presses` 가 계속 커지지 않게 함. 네임스페이스 `CSCommon.PillBar` → `CSCommonUX.PillBar`.
- **Section:** 제목 칩과 본문 사이 `HeadGap` 6 → 8, 칩 오른쪽 안쪽 `ChipPadR` 14 → 16. 옵션 패널의 부연 설명(`desc`)은 제목 줄 맨 오른쪽 회색 보조 알약으로 옮김(제목이 없을 때만 본문에 그림).
- **입력:** 마우스 뗌 판정 한 프레임 늦춤(`releaseFrame`), `Hover` 가 잘린 영역(`visible`)까지 봄.
- **토큰 추가(2026-11-13, F1 3.0 보드):** `Look.HeroLine`(히어로 1px 선) · `Look.SeriesYellow*` · `Look.SeriesBlue*`(Fill · Ink · Hero · HeroLine — 팔레트와 상관없는 노랑 · 파랑 파스텔, 모드가 고름) · `Look.OkSubFill` · `OkSubLine`(잘됨 옅은 판, 늘 청록) · `Look.SkySubFill`(하늘 옅은 면), `Button.HCompact` 36, `Chip.View`(보기 칩 28 · 흰 면 변형, 치수 `CSCommonDesign.ViewChip*`), `Badge.Pair`(파스텔 짝을 직접 주는 상태 알약). 추가만 — 기존 이름 · 값은 그대로.
- **토큰 추가(2026-11-13, F1 4.0 레이스 · 레이더 판):** `Look.WarnSubFill` · `WarnSubLine`(경고 옅은 판 면 · 1px 선, 늘 주황 — 코션 판) · `Look.SkyHeroFill`(하늘 히어로 면, 선은 `SkySubFill` — 레이더 도착 예측) · `Look.PastelRowFill`(파스텔 줄 강조 면 — 타이밍 표 선두 줄). 추가만 — 기존 이름 · 값은 그대로.
- **토큰 추가(2026-11-13, F1 4.0 방송 연출 · 팬 · 팀 · 설정):** `Pal.Fixed*` 방송 고정색(`FixedSectorRed` · `Blue` · `Yellow` · `FixedOvertake` · `FixedBroadcastInk` · `Panel` · `Card` · `Track` · `Grid` · `GridGlow`, 지구 `FixedGlobeSeaDeep` · `Sea` · `Land` · `Space` · `SpaceGlow` · `Halo` — 테마와 상관없음) · `Pal.CatRed` · `CatViolet` · `CatBlue` · `CatTeal`(분류 색 넷, 라이트 값 따로) · `Look.PastelPickFill` · `PastelPickLine`(파스텔 고른 칸 면 · 선) · `Look.MeterTrack`(값 막대 홈) · `CSCommonDesign.ChartDimAlpha` 0.75(차트 선 흐림). 추가만 — 기존 이름 · 값은 그대로.
- **글 넘침 안전망(2026-11-14):** 한 줄 글(`Draw.Text` · `TextFit` · `TextBrand` · `TextWide`)이 칸보다 길면 칸 밖으로 넘치거나 잘리지 않게 `CSCommonDesign.FontSqueezeMin` 9 까지 줄임(예전엔 `TextFit` 하한에서 칸 밖으로 넘쳐 그림). 하한(`FontMin`) 아래로 줄면 자세한 로그에 칸 · 문구를 고칠 곳으로 남김. 설정 패널 정보 줄은 배지 몫만큼 이름 글 폭을 줄여 값이 이름과 겹치지 않음.
- **공통 부품:** `InputBlocker`(모드 7벌 → 1벌, `Begin(모드 이름)` · 값 바뀔 때만 대입 · 맨 앞은 보일 때와 30틀마다 · `At`), `FontPatch`(F1에서 옮김, 글꼴 재빌드 알림 스택 넘침 · 프리징 방지, 여러 모드가 켜져도 한 번만 붙음), `PerfWatch`(할당 없는 계측, N초 평균 · 최대, 50ms 즉시, 로그 함수 `PerfWatch.Log` 바꿔 끼움), `Scan`(건물 전수 스캔 공유 · 프레임 분할 · 구독, 모드 컨트롤러 Update 에서 `Scan.Tick()`, 자가 검사 포함).

## UX 1.3 변경 (2026-11-10)

Dockbar 정리 · Section 접기 제거 · TextPill · RoundBottom · Draw.Block · Select 닫힘 애니메이션 · 눌림 되튐 · SubDockBar 정렬. 세부는 커밋 본문 참고.

## UX 1.1 (B안 선명 대비)

- 라이트: 보조 글 8.1:1 · 흐린 글 띠 위 4.9:1, 헤어라인 #D3D7DC · 띠 #E4E7EB, 입력 칸 · 떠 있는 판 테두리 #B9BFC7
- 다크: 보조 글 9.4:1 · 흐린 글 6.6:1, 헤어라인 #343A44, 입력 칸 · 떠 있는 판 테두리 #4A515D
- 주 단추는 주 색(청록 · F1 빨강)으로 꽉 채우고 OnPrimary 글
- 단추 문구(아이콘 + 글 묶음)는 늘 칸 정가운데

## 하단 툴바 · 서브 툴바 · 독 (`CSCommonUX.Toolbar` · `SubToolBar` · `Dockbar` · `SubDockBar`)

셋 다 글래스 · 그림자 없이 불투명 판(`Look.BarSurface`) + 1px 선(`Look.BarLine`)만 씁니다(치수 · 계산 · 뜯어져 나오기는 `PillBar`, 파일 `UI/CSCommonUXToolbar.cs` · `UI/CSCommonToolbar.cs`). 메인 알약은 높이 64 · 안쪽 8 · 칸 48 · 반지름 = 높이/2 · 칸 사이 2이고, 안 고른 칸은 아이콘만(폭 44), 고른 칸은 모드 파스텔 채움(`Look.PastelFill` · `PastelInk`) + [아이콘 24 · 8 · 이름 14 굵게] 한 덩어리를 가운데에 두며 폭 = 24 + 8 + 글폭 + 28 입니다. 고른 칸을 옮기면 칸 폭이 부드럽게 늘고 줄며 글은 열린 만큼 나타나고, 누르면 0.96배, 안 고른 칸 호버는 옅은 알약입니다.

- `Toolbar.Bar` — 모드 창 하단(창 아래 14). 설정은 탭에서 빼 알약 오른쪽 12 에 지름 64 FAB(고르면 파스텔), 알약 + FAB 를 묶어 가운데 정렬. 스크롤 끝 안전 영역 `Toolbar.SafeH`.
- `SubToolBar.Bar` — 메인 위 12 에 높이 48 · 안쪽 8 · 칸 32(반지름 16) · 칸 폭 = 글폭 + 28(최소 64) · 칸 사이 4 · 글 13, 고른 칸 서브 파스텔(`PastelSubFill` · `PastelSubInk`). 메인 뒤(아래 가운데 기준)에서 가로 62% · 세로 55% · 아래로 68 숨어 있다 0.46초 동안 올라오며 끝에서 살짝 튕기고(투명 0.22초), 없어질 때 거꾸로 들어가며 다 나오기 전에는 클릭을 받지 않습니다. 안전 영역 `SubToolBar.SafeH`.
- `CSCommonToolbar.Dockbar` — 지도 도구 독. 위에서 아래로 [도구 알약 + 오른쪽 12 FAB 줄(설정 막대 `PillBar.IconGroup` · 되돌리기 파랑 · 취소 빨강 · 확정 = 모드 파스텔 · 모드 나가기 = 판 면 + 빨강 아이콘, 맨 오른쪽)] → 12 → `SubDockBar` 옵션 막대(높이 48 · 칩 32 · 반지름 24 · 칩 사이 4, 이름 글 `PillBar.SubText` · 구분선 `SubSep`) → 12 → 안내 알약(높이 34 · 반지름 17 · 좌우 16 · 굵은 머리 + 6 + 설명, 문구의 첫 세 칸 띄움 앞이 머리). 옵션 막대는 메인 뒤에서 **아래로**(위 가운데 기준 · 숨으면 위로 68) 뜯어져 나오고, 옵션이 있는 도구끼리 바꾸면 접히지 않고 폭만 바뀝니다. 옵션 막대가 없으면 안내 알약이 메인 아래 12 로 올라옵니다. `top` 을 주면 그 높이에 붙고(주차장 · 지구 그리기), 안 주면 화면 아래에 붙어 옵션 · 안내 자리를 메인 아래에 비워 둡니다(F1 서킷 에디터 · 플레이스메이커).

파스텔 값은 `CSCommonTheme.Use(light, palette)` 가 청록(정치 · 주차장) · F1 빨강 · 하늘(기상청) · 보라(지구 그리기) · 금색(올림픽) 팔레트에 맞춰 채웁니다.

## 모달 (`CSCommonUX.Modal`)

`Modal.Frame` 은 화면 덮개(`Look.Scrim` — 다크 검정 55% · 라이트 35%) 위 가운데 판을 그립니다: 폭 최대 560(`Modal.Place` 가 화면이 좁으면 줄이고 매번 가운데) · 모서리 14 · 불투명 면(`Look.FieldFocus`) · 1px 선(`Look.BarLine`) · 모달에만 그림자(다크 0 10 26 검정 50% · 라이트 0 8 22 검정 12%). 머리 56 = 왼쪽 20 제목 17 굵게 · 12 뒤 설명 12.5 보조색 · 오른쪽 16 지름 32 원 닫기(회색 알약 면 + X 16). 몸은 좌우 20 · 위 4 · 아래 16 · 요소 사이 12 이고, 몸 부품은 `Modal.Tabs`(알약 홈 + 고른 칸 파스텔 알약 36) · `Textfield`(알약 40) · 타일 격자(4열 · 높이 72 · 사이 8 — 칸은 `CSCommonUX.Tile.In`, 반지름 20 · 고르면 파스텔 · 글 색 `Tile.Ink` · `Tile.Sub`)입니다. 발(`footer`)은 64 · `Look.Foot` 면 + 위 1px `Look.FootLine`, `Modal.FootButton` 이 오른쪽부터 [주 = 모드 파스텔 알약 40] [보통 = 회색 알약 40] 을 8 간격 · 좌우 16 으로 놓고 아이콘 + 글을 한 덩어리로 가운데 둡니다. 열 때 10 아래에서 0.18초, 닫을 때 0.14초(`CSCommonAnim.Modal` · `BeginModal`, 곡선 `Modal.Ease` = cubic-bezier(.2,.8,.2,1)), 단추 눌림 0.96배. 캘린더형은 `Modal.Sheet`(폭 448 · 머리 띠 44 `Look.Foot` · 가운데 6점 손잡이 · 원 닫기, 몸 좌우 24) + `SheetTitle`(밑줄 제목 56 · 글 22 · 왼쪽 40 들임 · 초점 2px 파스텔) · `SheetTabs`(32 알약) · `SheetRow`(48 · 아이콘 20 · 호버 면) · 발 `More`("옵션 더보기" ↔ "간단히 보기" + 쉐브론 180°) · `Save`(파스텔 알약 · 누르면 1.6초 ✓ 저장됨)이고, `Modal.Expand` 가 폭 448 → 640 · 추가 영역 0 → 230 을 0.44초 같은 곡선으로 펼치며 추가 영역 투명도는 0.28초(열 때만 0.14초 지연, `BeginExtra` · `EndExtra`)입니다. Sheet · Expand 이름은 시안 제안(확정 전)입니다. 알림 · 변경 내역 창(`CSCommonDialog`)도 같은 틀을 씁니다.

## 글 칸 (`CSCommonUX.Textfield`)

`Textfield.In` 은 알약 칸입니다: 높이 40 · 반지름 = 높이/2 · 안쪽 왼쪽 16 · 오른쪽 8 · 요소 사이 4 · 글 14.5 · 그림자 없음. 바탕 `Look.Pill`(라이트 #F1F3F4 · 다크 #1B1F26), 호버는 안쪽 1px `Look.PillHoverLine`, 초점은 바탕 `Look.FieldFocus` + 안쪽 1.5px 모드 파스텔 테두리(`Look.PastelRing` — 청록 라이트 #0B4F47 · 다크 #B8EFE6, 커서도 같은 색), 잘못은 안쪽 1.5px 빨강(`Pal.Red`, 초점이어도 유지), 꺼짐은 `Look.Off` 바탕 · 글 · 단위 `OffText` · 호버/오류 테두리 없음. 호버 · 초점 전환은 0.12초. 앞 아이콘 16(Dim, 오른쪽 4), 앞뒤 단위는 값이 있을 때만 굵은 보조색, 지우기 ✕ 는 초점이고 값이 있을 때만 지름 28 원(아이콘 14 · 누름 영역 = 칸 오른쪽 끝 · 눌림 표시 없음). `Labeled` 는 위 이름 12 보조색 · 아래 도움말 12 · 높이 18(잘못이면 빨강).

## 옵션 패널 머리 마크

- `CSCommonOptionsPanel.LogoIcon`(SVG path, viewBox 24)을 넘기면 로고 그림이 없는 모드 머리에 28px 둥근 타일 + 아이콘이 붙음. 둘 다 없으면 자리를 비움(예전 빨강 "F1" 글 제거).

## 쓰는 법

모드 저장소 뿌리에 `CSCommon` 폴더를 그대로 복사하고, 모드 `.csproj` 에서 `CSCommon.projitems` 를 가져옵니다. 이 저장소가 원본이며, 모드 쪽 폴더를 고치면 여기에도 같이 올립니다.

## 라이선스

[MIT](LICENSE) — 자유롭게 쓰고 고치고 배포할 수 있습니다(저작권 표기 유지).
