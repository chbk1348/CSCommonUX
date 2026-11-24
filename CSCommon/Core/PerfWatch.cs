using System;
using System.Diagnostics;

namespace CSCommonUX
{
    /// <summary>
    /// 무거운 곳 재기(할당 없음, 한 인스턴스는 한 스레드만): N초마다 평균 · 최대 ms 한 줄(최대 2ms 밑이면 조용),
    /// 한 번에 50ms 넘으면 바로 한 줄. Begin · End 사이, 또는 Now로 잰 시작을 End(start)에.
    /// 평균 줄은 On()이 참일 때만 Log로 남김(기본: CSCommonLog.Detailed '자세한 로그' 켰을 때만 · CSCommonLog.Info) — 꺼져 있으면 줄 글을 아예 만들지 않음.
    /// 자기 '자세한 로그' 스위치가 따로 있는 모드는 On을 그 스위치로 바꿔 끼움.
    /// 한 번 50ms 넘는 줄은 드물고 멈춤을 찾는 단서라 스위치와 상관없이 늘 SlowLog로 남김(GC 횟수 차이 · 힙 크기 붙임).
    /// </summary>
    public sealed class PerfWatch
    {
        /// <summary>평균 줄을 남길 곳. 모드가 OnEnabled 등에서 바꿔 끼움(모드 DLL마다 따로).</summary>
        public static Action<string> Log = CSCommonLog.Info;

        /// <summary>한 번 50ms 넘는 줄을 남길 곳(늘 남김). 모드가 바꿔 끼울 수 있음(모드 DLL마다 따로).</summary>
        public static Action<string> SlowLog = CSCommonLog.Info;

        /// <summary>평균 줄을 남길지(자세한 로그). 모드가 자기 스위치로 바꿔 끼움(모드 DLL마다 따로).</summary>
        public static Func<bool> On = () => CSCommonLog.Detailed;

        static readonly double ToMs = 1000.0 / Stopwatch.Frequency;
        static readonly long Slow = Stopwatch.Frequency / 20, Quiet = Stopwatch.Frequency / 500;

        // shortcut: Now로 잰 시작의 GC 횟수는 스레드마다 마지막 Now 것 — 같은 스레드에서 Now가 겹치면 바깥 재기의 GC 차이가 적게 나옴, 정확히 봐야 하면 End(begin, gc)를 받도록
        [ThreadStatic] static int nowGc;

        readonly string name;
        readonly long every;
        long start, sum, max, since;
        int count, startGc;

        public PerfWatch(string name, float seconds = 60f)
        {
            this.name = name;
            every = (long)(seconds * Stopwatch.Frequency);
        }

        public static long Now
        {
            get
            {
                nowGc = GC.CollectionCount(0);
                return Stopwatch.GetTimestamp();
            }
        }

        public void Begin()
        {
            startGc = GC.CollectionCount(0);
            start = Stopwatch.GetTimestamp();
        }

        public void End() => End(start, startGc);

        public void End(long begin) => End(begin, nowGc);

        void End(long begin, int gc)
        {
            long now = Stopwatch.GetTimestamp(), t = now - begin;
            sum += t;
            count++;
            if (t > max) max = t;
            if (t > Slow) SlowLog($"성능: {name} 한 번 {t * ToMs:0.0}ms · GC +{GC.CollectionCount(0) - gc} · 힙 {GC.GetTotalMemory(false) / 1048576:#,0}MB");
            if (since == 0) since = now;
            if (now - since < every) return;
            if (max >= Quiet && On()) Log($"성능: {name} 평균 {sum * ToMs / count:0.00}ms · 최대 {max * ToMs:0.0}ms · {count}회");
            sum = max = 0;
            count = 0;
            since = now;
        }
    }
}
