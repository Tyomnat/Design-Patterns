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
        const string TYPE_PLAYER = "player";
        const string TYPE_ENEMY_FAST = "enemy_fast";
        const string TYPE_ENEMY_SLOW = "enemy_slow";
        const string TYPE_ENEMY_NORMAL = "enemy_normal";
        const string TYPE_ENEMY_GHOST = "enemy_ghost";


        private static List<int> mapObjectsIds = new List<int>();
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Subject Subject = new Subject();
        private static List<Player> players = new List<Player>();
        private static List<Enemy> enemies = new List<Enemy>();

        private static Map map = new Map(512, 512);

        static void Main(string[] args)
        {
            StartServer();
        }

        /// <summary>
        /// Main Server Loop
        /// </summary>
        private static void StartServer()
        {
            // Starts the server and awaits 2 client connections
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 50));
            serverSocket.Listen(10);
            Parallel.Invoke(
                () => serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null),
                () => serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null));
            Console.WriteLine("Server started. Waiting for 2 players");

            // Forced wait for clients to join in
            while (players.Count != 2)
            {
            }

            // Start Listening Thread for each player
            for (int i = 0; i < 1; i++)
            {
                Enemy enemy = new NormalEnemy(GenerateGameObjectId(TYPE_ENEMY_NORMAL), map);
                enemy.SetAlgorithm(new NormalAlgorithm());
                enemies.Add(enemy);
            }
            for (int i = 0; i < 1; i++)
            {
                Enemy enemy = new SlowEnemy(GenerateGameObjectId(TYPE_ENEMY_SLOW), map);
                enemy.SetAlgorithm(new SlowAlgorithm());
                enemies.Add(enemy);
            }
            for (int i = 0; i < 1; i++)
            {
                Enemy enemy = new FastEnemy(GenerateGameObjectId(TYPE_ENEMY_FAST), map);
                enemy.SetAlgorithm(new FastAlgorithm());
                enemies.Add(enemy);
            }
            for (int i = 0; i < 1; i++)
            {
                Enemy enemy = new SlowEnemy(GenerateGameObjectId(TYPE_ENEMY_GHOST), map);
                enemy.SetAlgorithm(new GhostAlgorithm());
                enemies.Add(enemy);
            }

            foreach (var pl in players)
            {
                Thread thread = new Thread(() => PlayerCommunication(pl));
                thread.Start();
            }

            // Start main Game thread which handles Map updates (all future game events as well probably)
            Thread mainThread = new Thread(() => SendMapUpdate());
            mainThread.Start();

            // Infinite loop for program not to shut down, eventually will have to be replaced
            // with game end condition
            while (!false)
            {
            }

            //Console.WriteLine("2 players connected. Game starting");
            //Console.ReadLine();
        }

        /// <summary>
        /// Receives client connection to the server;
        /// Creates new player object and saves it into global player list;
        /// Registers the player for Observer design pattern Subject object.
        /// </summary>
        /// <param name="AR"></param>
        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket clientSocket = serverSocket.EndAccept(AR);
            Console.WriteLine("Connection received");

            byte[] responseBuffer = new byte[1024];
            clientSocket.Receive(responseBuffer);
            string username = (players.Count + 1).ToString();
            Console.WriteLine("player connected: " + username);
            int id = GenerateGameObjectId(TYPE_PLAYER);
            Player newPlayer = new Player(id, username, clientSocket, map);
            players.Add(newPlayer);
            Subject.Register(newPlayer);
        }

        /// <summary>
        /// Generate an ID for players (100; 200)
        /// First player is always given ID 100.
        /// </summary>
        /// <returns>Player ID</returns>
        private static int GenerateGameObjectId(string Type)
        {
            Random rnd = new Random();
            int randInt = 0;
            int lowerB = 0;
            int upperB = 0;
            switch (Type)
            {
                case TYPE_PLAYER:
                    randInt = 100;
                    lowerB = 101;
                    upperB = 200;
                    break;
                case TYPE_ENEMY_SLOW:
                    randInt = 200;
                    lowerB = 201;
                    upperB = 220;
                    break;
                case TYPE_ENEMY_NORMAL:
                    randInt = 220;
                    lowerB = 221;
                    upperB = 240;
                    break;
                case TYPE_ENEMY_FAST:
                    randInt = 240;
                    lowerB = 241;
                    upperB = 260;
                    break;
                case TYPE_ENEMY_GHOST:
                    randInt = 260;
                    lowerB = 261;
                    upperB = 280;
                    break;

            }
            while (mapObjectsIds.Contains(randInt))
            {
                randInt = rnd.Next(lowerB, upperB);
            }
            mapObjectsIds.Add(randInt);
            return randInt;
        }


        private static void PlayerCommunication(Player player)
        {
            if (player.GetSocket().Connected)
            {
                // Initial player ID send out to store in the client side
                byte[] bytes = Encoding.ASCII.GetBytes("id:" + player.Id.ToString());
                player.GetSocket().Send(bytes);
            }
            while (player.GetSocket().Connected)
            {
                // Receive updated movement direction if any
                player.HandlePlayerMoved(player.ReceiveMessage());
            }
        }

        /// <summary>
        /// Main Game Threaded function
        /// </summary>
        private static void SendMapUpdate()
        {
            // Kill while in the future on game end condition
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

                // Server timer forcing movement updates every 200ms
                var t = Task.Factory.StartNew(() =>
                {
                    Task.Delay(200).Wait();
                });
                t.Wait();

                //Function to run update to all map elements
                UpdateMap();
                Event gameEvent = new Event("map_updated", JsonConvert.SerializeObject(map));
                Subject.Update(gameEvent);
            }
        }

        /// <summary>
        /// Map update function
        /// </summary>
        private static void UpdateMap()
        {
            List<int> movedIds = new List<int>();

            // X loop
            for (int i = 0; i < map.Objects.GetLength(0); i++)
            {
                // Y loop
                for (int j = 0; j < map.Objects[i].Length; j++)
                {
                    // Players
                    if (map.Objects[i][j].Id >= 100 && map.Objects[i][j].Id < 200 && !movedIds.Contains(map.Objects[i][j].Id))
                    {
                        // Making sure not to move the same ID more than once
                        movedIds.Add(map.Objects[i][j].Id);
                        HandlePlayerMovement(map.Objects[i][j].Id, i, j);
                    }
                    else if (map.Objects[i][j].Id >= 200 && map.Objects[i][j].Id < 280 && !movedIds.Contains(map.Objects[i][j].Id))
                    {
                        movedIds.Add(map.Objects[i][j].Id);
                        HandleEnemyMovement(map.Objects[i][j].Id, i, j);
                    }
                    // AIs ?
                }
            }
        }

        private static void HandleEnemyMovement(int id, int x, int y)
        {
            Enemy enemy = enemies.Find(E => E.Id == id);
            int newX;
            int newY;
            bool canMove = enemy.executeAlgorithm(x, y, map, out newX, out newY);
            if (canMove)
            {
                map.Objects[newX][newY] = new MapObject(map.Objects[newX][newY].X, map.Objects[newX][newY].Y, id, true);

                if (map.Objects[x][y].Id != 1)
                {
                    map.Objects[x][y] = new MapObject(map.Objects[x][y].X, map.Objects[x][y].Y);
                }

            }
            return;
        }

        /// <summary>
        /// Function to handle player movement
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="x">Current player X coordinate according to MapObjects array</param>
        /// <param name="y">Current player Y coordinate according to MapObjects array</param>
        private static void HandlePlayerMovement(int id, int x, int y)
        {
            // Get the appropriate player
            Player player = players.Find(P => P.Id == id);

            // Get intended movement direction
            string dir = player.GetDirection();

            // Intended move coordinate variables
            int newX = x, newY = y;

            // Calculate coordinates
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
                return; // Player does not move, because target location is impassable

            // Move player to the new location
            map.Objects[newX][newY] = new MapObject(map.Objects[newX][newY].X, map.Objects[newX][newY].Y, map.Objects[x][y].Id, true);

            // Remove old player location and make it an empty space (ID 0 by default)
            map.Objects[x][y] = new MapObject(map.Objects[x][y].X, map.Objects[x][y].Y);
        }
    }
}
