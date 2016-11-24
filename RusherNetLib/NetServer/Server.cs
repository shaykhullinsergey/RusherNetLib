using System;
using System.Net;
using System.Net.Sockets;
using RusherNetLib.Core;

namespace RusherNetLib.NetServer {
    public sealed class Server : BaseServer {
        private event ServerHandler OnStarted;
        private event ServerHandler OnAccepted;
        private event ServerHandler OnSended;
        private event ServerHandler OnRecieved;
        private event ServerHandler OnDisconnected;
        private event ServerHandler OnStopped;

        public Server() {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IsRunning = false;
        }
        public override IServer AddHandler(ServerType type, ServerHandler handler) {
            switch (type) {
                case ServerType.Started: OnStarted += handler;
                    break;
                case ServerType.Accepted: OnAccepted += handler;
                    break;
                case ServerType.Sended: OnSended += handler;
                    break;
                case ServerType.Received: OnRecieved += handler;
                    break;
                case ServerType.Disconnected: OnDisconnected += handler;
                    break;
                case ServerType.Stopped: OnStopped += handler;
                    break;
            }
            return this;
        }

        public override IServer Start(string host, int port) {
            Socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            Socket.Listen(10);
            IsRunning = true;
            Socket.BeginAccept(AcceptCallback, null);
            InvokeEvent(ServerType.Started, null, default(Message));
            return this;
        }

        public override void Stop() {
            IsRunning = false;
            Socket.Close();
            InvokeEvent(ServerType.Stopped, null, default(Message));
        }
        internal override void InvokeEvent(ServerType type, IConnection conn, IMessage msg) {
            switch (type) {
                case ServerType.Started: OnStarted?.Invoke(conn, msg);
                    break;
                case ServerType.Accepted: OnAccepted?.Invoke(conn, msg);
                    break;
                case ServerType.Sended:  OnSended?.Invoke(conn, msg);
                    break;
                case ServerType.Received: OnRecieved?.Invoke(conn, msg);
                    break;
                case ServerType.Disconnected: OnDisconnected?.Invoke(conn, msg);
                    break;
                case ServerType.Stopped: OnStopped?.Invoke(conn, msg);
                    break;
            }
        }
        private void AcceptCallback(IAsyncResult ar) {
            if (IsRunning) {
                var client = Socket.EndAccept(ar);
                Socket.BeginAccept(AcceptCallback, null);
                new Connection(client, this);
            }
        }
    }
}
