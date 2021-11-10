using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using Server.PointItems;
using Server.Powerups;

namespace Server
{
    class Program
    {
        const string TYPE_PLAYER = "player";

        private static Facade facade = new Facade();

        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Player> players = new List<Player>();
        private static List<PlayerScore> PlayerScores = new List<PlayerScore>();
        private static Subject Subject = new Subject();

                
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

            facade.GenerateAllEnemy();
            facade.GenerateItems();


            foreach (var pl in players)
            {
                Thread thread = new Thread(() => facade.PlayerCommunication(pl));
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
            int id = facade.GenerateGameObjectId(TYPE_PLAYER);
            Player newPlayer = new Player(id, username, clientSocket, Map.GetInstance());
            PlayerScore playerScore = new PlayerScore(id);
            PlayerScores.Add(playerScore);
            players.Add(newPlayer);
            Subject.Register(newPlayer);
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
                facade.UpdateMap(players, PlayerScores);
                var taskMapUpdate = Task.Factory.StartNew(() =>
                {
                    Event gameEvent = new Event("map_updated", JsonConvert.SerializeObject(Map.GetInstance()));
                    Subject.Update(gameEvent);
                });
                taskMapUpdate.Wait();

                Event playerScoresUpdatedEvent = new Event("scores_updated", JsonConvert.SerializeObject(PlayerScores));
                Subject.Update(playerScoresUpdatedEvent);

            }
        }





    }
}
