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
        private string _savePath = AppDomain.CurrentDomain.BaseDirectory + "\\resources";
        private string _backupJobFileName = "backup_jobs.json";
        private string _path;

        public BackupViewModel()
        {
            _backupJobList = new BackupJobList();
            _path = _savePath + "\\" + _backupJobFileName;
            _backupView = new BackupView();
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
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            string path = _savePath + "\\" + _backupJobFileName;
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
            _backupView.ShowStartMessage();

            while (continueLoop)
            {
                LoadBackupJobs();
                _backupView.ShowMenu();
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
                        continueLoop = false;
                        break;
                    default:
                        _backupView.ShowInvalidOption();
                        break;
                }
                Console.Clear();
            }
        }
        public void ShowAllBackupJobs()
        {
            _backupView.ShowAllBackupJobs(GetBackupJobList());
        }
        public void ShowAddBackupJob()
        {
            _backupView.ShowAddBackupJobName();
            string ?name = Console.ReadLine();
            _backupView.ShowAddBackupJobSource();
            string ?source = Console.ReadLine();
            _backupView.ShowAddBackupJobDestination();
            string ?destination = Console.ReadLine();
            _backupView.ShowAddBackupJobStrategy();
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
            _backupView.ShowRemoveBackupJob();
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
            _backupView.ShowRemoveBackupJob();
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int index))
            {
                _backupView.ShowAddBackupJobName();
                string? name = Console.ReadLine();
                _backupView.ShowAddBackupJobSource();
                string? source = Console.ReadLine();
                _backupView.ShowAddBackupJobDestination();
                string? destination = Console.ReadLine();
                _backupView.ShowAddBackupJobStrategy();
                string? strategyInput = Console.ReadLine();
                if (int.TryParse(strategyInput, out int strategyId))
                {
                    bool isValid = false;
                    if (name != null && source != null && destination != null)
                    {
                        isValid = UpdateBackupJob(index, name, source, destination, strategyId);
                    }
                    else
                    {
                        ShowInvalidOption();
                    }
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
            else
            {
                Console.WriteLine("Invalid input");
            }
        }
        public void ShowExecuteBackupJob()
        {
            _backupView.ShowExecuteBackupJob();
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int index))
            {
                ExecuteBackupJob(index);
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
        }
        public void ShowInvalidOption()
        {
            _backupView.ShowInvalidOption();
            Console.ReadKey();
        }
    }
}