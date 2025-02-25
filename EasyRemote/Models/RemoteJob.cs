using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRemote.Models
{
	public class RemoteJob
	{
		public string Name { get; set; }
		public string Source { get; set; }
		public string Destination { get; set; }
		public string Type { get; set; }
		public bool IsRunning { get; set; }
		public bool IsPaused { get; set; }

		public RemoteJob(string name, string source, string destination, string type)
		{
			Name = name;
			Source = source;
			Destination = destination;
			Type = type;
			IsRunning = false;
			IsPaused = false;
		}

		public RemoteJob(string job)
		{
			string[] jobParts = job.Split(';');
			Name = jobParts[0];
			Source = jobParts[1];
			Destination = jobParts[2];
			Type = jobParts[3];
			IsRunning = false;
			IsPaused = false;
		}
	}
}
