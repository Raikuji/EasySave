using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyCmd.ViewModel;

namespace EasyCmd.View
{
    internal class BackupView
    {
        public void ShowStartMessage()
        {
            Console.WriteLine("Welcome to EasyCmd Backup");
        }
        public void ShowMenu()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Show backup jobs");
            Console.WriteLine("2. Add backup job");
            Console.WriteLine("3. Remove backup job");
            Console.WriteLine("4. Update backup job");
            Console.WriteLine("5. Execute backup job");
            Console.WriteLine("6. Exit");
        }
        public void ShowAllBackupJobs(string jobs)
        {
            Console.WriteLine("Backup jobs:");
            Console.WriteLine(jobs);
        }
        public void ShowInvalidOption()
        {
            Console.WriteLine("Invalid option");
        }
        public void ShowAddBackupJobName()
        {
            Console.WriteLine("Enter the name of the backup job:");
        }
        public void ShowAddBackupJobSource()
        {
            Console.WriteLine("Enter the source of the backup job:");
        }
        public void ShowAddBackupJobDestination()
        {
            Console.WriteLine("Enter the destination of the backup job:");
        }
        public void ShowAddBackupJobStrategy()
        {
            Console.WriteLine("Enter the strategy id of the backup job:");
            Console.WriteLine("1. Full");
            Console.WriteLine("2. Differential");
        }
        public void ShowRemoveBackupJob()
        {
            Console.WriteLine("Enter the index of the backup job:");
        }
        public void ShowExecuteBackupJob()
        {
            Console.WriteLine("Enter the index of the backup job to execute:");
        }
    }
}
