using RusherNetLib.Core;
using System.Net.Sockets;

namespace RusherNetLib.NetClient {
    public abstract class BaseClient : IClient {
        public Socket Socket { get; protected set; }
        public bool IsRunning { get; protected set; }
        public SocketError SocketError => socketError;
        protected SocketError socketError;
        protected byte[] buffer;
        
        internal abstract void InvokeEvent(ClientType type, IConnection conn, IMessage msg);
        public abstract IClient AddHandler(ClientType type, ClientHandler handler);
        public abstract IClient Connect(string host, int port);
        public abstract IMessage CreateMessage();
        public abstract void Disconnect();
        public abstract void Send(IMessage message);
    }
}
