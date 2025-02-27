using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyCmd.Model;
using EasyCmd.View;
using EasyLog;

namespace EasyCmd.ViewModel
{
    /// <summary>
    /// Class that represents the view model of the backup.
    /// </summary>
    internal class BackupViewModel
    {
        private BackupView _backupView;
        private string RESOURCEPATH = AppDomain.CurrentDomain.BaseDirectory + "\\resources";
        private string BACKUPJOBFILENAME = "backup_jobs.json";
        private string _path;

		/// <summary>
		/// Constructor of the BackupViewModel class.
		/// </summary>
		public BackupViewModel()
        {
			_path = RESOURCEPATH + "\\" + BACKUPJOBFILENAME;
			BackupJobList.GetInstance();
			Settings.GetInstance().LoadSettings();
			switch (Settings.GetInstance().LogFormat)
			{
				case LogFormat.JSON:
					Settings.GetInstance().LogFormat = LogFormat.JSON;
					break;
				case LogFormat.XML:
					Settings.GetInstance().LogFormat = LogFormat.XML;
					break;
				default:
					Settings.GetInstance().LogFormat = LogFormat.JSON;
					break;
			}
			LanguageDictionary language = new LanguageDictionary();
			switch (Settings.GetInstance().LanguageCode)
			{
				case "en":
					language.LoadLanguage(RESOURCEPATH + "\\" + "en_cmd.json");
					break;
				case "fr":
					language.LoadLanguage(RESOURCEPATH + "\\" + "fr_cmd.json");
					break;
				default:
					language.LoadLanguage(RESOURCEPATH + "\\" + "en_cmd.json");
					break;
			}
			Language.GetInstance().SetLanguage(language);
			_backupView = new BackupView();
        }

        /// <summary>
        /// Returns the number of backup jobs.
        /// </summary>
        /// <returns></returns>
        public int GetBackupJobCount()
        {
            return BackupJobList.GetInstance().Count();
        }

        /// <summary>
        /// Returns the list of backup jobs.
        /// </summary>
        /// <returns></returns>
        public string GetBackupJobList()
        {
            return BackupJobList.GetInstance().ToString();
        }

        /// <summary>
        /// Loads the backup jobs.
        /// </summary>
        public void LoadBackupJobs()
        {
			BackupJobList.GetInstance().LoadBackupJobs(_path);
        }

        /// <summary>
        /// Saves the backup jobs.
        /// </summary>
        public void SaveBackupJobs()
        {
            if (!Directory.Exists(RESOURCEPATH))
            {
                Directory.CreateDirectory(RESOURCEPATH);
            }
            string path = RESOURCEPATH + "\\" + BACKUPJOBFILENAME;
			BackupJobList.GetInstance().SaveBackupJobs(path);
        }

		/// <summary>
		/// Adds a backup job.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <param name="strategyId"></param>
		/// <returns></returns>
		public bool AddBackupJob(string name, string source, string destination, int strategyId)
		{
			bool isValid;
			try
			{
				BackupJobList.GetInstance().AddJob(new BackupJob(name, source, destination, strategyId));
				isValid = true;
			}
			catch (ArgumentException)
			{
				isValid = false;
			}
			return isValid;
		}

		/// <summary>
		/// Removes a backup job.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool RemoveBackupJob(int index)
		{
			bool isValid;
			try
			{
				BackupJobList.GetInstance().RemoveJobAt(index - 1);
				isValid = true;
			}
			catch (ArgumentOutOfRangeException)
			{
				isValid = false;
			}
			return isValid;
		}

        /// <summary>
        /// Updates a backup job.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="strategyId"></param>
        /// <returns></returns>
        public bool UpdateBackupJob(int index, string name, string source, string destination, int strategyId)
        {
            bool isValid;
            try
            {
				BackupJobList.GetInstance().Update(index - 1, new BackupJob(name, source, destination, strategyId));
                isValid = true;
            }
            catch (ArgumentOutOfRangeException)
            {
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Executes a backup job.
        /// </summary>
        /// <param name="index"></param>
        public void ExecuteBackupJob(int index)
        {
			BackupJobList.GetInstance().Execute(index - 1);
        }

        internal bool ExecuteRange(string arg)
        {
            bool isValid = false;
            if (int.TryParse(arg.Substring(0, arg.IndexOf('-')), out int firstIndex))
            {
                if (int.TryParse(arg.Substring(arg.IndexOf('-') + 1), out int lastIndex))
                {
                    isValid = BackupJobList.GetInstance().ExecuteRange(firstIndex - 1, lastIndex - 1);
                }
            }
            return isValid;
        }

        /// <summary>
        /// Executes some backup jobs.
        /// </summary>
        /// <param name="args"></param>
        public bool ExecuteSomeBackupJobs(string[] args)
        {
            try
            {
                bool isValid = false;
                LoadBackupJobs();
                foreach (string arg in args)
                {
                    if (int.TryParse(arg, out int index))
                    {
                        isValid = BackupJobList.GetInstance().Execute(index - 1);
                    }
                    else
                    {
                        if (arg.Contains("-"))
                        {
                            isValid = ExecuteRange(arg);
                        }
                        else if (arg.Contains(","))
                        {
                            string[] indexes = arg.Split(',');
                            foreach (string indexMulti in indexes)
                            {
                                if (int.TryParse(indexMulti, out int i))
                                {
                                    isValid = BackupJobList.GetInstance().Execute(i - 1);
                                }
                            }
                        }
                    }
                }
                return isValid;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Shows the views of the EasyCmd program.
        /// </summary>
        public void Show()
        {
            bool continueLoop = true;
            _backupView.Display(Language.GetInstance().GetString("Start"));

            while (continueLoop)
            {
                LoadBackupJobs();
                _backupView.Display(Language.GetInstance().GetString("Menu"));
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        ShowAllBackupJobs();
                        Console.ReadLine();
                        break;
                    case "2":
                        Console.Clear();
                        ShowAllBackupJobs();
                        ShowAddBackupJob();
                        SaveBackupJobs();
                        break;
                    case "3":
                        Console.Clear();
                        ShowAllBackupJobs();
                        ShowRemoveBackupJob();
                        SaveBackupJobs();
                        break;
                    case "4":
                        Console.Clear();
                        ShowAllBackupJobs();
                        ShowUpdateBackupJob();
                        SaveBackupJobs();
                        break;
                    case "5":
                        Console.Clear();
                        ShowAllBackupJobs();
                        ShowExecuteBackupJob();
                        break;
                    case "6":
                        Console.Clear();
                        ChangeLanguage();
                        break;
                    case "7":
                        Console.Clear();
                        ChangeLogFormat();
                        break;
                    case "8":
                        continueLoop = false;
                        break;
                    default:
                        ShowInvalidOption();
                        break;
                }
                Console.Clear();
            }
        }

        private void ChangeLogFormat()
        {
            _backupView.Display(Language.GetInstance().GetString("LogFormat"));
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
					Settings.GetInstance().LogFormat = LogFormat.JSON;
                    break;
                case "2":
					Settings.GetInstance().LogFormat = LogFormat.XML;
                    break;
                default:
                    ShowInvalidOption();
                    break;
            };
			Settings.GetInstance().SaveSettings();
		}

        /// <summary>
        /// Shows all backup jobs.
        /// </summary>
        public void ShowAllBackupJobs()
        {
            _backupView.Display(Language.GetInstance().GetString("All"), GetBackupJobList());
        }

        /// <summary>
        /// Shows the add backup job view.
        /// </summary>
        public void ShowAddBackupJob()
        {
            _backupView.Display(Language.GetInstance().GetString("AddName"));
            string? name = Console.ReadLine();
            _backupView.Display(Language.GetInstance().GetString("AddSource"));
            string? source = Console.ReadLine();
            _backupView.Display(Language.GetInstance().GetString("AddDestination"));
            string? destination = Console.ReadLine();
            _backupView.Display(Language.GetInstance().GetString("AddStrategy"));
            string? strategyInput = Console.ReadLine();
            _backupView.Display(Language.GetInstance().GetString("AddStrategy"));

            if (int.TryParse(strategyInput, out int strategyId))
            {
                bool isValid = false;
                if (name != null && source != null && destination != null)
                {
                    isValid = AddBackupJob(name, source, destination, strategyId);
                }
                else
                {
                    ShowInvalidOption();
                }
            }
            else
            {
                ShowInvalidOption();
            }
        }

        /// <summary>
        /// Shows the remove backup job view.
        /// </summary>
        public void ShowRemoveBackupJob()
        {
            _backupView.Display(Language.GetInstance().GetString("Remove"));
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int index))
            {
                bool isValid = RemoveBackupJob(index);
                if (!isValid)
                {
                    ShowInvalidOption();
                }
            }
            else
            {
                ShowInvalidOption();
            }
        }

        /// <summary>
        /// Shows the update backup job view.
        /// </summary>
        public void ShowUpdateBackupJob()
        {
            _backupView.Display(Language.GetInstance().GetString("Remove"));
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int index))
            {
                ShowAddBackupJob();
            }
            else
            {
                ShowInvalidOption();
            }
        }

        /// <summary>
        /// Shows the menu to execute a backup job.
        /// </summary>
        public void ShowExecuteBackupJob()
        {
            _backupView.Display(Language.GetInstance().GetString("Execute"));
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int index))
            {
                ExecuteBackupJob(index);
            }
            else
            {
                ShowInvalidOption();
            }
        }

        /// <summary>
        /// Shows the menu to change the language and changes it.
        /// </summary>
        public void ChangeLanguage()
        {
            _backupView.Display(Language.GetInstance().GetString("Lang"));
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int languageId))
            {
                LanguageDictionary language = new LanguageDictionary();
                string langFileName = "en_cmd.json";
                bool exists = true;
                switch (languageId)
                {
                    case 1:
                        langFileName = "en_cmd.json";
						Settings.GetInstance().LanguageCode = "en";
						break;
                    case 2:
                        langFileName = "fr_cmd.json";
						Settings.GetInstance().LanguageCode = "fr";
						break;
                    default:
                        ShowInvalidOption();
                        exists = false;
                        break;
                }
                if (exists)
                {
                    language.LoadLanguage(RESOURCEPATH + "\\" + langFileName);
                    Language.GetInstance().SetLanguage(language);
					Settings.GetInstance().SaveSettings();
				}
            }
        }

        /// <summary>
        /// Shows the invalid option message.
        /// </summary>
        public void ShowInvalidOption()
        {
            _backupView.Display(Language.GetInstance().GetString("InvalidOption"));
            Console.ReadKey();
        }
    }
}