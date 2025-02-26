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
        private List<string> _fileExtensions;
        private List<string> _lockProcesses;
        private long _maxFileSizeInKB;
        private string _notificationMessage = string.Empty; // Initialisation du champ

        public ICommand ChangeLogFormat { get; }
        public ICommand ChangeLanguage { get; }
        public ICommand AddFileExtension { get; }
        public ICommand RemoveFileExtension { get; }
        public ICommand AddLockProcess { get; }
        public ICommand RemoveLockProcess { get; }
        public ICommand SaveMaxFileSizeCommand { get; }

        public SettingsViewModel()
        {
            Settings.GetInstance().LoadSettings();
            _logFormat = Settings.GetInstance().LogFormat;
            _language = Settings.GetInstance().LanguageCode;
            _fileExtensions = Settings.GetInstance().FilesToEncrypt;
            _lockProcesses = Settings.GetInstance().LockProcesses;
            _maxFileSizeInKB = Settings.GetInstance().MaxFileSizeInKB;

            ChangeLogFormat = new RelayCommand<LogFormat>(UpdateLogFormat);
            ChangeLanguage = new RelayCommand<string?>(UpdateLanguage);
            AddFileExtension = new RelayCommand(AddExtension);
            RemoveFileExtension = new RelayCommand<string>(RemoveExtension);
            AddLockProcess = new RelayCommand(AddProcess);
            RemoveLockProcess = new RelayCommand<string>(RemoveProcess);
            SaveMaxFileSizeCommand = new RelayCommand(SaveMaxFileSize);
        }

        private void RemoveExtension(string? obj)
        {
            if (obj != null)
            {
                Settings.GetInstance().FilesToEncrypt.Remove(obj);
                Settings.GetInstance().SaveSettings();
                MainWindowViewModel.Instance.ChangeView("Settings");
            }
        }

        private void AddExtension()
        {
            Settings.GetInstance().FilesToEncrypt.Add(NewFileExtension);
            FileExtensions = Settings.GetInstance().FilesToEncrypt;
            Settings.GetInstance().SaveSettings();
            MainWindowViewModel.Instance.ChangeView("Settings");
        }

        public string NewFileExtension { get; set; } = string.Empty;

        private void RemoveProcess(string? obj)
        {
            if (obj != null)
            {
                Settings.GetInstance().LockProcesses.Remove(obj);
                Settings.GetInstance().SaveSettings();
                MainWindowViewModel.Instance.ChangeView("Settings");
            }
        }

        private void AddProcess()
        {
            Settings.GetInstance().LockProcesses.Add(NewLockProcess);
            LockProcesses = Settings.GetInstance().LockProcesses;
            Settings.GetInstance().SaveSettings();
            MainWindowViewModel.Instance.ChangeView("Settings");
        }

        public string NewLockProcess { get; set; } = string.Empty;

        public LogFormat LogFormat
        {
            get => _logFormat;
            set => SetProperty(ref _logFormat, value);
        }

        public string CurrentLanguage
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        public List<string> FileExtensions
        {
            get => _fileExtensions;
            set => SetProperty(ref _fileExtensions, value);
        }

        public List<string> LockProcesses
        {
            get => _lockProcesses;
            set => SetProperty(ref _lockProcesses, value);
        }

        public long MaxFileSizeInKB
        {
            get => _maxFileSizeInKB;
            set => SetProperty(ref _maxFileSizeInKB, value);
        }

        public string NotificationMessage
        {
            get => _notificationMessage;
            set => SetProperty(ref _notificationMessage, value);
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
            OnPropertyChanged(nameof(LogBoxSetting));
            OnPropertyChanged(nameof(FrenchRadioSetting));
            OnPropertyChanged(nameof(EnglishRadioSetting));
            OnPropertyChanged(nameof(EncryptExtensionSetting));
            OnPropertyChanged(nameof(ProcessLockSetting));
            OnPropertyChanged(nameof(AddButtonSetting));
            OnPropertyChanged(nameof(RemoveButtonSetting));
            MainWindowViewModel.Instance.UpdateLanguage();
        }

        public void SaveMaxFileSize()
        {
            if (long.TryParse(MaxFileSizeInKB.ToString(), out long result) && result > 0)
            {
                Settings.GetInstance().MaxFileSizeInKB = result;
                Settings.GetInstance().SaveSettings();
                NotificationMessage = $"Maximum file size set to {MaxFileSizeInKB} Ko.";
            }
            else
            {
                NotificationMessage = "Please enter a valid value for the maximum file size.";
            }
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

        public string LogBoxSetting => Language.GetInstance().GetString("LogBoxSetting");
        public string FrenchRadioSetting => Language.GetInstance().GetString("FrenchRadioSetting");
        public string EnglishRadioSetting => Language.GetInstance().GetString("EnglishRadioSetting");
        public string EncryptExtensionSetting => Language.GetInstance().GetString("EncryptExtensionSetting");
        public string ProcessLockSetting => Language.GetInstance().GetString("ProcessLockSetting");
        public string AddButtonSetting => Language.GetInstance().GetString("AddButtonSetting");
        public string RemoveButtonSetting => Language.GetInstance().GetString("RemoveButtonSetting");
    }
}
