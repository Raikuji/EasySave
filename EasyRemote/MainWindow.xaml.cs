using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace EasySaveClient
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;

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
            catch (Exception ex)
            {
                StatusText.Text = "Erreur de connexion";
            }
        }

        private void ReceiveData()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Dispatcher.Invoke(() => StatusText.Text = message); // Mise à jour UI
            }
        }

        private void SendCommand(string command)
        {
            byte[] data = Encoding.UTF8.GetBytes(command);
            stream.Write(data, 0, data.Length);
        }

        private void Pause_Click(object sender, RoutedEventArgs e) => SendCommand("PAUSE");
        private void Play_Click(object sender, RoutedEventArgs e) => SendCommand("PLAY");
        private void Stop_Click(object sender, RoutedEventArgs e) => SendCommand("STOP");
    }
}
