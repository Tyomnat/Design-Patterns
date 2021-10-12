using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Player : IObserver
    {
        public int Id { get; set; }
        private string username;
        private Socket socket;
        public int X { get; set; }
        public int Y { get; set; }

        public Player(int id, string username, Socket socket, Map map)
        {
            Id = id;
            this.username = username;
            this.socket = socket;
            Random rnd = new Random();
            X = rnd.Next(0, map.Objects.GetLength(0));
            Y = rnd.Next(0, map.Objects[X].Length);
            map.Objects[X][Y].Id = id;
        }

        public string GetUsername()
        {
            return this.username;
        }

        public Socket GetSocket()
        {
            return this.socket;
        }

        public void SendMessage(string message)
        {
            byte[] bytes = new byte[10000];
            bytes = Encoding.ASCII.GetBytes(message);
            //Console.WriteLine(Encoding.ASCII.GetString(bytes));
            //Console.ReadLine();
            this.socket.Send(bytes);
        }

        public string ReceiveMessage()
        {
            byte[] responseBuffer = new byte[1024];
            this.socket.Receive(responseBuffer);
            return Encoding.ASCII.GetString(responseBuffer);
        }

        public void Update(Event gameEvent)
        {
            if (gameEvent.Type == "player_moved")
            {
                HandlePlayerMoved(gameEvent.Data);
            }
            if (gameEvent.Type == "map_updated")
            {
                HandleMapUpdated(gameEvent.Data);
            }


        }

        private void HandlePlayerMoved(string data)
        {
            Console.WriteLine(username + " moved");
        }
        private void HandleMapUpdated(string data)
        {
            SendMessage("map:" + data);
        }
    }
}
