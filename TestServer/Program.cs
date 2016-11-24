using System;
using RusherNetLib.Core;
using RusherNetLib.NetServer;

namespace TestServer {
    class Program {
        static void Main(string[] args) {
            var server = new Server()
                .AddHandler(ServerType.Started, OnStarted)
                .AddHandler(ServerType.Accepted, OnAccepted)
                .AddHandler(ServerType.Sended, OnSended)
                .AddHandler(ServerType.Received, OnReceived)
                .AddHandler(ServerType.Disconnected, OnDisconnected)
                .AddHandler(ServerType.Stopped, OnStopped)
                .Start("127.0.0.12", 4000);
            Console.ReadLine();
            server.Stop();
            Console.ReadLine();
        }
        //Когда сервер стартовал
        private static void OnStarted(IConnection conn, IMessage msg) {
            Console.WriteLine("OnStarted");
        }
        //Когда сервер принял клиент
        private static void OnAccepted(IConnection conn, IMessage msg) {
            Console.WriteLine("OnAccepted");
        }
        //Когда сервер отослал данные
        private static void OnSended(IConnection conn, IMessage msg) {
            Console.WriteLine("OnSended");
        }
        //Когда сервер принял данные
        private static void OnReceived(IConnection conn, IMessage msg) {
            Console.WriteLine("OnReceived");
        }
        //Когда сервер отключил клиент
        private static void OnDisconnected(IConnection conn, IMessage msg) {
            Console.WriteLine("OnDisconnected");
        }
        //Когда сервер остановился
        private static void OnStopped(IConnection conn, IMessage msg) {
            Console.WriteLine("OnStopped");
        }
    }
}

