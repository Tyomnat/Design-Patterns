using Server.Proxy;
using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Player : IObserver, IOriginator<Player>
    {
        public int Id { get; set; }
        public string username;
        private bool isAlive;
        public int lives = 3;
        public String color;
        private Socket socket;
        public int X { get; set; }
        public int Y { get; set; }

        //private Player State;

        private enum Direction
        {
            Left, Right, Up, Down
        }
        private Direction direction;

        public Caretaker<Player> Caretaker;

        public Player(int id, string username, Socket socket, Map map)
        {
            Id = id;
            this.username = username;
            this.socket = socket;
            //Generate random starting point in map
            Random rnd = new Random();
            X = rnd.Next(0, map.Objects.GetLength(0));
            Y = rnd.Next(0, map.Objects[X].Length);
            map.Objects[X][Y].Id = id;
            map.Objects[X][Y].isSolid = true;

            this.Caretaker = new Caretaker<Player>(this);
        }

        private Player(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Player()
        {

        }

        public string GetUsername()
        {
            return this.username;
        }

        public Socket GetSocket()
        {
            return this.socket;
        }

        public string GetDirection()
        {
            switch (direction)
            {
                case Direction.Up:
                    return "Up";
                case Direction.Right:
                    return "Right";
                case Direction.Down:
                    return "Down";
                case Direction.Left:
                    return "Left";
            }
            return "";
        }

        public void SendMessage(string message)
        {
            byte[] bytes = new byte[10000];
            bytes = Encoding.ASCII.GetBytes(message);
            this.socket.Send(bytes);
        }

        public void ReceiveMessage(CommandReceiver commandReceiver, PlayerController playerController, Game game, Subject subject)
        {

            byte[] responseBuffer = new byte[1024];
            this.socket.Receive(responseBuffer);
            //return Encoding.ASCII.GetString(responseBuffer).Split("\0")[0];
            commandReceiver.ExecuteAction(Encoding.ASCII.GetString(responseBuffer).Split("\0")[0], playerController, this, game, subject);
        }

        public void Update(Event gameEvent)
        {
            /*if (gameEvent.Type == "player_moved")
            {
                HandlePlayerMoved(gameEvent.Data);
            }*/
            if (gameEvent.Type == "map_updated")
            {
                HandleMapUpdated(gameEvent.Data);
            }
            if (gameEvent.Type == "scores_updated")
            {
                SendMessage("scores_" + gameEvent.Data);
            }
            if (gameEvent.Type == "game_ended")
            {
                SendMessage("game_ended" + gameEvent.Data);
            }
            if (gameEvent.Type == "draw_paused")
            {
                SendMessage("draw_paused" + gameEvent.Data);
            }
            if (gameEvent.Type == "new_message")
            {
                SendMessage("new_message" + gameEvent.Data);
            }
        }

        public void Move(string data)
        {
            //Receive change of direction
            switch (data)
            {
                case "Up":
                    direction = Direction.Up;
                    break;
                case "Right":
                    direction = Direction.Right;
                    break;
                case "Down":
                    direction = Direction.Down;
                    break;
                case "Left":
                    direction = Direction.Left;
                    break;
            }
        }
        private void HandleMapUpdated(string data)
        {
            SendMessage("map_" + data);
        }

        public void HandleDamage()
        {
            lives--;
            Event takeDamage = new Event("takeDamage", this.lives.ToString());
            SendMessage(takeDamage.Type + "_" + takeDamage.Data);
            if (lives == 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (Map.GetInstance().Objects[i][j].Id == this.Id)
                            Map.GetInstance().Objects[i][j] = new MapObject(Map.GetInstance().Objects[i][j].X, Map.GetInstance().Objects[i][j].Y);
                    }
                }
            }
        }

        public Memento<Player> CreateMemento()
        {
            // Create memento and set state to current state.
            var memento = new Memento<Player>(new Player(X, Y));

            return memento;
        }

        public void SetMemento(Memento<Player> memento)
        {

            Map.GetInstance().Objects[X][Y] = new MapObject(Map.GetInstance().Objects[X][Y].X, Map.GetInstance().Objects[X][Y].Y);

            this.X = memento.State.X;
            this.Y = memento.State.Y;
            Map.GetInstance().Objects[X][Y].Id = Id;
            Map.GetInstance().Objects[X][Y].isSolid = true;

        }
    }
}
