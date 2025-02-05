using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyCmd.Model;
using EasyCmd.View;

namespace EasyCmd.ViewModel
{
    internal class BackupViewModel
    {
        private BackupJobList _backupJobList;
        private BackupView _backupView;
        private string RESOURCEPATH = AppDomain.CurrentDomain.BaseDirectory + "\\resources";
        private string BACKUPJOBFILENAME = "backup_jobs.json";
        private string _path;

        public BackupViewModel()
        {
            _backupJobList = new BackupJobList();
            _path = RESOURCEPATH + "\\" + BACKUPJOBFILENAME;
            _backupView = new BackupView();
            LanguageDictionary english = new LanguageDictionary();
            english.LoadLanguage(RESOURCEPATH + "\\" + "en.json");
            Language.GetInstance().SetLanguage(english);
        }
        public int GetBackupJobCount()
        {
            return _backupJobList.Count();
        }
        public string GetBackupJobList()
        {
            return _backupJobList.ToString();
        }
        public void LoadBackupJobs()
        {
            _backupJobList.LoadBackupJobs(_path);
        }
        public void SaveBackupJobs()
        {
            if (!Directory.Exists(RESOURCEPATH))
            {
                Directory.CreateDirectory(RESOURCEPATH);
            }
            string path = RESOURCEPATH + "\\" + BACKUPJOBFILENAME;
            _backupJobList.SaveBackupJobs(path);
        }
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
        public void ExecuteBackupJob(int index)
        {
            _backupJobList.Execute(index - 1);
        }
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
                        continueLoop = false;
                        break;
                    default:
                        ShowInvalidOption();
                        break;
                }
                Console.Clear();
            }
        }
        public void ShowAllBackupJobs()
        {
            _backupView.Display(Language.GetInstance().GetString("All"),GetBackupJobList());
        }
        public void ShowAddBackupJob()
        {
            _backupView.Display(Language.GetInstance().GetString("AddName"));
            string ?name = Console.ReadLine();
            _backupView.Display(Language.GetInstance().GetString("AddSource"));
            string ?source = Console.ReadLine();
            _backupView.Display(Language.GetInstance().GetString("AddDestination"));
            string ?destination = Console.ReadLine();
            _backupView.Display(Language.GetInstance().GetString("AddStrategy"));
            string? strategyInput = Console.ReadLine();
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
        public void ShowInvalidOption()
        {
            _backupView.Display(Language.GetInstance().GetString("InvalidOption"));
            Console.ReadKey();
        }
    }
}