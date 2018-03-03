using System;
using System.Net.Sockets;
using RusherNetLib.Core;

namespace RusherNetLib.NetClient
{
	public sealed class Client : BaseClient
	{
		private event ClientHandler OnConnected;
		private event ClientHandler OnSended;
		private event ClientHandler OnReceived;
		private event ClientHandler OnDisconnected;

		public Client()
		{
			buffer = new byte[2048];
			IsRunning = false;
		}
		public override IClient AddHandler(ClientType type, ClientHandler handler)
		{
			switch (type)
			{
				case ClientType.Connected:
					OnConnected += handler;
					break;
				case ClientType.Sended:
					OnSended += handler;
					break;
				case ClientType.Received:
					OnReceived += handler;
					break;
				case ClientType.Disconnected:
					OnDisconnected += handler;
					break;
			}
			return this;
		}
		public override IClient Connect(string host, int port)
		{
			Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Socket.BeginConnect(host, port, ConnectCallback, null);
			return this;
		}
		public override IMessage CreateMessage()
		{
			return new Message();
		}
		public override void Send(IMessage message)
		{
			byte[] data = message.GetBytes();
			Socket.BeginSend(data, 0, data.Length, SocketFlags.None, out socketError, SendCallback, null);
		}
		public override void Disconnect()
		{
			IsRunning = false;
			Socket.Close();
			Socket.Dispose();
		}
		internal override void InvokeEvent(ClientType type, IConnection conn, IMessage msg)
		{
			switch (type)
			{
				case ClientType.Connected:
					OnConnected?.Invoke(conn, msg);
					break;
				case ClientType.Sended:
					OnSended?.Invoke(conn, msg);
					break;
				case ClientType.Received:
					OnReceived?.Invoke(conn, msg);
					break;
				case ClientType.Disconnected:
					OnDisconnected?.Invoke(conn, msg);
					break;
			}
		}
		private void ConnectCallback(IAsyncResult ar)
		{
			Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, ReceiveCallback, null);
			switch (socketError)
			{
				case SocketError.Success:
					IsRunning = true;
					InvokeEvent(ClientType.Connected, this, default(Message));
					break;
				default:
					InvokeEvent(ClientType.Disconnected, this, default(Message));
					break;
			}
		}
		private void SendCallback(IAsyncResult ar)
		{
			int sended = Socket.EndSend(ar, out socketError);
			switch (socketError)
			{
				case SocketError.Success:
					InvokeEvent(ClientType.Sended, this, new Message(sended));
					break;
				default:
					InvokeEvent(ClientType.Sended, this, default(Message));
					break;
			}
		}
		private void ReceiveCallback(IAsyncResult ar)
		{
			if (!IsRunning)
			{
				InvokeEvent(ClientType.Disconnected, this, default(Message));
				return;
			}
			int recieved = Socket.EndReceive(ar, out socketError);
			switch (socketError)
			{
				case SocketError.Success:
					if (recieved > 0)
					{
						var data = new byte[recieved];
						Array.Copy(buffer, data, recieved);
						InvokeEvent(ClientType.Received, this, new Message(data));
					}
					Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, ReceiveCallback, null);
					break;
				default:
					InvokeEvent(ClientType.Disconnected, this, default(Message));
					break;
			}
		}
	}
}
