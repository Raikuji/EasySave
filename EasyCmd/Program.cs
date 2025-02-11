using EasyCmd.Model;
using EasyCmd.ViewModel;
using System;

namespace EasyCmd
{
    public class Program
    {
        public static void Main(string[] args)
        {
			BackupViewModel backupViewModel = new BackupViewModel();
			if (args.Length == 0)
            {
                backupViewModel.Show();
            }
            else
            {
				bool hasWorked = backupViewModel.ExecuteSomeBackupJobs(args);
                Environment.Exit(hasWorked ? 0 : 1);
			}
        }
    }
}