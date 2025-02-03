using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupJobList
    {
        private List<BackupJob> _backupJobs;
        public BackupJobList()
        {
            _backupJobs = new List<BackupJob>();
        }
        public void Add(BackupJob backupJob)
        {
            _backupJobs.Add(backupJob);
        }
        public void Remove(int index)
        {
            _backupJobs.RemoveAt(index);
        }
        public void Update(int index, BackupJob backupJob)
        {
            _backupJobs[index] = backupJob;
        }
        public void Execute(int index)
        {
            _backupJobs[index].Execute();
        }
        public int Count()
        {
            return _backupJobs.Count;
        }
    }
}
