using System.IO;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyCmd.Model;

namespace EasyGui.ViewModels
{
	internal class AddBackupJobViewModel : ObservableObject
	{
		private string _name;
		private string _sourceFolder;
		private string _destinationFolder;
		private int _type;

		private System.Windows.Visibility _sourceError;
		private System.Windows.Visibility _destinationError;

		public ICommand AddBackupJob { get; }
		public ICommand BrowseSource { get; }
		public ICommand BrowseDestination { get; }

		public AddBackupJobViewModel()
		{
			_name = "";
			_sourceFolder = "";
			_destinationFolder = "";
			_type = 1;
			_sourceError = System.Windows.Visibility.Hidden;
			_destinationError = System.Windows.Visibility.Hidden;
			AddBackupJob = new RelayCommand(AddBackup);
			BrowseSource = new RelayCommand(BrowseSourceFolder);
			BrowseDestination = new RelayCommand(BrowseDestinationFolder);
		}

		public void SetBackupJob(BackupJob backupJob)
		{
			Name = backupJob.Name;
			SourceFolder = backupJob.Source;
			DestinationFolder = backupJob.Destination;
			if (backupJob.GetStrategyId() == 1)
			{
				FullType = true;
			}
			else
			{
				IncrementalType = true;
			}
		}

		public void AddBackup()
		{
			if (!(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(SourceFolder) || string.IsNullOrEmpty(DestinationFolder)))
			{
				SourceError = System.Windows.Visibility.Hidden;
				DestinationError = System.Windows.Visibility.Hidden;
				if (Directory.Exists(SourceFolder))
				{
					bool create = true;
					if (!Directory.Exists(DestinationFolder))
					{
						try
						{
							Directory.CreateDirectory(DestinationFolder);
						}
						catch (Exception)
						{
							create = false;
						}
					}
					if (create)
					{
						BackupJobList.GetInstance().AddJob(new BackupJob(Name, SourceFolder, DestinationFolder, Type));
						BackupJobList.GetInstance().SaveBackupJobs(BackupJobListViewModel.PATH);
						MainWindowViewModel.Instance.ChangeView("BackupList");
					}
					else
					{
						DestinationError = System.Windows.Visibility.Visible;
					}
				}
				else
				{
					SourceError = System.Windows.Visibility.Visible;
				}
			}
		}

		public void BrowseSourceFolder()
		{
			using var dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				_sourceFolder = dialog.SelectedPath;
			}
			OnPropertyChanged(nameof(SourceFolder));
		}

		public void BrowseDestinationFolder()
		{
			using var dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				_destinationFolder = dialog.SelectedPath;
			}
			OnPropertyChanged(nameof(DestinationFolder));
		}

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public string SourceFolder
		{
			get => _sourceFolder;
			set => SetProperty(ref _sourceFolder, value);
		}

		public string DestinationFolder
		{
			get => _destinationFolder;
			set => SetProperty(ref _destinationFolder, value);
		}

		public int Type
		{
			get => _type;
			set => SetProperty(ref _type, value);
		}

		public bool FullType
		{
			get => Type == 1;
			set => SetProperty(ref _type, value ? 1 : 2);
		}

		public bool IncrementalType
		{
			get => Type == 2;
			set => SetProperty(ref _type, value ? 2 : 1);
		}

		public System.Windows.Visibility SourceError 
		{ 
			get => _sourceError; 
			set => SetProperty(ref _sourceError, value); 
		}

		public System.Windows.Visibility DestinationError
		{
			get => _destinationError;
			set => SetProperty(ref _destinationError, value);
		}

		public static string NameLabelAdd => Language.GetInstance().GetString("NameLabelAdd");
		public static string SourceLabelAdd => Language.GetInstance().GetString("SourceLabelAdd");
		public static string DestinationLabelAdd => Language.GetInstance().GetString("DestinationLabelAdd");
		public static string TypeLabelAdd => Language.GetInstance().GetString("TypeLabelAdd");
		public static string FullLabelAdd => Language.GetInstance().GetString("FullLabelAdd");
		public static string IncrementalLabelAdd => Language.GetInstance().GetString("IncrementalLabelAdd");
		public static string SaveButtonAdd => Language.GetInstance().GetString("SaveButtonAdd");
	}
}