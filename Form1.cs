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
        public List<Bitmap> imgs = new List<Bitmap>();
        public int IF;
        public int spd;
        public int Type;
    }
    public class CMap
    {
        public int X, Y;
        public Rectangle Dst;
        public Rectangle Src;
        public Bitmap img;
        public List<Obs> Obsticals = new List<Obs>();
        public List<Obs> Coins = new List<Obs>();
        public List<Obs> Poition = new List<Obs>();
        public Obs Elevator = new Obs();
    }
    public class CHero
    {
        public int X, Y, W, H;
        public int PosX;
        public bool Rig, Lef, U, Dow, Spt;
        public List<Bitmap> Idle = new List<Bitmap>();
        public List<Bitmap> Left = new List<Bitmap>();
        public List<Bitmap> Right = new List<Bitmap>();
        public List<Bitmap> Jump = new List<Bitmap>();
        public List<Bitmap> DeadRig = new List<Bitmap>();
        public List<Bitmap> DeadLef = new List<Bitmap>();
        public List<Bitmap> Current;
        public bool HP;
        public List<Bitmap>[] Heart = new List<Bitmap>[6];
        public int[] IF_Heart = new int[6];
        public List<Bitmap> Sprint = new List<Bitmap>();
        public int IF_Sprint;
        public int Sprint_Counter, Sprt_Count;
        public int Idle_Count;
        public int spd, Sprt;
        public int IF, IF_MAX;
        public int Dir;
        public bool Elevator;
        public List<Bitmap> Coin= new List<Bitmap>();
        public int Coins;
        public int IF_Coins;
    }
    public partial class Form1 : Form
    {
        int LimitX, LimitY1, LimitY2;
        int StartX = 0, StartY = 0;
        float ScaleX, ScaleY;
        CMap Map1;
        Bitmap off;
        CHero Hero;
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
            tt.Interval = 5;
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            Hero_Move();
            Map1Move();
            Hero_ability();



            DrawBubb(CreateGraphics());
        }
        void Hero_Move()
        {
            if (Hero.Spt == true && Hero.IF_Sprint != Hero.Sprint.Count - 1)
            {
                Hero.Sprt_Count++;
                if (Hero.Sprt_Count == 5)
                {
                    Hero.Sprt_Count = 0;
                    Hero.IF_Sprint++;
                }
                Hero.Sprt = 15;
            }
            else if (Hero.Spt)
            {
                Hero.Sprt = 0;
            }
            if (Hero.Lef && Hero.PosX - Hero.spd - Hero.Sprt > 0)
            {
                Hero.PosX -= Hero.spd + Hero.Sprt;
                if (Hero.Dir != 2)
                {
                    Hero.Dir = 2;
                    Hero.IF = 1;
                    Hero.IF_MAX = Hero.Left.Count;
                    Hero.Current = Hero.Left;
                }
            }
            if (Hero.U && Hero.Y + Hero.Current[Hero.IF].Height - 5 > LimitY1)
            {
                Hero.Y -= 5;
            }
            if (Hero.Rig && Hero.PosX + Hero.W+ Hero.spd + Hero.Sprt < LimitX)
            {
                Hero.PosX += Hero.spd + Hero.Sprt;
                if (Hero.Dir != 1)
                {
                    Hero.Dir = 1;
                    Hero.IF = 1;
                    Hero.IF_MAX = Hero.Left.Count;
                    Hero.Current = Hero.Right;
                }
            }
            if (Hero.Dow && Hero.Y + Hero.Current[Hero.IF].Height + 5 < LimitY2)
            {
                Hero.Y += 5;
            }
            if (Hero.Dir == 0) Hero.Idle_Count++;

            if (Hero.Idle_Count == 5 || Hero.Dir != 0)
            {
                if((Hero.IF!=Hero.Current.Count-1&&!Hero.HP)||Hero.HP) Hero.IF = (Hero.IF + 1) % Hero.Current.Count;
                Hero.Idle_Count = 0;
            }

            if (Hero.PosX >= ClientSize.Width / 2 && Hero.PosX < LimitX - (ClientSize.Width / 2) - 20)
            {
                StartX = Hero.PosX - Hero.X;
            }
            else if (Hero.PosX <= ClientSize.Width) StartX = 0;
            Hero.X = Hero.PosX - StartX;

            //Coins
            for (int i = 0; i < Map1.Coins.Count; i++)
            {
                Obs pTrv = Map1.Coins[i];
                if (
                    isHit(Hero.PosX, Hero.Y, Hero.W, Hero.H,
                        pTrv.X, pTrv.Y, pTrv.W, pTrv.H)
                    ||
                    isHit(pTrv.X, pTrv.Y, pTrv.W, pTrv.H,
                            Hero.PosX, Hero.Y, Hero.W, Hero.H)
                    )
                {
                    Hero.Coins++;
                    Map1.Coins.RemoveAt(i);
                    break;
                }
            }
            //Poitions
            for (int i = 0; i < Map1.Poition.Count; i++)
            {
                Obs pTrv = Map1.Poition[i];
                if (
                    isHit(Hero.PosX, Hero.Y, Hero.W, Hero.H,
                        pTrv.X, pTrv.Y, pTrv.W, pTrv.H)
                    ||
                    isHit(pTrv.X, pTrv.Y, pTrv.W, pTrv.H,
                            Hero.PosX, Hero.Y, Hero.W, Hero.H)
                    )
                {
                    if (pTrv.Type == 5) HeroDamage(20);
                    Map1.Poition.RemoveAt(i);
                    break;
                }
            }
        }
        void Hero_ability()
        {
            if (Hero.IF_Sprint != 0&&Hero.HP)
            {
                Hero.Sprint_Counter++;
                if (Hero.Sprint_Counter == 20)
                {
                    Hero.Sprint_Counter = 0;
                    Hero.IF_Sprint--;
                }
            }
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
                if (Map1.Elevator.IF != 2 && Map1.Elevator.IF != 0) Map1.Elevator.IF = 2;
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
            else if (Map1.Elevator.IF != 0)
            {
                Map1.Elevator.Y += Map1.Elevator.spd;
                Hero.Y += Map1.Elevator.spd;
                Map1.Elevator.IF = 1;
            }


            //Coins
            for (int i = 0; i < Map1.Coins.Count; i++)
            {
                Obs pTrv = Map1.Coins[i];
                pTrv.IF = (pTrv.IF + 1) % pTrv.imgs.Count;
            }
        }
        void CreateMap()
        {
            Map1 = new CMap();
            Hero = new CHero();
            Map1.img = new Bitmap("assets/Maps/1.png");


            //Elevator
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/1.png"));
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/2.png"));
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/0.png"));
            Map1.Elevator.X = 3453;
            Map1.Elevator.H = 220;
            Map1.Elevator.Y = 599 - Map1.Elevator.H;
            Map1.Elevator.W = 157;
            Map1.Elevator.spd = -3;

            //Obsticals
            CreateObsticals();
            //Coins
            CreateCoins();
            //Poitions
            CreatePoitions();
        }
        void CreatePoitions()
        {
            Obs Pnn = new Obs();
            Pnn.imgs.Add(new Bitmap("assets/assets/poitions/5.png"));
            Pnn.X = 2500;
            Pnn.Y = 419;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Pnn.Type = 5;
            Map1.Poition.Add(Pnn);
        }
        void CreateObsticals()
        {
            Obs Pnn = new Obs();
            Pnn.X = 2105;
            Pnn.Y = 282;
            Pnn.W = 218;
            Pnn.H = 112;
            Map1.Obsticals.Add(Pnn);

        }
        void CreateCoins()
        {
            Obs Pnn = new Obs();
            for (int i = 0; i <= 9; i++)
            {
                Pnn.imgs.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Pnn.X = 577;
            Pnn.Y = 350;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
            Pnn = new Obs();
            for (int i = 0; i <= 9; i++)
            {
                Pnn.imgs.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Pnn.X = 2103;
            Pnn.Y = 230;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
            Pnn = new Obs();
            for (int i = 0; i <= 9; i++)
            {
                Pnn.imgs.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Pnn.X = 2103;
            Pnn.Y = 500;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
            Pnn = new Obs();
            for (int i = 0; i <= 9; i++)
            {
                Pnn.imgs.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Pnn.X = 3670;
            Pnn.Y = 419;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
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
            for (int i = 3; i <= 8; i++)
            {
                Hero.Idle.Add(new Bitmap("assets/Hero1/Right/" + i + ".png"));

            }
            for (int i = 128; i <= 132; i++)
            {
                Hero.DeadRig.Add(new Bitmap("assets/Hero1/Right/" + i + ".png"));
            }
            for (int i = 126; i <= 129; i++)
            {
                Hero.DeadLef.Add(new Bitmap("assets/Hero1/Left/" + i + ".png"));
            }
            for (int i = 0; i <= 9; i++)
            {
                Hero.Coin.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Hero.Rig = false;
            Hero.Lef = false;
            Hero.U = false;
            Hero.Dow = false;
            Hero.IF_MAX = Hero.Right.Count;
            LimitX = Map1.img.Width;
            LimitY1 = 574;
            LimitY2 = 700;
            Hero.PosX = 0;
            Hero.W = 150;
            Hero.H = 150;
            Hero.X = 0;
            Hero.Y = LimitY1;
            Hero.Dir = 0;
            Hero.Current = Hero.Idle;
            Hero.HP = true;
            Hero.spd = 15;
            Hero.Coins = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateMap();
            StartX = 0; StartY = 0;
            ScaleX = (float)ClientSize.Width / (float)Map1.img.Width;
            ScaleY = (float)ClientSize.Height / (float)Map1.img.Height;
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            CreateHero();
        }
        void HeroDamage(int Damage)
        {
            int i;
            for (i = 5; i >= 0; i--)
            {
                if (Hero.IF_Heart[i] != 4)
                {
                    while (Damage != 0)
                    {
                        Hero.IF_Heart[i]++;
                        Damage--;
                        if (Hero.IF_Heart[i] == 4) i--;
                        if (i == -1)
                        {
                            Damage = 0;
                            Hero.HP = false;
                            Hero.Rig = false;
                            Hero.Dow = false;
                            Hero.Lef= false;
                            Hero.U = false;
                            if(Hero.Dir==2||Hero.Dir==0)Hero.Current = Hero.DeadRig;
                            else Hero.Current = Hero.DeadLef;
                            Hero.IF = 0;
                        }
                    }
                    break;
                }
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) Hero.Lef = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) Hero.Rig = false;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) Hero.Dow = false;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) Hero.U = false;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.ShiftKey)
            {
                Hero.Sprt = 0;
                Hero.Spt = false;
            }

            if (!Hero.Lef && !Hero.Rig && !Hero.Dow && !Hero.U&&Hero.HP)
            {
                Hero.Dir = 0;
                Hero.Current = Hero.Idle;
                Hero.IF = 0;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawBubb(CreateGraphics());

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Hero.HP)
            {

                if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.A) && !Hero.Elevator) Hero.Lef = true;
                if ((e.KeyCode == Keys.Right || e.KeyCode == Keys.D) && !Hero.Elevator) Hero.Rig = true;
                if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.S) && !Hero.Elevator) Hero.Dow = true;
                if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.W) && !Hero.Elevator) Hero.U = true;
                if (e.KeyCode == Keys.ShiftKey && Hero.IF_Sprint < Hero.Sprint.Count - 1)
                {
                    Hero.Spt = true;
                    Hero.Sprt = 15;
                    HeroDamage(5);
                }
                if (e.KeyCode == Keys.B) //Single Bullet
                {

                }
                if (e.KeyCode == Keys.V) //Multi Bullet
                {

                }

                if (e.KeyCode == Keys.Y &&
                    (isHit(Hero.PosX, Hero.Y, Hero.W, Hero.H,
                     Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H)
                    || isHit(Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H,
                        Hero.PosX, Hero.Y, Hero.W, Hero.H)
                    ))
                {
                    Map1.Elevator.IF = 2;
                    Map1.Elevator.spd = -3;
                    Hero.Y = Map1.Elevator.Y + Map1.Elevator.H - Hero.H;
                    Hero.Elevator = true;
                }
                if (e.KeyCode == Keys.U &&
                    (isHit(Hero.PosX, Hero.Y, Hero.W, Hero.H,
                     Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H)
                    || isHit(Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H,
                        Hero.PosX, Hero.Y, Hero.W + Hero.H, Hero.H)
                    ))
                {
                    Map1.Elevator.IF = 2;
                    Map1.Elevator.spd = 3;
                    if (Map1.Elevator.Y <= 0) Map1.Elevator.Y = 1;
                    Hero.Y = Map1.Elevator.Y + Map1.Elevator.H - Hero.H;
                    Hero.Elevator = true;
                }

            }
            else
            {
                if (e.KeyCode == Keys.R)
                {
                    Form1_Load(1,EventArgs.Empty);
                }
            }

        }
        bool isHit(int X1, int Y1, int W1, int H1, int X2, int Y2, int W2, int H2)
        {
            if (
                (
                   X1 >= X2
                && X1 <= X2 + W2
                && Y1 >= Y2
                && Y1 <= Y2 + H2
                )
                ||
                (
                   X1 + W1 >= X2
                && X1 + W1 <= X2 + W2
                && Y1 >= Y2
                && Y1 <= Y2 + H2
                )
                ||
                (
                   X1 >= X2
                && X1 <= X2 + W2
                && Y1 + H1 >= Y2
                && Y1 + H1 <= Y2 + H2
                )
                ||
                (
                   X1 + W1 >= X2
                && X1 + W1 <= X2 + W2
                && Y1 + H1 >= Y2
                && Y1 + H1 <= Y2 + H2
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
            DrawHero(g);
            if (Hero.HP)
            {
                //Hero
            }
            else
            {
                
                g.DrawString("YOU DEAD", new Font("Arial", 36, FontStyle.Bold), Brushes.Red, ClientSize.Width / 2 - 40, ClientSize.Height / 2 - 30);
                g.DrawString("Press R to Restart", new Font("Arial", 36, FontStyle.Bold), Brushes.Red, ClientSize.Width / 2 - 80, ClientSize.Height / 2 + 40);
            }
        }
        void DrawHero(Graphics g)
        {
            //Draw Hearts
            for (int i = 0; i < 6; i++)
            {
                g.DrawImage(Hero.Heart[i][Hero.IF_Heart[i]], 60 * i, 0, 60, 60);
            }
            //Draw Sprint
            g.DrawImage(Hero.Sprint[Hero.IF_Sprint], 0, 70, Hero.Sprint[0].Width, Hero.Sprint[0].Height);
            //Draw Coins Number
            Hero.IF_Coins = (Hero.IF_Coins + 1) % Hero.Coin.Count;
            g.DrawString("" + Hero.Coins, new Font("Arial",40), Brushes.White, ClientSize.Width - 50, 10);
            g.DrawImage(Hero.Coin[Hero.IF_Coins], ClientSize.Width - 100, 15);

            if (!Hero.Elevator) g.DrawImage(Hero.Current[Hero.IF], Hero.X, Hero.Y * ScaleY, Hero.W, Hero.H * ScaleY);
            
        }
        void DrawMap(Graphics g)
        {
            Map1.Dst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Map1.Src = new Rectangle(StartX, StartY, ClientSize.Width, Map1.img.Height);
            g.DrawImage(Map1.img, Map1.Dst, Map1.Src, GraphicsUnit.Pixel);
            //Elevator
            g.DrawImage(Map1.Elevator.imgs[Map1.Elevator.IF], (Map1.Elevator.X - StartX), Map1.Elevator.Y * ScaleY, Map1.Elevator.W, Map1.Elevator.H * ScaleY);
            //Draw Coins
            for (int i = 0; i < Map1.Coins.Count; i++)
            {
                Obs pTrv = Map1.Coins[i];
                g.DrawImage(pTrv.imgs[pTrv.IF], pTrv.X-StartX, pTrv.Y*ScaleY, pTrv.W, pTrv.H*ScaleY);
            }
            for (int i = 0; i < Map1.Poition.Count; i++)
            {
                Obs pTrv = Map1.Poition[i];
                g.DrawImage(pTrv.imgs[pTrv.IF], pTrv.X-StartX, pTrv.Y*ScaleY, pTrv.W, pTrv.H*ScaleY);
            }


        }
    }
}
