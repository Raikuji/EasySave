using EasyCmd.Model;
using EasyCmd.ViewModel;
using System;

namespace EasyCmd;

public static class Program
{
    public static void Main(string[] args)
    {
        BackupViewModel backupViewModel = new BackupViewModel();
        backupViewModel.Show();
    }
}