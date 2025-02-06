using System.Text.Json;

namespace EasyLog
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
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