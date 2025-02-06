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
            backupViewModel.Show();
        }
    }
}