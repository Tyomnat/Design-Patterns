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
    /// <summary>
    /// Main game form, that displays the actual gameplay
    /// </summary>
    public partial class GameForm : Form
    {
        private Socket socket;
        private static Player player = new Player();
        private static Map Map;

        public GameForm(Socket socket)
        {
            this.socket = socket;
            InitializeComponent(); // Default form initialization method
            //this.Size = new Size();
            this.ClientSize = new Size(544, 544);
            DoubleBuffered = true; // Making form not flicker due to rapid repainting
            StartCommunication(); // Start communication thread
        }

        private void StartCommunication()
        {
            Thread thread = new Thread(() => Communicate(socket));
            thread.Start();
        }

        static void Communicate(Socket serverSocket)
        {
            while (serverSocket.Connected)
            {
                // Receive message
                byte[] messageBuffer = new byte[100000];
                serverSocket.Receive(messageBuffer);
                string message = Encoding.ASCII.GetString(messageBuffer);

                // Start of message will contain socket intended information
                // Map update socket
                if (message.Contains("map"))
                {
                    // Don't ask
                    string json = message.Split("_")[1].Split("\0")[0].Replace("map", "");
                    dynamic d = JsonConvert.DeserializeObject(json);

                    // New map object
                    Map map = new Map(int.Parse((string)d.Width), int.Parse((string)d.Height));
                    int j = 0;
                    foreach (var mapObjectsArr in d.Objects) // X loop
                    {
                        MapObject[] newMapObjectsArr = new MapObject[map.Height / 32];
                        int i = 0;
                        foreach (var mapObjectParsed in mapObjectsArr) // Y loop
                        {
                            MapObject mapObject = new MapObject(int.Parse((string)mapObjectParsed.X),
                                                                int.Parse((string)mapObjectParsed.Y),
                                                                int.Parse((string)mapObjectParsed.Id));
                            newMapObjectsArr[i] = mapObject;
                            i++;
                        }
                        map.Objects[j] = newMapObjectsArr;
                        j++;
                    }
                    // Assign local map to Global map
                    Map = map;
                }
                // Receiving server assigned current client player ID
                else if (message.Contains("id"))
                {
                    player.Id = int.Parse(message.Split(":")[1]);
                }
            }
        }

        /// <summary>
        /// Default Form paint method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            // If we have the map
            if (Map != null)
            {
                for (int i = 0; i < Map.Objects.GetLength(0); i++) // X loop
                {
                    for (int j = 0; j < Map.Objects[i].Length; j++) // Y loop
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
                        else if (Map.Objects[i][j].Id >= 220 && Map.Objects[i][j].Id < 240)
                        {
                            e.Graphics.FillRectangle(Brushes.PaleVioletRed, Map.Objects[i][j].X + 4, Map.Objects[i][j].Y + 4, 24, 24);
                        }
                        else if (Map.Objects[i][j].Id >= 200 && Map.Objects[i][j].Id < 220)
                        {
                            e.Graphics.FillRectangle(Brushes.Yellow, Map.Objects[i][j].X + 4, Map.Objects[i][j].Y + 4, 24, 24);
                        }
                        else if (Map.Objects[i][j].Id >= 240 && Map.Objects[i][j].Id < 260)
                        {
                            e.Graphics.FillRectangle(Brushes.Blue, Map.Objects[i][j].X + 4, Map.Objects[i][j].Y + 4, 24, 24);
                        }
                        else if (Map.Objects[i][j].Id >= 260 && Map.Objects[i][j].Id < 280)
                        {
                            e.Graphics.FillRectangle(Brushes.DarkRed, Map.Objects[i][j].X + 4, Map.Objects[i][j].Y + 4, 24, 24);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Default form keydown method
        /// We send intended movement direction to the server once a keydown is registered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                SendMessage("Left");
            }
            else if (e.KeyCode == Keys.Right)
            {
                SendMessage("Right");
            }
            else if (e.KeyCode == Keys.Up)
            {
                SendMessage("Up");
            }
            else if (e.KeyCode == Keys.Down)
            {
                SendMessage("Down");
            }
            else if (e.KeyCode == Keys.Z)
            {
                SendMessage("Undo");
            }
        }

        /// <summary>
        /// Default Form Load method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Timer calling form redraw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrMoving_Tick_1(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Send information to server
        /// </summary>
        /// <param name="message"></param>
        private void SendMessage(string message)
        {
            byte[] commandBuffer = Encoding.ASCII.GetBytes(message);
            this.socket.Send(commandBuffer);
        }
    }
}
