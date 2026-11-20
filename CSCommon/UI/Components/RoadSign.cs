using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// CSCommonUX.RoadSign — 게임 화면 도로 표지판(일본 고속도로 안내 표지 기준 · 대중교통 캔버스 RoadSigns): 초록 판 · 흰 테 · 흰 글, 로마자는 한자 높이의 1/2.
    /// Dir = 위 작은 판(번호 · 이름 · 로마자) + 판 여러 장(노선 상자 또는 「出口 EXIT」 · 행선지 · 화살표(갈래 ↖ / 직진 ↑) · 아래 다른 노선 상자). 기둥은 그리지 않음(표지만 떠 있음).
    /// Toll = 요금소 판(料金所 TOLL GATE · ETC 전용 · ETC/一般 · 一般). Marker = 노선 번호 표식. 모두 눌렀다 떼면 true.
    /// </summary>
    public static class RoadSign
    {
        public struct Panel
        {
            /// <summary>노선 상자(빈 글이면 Label 을 씀) · 노선 이름 · 「出口 EXIT」 같은 글 상자 · 행선지 한자 · 로마자 · 갈래(↖, 왼쪽) · 아래 다른 노선 번호.</summary>
            public string Code, Route, Label;
            public string[] Jp, En;
            public bool Exit;
            public string[] More;
        }

        public struct Dir
        {
            public string No, Name, NameEn;
            public Panel[] Panels;
        }

        const float D = CSCommonDesign.SignDestFont, P = CSCommonDesign.SignPlateFont;

        static float W(string s, float size) => string.IsNullOrEmpty(s) ? 0f : Mathf.Ceil(Draw.TextWidth(s, size, true));
        static float BoxW(string s) => W(s, CSCommonDesign.SignBoxFont) + CSCommonDesign.SignBoxH * 0.5f;

        static float PlateW(Dir d) => CSCommonDesign.SignPad * 2f + (string.IsNullOrEmpty(d.No) ? 0f : BoxW(d.No) + CSCommonDesign.SignPanelGap) + W(d.Name, P) + CSCommonDesign.SignPanelGap + W(d.NameEn, P * 0.5f);

        static float DestsW(Panel p)
        {
            float w = 0f;
            for (int i = 0; i < p.Jp.Length; i++) w += Mathf.Max(W(p.Jp[i], D), W(i < p.En.Length ? p.En[i] : "", D * 0.5f)) + (i > 0 ? CSCommonDesign.SignGap : 0f);
            return w;
        }

        static float HeadW(Panel p) => string.IsNullOrEmpty(p.Code) ? BoxW(p.Label) : BoxW(p.Code) + CSCommonDesign.SignPanelGap * 0.5f + W(p.Route, CSCommonDesign.SignRouteFont);

        static float MoreW(Panel p)
        {
            float w = 0f;
            if (p.More != null) foreach (var m in p.More) w += BoxW(m) + CSCommonDesign.SignPanelGap;
            return w;
        }

        static float PanelW(Panel p) => CSCommonDesign.SignPad * 2f + Mathf.Max(Mathf.Max(HeadW(p), MoreW(p)), CSCommonDesign.SignArrowW + CSCommonDesign.SignGap + DestsW(p));

        /// <summary>표지 아래 끝 가운데가 foot(논리 좌표)일 때 표지 전체 칸(위 판 위 끝 ~ 판 아래 끝).</summary>
        public static Rect DirCard(Vector2 foot, Dir d)
        {
            float pw = 0f;
            foreach (var p in d.Panels) pw += PanelW(p);
            pw += CSCommonDesign.SignPanelGap * Mathf.Max(0, d.Panels.Length - 1);
            float w = Mathf.Max(pw, PlateW(d));
            float h = CSCommonDesign.SignPlateH + CSCommonDesign.SignPanelGap + CSCommonDesign.SignPanelH;
            return new Rect(Mathf.Round(foot.x - w * 0.5f), Mathf.Round(foot.y - h), w, h);
        }

        static void Board(Rect r)
        {
            Draw.Round(r, Pal.RoadSignGreen, CSCommonDesign.SignRadius);
            float i = CSCommonDesign.SignRimInset;
            Draw.RoundOutline(new Rect(r.x + i, r.y + i, r.width - i * 2f, r.height - i * 2f), Pal.RoadSignInk, CSCommonDesign.SignRadius - i, CSCommonDesign.SignRim);
        }

        /// <summary>흰 상자(노선 번호 · 초록 글) 또는 테 상자(出口 같은 글).</summary>
        static float Box(float x, float cy, string s, bool filled)
        {
            float w = BoxW(s), h = CSCommonDesign.SignBoxH;
            var r = new Rect(x, Mathf.Round(cy - h * 0.5f), w, h);
            if (filled) Draw.Round(r, Pal.RoadSignInk, 2f);
            else Draw.RoundOutline(r, Pal.RoadSignInk, 2f, 1f);
            Draw.TextFit(r, s, CSCommonDesign.SignBoxFont, filled ? Pal.RoadSignGreen : Pal.RoadSignInk, true, TextAnchor.MiddleCenter);
            return w;
        }

        /// <summary>화살표(굵은 선 · 머리 두 줄): deg 0 = 위, -45 = 왼쪽 위.</summary>
        static void Arrow(Rect r, float deg)
        {
            float a = deg * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Sin(a), -Mathf.Cos(a));
            float len = r.height * 0.85f, head = r.width * 0.55f, s = CSCommonDesign.SignArrowStroke;
            var c = r.center;
            var tip = c + dir * len * 0.5f;
            Draw.Line(c - dir * len * 0.5f, tip, s, Pal.RoadSignInk);
            for (int k = -1; k <= 1; k += 2)
            {
                float b = a + Mathf.PI + k * 0.7f;
                Draw.Line(tip, tip + new Vector2(Mathf.Sin(b), -Mathf.Cos(b)) * head, s, Pal.RoadSignInk);
            }
        }

        /// <summary>방향 안내 표지(card = DirCard 값).</summary>
        public static bool DirIn(Rect card, Vector2 foot, Dir d)
        {
            if (Draw.Repaint)
            {
                float gap = CSCommonDesign.SignPanelGap, pad = CSCommonDesign.SignPad;
                // 위 작은 판
                float plw = PlateW(d);
                var plate = new Rect(Mathf.Round(card.center.x - plw * 0.5f), card.y, plw, CSCommonDesign.SignPlateH);
                Board(plate);
                float x = plate.x + pad, cy = plate.center.y;
                if (!string.IsNullOrEmpty(d.No)) x += Box(x, cy, d.No, true) + gap;
                Draw.TextFit(new Rect(x, plate.y, W(d.Name, P), plate.height), d.Name, P, Pal.RoadSignInk, true);
                x += W(d.Name, P) + gap;
                Draw.TextFit(new Rect(x, plate.y, W(d.NameEn, P * 0.5f), plate.height), d.NameEn, P * 0.5f, Pal.RoadSignInk, true);
                // 판들
                float pwSum = 0f;
                foreach (var p in d.Panels) pwSum += PanelW(p);
                pwSum += gap * Mathf.Max(0, d.Panels.Length - 1);
                float px = Mathf.Round(card.center.x - pwSum * 0.5f), py = plate.yMax + gap;
                foreach (var p in d.Panels)
                {
                    float w = PanelW(p);
                    var r = new Rect(px, py, w, CSCommonDesign.SignPanelH);
                    Board(r);
                    var inner = new Rect(r.x + pad, r.y + pad, r.width - pad * 2f, r.height - pad * 2f);
                    // 머리: 노선 상자 + 이름 또는 글 상자 (갈래는 왼쪽, 직진은 오른쪽 맞춤)
                    float hw = HeadW(p), hx = p.Exit ? inner.x : inner.xMax - hw, hcy = inner.y + CSCommonDesign.SignBoxH * 0.5f;
                    if (string.IsNullOrEmpty(p.Code)) { if (!string.IsNullOrEmpty(p.Label)) Box(hx, hcy, p.Label, false); }
                    else
                    {
                        float bw = Box(hx, hcy, p.Code, true) + gap * 0.5f;
                        Draw.TextFit(new Rect(hx + bw, inner.y, W(p.Route, CSCommonDesign.SignRouteFont), CSCommonDesign.SignBoxH), p.Route, CSCommonDesign.SignRouteFont, Pal.RoadSignInk, true);
                    }
                    // 가운데: 화살표 + 행선지
                    float dw = DestsW(p), aw = CSCommonDesign.SignArrowW, ah = CSCommonDesign.SignArrowH;
                    float midY = inner.center.y;
                    float ax = p.Exit ? inner.x : inner.xMax - aw;
                    Arrow(new Rect(ax, Mathf.Round(midY - ah * 0.5f), aw, ah), p.Exit ? -45f : 0f);
                    float dx = p.Exit ? ax + aw + CSCommonDesign.SignGap : ax - CSCommonDesign.SignGap - dw;
                    float lineH = D + D * 0.5f + 2f;
                    for (int i = 0; i < p.Jp.Length; i++)
                    {
                        string en = i < p.En.Length ? p.En[i] : "";
                        float cw = Mathf.Max(W(p.Jp[i], D), W(en, D * 0.5f));
                        float top = Mathf.Round(midY - lineH * 0.5f);
                        Draw.TextFit(new Rect(dx, top, cw, D + 2f), p.Jp[i], D, Pal.RoadSignInk, true);
                        Draw.TextFit(new Rect(dx, top + D + 2f, cw, D * 0.5f + 2f), en, D * 0.5f, Pal.RoadSignInk, true);
                        dx += cw + CSCommonDesign.SignGap;
                    }
                    // 아래: 다른 노선 상자
                    if (p.More != null)
                    {
                        float mx = p.Exit ? inner.x : inner.xMax - MoreW(p) + gap, mcy = inner.yMax - CSCommonDesign.SignBoxH * 0.5f;
                        foreach (var m in p.More) mx += Box(mx, mcy, m, true) + gap;
                    }
                    px += w + gap;
                }
            }
            return Draw.Button(card);
        }

        /// <summary>요금소 표지 칸(위 이름 판 + 요금소 판).</summary>
        public static Rect TollCard(Vector2 foot, string name)
        {
            float w = Mathf.Max(CSCommonDesign.SignTollW, CSCommonDesign.SignPad * 2f + W(name, P));
            float h = CSCommonDesign.SignPlateH + CSCommonDesign.SignPanelGap + CSCommonDesign.SignTollH;
            return new Rect(Mathf.Round(foot.x - w * 0.5f), Mathf.Round(foot.y - h), w, h);
        }

        public static bool TollIn(Rect card, Vector2 foot, string name)
        {
            if (Draw.Repaint)
            {
                float gap = CSCommonDesign.SignPanelGap, pad = CSCommonDesign.SignPad;
                float plw = CSCommonDesign.SignPad * 2f + W(name, P);
                var plate = new Rect(Mathf.Round(card.center.x - plw * 0.5f), card.y, plw, CSCommonDesign.SignPlateH);
                Board(plate);
                Draw.TextFit(plate, name, P, Pal.RoadSignInk, true, TextAnchor.MiddleCenter);
                var r = new Rect(Mathf.Round(card.center.x - CSCommonDesign.SignTollW * 0.5f), plate.yMax + gap, CSCommonDesign.SignTollW, CSCommonDesign.SignTollH);
                Board(r);
                const string jp = "料金所", en = "TOLL GATE";
                float tw = W(jp, P + 2f), ew = W(en, (P + 2f) * 0.5f), tx = Mathf.Round(r.center.x - (tw + gap + ew) * 0.5f);
                Draw.TextFit(new Rect(tx, r.y + pad * 0.5f, tw, P + 6f), jp, P + 2f, Pal.RoadSignInk, true);
                Draw.TextFit(new Rect(tx + tw + gap, r.y + pad * 0.5f, ew, P + 6f), en, (P + 2f) * 0.5f, Pal.RoadSignInk, true);
                float ly = r.y + pad * 0.5f + P + 8f, lh = r.yMax - pad - ly, lw = (r.width - pad * 2f - gap * 2f) / 3f;
                string[] lanes = { "ETC専用", "ETC/一般", "一般" };
                Color[] bg = { Pal.RoadSignEtc, Pal.RoadSignInk, Pal.RoadSignGeneral };
                for (int i = 0; i < 3; i++)
                {
                    var lr = new Rect(r.x + pad + i * (lw + gap), ly, lw, lh);
                    Draw.Round(lr, bg[i], 3f);
                    Draw.TextFit(lr, lanes[i], CSCommonDesign.SignRouteFont, i == 1 ? Pal.RoadSignGreen : Pal.RoadSignInk, true, TextAnchor.MiddleCenter);
                }
            }
            return Draw.Button(card);
        }

        /// <summary>노선 번호 표식(가운데 center): 흰 상자 번호 + 노선 이름.</summary>
        public static Rect MarkerRect(Vector2 center, string code, string name)
        {
            float w = CSCommonDesign.SignPad + BoxW(code) + CSCommonDesign.SignPanelGap + W(name, CSCommonDesign.SignRouteFont) + CSCommonDesign.SignPad;
            return new Rect(Mathf.Round(center.x - w * 0.5f), Mathf.Round(center.y - CSCommonDesign.SignMarkH * 0.5f), w, CSCommonDesign.SignMarkH);
        }

        public static bool MarkerIn(Rect r, string code, string name)
        {
            if (Draw.Repaint)
            {
                Board(r);
                float x = r.x + CSCommonDesign.SignPad;
                x += Box(x, r.center.y, code, true) + CSCommonDesign.SignPanelGap;
                Draw.TextFit(new Rect(x, r.y, W(name, CSCommonDesign.SignRouteFont), r.height), name, CSCommonDesign.SignRouteFont, Pal.RoadSignInk, true);
            }
            return Draw.Button(r);
        }
    }
}
