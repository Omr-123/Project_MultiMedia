using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_MultiMedia
{
    public class Obs
    {
        public int X, Y, W, H;
        public List<Bitmap> imgs=new List<Bitmap>();
        public int IF;
        public int spd;

    }
    public class CMap
    {
        public int X,Y;
        public Rectangle Dst;
        public Rectangle Src;
        public Bitmap img;
        public List<Obs> Obsticals = new List<Obs>();
        public Obs Elevator = new Obs();
    }
    public class CHero
    {
        public int X, Y,W,H;
        public int PosX;
        public bool Rig, Lef,U,Dow;
        public List<Bitmap> Left = new List<Bitmap>();
        public List<Bitmap> Right = new List<Bitmap>();
        public List<Bitmap> Jump = new List<Bitmap>();
        public List<Bitmap> Current;
        public int HP;
        public List<Bitmap> []Heart = new List<Bitmap>[6];
        public int []IF_Heart = new int[6];
        public List<Bitmap> Sprint = new List<Bitmap>();
        public int IF_Sprint;
        public int spd;
        public int IF,IF_MAX;
        public int Dir;
        public bool Elevator;
    }
    public partial class Form1 : Form
    {
        int LimitX, LimitY1,LimitY2;
        int StartX = 0, StartY = 0;
        float ScaleX, ScaleY;
        CMap Map1 = new CMap();
        Bitmap off;
        CHero Hero = new CHero();
        Timer tt = new Timer();
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            KeyDown += Form1_KeyDown;
            Paint += Form1_Paint;
            Load += Form1_Load;
            tt.Tick += Tt_Tick;
            KeyUp += Form1_KeyUp;
            MouseDown += Form1_MouseDown;
            tt.Start();
            tt.Interval = 10;
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            Map1Move();

            DrawBubb(CreateGraphics());

        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show("X -> " + e.X + " , Y -> " + e.Y);
        }

        void Map1Move()
        {
            //Elevator
            if (Map1.Elevator.Y <= 0)
            {
                Map1.Elevator.spd *= 0;
                if (Map1.Elevator.IF != 2&&Map1.Elevator.IF!=0)  Map1.Elevator.IF = 2;
                else Map1.Elevator.IF = 0;
                Hero.Elevator = false;
            }
            else if (Map1.Elevator.Y + Map1.Elevator.H + Map1.Elevator.spd >= 599)
            {
                Map1.Elevator.spd *= 0;
                if (Map1.Elevator.IF != 2 && Map1.Elevator.IF != 0) Map1.Elevator.IF = 2;
                else Map1.Elevator.IF = 0;
                Hero.Elevator = false;
            }
            else if(Map1.Elevator.IF!=0)
            {
                Map1.Elevator.Y += Map1.Elevator.spd;
                Hero.Y += Map1.Elevator.spd;
                Map1.Elevator.IF = 1;
            }
                
        }
        void CreateMap()
        {

            Map1.img = new Bitmap("assets/Maps/1.png");
          

            //Elevator
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/1.png"));
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/2.png"));
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/0.png"));
            Map1.Elevator.X = 3453;
            Map1.Elevator.H = 220;
            Map1.Elevator.Y = 599  - Map1.Elevator.H;
            Map1.Elevator.W = 157;
            Map1.Elevator.spd = -3;

            //Obsticals
            Obs Pnn = new Obs();
            Pnn.X =2105;
            Pnn.Y =282;
            Pnn.W =218;
            Pnn.H =112;
            Map1.Obsticals.Add(Pnn);
        }
        void CreateHero()
        {
            for (int i = 80; i <= 89; i++)
            {
                Hero.Right.Add(new Bitmap("assets/Hero1/Right/" + i + ".png"));
            }
            for (int i = 79; i <= 88; i++)
            {
                Hero.Left.Add(new Bitmap("assets/Hero1/Left/" + i + ".png"));
            }
            for (int i2 = 0; i2 < 6; i2++)
            {
                Hero.Heart[i2] = new List<Bitmap>();
                for (int i = 0; i <= 4; i++)
                {
                    Hero.Heart[i2].Add(new Bitmap("assets/assets/hearts/" + i + ".png"));
                    Hero.IF_Heart[i2] = 0;
                }
            }
            for (int i = 0; i <= 6; i++)
            {
                Hero.Sprint.Add(new Bitmap("assets/assets/sprint/" + i + ".png"));
            }
            Hero.Rig = false;
            Hero.Lef = false;
            Hero.U = false;
            Hero.Dow = false;
            Hero.IF_MAX = Hero.Right.Count;
            LimitX = Map1.img.Width;
            LimitY1 = 574;
            LimitY2 = 700;
            Hero.W = 150;
            Hero.H = 150;
            Hero.X = 0;
            Hero.Y = LimitY1;
            Hero.Dir = 1;
            Hero.Current = Hero.Right;
            Hero.HP = 100;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateMap();
            ScaleX = (float)ClientSize.Width  / (float)Map1.img.Width;
            ScaleY = (float) ClientSize.Height/ (float)Map1.img.Height;
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            CreateHero();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) Hero.Lef = false;
            if (e.KeyCode == Keys.Right) Hero.Rig = false;
            if (e.KeyCode == Keys.Down) Hero.Dow = false;
            if (e.KeyCode == Keys.Up) Hero.U = false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawBubb(CreateGraphics());

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left&&!Hero.Elevator) Hero.Lef = true;
            if (e.KeyCode == Keys.Right&& !Hero.Elevator) Hero.Rig = true;
            if (e.KeyCode == Keys.Down&& !Hero.Elevator) Hero.Dow = true;
            if (e.KeyCode == Keys.Up&& !Hero.Elevator) Hero.U = true;
            if (e.KeyCode == Keys.ShiftKey) Hero.IF_Sprint = (Hero.IF_Sprint + 1) % Hero.Sprint.Count;
            if (e.KeyCode == Keys.B) //Single Bullet
            {

            }
            if (e.KeyCode == Keys.V) //Multi Bullet
            {

                MessageBox.Show("Elevator X:" + Map1.Elevator.X + ", Hero X="+Hero.PosX);
                MessageBox.Show("   " + (Hero.Y + Hero.H) + "," + Hero.H+", "+ Map1.Elevator.Y + " , "+ Map1.Elevator.H);
            }
            
            if (e.KeyCode == Keys.Y &&
                (isHit(Hero.PosX, Hero.Y, Hero.W, Hero.H,
                 Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H)
                || isHit(Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H,
                    Hero.PosX, Hero.Y , Hero.W, Hero.H)
                ))
            {
                Map1.Elevator.IF = 2;
                Map1.Elevator.spd = -3;
                Hero.Y = Map1.Elevator.Y + Map1.Elevator.H - Hero.H;
                Hero.Elevator = true;
            }
            if (e.KeyCode == Keys.U &&
                (isHit(Hero.PosX, Hero.Y , Hero.W, Hero.H,
                 Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H)
                || isHit(Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H,
                    Hero.PosX, Hero.Y, Hero.W + Hero.H , Hero.H)
                ))
            {
                Map1.Elevator.IF = 2;
                Map1.Elevator.spd = 3;
                if (Map1.Elevator.Y <= 0) Map1.Elevator.Y = 1;
                Hero.Y = Map1.Elevator.Y + Map1.Elevator.H - Hero.H;
                Hero.Elevator = true;
            }




            if (Hero.Lef && Hero.PosX - 5 > 0)
            {
                Hero.PosX -= 40;
                if (Hero.Dir != 2)
                {
                    Hero.Dir = 2;
                    Hero.IF = -1;
                    Hero.IF_MAX = Hero.Left.Count;
                    Hero.Current = Hero.Left;
                }
                Hero.IF = (Hero.IF + 1) % Hero.IF_MAX;
            }
            if (Hero.U && Hero.Y+ Hero.Current[Hero.IF].Height - 5 > LimitY1)
            {
                Hero.Y -= 5;
            }
            if (Hero.Rig && Hero.PosX + Hero.Current[Hero.IF].Width+80 < LimitX-30)
            {
                Hero.PosX += 40;
                if (Hero.Dir != 1)
                {
                    Hero.Dir = 1;
                    Hero.IF = -1;
                    Hero.IF_MAX = Hero.Left.Count;
                    Hero.Current = Hero.Right;
                }
                Hero.IF = (Hero.IF + 1) % Hero.IF_MAX;
            }
            if (Hero.Dow && Hero.Y + Hero.Current[Hero.IF].Height + 5 < LimitY2)
            {
                Hero.Y += 5;
            }
            if (Hero.PosX >= ClientSize.Width / 2 && Hero.PosX < LimitX-(ClientSize.Width/2)-20)
            {
                StartX = Hero.PosX - Hero.X;
            }
            Hero.X = Hero.PosX - StartX;



        }
        bool isHit(int X1,int Y1,int W1,int H1,int X2,int Y2,int W2,int H2)
        {
            if (
                (
                   X1 >= X2
                && X1 <= X2 + W2
                && Y1>=Y2
                && Y1<=Y2+H2
                )
                ||
                (
                   X1+W1 >= X2
                && X1+W1 <= X2 + W2
                && Y1 >= Y2
                && Y1 <= Y2 + H2
                )
                ||
                (
                   X1 >= X2
                && X1 <= X2 + W2
                && Y1+H1 >= Y2
                && Y1+H1 <= Y2 + H2
                )
                ||
                (
                   X1 +W1>= X2
                && X1+W1 <= X2 + W2
                && Y1+H1 >= Y2
                && Y1+H1 <= Y2 + H2
                )
                )
            {
                return true;
            }
            return false;
        }
        void DrawBubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            DrawMap(g);
            //Hero
            DrawHero(g);
        }
        void DrawHero(Graphics g)
        {
            for (int i = 0; i < 6; i++)
            {
                g.DrawImage(Hero.Heart[i][Hero.IF_Heart[i]],60*i,0,60,60);
            }
            //Draw Sprint
            g.DrawImage(Hero.Sprint[Hero.IF_Sprint],0,70,Hero.Sprint[0].Width,Hero.Sprint[0].Height);

            if(!Hero.Elevator) g.DrawImage(Hero.Current[Hero.IF], Hero.X, Hero.Y*ScaleY,Hero.W,Hero.H * ScaleY);
        }
        void DrawMap(Graphics g)
        {
            Map1.Dst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Map1.Src = new Rectangle(StartX, StartY, ClientSize.Width, Map1.img.Height);
            g.DrawImage(Map1.img, Map1.Dst, Map1.Src, GraphicsUnit.Pixel);
            //Elevator
            g.DrawImage(Map1.Elevator.imgs[Map1.Elevator.IF], (Map1.Elevator.X-StartX), Map1.Elevator.Y* ScaleY, Map1.Elevator.W, Map1.Elevator.H*ScaleY);
        }
    }
}
