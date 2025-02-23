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

namespace EasyGui.ViewModels
{
	public class BackupJobListViewModel : ObservableObject
	{
		private BackupJobList _backupJobs;
		private bool _isRunning = false;

		public static string RESOURCEPATH = AppDomain.CurrentDomain.BaseDirectory + "\\resources";
		public static string BACKUPJOBFILENAME = "backup_jobs.json";
		public static string PATH = $"{RESOURCEPATH}\\{BACKUPJOBFILENAME}";

		public ICommand UpdateBackupJobCommand { get; }
		public ICommand DeleteBackupJobCommand { get; }
		public ICommand ExecuteBackupJobCommand { get; }

		public BackupJobListViewModel()
		{
			_backupJobs = BackupJobList.GetInstance();
			LoadBackupJobs();
			UpdateBackupJobCommand = new RelayCommand<BackupJob>(UpdateBackupJob);
			DeleteBackupJobCommand = new RelayCommand<BackupJob>(DeleteBackupJob);
			ExecuteBackupJobCommand = new RelayCommand<BackupJob>(ExecuteBackupJob);
		}

		public void LoadBackupJobs()
		{
			if (File.Exists(PATH))
			{
				BackupJobList.GetInstance().LoadBackupJobs(PATH);
				OnPropertyChanged(nameof(BackupJobs));
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
				MainWindowViewModel.Instance.BackupJob = job;
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
				MainWindowViewModel.Instance.StatusMessage = $"{job.Name} {Language.GetInstance().GetString("JobRunning")}";
				await job.ExecuteAsync();
				MainWindowViewModel.Instance.StatusMessage = $"{job.Name} {Language.GetInstance().GetString("JobEnded")}";
			}
		}

		public async void Execute()
		{
			if (!_isRunning)
			{
				_isRunning = true;
				foreach (BackupJob job in BackupJobs)
				{
					if (job.IsRunning)
					{
						MainWindowViewModel.Instance.StatusMessage = $"{job.Name} {Language.GetInstance().GetString("JobRunning")}";
						await job.ExecuteAsync();
						MainWindowViewModel.Instance.StatusMessage = $"{job.Name} {Language.GetInstance().GetString("JobEnded")}";
					}
				}
				_isRunning = false;
			}
		}

		public BackupJobList BackupJobs
		{
			get => _backupJobs;
			set => SetProperty(ref _backupJobs, value);
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
	}
}
