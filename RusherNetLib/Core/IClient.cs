namespace RusherNetLib.Core
{
  public delegate void ClientHandler(IConnection conn, IMessage msg);
  public enum ClientType
  {
    Connected, Sended, Received, Disconnected,
  }
  public interface IClient : IConnection
  {
    IClient Connect(string host, int port);
    IClient AddHandler(ClientType type, ClientHandler handler);
  }
}
