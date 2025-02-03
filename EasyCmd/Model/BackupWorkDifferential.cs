using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupWorkDifferential : IBackupWorkStrategy
    {
        public void Execute(string source, string destination)
        {
            Console.WriteLine("Differential backup");
        }
    }
}
