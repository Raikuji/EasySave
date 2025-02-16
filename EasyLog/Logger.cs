using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml;

namespace EasyLog
{
    public enum LogFormat
    {
        JSON,
        XML
    }

    public class Logger
    {
        private readonly string _logFilePath;
        private readonly LogFormat _format;

        public Logger(string logFilePath, LogFormat format)
        {
            switch (format)
			{
				case LogFormat.JSON:
					_logFilePath = $"{logFilePath}.json";
					break;
				case LogFormat.XML:
					_logFilePath = $"{logFilePath}.xml";
					break;
				default:
					throw new ArgumentException("Invalid log format");
			}
			_format = format;
        }

        public void Log(Dictionary<string, object> data)
        {
            string output;
            if (_format == LogFormat.JSON)
            {
                output = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.AppendAllText(_logFilePath, output + Environment.NewLine);
            }
            else // XML
            {
                output = SerializeToXml(data);
                File.WriteAllText(_logFilePath, output);
            }
        }

        private string SerializeToXml(Dictionary<string, object> data)
        {
			XElement root;
			if (File.Exists(_logFilePath))
			{
				root = XElement.Load(_logFilePath);
			}
			else
			{
				root = new XElement("Logs");
			}

			var logEntry = new XElement("Log");
			foreach (var kvp in data)
			{
				logEntry.Add(new XElement(kvp.Key, kvp.Value));
			}

			root.Add(logEntry);
			return root.ToString();
		}
	}
}
