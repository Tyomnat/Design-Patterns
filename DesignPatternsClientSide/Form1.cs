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

        private int _x;
        private int _y;
        private Position _objPosition;
        private Socket socket;
        private static Player player = new Player();
        private static int[][] Map;
        //private Button btnAdd = new Button();
        //private List<PictureBox> walls = new List<PictureBox>();

        public Form1(Socket socket)
        {
            this.socket = socket;
            InitializeComponent();
            Random rnd = new Random();
            _x = rnd.Next(150, 300);
            _y = rnd.Next(0, 150);
            _objPosition = Position.Down;

            StartCommunication();
        }

        private void StartCommunication()
        {
            Thread thread = new Thread(() => WorkThreadFunction(socket));
            thread.Start();
        }

        static void Communicate(Socket serverSocket)// serverSocket in form. spawn thread for receiving (then push message to list). send only when needed, not in while                                                         //or open 2 sockets - sender/receiver?
        {
            while (serverSocket.Connected)
            {
                // Receive a message from the server
                byte[] messageBuffer = new byte[100000];
                serverSocket.Receive(messageBuffer);
                string message = Encoding.ASCII.GetString(messageBuffer);


                Console.WriteLine(message);
                // Process the server's message
                if (message.Contains("action needed"))
                {
                    //Console.Write("> ");
                    //string command = Console.ReadLine();
                    byte[] commandBuffer = Encoding.ASCII.GetBytes("sdfsdf");
                    serverSocket.Send(commandBuffer); // need try-catch, perhaps move to another 'SendCommand()' function
                }
                if (message.Contains("map"))
                {
                    string json = message.Split(":")[1].Split("\0")[0];
                    dynamic d = JsonConvert.DeserializeObject(json);
                    Map map = new Map();
                    foreach (var item in d)
                    {
                        MapObject mapObject = new MapObject(item.Id, item.X, item.Y);//create classes
                    }
                }
                if (message.Contains("id"))
                {
                    player.Id = int.Parse(message.Split(":")[1]);
                }
            }
        }

        static void WorkThreadFunction(Socket serverSocket)
        {

            Communicate(serverSocket);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.FillEllipse(Brushes.BlueViolet, _x, _y, 30, 30);

            if (Map != null)
            {
                for (int i = 0; i < Map.GetLength(0); i++)
                {
                    for (int j = 0; j < Map[i].Length; j++)
                    {
                        if (Map[i][j] == player.Id)
                        {
                            e.Graphics.FillEllipse(Brushes.BlueViolet, i, j, 30, 30);
                        }
                        else if (Map[i][j] != 0)
                        {
                            e.Graphics.FillRectangle(Brushes.Black, i, j, 30, 30);
                        }
                    }
                }
            }
            //e.Graphics.DrawImage(new Bitmap("mushroom.png"), _x, _y, 64, 64);
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                _objPosition = Position.Left;
            }
            else if (e.KeyCode == Keys.Right)
            {
                _objPosition = Position.Right;
            }
            else if (e.KeyCode == Keys.Up)
            {
                _objPosition = Position.Up;
            }
            else if (e.KeyCode == Keys.Down)
            {
                _objPosition = Position.Down;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //this.maximizebox = false;
            //this.minimizebox = false;
            //this.startposition = formstartposition.centerscreen;
            //this.btnadd.backcolor = color.gray;
            //this.btnadd.text = "add";
            //this.btnadd.location = new system.drawing.point(90, 25);
            //this.btnadd.size = new system.drawing.size(50, 25);

            ////picture.location = new system.drawing.point(1, 1);
            ////picture.show();
            //for (int i = 0; i < 10; i++)
            //{
            //    picturebox picture = new picturebox();
            //    picture.size = new system.drawing.size(100, 30);
            //    picture.backcolor = system.drawing.color.maroon;
            //    picture.location = new point(i * 10, i * 20);
            //    this.walls.add(picture);
            //    this.controls.add(picture);
            //}

            //this.controls.add(btnadd);
        }

        private void tmrMoving_Tick_1(object sender, EventArgs e)
        {

            if (_objPosition == Position.Right)
            {
                //_x += 10;
                sendMessage("right");
            }
            else if (_objPosition == Position.Left)
            {
                /// _x -= 10;
                sendMessage("left");
            }
            else if (_objPosition == Position.Up)
            {
                // _y -= 10;
                sendMessage("up");
            }
            else if (_objPosition == Position.Down)
            {
                //_y += 10;
                sendMessage("down");
            }

            Invalidate();
        }

        private void sendMessage(string message)
        {
            // Receive a message from the server

            // Process the server's message

            //Console.Write("> ");
            //string command = Console.ReadLine();
            byte[] commandBuffer = Encoding.ASCII.GetBytes(message);
            this.socket.Send(commandBuffer); // need try-catch, perhaps move to another 'SendCommand()' function

        }
    }
}
