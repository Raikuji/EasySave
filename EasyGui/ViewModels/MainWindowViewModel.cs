﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyCmd.Model;

namespace EasyGui.ViewModels
{
    class MainWindowViewModel : ObservableObject
	{
		private Page _currentView;

		public ICommand ChangeViewCommand { get; }
		public ICommand CloseApplicationCommand { get; }

		public MainWindowViewModel()
		{
			ChangeViewCommand = new RelayCommand<string>(ChangeView);
			CloseApplicationCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());
			_currentView = new Views.BackupJobListView();
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
					if (BackupJob != null)
						((AddBackupJobViewModel)CurrentView.DataContext).SetBackupJob(BackupJob);
					break;
				case "Settings":
					//CurrentView = new Views.SettingsView();
					break;
				default:
					CurrentView = new Views.BackupJobListView();
					break;
			}
		}

		public BackupJob? BackupJob { get; set; }

		public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();
	}
}
