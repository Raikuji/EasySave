using System.Text.Json;

namespace EasyLog
{
    public class Logger
    {
        private readonly string _logFilePath;
        private readonly LogFormat _format;

        public Logger(string logFilePath, LogFormat format)
        {
            _logFilePath = logFilePath;
            _format = format;
        }

        public void Log(Dictionary<string, object> data)
        {
            if (File.Exists(_logFilePath))
            {
                File.AppendAllText(_logFilePath, $",{Environment.NewLine}");
            }
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.AppendAllText(_logFilePath, json);
        }
    }
}