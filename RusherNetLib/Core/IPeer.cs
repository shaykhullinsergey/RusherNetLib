using System.Net.Sockets;

namespace RusherNetLib.Core {
    public interface IPeer {
        Socket Socket { get; }
        bool IsRunning { get; }
    }
}





