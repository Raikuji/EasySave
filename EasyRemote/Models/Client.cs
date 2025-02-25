using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyRemote.Models
{
	public class Client
	{
		private Socket? _clientSocket;
		public Socket ClientSocket => _clientSocket;

		private static Client? _instance;

		public static Client GetInstance()
		{
			if (_instance == null)
			{
				_instance = new Client();
			}
			return _instance;
		}

		public void ConnectToServer(string ipAdress, int port)
		{
			_clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				_clientSocket.Connect(ipAdress, port);
			}
			catch
			{
				
			}
		}

		public async Task<string> ReceiveDataAsync()
		{
			if (_clientSocket != null)
			{
				try
				{
					byte[] buffer = new byte[1024];
					int received = await _clientSocket.ReceiveAsync(buffer, SocketFlags.None);
					if (received == 0)
					{
						return "Error";
					}
					return Encoding.UTF8.GetString(buffer, 0, received);
				}
				catch (Exception ex)
				{
					// Handle receive exception
					Console.WriteLine($"Receive error: {ex.Message}");
					return "Error";
				}
			}
			return "Error";
		}

		public void SendData(string data)
		{
			if (_clientSocket != null)
			{
				_clientSocket.Send(Encoding.UTF8.GetBytes(data));
			}
		}
	}
}
