using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    /// <summary>
    /// Interface that represents the backup work strategy.
    /// </summary>
    internal interface IBackupWorkStrategy
    {
        /// <summary>
        /// Executes the backup work.
        /// </summary>
        /// <param name="backupJob"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        void Execute(BackupJob backupJob,string source, string destination);
    }
}
