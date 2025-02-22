using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Text.Json;
using EasyLog;
namespace EasyCmd.Model
{
	/// <summary>
	/// Represents a backup job.
	/// </summary>
	public class BackupJob
	{
		public string Name { get; set; }
		public string Source { get; set; }
		public string Destination { get; set; }
		public string Strategy { get; set; }
		private IBackupWorkStrategy BackupStrategy { get; }
		private WorkState _workState;
		private ManualResetEvent _pauseEvent;

		/// <summary>
		/// Constructor of the BackupJob class.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <param name="strategyId"></param>
		public BackupJob(string name, string source, string destination, int strategyId)
		{
			Name = name;
			Source = source;
			Destination = destination;
			Strategy = GetBackupStrategy(strategyId).GetType().Name;
			BackupStrategy = GetBackupStrategy(strategyId);
			_workState = new WorkState();
			_pauseEvent = new ManualResetEvent(true);
		}

		/// <summary>
		/// Returns the source of the backup job.
		/// </summary>
		/// <param name="strategyId"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public IBackupWorkStrategy GetBackupStrategy(int strategyId)
		{
			return strategyId switch
			{
				1 => new BackupWorkFull(),
				2 => new BackupWorkDifferential(),
				_ => throw new ArgumentException("Invalid strategy ID")
			};
		}

		/// <summary>
		/// Returns the strategy ID of the backup job.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int GetStrategyId()
		{
			return BackupStrategy switch
			{
				BackupWorkFull => 1,
				BackupWorkDifferential => 2,
				_ => throw new ArgumentException("Invalid strategy")
			};
		}

		/// <summary>
		/// Returns a string representation of the backup job.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{Name} {Source} {Destination} {BackupStrategy.GetType().Name}";
		}

		/// <summary>
		/// Returns a JSON representation of the backup job.
		/// </summary>
		/// <returns></returns>
		public string ToJson()
		{
			dynamic obj = new ExpandoObject();
			obj.name = Name;
			obj.source = Source;
			obj.destination = Destination;
			obj.strategyId = GetStrategyId();
			return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
		}

		public void Log(string source, string destination, long size, DateTime transfertStart, int encryptionTime)
		{
			BackupJobLog backupJobLog = new BackupJobLog(Name, source, destination, size, (DateTime.Now - transfertStart).TotalSeconds, encryptionTime, DateTime.Now);
			backupJobLog.Log();
		}

		/// <summary>
		/// Sets the total work state of the backup job.
		/// </summary>
		/// <param name="totalFiles"></param>
		/// <param name="totalSize"></param>
		public void SetTotalWorkState(int totalFiles, long totalSize)
		{
			_workState.SetTotal(totalFiles, totalSize);
			WorkStateNode.AddOrUpdateWorkStateNode(Name, Source, Destination, _workState);
		}

		/// <summary>
		/// Updates the work state of the backup job.
		/// </summary>
		/// <param name="files"></param>
		/// <param name="size"></param>
		/// <param name="currentFileSource"></param>
		/// <param name="currentFileDestination"></param>
		public void UpdateWorkState(int files, long size, string currentFileSource, string currentFileDestination)
		{
			_workState.UpdateRemaining(files, size);
			WorkStateNode.AddOrUpdateWorkStateNode(Name, currentFileSource, currentFileDestination, _workState);
		}

		/// <summary>
		/// Returns the work state of the backup job.
		/// </summary>
		/// <returns></returns>
		public WorkState GetWorkState()
		{
			return _workState;
		}

		/// <summary>
		/// Executes the backup job.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="DirectoryNotFoundException"></exception>
		public bool Execute()
		{
			bool success = true;
			try
			{
				if (string.IsNullOrWhiteSpace(Source) || string.IsNullOrWhiteSpace(Destination))
				{
					throw new ArgumentNullException();
				}
				if (!Directory.Exists(Source))
				{
					throw new DirectoryNotFoundException(Source);
				}
				if (!Directory.Exists(Destination))
				{
					Directory.CreateDirectory(Destination);
				}
				Thread thread = new Thread(() => BackupStrategy.Execute(this, Source, Destination));
				thread.Start();
			}
			catch (Exception)
			{
				success = false;
			}
			finally
			{
				UpdateWorkState(0, 0, "", "");
			}
			return success;
		}

		public void Pause()
		{
			_pauseEvent.WaitOne();
		}

		public void Resume()
		{
			_pauseEvent.Set();
		}

		public int EncryptFile(string filePath)
		{
			int encryptionTime = 0;
			string extension = filePath.Split(".").Last();
			if (Settings.GetInstance().FileExtensions.Contains(extension))
			{
				string cryptoSoftPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CryptoSoft", "CryptoSoft.exe");
				if (!File.Exists(cryptoSoftPath))
				{
					throw new FileNotFoundException($"CryptoSoft.exe not found at {cryptoSoftPath}");
				}
				Process process = new();
				process.StartInfo.FileName = cryptoSoftPath;
				process.StartInfo.Arguments = $"\"{filePath}\" {Settings.GetInstance().Key}";
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				process.WaitForExit();
				encryptionTime = process.ExitCode;
			}
			return encryptionTime;
		}
	}
}