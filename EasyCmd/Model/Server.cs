using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
	public class Server
	{
		private Socket _serverSocket;

		public Server(int port)
		{
			_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
			_serverSocket.Listen(10);
		}

		public async Task StartAsync()
		{
			while (true)
			{
				var client = await _serverSocket.AcceptAsync();
				_ = Task.Run(() => HandleClientAsync(client));
			}
		}

		private async Task HandleClientAsync(Socket client)
		{
			using (client)
			{
				var buffer = new byte[1024];
				var received = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
				var message = "Hello from the server!";
				var data = Encoding.UTF8.GetBytes(message);

				await client.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
			}
		}

		public void Stop()
		{
			_serverSocket.Close();
		}
	}
}