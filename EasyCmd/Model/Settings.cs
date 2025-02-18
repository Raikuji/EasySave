using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EasyLog;

namespace EasyCmd.Model
{
	public class Settings
	{
		private static Settings? _instance;
		private string _language;
		private LogFormat _logFormat;
		public static string SETTINGSDIR = $"{AppDomain.CurrentDomain.BaseDirectory}\\resources";
		public static string SETTINGSFILEPATH = $"{SETTINGSDIR}\\settings.json";

		[JsonConstructor]
		private Settings()
		{
			_language = "en";
			_logFormat = LogFormat.JSON;
		}
		public static Settings GetInstance()
		{
			if (_instance == null)
			{
				_instance = new Settings();
			}
			return _instance;
		}
		public string LanguageCode
		{
			get => _language;
			set => _language = value;
		}
		public LogFormat LogFormat
		{
			get => _logFormat;
			set => _logFormat = value;
		}
		public void LoadSettings()
		{
			if (File.Exists(SETTINGSFILEPATH))
			{
				_instance = JsonSerializer.Deserialize<Settings>(File.ReadAllText(SETTINGSFILEPATH));
			}
		}
		public void SaveSettings()
		{
			if (!Directory.Exists(SETTINGSDIR))
			{
				Directory.CreateDirectory(SETTINGSDIR);
			}
			File.WriteAllText(SETTINGSFILEPATH, JsonSerializer.Serialize(this));
		}

		public void SetLanguage()
		{
			LanguageDictionary languageDictionnary = new LanguageDictionary();
			switch (LanguageCode)
			{
				case "en":
					languageDictionnary.LoadLanguage(SETTINGSDIR + "\\" + "en.json");
					break;
				case "fr":
					languageDictionnary.LoadLanguage(SETTINGSDIR + "\\" + "fr.json");
					break;
				default:
					languageDictionnary.LoadLanguage(SETTINGSDIR + "\\" + "en.json");
					break;
			}
			Language.GetInstance().SetLanguage(languageDictionnary);
		}
	}
}
