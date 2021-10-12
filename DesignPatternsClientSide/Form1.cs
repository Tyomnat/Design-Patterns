using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DesignPatternsClientSide
{
    public partial class Form1 : Form
    {
        enum Position
        {
            Left, Right, Up, Down
        }

        private Socket socket;
        private static Player player = new Player();
        private static Map Map;

        public Form1(Socket socket)
        {
            this.socket = socket;
            InitializeComponent();
            DoubleBuffered = true;
            StartCommunication(this);
        }

        private void StartCommunication(Form1 frm)
        {
            Thread thread = new Thread(() => WorkThreadFunction(socket, frm));
            thread.Start();
        }

        static void Communicate(Socket serverSocket, Form1 frm)
        {
            while (serverSocket.Connected)
            {
                byte[] messageBuffer = new byte[100000];
                serverSocket.Receive(messageBuffer);
                string message = Encoding.ASCII.GetString(messageBuffer);

                if (message.Contains("map"))
                {
                    string json = message.Split("_")[1].Split("\0")[0].Replace("map", "");
                    dynamic d = JsonConvert.DeserializeObject(json);

                    Map map = new Map(int.Parse((string)d.Width), int.Parse((string)d.Height));
                    int j = 0;
                    foreach (var mapObjectsArr in d.Objects)
                    {
                        MapObject[] newMapObjectsArr = new MapObject[map.Height / 32];
                        int i = 0;
                        foreach (var mapObjectParsed in mapObjectsArr)
                        {
                            MapObject mapObject = new MapObject(int.Parse((string)mapObjectParsed.X), int.Parse((string)mapObjectParsed.Y), int.Parse((string)mapObjectParsed.Id));
                            newMapObjectsArr[i] = mapObject;
                            i++;

                        }
                        map.Objects[j] = newMapObjectsArr;
                        j++;
                    }
                    // Assign local map to Global map
                    Map = map;
                }
                else if (message.Contains("id"))
                {
                    player.Id = int.Parse(message.Split(":")[1]);
                }
            }
        }

        static void WorkThreadFunction(Socket serverSocket, Form1 frm)
        {
            Communicate(serverSocket, frm);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (Map != null)
            {
                for (int i = 0; i < Map.Objects.GetLength(0); i++)
                {
                    for (int j = 0; j < Map.Objects[i].Length; j++)
                    {
                        if (Map.Objects[i][j].Id == player.Id) // Current client player
                        {
                            e.Graphics.FillEllipse(Brushes.Green, Map.Objects[i][j].X, Map.Objects[i][j].Y, 32, 32);
                        }
                        else if (Map.Objects[i][j].Id >= 100 && Map.Objects[i][j].Id < 200) // Other client players
                        {
                            e.Graphics.FillEllipse(Brushes.Red, Map.Objects[i][j].X, Map.Objects[i][j].Y, 32, 32);
                        }
                        else if (Map.Objects[i][j].Id == 1) // Wall
                        {
                            e.Graphics.FillRectangle(Brushes.Black, Map.Objects[i][j].X, Map.Objects[i][j].Y, 32, 32);
                        }
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Send direction change on keydown is more optimal
            if (e.KeyCode == Keys.Left)
            {
                sendMessage("Left");
            }
            else if (e.KeyCode == Keys.Right)
            {
                sendMessage("Right");
            }
            else if (e.KeyCode == Keys.Up)
            {
                sendMessage("Up");

            }
            else if (e.KeyCode == Keys.Down)
            {
                sendMessage("Down");

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tmrMoving_Tick_1(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void sendMessage(string message)
        {
            byte[] commandBuffer = Encoding.ASCII.GetBytes(message);
            this.socket.Send(commandBuffer);

        }
    }
}
