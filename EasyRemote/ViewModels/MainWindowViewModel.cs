using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyRemote.Models;

namespace EasyRemote.ViewModels
{
	partial class MainWindowViewModel : ObservableObject
	{
		public RemoteJobList remoteJobs { get; } = RemoteJobList.GetInstance();

		public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

		public ICommand ConnectCommand { get; }

		public MainWindowViewModel()
		{
			ConnectCommand = new RelayCommand(Connect);
			ServerAdress = "127.0.0.1";
			PortNumber = 6464;
			ListTest = new ObservableCollection<string>();
			OnPropertyChanged(nameof(ListTest));
		}

		public async void Connect()
		{
			OnPropertyChanged(nameof(ListTest));
			Client.GetInstance().ConnectToServer(ServerAdress, PortNumber);
			//Client.GetInstance().SendData("Hello Server");
			await ReceiveDataAsync();
		}

		public async Task ReceiveDataAsync()
		{
			while (true)
			{
				string data = await Task.Run(() => Client.GetInstance().ReceiveDataAsync());
				if (!string.IsNullOrEmpty(data))
				{
					ListTest.Add(data);
				}
			}
		}

		public string ServerAdress { get; set; }
		public int PortNumber { get; set; }
		public ObservableCollection<String> ListTest { get; set; }
	}
}
