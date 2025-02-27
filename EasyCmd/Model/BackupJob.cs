﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Text.Json;
using System.Threading;
using EasyLog;

namespace EasyCmd.Model
{
	/// <summary>
	/// Represents a backup job.
	/// </summary>
	public class BackupJob : INotifyPropertyChanged
	{
		public string Name { get; set; }
		public string Source { get; set; }
		public string Destination { get; set; }
		public string Strategy { get; set; }
		private IBackupWorkStrategy BackupStrategy { get; }
		private WorkState _workState;
		public bool IsRunning { get; set; }
		public bool IsPaused { get; set; }

		private double _progressValue;
		public double ProgressValue
		{
			get => _progressValue;
			set
			{
				if (_progressValue != value)
				{
					_progressValue = value;
					OnPropertyChanged(nameof(ProgressValue));
				}
			}
		}

		public Thread? BackupThread;
		public ManualResetEventSlim PauseEvent;

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
			IsRunning = false;
			PauseEvent = new ManualResetEventSlim(true);
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
			ProgressValue = Progress();
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

		public double Progress()
		{
			if (_workState.GetTotalSize() > 0)
				return (double)(100 - ((_workState.GetRemainingSize() * 100) / _workState.GetTotalSize()));
			return 0;
		}

		public void Stop()
		{
			if (IsPaused)
			{
				Resume();
			}
			IsRunning = false;
		}

		/// <summary>
		/// Executes the backup job.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="DirectoryNotFoundException"></exception>
		public bool Execute()
		{
			bool success = false;
			IsRunning = true;
			foreach (string process in Settings.GetInstance().LockProcesses)
			{
				if (Process.GetProcessesByName(process).Length > 0)
				{
					IsRunning = false;
				}
			}
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

				BackupThread = new Thread(() =>
				{
					try
					{
						BackupStrategy.Execute(this, Source, Destination);
					}
					catch (Exception)
					{
						IsRunning = false;
					}
					finally
					{
						UpdateWorkState(0, 0, "", "");
						IsRunning = false;
					}
				});
				BackupThread.Start();
				success = true;
			}
			catch (Exception)
			{
				IsRunning = false;
			}
			return success;
		}

		public static int EncryptFile(string filePath)
		{
			Mutex mutex = new(true, "CryptoSoftMutex");
			int encryptionTime = 0;
			string extension = Path.GetExtension(filePath).ToLower();
			if (Settings.GetInstance().FilesToEncrypt.Contains(extension))
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
				mutex.WaitOne();
				process.Start();
				process.WaitForExit();
				mutex.ReleaseMutex();
				encryptionTime = process.ExitCode;
			}
			return encryptionTime;
		}

		public void Pause()
		{
			IsPaused = true;
			PauseEvent.Reset();
		}

		public void Resume()
		{
			IsPaused = false;
			PauseEvent.Set();
		}

		public static void CopyBigFile(string filePath, string destFilePath)
		{
			using (Mutex mutex = new Mutex(false, "Global\\BigFileCopyMutex"))
			{
				try
				{
					mutex.WaitOne();
					{
						File.Copy(filePath, destFilePath, true);
					}
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}
		}
	}
}