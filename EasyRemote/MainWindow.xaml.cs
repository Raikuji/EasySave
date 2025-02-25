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
                Dispatcher.Invoke(() => StatusText.Text = "Server connection error.");
                MessageBox.Show("Unable to connect to the server.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReceiveData()
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    if (stream == null) return; // Check that stream is not null

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Dispatcher.Invoke(() => StatusText.Text = message); // UI update
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => StatusText.Text = "Disconnected from the server.");
            }
        }

        private void SendCommand(string command)
        {
            if (stream == null)
            {
                MessageBox.Show("Connection to server lost. Relaunch client.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when sending order.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e) => SendCommand("PAUSE");
        private void Play_Click(object sender, RoutedEventArgs e) => SendCommand("PLAY");
        private void Stop_Click(object sender, RoutedEventArgs e) => SendCommand("STOP");
    }
}
