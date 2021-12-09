using Newtonsoft.Json;
using Server.Enemies;
using Server.PointItems;
using Server.Powerups;
using Server.Proxy;
using Server.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Facade
    {
        const string TYPE_PLAYER = "player";
        const string TYPE_ENEMY_FAST = "enemy_fast";
        const string TYPE_ENEMY_SLOW = "enemy_slow";
        const string TYPE_ENEMY_NORMAL = "enemy_normal";
        const string TYPE_ENEMY_GHOST = "enemy_ghost";
        const string TYPE_ENEMY_SHOT = "enemy_shot";

        private static List<int> mapObjectsIds = new List<int>();
        private static List<Enemy> enemies = new List<Enemy>();
        private static readonly object EnemyListLock = new object();
        private static readonly object IdListLock = new object();
        private static PlayerController playerController = new PlayerController();
        private static List<int> PickupItemsIds = new List<int>() { 500, 501 };
        private static List<int> PowerupsIds = new List<int>() { 502, 503, 504 };
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Player> players = new List<Player>();
        private static List<PlayerScore> PlayerScores = new List<PlayerScore>();
        private static Subject Subject = new Subject();
        private static CommandProxy Proxy = new CommandProxy(new PlayerCommandReceiver());

        public void GenerateAllEnemies()
        {
            for (int i = 0; i < 1; i++)
            {
                Thread thread = new Thread(() =>
                {
                    Enemy enemy = new NormalEnemy(GenerateGameObjectId(TYPE_ENEMY_NORMAL), Map.GetInstance());
                    enemy.SetAlgorithm(new NormalAlgorithm());
                    AddToEnemyListThreadSafe(enemy);
                });
                thread.Start();

            }

            Enemy enemy = new SlowEnemy(GenerateGameObjectId(TYPE_ENEMY_SLOW), Map.GetInstance());
            enemy.SetAlgorithm(new SlowAlgorithm());
            AddToEnemyListThreadSafe(enemy);

            //for (int i = 0; i < 2; i++)
            //{
            //    Thread thread = new Thread((enemy) =>
            //    {
            //        Enemy enemyClone = (enemy as SlowEnemy).ShallowCopy();
            //        enemyClone.Id = GenerateGameObjectId(TYPE_ENEMY_SLOW);
            //        enemyClone.SetPosition(Map.GetInstance());
            //        enemyClone.SetAlgorithm(enemyClone.CloneMovementAlgorithm((enemy as SlowEnemy).GetAlgorithm()));
            //        AddToEnemyListThreadSafe(enemyClone);
            //    });
            //    thread.Start(enemy);
            //}
            for (int i = 0; i < 1; i++)
            {
                Thread thread = new Thread(() =>
                {
                    Enemy enemy = new FastEnemy(GenerateGameObjectId(TYPE_ENEMY_FAST), Map.GetInstance());
                    enemy.SetAlgorithm(new FastAlgorithm());
                    AddToEnemyListThreadSafe(enemy);
                });
                thread.Start();
            }
            for (int i = 0; i < 1; i++)
            {
                Thread thread = new Thread(() =>
                {
                    Enemy enemy = new SlowEnemy(GenerateGameObjectId(TYPE_ENEMY_GHOST), Map.GetInstance());
                    enemy.SetAlgorithm(new GhostAlgorithm());
                    AddToEnemyListThreadSafe(enemy);
                });
                thread.Start();
            }
        }

        public void AwaitPlayerConnections()
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

        }

        public void StartGame(Game game)
        {
            foreach (var pl in players)
            {
                Thread thread = new Thread(() => PlayerCommunication(pl, game));
                thread.Start();
            }
            game.PlayerCount = players.Count;

            // Start main Game thread which handles Map updates (all future game events as well probably)
            Thread mainThread = new Thread(() => SendMapUpdate(game));
            mainThread.Start();
            game.state.Handle();
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
            Player newPlayer = new Player(id, username, clientSocket, Map.GetInstance());
            PlayerScore playerScore = new PlayerScore(id);
            PlayerScores.Add(playerScore);
            players.Add(newPlayer);
            Subject.Register(newPlayer);
        }

        /// <summary>
        /// Main Game Threaded function
        /// </summary>
        private static void SendMapUpdate(Game game)
        {
            bool paused = false;
            Random r = new Random();
            // Kill while in the future on game end condition
            while (game.state.GetType().Name != typeof(GameEndState).Name)
            {

                game.MaxScore = PlayerScores.Max(s => s.Score);
                game.state.Handle();
                game.PlayerCount = players.Where(p => p.lives > 0).Count();

                // Server timer forcing movement updates every 200ms
                var t = Task.Factory.StartNew(() =>
                {
                    Task.Delay(200).Wait();
                });
                t.Wait();

                //Function to run update to all map elements
                if (game.state.GetType().Name != typeof(PausedState).Name)
                {

                    if (r.Next(1, 40) == 8)
                    {
                        (new Facade()).GenerateItems();
                    }
                    if (paused)
                    {
                        paused = false;
                        Subject.Update(new Event("draw_paused", ""));
                    }
                    UpdateMap();
                    var taskMapUpdate = Task.Factory.StartNew(() =>
                    {
                        Event gameEvent = new Event("map_updated", JsonConvert.SerializeObject(Map.GetInstance()));
                        Subject.Update(gameEvent);
                    });
                    taskMapUpdate.Wait();

                    Event playerScoresUpdatedEvent = new Event("scores_updated", JsonConvert.SerializeObject(PlayerScores));
                    Subject.Update(playerScoresUpdatedEvent);
                }
                else if (!paused)
                {
                    paused = true;
                    Subject.Update(new Event("draw_paused", ""));
                }
            }
            game.winnerId = PlayerScores.OrderByDescending(i => i.Score).First().PlayerId;
            Subject.Update(new Event("game_ended", game.winnerId.ToString()));
        }



        public void GenerateItems()
        {
            Random rnd = new Random();
            for (int i = 0; i < 4; i++)
            {

                Thread thread = new Thread((rnd) =>
                {
                    int rn = (rnd as Random).Next(1, 5);
                    Cherry cherry = new Cherry(Map.GetInstance());
                    SetPointItemValues(rn, cherry);

                    rn = (rnd as Random).Next(1, 5);
                    Apple apple = new Apple(Map.GetInstance());
                    SetPointItemValues(rn, apple);
                });
                thread.Start(rnd);
            }

        }

        public void GeneratePowerups()
        {
            new Rocket(Map.GetInstance());
            new Shield(Map.GetInstance());
            new SpeedBoost(Map.GetInstance());
        }

        public void SetPointItemValues(int randInt, PointItem pi)
        {
            Sound sound;
            if (randInt == 1)
            {
                sound = new PoisonousSound();
                pi.Amount = -pi.Amount;
            }
            else
            {
                sound = new TastySound();
            }

            pi.SetSound(sound);
        }


        public void AddToEnemyListThreadSafe(Enemy enemy)
        {
            lock (EnemyListLock)
            {
                enemies.Add(enemy);
            }
        }

        public static void AddIdToListThreadSafe(int id)
        {
            lock (IdListLock)
            {
                mapObjectsIds.Add(id);
            }
        }


        /// <summary>
        /// Generate an ID for players (100; 200)
        /// First player is always given ID 100.
        /// </summary>
        /// <returns>Player ID</returns>
        public static int GenerateGameObjectId(string Type)
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
                case TYPE_ENEMY_SHOT:
                    randInt = 1000;
                    lowerB = 1001;
                    upperB = 2000;
                    break;

            }
            while (mapObjectsIds.Contains(randInt))
            {
                randInt = rnd.Next(lowerB, upperB);
            }
            AddIdToListThreadSafe(randInt);
            return randInt;
        }



        public void PlayerCommunication(Player player, Game game)
        {
            if (player.GetSocket().Connected)
            {
                // Initial player ID send out to store in the client side
                byte[] bytes = Encoding.ASCII.GetBytes("id:" + player.Id.ToString() + ":username:" + player.GetUsername() + "eventend");
                player.GetSocket().Send(bytes);
            }
            while (player.GetSocket().Connected)
            {
                // Receive updated movement direction if any
                player.ReceiveMessage(Proxy, playerController, game, Subject);//execute action       
            }
        }


        /// <summary>
        /// Map update function
        /// </summary>
        private static void UpdateMap()
        {
            List<int> movedIds = new List<int>();

            // X loop
            for (int i = 0; i < Map.GetInstance().Objects.GetLength(0); i++)
            {
                // Y loop
                for (int j = 0; j < Map.GetInstance().Objects[i].Length; j++)
                {
                    // Players
                    if (Map.GetInstance().Objects[i][j].Id >= 100 && Map.GetInstance().Objects[i][j].Id < 200 && !movedIds.Contains(Map.GetInstance().Objects[i][j].Id))
                    {
                        // Making sure not to move the same ID more than once
                        movedIds.Add(Map.GetInstance().Objects[i][j].Id);
                        HandlePlayerMovement(Map.GetInstance().Objects[i][j].Id, i, j);
                    } // Enemies
                    else if (Map.GetInstance().Objects[i][j].Id >= 200 && Map.GetInstance().Objects[i][j].Id < 280 && !movedIds.Contains(Map.GetInstance().Objects[i][j].Id))
                    {
                        movedIds.Add(Map.GetInstance().Objects[i][j].Id);
                        HandleEnemyMovement(Map.GetInstance().Objects[i][j].Id, i, j);
                    } // Enemy shot
                    else if (Map.GetInstance().Objects[i][j].Id >= 1000)
                    {
                        movedIds.Add(Map.GetInstance().Objects[i][j].Id);
                        HandleEnemyShot((Map.GetInstance().Objects[i][j] as EnemyAttackFire), i, j);
                    }
                }
            }
        }

        public static void HandleEnemyShot(EnemyAttackFire shot, int x, int y)
        {
            int newX = 0, newY = 0;
            Map map = Map.GetInstance();
            switch (shot.direction)
            {
                case "Up":
                    newY = y + 1;
                    newX = x;
                    break;
                case "Down":
                    newY = y - 1;
                    newX = x;
                    break;
                case "Left":
                    newY = y;
                    newX = x - 1;
                    break;
                case "Right":
                    newY = y;
                    newX = x + 1;
                    break;
            }

            //Out of bounds
            if (
                newX < 0 ||
                newX > map.Objects.GetLength(0) - 1 ||
                newY < 0 ||
                newY > map.Objects[newX].Length - 1)
            {
                map.Objects[x][y] = new MapObject(map.Objects[x][y].X, map.Objects[x][y].Y);
            }
            else if (map.Objects[newX][newY].Id >= 100 && map.Objects[newX][newY].Id < 200)
            { //Player 
                Player player = players.Find(P => P.Id == map.Objects[newX][newY].Id);
                player.HandleDamage();
                map.Objects[x][y] = new MapObject(map.Objects[x][y].X, map.Objects[x][y].Y);
            }
            else if (map.Objects[newX][newY].Id > 0)
            { // Anything except players
                map.Objects[x][y] = new MapObject(map.Objects[x][y].X, map.Objects[x][y].Y);
            }
            else if (map.Objects[newX][newY].Id == 0)
            { // Empty
                Map.GetInstance().Objects[newX][newY] = new EnemyAttackFire(shot.direction, Map.GetInstance().Objects[newX][newY].X, Map.GetInstance().Objects[newX][newY].Y, shot.Id);

                Map.GetInstance().Objects[x][y] = new MapObject(Map.GetInstance().Objects[x][y].X, Map.GetInstance().Objects[x][y].Y);
            }
        }


        public static void HandleEnemyMovement(int id, int x, int y)
        {
            Enemy enemy = enemies.Find(E => E.Id == id);
            if (enemy != null)
            {
                int newX;
                int newY;
                enemy.State = enemy.executeAlgorithm(x, y, Map.GetInstance(), out newX, out newY);

                switch (enemy.State)
                {
                    case "moving":
                        Map.GetInstance().Objects[newX][newY] = new MapObject(Map.GetInstance().Objects[newX][newY].X, Map.GetInstance().Objects[newX][newY].Y, id, true);

                        if ((!PickupItemsIds.Contains(Map.GetInstance().Objects[x][y].Id) && !PowerupsIds.Contains(Map.GetInstance().Objects[x][y].Id)) &&
                            Map.GetInstance().Objects[x][y].Id != 1)
                        {
                            Map.GetInstance().Objects[x][y] = new MapObject(Map.GetInstance().Objects[x][y].X, Map.GetInstance().Objects[x][y].Y);
                        }
                        return;
                    case "attacking":
                        string dir = "";
                        if (enemy.GetAlgorithm().GetType() == typeof(GhostAlgorithm) || enemy.GetAlgorithm().GetType() == typeof(SlowAlgorithm))
                        {
                            enemy.Attack(x, y, players, new AdapterMeleeAttack(new AttackMelee()), out dir);
                        }
                        else if (enemy.GetAlgorithm().GetType() == typeof(NormalAlgorithm) || enemy.GetAlgorithm().GetType() == typeof(FastAlgorithm))
                        {
                            if (enemy.Attack(x, y, players, new AdapterRangeAttack(new AttackRange()), out dir))
                            {
                                int shotX = 0, shotY = 0;
                                switch (dir)
                                {
                                    case "Up":
                                        shotY = y + 1;
                                        shotX = x;
                                        break;
                                    case "Down":
                                        shotY = y - 1;
                                        shotX = x;
                                        break;
                                    case "Left":
                                        shotY = y;
                                        shotX = x - 1;
                                        break;
                                    case "Right":
                                        shotY = y;
                                        shotX = x + 1;
                                        break;
                                }
                                if (
                                    shotX < 0 ||
                                    shotX > Map.GetInstance().Objects.GetLength(0) - 1 ||
                                    shotY < 0 ||
                                    shotY > Map.GetInstance().Objects[shotX].Length - 1)
                                {
                                    return;
                                }
                                else
                                {
                                    Map.GetInstance().Objects[shotX][shotY] = new EnemyAttackFire(dir, Map.GetInstance().Objects[shotX][shotY].X, Map.GetInstance().Objects[shotX][shotY].Y, GenerateGameObjectId(TYPE_ENEMY_SHOT));
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                        return;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// Function to handle player movement
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="x">Current player X coordinate according to MapObjects array</param>
        /// <param name="y">Current player Y coordinate according to MapObjects array</param>
        public static void HandlePlayerMovement(int id, int x, int y)
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
            if (newX < 0 || newX > Map.GetInstance().Objects.GetLength(0) - 1 || newY < 0 || newY > Map.GetInstance().Objects[newX].Length - 1)
                return; // Player does not move, because move goes over boundaries

            // Process obstacles
            if (Map.GetInstance().Objects[newX][newY].isSolid == true)
                return; // Player does not move, because target location is impassable

            if (PickupItemsIds.Contains(Map.GetInstance().Objects[newX][newY].Id))
            {
                PlayerScore ps = PlayerScores.Find(PS => PS.PlayerId == player.Id);
                if (ps != null)
                {
                    PointItem pi = (Map.GetInstance().Objects[newX][newY] as PointItem);
                    if (pi != null)
                    {
                        ps.Score += pi.Amount;
                        player.SendMessage("point_item_pickup" + pi.Play() + "eventend");
                    }

                }
            }
            if (PowerupsIds.Contains(Map.GetInstance().Objects[newX][newY].Id))
            {
                Powerup powerup = (Map.GetInstance().Objects[newX][newY] as Powerup);
                if (powerup != null)
                {
                    player.SendMessage("powerup_pickup" + powerup.Type + "eventend");
                }
            }
            // Move player to the new location
            Map.GetInstance().Objects[newX][newY] = new MapObject(Map.GetInstance().Objects[newX][newY].X, Map.GetInstance().Objects[newX][newY].Y, Map.GetInstance().Objects[x][y].Id, true);
            player.Caretaker.Save();
            player.X = newX;
            player.Y = newY;
            //set player.X,Y to newX, newY
            // Remove old player location and make it an empty space (ID 0 by default)
            Map.GetInstance().Objects[x][y] = new MapObject(Map.GetInstance().Objects[x][y].X, Map.GetInstance().Objects[x][y].Y);
        }



    }
}
