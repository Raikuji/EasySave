using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyCmd.Model;
using EasyLog;

namespace EasyGui.ViewModels
{
	class SettingsViewModel : ObservableObject
	{
		private LogFormat _logFormat;
		private string _language;
		public ICommand ChangeLogFormat { get; }
		public ICommand ChangeLanguage { get; }

		public SettingsViewModel()
		{
			Settings.GetInstance().LoadSettings();
			_logFormat = Settings.GetInstance().LogFormat;
			_language = Settings.GetInstance().LanguageCode;
			ChangeLogFormat = new RelayCommand<LogFormat>(UpdateLogFormat);
			ChangeLanguage = new RelayCommand<string?>(UpdateLanguage);
		}

		public LogFormat LogFormat
		{
			get => _logFormat;
			set => SetProperty(ref _logFormat, value);
		}

		public string Language
		{
			get => _language;
			set => SetProperty(ref _language, value);
		}

		public void UpdateLogFormat(LogFormat format)
		{
			_logFormat = format;
			Settings.GetInstance().LogFormat = format;
			Settings.GetInstance().SaveSettings();
		}

		public void UpdateLanguage(string? language)
		{
			if (language is null) return;
			_language = language;
			Settings.GetInstance().LanguageCode = language;
			Settings.GetInstance().SetLanguage();
			Settings.GetInstance().SaveSettings();
			MainWindowViewModel.Instance.UpdateLanguage();
		}

		public bool LogFormatJson
		{
			get => _logFormat == LogFormat.JSON;
			set
			{
				if (value)
				{
					UpdateLogFormat(LogFormat.JSON);
				}
			}
		}

		public bool LogFormatXml
		{
			get => _logFormat == LogFormat.XML;
			set
			{
				if (value)
				{
					UpdateLogFormat(LogFormat.XML);
				}
			}
		}

		public bool LanguageEnglish
		{
			get => _language == "en";
			set
			{
				if (value)
				{
					UpdateLanguage("en");
				}
			}
		}

		public bool LanguageFrench
		{
			get => _language == "fr";
			set
			{
				if (value)
				{
					UpdateLanguage("fr");
				}
			}
		}
	}
}
