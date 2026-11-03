using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace CSCommon.UI
{
    /// <summary>
    /// 게임 UI 부품용 그림 모음(실행 중에 그려 만듦, 그림 파일 없음). 모두 흰색이라 부품 색(color)으로 물들임.
    /// 둥근 모서리 · 알약은 9칸 나눔(border)이라 어떤 크기로 늘려도 모서리가 그대로.
    /// </summary>
    internal static class CSCommonAtlas
    {
        public const string Round10 = "UxRound10", Round6 = "UxRound6", RoundLine10 = "UxRoundLine10", RoundLine6 = "UxRoundLine6";
        public const string Pill = "UxPill", PillLine = "UxPillLine", Disc = "UxDisc", White = "UxWhite", Ring = "UxRing";
        /// <summary>얇은 막대(슬라이더 홈 · 채움, 높이 8 안팎) · 아주 얇은 막대(스크롤 손잡이, 폭 4). 9칸 나눔 테두리가 막대보다 크면
        /// 그 크기 밑으로 줄지 않아 막대가 부풀어 그려짐 — 알약(테두리 15)을 얇은 막대에 쓰지 말 것.</summary>
        public const string Bar = "UxBar", Thin = "UxThin";

        static UITextureAtlas atlas;

        public static UITextureAtlas Atlas
        {
            get
            {
                // 도시를 불러올 때 게임이 안 쓰는 자원을 정리(UnloadUnusedAssets)하면 그림만 지워져 부품이 투명하게 보였음(업데이트 알림 창 등)
                // → 지울 수 없게 표시(DontUnloadUnusedAsset)하고, 그래도 재질 · 그림이 사라졌으면 다시 만듦
                if (atlas == null || atlas.material == null || atlas.material.mainTexture == null) Build();
                return atlas;
            }
        }

        static void Build()
        {
            var names = new List<string>();
            var textures = new List<Texture2D>();
            var borders = new List<RectOffset>();
            void Add(string name, Texture2D tex, int border)
            {
                names.Add(name);
                textures.Add(tex);
                borders.Add(new RectOffset(border, border, border, border));
            }

            Add(Round10, RoundRect(32, 10f, 0f), 12);
            Add(Round6, RoundRect(24, 6f, 0f), 8);
            Add(RoundLine10, RoundRect(32, 10f, 1f), 12);
            Add(RoundLine6, RoundRect(24, 6f, 1f), 8);
            Add(Pill, RoundRect(32, 16f, 0f), 15);
            Add(PillLine, RoundRect(32, 16f, 1f), 15);
            Add(Disc, RoundRect(48, 24f, 0f), 0);
            Add(Ring, RoundRect(48, 24f, 11.5f), 0); // 글 속 목록 빈 원(선 = 지름 × 0.24)
            Add(White, Solid(8), 0); // 단색이라 나눌 필요 없음 — 테두리가 있으면 1px 선이 그 두께 밑으로 안 줄어듦
            Add(Bar, RoundRect(12, 4f, 0f), 4);
            Add(Thin, RoundRect(6, 2f, 0f), 2);

            var sheet = new Texture2D(256, 256, TextureFormat.ARGB32, false) { name = "F1UXAtlas", filterMode = FilterMode.Bilinear };
            Rect[] regions = sheet.PackTextures(textures.ToArray(), 2, 512);
            sheet.filterMode = FilterMode.Bilinear;

            var material = Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = sheet;
            atlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            atlas.name = "F1UXAtlas";
            atlas.material = material;
            sheet.hideFlags = material.hideFlags = atlas.hideFlags = HideFlags.DontUnloadUnusedAsset;
            foreach (var t in textures) t.hideFlags = HideFlags.DontUnloadUnusedAsset;
            for (int i = 0; i < names.Count; i++)
            {
                atlas.AddSprite(new UITextureAtlas.SpriteInfo
                {
                    name = names[i],
                    texture = textures[i],
                    region = regions[i],
                    border = borders[i],
                });
            }
        }

        /// <summary>둥근 사각(size × size). line 0 = 채움, 아니면 그 두께 테두리만. 가장자리는 덮인 비율로 부드럽게.</summary>
        static Texture2D RoundRect(int size, float radius, float line)
        {
            var tex = new Texture2D(size, size, TextureFormat.ARGB32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[size * size];
            float half = size * 0.5f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                // 둥근 사각 부호 거리(안쪽 음수)
                float qx = Mathf.Abs(x + 0.5f - half) - (half - radius);
                float qy = Mathf.Abs(y + 0.5f - half) - (half - radius);
                float outside = new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f)).magnitude + Mathf.Min(Mathf.Max(qx, qy), 0f) - radius;
                float a = Mathf.Clamp01(0.5f - outside);
                if (line > 0f) a *= Mathf.Clamp01(0.5f + outside + line);
                px[y * size + x] = new Color32(255, 255, 255, (byte)(a * 255f));
            }
            tex.SetPixels32(px);
            tex.Apply(false);
            return tex;
        }

        static Texture2D Solid(int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.ARGB32, false) { filterMode = FilterMode.Point };
            var px = new Color32[size * size];
            for (int i = 0; i < px.Length; i++) px[i] = new Color32(255, 255, 255, 255);
            tex.SetPixels32(px);
            tex.Apply(false);
            return tex;
        }
    }
}
