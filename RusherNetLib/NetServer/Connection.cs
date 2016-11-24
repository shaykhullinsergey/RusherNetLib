using System;
using System.Net.Sockets;
using RusherNetLib.Core;

namespace RusherNetLib.NetServer {
    internal sealed class Connection : IConnection {
        public bool IsRunning { get; private set; }
        public Socket Socket { get; private set; }
        public SocketError SocketError => socketError;
        private SocketError socketError;
        private BaseServer server;
        private byte[] buffer;
        
        public Connection(Socket socket, Server server) {
            Socket = socket;
            this.server = server;
            buffer = new byte[2048];
            IsRunning = false;

            Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, ReceiveCallback, null);
            switch (socketError) {
                case SocketError.Success:
                    IsRunning = true;
                    server.InvokeEvent(ServerType.Accepted, this, default(Message));
                    break;
                default:
                    server.InvokeEvent(ServerType.Disconnected, this, default(Message));
                    break;
            }
        }
        public IMessage CreateMessage() {
            return new Message();
        }
        public void Send(IMessage message) {
            byte[] data = message.GetBytes();
            Socket.BeginSend(data, 0, data.Length, SocketFlags.None, out socketError, SendCallback, null);
        }
        public void Disconnect() {
            Socket.Close();
            Socket.Dispose();
        }
        private void SendCallback(IAsyncResult ar) {
            Socket.EndSend(ar, out socketError);
            switch (socketError) {
                case SocketError.Success:
                    server.InvokeEvent(ServerType.Sended, this, new Message());
                    break;
                default:
                    server.InvokeEvent(ServerType.Disconnected, this, default(Message));
                    break;
            }
        }
        private void ReceiveCallback(IAsyncResult ar) {
            int recieved = Socket.EndReceive(ar, out socketError);
            switch (socketError) {
                case SocketError.Success:
                    if (recieved > 0) {
                        var data = new byte[recieved];
                        Array.Copy(buffer, data, recieved);
                        server.InvokeEvent(ServerType.Received, this, new Message(data));
                    }
                    Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, ReceiveCallback, null);
                    break;
                default:
                    server.InvokeEvent(ServerType.Disconnected, this, default(Message));
                    break;
            }
        }
    }
}
