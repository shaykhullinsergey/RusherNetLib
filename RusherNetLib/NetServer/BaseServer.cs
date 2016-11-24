using RusherNetLib.Core;
using System.Net.Sockets;

namespace RusherNetLib.NetServer {
    public abstract class BaseServer : IServer {
        public Socket Socket { get; protected set; }
        public bool IsRunning { get; protected set; }

        internal abstract void InvokeEvent(ServerType type, IConnection conn, IMessage msg);
        public abstract IServer AddHandler(ServerType type, ServerHandler handler);
        public abstract IServer Start(string host, int port);
        public abstract void Stop();
    }
}


