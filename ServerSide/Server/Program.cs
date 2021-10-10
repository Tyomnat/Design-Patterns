using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //public static Socket clientSocketGlobal = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private static List<Player> players = new List<Player>();

        static void Main(string[] args)
        {
            StartServer();
        }

        private static void StartServer()
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 50));
            serverSocket.Listen(10);
            Parallel.Invoke(
                () => serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null),
                () => serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null));
            Console.WriteLine("Server started. Waiting for 2 players");


            while (players.Count != 2)
            {

            }

            foreach (var pl in players)
            {
                Thread thread = new Thread(() => ReceiveMessage(pl));
                thread.Start();
            }
            while (!false)
            {

            }


            Console.WriteLine("2 players connected. Game starting");

            Console.ReadLine();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket clientSocket = serverSocket.EndAccept(AR);
            //clientSocketGlobal = clientSocket;
            Console.WriteLine("Connection received");

            byte[] responseBuffer = new byte[1024];
            clientSocket.Receive(responseBuffer);
            string username = (players.Count + 1).ToString();
            Console.WriteLine("player connected: " + username);
            players.Add(new Player(username, clientSocket));

        }

        private static void ReceiveMessage(Player player)
        {

            while (player.GetSocket().Connected)
            {
                byte[] messageBuffer = new byte[1024];
                player.GetSocket().Receive(messageBuffer);
                string message = Encoding.ASCII.GetString(messageBuffer);
                Console.WriteLine(player.GetUsername() + "_" + message);


            }
        }
    }
}
