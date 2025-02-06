using EasyLog;

namespace EasyCmd.Model
{
    internal class BackupJobLog
    {
        private Dictionary<string, object> _logDictionary;
        private Logger _logger;
        public static string LOGPATH = $"{AppDomain.CurrentDomain.BaseDirectory}\\log";
        static string LOGFILE = $"backup_{DateTime.Now.ToString("yyyyMMdd")}.log";


        public BackupJobLog(string name, string source, string destination, long size, double transfertTime, DateTime time)
        {
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
            _logger.Log(_logDictionary);
        }
    }
}
