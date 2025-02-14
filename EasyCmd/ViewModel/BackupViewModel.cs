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
        private BackupJobList _backupJobList;
        private BackupView _backupView;
        private string RESOURCEPATH = AppDomain.CurrentDomain.BaseDirectory + "\\resources";
        private string BACKUPJOBFILENAME = "backup_jobs.json";
        private string _path;

        /// <summary>
        /// Constructor of the BackupViewModel class.
        /// </summary>
        public BackupViewModel()
        {
            _backupJobList = new BackupJobList();
            _path = RESOURCEPATH + "\\" + BACKUPJOBFILENAME;
            _backupView = new BackupView();
            LanguageDictionary english = new LanguageDictionary();
            english.LoadLanguage(RESOURCEPATH + "\\" + "en.json");
            Language.GetInstance().SetLanguage(english);
            BackupJobLog.logFormat = LogFormat.JSON;
        }

        /// <summary>
        /// Returns the number of backup jobs.
        /// </summary>
        /// <returns></returns>
        public int GetBackupJobCount()
        {
            return _backupJobList.Count();
        }

        /// <summary>
        /// Returns the list of backup jobs.
        /// </summary>
        /// <returns></returns>
        public string GetBackupJobList()
        {
            return _backupJobList.ToString();
        }

        /// <summary>
        /// Loads the backup jobs.
        /// </summary>
        public void LoadBackupJobs()
        {
            _backupJobList.LoadBackupJobs(_path);
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
            _backupJobList.SaveBackupJobs(path);
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
                _backupJobList.Add(new BackupJob(name, source, destination, strategyId));
                isValid = true;
            }
            catch (ArgumentException e)
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
                _backupJobList.Remove(index - 1);
                isValid = true;
            }
            catch (ArgumentOutOfRangeException e)
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
                _backupJobList.Update(index - 1, new BackupJob(name, source, destination, strategyId));
                isValid = true;
            }
            catch (ArgumentOutOfRangeException e)
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
            _backupJobList.Execute(index - 1);
        }

        internal bool ExecuteRange(string arg)
        {
            bool isValid = false;
            if (int.TryParse(arg.Substring(0, arg.IndexOf('-')), out int firstIndex))
            {
                if (int.TryParse(arg.Substring(arg.IndexOf('-') + 1), out int lastIndex))
                {
                    isValid = _backupJobList.ExecuteRange(firstIndex - 1, lastIndex - 1);
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
                        isValid = _backupJobList.Execute(index - 1);
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
                                    isValid = _backupJobList.Execute(i - 1);
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
                    BackupJobLog.logFormat = LogFormat.JSON;
                    break;
                case "2":
                    BackupJobLog.logFormat = LogFormat.XML;
                    break;
                default:
                    ShowInvalidOption();
                    break;
            }
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
                string langFileName = "en.json";
                bool exists = true;
                switch (languageId)
                {
                    case 1:
                        langFileName = "en.json";
                        break;
                    case 2:
                        langFileName = "fr.json";
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