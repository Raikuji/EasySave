using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
		private List<string> _filesToEncrypt;
		private List<string> _lockProcesses;
        private List<string> _priorityExtensions;
        private Byte[] _key;
		private int _bigFileSize;
		public static string SETTINGSDIR = $"{AppDomain.CurrentDomain.BaseDirectory}\\resources";
		public static string SETTINGSFILEPATH = $"{SETTINGSDIR}\\settings.json";

		[JsonConstructor]
		private Settings()
		{
			_language = "en";
			_logFormat = LogFormat.JSON;
			_filesToEncrypt = new List<string>();
			_lockProcesses = new List<string>();
			_priorityExtensions = new List<string>();
            Aes aes = Aes.Create();
			aes.KeySize = 256;
			_key = aes.Key;
			_bigFileSize = 1024;
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
		public List<string> FilesToEncrypt
		{
			get => _filesToEncrypt;
			set => _filesToEncrypt = value;
		}
		public List<string> LockProcesses
		{
			get => _lockProcesses;
			set => _lockProcesses = value;
		}
		public List<string> PriorityExtensions
        {
            get => _priorityExtensions;
            set => _priorityExtensions = value;
        }
        public Byte[] Key
		{
			get => _key;
			set => _key = value;
		}
		public int BigFileSize
		{
			get => _bigFileSize;
			set => _bigFileSize = value;
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
