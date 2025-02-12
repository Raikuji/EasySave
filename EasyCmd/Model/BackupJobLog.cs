using EasyLog;

namespace EasyCmd.Model
{
    internal class BackupJobLog
    {
        private Dictionary<string, object> _logDictionary;
        private Logger _logger;
        private LogFormat _logFormat;
        public static string LOGPATH = $"{AppDomain.CurrentDomain.BaseDirectory}\\log";
        string _logfile = $"backup_{DateTime.Now.ToString("yyyyMMdd")}";


        public BackupJobLog(string name, string source, string destination, long size, double transfertTime, DateTime time, string logFormat)
        {
            if (logFormat == "JSON")
            {
                _logfile += ".json";
                _logFormat = LogFormat.JSON;
            }
            else
            {
                _logfile += ".xml";
                _logFormat = LogFormat.XML;
            }
            _logger = new Logger($"{LOGPATH}\\{LOGFILE}");
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
            
            _logger = new Logger($"{LOGPATH}\\{LOGFILE}", _logFormat);
            _logger.Log(_logDictionary);
        }

