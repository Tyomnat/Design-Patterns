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

        //private Button btnAdd = new Button();
        //private List<PictureBox> walls = new List<PictureBox>();

        public Form1(Socket socket)
        {
            this.socket = socket;
            InitializeComponent();

            _x = 50;
            _y = 50;
            _objPosition = Position.Down;

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.BlueViolet, _x, _y, 30, 30);
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
                _x += 10;
                sendMessage("right");
            }
            else if (_objPosition == Position.Left)
            {
                _x -= 10;
                sendMessage("left");
            }
            else if (_objPosition == Position.Up)
            {
                _y -= 10;
                sendMessage("up");
            }
            else if (_objPosition == Position.Down)
            {
                _y += 10;
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
