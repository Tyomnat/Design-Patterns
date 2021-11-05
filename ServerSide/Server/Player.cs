using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Player : IObserver
    {
        public int Id { get; set; }
        private string username;
        private bool isAlive;
        private int lives;
        public String color;
        private Socket socket;
        public int X { get; set; }
        public int Y { get; set; }
        private enum Direction
        {
            Left, Right, Up, Down
        }
        private Direction direction;

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

        public string ReceiveMessage()
        {
            byte[] responseBuffer = new byte[1024];
            this.socket.Receive(responseBuffer);
            return Encoding.ASCII.GetString(responseBuffer).Split("\0")[0];
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
                SendMessage("scores_" + gameEvent.Data + "eventend");
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
            SendMessage("map_" + data + "eventend");
        }
    }
}
