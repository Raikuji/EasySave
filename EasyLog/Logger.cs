using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.Xaml;

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
            _logFilePath = logFilePath;
            _format = format;
        }

        public void Log(Dictionary<string, object> data)
        {
            string output;
            if (_format == LogFormat.JSON)
            {
                output = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }
            else // XAML
            {
                output = SerializeToXml(data);
            }

            File.AppendAllText(_logFilePath, output + Environment.NewLine);
        }

        private string SerializeToXml(Dictionary<string, object> data)
        {
            var root = new XElement("Log");
            foreach (var kvp in data)
            {
                root.Add(new XElement(kvp.Key, kvp.Value));
            }
            return XamlServices.Save(root);
        }
    }