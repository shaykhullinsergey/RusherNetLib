using System;
using RusherNetLib.Core;
using RusherNetLib.NetClient;

namespace TestClient {
    class Program {
        static void Main(string[] args) {
            var client = new Client()
                .AddHandler(ClientType.Connected, OnConnected)
                .AddHandler(ClientType.Sended, OnSended)
                .AddHandler(ClientType.Received, OnReceived)
                .AddHandler(ClientType.Disconnected, OnDisconnected)
                .Connect("127.0.0.12", 4000);
            Console.ReadLine();
            client.Disconnect();
            Console.ReadLine();
        }
        //Когда клиент подключился
        static void OnConnected(IConnection conn, IMessage msg) {
            Console.WriteLine("OnConnected");
        }
        //Когда клиент отправил данные
        static void OnSended(IConnection conn, IMessage msg) {
            Console.WriteLine("OnSended");
        }
        //Когда клиент принял данные
        static void OnReceived(IConnection conn, IMessage msg) {
            Console.WriteLine("OnReceived");
        }
        //Когда клиент отключился
        static void OnDisconnected(IConnection conn, IMessage msg) {
            Console.WriteLine("OnDisconnected");
        }
    }
}
