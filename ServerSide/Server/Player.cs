using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Player
    {
        private string username;
        private Socket socket;

        public Player(string username, Socket socket)
        {
            this.username = username;
            this.socket = socket;
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
            this.socket.Send(Encoding.ASCII.GetBytes(message));
        }

        public string ReceiveMessage()
        {
            byte[] responseBuffer = new byte[1024];
            this.socket.Receive(responseBuffer);
            return Encoding.ASCII.GetString(responseBuffer);
        }
    }
}
