using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyCmd.Model;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Diagnostics;

namespace EasyGui.ViewModels
{
	public partial class BackupJobListViewModel : ObservableObject
	{
		[ObservableProperty]
		private BackupJobList _backupJobs;

		[ObservableProperty]
		private BackupJob? _selectedJob;

		[ObservableProperty]
		private bool _isRunning = false;

		[ObservableProperty]
		private bool _isNotRunning = true;

		public static string RESOURCEPATH = AppDomain.CurrentDomain.BaseDirectory + "\\resources";
		public static string BACKUPJOBFILENAME = "backup_jobs.json";
		public static string PATH = $"{RESOURCEPATH}\\{BACKUPJOBFILENAME}";

		public ICommand UpdateBackupJobCommand { get; }
		public ICommand DeleteBackupJobCommand { get; }
		public ICommand ExecuteBackupJobCommand { get; }
		public ICommand PauseBackupJobCommand { get; }
		public ICommand StopBackupJobCommand { get; }


		public BackupJobListViewModel()
		{
			_backupJobs = BackupJobList.GetInstance();
			LoadBackupJobs();
			UpdateBackupJobCommand = new RelayCommand<BackupJob>(UpdateBackupJob);
			DeleteBackupJobCommand = new RelayCommand<BackupJob>(DeleteBackupJob);
			ExecuteBackupJobCommand = new RelayCommand<BackupJob>(ExecuteBackupJob);
			PauseBackupJobCommand = new RelayCommand<BackupJob>(PauseBackupJob);
			StopBackupJobCommand = new RelayCommand<BackupJob>(StopBackupJob);
			pauseMenuList = Language.GetInstance().GetString("PauseMenuList");
			ProcessWatcher.GetInstance().OnProcessStateChanged += UpdatePauseMenuAction;
		}

		public void SelectionChanged(object? sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				SelectedJob = e.AddedItems[0] as BackupJob;
				if (SelectedJob != null)
				{
					IsRunning = SelectedJob.IsRunning;
					IsNotRunning = !SelectedJob.IsRunning;
					UpdatePauseMenu(SelectedJob);
				}
			}
		}

		public void LoadBackupJobs()
		{
			if (File.Exists(PATH))
			{
				BackupJobList.GetInstance().LoadBackupJobs(PATH);
			}
		}

		private void RemoveBackupJob(BackupJob? job)
		{
			if (job != null)
			{
				BackupJobList.GetInstance().RemoveJob(job);
				BackupJobList.GetInstance().SaveBackupJobs(PATH);
			}
		}

		public void UpdateBackupJob(BackupJob? job)
		{
			if (job != null)
			{
				RemoveBackupJob(job);
				MainWindowViewModel.Instance.CurrentBackupJob = job;
				MainWindowViewModel.Instance.ChangeView("UpdateBackup");
			}
		}

		public void DeleteBackupJob(BackupJob? job)
		{
			if (job != null)
			{
				RemoveBackupJob(job);
				MainWindowViewModel.Instance.ChangeView("BackupList");
			}
		}

		public async void ExecuteBackupJob(BackupJob? job)
		{
			if (job != null)
			{
				IsRunning = true;
				IsNotRunning = false;
				await Task.Run(() =>
				{
					MainWindowViewModel.Instance.AddRunningJob(job);
					job.Execute();
					while (job.IsRunning)
					{
						System.Windows.Application.Current.Dispatcher.Invoke(() =>
						{
							job.ProgressValue = job.Progress();
						});
					}
				}).ContinueWith(t =>
				{
					IsRunning = false;
					IsNotRunning = true;
				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
		}

		public void PauseBackupJob(BackupJob? job)
		{
			if (job != null)
			{
				if (!job.IsPaused)
					job.Pause();
				else
					job.Resume();
				UpdatePauseMenu(job);
			}
		}

		public void UpdatePauseMenu(BackupJob? job)
		{
			if (job != null)
			{
				PauseMenuList = job.IsPaused ? Language.GetInstance().GetString("ResumeMenuList") : Language.GetInstance().GetString("PauseMenuList");
				OnPropertyChanged(nameof(PauseMenuList));
			}
		}

		public void UpdatePauseMenuAction(bool paused)
		{
			PauseMenuList = paused ? Language.GetInstance().GetString("ResumeMenuList") : Language.GetInstance().GetString("PauseMenuList");
			OnPropertyChanged(nameof(PauseMenuList));
		}

		public void StopBackupJob(BackupJob? job)
		{
			if (job != null)
			{
				job.Stop();
				IsRunning = false;
				IsNotRunning = true;
			}
		}

		public static int ListSize => Settings.GetInstance().LanguageCode == "en" ? 620 : 610;
		public static int MenuSize => 685 - ListSize;
		public static string NameHeaderList => Language.GetInstance().GetString("NameHeaderList");
		public static string SourceHeaderList => Language.GetInstance().GetString("SourceHeaderList");
		public static string DestinationHeaderList => Language.GetInstance().GetString("DestinationHeaderList");
		public static string TypeHeaderList => Language.GetInstance().GetString("TypeHeaderList");
		public static string ExecuteMenuList => Language.GetInstance().GetString("ExecuteMenuList");
		public static string EditMenuList => Language.GetInstance().GetString("EditMenuList");
		public static string DeleteMenuList => Language.GetInstance().GetString("DeleteMenuList");
		public static string StopMenuList => Language.GetInstance().GetString("StopMenuList");

		private string pauseMenuList;

		public string PauseMenuList { get => pauseMenuList; set => SetProperty(ref pauseMenuList, value); }
	}
}
