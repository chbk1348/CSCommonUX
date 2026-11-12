using System;
using System.Diagnostics;

namespace CSCommonUX
{
    /// <summary>
    /// 무거운 곳 재기(할당 없음, 한 인스턴스는 한 스레드만): N초마다 평균 · 최대 ms 한 줄(최대 2ms 밑이면 조용),
    /// 한 번에 50ms 넘으면 바로 한 줄. Begin · End 사이, 또는 Now로 잰 시작을 End(start)에.
    /// 줄은 Log로 남김(모드가 자기 로그 함수로 바꿀 수 있음, 기본 CSCommonLog.Info).
    /// </summary>
    public sealed class PerfWatch
    {
        /// <summary>줄을 남길 곳. 모드가 OnEnabled 등에서 바꿔 끼움(모드 DLL마다 따로).</summary>
        public static Action<string> Log = CSCommonLog.Info;

        static readonly double ToMs = 1000.0 / Stopwatch.Frequency;
        static readonly long Slow = Stopwatch.Frequency / 20, Quiet = Stopwatch.Frequency / 500;
        readonly string name;
        readonly long every;
        long start, sum, max, since;
        int count;

        public PerfWatch(string name, float seconds = 60f)
        {
            this.name = name;
            every = (long)(seconds * Stopwatch.Frequency);
        }

        public static long Now => Stopwatch.GetTimestamp();

        public void Begin() => start = Stopwatch.GetTimestamp();

        public void End() => End(start);

        public void End(long begin)
        {
            long now = Stopwatch.GetTimestamp(), t = now - begin;
            sum += t;
            count++;
            if (t > max) max = t;
            if (t > Slow) Log($"성능: {name} 한 번 {t * ToMs:0.0}ms");
            if (since == 0) since = now;
            if (now - since < every) return;
            if (max >= Quiet) Log($"성능: {name} 평균 {sum * ToMs / count:0.00}ms · 최대 {max * ToMs:0.0}ms · {count}회");
            sum = max = 0;
            count = 0;
            since = now;
        }
    }
}
