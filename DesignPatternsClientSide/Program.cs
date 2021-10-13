using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DesignPatternsClientSide
{
    static class Program
    {
        static void Main()
        {
            Socket serverSocket = ConnectToServer(50);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm(serverSocket));
        }

        static Socket ConnectToServer(int port)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            int attempts = 0;
            while (!serverSocket.Connected && attempts < 300)
            {
                try
                {
                    attempts++;
                    serverSocket.Connect(IPAddress.Loopback, port);

                    if (serverSocket.Connected)
                    {
                        byte[] commandBuffer = Encoding.ASCII.GetBytes("connect");
                        serverSocket.Send(commandBuffer);
                        break;
                    }
                }
                catch (SocketException)
                {
                }
            }

            return serverSocket;
        }
    }
}
