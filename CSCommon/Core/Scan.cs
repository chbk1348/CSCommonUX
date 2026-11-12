using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.Threading;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// 건물 전수 스캔 1개를 여러 구독자가 나눠 씀(프레임 분할, 메인 스레드 전용).
    /// - Subscribe(이름, visit, commit, perFrame, everySec): everySec마다 한 바퀴. 같은 바퀴에 탄 구독자는 칸을 한 번만 읽고 visit을 차례로 부름.
    ///   한 바퀴가 끝나면 commit — 구독자는 visit에서 모은 것으로 새 배열을 만들어 통째로 갈아 끼움(시뮬 스레드가 읽어도 안전하게).
    /// - visit은 만들어진(Created) 건물만, id 1부터. 한 프레임에 읽는 칸 수는 이번 바퀴 구독자들의 perFrame 중 가장 큰 값.
    /// - Handle.Invalidate(): 다음 Tick에 새 바퀴를 시작(진행 중인 바퀴가 있으면 그 뒤). Touch(id): 다음 Tick에 그 칸만 visit,
    ///   바퀴 밖이면 이어서 commit — visit이 칸마다 덮어쓰는 구독자만 쓸 것(합계를 더하는 구독자는 두 번 셈).
    /// - 누가 돌리나: 모드 컨트롤러(MonoBehaviour)의 Update에서 Scan.Tick()을 매 프레임 부름. 모드 DLL마다 따로 한 벌이라
    ///   그 모드의 구독자만 돎. 한 프레임에 여러 번 불려도 한 번만 일함.
    /// - 시뮬 스레드에서 Subscribe · Tick · Invalidate · Touch · Unsubscribe를 부르면 아무것도 안 하고 로그 한 줄(처음 한 번).
    /// </summary>
    public static class Scan
    {
        public delegate void Visit(int id, ref Building b);

        public sealed class Handle
        {
            internal readonly string Name;
            internal readonly Visit OnVisit;
            internal readonly Action OnCommit;
            internal readonly int PerFrame;
            internal readonly float Every;
            internal float Due;
            internal bool InPass, Removed, Again;
            internal readonly List<int> Touched = new();

            internal Handle(string name, Visit visit, Action commit, int perFrame, float every)
            {
                Name = name;
                OnVisit = visit;
                OnCommit = commit;
                PerFrame = Math.Max(1, perFrame);
                Every = every;
            }

            /// <summary>다음 Tick에 새 바퀴(진행 중인 바퀴가 끝난 뒤).</summary>
            public void Invalidate()
            {
                if (!Main("Invalidate")) return;
                if (InPass) Again = true; // 진행 중인 바퀴는 이미 지난 칸이 있어 끝나면 바로 한 바퀴 더
                else Due = 0f;
            }

            /// <summary>그 칸만 다음 Tick에 다시 visit.</summary>
            public void Touch(int id)
            {
                if (Main("Touch")) Touched.Add(id);
            }

            public void Unsubscribe()
            {
                if (Main("Unsubscribe")) Removed = true;
            }
        }

        /// <summary>바퀴 상태(실제 · 자가 검사가 따로 가짐).</summary>
        sealed class Runner
        {
            readonly List<Handle> subs = new(), pass = new();
            int cursor;
            int budget;

            public void Add(Handle h) => subs.Add(h);

            public void Step(Building[] buf, int size, float now)
            {
                subs.RemoveAll(h => h.Removed);
                for (int s = 0; s < subs.Count; s++)
                {
                    var h = subs[s];
                    if (h.Touched.Count == 0) continue;
                    for (int k = 0; k < h.Touched.Count; k++)
                    {
                        int id = h.Touched[k];
                        if (id > 0 && id < size && (buf[id].m_flags & Building.Flags.Created) != 0) Call(h, id, ref buf[id]);
                    }
                    h.Touched.Clear();
                    if (!h.InPass) Commit(h);
                }
                if (pass.Count == 0)
                {
                    budget = 0;
                    for (int s = 0; s < subs.Count; s++)
                    {
                        var h = subs[s];
                        if (now < h.Due) continue;
                        h.InPass = true;
                        pass.Add(h);
                        budget = Math.Max(budget, h.PerFrame);
                    }
                    if (pass.Count == 0) return;
                    cursor = 1;
                }
                int end = Math.Min(size, cursor + budget);
                for (int i = cursor; i < end; i++)
                {
                    if ((buf[i].m_flags & Building.Flags.Created) == 0) continue;
                    for (int s = 0; s < pass.Count; s++)
                        if (!pass[s].Removed) Call(pass[s], i, ref buf[i]);
                }
                cursor = end;
                if (end < size) return;
                for (int s = 0; s < pass.Count; s++)
                {
                    var h = pass[s];
                    h.InPass = false;
                    h.Due = h.Again ? 0f : now + h.Every;
                    h.Again = false;
                    if (!h.Removed) Commit(h);
                }
                pass.Clear();
            }

            static void Call(Handle h, int id, ref Building b)
            {
                try { h.OnVisit(id, ref b); }
                catch (Exception e) { CSCommonLog.Exception(e, "스캔 " + h.Name + " visit " + id); }
            }

            static void Commit(Handle h)
            {
                try { h.OnCommit?.Invoke(); }
                catch (Exception e) { CSCommonLog.Exception(e, "스캔 " + h.Name + " commit"); }
            }
        }

        static readonly Runner runner = new();
        static int lastFrame = -1;
        static bool warned, checkedSelf;

        public static Handle Subscribe(string name, Visit visit, Action commit, int perFrame = 2000, float everySec = 10f)
        {
            if (!Main("Subscribe " + name) || visit == null) return null;
            var h = new Handle(name, visit, commit, perFrame, everySec);
            runner.Add(h);
            return h;
        }

        /// <summary>모드 컨트롤러 Update에서 매 프레임.</summary>
        public static void Tick()
        {
            if (!Main("Tick") || Time.frameCount == lastFrame) return;
            lastFrame = Time.frameCount;
            if (!checkedSelf)
            {
                checkedSelf = true;
                if (!SelfCheck()) CSCommonLog.Warn("스캔 자가 검사 실패: 칸을 한 번씩 읽지 않음");
            }
            if (!Singleton<BuildingManager>.exists) return;
            var bm = Singleton<BuildingManager>.instance;
            runner.Step(bm.m_buildings.m_buffer, (int)bm.m_buildings.m_size, Time.realtimeSinceStartup);
        }

        static bool Main(string what)
        {
            if (Dispatcher.currentSafe == ThreadHelper.dispatcher) return true;
            if (!warned)
            {
                warned = true;
                CSCommonLog.Warn("스캔 " + what + ": 메인 스레드가 아니라 거부함");
            }
            return false;
        }

        /// <summary>가짜 건물 배열로: 두 구독자가 만든 칸만 한 번씩 · 바퀴 끝에 commit 한 번 · Touch는 그 칸만 다시 · Unsubscribe 뒤엔 안 불림.</summary>
        internal static bool SelfCheck()
        {
            const int size = 50;
            var buf = new Building[size];
            for (int i = 1; i < size; i++)
                if (i % 3 != 0) buf[i].m_flags = Building.Flags.Created;
            var r = new Runner();
            int[] seenA = new int[size], seenB = new int[size];
            int commitsA = 0, commitsB = 0;
            var a = new Handle("A", (int id, ref Building b) => seenA[id]++, () => commitsA++, 7, 100f);
            var b2 = new Handle("B", (int id, ref Building b) => seenB[id]++, () => commitsB++, 3, 100f);
            r.Add(a);
            r.Add(b2);
            int steps = 0;
            while (commitsA == 0 && steps++ < 100) r.Step(buf, size, 0f);
            bool ok = commitsA == 1 && commitsB == 1 && steps == 7; // 49칸 / 프레임당 7(둘 중 큰 값)
            for (int i = 0; i < size; i++)
            {
                int want = i > 0 && i % 3 != 0 ? 1 : 0;
                ok &= seenA[i] == want && seenB[i] == want;
            }
            r.Step(buf, size, 1f); // everySec 전: 새 바퀴 없음
            a.Touched.Add(4);
            a.Touched.Add(3); // 안 만든 칸
            r.Step(buf, size, 1f);
            ok &= seenA[4] == 2 && seenA[3] == 0 && commitsA == 2 && seenB[4] == 1;
            b2.Removed = true;
            a.Due = 0f;
            for (int k = 0; k < 10; k++) r.Step(buf, size, 2f);
            ok &= commitsA == 3 && commitsB == 1 && seenB[1] == 1;
            return ok;
        }
    }
}
