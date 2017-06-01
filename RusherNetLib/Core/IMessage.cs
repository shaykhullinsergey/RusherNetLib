namespace RusherNetLib.Core
{
  public interface IMessage : IBuffer
  {
    int Sended { get; }
    int Received { get; }
    dynamic this[string name] { get; set; }
  }
}
