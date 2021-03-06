using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class Form1 : Form
    {
        //объявление объектов и необходимых параметров
        PictureBox[] cloud;
        PictureBox ship;
        PictureBox bullet;
        PictureBox submarine;
        int backgroundspeed;
        int shipspeed;
        Random rnd;
        
        public Form1()
        {
            InitializeComponent();
        }
        //Основной таймер игры
        private void gmtmr_Tick(object sender, EventArgs e)
        {
            //перемещение облаков
            for (int i=0; i< cloud.Length; i++)
            {
                cloud[i].Left += backgroundspeed;
                if(cloud[i].Left >= 1280)
                {
                    cloud[i].Left = cloud[i].Height;
                }
            }
            for (int i = cloud.Length; i<cloud.Length; i++)
            {
                cloud[i].Left += backgroundspeed - 10;
                if (cloud[i].Left >= 1280)
                {
                    cloud[i].Left = cloud[i].Left;
                }
            }
            //перемещение корабля
            ship.Left += shipspeed;
            //Условия проигрыша
            if (ship.Left >= 850)
            {
                gmtmr.Enabled = false;
                Blttmr.Enabled = false;
                MessageBox.Show("You lose!","Game over!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Application.Exit();
            }
            //Условия попадания пули в корабль
            bool condition1 = (ship.Left < bullet.Left + bullet.Width / 2) && (ship.Left + ship.Width > bullet.Left + bullet.Width / 2);
            bool condition2 = (ship.Location.Y <= bullet.Location.Y + bullet.Height / 2) && (ship.Location.Y + ship.Height >= bullet.Location.Y + bullet.Height / 2);
            //Победа
            if (condition1 && condition2)
            {
                gmtmr.Enabled = false;
                Blttmr.Enabled = false;
                ship.Load("../explosion.png");
                MessageBox.Show("You win!", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //без комментариев
            backgroundspeed = 2;
            cloud = new PictureBox[20];
            ship = new PictureBox();
            bullet = new PictureBox();
            submarine = new PictureBox();
            rnd = new Random();

            //создание объектов
            shipspeed = 4;
            ship.Size = new Size(100, 30);
            ship.Location = new Point(-100, rnd.Next(220, 250));
            ship.BackColor = Color.Transparent;
            ship.SizeMode = PictureBoxSizeMode.StretchImage;
            ship.Load("../ufo.png");
            this.Controls.Add(ship);

            submarine.Size = new Size(100, 50);
            submarine.Location = new Point(350, 370);
            submarine.BackColor = Color.Transparent;
            submarine.SizeMode = PictureBoxSizeMode.StretchImage;
            submarine.Load("../submarine.png");
            this.Controls.Add(submarine);

            bullet.Size = new Size(15, 15);
            bullet.Location = new Point(400, 380);
            bullet.BackColor = Color.Transparent;
            bullet.SizeMode = PictureBoxSizeMode.StretchImage;
            bullet.Load("../bullet.png");
            this.Controls.Add(bullet);

            //для нормальной прорисовки
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            
            //облачка
            for (int i = 0; i < cloud.Length; i++)
            {
                cloud[i] = new PictureBox();
                cloud[i].BorderStyle = BorderStyle.None;
                cloud[i].Location = new Point(rnd.Next(-1000, 1280), rnd.Next(0, 100));
                if (i % 2 == 1)
                {
                    cloud[i].Size = new Size(rnd.Next(100, 225), rnd.Next(30, 70));
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 255, 255, 255);
                }
                else
                {
                    cloud[i].Size = new Size(rnd.Next(100, 225), rnd.Next(30, 70));
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 205, 255, 255);
                }
                this.Controls.Add(cloud[i]);
            }
        }

        //таймер пули (запускается по нажатию), тик совпадает с основным таймером
        private void Blttmr_Tick(object sender, EventArgs e)
        {
            int bulletspeed = 6;
            //перемещение пули
            if (ship.Location.X + ship.Width / 2 < bullet.Location.X)
            {
                bullet.Location = new Point(bullet.Location.X - bulletspeed, bullet.Location.Y);
                bullet.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            if (ship.Location.X + ship.Width / 2 > bullet.Location.X)
            {
                bullet.Location = new Point(bullet.Location.X + bulletspeed, bullet.Location.Y);
            }
            if (ship.Location.Y + ship.Height/2 < bullet.Location.Y)
            {
                bullet.Location = new Point(bullet.Location.X, bullet.Location.Y - bulletspeed);
            }
            if (ship.Location.Y + ship.Height / 2 > bullet.Location.Y)
            {
                bullet.Location = new Point(bullet.Location.X, bullet.Location.Y + bulletspeed);
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
               //"Выстел" по нажатию
               Blttmr.Enabled = true;
        }
    }
}
