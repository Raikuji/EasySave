using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
	public class ProcessWatcher
	{
		private static ProcessWatcher _instance;

		ManagementEventWatcher _processStartEvent;
		ManagementEventWatcher _processStopEvent;
		public event Action<bool>? OnProcessStateChanged;

		public static ProcessWatcher GetInstance() 
		{
			if (_instance == null)
			{
				_instance = new ProcessWatcher();
			}
			return _instance;
		}
		private ProcessWatcher()
		{
			try
			{
				_processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
				_processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");

				_processStartEvent.EventArrived += new EventArrivedEventHandler(ProcessStartEvent_EventArrived);
				_processStartEvent.Start();
				_processStopEvent.EventArrived += new EventArrivedEventHandler(ProcessStopEvent_EventArrived);
				_processStopEvent.Start();

				Console.WriteLine("ProcessWatcher started successfully.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to start ProcessWatcher: {ex.Message}");
			}
		}

		void ProcessStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
		{
			string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
			int processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
			try
			{
				Process p = Process.GetProcessById(processID);
				foreach (string process in Settings.GetInstance().LockProcesses)
				{
					if (process.Equals(p.ProcessName, StringComparison.OrdinalIgnoreCase))
					{
						BackupJobList.GetInstance().PauseAll();
						OnProcessStateChanged?.Invoke(true);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to check process: {ex.Message}");
			}
		}

		void ProcessStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
		{
			string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
			int processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
			try
			{
				processName = processName.Split(".")[0];
				foreach (string process in Settings.GetInstance().LockProcesses)
				{
					if (process.Contains(processName, StringComparison.OrdinalIgnoreCase))
					{
						BackupJobList.GetInstance().ResumeAll();
						OnProcessStateChanged?.Invoke(false);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to check process: {ex.Message}");
			}
		}
	}
}

