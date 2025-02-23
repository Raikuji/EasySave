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
		ManagementEventWatcher _processStartEvent;
		ManagementEventWatcher _processStopEvent;

		public ProcessWatcher()
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
						BackupJobList.GetInstance().StopAll();
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to check process: {ex.Message}");
			}
		}

		void ProcessStopEvent_EventArrived(object sender, EventArrivedEventArgs e) { }
	}
}

