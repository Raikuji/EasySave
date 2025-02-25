using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace EasySaveClient
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private int selectedJobId = -1;
        private bool isCommandInProgress = false; // Flag pour gérer les commandes

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 5000);
                stream = client.GetStream();

                Thread receiveThread = new Thread(ReceiveData);
                receiveThread.Start();
            }
            catch (Exception)
            {
                Dispatcher.Invoke(() => StatusText.Text = "Erreur de connexion au serveur.");
                MessageBox.Show("Impossible de se connecter au serveur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReceiveData()
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    if (stream == null) return;

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Dispatcher.Invoke(() =>
                    {
                        if (message.StartsWith("Travaux:")) // Affichage des travaux
                        {
                            JobListBox.Items.Clear();
                            string[] jobs = message.Replace("Travaux:", "").Trim().Split('\n');
                            foreach (var job in jobs)
                            {
                                JobListBox.Items.Add(job);
                            }
                        }
                        else
                        {
                            StatusText.Text = message;
                        }
                    });
                }
            }
            catch (Exception)
            {
                Dispatcher.Invoke(() => StatusText.Text = "Déconnecté du serveur.");
            }
        }

        private void SendCommand(string command)
        {
            if (stream == null)
            {
                MessageBox.Show("Connexion perdue. Relancez le client.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors de l'envoi de la commande.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (selectedJobId >= 0 && !isCommandInProgress)
            {
                ExecuteCommand("PAUSE");
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (selectedJobId >= 0 && !isCommandInProgress)
            {
                ExecuteCommand("PLAY");
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (selectedJobId >= 0 && !isCommandInProgress)
            {
                ExecuteCommand("STOP");
            }
        }

        private void ExecuteCommand(string command)
        {
            // Désactiver tous les boutons pendant le traitement
            isCommandInProgress = true;
            ToggleUI(false);

            // Envoi de la commande au serveur
            SendCommand($"{command} {selectedJobId}");

            // Attendre un peu pour éviter les conflits et réactiver l'interface
            Thread.Sleep(500);

            // Réactivation de l'interface après avoir traité la commande
            isCommandInProgress = false;
            ToggleUI(true);
        }

        private void JobListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isCommandInProgress)
            {
                selectedJobId = JobListBox.SelectedIndex;

                // Activer/désactiver les boutons en fonction de la sélection
                bool isJobSelected = selectedJobId >= 0;
                PauseButton.IsEnabled = isJobSelected;
                PlayButton.IsEnabled = isJobSelected;
                StopButton.IsEnabled = isJobSelected;
            }
        }

        private void ToggleUI(bool enable)
        {
            // Désactive ou réactive les boutons
            PauseButton.IsEnabled = enable && selectedJobId >= 0;
            PlayButton.IsEnabled = enable && selectedJobId >= 0;
            StopButton.IsEnabled = enable && selectedJobId >= 0;

            // Désactive la sélection pendant qu'une commande est en cours
            JobListBox.IsHitTestVisible = enable;
        }
    }
}
