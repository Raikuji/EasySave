using System;
using System.Collections.Generic;
using EasySave.Model;

namespace EasySave
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define a list of backup jobs
            List<BackupJob> jobs = new List<BackupJob>
            {
                new BackupJob(@"C:\Source1", @"D:\Backup1"),
                new BackupJob(@"C:\Source2", @"D:\Backup2"),
                new BackupJob(@"C:\Source3", @"D:\Backup3")
            };

            // Create and start the backup manager
            BackupManager manager = new BackupManager();
            manager.StartBackup(jobs);
        }
    }
}