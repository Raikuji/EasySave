using EasyLog;

namespace EasyCmd.Model
{
    internal class BackupJobLog
    {
        private Dictionary<string, object> _logDictionary;
        private Logger _logger;
        private LogFormat _logFormat;
        public static string LOGPATH = $"{AppDomain.CurrentDomain.BaseDirectory}\\log";
        private string _logfile = $"backup_{DateTime.Now.ToString("yyyyMMdd")}";

        public BackupJobLog(string name, string source, string destination, long size, double transfertTime, DateTime time, string logFormat)
        {
            // Log format verification and assignment
            if (logFormat == "JSON")
            {
                _logfile += ".json";
                _logFormat = LogFormat.JSON;
            }
            else if (logFormat == "XML")
            {
                _logfile += ".xml";
                _logFormat = LogFormat.XML;
            }
            else
            {
                throw new ArgumentException("Format de log invalide. Utilisez 'JSON' ou 'XML'.");
            }

            // Ensure that the log file exists
            if (!Directory.Exists(LOGPATH))
            {
                Directory.CreateDirectory(LOGPATH);
            }

            // Logger initialization
            _logger = new Logger($"{LOGPATH}\\{_logfile}", _logFormat);

            // Dictionary creation with backup information
            _logDictionary = new Dictionary<string, object>();
            _logDictionary.Add("Name", name);
            _logDictionary.Add("Source", source);
            _logDictionary.Add("Destination", destination);
            _logDictionary.Add("Size", size);
            _logDictionary.Add("TransfertTime", transfertTime);
            _logDictionary.Add("Time", time);
        }

        public void Log()
        {
            // Data logging in the file with the correct format
            _logger.Log(_logDictionary);
        }
    }
}
