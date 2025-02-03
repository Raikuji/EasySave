using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupWorkFull : IBackupWorkStrategy
    {
        public void Execute(string source, string destination)
        {
            Console.WriteLine("Full backup");
        }
    }
}
