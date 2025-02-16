using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EasyLog;

namespace EasyCmd.Model
{
	/// <summary>
	/// Class that represents the list of backup jobs.
	/// </summary>
	public class BackupJobList : ObservableCollection<BackupJob>
	{
		private List<BackupJob> _backupJobs;
		public List<BackupJob> BackupJobs { get => _backupJobs; }

		/// <summary>
		/// Constructor of the BackupJobList class.
		/// </summary>
		public BackupJobList()
		{
			_backupJobs = new List<BackupJob>();
		}

		public void AddJob(BackupJob backupJob)
		{
			Add(backupJob);
			BackupJobs.Add(backupJob);
		}

		/// <summary>
		/// Removes a backup job from the list.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveJobAt(int index)
		{
			WorkStateNode.RemoveWorkStateNode(BackupJobs[index].Name);
			BackupJobs.RemoveAt(index);
		}

		/// <summary>
		/// Removes a backup job from the list.
		/// </summary>
		/// <param name="job"></param>
		public void RemoveJob(BackupJob job)
		{
			WorkStateNode.RemoveWorkStateNode(job.Name);
			BackupJobs.Remove(job);
		}

		/// <summary>
		/// Updates a backup job in the list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="backupJob"></param>
		public void Update(int index, BackupJob backupJob)
		{
			_backupJobs[index] = backupJob;
		}
    
		/// <summary>
		/// Executes a backup job.
		/// </summary>
		/// <param name="index"></param>
		public bool Execute(int index)
		{
			return _backupJobs[index].Execute();
		}

		/// <summary>
		/// Returns a string of all backup jobs in the list.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (BackupJob backupJob in _backupJobs)
			{
				stringBuilder.Append(_backupJobs.IndexOf(backupJob) + 1 + ". ");
				stringBuilder.Append(backupJob.ToString());
				if (backupJob != _backupJobs.Last())
				{
					stringBuilder.Append(Environment.NewLine);
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Saves the backup jobs to a file.
		/// </summary>
		/// <param name="path"></param>
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

		/// <summary>
		/// Loads the backup jobs from a file.
		/// </summary>
		/// <param name="path"></param>
		/// <exception cref="ArgumentException"></exception>
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
						AddJob(new BackupJob(name, source, destination, strategyId));
					}
					else
					{
						throw new ArgumentException("Invalid backup job");
					}
				}
			}
		}
    internal bool ExecuteRange(int v1, int v2)
    {
        bool result = false;
        for (int i = v1; i <= v2; i++)
        {
            result = Execute(i);
        }
        return result;
    }
  }
}
