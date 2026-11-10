using System;
using System.Text;
using UnityEngine;

namespace CSCommonUX
{
    /// <summary>
    /// output_log.txt에 남김. 머리는 모드가 정한 이름(예 "[F1]: ").
    /// Detail은 '자세한 로그'가 켜졌을 때만 남김(설정 XML의 DetailedLogging).
    /// </summary>
    public static class CSCommonLog
    {
        static readonly StringBuilder line = new(160);
        static readonly object gate = new();

        /// <summary>줄 머리. UxMod가 OnEnabled에서 LogName으로 정함.</summary>
        public static string Name = "[F1UX]";

        public static bool Detailed;

        public static void Info(string message) => Write("", message);

        public static void Warn(string message) => Write("경고: ", message);

        public static void Error(string message) => Write("오류: ", message);

        public static void Detail(string message)
        {
            if (Detailed) Write("", message);
        }

        public static void Exception(Exception e, string what)
        {
            lock (gate)
            {
                line.Length = 0;
                line.Append(Name).Append(": 예외 · ").Append(what).AppendLine();
                for (var x = e; x != null; x = x.InnerException)
                {
                    line.Append(x.GetType().Name).Append(": ").AppendLine(x.Message);
                    line.AppendLine(x.StackTrace);
                }
                Debug.Log(line.ToString());
            }
        }

        static void Write(string head, string message)
        {
            lock (gate)
            {
                line.Length = 0;
                line.Append(Name).Append(": ").Append(head).Append(message);
                Debug.Log(line.ToString());
            }
        }
    }
}
