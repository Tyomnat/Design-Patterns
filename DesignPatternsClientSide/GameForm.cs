using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
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
        private static List<PlayerScore> PlayerScores = new List<PlayerScore>();
        private static bool dead = false;
        private static bool win = false;
        private static bool drawPaused = false;
        private static int winnerId = -1;
        private static List<string> chatContent = new List<string>();
        private static string currentMsg = "";



        public GameForm(Socket socket)
        {
            this.socket = socket;
            InitializeComponent(); // Default form initialization method
            //this.Size = new Size();
            this.ClientSize = new Size(544, 684);
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
                string messages = Encoding.ASCII.GetString(messageBuffer);
                string[] events = messages.Split("eventend");
                // Start of message will contain socket intended information
                // Map update socket
                foreach (var message in events)
                {
                    if (message.Contains("scores"))
                    {
                        string json = message.Split("_")[1].Split("\0")[0].Replace("scores", "");
                        dynamic d = JsonConvert.DeserializeObject(json);
                        List<PlayerScore> newScoreList = new List<PlayerScore>();
                        foreach (var playerScoreParsed in d)
                        {
                            PlayerScore playerScore = new PlayerScore(int.Parse((string)playerScoreParsed.PlayerId),
                                                                int.Parse((string)playerScoreParsed.Score));
                            newScoreList.Add(playerScore);

                        }
                        PlayerScores = newScoreList;
                        // message = message.Split("_")[1];
                    }

                    else if (message.Contains("map"))
                    {
                        // Don't ask
                        string json = message.Split("\0")[0].Replace("map", "").Replace("_", "");
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
                        player.Username = message.Split(":")[3];
                        player.Lives = 3;

                    }
                    else if (message.Contains("point_item_pickup"))
                    {
                        string soundType = message.Split("point_item_pickup")[1];
                        PlayPickupItemSound(soundType);
                    }
                    else if (message.Contains("takeDamage"))
                    {
                        string newHP = message.Split("takeDamage_")[1];
                        player.Lives = int.Parse(newHP);
                    }
                    else if (message.Contains("game_ended"))
                    {
                        if (!dead)
                        {
                            string parsedId = message.Split("game_ended")[1];
                            winnerId = int.Parse(parsedId);
                            win = true;
                        }
                    }
                    else if (message.Contains("draw_paused"))
                    {
                        drawPaused = !drawPaused;
                    }
                    else if (message.Contains("new_message"))
                    {
                        string parsed = message.Split("new_message")[1];
                        currentMsg = handleChat(parsed);

                    }
                    else if (message.Contains("username_updated"))
                    {
                        string parsed = message.Split("username_updated")[1];
                        player.Username = parsed;
                    }
                }

            }
        }

        private static void PlayPickupItemSound(string soundType)
        {
            string sound = "";
            switch (soundType)
            {
                case "poisonousSound":
                    sound = "poisonoussound";
                    break;
                case "tastySound":
                    sound = "tastysound";
                    break;
                default:
                    break;

            }
            if (sound != "")
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

                SoundPlayer simpleSound = new SoundPlayer(projectDirectory + "/sounds/" + sound + ".wav");
                simpleSound.Play();
            }
        }

        private string findPlayerScore(int Id)
        {
            return PlayerScores.Find(PS => PS.PlayerId == Id).Score.ToString();
        }

        /// <summary>
        /// Default Form paint method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            if (currentMsg != "")
            {
                chatArea.Text = currentMsg;
                currentMsg = "";
            }
            if (player.Lives > 0)
                lives.Text = "Lives: " + player.Lives.ToString();
            else
            {
                lives.Visible = false;
                gameOver.Visible = true;
                dead = true;
            }
            if (win && player.Id == winnerId)
            {
                Win.Visible = true;
            }
            else if (win)
            {
                gameOver.Visible = true;
            }
            pausedLabel.Visible = drawPaused;

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
                            e.Graphics.DrawString(findPlayerScore(player.Id), new Font("Arial", 10), Brushes.Black, new PointF(Map.Objects[i][j].X + 8, Map.Objects[i][j].Y + 8));
                        }
                        else if (Map.Objects[i][j].Id >= 100 && Map.Objects[i][j].Id < 200) // Other client players
                        {

                            e.Graphics.FillEllipse(Brushes.Red, Map.Objects[i][j].X, Map.Objects[i][j].Y, 32, 32);
                            e.Graphics.DrawString(findPlayerScore(Map.Objects[i][j].Id), new Font("Arial", 10), Brushes.Black, new PointF(Map.Objects[i][j].X + 8, Map.Objects[i][j].Y + 8));
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
                        else if (Map.Objects[i][j].Id == 500)//cherry
                        {
                            e.Graphics.FillEllipse(Brushes.Red, Map.Objects[i][j].X + 11, Map.Objects[i][j].Y + 11, 10, 10);
                        }
                        else if (Map.Objects[i][j].Id == 501)//apple
                        {
                            e.Graphics.FillEllipse(Brushes.GreenYellow, Map.Objects[i][j].X + 6, Map.Objects[i][j].Y + 6, 20, 20);
                        }
                        else if (Map.Objects[i][j].Id == 502)//rocket
                        {
                            DrawSprite(e, "rocket", Map.Objects[i][j].X, Map.Objects[i][j].Y);
                        }
                        else if (Map.Objects[i][j].Id == 503)//speedBoost
                        {
                            DrawSprite(e, "speedBoost", Map.Objects[i][j].X, Map.Objects[i][j].Y);
                        }
                        else if (Map.Objects[i][j].Id == 504)//shield
                        {
                            DrawSprite(e, "shield", Map.Objects[i][j].X, Map.Objects[i][j].Y);
                        }
                        else if (Map.Objects[i][j].Id >= 1000)//enemy shot
                        {
                            e.Graphics.FillEllipse(Brushes.Black, Map.Objects[i][j].X + 8, Map.Objects[i][j].Y + 8, 16, 16);
                        }
                    }
                }
            }
        }

        private static void DrawSprite(PaintEventArgs e, string spriteName, int x, int y)
        {
            string spriteFileName = "";
            switch (spriteName)
            {
                case "rocket":
                    spriteFileName = "rocket";
                    break;
                case "speedBoost":
                    spriteFileName = "speed";
                    break;
                case "shield":
                    spriteFileName = "shield";
                    break;
                default:
                    break;

            }
            if (spriteFileName != "")
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                e.Graphics.DrawImage(new Bitmap(projectDirectory + "/sprites/" + spriteFileName + ".png"), x, y, 32, 32);
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
            else if (e.KeyCode == Keys.M)
            {
                SendMessage("memento");
            }
            else if (e.KeyCode == Keys.P)
            {
                SendMessage("game_pause_changed");
            }
            else if (e.KeyCode == Keys.Enter)
            {
                chatInput.Enabled = true;
                chatInput.Focus();
            }
        }

        /// <summary>
        /// Default Form Load method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {
            player.Lives = 3;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //chatArea.Text += chatInput.Text + "\n";


                //chatArea.Text = handleChat(chatInput.Text);
                SendMessage("chat_" + player.Username + ": " + chatInput.Text);
                chatInput.Text = "";
                chatInput.Enabled = false;
            }
        }

        private static string handleChat(string newMsg)
        {
            chatContent.Add(newMsg);
            if (chatContent.Count > 5)
            {
                chatContent.RemoveAt(0);
            }

            string content = "";
            foreach (var item in chatContent)
            {
                content += item + "\n";
            }
            return content;
        }
    }
}
