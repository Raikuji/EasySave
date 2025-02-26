using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyRemote.Models;

namespace EasyRemote.ViewModels
{
    partial class MainWindowViewModel : ObservableObject
    {
        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

        public ICommand ConnectCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand StopCommand { get; }

        public MainWindowViewModel()
        {
            ConnectCommand = new RelayCommand(Connect);
            PauseCommand = new RelayCommand(PauseJob);
            PlayCommand = new RelayCommand(PlayJob);
            StopCommand = new RelayCommand(StopJob);

            ServerAdress = "127.0.0.1";
            PortNumber = 5000;
            BackupJobs = new ObservableCollection<BackupJob>();
           
        }

        public async void Connect()
        {
            Client.GetInstance().ConnectToServer(ServerAdress, PortNumber);
            Client.GetInstance().SendData("LIST");
            await ReceiveDataAsync();
        }

        public async Task ReceiveDataAsync()
        {
            try
            {
                while (true)
                {
                    string data = await Task.Run(() => Client.GetInstance().ReceiveDataAsync());
                    Console.WriteLine($"Data received from server : {data}"); 

                    if (!string.IsNullOrEmpty(data))
                    {
                        ParseAndUpdateBackupJobs(data);
                    }
                    else
                    {
                        Console.WriteLine("Connection closed by server.");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
        }

        private void ParseAndUpdateBackupJobs(string data)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                BackupJobs.Clear();
                string[] lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    var parts = line.Split(new[] { '|' }, StringSplitOptions.TrimEntries);
                    if (parts.Length == 3)
                    {
                        BackupJobs.Add(new BackupJob
                        {
                            Name = parts[0].Trim(),
                            Progress = int.Parse(parts[1].Replace("Progress:", "").Replace("%", "").Trim()),
                            Status = parts[2].Replace("Status:", "").Trim()
                        });
                    }
                }
            });
        }

        private void PauseJob()
        {
            if (SelectedJob != null)
            {
                Client.GetInstance().SendData($"PAUSE {BackupJobs.IndexOf(SelectedJob)}");
            }
        }

        private void PlayJob()
        {
            if (SelectedJob != null)
            {
                Client.GetInstance().SendData($"PLAY {BackupJobs.IndexOf(SelectedJob)}");
            }
        }

        private void StopJob()
        {
            if (SelectedJob != null)
            {
                Client.GetInstance().SendData($"STOP {BackupJobs.IndexOf(SelectedJob)}");
            }
        }

        public string ServerAdress { get; set; }
        public int PortNumber { get; set; }
        public ObservableCollection<BackupJob> BackupJobs { get; set; }
        public BackupJob SelectedJob { get; set; }
    }
}
