using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
		private static Mutex _mutex = new Mutex();

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
			_mutex.WaitOne();
			if (_format == LogFormat.JSON)
			{
				File.WriteAllText(_logFilePath, SerializeToJson(data));
			}
			else
			{
				File.WriteAllTextAsync(_logFilePath, SerializeToXml(data));
			}
			_mutex.ReleaseMutex();
		}

		private string SerializeToJson(Dictionary<string, object> data)
		{
			JsonArray jsonArray;
			if (File.Exists(_logFilePath))
			{
				string existingJson = File.ReadAllText(_logFilePath);
				jsonArray = JsonNode.Parse(existingJson)?.AsArray() ?? new JsonArray();
			}
			else
			{
				jsonArray = new JsonArray();
			}

			JsonObject jsonObject = new JsonObject();
			foreach (var kvp in data)
			{
				jsonObject[kvp.Key] = JsonValue.Create(kvp.Value);
			}

			jsonArray.Add(jsonObject);
			string json = JsonSerializer.Serialize(jsonArray, new JsonSerializerOptions { WriteIndented = true });
			return json;
		}

		private string SerializeToXml(Dictionary<string, object> data)
		{
			XElement root;
			if (File.Exists(_logFilePath))
			{
				string existingXml = File.ReadAllText(_logFilePath);
				root = XElement.Parse(existingXml);
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
