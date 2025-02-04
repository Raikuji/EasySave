using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
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
            if (_backupJobs.Count > 5)
            {
                throw new ArgumentException("Backup job list is full");
            }
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
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (BackupJob backupJob in _backupJobs)
            {
                stringBuilder.Append(_backupJobs.IndexOf(backupJob)+1 + ". ");
                stringBuilder.Append(backupJob.ToString());
                if (backupJob != _backupJobs.Last())
                {
                    stringBuilder.Append(Environment.NewLine);
                }
            }
            return stringBuilder.ToString();
        }
        public void SaveBackupJobs(string path)
        {
            File.WriteAllText(path, "[");
            foreach (BackupJob backupJob in _backupJobs)
            {
                File.AppendAllText(path, backupJob.ToJson() + Environment.NewLine);
                if (backupJob != _backupJobs.Last())
                {
                    File.AppendAllText(path, ",");
                }
            }
            File.AppendAllText(path, "]");
        }
        public void LoadBackupJobs(string path)
        {
            if (File.Exists(path))
            {
                _backupJobs.Clear();
                string jsonString = File.ReadAllText(path);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                foreach (JsonElement element in jsonDocument.RootElement.EnumerateArray())
                {
                    string? name = element.GetProperty("name").GetString();
                    string? source = element.GetProperty("source").GetString();
                    string? destination = element.GetProperty("destination").GetString();
                    int strategyId = element.GetProperty("strategyId").GetInt32();

                    if (name != null && source != null && destination != null)
                    {
                        Add(new BackupJob(name, source, destination, strategyId));
                    }
                    else
                    {
                        throw new ArgumentException("Invalid backup job");
                    }
                }
            }
        }
    }
}
