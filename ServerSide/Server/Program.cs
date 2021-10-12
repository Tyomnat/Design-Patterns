using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace Server
{
    class Program
    {
        private static List<int> playerIds = new List<int>();
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //public static Socket clientSocketGlobal = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Subject Subject = new Subject();
        private static List<Player> players = new List<Player>();
        private static Map map = new Map(512, 512);

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
            Thread mainThread = new Thread(() => SendMapUpdate());
            mainThread.Start();
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
            int id = GeneratePlayerId();
            Player newPlayer = new Player(id, username, clientSocket, map);
            players.Add(newPlayer);
            Subject.Register(newPlayer);
        }

        private static int GeneratePlayerId()
        {
            Random rnd = new Random();
            int randInt = 100;
            while (playerIds.Contains(randInt))
            {
                randInt = rnd.Next(101, 200);
            }
            playerIds.Add(randInt);

            return randInt;
        }


        private static void ReceiveMessage(Player player)
        {
            if (player.GetSocket().Connected)
            {
                byte[] bytes = Encoding.ASCII.GetBytes("id:" + player.Id.ToString());
                player.GetSocket().Send(bytes);

            }
            while (player.GetSocket().Connected)
            {
                player.HandlePlayerMoved(player.ReceiveMessage());

            }
        }

        private static void SendMapUpdate()
        {
            while (true)
            {
                //byte[] messageBuffer = new byte[1024];
                //player.GetSocket().Receive(messageBuffer);
                //string message = Encoding.ASCII.GetString(messageBuffer);
                //Console.WriteLine(player.GetUsername() + "_" + message);
                //Event gameEvent = new Event("player_moved", "sdsd");
                //Subject.Update(gameEvent);
                //Console.WriteLine(JsonSerializer.Serialize(map.Objects));
                //Console.ReadLine();

                //Function to run update to all map elements
                var t = Task.Factory.StartNew(() =>
                {

                    Task.Delay(200).Wait();
                });

                t.Wait();
                UpdateMap();
                Event gameEvent = new Event("map_updated", JsonConvert.SerializeObject(map));
                Subject.Update(gameEvent);

            }
        }

        private static void UpdateMap()
        {
            List<int> movedIds = new List<int>();

            for (int i = 0; i < map.Objects.GetLength(0); i++)
            {
                for (int j = 0; j < map.Objects[i].Length; j++)
                {
                    if (map.Objects[i][j].Id >= 100 && map.Objects[i][j].Id < 200 && !movedIds.Contains(map.Objects[i][j].Id)) // Players
                    {
                        movedIds.Add(map.Objects[i][j].Id);
                        HandlePlayerMovement(map.Objects[i][j].Id, i, j);

                    }
                    // AIs ?
                }
            }
        }

        private static void HandlePlayerMovement(int id, int x, int y)
        {
            Player player = players.Find(P => P.Id == id);
            string dir = player.GetDirection();
            int newX = x, newY = y;
            switch (dir)
            {
                case "Up":
                    newY--;
                    break;
                case "Right":
                    newX++;
                    break;
                case "Down":
                    newY++;
                    break;
                case "Left":
                    newX--;
                    break;
            }
            // Process overboundaries
            if (newX < 0 || newX > map.Objects.GetLength(0) - 1 || newY < 0 || newY > map.Objects[newX].Length - 1)
                return; // Player does not move, because move goes over boundaries

            // Process obstacles
            if (map.Objects[newX][newY].isSolid == true)
                return; // Player does not move, because target location is inpassable

            // Move to new location
            map.Objects[newX][newY] = new MapObject(map.Objects[newX][newY].X, map.Objects[newX][newY].Y, map.Objects[x][y].Id, true);
            // Remove old location
            map.Objects[x][y] = new MapObject(map.Objects[x][y].X, map.Objects[x][y].Y);
        }
    }
}
