using System;
using System.IO;
using System.Xml.Serialization;

namespace CSCommonUX
{
    /// <summary>
    /// 모드 설정 XML 바탕: 마지막으로 본 업데이트 내역 판 · 자세한 로그를 같이 저장(언어 칸 없음 — 한국어만).
    /// 이어받는 클래스는 [XmlRoot]와 자기 칸만 두고 Load · Save로 읽고 씀.
    /// </summary>
    public abstract class CSCommonSettingsXml
    {
        [XmlElement("WhatsNewVersion")]
        public string WhatsNewVersion { get => CSCommonChangelog.LastSeenText; set => CSCommonChangelog.LastSeenText = value; }

        [XmlElement("DetailedLogging")]
        public bool DetailedLogging { get => CSCommonLog.Detailed; set => CSCommonLog.Detailed = value; }

        /// <summary>파일에서 읽음. 없거나 깨지면 기본값(깨진 파일은 .bad로 남김).</summary>
        public static T Load<T>(string path) where T : CSCommonSettingsXml, new()
        {
            if (!File.Exists(path)) return new T();
            try
            {
                using var reader = new StreamReader(path);
                return new XmlSerializer(typeof(T)).Deserialize(reader) as T ?? new T();
            }
            catch (Exception e)
            {
                CSCommonLog.Warn($"설정 파일을 읽지 못해 기본값으로: {e.Message}");
                try
                {
                    File.Copy(path, path + ".bad", true);
                }
                catch (Exception)
                {
                    // 남기지 못해도 기본값으로 계속
                }
                return new T();
            }
        }

        public static void Save<T>(string path, T settings) where T : CSCommonSettingsXml
        {
            try
            {
                string temp = path + ".tmp";
                using (var writer = new StreamWriter(temp))
                    new XmlSerializer(typeof(T)).Serialize(writer, settings);
                if (File.Exists(path)) File.Delete(path);
                File.Move(temp, path);
            }
            catch (Exception e)
            {
                CSCommonLog.Error($"설정 파일 저장 실패: {e.Message}");
            }
        }
    }
}
