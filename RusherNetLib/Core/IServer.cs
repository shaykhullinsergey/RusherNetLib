namespace RusherNetLib.Core {
    public delegate void ServerHandler(IConnection conn, IMessage msg);
    public enum ServerType {
        Started, Accepted, Sended, Received, Disconnected, Stopped,
    }
    public interface IServer : IPeer {
        IServer Start(string host, int port);
        IServer AddHandler(ServerType type, ServerHandler handler);
        void Stop();
    }
}




