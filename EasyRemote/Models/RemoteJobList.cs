using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRemote.Models
{
	public class RemoteJobList : ObservableCollection<RemoteJob>
	{
		private static RemoteJobList? _instance;

		public static RemoteJobList GetInstance()
		{
			if (_instance == null)
			{
				_instance = new RemoteJobList();
			}
			return _instance;
		}
	}
}
