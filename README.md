> [!CAUTION]
> **수정 금지 · 배포 금지.** 이 저장소는 공개되어 있지만 오픈소스가 아닙니다. 원저작자의 허락 없이 코드를 고치거나, 복사 · 재배포 · 다른 곳에 올리기(Steam 창작마당 포함) · 판매를 할 수 없습니다. 허락 없이 할 수 있는 일은 열람뿐입니다. → [라이선스](#-라이선스) · [LICENSE](LICENSE)

<div align="center">

# 🎨 CSCommon UX 3.8.1

### 시티즈: 스카이라인 모드 공용 UI · 디자인 정본

**모드 9개가 같은 모양 · 같은 치수로 그려지게 합니다.**<br>
색 · 치수 토큰과 버튼 · 입력 칸 · 모달 · 툴바 부품을 한곳에 두고, 모드는 git 서브모듈로 가져다 씁니다.

![UX](https://img.shields.io/badge/UX-3.8.1-2ED4BF?style=for-the-badge)
![License](https://img.shields.io/badge/라이선스-수정·배포%20금지-C62828?style=for-the-badge)
![Mods](https://img.shields.io/badge/사용%20모드-9개-5CC8FF?style=for-the-badge)

![Cities: Skylines](https://img.shields.io/badge/Cities%3A%20Skylines-1.21-1F6FEB?style=flat-square)
![.NET Framework 3.5](https://img.shields.io/badge/.NET%20Framework-3.5-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Language](https://img.shields.io/badge/언어-한국어-7F8894?style=flat-square)

[라이선스](#-라이선스) · [구성](#-구성) · [사용하는 모드](#-사용하는-모드) · [부품 규격](#-부품-규격) · [변경 내역](#-변경-내역)

</div>

---

## 🚫 라이선스

**수정 금지 · 배포 금지** — [LICENSE](LICENSE)

- 오픈소스가 아닙니다. 저작권은 원저작자(chbk1348)에게 있습니다.
- 원저작자의 허락 없이 코드를 고치거나, 이를 바탕으로 2차 저작물을 만들 수 없습니다.
- 원저작자의 허락 없이 복사 · 재배포 · 다른 곳에 올리기(Steam 창작마당 포함) · 판매를 할 수 없습니다. 고친 판도 마찬가지입니다.
- 허락 없이 할 수 있는 일은 열람뿐입니다. 허락 요청은 [이슈](https://github.com/chbk1348/CSCommonUX/issues)로 남겨 주세요.
- 이전에 MIT 라이선스로 공개된 판에는 그 당시 조건이 적용됩니다.

---

## 📦 구성

이 저장소(chbk1348/CSCommonUX)가 정본입니다. 모양 정본은 디자인 캔버스 "CSCommonUX 3.0"(비공개)이고, 규격을 바꾸면 코드 · 이 문서 · 캔버스 보드를 같이 고칩니다.

| 파일 | 내용 |
|---|---|
| `CSCommon/Style/Pal.cs` | `Pal` 색 토큰(다크 기본값) |
| `CSCommon/Style/CSCommonTheme.cs` | 다크 · 라이트 테마 값과 모드 팔레트(청록 · F1 빨강 · 보라 · 금색 · 하늘 · 초록)와 툴바 C안 파스텔 |
| `CSCommon/Style/CSCommonDesign.cs` | `CSCommonDesign` 치수 토큰(높이 · 간격 · 모서리 · 글자) |
| `CSCommon/Imgui/Draw.cs` | 그리기 바탕(`Draw` · 글 칸 · `Draw.Curve` 부드러운 선) |
| `CSCommon/UI/Components.cs` | IMGUI 컴포넌트(Button · Textfield · Select · Switch · Slider · Toast · Dialog …) |
| `CSCommon/UI/CSCommonKit.cs` · `CSCommonToolbar.cs` | 게임 UI(ColossalFramework) 부품 · 위 판 툴바 · 독(Dockbar) |
| `CSCommon/UI/CSCommonUXToolbar.cs` | 모드 창 하단 Toolbar · SubToolBar · 상세 화면 뒤로 `Toolbar.BackFab` |
| `CSCommon/UI/InputBlocker.cs` · `Core/FontPatch.cs` · `Core/PerfWatch.cs` · `Core/Scan.cs` | 입력 막기 · 글꼴 패치 · 계측 · 건물 스캔 공통 부품 |
| `CSCommon/Style/CSCommonDesign35.cs` · `Style/Pal.Role35.cs` | 3.5 치수 · 글 · 움직임 토큰(`CSCommonDesign35`) · 역할 색(`Pal.Ground*` · `Ink*` · `Sem*` · `Chart*` · `Accent*`) — 다크 전용 |
| `CSCommon/Imgui/Draw.Num.cs` · `Draw.Chart35.cs` | 3.5 숫자 글(600 · 자리 폭 같음) · 부드러운 선(`Draw.Smooth`) · 위만 둥근 막대(`Draw.RoundTop`) |
| `CSCommon/UI/Components/Surface.cs` 외 | 3.5 부품 `Surface` · `StatusDot` · `Kpi` · `ScoreBar` · `StageTrack` · `EmptyState` · `RowItem` · `Chart35` · 아이콘 45개(`Icons.Set35.cs`) · 자가 검사 `Check35` |
| `CSCommon/Style/CSCommonDesign35.Tabs.cs` · `.Layout.cs` · `UI/Components/Button35.cs` 외 | 3.6 탭 부품 치수 · 배치(열 폭 · 그림 크기) 토큰과 `Button35` · `Inputs35`(고르기 · 스위치 · 체크 · 폼 · 슬라이더 · 스테퍼) · `Tags35` · `Table35` · `Nav35`(탐색 줄 · 화면 머리 · 달력) · `Overlay35`(모달 · 확인 줄 · 토스트 · 툴팁 · 런처) · `Chat35` |
| `CSCommon/DesignCheck.targets` | 모드 코드에 색 값 · 치수 숫자 · 직접 그린 IMGUI 부품이 늘면 빌드 오류(CSC001) |

---

## 🧩 사용하는 모드

| 모드 | 공개 |
|---|---|
| FIA 월드 모터스포츠 챔피언십(F1) | 개인 사용 / 비공개 |
| 날씨 / 기후(기상청) | 개인 사용 / 비공개 |
| 주차장 그리기 | 개인 사용 / 비공개 |
| 지구 그리기 | 개인 사용 / 비공개 |
| 올림픽 | 개인 사용 / 비공개 |
| 거버넌스(정치) | 개인 사용 / 비공개 |
| 환경음 조절기 | 개인 사용 / 비공개 |
| 모드 허브(Yuki Mod Box) | 개인 사용 / 비공개 |
| 프로야구 리그 | 공개 저장소 · 열람만 ([CS-Baseball-Mod](https://github.com/chbk1348/CS-Baseball-Mod)) |

---

## 📐 부품 규격

### 🧱 3.5 토큰 · 부품 (`CSCommonDesign35` · `Pal` 3.5 역할 색 · 다크 전용)

3.0 부품 · 값은 그대로 두고 **추가만** 했습니다(3.0 부품 diff 0). 기준은 FIA 캔버스 '5.0 리뉴얼' R5Tok* · R5Part* 보드. 하단 툴바 · 서브 툴바 · FAB 은 3.1 그대로입니다. 3.5 버튼 · 스위치는 아직 없고, 창 틀 · 상태 칩은 3.5.1 에서 더했습니다.

- **색(`Pal`)**: 바탕 `GroundDeep #0e1014` · `GroundPage #14171c` · `GroundCard #191c22` · `GroundRaised #20242b`, 글 `InkTxt #eceef2` · `InkSub #9aa3af` · `InkDim #848c98`, 선 `Hairline`(#eceef2 8%) · `SelectedTopLine`, 시리즈 강조 `AccentF1` · `AccentNascar` · `AccentWec`(+ `OnAccent*`), 의미색 `SemGood` · `SemWarn` · `SemBad` · `OnSemBad`, 차트 `ChartGray1~4` · `ChartBase` · `ChartGrid` · `ChartCursor` · `ChartEventBand`, 덮개 `Scrim35`. 테마가 바꾸지 않습니다. 시리즈 강조는 부품이 고르지 않고 모드가 인자로 넘깁니다.
- **치수(`CSCommonDesign35`)**: 4px 격자 `Step4~64`, 카드 안쪽 20 · 히어로 · 모달 24 · 제목 → 본문 12 · 묶음 사이 16, 모서리 4 · 8 · 16 · 20, 줄 28 · 36 · 44 · 56 · 64, 글 12/16 · 14/20 · 16/24 · 20/28 · 28/36 · 48/56, 아이콘 12 · 16 · 18 · 24(획 2 하나, 상자 24), 아이콘 ↔ 글 칩 · 버튼 8 · 목록 행 12, 움직임 곡선 `Ease`(cubic-bezier(0.22, 1, 0.36, 1)) · 0.15 / 0.2 / 0.3초 · 누름 0.97배.
- **숫자 `Draw.Num` · `NumWidth`**: 굵기 600(게임 기본 UI 글꼴 = OpenSans-Semibold, 글꼴 파일 없음 · 엔진 굵게 안 씀) · 숫자 0~9 를 가장 넓은 숫자 폭 칸 가운데에(tabular) · 시계 콜론은 점 둘 · 매 틀 할당 0. KPI · 타이밍 · 축 · 수에 씁니다. 한글 제목은 `Draw.Text` 보통 굵기.
- **`Surface`**: `Frame`(Card · Hero · Inner · Slot · Modal · Ground 6종 면 · 선 · 모서리) → `Head`(제목 + 오른쪽 meta) → 몸 → `Foot`(버튼 줄 칸), `Height` · `Raise`(놓인 면보다 한 단 밝은 면 — 칩 · 단계 막대 홈).
- **`StatusDot`**: 줄 안 점 8(`Paint`, 진행 중만 1.2초 1 ↔ 0.4) · 툴바 칸 구석 점(`Corner`, 메인 위 6 · 오른쪽 6 / 서브 위 4 · 오른쪽 6 · 둘레 2 알약 바탕색 — 모드가 `Toolbar.Bar(extra)` · `SubToolBar.Bar(extra, extraW)` 콜백에서 부름) · 서브 칸 안 읽은 수 알약(`Count`, 높이 20 · '99+').
- **`Kpi`**: `Cell`(GroundCard + Hairline · r16 · 라벨 12 → 숫자 28/36 600 → 보조 12) · `Col`(4열 · 사이 16) · 높이 `Kpi.H` 108.
- **`ScoreBar` · `StageTrack`**: 점수 막대 6 / r3(지금 InkSub · 더해질 몫 강조 56%), 단계 · 섹터 3분할 막대 8 / r4 · 칸 사이 4 · 아래 라벨.
- **`EmptyState`**: 원 40 안 아이콘 18(잠김 16) → 12 → 제목 14 → 4 → 설명 14 → 16 → 버튼 칸.
- **`RowItem`**: 줄 44 · 아이콘 18 → 12 → 제목 14 + 캡션 12, 호버 GroundRaised r8(좌우 8 밖) 0.15초, `TeamBar` 4 × 16.
- **`Chart35`**: `Plot`(왼쪽 축 44 · 위 16 · 아래 28 · 오른쪽 범례 88) · `Grid`(가로 눈금선만, 축 숫자는 눈금선 높이에 세로 가운데) · `Line`(Catmull-Rom 부드러운 선 2, 조절점 영역 안, 재사용 버퍼) · `End` · `Cursor`(점선 2 / 4) · `EventBand` · `Legend`(팀 색 막대 3 × 12) · `Gray`(회색 4단계) · `Bar` + `PaintBar`(끝 4 둥글게 · 바닥 0).
- **아이콘**: `Icons` 에 새 45개(Redo · Edit · More · Play · Pause · Stop · Pointer · Up · ArrowLeft · ArrowRight · Home · Grid · List · Lines · Sliders · Window · Lock · Bell · Bug · Eye · Pulse · Trend · Bars · Calendar · Clock · Pin · Map · Road · Bus · User · Users · Flag · Trophy · Star · Shield · Bolt · Bulb · Volume · Music · Mail · Chat · Ticket · Coin · Flame · Heart) — 기존 14개와 합쳐 59개, 경로 글은 캔버스 글자 그대로. `Icons.Paint(경로, 칸, 색)` 이 칸 폭으로 크기 단계를 골라 획 2 로 그립니다.
- **`WindowFrame35`**: 창 틀(1180 × 920 · r20 · 면 `GroundPage` + 바깥 1px `Hairline` · 그림자 없음) — `Frame`(창 판 · 몸 `GroundDeep` 을 칠하고 몸 칸을 돌려줌) · `Body` · `BodyBottom`(몸 스크롤 안쪽 아래 = `Toolbar.SafeH` 88 · 서브 툴바가 있으면 `SubToolBar.SafeH` 148, 위 · 좌우는 `WindowBodyPad` 20) · `Series`(머리 왼쪽 시리즈 전환 칸 — 판 40 `GroundCard` + `Hairline` r8 · 안쪽 4 · 칸 32 r4 · 좌우 12 · 사이 4 · 글 14, 고른 칸 = 강조 면 · 강조 위 글, 안 고른 칸 `InkSub`, 강조는 모드가 넘김) · `Head`(머리 64 · 좌우 16 · 사이 12: 제목 16/24 + 아래 meta 12/16 `InkDim` … 상태 칩 → 날짜 14 `InkSub` 숫자 자리 폭 같음 → 닫기) · `Close`(40 정사각 · 투명 · Close 18 `InkSub`). 호버는 놓인 면보다 한 단 밝은 면(닫기 `GroundCard` · 안 고른 시리즈 칸 `GroundRaised`, `Surface.Raise`) 0.15초, 누르면 0.97배(닫기는 면만 — 아이콘 크기 그대로). 헤더에 뒤로 없음.
- **`Chip35`**: 칩 32 · 좌우 12 · r8 · 점 8 → 8 → 글 14 `InkTxt` 가운데. `Status`(상태 칩 — 점 = 뜻 색 · 진행 중이면 1.2초 오감, 면은 놓인 면보다 한 단을 부르는 쪽이 넘김) · `Width`.
- **자가 검사**: `Check35.SelfCheck(check)` — 곡선 · 축 · 막대 · 숫자 자리 폭 · 상태 점 · 아이콘 59개 중복 0 · 창 틀 몸 칸 · 호버 면 등 29칸.

### 🧭 하단 툴바 · 서브 툴바 · 독 (`CSCommonUX.Toolbar` · `SubToolBar` · `Dockbar` · `SubDockBar`)

셋 다 글래스 · 그림자 없이 불투명 판(`Look.BarSurface`) + 1px 선(`Look.BarLine`)만 씁니다(치수 · 계산 · 뜯어져 나오기는 `PillBar`, 파일 `UI/CSCommonUXToolbar.cs` · `UI/CSCommonToolbar.cs`). 메인 알약은 높이 64 · 안쪽 8 · 칸 48 · 반지름 = 높이/2 · 칸 사이 2이고, 안 고른 칸은 아이콘만(폭 44), 고른 칸은 모드 파스텔 채움(`Look.PastelFill` · `PastelInk`) + [아이콘 24 · 8 · 이름 14 굵게] 한 덩어리를 가운데에 두며 폭 = 24 + 8 + 글폭 + 28 입니다. 고른 칸을 옮기면 칸 폭이 부드럽게 늘고 줄며 글은 열린 만큼 나타나고, 누르면 0.96배, 안 고른 칸 호버는 옅은 알약입니다.

- `Toolbar.Bar` — 모드 창 하단(창 아래 14). 설정은 탭에서 빼 알약 오른쪽 12 에 지름 64 FAB(고르면 파스텔), 알약 + FAB 를 묶어 가운데 정렬. 스크롤 끝 안전 영역 `Toolbar.SafeH`.
- `SubToolBar.Bar` — 메인 위 12 에 높이 48 · 안쪽 8 · 칸 32(반지름 16) · 칸 폭 = 글폭 + 28(최소 64) · 칸 사이 4 · 글 13, 고른 칸 서브 파스텔(`PastelSubFill` · `PastelSubInk`). 메인 뒤(아래 가운데 기준)에서 가로 62% · 세로 55% · 아래로 68 숨어 있다 0.46초 동안 올라오며 끝에서 살짝 튕기고(투명 0.22초), 없어질 때 거꾸로 들어가며 다 나오기 전에는 클릭을 받지 않습니다. 안전 영역 `SubToolBar.SafeH`.
- `CSCommonToolbar.Dockbar` — 지도 도구 독. 위에서 아래로 [도구 알약 + 오른쪽 12 FAB 줄(설정 막대 `PillBar.IconGroup` · 되돌리기 파랑 · 취소 빨강 · 확정 = 모드 파스텔 · 모드 나가기 = 판 면 + 빨강 아이콘, 맨 오른쪽)] → 12 → `SubDockBar` 옵션 막대(높이 48 · 칩 32 · 반지름 24 · 칩 사이 4, 이름 글 `PillBar.SubText` · 구분선 `SubSep`) → 12 → 안내 알약(높이 34 · 반지름 17 · 좌우 16 · 굵은 머리 + 6 + 설명, 문구의 첫 세 칸 띄움 앞이 머리). 옵션 막대는 메인 뒤에서 **아래로**(위 가운데 기준 · 숨으면 위로 68) 뜯어져 나오고, 옵션이 있는 도구끼리 바꾸면 접히지 않고 폭만 바뀝니다. 옵션 막대가 없으면 안내 알약이 메인 아래 12 로 올라옵니다. `top` 을 주면 그 높이에 붙고(주차장 · 지구 그리기), 안 주면 화면 아래에 붙어 옵션 · 안내 자리를 메인 아래에 비워 둡니다(F1 서킷 에디터 · 플레이스메이커).

파스텔 값은 `CSCommonTheme.Use(light, palette)` 가 청록(정치 · 주차장) · F1 빨강 · 하늘(기상청) · 보라(지구 그리기) · 금색(올림픽) 팔레트에 맞춰 채웁니다.

### 🪟 모달 (`CSCommonUX.Modal`)

`Modal.Frame` 은 화면 덮개(`Look.Scrim` — 다크 검정 55% · 라이트 35%) 위 가운데 판을 그립니다: 폭 최대 560(`Modal.Place` 가 화면이 좁으면 줄이고 매번 가운데) · 모서리 14 · 불투명 면(`Look.FieldFocus`) · 1px 선(`Look.BarLine`) · 모달에만 그림자(다크 0 10 26 검정 50% · 라이트 0 8 22 검정 12%). 머리 56 = 왼쪽 20 제목 17 굵게 · 12 뒤 설명 12.5 보조색 · 오른쪽 16 지름 32 원 닫기(회색 알약 면 + X 16). 몸은 좌우 20 · 위 4 · 아래 16 · 요소 사이 12 이고, 몸 부품은 `Modal.Tabs`(알약 홈 + 고른 칸 파스텔 알약 36) · `Textfield`(알약 40) · 타일 격자(4열 · 높이 72 · 사이 8 — 칸은 `CSCommonUX.Tile.In`, 반지름 20 · 고르면 파스텔 · 글 색 `Tile.Ink` · `Tile.Sub`)입니다. 발(`footer`)은 64 · `Look.Foot` 면 + 위 1px `Look.FootLine`, `Modal.FootButton` 이 오른쪽부터 [주 = 모드 파스텔 알약 40] [보통 = 회색 알약 40] 을 8 간격 · 좌우 16 으로 놓고 아이콘 + 글을 한 덩어리로 가운데 둡니다. 열 때 10 아래에서 0.18초, 닫을 때 0.14초(`CSCommonAnim.Modal` · `BeginModal`, 곡선 `Modal.Ease` = cubic-bezier(.2,.8,.2,1)), 단추 눌림 0.96배. 캘린더형은 `Modal.Sheet`(폭 448 · 머리 띠 44 `Look.Foot` · 가운데 6점 손잡이 · 원 닫기, 몸 좌우 24) + `SheetTitle`(밑줄 제목 56 · 글 22 · 왼쪽 40 들임 · 초점 2px 파스텔) · `SheetTabs`(32 알약) · `SheetRow`(48 · 아이콘 20 · 호버 면) · 발 `More`("옵션 더보기" ↔ "간단히 보기" + 쉐브론 180°) · `Save`(파스텔 알약 · 누르면 1.6초 ✓ 저장됨)이고, `Modal.Expand` 가 폭 448 → 640 · 추가 영역 0 → 230 을 0.44초 같은 곡선으로 펼치며 추가 영역 투명도는 0.28초(열 때만 0.14초 지연, `BeginExtra` · `EndExtra`)입니다. Sheet · Expand 이름은 시안 제안(확정 전)입니다. 알림 · 변경 내역 창(`CSCommonDialog`)도 같은 틀을 씁니다.

### ✏️ 글 칸 (`CSCommonUX.Textfield`)

`Textfield.In` 은 알약 칸입니다: 높이 40 · 반지름 = 높이/2 · 안쪽 왼쪽 16 · 오른쪽 8 · 요소 사이 4 · 글 14.5 · 그림자 없음. 바탕 `Look.Pill`(라이트 #F1F3F4 · 다크 #1B1F26), 호버는 안쪽 1px `Look.PillHoverLine`, 초점은 바탕 `Look.FieldFocus` + 안쪽 1.5px 모드 파스텔 테두리(`Look.PastelRing` — 청록 라이트 #0B4F47 · 다크 #B8EFE6, 커서도 같은 색), 잘못은 안쪽 1.5px 빨강(`Pal.Red`, 초점이어도 유지), 꺼짐은 `Look.Off` 바탕 · 글 · 단위 `OffText` · 호버/오류 테두리 없음. 호버 · 초점 전환은 0.12초. 앞 아이콘 16(Dim, 오른쪽 4), 앞뒤 단위는 값이 있을 때만 굵은 보조색, 지우기 ✕ 는 초점이고 값이 있을 때만 지름 28 원(아이콘 14 · 누름 영역 = 칸 오른쪽 끝 · 눌림 표시 없음). `Labeled` 는 위 이름 12 보조색 · 아래 도움말 12 · 높이 18(잘못이면 빨강).

### 🏷️ 옵션 패널 머리 마크

- `CSCommonOptionsPanel.LogoIcon`(SVG path, viewBox 24)을 넘기면 로고 그림이 없는 모드 머리에 28px 둥근 타일 + 아이콘이 붙음. 둘 다 없으면 자리를 비움(예전 빨강 "F1" 글 제거).

### 🧩 3.6 탭 부품 (FIA 5.0 탭 화면 — 대시보드 · 서킷 · 레이스 · 팬 · 팀 · 설정)

3.0 · 3.1 부품과 토큰은 그대로이고 **추가만** 했습니다. 3.5 부품(`Surface` · `StageTrack` · `RowItem` · `Chart35` · `Chip35` · `EmptyState`)은 오버로드 · 새 함수만 더해 기존 호출은 그대로입니다. 강조 · 강조 위 글은 모드가 넘깁니다.

- **토큰**: 숫자 줄 `LineFigure` 32(카드 28/32, KPI 는 36 그대로) · 작은 숫자 `FontStat` · `LineStat` 20/28 600, 선 `ChartMuted`(.16) · `FocusLine`(.32) · `RefLine`(.48), 비 세기 `Rain1~4`, 서킷 그림 `MapPlate` · `MapPlateLine` · `MapGrid` · `MapPitLine` · `MapTrack` · `MapCorner` · `MapCornerLine` · `MapCornerInk` · `MapMissing` · `MapAttrMid` · `MapChip`. 부품 치수는 `CSCommonDesign35.Tabs.cs`, 열 폭 · 그림 크기는 `CSCommonDesign35.Layout.cs`(모드에 숫자 금지).
- **`Button35`**(주 · 보조 · 고스트 · 위험, 40 · 좌우 16 / 작은 32 · 좌우 12, r8 · 글 14 · 아이콘 18 → 8, 누름 0.97 · 꺼짐 = 한 단 면 · InkDim · 초점 링 강조 2 · 바깥 2) · `IconButton35`(32 · 40 정사각) · `TextLink35`(14 → 4 → Right 16, 호버 InkTxt).
- **입력**: `Segmented35`(판 40 · 칸 32 / 작은 32 · 칸 24 · 고른 칸 GroundRaised + 위 선, `Tiles` 등급 칸) · `Switch35`(40 × 24 · 손잡이 16 · 켜짐 강조 면) · `CheckBox35`(18 · r4) · `Field35`(폼 줄 56 · 라벨 96 · 값 칸 40 · 입력 칸 32) · `Select35`(264 × 56 · 목록은 3.0 Select) · `Slider35`(240 × 16 · 홈 4) · `Stepper35`(값 48 · 버튼 32 × 2).
- **표시**: `Chip35.Plain`(점 없음 · 글 색 · 고정 폭) · `Chip35.Status(…, w)`(고정 폭 120) · `Chip35.Filter`(고르기 칩) · `Tag35`(번호 24 · 글 태그 · 차 번호 32 × 20) · `TyreBadge35`(원 20 · 테 1.5) · `Avatar35`(20 ~ 32 · 상태 점 · 봇 칸) · `Meter35`(가는 막대 8 · 4) · `StackBar35`(쌓인 · 몫 막대 12 · 조각 사이 2) · `HBar35`(가로 막대 20 · 끝 r4 · 세로 눈금 · 기준 점선) · `StageTrack`(칸 폭 비율 · 부분 채움 · 라벨 둘 · 섹터 칸 색).
- **줄 · 표 · 머리**: `RowItem.Line`(한 줄 + 캡션 + 꺾쇠) · `Message`(56) · `Step`(44 · 됨 · 지금 · 남음) · `Status`(56 + 오른쪽 칸) · `Value`(24 ~ 44 값 줄) · `Table35`(칸 나누기 · 머리 · 선두 줄) · `Surface.Head`(제목 앞 점 · 오른쪽 작은 버튼) · `Surface.Meta`(점 붙은 meta) · `PageHead35`(56) · `NavRow35`(44 / 36).
- **겹치는 판**: `Modal35`(덮개 Scrim35 · 판 560 / 720 · 머리 56 · 바닥 64 · 0.3초 8 올라옴) · `Notice35`(44) · `Toast35`(52 · r16 · 정보 · 성공 · 경고 · 오류) · `Tooltip35`(32 · 등록은 `CSCommonToolbar.Tip35`) · `Launcher35`(원 40 · 점 8 / 6).
- **달력 · 채팅 · 차트**: `Calendar35`(7열 · 작은 날 32 · 주 120 · 일정 칩 5꼴) · `Chat35`(날짜 줄 · 메시지 · 반응 태그 · 봇 · 알림 줄 · 입력 중) · `ChatInput35`(48) · `EmptyState.Row`(가로형) · `Chart35.Grid(…, ticks, top, bottom)` · `StageLine` · 세로 막대 묶음(`BarPlot` · `BarGrid` · `BarSlot` · `BarValue` · `FutureBar` · `BarLegend`).
- **아이콘**: 15개 더함(Alert · Article · Doc · Hash · Branch · Smile · Swap · Toolkit · Camera · Cloud · Chequer · Circle · NoteCircle · Rings · BusFront) — 74개, 획 2 하나.
- **자가 검사**: 칸 폭 비율 · 섹터 색 · 쌓인 막대 조각 · 가로 막대 · 표 칸 · 달력 · 모달 · 막대 묶음 · 스위치 · 채팅 띠 · 열 폭 합 · 토큰 4px 격자 18칸 더함(47칸).

### 🧩 3.7 탭 토큰 · 꼴 모음 (FIA 5.0 탭 구현 뒤 — 모드가 '정본에 필요'로 남긴 것)

3.6 과 같이 **추가만** 했습니다(기존 시그니처는 그대로, 선택 인자 · 오버로드만). 이름은 쓰임으로 짓고, 같은 쓰임은 탭이 달라도 하나로 합쳤습니다.

- **열 폭**(`CSCommonDesign35.Layout.cs`, 칸 사이 `TableColGap` 12): 순위 `ColRank` 28 · 숫자 `ColNumXs` 32 · `ColNumS` 44 · `ColNum` 48 · 변화 `ColMove` 36 · 짧은 값 `ColShort` 40 · `ColCarWide` 56 · `ColStage` 64 · 간격 `ColGap` 72 · 랩 시간 `ColLapTime` 80 · 약어 `ColCode` 72 · 이름표 `ColLabel` 88 · 이름 `ColNameS` 80 · `ColNameM` 112 · `ColName` 120 · `ColNameL` 128 · `ColNameXl` 152 · 시각 칸 `ColGutter` 56.
- **칸 · 그림 크기**: 줄 오른쪽 칸 `RowSideW` 120 · 카드 오른쪽 칸 `CardSideW` 240 · 차트 넓은 범례 `ChartLegendWideW` 120 · 뉴스 히어로 `NewsHeroH` 296 · 팩트 첫 칸 `FactLeadW` 236 · 포디움 `PodiumRowW` 420 · `PodiumFirstH` 160 / `SecondH` 132 / `ThirdH` 116 · 라운드 점 `RoundDot` 20.
- **부품 치수 · 투명도**(`CSCommonDesign35.Tabs.cs`): 1px 선 `HairW` · 강수 그림 `RainChartH` 140 · `RainPlotH` 100 · 섹터 줄 `RowSector` 64 · `SectorNameW` 24 · 비 세기 `RainAlpha1~4` .28 / .38 / .5 / .56 · 지도 띠 `MapBandAlpha` .22 · `MapBandSoftAlpha` .16 · 순위 변화 차트 `BumpAxisW` 64 · `BumpTop` 12 · `BumpLegendX` 16 / `Wide` 24 · 차트 캡션 줄 `ChartNoteH` 20 · 기사 링크 줄 `RowArticle` 64 · 참가자 줄 `RowMember` 32 · 설명 머리 `PageHeadDescH` 52 · 값 줄 `RowLabelMinW` 240 · `RowValueShare` .4 · 토스트 비켜 가기 `ToastAvoidGap` 8.
- **서킷 그림 치수**(모드 `CircuitMap` 이 씀, 캔버스 값 — 격자 밖 홀수는 자가 검사 예외): `MapTrackW` 18 · `MapTrackMiniW` 6 · `MapLineW` 5 · `MapLineMiniW` 2 · `MapStartW` 3 · `MapCornerD` 20 · `MapPitW` 2 · `MapPitDash` / `Gap` 6 · `MapGridRow` 80 · `MapGridCol` 100 · `MapMiniR` 12 · 핀 `MapPinW` 44 × `MapPinH` 22 · `MapPinR` 11 · 머리 글 `FontMapHead` 15 · 범례 `MapLegendW` 14 × `MapLegendH` 4 · `MapLegendGap` 6.
- **색**: `Pal.LauncherRing`(게임 화면 위 런처 점 둘레 — GroundDeep 과 같은 값).
- **꼴 · 옵션**
  - `ChatInput35.Paint(r, value, max, placeholder, accent, onAccent, sendTip, out send, name)` — 반응 버튼 없는 입력 줄.
  - `Toast35.Flush(avoidA, avoidB)` · `Toast35.AvoidY` — 아래 독 · 안내 칸과 겹치면 그 위 8 로 비켜 그림(`Flush()` 는 그대로).
  - `EmptyState.Height(…, smallButton)` · `EmptyState.Paint(…, smallButton)` — 버튼 줄 32(작은 보조 버튼).
  - `Surface.Head(…, titleInk:)` — 제목 색 인자(기본 · 점 붙은 머리). 위치 인자 `Color` 는 점 붙은 머리로 가니 이름으로 넘깁니다.
  - `PageHead35.Desc(r, title, desc, meta)` — 설명 붙은 머리 52(제목 20 → 4 → 설명 14 · 오른쪽 meta).
  - `Tooltip35.Float(anchor, text, show)` — 창 밖(게임 화면 위 런처) 툴팁, 그 자리에서 0.4초 머물면 그림.
  - `Chat35.BotLine(r, name, body)` · `Chat35.BotLineH` 52 — 관리봇 한 줄(왼쪽 44 · Shield 16 · 이름 · 글 InkSub).
- **자가 검사**: 토스트 비켜 가기 · 작은 버튼 빈 상태 · 관리봇 줄 · 설명 머리 · 열 폭 단계 · 투명도 단계 · 포디움 · 강수 그림 · 런처 둘레 6칸 더함(53칸), 새 토큰도 4px 격자 검사에 들어갑니다.

### 📺 3.8 방송 카드 토큰 · 순위 변화 칸 · 포디움 칸 (FIA 5.0 방송 오버레이 B1 · B2 · B3 · B5)

3.7 과 같이 **추가만** 했습니다(기존 시그니처는 그대로, 오버로드 · 새 부품만). 값은 FIA 캔버스 R5BcLayout · R5BcBattle · R5BcPit · R5BcDelta · R5BcPost 의 게임 화면 1920 × 1080 논리 px 그대로입니다.

- **방송 카드 토큰**(`CSCommonDesign35.Tabs.cs`): 카드 폭 `CastCardW` 400 · 화면 오른쪽 `CastEdge` 24 · 화면 아래 `CastBottom` 120(아래 맞춤) · 높이 배틀 `CastBattleH` 252 · 피트 스톱 `CastPitH` 212 · 카드 바꿈 이동 `CastSwapShift` 8 · 양쪽 칸 `CastSideW` 120 · 번호 태그 `CastTagW` 24 · 간격 추이 칸 `CastSparkH` 64 · 라벨 칸 `CastSparkLabelW` 96 · 피트 안쪽 칸 `CastStopW` 120 · `CastCellW` 112.
- **퀄리 델타 바 토큰**: `DeltaBarW` 320 × `DeltaBarH` 76 · 랩 패널과 사이 `DeltaGap` 8 · 델타 글 칸 `DeltaValueW` 80 · 막대 끝 값 `DeltaRange` ±0.5초 · 변화 칸 아이콘 → 수 `MoveGap` 2.
- **B5 결승 뒤 장면 토큰**(`CSCommonDesign35.CastPost` — 방송 연출 보드 1600 × 900 px, 인트로 방송 룩이라 3.5 글 크기 · 4px 격자 밖이어서 따로 둠): 띠 칸 `Col2X` 560 · `Col3X` 860 · 구분선 `Div1X` 520 · `Div2X` 820 · 체커기 `FlagD` 40 · 우승 팀 막대 `WinnerBarW` 6 × `WinnerBarH` 30 · 결과 기다림 글 `WaitFont` 24 · 기다린 시간 막대 `ProgressH` 4 · 건너뛰기 `SkipRight` 44 · `SkipTopBand` 40 / `SkipTopPlate` 128 · `SkipFont` 16 · 표 `RowX` 240 · `RowW` 1120 · `RowTop` 148 · 머리 `HeadY` 120 · 변화 `ChangeW` 60 · 팀 `TeamW` 160 · 이번 라운드 `RoundW` 96 · 점수 `PtsW` 120 · 시상대 `PodiumX` 236 · `PodiumW` 360 · `PodiumGap` 24 · 높이 360 / 300 / 260 · 바닥 `PodiumBottom` 760 · 칸 안 글 30 / 32 / 20. 줄 50 · 간격 58 · P 칸 68 · 띠 머리 42 · 몸 95 처럼 인트로에 이미 있는 값은 모드 인트로 상수를 그대로 씁니다.
- **부품**
  - `Move35.Paint(r, change, count)` · `Move35.Icon(change)` · `Move35.Ink(change)` — 순위 변화 칸(폭 `ColMove` 36 · 아이콘 12 → 2 → 수 12 · 오름 Up SemGood · 내림 Chevron SemBad · 그대로 Minus InkDim, 수 없음). 방송 판은 Icon · Ink 만 골라 씁니다.
  - `Podium35.Slot(left, bottom, place, w, gap, first, second, third)` · `Podium35.Column(place)` — 포디움 칸 자리(2 · 1 · 3위 차례 · 폭 같음 · 바닥 맞춤). 판 · 글은 부르는 쪽 룩으로 그립니다.
  - `Meter35.Signed(r, v, range, fill, home)` · `Meter35.SignedFill(track, v, range)` — ± 막대(가운데 0 · 홈 8 r4 · 가운데 표시 2 InkDim).
  - `Tag35.IconLabel(centerX, cy, icon, iconInk, text, face)` — 아이콘 글 태그 24(좌우 8 · 아이콘 12 → 4 → 글 12).
  - `Chart35.End(p, accent, ring)` — 놓인 면이 GroundCard 가 아닐 때 선 끝 표식(반지름 4 · 테 1).
- **자가 검사**: 배틀 카드 252 · 피트 카드 212 · 델타 바 320 × 76 칸 합 · ± 막대 채움 · 순위 변화 칸 · 포디움 칸 자리 · 결승 뒤 장면 배치 7칸 더함(60칸), 새 토큰도 4px 격자 검사에 들어갑니다.

---

## 📝 변경 내역

<details open>
<summary><b>UX 3.8.1 변경 (2026-11-28)</b></summary>

- **토스트 앞 판 등록(고침):** `Toast35.Flush` 가 그린 자리를 `Draw.Block` 으로 앞 판에 남기고 그 안에서 그림. 토스트 밑에 있던 창 단추 · 목록이 토스트 위 클릭 · 호버를 함께 받던 것을 막음 — 토스트 단추는 그대로 눌림. 시그니처 · 치수 · 색은 그대로.
- **판:** 설정 패널 머리 · 켜짐 로그의 CSCommonUX 판을 3.8.1 로.

</details>

<details>
<summary><b>UX 3.8 변경 (2026-11-28)</b></summary>

- **방송 카드 토큰 · 순위 변화 칸 · 포디움 칸 · B5 결승 뒤 장면 토큰(추가만):** 위 「3.8 방송 카드 토큰」 절. 기존 부품 · 토큰 · 시그니처는 그대로라 다른 모드 화면은 바뀌지 않음.
- **판:** 설정 패널 머리 · 켜짐 로그의 CSCommonUX 판을 3.8 로.

</details>

<details>
<summary><b>UX 3.7 변경 (2026-11-28)</b></summary>

- **탭 토큰 · 꼴 모음(추가만):** 위 「3.7 탭 토큰 · 꼴 모음」 절. 기존 부품 · 토큰 · 시그니처는 그대로라 다른 모드 화면은 바뀌지 않음.
- **넣지 않음:** `Menu35`(펼친 목록 3.5) — 캔버스 규격이 없어 다음으로 미룸(지금은 3.0 `Select` 목록).
- **판:** 설정 패널 머리 · 켜짐 로그의 CSCommonUX 판을 3.7 로.

</details>

<details>
<summary><b>UX 3.6 변경 (2026-11-27)</b></summary>

- **탭 부품 추가(추가만):** 위 「3.6 탭 부품」 절. 3.0 · 3.1 부품 · 토큰은 그대로라 다른 모드 화면은 바뀌지 않음. `CSCommonToolbar` 에는 3.5 툴팁 등록(`Tip35`)과 그리기 분기만 더함 — 기존 `Tip` 은 그대로.
- **판:** 설정 패널 머리 · 켜짐 로그의 CSCommonUX 판을 3.6 으로.

</details>

<details>
<summary><b>UX 3.5.2 변경 (2026-11-27)</b></summary>

- **창 틀 호버 · 누름(추가만):** `WindowFrame35.Close` · `Series` 안 고른 칸에 마우스를 올리면 놓인 면보다 한 단 밝은 면(`Surface.Raise` — 새 색 토큰 없음), 누르면 0.97배.
- **자가 검사:** 호버 면 한 칸 더함(29칸).
- **판:** 설정 패널 머리 · 켜짐 로그의 CSCommonUX 판을 3.5.2 로.

</details>

<details open>
<summary><b>UX 3.5.1 변경 (2026-11-27)</b></summary>

- **창 틀 3.5 추가(추가만):** `WindowFrame35`(창 판 · 몸 · 머리 · 시리즈 전환 칸 · 닫기) · `Chip35`(상태 칩) · `CSCommonDesign35` 창 머리 · 닫기 · 시리즈 칸 · 칩 치수. 3.0 `WindowHead` · `Panel` · `Segmented` 와 3.1 툴바는 그대로라 다른 모드 화면은 바뀌지 않음.
- **자가 검사:** 창 틀 몸 칸 · 툴바 안전 영역 한 칸 더함(28칸).
- **판:** 설정 패널 머리 · 켜짐 로그의 CSCommonUX 판을 3.5.1 로.

</details>

<details open>
<summary><b>UX 3.5 변경 (2026-11-27)</b></summary>

- **3.5 토큰 · 부품 추가(추가만):** 위 「3.5 토큰 · 부품」 절. 새 파일만 더하고 `Pal` · `Icons` 선언에 `partial` 한 단어씩, `CSCommon.projitems` 에 새 파일 줄만 더함 — 3.0 · 3.1 부품 · 툴바는 그대로라 모드는 서브모듈을 올려도 화면이 바뀌지 않음.
- **판:** 설정 패널 머리 · 켜짐 로그의 CSCommonUX 판을 3.5 로.

</details>

<details open>
<summary><b>UX 3.0 변경 (2026-11-15)</b></summary>

- **상세 화면 뒤로 FAB:** `Toolbar.BackFab` — 탭 메인에서 들어가는 상세 화면의 하단 툴바 왼쪽 뒤로가기 원 버튼(올림픽에서 쓰던 계산을 공용으로). 툴팁은 돌아갈 탭 이름.
- **곡선 그리기:** `Draw.Curve` + `CSCommonDesign.CurveSteps` — 점 사이 가운데를 조절점으로 쓰는 부드러운 선 그래프.
- **입력 칸 단축키 막기:** `Draw.Field` · `InputBlocker` — 글 칸에 초점이 있으면 게임 단축키 · 카메라 키를 막음.
- **야구 토큰:** `CSCommonPalette.Green`(라이트 · 야구 초록) · 팀 색 `Pal.Ball*` · 볼카운트 색 · 화면 치수 토큰. 추가만 — 기존 이름 · 값은 그대로.

</details>

<details>
<summary><b>UX 3.0 변경 (2026-11-11)</b></summary>

- **성능:** `Draw.TextWidth` · `TextHeight` 캐시(`widthCache` · `heightCache`), 아이콘 · 말줄임 캐시 키를 `TextKey` 로, `Draw.Field` GUIStyle 캐시(`fieldStyles`). 모드 사본 9곳에 같이 반영.
- **툴바:** `DockTool` 툴팁 문자열을 마우스를 올렸을 때만 조립. `Fab` · `SubChip` · `IconGroup` 애니메이션 키를 자리와 무관하게 바꿔 `slots` · `presses` 가 계속 커지지 않게 함. 네임스페이스 `CSCommon.PillBar` → `CSCommonUX.PillBar`.
- **Section:** 제목 칩과 본문 사이 `HeadGap` 6 → 8, 칩 오른쪽 안쪽 `ChipPadR` 14 → 16. 옵션 패널의 부연 설명(`desc`)은 제목 줄 맨 오른쪽 회색 보조 알약으로 옮김(제목이 없을 때만 본문에 그림).
- **입력:** 마우스 뗌 판정 한 프레임 늦춤(`releaseFrame`), `Hover` 가 잘린 영역(`visible`)까지 봄.
- **토큰 추가(2026-11-13, F1 3.0 보드):** `Look.HeroLine`(히어로 1px 선) · `Look.SeriesYellow*` · `Look.SeriesBlue*`(Fill · Ink · Hero · HeroLine — 팔레트와 상관없는 노랑 · 파랑 파스텔, 모드가 고름) · `Look.OkSubFill` · `OkSubLine`(잘됨 옅은 판, 늘 청록) · `Look.SkySubFill`(하늘 옅은 면), `Button.HCompact` 36, `Chip.View`(보기 칩 28 · 흰 면 변형, 치수 `CSCommonDesign.ViewChip*`), `Badge.Pair`(파스텔 짝을 직접 주는 상태 알약). 추가만 — 기존 이름 · 값은 그대로.
- **토큰 추가(2026-11-13, F1 4.0 레이스 · 레이더 판):** `Look.WarnSubFill` · `WarnSubLine`(경고 옅은 판 면 · 1px 선, 늘 주황 — 코션 판) · `Look.SkyHeroFill`(하늘 히어로 면, 선은 `SkySubFill` — 레이더 도착 예측) · `Look.PastelRowFill`(파스텔 줄 강조 면 — 타이밍 표 선두 줄). 추가만 — 기존 이름 · 값은 그대로.
- **토큰 추가(2026-11-13, F1 4.0 방송 연출 · 팬 · 팀 · 설정):** `Pal.Fixed*` 방송 고정색(`FixedSectorRed` · `Blue` · `Yellow` · `FixedOvertake` · `FixedBroadcastInk` · `Panel` · `Card` · `Track` · `Grid` · `GridGlow`, 지구 `FixedGlobeSeaDeep` · `Sea` · `Land` · `Space` · `SpaceGlow` · `Halo` — 테마와 상관없음) · `Pal.CatRed` · `CatViolet` · `CatBlue` · `CatTeal`(분류 색 넷, 라이트 값 따로) · `Look.PastelPickFill` · `PastelPickLine`(파스텔 고른 칸 면 · 선) · `Look.MeterTrack`(값 막대 홈) · `CSCommonDesign.ChartDimAlpha` 0.75(차트 선 흐림). 추가만 — 기존 이름 · 값은 그대로.
- **글 넘침 안전망(2026-11-14):** 한 줄 글(`Draw.Text` · `TextFit` · `TextBrand` · `TextWide`)이 칸보다 길면 칸 밖으로 넘치거나 잘리지 않게 `CSCommonDesign.FontSqueezeMin` 9 까지 줄임(예전엔 `TextFit` 하한에서 칸 밖으로 넘쳐 그림). 하한(`FontMin`) 아래로 줄면 자세한 로그에 칸 · 문구를 고칠 곳으로 남김. 설정 패널 정보 줄은 배지 몫만큼 이름 글 폭을 줄여 값이 이름과 겹치지 않음.
- **허브 공유(2026-11-12):** 모드 허브가 AppDomain 데이터(`CSCommonUX.Hub.v1`)에 표를 올리면 `InputBlocker` 패널을 허브가 한 벌만 깔고(`Host` · `HostFrame`, 모드는 판 칸만 올림), `Draw.Begin` 이 그린 순서를 남겨 위에 그려진 다른 모드 칸 안에서는 `Hover` · `UnderOverlay` 가 받지 않음. 허브가 없으면 예전처럼 모드 혼자.
- **서브 툴바 다시 나타남 고침(2026-11-12):** 한 틀이 길어 0.25초 넘게 안 불렸어도 바로 앞 틀에 불렸으면 뜯어져 나오기 · 등장을 처음부터 다시 하지 않음.
- **Stepper · Badge(2026-11-12):** `Stepper.In` 에 `enabled`(잠금) · `canMinus` · `canPlus`(끝값), `Badge.At` 에 `dot`(상태 점 6) · `minW`(최소 폭). 기본값 = 예전 동작.
- **설정 패널 진행 막대(2026-11-13):** `CSCommonSection.Progress` — 막대 + 오른쪽 글, 일하는 동안 앞에 스피너, 다른 스레드가 올리는 값도 그릴 때마다 읽음.
- **코드 칩 국기 · 지도 표식(2026-11-14):** `Section.CodeFlag`(코드 → 국기 그림)를 주면 [국기 + 코드] 칩, 폭은 가장 넓은 코드 기준(`Section.CodeSpan`). `Pal.MapTag` · `MapTagInk` · `MapTagSub` · `MapTagShadow` — 게임 화면 위 흰 표식. 확장점이 없으면 예전 그대로.
- **공통 부품:** `InputBlocker`(모드 7벌 → 1벌, `Begin(모드 이름)` · 값 바뀔 때만 대입 · 맨 앞은 보일 때와 30틀마다 · `At`), `FontPatch`(F1에서 옮김, 글꼴 재빌드 알림 스택 넘침 · 프리징 방지, 여러 모드가 켜져도 한 번만 붙음), `PerfWatch`(할당 없는 계측, N초 평균 · 최대, 50ms 즉시, 로그 함수 `PerfWatch.Log` 바꿔 끼움), `Scan`(건물 전수 스캔 공유 · 프레임 분할 · 구독, 모드 컨트롤러 Update 에서 `Scan.Tick()`, 자가 검사 포함).

</details>

<details>
<summary><b>UX 1.3 변경 (2026-11-10)</b></summary>

Dockbar 정리 · Section 접기 제거 · TextPill · RoundBottom · Draw.Block · Select 닫힘 애니메이션 · 눌림 되튐 · SubDockBar 정렬. 세부는 커밋 본문 참고.

</details>

<details>
<summary><b>UX 1.1 (B안 선명 대비)</b></summary>

- 라이트: 보조 글 8.1:1 · 흐린 글 띠 위 4.9:1, 헤어라인 #D3D7DC · 띠 #E4E7EB, 입력 칸 · 떠 있는 판 테두리 #B9BFC7
- 다크: 보조 글 9.4:1 · 흐린 글 6.6:1, 헤어라인 #343A44, 입력 칸 · 떠 있는 판 테두리 #4A515D
- 주 단추는 주 색(청록 · F1 빨강)으로 꽉 채우고 OnPrimary 글
- 단추 문구(아이콘 + 글 묶음)는 늘 칸 정가운데

</details>
