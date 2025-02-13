using System;
using System.Collections.Generic;
using System.IO;
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
            english.LoadLanguage(RESOURCEPATH + "\\en.json");
            Language.GetInstance().SetLanguage(english);
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
            _backupJobList.SaveBackupJobs(_path);
        }

        public bool AddBackupJob(string name, string source, string destination, int strategyId, LogFormat logFormat = LogFormat.JSON)
        {
            bool isValid;
            try
            {
                _backupJobList.Add(new BackupJob(name, source, destination, strategyId, logFormat));
                isValid = true;
            }
            catch (ArgumentException)
            {
                isValid = false;
            }
            return isValid;
        }

        public bool UpdateBackupJob(int index, string name, string source, string destination, int strategyId, LogFormat logFormat = LogFormat.JSON)
        {
            bool isValid;
            try
            {
                _backupJobList.Update(index - 1, new BackupJob(name, source, destination, strategyId, logFormat));
                isValid = true;
            }
            catch (ArgumentOutOfRangeException)
            {
                isValid = false;
            }
            return isValid;
        }
    }
}
