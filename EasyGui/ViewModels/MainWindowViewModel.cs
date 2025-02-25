using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyCmd.Model;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows;

namespace EasyGui.ViewModels
{
	partial class MainWindowViewModel : ObservableObject
	{
		private Page _currentView;

		public ICommand ChangeViewCommand { get; }
		public ICommand CloseApplicationCommand { get; }
		public ICommand ClearStatusListCommand { get; }

		[ObservableProperty]
		private ObservableCollection<BackupJob> _statusList;

		[ObservableProperty]
		private string _isStatusListVisible;

		[ObservableProperty]
		private bool _isExpanded;

		public MainWindowViewModel()
		{
			ChangeViewCommand = new RelayCommand<string>(ChangeView);
			CloseApplicationCommand = new RelayCommand(CloseApplication);
			ClearStatusListCommand = new RelayCommand(ClearStatusList);
			_currentView = new Views.BackupJobListView();
			Settings.GetInstance().LoadSettings();
			Settings.GetInstance().SetLanguage();
			_statusList = new ObservableCollection<BackupJob>();
			_isStatusListVisible = "Hidden";
			_isExpanded = false;
			BindingOperations.EnableCollectionSynchronization(StatusList, new object());
		}

		public void CloseApplication()
		{
			BackupJobList.GetInstance().StopAll();
			System.Windows.Application.Current.Shutdown();
		}

		public void AddRunningJob(BackupJob job)
		{
			if (IsStatusListVisible == "Hidden")
			{
				IsStatusListVisible = "Visible";
				IsExpanded = true;
			}
			lock (StatusList)
			{
				if (!StatusList.Contains(job))
				{
					System.Windows.Application.Current.Dispatcher.Invoke(() =>
					{
						StatusList.Add(job);
					});
				}
			}
		}

		public void UpdateRunningJob(BackupJob job)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(() =>
			{
				OnPropertyChanged(nameof(job.ProgressValue));
			});
		}

		public void ClearStatusList()
		{
			lock (StatusList)
			{
				StatusList.Clear();
			}
			IsStatusListVisible = "Hidden";
			IsExpanded = false;
		}

		public Page CurrentView
		{
			get => _currentView;
			set => SetProperty(ref _currentView, value);
		}

		public void ChangeView(string? view)
		{
			switch (view)
			{
				case "BackupList":
					CurrentView = new Views.BackupJobListView();
					break;
				case "AddBackup":
					CurrentView = new Views.AddBackupJobView();
					break;
				case "UpdateBackup":
					CurrentView = new Views.AddBackupJobView();
					if (CurrentBackupJob != null)
						((AddBackupJobViewModel)CurrentView.DataContext).SetBackupJob(CurrentBackupJob);
					break;
				case "Settings":
					CurrentView = new Views.SettingsView();
					break;
				default:
					CurrentView = new Views.BackupJobListView();
					break;
			}
		}

		public BackupJob? CurrentBackupJob { get; set; }

		public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

		public string FileDockMain => Language.GetInstance().GetString("FileDockMain");
		public string QuitDockMain => Language.GetInstance().GetString("QuitDockMain");
		public string ListMenuMain => Language.GetInstance().GetString("ListMenuMain");
		public string AddMenuMain => Language.GetInstance().GetString("AddMenuMain");
		public string SettingsMain => Language.GetInstance().GetString("SettingsMain");
		public string ExpanderMain => Language.GetInstance().GetString("ExpanderMain");
		public string ClearStatusListMain => Language.GetInstance().GetString("ClearStatusListMain");

		public void UpdateLanguage()
		{
			OnPropertyChanged(nameof(FileDockMain));
			OnPropertyChanged(nameof(QuitDockMain));
			OnPropertyChanged(nameof(ListMenuMain));
			OnPropertyChanged(nameof(AddMenuMain));
			OnPropertyChanged(nameof(SettingsMain));
			OnPropertyChanged(nameof(ExpanderMain));
			OnPropertyChanged(nameof(ClearStatusListMain));
		}
	}
}
