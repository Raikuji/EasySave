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

		public static string RESOURCEPATH = AppDomain.CurrentDomain.BaseDirectory + "\\resources";
		public static string BACKUPJOBFILENAME = "backup_jobs.json";
		public static string PATH = $"{RESOURCEPATH}\\{BACKUPJOBFILENAME}";

		public ICommand UpdateBackupJobCommand { get; }
		public ICommand DeleteBackupJobCommand { get; }
		public ICommand ExecuteBackupJobCommand { get; }

		public BackupJobListViewModel()
		{
			_backupJobs = new BackupJobList();
			LoadBackupJobs();
			UpdateBackupJobCommand = new RelayCommand<BackupJob>(UpdateBackupJob);
			DeleteBackupJobCommand = new RelayCommand<BackupJob>(DeleteBackupJob);
			ExecuteBackupJobCommand = new RelayCommand<BackupJob>(ExecuteBackupJob);
		}

		public void LoadBackupJobs()
		{
			if (File.Exists(PATH))
			{
				BackupJobs.LoadBackupJobs(PATH);
				OnPropertyChanged(nameof(BackupJobs));
			}
		}

		private void RemoveBackupJob(BackupJob job)
		{
			BackupJobs.RemoveJob(job);
			BackupJobs.SaveBackupJobs(PATH);
		}

		public void UpdateBackupJob(BackupJob job)
		{
			RemoveBackupJob(job);
			MainWindowViewModel.Instance.BackupJob = job;
			MainWindowViewModel.Instance.ChangeView("UpdateBackup");
		}

		public void DeleteBackupJob(BackupJob job)
		{
			RemoveBackupJob(job);
			MainWindowViewModel.Instance.ChangeView("BackupList");
		}

		public void ExecuteBackupJob(BackupJob job)
		{
			job.Execute();
		}

		public BackupJobList BackupJobs
		{
			get => _backupJobs;
			set => SetProperty(ref _backupJobs, value);
		}
	}
}
