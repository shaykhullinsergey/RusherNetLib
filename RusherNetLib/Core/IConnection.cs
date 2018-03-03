using System.Net.Sockets;

namespace RusherNetLib.Core
{
	public interface IConnection : IPeer
	{
		SocketError SocketError { get; }
		IMessage CreateMessage();
		void Send(IMessage message);
		void Disconnect();
	}
}



