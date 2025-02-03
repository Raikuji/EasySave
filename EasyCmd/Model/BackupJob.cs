using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupJob
    {
        private string _name;
        private string _source;
        private string _destination;
        private IBackupWorkStrategy _backupStrategy;

        public BackupJob(string name, string source, string destination, IBackupWorkStrategy backupStrategy)
        {
            _name = name;
            _source = source;
            _destination = destination;
            _backupStrategy = backupStrategy;
        }

        public void Execute()
        {
            _backupStrategy.Execute(_source, _destination);
        }
    }
}
