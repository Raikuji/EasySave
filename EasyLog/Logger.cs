using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace EasyLog
{
    public class Logger
    {
        private readonly string _logDirectory;  // Path to the directory where logs will be stored

        // Constructor that receives the log directory path
        public Logger(string logDirectory)
        {
            // Check and create the directory if it does not exist
            _logDirectory = logDirectory;
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        /// Method to write a log entry.
        public void LogAction(
            string backupName,
            string sourceFilePath,
            string destinationFilePath,
            string backupType,
            long fileSize,
            long transferTimeMs)
        {
            // 1) Create a LogEntry object
            var entry = new LogEntry
            {
                ["Name"] = "Save1",
                ["FileSize"] = fileSize,
                ["Timestamp"] = DateTime.Now
                ["transferTimeMs"] = transferTimeMs,
                ["BackupType"] = backupType;
                ["sourceFilePath"] = sourceFilePath,
                ["destinationFilePath"] = destinationFilePath
            };

            // 2) Generate the daily log file name (example: 2023-02-03.json)
            string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            string logFilePath = Path.Combine(_logDirectory, logFileName);

            // 3) Serialize the object to JSON
            string json = JsonSerializer.Serialize(entry);

            // 4) Write the JSON to the file, followed by a newline
            //    Open in append mode to avoid overwriting existing content
            using (var writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine(json);
            }
        }
    }
}
