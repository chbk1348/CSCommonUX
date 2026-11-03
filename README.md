# CSCommon UX 1.1

Cities: Skylines 모드(F1 그랑프리 · 환경음 조절기 · 기상청 · 주차장 · 농장 …)가 같이 쓰는 UI 공유 폴더입니다.

- `CSCommon/Style/Pal.cs` — 색 토큰(다크 기본값)
- `CSCommon/Style/CSCommonTheme.cs` — 다크 · 라이트 테마 값과 모드 팔레트(청록 · F1 빨강)
- `CSCommon/Style/CSCommonDesign.cs` — 치수 토큰(높이 · 간격 · 모서리 · 글자)
- `CSCommon/UI/Components.cs` — IMGUI 컴포넌트(Button · Textfield · Select · Switch · Slider · Toast · Dialog …)
- `CSCommon/UI/CSCommonKit.cs` · `CSCommonToolbar.cs` — 게임 UI(ColossalFramework) 부품 · 툴바
- `CSCommon/DesignCheck.targets` — 모드 코드에 색 값 · 치수 숫자 · 직접 그린 IMGUI 부품이 늘면 빌드 오류(CSC001)

## UX 1.1 (B안 선명 대비)

- 라이트: 보조 글 8.1:1 · 흐린 글 띠 위 4.9:1, 헤어라인 #D3D7DC · 띠 #E4E7EB, 입력 칸 · 떠 있는 판 테두리 #B9BFC7
- 다크: 보조 글 9.4:1 · 흐린 글 6.6:1, 헤어라인 #343A44, 입력 칸 · 떠 있는 판 테두리 #4A515D
- 주 단추는 주 색(청록 · F1 빨강)으로 꽉 채우고 OnPrimary 글
- 단추 문구(아이콘 + 글 묶음)는 늘 칸 정가운데

## 쓰는 법

모드 저장소 뿌리에 `CSCommon` 폴더를 그대로 복사하고, 모드 `.csproj` 에서 `CSCommon.projitems` 를 가져옵니다. 이 저장소가 원본이며, 모드 쪽 폴더를 고치면 여기에도 같이 올립니다.

## 라이선스

[MIT](LICENSE) — 자유롭게 쓰고 고치고 배포할 수 있습니다(저작권 표기 유지).
