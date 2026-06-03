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
        public int IF,Counter;
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
        public List<Obs> Laser= new List<Obs>();
        
        public Obs Elevator = new Obs();
        public Obs Ladder = new Obs();
    }
    public class CHero
    {
        public int X, Y, W, H;
        public int PosX,PosY;
        public bool Rig, Lef, U, Dow, Spt;
        public List<Bitmap> Idle = new List<Bitmap>();
        public List<Bitmap> Move = new List<Bitmap>();
        public List<Bitmap> Run = new List<Bitmap>();
        public List<Bitmap> Jump = new List<Bitmap>();
        public List<Bitmap> JumpDou = new List<Bitmap>();
        public List<Bitmap> Dead = new List<Bitmap>();
        public List<Bitmap> Bullet = new List<Bitmap>();
        public Bitmap SBullett = new Bitmap("assets/Hero1/Bullets/Bullet.png");
        public Bitmap MBullett = new Bitmap("assets/Hero1/Bullets/Bullet2.png");
        public int SBulletX, SBulletY;
        public List<int> MBulletX=new List<int>(), MBulletY=new List<int>();
        public List<Bitmap> Current;
        
        public bool Elevator,HP,SBullet;
        public List<bool> MBullet=new List<bool>();
        public List<Bitmap>[] Heart = new List<Bitmap>[6];
        public int[] IF_Heart = new int[6];
        public List<Bitmap> Sprint = new List<Bitmap>();
        public int IF_Sprint,Jum,J1=1,J2=-6,J3=5,Jumper_Counter;
        public int Sprint_Counter, Sprt_Count;
        public int Idle_Count;
        public int spd, Sprt;
        public int IF;
        public string Dir;
        public List<Bitmap> Coin= new List<Bitmap>();
        public int Coins;
        public int IF_Coins;
    }
    public class Enemy
    {
        public int X,Y,W,H;
        public List<Bitmap> Move = new List<Bitmap>();
        public List<Bitmap> Current;
        public List<int> FireX = new List<int>(),FireY=new List<int>();
        public Bitmap Bullet = new Bitmap("assets/enemy3/Bullet.png");
        public List<Bitmap> Fly = new List<Bitmap>();
        public List<Bitmap> Ready = new List<Bitmap>();
        public int IF;
        public string Dir="right";
        public int spd = 10;
        public int Counter;
        public int HP=100;
    }
    public class planes
    { 
        public Bitmap img;
        public int X=-1, Y=0, W, H;
    }
    public class Big_Boss
    {
        public int X=-1, Y, W, H;
        public string Status="Phase1",Dir="left",Action="Idle";
        public List<Bitmap> Health1 = new List<Bitmap>();
        public List<Bitmap> Health2 = new List<Bitmap>();
        public List<Bitmap> CurrentHealth= new List<Bitmap>();
        //Phase 1
        public List<Bitmap> Idle= new List<Bitmap>();
        public List<Bitmap> Move= new List<Bitmap>();
        public List<Bitmap> Fist= new List<Bitmap>();
        public List<Bitmap> Sword= new List<Bitmap>();
        //Phase 2
        public List<Bitmap> Idle_Aura= new List<Bitmap>();
        public List<Bitmap> Move_Aura= new List<Bitmap>();
        public List<Bitmap> Blade= new List<Bitmap>();
        public Bitmap BladeEffect;
        public int BladeX=-1, BladeY=-1,BladeW,BladeH;
        public List<Bitmap> Canonn= new List<Bitmap>();
        public int CanonnX=-1, CanonnY=-1,CanonnW;
        public Bitmap Canonn_Fire;
        public List<Bitmap> Teleport= new List<Bitmap>();
        public int TeleX = -1, TeleY = -1, TeleW,TeleH;
        public Bitmap Teleport_Effect;
        
        public List<Bitmap> Freeze= new List<Bitmap>();
        public List<Bitmap> Damage= new List<Bitmap>();

        public List<Bitmap> Current= new List<Bitmap>();
        public List<Bitmap> Back= new List<Bitmap>();
        public List<Bitmap> End = new List<Bitmap>();
        public int IF_Health, Hit, Phase = 1, IF, Counter_Hit, Counter_Damage,IF_Counter,IF_C=5;
        public int Time=0,Time_Count=0,Count_Down=0,CoolDown=0;



    }
    public partial class Form1 : Form
    {
        int Fl,Alpha;
        int LimitX, LimitY1, LimitY2;
        int BaseY=0;
        int StartX = 0, StartY = 0;
        float ScaleX, ScaleY;
        bool isDrag=false;
        Random RR = new Random();
        Big_Boss Boss;
        CMap Map1;
        List<Enemy> Enemy2;
        Bitmap off;
        planes Plane;
        CHero Hero;
        int Cou = 0;
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
            MouseUp += Form1_MouseUp;
            tt.Start();
            tt.Interval = 5;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left) isDrag = false;
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            Hero_Move();
            Map1Move();
            LaserMove();
            MoveEnemy();
            MoveBoss();
            Hero_ability();
            MoveBoss();
            DrawBubb(CreateGraphics());
        }
        void Hero_Move()
        {
            if (Hero.Jum == 0)
            {
                if (Hero.Spt&& Hero.IF_Sprint != Hero.Sprint.Count - 1)
                {
                    Hero.Sprt_Count++;
                    Hero.Current = Hero.Run;
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
                    Hero.Current = Hero.Move;
                }
                if (Hero.Lef && Hero.PosX - Hero.spd - Hero.Sprt > 0)
                {
                    Hero.PosX -= Hero.spd + Hero.Sprt;
                    if (Hero.Dir != "left")
                    {
                        Hero.Dir = "left";
                        Hero.IF = 1;
                        if (Hero.W > 0)
                        {
                            Hero.X += Hero.W;
                            Hero.PosX += Hero.W;
                            Hero.W *= -1;
                        }
                    }
                    if (Hero.Spt && Hero.IF_Sprint != Hero.Sprint.Count - 1) Hero.Current = Hero.Run;
                    else Hero.Current = Hero.Move;
                } 
                if (Hero.U && ((Hero.PosY + Hero.H - 5 > LimitY1) ||
                    (
                        isHit(Hero.PosX, Hero.PosY, Hero.W-40, Hero.H,
                            Map1.Ladder.X, Map1.Ladder.Y, Map1.Ladder.W, Map1.Ladder.H)
                        ||
                        isHit(Map1.Ladder.X, Map1.Ladder.Y, Map1.Ladder.W, Map1.Ladder.H,
                                Hero.PosX, Hero.PosY, Hero.W-40, Hero.H)
                       )
                       && (Hero.PosY - 5 > 26)
                    ))
                {
                    Hero.PosY -= 5;
                }
                if (Hero.Rig && Hero.PosX + Hero.W + Hero.spd + Hero.Sprt < LimitX-Hero.W-20)
                {
                    Hero.PosX += Hero.spd + Hero.Sprt;
                    if (Hero.Dir != "right")
                    {
                        Hero.Dir = "right";
                        Hero.IF = 1;
                        
                        if (Hero.W < 0)
                        {
                            Hero.PosX += Hero.W;
                            Hero.X += Hero.W;
                            Hero.W *= -1;
                        }
                    }
                    if (Hero.Spt && Hero.IF_Sprint != Hero.Sprint.Count - 1) Hero.Current = Hero.Run;
                    else Hero.Current = Hero.Move;
                }
                if (Hero.Dow && Hero.PosY + Hero.H + 5 < LimitY2) Hero.PosY += 5;
                
                if (Hero.Dir == "idle") Hero.Idle_Count++;

                if (Hero.Idle_Count == 5 || Hero.Dir != "idle")
                {
                    if ((Hero.IF != Hero.Current.Count - 1 && !Hero.HP) || Hero.HP) Hero.IF = (Hero.IF + 1) % Hero.Current.Count;
                    Hero.Idle_Count = 0;
                }
            }
            else
            {
                if (Hero.IF + Hero.J1 == Hero.Current.Count)
                {
                    Hero.J1 = -1;
                    Hero.J2 = 6;
                }
                Hero.Jumper_Counter++;
                if (Hero.Jumper_Counter == 3)
                {
                    Hero.IF += Hero.J1;
                    Hero.Jumper_Counter = 0;
                }
                if (Hero.IF == 0)
                {
                    Hero.Current = Hero.Idle;
                    Hero.Dir = "idle";
                    Hero.Jum = 0;
                    Hero.J1 = 1;
                    Hero.J2 = -6;
                    Hero.IF = 0;
                }
                else
                {
                    if (!CheckObsticals(0, Hero.J2)) Hero.PosY += Hero.J2;
                    if (!CheckObsticals(Hero.J3, 0)) Hero.PosX += Hero.J3;
                }

            }


            if (Hero.PosX >= ClientSize.Width / 2 && Hero.PosX < LimitX - (ClientSize.Width / 2) - 20)
            {
                StartX = Hero.PosX - Hero.X;
                if (StartX < 0) StartX = 0;
            }
            else if (Hero.PosX <= ClientSize.Width) StartX = 0;
            Hero.X = Hero.PosX - StartX;
            
            Hero.Y = Hero.PosY - StartY;
            //Coins
            for (int i = 0; i < Map1.Coins.Count; i++)
            {
                Obs pTrv = Map1.Coins[i];
                if (
                    isHit(Hero.PosX, Hero.PosY, Hero.W, Hero.H,
                        pTrv.X, pTrv.Y, pTrv.W, pTrv.H)
                    ||
                    isHit(pTrv.X, pTrv.Y, pTrv.W, pTrv.H,
                            Hero.PosX, Hero.PosY, Hero.W, Hero.H)
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
                    isHit(Hero.PosX, Hero.PosY, Hero.W, Hero.H,
                        pTrv.X, pTrv.Y, pTrv.W, pTrv.H)
                    ||
                    isHit(pTrv.X, pTrv.Y, pTrv.W, pTrv.H,
                            Hero.PosX, Hero.PosY, Hero.W, Hero.H)
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
            if (Hero.IF_Sprint != 0)
            {
                Hero.Sprint_Counter++;
                if (Hero.Sprint_Counter == 20)
                {
                    Hero.Sprint_Counter = 0;
                    Hero.IF_Sprint--;
                }
            }
            if (Hero.SBulletX != -1)
            {
                if (Hero.SBullet == true)
                {
                    if (Hero.IF + 1 == Hero.Current.Count)
                    {
                        Hero.Current = Hero.Idle;
                        if (Hero.Dir == "right" && Hero.W < 0) Hero.W *= -1;
                        else if (Hero.Dir == "left" && Hero.W > 0) Hero.W *= -1;
                        Hero.IF = 0;
                        Hero.SBullet = false;
                    }
                }
                if (Hero.SBulletX > Hero.PosX) Hero.SBulletX += 32;
                else Hero.SBulletX -= 32;
                //Damage Enemy2
                for (int i = 0; i < Enemy2.Count; i++)
                {
                    Enemy pTrv = Enemy2[i];
                    int A = 1;
                    if (pTrv.W <= 0) A = -1;
                    if (isHit(Hero.SBulletX, Hero.SBulletY, Hero.SBullett.Width, Hero.SBullett.Height,
                     pTrv.X, pTrv.Y, pTrv.W*A, pTrv.H)
                    || isHit(pTrv.X, pTrv.Y, pTrv.W*A, pTrv.H,
                        Hero.SBulletX, Hero.SBulletY, Hero.SBullett.Width, Hero.SBullett.Height)
                    )
                    {
                        Hero.SBullet = false;
                        Hero.SBulletX = -1;
                        Hero.SBulletY = -1;
                        pTrv.HP -= 40;
                        if (pTrv.HP <= 0)
                        {
                            Enemy2.RemoveAt(i);
                            i--;
                        }
                    }
                }
                //Damage Big_Boss
                if (Boss.X != -1 && Boss.Status!="End")
                {
                    int A = 1;
                    if (Boss.W <= 0) A = -1;
                    if (isHit(Hero.SBulletX, Hero.SBulletY, Hero.SBullett.Width, Hero.SBullett.Height,
                     Boss.X, Boss.Y, Boss.W*A, Boss.H)
                    || isHit(Boss.X, Boss.Y, Boss.W*A, Boss.H,
                        Hero.SBulletX, Hero.SBulletY, Hero.SBullett.Width, Hero.SBullett.Height)
                    )
                    {
                        if (Boss.Action == "Idle")
                        {
                            Boss.Back = Boss.Current;
                            Boss.Current = Boss.Damage;
                        }
                        Hero.SBullet = false;
                        Hero.SBulletX = -1;
                        Hero.SBulletY = -1;
                        Boss.Hit += 4;
                        if (Boss.Hit >= 3)
                        {
                            Boss.Hit = 0;
                            Boss.IF_Health++;
                            if (Boss.IF_Health == Boss.CurrentHealth.Count - 1)
                            {
                                if (Boss.Status == "Phase1")
                                {
                                    Boss.Status = "Phase2";
                                    Boss.IF_Health = 0;
                                    Boss.CurrentHealth = Boss.Health2;
                                    Boss.Action = "Idle";
                                    Boss.IF = 0;
                                    Boss.IF_C = 5;
                                    Boss.Current = Boss.Idle_Aura;
                                    Boss.CoolDown = 0;
                                    Boss.IF_Counter = 0;
                                    Boss.Count_Down = 0;
                                }
                                else
                                {
                                    Boss.IF = 0;
                                    Boss.Current = Boss.End;
                                    Boss.IF = 2;
                                    Boss.Status = "End";
                                }
                            }
                        }
                    }
                }

                if (Hero.SBulletX < Hero.PosX - 900 || Hero.SBulletX > Hero.PosX + 900 || Hero.SBulletX < 0 || Hero.SBulletX > LimitX)
                {
                    Hero.SBullet = false;
                    Hero.SBulletX = -1;
                    Hero.SBulletY = -1;
                }


            }
            if (isDrag)
            {
                Cou++;
                if (Cou == 4)
                {

                    Hero.MBullet.Add(true);
                    Hero.MBulletY.Add(Hero.PosY + Hero.H / 2);
                    Hero.IF = 0;
                    Hero.Current = Hero.Bullet;
                    if (Hero.Dir == "right")
                    {
                        Hero.MBulletX.Add(Hero.PosX + Hero.W + 10);
                    }
                    else
                    {
                        Hero.MBulletX.Add(Hero.PosX - 10);

                    }

                    Cou = 0;
                }
            }

            for (int i = 0; i < Hero.MBulletX.Count; i++)
            {
                if (Hero.MBullet[i] == true)
                {
                    if (Hero.IF + 1 == Hero.Current.Count)
                    {
                        Hero.Current = Hero.Idle;
                        if (Hero.Dir == "right" && Hero.W < 0) Hero.W *= -1;
                        else if (Hero.Dir == "left" && Hero.W > 0) Hero.W *= -1;
                        Hero.IF = 0;
                        Hero.MBullet[i] = false;
                    }
                }
                if (Hero.MBulletX[i] > Hero.PosX) Hero.MBulletX[i] += 32;
                else Hero.MBulletX[i] -= 32;
                if (Hero.MBulletX[i] < Hero.PosX - 900 || Hero.MBulletX[i] > Hero.PosX + 900 || Hero.MBulletX[i] < 0 || Hero.MBulletX[i] > LimitX)
                {
                    Hero.MBullet.RemoveAt(i);
                    Hero.MBulletX.RemoveAt(i);
                    Hero.MBulletY.RemoveAt(i);
                    i--;
                }
                else
                {
                    //Damage Enemy2
                    for (int i2 = 0; i2 < Enemy2.Count && i>=0; i2++)
                    {
                        Enemy pTrv = Enemy2[i2];
                        int A = 1;
                        if (A <= 0) A *= -1;
                        if (isHit(Hero.MBulletX[i], Hero.MBulletY[i], Hero.SBullett.Width, Hero.SBullett.Height,
                         pTrv.X, pTrv.Y, pTrv.W*A, pTrv.H)
                        || isHit(pTrv.X, pTrv.Y, pTrv.W*A, pTrv.H,
                            Hero.MBulletX[i], Hero.MBulletY[i], Hero.SBullett.Width, Hero.SBullett.Height)
                        )
                        {
                            Hero.MBullet.RemoveAt(i);
                            Hero.MBulletX.RemoveAt(i);
                            Hero.MBulletY.RemoveAt(i);
                            i--;
                            pTrv.HP -= 40;
                            if (pTrv.HP <= 0)
                            {
                                Enemy2.RemoveAt(i2);
                                i2--;
                            }
                        }
                    }
                    //Damage Boss
                    if (Boss.X != -1 && i >= 0 && Boss.Status!="End")
                    {
                        int A = 1;
                        if (Boss.W <= 0) A = -1;
                        if (isHit(Hero.MBulletX[i], Hero.MBulletY[i], Hero.MBullett.Width, Hero.MBullett.Height,
                         Boss.X, Boss.Y, Boss.W*A, Boss.H)
                        || isHit(Boss.X, Boss.Y, Boss.W*A, Boss.H,
                            Hero.MBulletX[i], Hero.MBulletY[i], Hero.MBullett.Width, Hero.MBullett.Height)
                        )
                        {
                            if (Boss.Action == "Idle")
                            {
                                Boss.Back = Boss.Current;
                                Boss.Current = Boss.Damage;
                            }
                            Hero.MBullet.RemoveAt(i);
                            Hero.MBulletX.RemoveAt(i);
                            Hero.MBulletY.RemoveAt(i);
                            i--;
                            Boss.Hit ++;
                            if (Boss.Hit >= 3)
                            {
                                Boss.Hit = 0;
                                Boss.IF_Health++;
                                if (Boss.IF_Health == Boss.CurrentHealth.Count - 1)
                                {
                                    if (Boss.Status == "Phase1")
                                    {
                                        Boss.Status = "Phase2";
                                        Boss.IF_Health = 0;
                                        Boss.CurrentHealth = Boss.Health2;
                                        Boss.Action = "Idle";
                                        Boss.IF = 0;
                                        Boss.IF_C = 5;
                                        Boss.Current = Boss.Idle_Aura;
                                        Boss.CoolDown = 0;
                                        Boss.IF_Counter = 0;
                                        Boss.Count_Down = 0;
                                    }
                                    else
                                    {
                                        Boss.IF = 0;
                                        Boss.Current = Boss.End;
                                        Boss.IF = 2;
                                        Boss.Status = "End";
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left&&Hero.HP&&Hero.Jum==0) isDrag = true;
        }
        bool CheckObsticals(int X,int Y)
        {
            return false;
        }
        void Map1Move()
        {
            //Elevator
            if (Map1.Elevator.Y <= 0+BaseY)
            {
                Map1.Elevator.spd *= 0;
                if (Map1.Elevator.IF != 2 && Map1.Elevator.IF != 0) Map1.Elevator.IF = 2;
                else Map1.Elevator.IF = 0;
                Hero.Elevator = false;
            }
            else if (Map1.Elevator.Y + Map1.Elevator.H + Map1.Elevator.spd >= 599+BaseY)
            {
                Map1.Elevator.spd *= 0;
                if (Map1.Elevator.IF != 2 && Map1.Elevator.IF != 0) Map1.Elevator.IF = 2;
                else Map1.Elevator.IF = 0;
                Hero.Elevator = false;
            }
            else if (Map1.Elevator.IF != 0)
            {
                Map1.Elevator.Y += Map1.Elevator.spd;
                Hero.PosY += Map1.Elevator.spd;
                Hero.Y = Hero.PosY - StartY;
                Map1.Elevator.IF = 1;
            }


            //Coins
            for (int i = 0; i < Map1.Coins.Count; i++)
            {
                Obs pTrv = Map1.Coins[i];
                pTrv.IF = (pTrv.IF + 1) % pTrv.imgs.Count;
            }

            //Plane Move
            if (Hero.PosX > 1388 && Plane.X == -1)
            {
                Plane.X = 2;
                Plane.Y = 0;
              
            }
            else if (Plane.X + Plane.W > LimitX) Plane.X = -2;
            else if(Plane.X>=0) Plane.X += 20;
        }
        void MoveEnemy()
        {
            if((Plane.X>=1840&&Plane.X<=1900 && Enemy2.Count==0)|| (Plane.X >= 2100 && Plane.X <= 2150 && Enemy2.Count == 1))
            {
                Enemy Pnn = new Enemy();
                for (int i = 182; i <= 194; i++) //Fly
                {
                    Pnn.Fly.Add(new Bitmap("assets/enemy3/" + i + ".png"));
                }
                for (int i = 199; i <= 204; i++) //Ready
                {
                    Pnn.Ready.Add(new Bitmap("assets/enemy3/" + i + ".png"));
                }
                for (int i = 41; i <= 62; i++) //Ready
                {
                    Pnn.Move.Add(new Bitmap("assets/enemy3/" + i + ".png"));
                }
                Pnn.X = Plane.X;
                Pnn.Y = Plane.Y + Plane.H;
                Pnn.W = Pnn.Fly[0].Width;
                Pnn.H = Pnn.Fly[0].Height;
                Pnn.Current = Pnn.Fly;
                Enemy2.Add(Pnn);
            }


            for (int i = 0; i < Enemy2.Count; i++)
            {
                Enemy pTrv = Enemy2[i];

                pTrv.Counter++;
                if (pTrv.Counter == 7 || (pTrv.IF>=12 &&pTrv.IF<=14))
                {
                    pTrv.IF = (pTrv.IF + 1) % pTrv.Current.Count;
                    pTrv.Counter = 0;
                }
                if (pTrv.Current == pTrv.Fly)
                {
                    if(pTrv.Y+5<566)pTrv.Y += 5;
                }
                if (pTrv.Current == pTrv.Fly && pTrv.Y +5 > 566)
                {
                    pTrv.Current = pTrv.Ready;
                    pTrv.IF = 0;
                }
                else if (pTrv.Current == pTrv.Ready && pTrv.IF + 1 == pTrv.Ready.Count)
                {
                    pTrv.Current = pTrv.Move;
                    pTrv.IF = 0;
                }
                if (pTrv.Current == pTrv.Move)
                {
                    if (pTrv.IF == 12)
                    {
                        pTrv.FireY.Add(pTrv.Y);
                        if (pTrv.Dir == "right") pTrv.FireX.Add(pTrv.X+pTrv.W+ 10);
                        else pTrv.FireX.Add(pTrv.X- 10+pTrv.W);
                    }
                    Enemy2Damage();
                    if (pTrv.X > 2400)
                    {
                        pTrv.spd = -10;
                        pTrv.Dir = "left"; 
                        pTrv.W *= -1;
                        pTrv.X = 2350;
                    }
                    if (pTrv.X < 1750)
                    {
                        pTrv.spd = 10;
                        pTrv.Dir = "right";
                        pTrv.X = 1780;
                        pTrv.W *= -1;
                    }
                    pTrv.X += pTrv.spd;
                }

            }
        }
        void Enemy2Damage()
        {
            //Hero Damage

            for (int i2 = 0; i2 < Enemy2.Count; i2++)
            {
                Enemy pTrv = Enemy2[i2];
                for (int i = 0; i < pTrv.FireX.Count; i++)
                {
                    if (pTrv.FireX[i] > pTrv.X) pTrv.FireX[i] += 10;
                    else pTrv.FireX[i] -= 10;

                    if (pTrv.FireX[i] < pTrv.X - 500 || pTrv.FireX[i] > pTrv.X + 500 || pTrv.FireX[i] < 0 || pTrv.FireX[i] > LimitX)
                    {
                        pTrv.FireX.RemoveAt(i);
                        pTrv.FireY.RemoveAt(i);
                        i--;
                    }
                    else if (isHit(Hero.PosX, Hero.PosY, Hero.W, Hero.H,
                     pTrv.FireX[i], pTrv.FireY[i], pTrv.Bullet.Width, pTrv.Bullet.Height)
                    || isHit(pTrv.FireX[i], pTrv.FireY[i], pTrv.Bullet.Width, pTrv.Bullet.Height,
                        Hero.PosX, Hero.PosY, Hero.W + Hero.H, Hero.H)
                    )
                    {
                        HeroDamage(1);
                        pTrv.FireX.RemoveAt(i);
                        pTrv.FireY.RemoveAt(i);
                        i--;
                    }

                }
            }

        }
        void CreateMap()
        {
            Map1 = new CMap();
            Hero = new CHero();
            Map1.img = new Bitmap("assets/Maps/1.png");
            Enemy2 = new List<Enemy>();
            //Elevator
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/1.png"));
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/2.png"));
            Map1.Elevator.imgs.Add(new Bitmap("assets/assets/Elevator/0.png"));
            Map1.Elevator.X = 3453;
            Map1.Elevator.H = 220;
            Map1.Elevator.Y = 599 - Map1.Elevator.H+BaseY;
            Map1.Elevator.W = 157;
            Map1.Elevator.spd = -3;

            //Ladder
            Map1.Ladder.X =2911;
            Map1.Ladder.Y =37;
            Map1.Ladder.W =2999-Map1.Ladder.X;
            Map1.Ladder.H =583-Map1.Ladder.Y;

            //Obsticals
            CreateObsticals();
            //Coins
            CreateCoins();
            //Poitions
            CreatePoitions();
            //Laser
            for (int i = 1; i <= 6; i++)
            {
                Obs Pnn = new Obs();
                for (int i2 = 0; i2 < 6; i2++)
                {
                    Pnn.imgs.Add(new Bitmap("assets/assets/laser/" + i2 + ".png"));
                }
                Pnn.W = Pnn.imgs[0].Width;
                Pnn.H = 678 - 403;
                Pnn.X = 4001 + (i * Pnn.W/2);
                Pnn.Y = 402;

                Map1.Laser.Add(Pnn);
            }
            //Plane
            Plane = new planes();
            Plane.img = new Bitmap("assets/Plane.png");
            Plane.img.MakeTransparent(Color.White);
            Plane.W = Plane.img.Width / 2;
            Plane.H = Plane.img.Height/2;
        }
        void CreateBoss()
        {
            for (int i = 0; i <=5; i++)
            {
                Boss.Health1.Add(new Bitmap("assets/big_boss/health/" + i + ".png"));
            }
            for (int i = 30; i <=35; i++)
            {
                Boss.Health2.Add(new Bitmap("assets/big_boss/health/" + i + ".png"));
            }
            for (int i = 0; i <= 6; i++)
            {
                Boss.Idle.Add(new Bitmap("assets/big_boss/11/" + i + ".png"));
            }
            for (int i = 0; i <= 2; i++)
            {
                Boss.Idle_Aura.Add(new Bitmap("assets/big_boss/1/" + i + ".png"));
            }
            for (int i = 3; i <= 8; i++)
            {
                Boss.Move.Add(new Bitmap("assets/big_boss/1/" + i + ".png"));
            }
            for (int i = 0; i <= 2; i++)
            {
                Boss.Move_Aura.Add(new Bitmap("assets/big_boss/1/" + i + ".png"));
            }
            for (int i = 0; i <= 6; i++)
            {
                Boss.Damage.Add(new Bitmap("assets/big_boss/3/" + i + ".png"));
            }
            for (int i = 0; i <= 3; i++)
            {
                Boss.Canonn.Add(new Bitmap("assets/big_boss/5/" + i + ".png"));
            }
            Boss.Canonn_Fire = new Bitmap("assets/big_boss/5/5.png");
            for (int i = 4; i >= 1; i--)
            {
                Boss.Blade.Add(new Bitmap("assets/big_boss/8/" + i + ".png"));
            }
            Boss.BladeEffect = new Bitmap("assets/big_boss/8/0.png");
            for (int i = 0; i <= 2; i++)
            {
                Boss.Sword.Add(new Bitmap("assets/big_boss/9/" + i + ".png"));
            } 
            for (int i = 0; i <= 11; i++)
            {
                Boss.Fist.Add(new Bitmap("assets/big_boss/2/" + i + ".png"));
            }
            for (int i = 0; i <= 2; i++)
            {
                Boss.Teleport.Add(new Bitmap("assets/big_boss/6/" + i + ".png"));
            }      
            for (int i = 3; i <= 5; i++)
            {
                Boss.End.Add(new Bitmap("assets/big_boss/10/" + i + ".png"));
            }
            Boss.Teleport_Effect = new Bitmap("assets/big_boss/6/3.png");
            Boss.Action = "Idle";
            Boss.CurrentHealth = Boss.Health1;
            Boss.Current =Boss.Idle ;
            Boss.W =-Boss.Idle[0].Width;
            Boss.H =Boss.Idle[0].Height;
            Boss.Y = 513;
        }
        void MoveBoss()
        {
            if (Boss.X != -1 && StartX <=Boss.X && StartX + ClientSize.Width >= Boss.X && Boss.Status!="End")
            {
                if (Boss.CoolDown != -1) Boss.CoolDown++;
                
                
                if (Boss.Status == "Phase1" && Boss.Current == Boss.Idle && Boss.CoolDown > 110)
                {
                    int R = RR.Next(1, 3);
                    if (R == 1) Boss.Action = "Fist";
                    else Boss.Action = "Sword";
                    Boss.Count_Down = 0;
                    Boss.Time_Count = 0;
                    Boss.IF = 0;
                    Boss.Time = 200;
                    Boss.CoolDown = -1;
                }
                else if (Boss.Status == "Phase2" && Boss.Current == Boss.Idle_Aura && Boss.CoolDown > 110)
                {
                    int R = RR.Next(1, 4);
                    if (R == 1) Boss.Action = "Blade";
                    else if(R==2)Boss.Action = "Canonn";
                    else Boss.Action = "Teleport";
                    Boss.Current = Boss.Move_Aura;
                    Boss.IF = 0;
                    Boss.Count_Down = 0;
                    Boss.Time = 240;
                    Boss.CoolDown = -1;
                }
                
            
            }
            if (Boss.X != -1)
            {
                if ((Boss.Dir == "right" && Boss.W < 0) || (Boss.Dir == "left" && Boss.W > 0))
                {
                    Boss.X += Boss.W;
                    Boss.W *= -1;
                }

                Boss.IF_Counter++;
                if (Boss.IF_Counter == Boss.IF_C)
                {
                    Boss.IF = (Boss.IF + 1) % Boss.Current.Count;
                    Boss.IF_Counter = 0;
                }
                if (Boss.Current == Boss.Damage && Boss.IF + 1 == Boss.Damage.Count)
                {
                    if (Boss.Status == "Phase1") Boss.Current = Boss.Idle;
                    else Boss.Current = Boss.Idle_Aura;
                    Boss.Action = "Idle";
                    Boss.IF = 0;
                }
                if (Boss.X > Hero.PosX && Boss.Dir == "right") Boss.Dir = "left";
                if (Boss.X < Hero.PosX && Boss.Dir == "left") Boss.Dir = "right";
                //Fist
                if (Boss.Action == "Fist")
                {
                    Boss.Time_Count++;
                    Boss.Count_Down++;
                    Boss.IF_C = 6;
                    int A = 1, W = 0, W2 = 0, Flag = 0;
                    if (Hero.W <= 0)
                    {
                        W = Hero.W;
                        A = -1;
                        W2 = -Hero.W;
                    }
                    if (Hero.PosX + Hero.W + W2 < Boss.X + Boss.W && Boss.Dir == "left") Boss.X -= 2;
                    else if (Hero.PosX + W > Boss.X + Boss.W && Boss.Dir == "right") Boss.X += 2;
                    else Flag++;
                    if (Hero.PosY + Hero.H > Boss.Y + Boss.H) Boss.Y++;
                    else if (Hero.PosY + Hero.H < Boss.Y + Boss.H) Boss.Y--;
                    else Flag++;

                    if (Flag != 2 && Boss.Count_Down > 7)
                    {
                        if (Boss.Current != Boss.Move) Boss.IF = 0;
                        Boss.Current = Boss.Move;
                    }
                    else if (Boss.Count_Down > 7)
                    {
                        if (Boss.Current != Boss.Fist) Boss.IF = 0;
                        Boss.Current = Boss.Fist;
                        Boss.Count_Down = 0;
                        HeroDamage(1);
                    }

                    if (Boss.Time_Count >= Boss.Time)
                    {
                        Boss.Action = "Idle";
                        Boss.IF = 0;
                        Boss.IF_C = 5;
                        Boss.Current = Boss.Idle;
                        Boss.CoolDown = 0;
                        Boss.IF_Counter = 0;
                        Boss.Count_Down = 0;
                    }
                }
                //Sword
                if (Boss.Action == "Sword")
                {
                    Boss.Time_Count++;
                    Boss.Count_Down++;
                    Boss.IF_C = 5;
                    int A = 1, W = 0, W2 = 0, Flag = 0;
                    if (Hero.W <= 0)
                    {
                        W = Hero.W;
                        A = -1;
                        W2 = -Hero.W;
                    }
                    if (Hero.PosX + Hero.W + W2 < Boss.X + Boss.W && Boss.Dir == "left") Boss.X -= 2;
                    else if (Hero.PosX + W > Boss.X + Boss.W && Boss.Dir == "right") Boss.X += 2;
                    else Flag++;
                    if (Hero.PosY + Hero.H > Boss.Y + Boss.H) Boss.Y++;
                    else if (Hero.PosY + Hero.H < Boss.Y + Boss.H) Boss.Y--;
                    else Flag++;

                    if (Flag != 2 && Boss.Count_Down > 7)
                    {
                        if (Boss.Current != Boss.Move) Boss.IF = 0;
                        Boss.Current = Boss.Move;
                    }
                    else if (Boss.Count_Down > 7)
                    {
                        if (Boss.Current != Boss.Sword) Boss.IF = 0;
                        Boss.Current = Boss.Sword;
                        Boss.Count_Down = 0;
                        HeroDamage(2);
                    }

                    if (Boss.Time_Count >= Boss.Time)
                    {
                        Boss.Action = "Idle";
                        Boss.IF = 0;
                        Boss.IF_C = 5;
                        Boss.Current = Boss.Idle;
                        Boss.CoolDown = 0;
                        Boss.IF_Counter = 0;
                        Boss.Count_Down = 0;
                    }
                }

                //Canonn
                if (Boss.Action == "Canonn")
                {
                    if ((Hero.PosY > Boss.Y && Hero.PosY < Boss.Y + Boss.H) || Boss.Current == Boss.Canonn)
                    {
                        if (Boss.Current != Boss.Canonn) Boss.IF = 0;
                        Boss.IF_C = 8;
                        Boss.Current = Boss.Canonn;
                        if (Boss.IF == 3 && Boss.CanonnX == -1)
                        {
                            if (Boss.Dir == "left")
                            {
                                Boss.CanonnW = -Boss.Canonn_Fire.Width;
                                Boss.CanonnX = Boss.X - 10 - Boss.Canonn_Fire.Width + Boss.CanonnW;
                            }
                            else
                            {
                                Boss.CanonnW = Boss.Canonn_Fire.Width;
                                Boss.CanonnX = Boss.X + Boss.W + 10;
                            }
                            Boss.CanonnY = Boss.Y + Boss.H - Boss.Canonn_Fire.Height;
                        }
                        else if (Boss.CanonnX != -1)
                        {
                            Boss.IF = 3;
                            BossHit(Boss.CanonnX, Boss.CanonnY, Boss.Canonn_Fire.Width, Boss.Canonn_Fire.Height, 1);
                        }
                        if (Boss.CanonnX > Boss.X && Boss.CanonnX != -1) Boss.CanonnX += 15;
                        else if (Boss.CanonnX != -1)
                        {
                            Boss.CanonnX -= 15;
                            if (Boss.CanonnX <= 0) Boss.CanonnX = 0;
                        }



                        if ((Boss.X < Boss.CanonnX - 600 || Boss.X > Boss.CanonnX + 600 || Boss.CanonnX <= 0 || Boss.CanonnX > LimitX) && Boss.CanonnX != -1)
                        {
                            Boss.CanonnX = -1;
                            Boss.CanonnY = -1;
                            Boss.Action = "Idle";
                            Boss.Current = Boss.Idle_Aura;
                            Boss.IF_Counter = 0;
                            Boss.IF_C = 5;
                            Boss.IF = 0;
                            Boss.CoolDown = 0;
                        }
                    }
                    else
                    {
                        Boss.Current = Boss.Move;
                        if (Boss.Y + Boss.H < Hero.H + Hero.PosY) Boss.Y += 2;
                        else Boss.Y -= 2;
                    }
                }
                //Blade
                if (Boss.Action == "Blade")
                {

                    if (Boss.Current != Boss.Blade) Boss.IF = 0;
                    Boss.IF_C = 8;
                    Boss.Current = Boss.Blade;
                    if (Boss.IF == 3 && Boss.BladeX == -1)
                    {
                        if (Boss.Dir == "left")
                        {
                            Boss.BladeW = -Boss.BladeEffect.Width / 2;
                            Boss.BladeX = Boss.X - 10;
                        }
                        else
                        {
                            Boss.BladeW = Boss.BladeEffect.Width;
                            Boss.BladeX = Boss.X + Boss.W + 10;
                        }
                        Boss.BladeH = (int)(Boss.BladeEffect.Height * 3);
                        Boss.BladeY = Boss.Y + Boss.H - Boss.BladeH;
                    }
                    else if (Boss.BladeX != -1)
                    {
                        Boss.IF = 3;
                        if (BossHit(Boss.BladeX, Boss.BladeY, Boss.BladeW, Boss.BladeH, 5))
                        {
                            Boss.BladeX = 0;
                        }
                    }
                    if (Boss.BladeX > Boss.X && Boss.BladeX != -1) Boss.BladeX += 25;
                    else if (Boss.BladeX != -1)
                    {
                        Boss.BladeX -= 25;
                        if (Boss.BladeX <= 0) Boss.BladeX = 0;
                    }
                    if ((Boss.X < Boss.BladeX - 800 || Boss.X > Boss.BladeX + 800 || Boss.BladeX <= 0 || Boss.BladeX > LimitX) && Boss.BladeX != -1)
                    {
                        Boss.BladeX = -1;
                        Boss.BladeY = -1;
                        Boss.Action = "Idle";
                        Boss.Current = Boss.Idle_Aura;
                        Boss.IF_Counter = 0;
                        Boss.IF_C = 5;
                        Boss.IF = 0;
                        Boss.CoolDown = 0;
                    }

                }
                //Teleport
                if (Boss.Action == "Teleport")
                {
                    Boss.Time_Count += 3;
                    Boss.Count_Down++;
                    Boss.IF_C = 5;
                    if (Boss.Current != Boss.Teleport) Boss.IF = 0;
                    Boss.Current = Boss.Teleport;
                    if (Boss.IF == 2 && Boss.TeleX == -1)
                    {
                        if (Hero.Dir == "right")
                        {
                            Boss.TeleW = Boss.Teleport_Effect.Width;
                            Boss.TeleX = Hero.PosX - 10;
                        }
                        else
                        {
                            Boss.TeleW = -Boss.Teleport_Effect.Width;
                            Boss.TeleX = Hero.PosX + Boss.BladeEffect.Width + Hero.W + 20;
                        }
                        Boss.TeleH = Boss.H;
                        Boss.TeleY = Boss.Y + Boss.H - Boss.TeleH;
                    }
                    if (Boss.TeleX != -1) Boss.IF = 2;
                    if (BossHit(Boss.TeleX, Boss.TeleY, Boss.TeleW, Boss.TeleH, 0) && Boss.Count_Down > 6 && Boss.TeleX != -1)
                    {
                        HeroDamage(2);
                        Boss.Count_Down = 0;
                    }

                    if (Boss.Time_Count >= Boss.Time)
                    {
                        Boss.Action = "Idle";
                        Boss.Time_Count = 0;
                        Boss.IF = 0;
                        Boss.IF_C = 5;
                        Boss.Current = Boss.Idle_Aura;
                        Boss.CoolDown = 0;
                        Boss.IF_Counter = 0;
                        Boss.Count_Down = 0;
                        Boss.TeleX = -1;
                    }
                }
                //End
                if (Boss.Status == "End")
                {
                    Boss.Count_Down++;
                    if (Boss.Count_Down > 16 && Fl == 1 && Alpha < 255)
                    {
                        StartX += RR.Next(4, 16);
                        StartY += RR.Next(4, 16);
                        Fl = 0;
                        Boss.Count_Down++;
                        Alpha += 4;
                    }
                    else if (Boss.Count_Down > 16 && Fl == 0 && Alpha < 255)
                    {
                        StartX -= RR.Next(4, 16);
                        StartY -= RR.Next(4, 16);
                        Alpha += 4;
                        Fl = 1;
                        Boss.Count_Down++;
                    }
                }

            }
            else if (Hero.PosX >= 4431 && Hero.PosX <= 4550 && Boss.X == -1)
            {
                Boss.X = 5278 + Boss.W;
            }
        }
        bool BossHit(int X,int Y,int W,int H,int Damage)
        {
            if (H <= 0) H *= -1;
            if (isHit(Hero.PosX, Hero.PosY, Hero.W, Hero.H,
                     X, Y, W, H)
                    || isHit(X, Y, W, H,
                        Hero.PosX, Hero.Y, Hero.W, Hero.H)
                    )
            {
                HeroDamage(Damage);
                return true;
            }
            return false;
        }
        void LaserMove()
        {
            if (Hero.Coins >= 2)
            {
                for (int i = 0; i < Map1.Laser.Count; i++)
                {
                    Obs pTrv = Map1.Laser[i];
                    pTrv.Counter++;
                    if (pTrv.IF + 1 == pTrv.imgs.Count)
                    {
                        Map1.Laser.RemoveAt(i);
                        i--;
                    }
                    else if(pTrv.Counter==5)
                    {
                        pTrv.IF = (pTrv.IF + 1) % pTrv.imgs.Count;
                        pTrv.Counter = 0;
                    }

                }
            }
            else
            {
                for (int i = 0; i < Map1.Laser.Count; i++)
                {
                    Obs pTrv = Map1.Laser[i];
                    if (isHit(Hero.PosX, Hero.PosY, Hero.W, Hero.H,
                     pTrv.X, pTrv.Y, pTrv.W, pTrv.H)
                    || isHit(pTrv.X, pTrv.Y, pTrv.W, pTrv.H,
                        Hero.PosX, Hero.PosY, Hero.W + Hero.H, Hero.H)
                    )
                    {
                        HeroDamage(1);
                    }
                }
            }
        }
        void CreatePoitions()
        {
            Obs Pnn = new Obs();
            Pnn.imgs.Add(new Bitmap("assets/assets/poitions/5.png"));
            Pnn.X = 2500;
            Pnn.Y = 419+BaseY;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Pnn.Type = 5;
            Map1.Poition.Add(Pnn);
        }
        void CreateObsticals()
        {
            Obs Pnn = new Obs();
            Pnn.X = 2105;
            Pnn.Y = 282+BaseY;
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
            Pnn.Y = 350+BaseY;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
            Pnn = new Obs();
            for (int i = 0; i <= 9; i++)
            {
                Pnn.imgs.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Pnn.X = 2103;
            Pnn.Y = 230+BaseY;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
            Pnn = new Obs();
            for (int i = 0; i <= 9; i++)
            {
                Pnn.imgs.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Pnn.X = 2103;
            Pnn.Y = 500+BaseY;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
            Pnn = new Obs();
            for (int i = 0; i <= 9; i++)
            {
                Pnn.imgs.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            Pnn.X = 3670;
            Pnn.Y = 419+BaseY;
            Pnn.W = Pnn.imgs[0].Width;
            Pnn.H = Pnn.imgs[0].Height;
            Map1.Coins.Add(Pnn);
        }
        void CreateHero()
        {
            for (int i = 80; i <= 89; i++)
            {
                Hero.Move.Add(new Bitmap("assets/Hero1/Right/" + i + ".png"));
            }
            for (int i = 140; i <= 149; i++)
            {
                Hero.Run.Add(new Bitmap("assets/Hero1/Right/" + i + ".png"));
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
                Hero.Dead.Add(new Bitmap("assets/Hero1/Right/" + i + ".png"));
            }
            for (int i = 0; i <= 9; i++)
            {
                Hero.Coin.Add(new Bitmap("assets/assets/coins/" + i + ".png"));
            }
            for (int i = 5; i >= 0; i--)
            {
                Hero.JumpDou.Add(new Bitmap("assets/Hero1/Right/Jump/" + i + ".png"));
            }
            for (int i = 5; i >= 3; i--)
            {
                Hero.Jump.Add(new Bitmap("assets/Hero1/Right/Jump/" + i + ".png"));
            }

            for (int i = 133; i <= 136; i++)
            {
                Hero.Bullet.Add(new Bitmap("assets/Hero1/Right/" + i + ".png"));
            }
            Hero.SBullett.MakeTransparent(Color.White);
            Hero.MBullett.MakeTransparent(Color.White);
            Hero.SBullet = false;
            Hero.Rig = false;
            Hero.Lef = false;
            Hero.U = false;
            Hero.Dow = false;
            Hero.SBulletX = -1;
            Hero.SBulletY = -1;
            LimitX = Map1.img.Width;
            LimitY1 = 574 + BaseY;
            LimitY2 = 700 + BaseY;
            Hero.PosX = 0;
            Hero.PosY = LimitY1;
            Hero.W = 150;
            Hero.H = 150;
            Hero.X = 0;
            Hero.Y = 574;
            Hero.Dir = "right";
            Hero.Current = Hero.Idle;
            Hero.HP = true;
            Hero.spd = 15;
            Hero.Coins = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateMap();
            StartX = 0; StartY = BaseY;
            ScaleX = (float)ClientSize.Width / (float)1403f;
            ScaleY = (float)ClientSize.Height / (float)(Map1.img.Height-BaseY);
            Alpha = 0;
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            Hero = new CHero();
            Boss = new Big_Boss();
            CreateHero(); 
            CreateBoss();

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
                            Hero.Current = Hero.Dead;
                            if (Hero.Dir == "right" && Hero.W < 0) Hero.W *= -1;
                            else if (Hero.Dir == "left" && Hero.W > 0) Hero.W *= -1 ;
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

            if (!Hero.Lef && !Hero.Rig && !Hero.Dow && !Hero.U&&Hero.HP && !Hero.SBullet && Hero.Jum==0)
            {
                Hero.Current = Hero.Idle;
                if ((Hero.Dir == "right" && Hero.W < 0)|| (Hero.Dir == "left" && Hero.W > 0))
                {
                    Hero.W *= -1;
                }
                Hero.IF = 0;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawBubb(CreateGraphics());

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Hero.HP && Boss.Status!="End")
            {

                if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.A) && !Hero.Elevator&&Hero.Jum==0) Hero.Lef = true;
                if ((e.KeyCode == Keys.Right || e.KeyCode == Keys.D) && !Hero.Elevator && Hero.Jum == 0) Hero.Rig = true;
                if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.S) && !Hero.Elevator && Hero.Jum == 0) Hero.Dow = true;
                if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.W) && !Hero.Elevator && Hero.Jum == 0) Hero.U = true;
                if (e.KeyCode == Keys.Space && !Hero.Elevator&&Hero.Jum!=2)
                {
                    Hero.IF = 0;
                    if (Hero.Jum == 0)
                    {
                        if (Hero.W < 0) Hero.J3 = -5;
                        else Hero.J3 = 5;
                        Hero.Jum = 1;
                        Hero.Current = Hero.Jump;
                    }
                    else if (Hero.Jum == 1)
                    {
                        Hero.Jum = 2;
                        Hero.Current = Hero.JumpDou;
                    }
                }
                
                if (e.KeyCode == Keys.ShiftKey && Hero.IF_Sprint < Hero.Sprint.Count - 1)
                {
                    Hero.Spt = true;
                    Hero.Sprt = 15;
                }
                if (e.KeyCode == Keys.B&&Hero.SBulletX ==-1) //Single Bullet
                {
                    Hero.SBullet = true;
                    Hero.SBulletY = Hero.PosY + Hero.H / 2;
                    Hero.IF = 0;
                    Hero.Current = Hero.Bullet;
                    if (Hero.Dir == "right")
                    {
                        Hero.SBulletX = Hero.PosX + Hero.W + 10;
                    }
                    else
                    {
                        Hero.SBulletX = Hero.PosX -10;
                    }
                }


                if (e.KeyCode == Keys.Y &&
                    (isHit(Hero.PosX, Hero.PosY, Hero.W, Hero.H,
                     Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H)
                    || isHit(Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H,
                        Hero.PosX, Hero.PosY, Hero.W, Hero.H)
                    ))
                {
                    Map1.Elevator.IF = 2;
                    Map1.Elevator.spd = -3;
                    Hero.PosY = Map1.Elevator.Y + Map1.Elevator.H - Hero.H;
                    Hero.Elevator = true;
                }
                if (e.KeyCode == Keys.U &&
                    (isHit(Hero.PosX, Hero.PosY, Hero.W, Hero.H,
                     Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H)
                    || isHit(Map1.Elevator.X, Map1.Elevator.Y, Map1.Elevator.W, Map1.Elevator.H,
                        Hero.PosX, Hero.PosY, Hero.W + Hero.H, Hero.H)
                    ))
                {
                    Map1.Elevator.IF = 2;
                    Map1.Elevator.spd = 3;
                    if (Map1.Elevator.Y <= 0) Map1.Elevator.Y = 1;
                    Hero.PosY = Map1.Elevator.Y + Map1.Elevator.H - Hero.H;
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
            if (W1 < 0) W1 *= -1;
            if (W2 < 0) W2 *= -1;
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
            DrawBoss(g);

            if(Hero.HP && Boss.Status == "End")
            {
                if (Alpha < 255)
                {
                    Brush A = new SolidBrush(Color.FromArgb(Alpha, 0, 0, 0));
                    g.FillRectangle(A, 0, 0, ClientSize.Width, ClientSize.Height);
                }
                else
                {
                    Brush A = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
                    g.FillRectangle(A, 0, 0, ClientSize.Width, ClientSize.Height);
                    g.DrawString("OVERLORD PURGED", new Font("Impact", 50, FontStyle.Bold), Brushes.Gold, 100, 100);
                    g.DrawString("The anomaly has stabilized. The world is safe... for now.", new Font("Arial", 16, FontStyle.Regular), Brushes.White, 100, 210);
                    g.DrawString("PROJECT MEMBERS:", new Font("Arial", 14, FontStyle.Bold), Brushes.DeepSkyBlue, 100, 300);
                    g.DrawString("- Omar", new Font("Arial", 14, FontStyle.Regular), Brushes.White, 120, 335);
                    g.DrawString("- Zahretalola", new Font("Arial", 14, FontStyle.Regular), Brushes.White, 120, 370);
                    g.DrawString("Press [R] to Restart", new Font("Courier New", 14, FontStyle.Bold), Brushes.LightGray, 100, 480);
                }
            }
            else if(!Hero.HP)
            {
                g.DrawString("YOU DEAD", new Font("Arial", 36, FontStyle.Bold), Brushes.Red, ClientSize.Width / 2 - 40, ClientSize.Height / 2 - 30);
                g.DrawString("Press [R] to Restart", new Font("Arial", 36, FontStyle.Bold), Brushes.Red, ClientSize.Width / 2 - 80, ClientSize.Height / 2 + 40);
            }
        }
        void DrawHero(Graphics g)
        {
            //Draw Hearts
            for (int i = 0; i < 6; i++)
            {
                g.DrawImage(Hero.Heart[i][Hero.IF_Heart[i]], (60 * i)*ScaleX, 0, 60, 60);
            }
            //Draw Sprint
            g.DrawImage(Hero.Sprint[Hero.IF_Sprint], 0, 70, Hero.Sprint[0].Width, Hero.Sprint[0].Height);
            //Draw Coins Number
            Hero.IF_Coins = (Hero.IF_Coins + 1) % Hero.Coin.Count;
            g.DrawString("" + Hero.Coins, new Font("Arial",40), Brushes.White, ClientSize.Width - 50, 10);
            g.DrawImage(Hero.Coin[Hero.IF_Coins], (ClientSize.Width - 100), 15);
            if (Hero.SBulletX != -1) g.DrawImage(Hero.SBullett, Hero.SBulletX - StartX, Hero.SBulletY * ScaleY, 60, 60);
            if (!Hero.Elevator) g.DrawImage(Hero.Current[Hero.IF], Hero.X*ScaleX, Hero.Y * ScaleY, Hero.W, Hero.H * ScaleY);
            for (int i = 0; i < Hero.MBulletX.Count; i++)
            {
                int Neg = 1;
                if (Hero.MBulletX[i] < Hero.PosX) Neg = -1;
                g.DrawImage(Hero.MBullett, Hero.MBulletX[i] - StartX, Hero.MBulletY[i] * ScaleY, 60* Neg, 60);
            }
        }
        void DrawBoss(Graphics g)
        {
            if (Boss.X != -1)
            {
                //Draw Heatlh Bar Of Boss
                g.DrawImage(Boss.CurrentHealth[Boss.IF_Health], (ClientSize.Width / 2) - Boss.CurrentHealth[0].Width/2, 2 * ScaleY);

                //Draw Boss
                g.DrawImage(Boss.Current[Boss.IF], (Boss.X - StartX) * ScaleX, Boss.Y * ScaleY, Boss.W, Boss.H*ScaleY);

                //Draw Canonn
                if (Boss.CanonnX != -1) g.DrawImage(Boss.Canonn_Fire, (Boss.CanonnX - StartX) * ScaleX, Boss.CanonnY * ScaleY, Boss.CanonnW, Boss.Canonn_Fire.Height * ScaleY);
                //Draw Blade
                if (Boss.BladeX != -1) g.DrawImage(Boss.BladeEffect, (Boss.BladeX - StartX) * ScaleX, Boss.BladeY * ScaleY, Boss.BladeW, Boss.BladeH * ScaleY);
                //Draw Teleport Effect
                if (Boss.TeleX!= -1) g.DrawImage(Boss.Teleport_Effect, (Boss.TeleX- StartX) * ScaleX, Boss.TeleY* ScaleY, Boss.TeleW, Boss.TeleH* ScaleY);
            }
        }
        void DrawMap(Graphics g)
        {
            Map1.Dst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Map1.Src = new Rectangle(StartX, StartY, 1403, Map1.img.Height-BaseY);
            g.DrawImage(Map1.img, Map1.Dst, Map1.Src, GraphicsUnit.Pixel);
            //Elevator
            g.DrawImage(Map1.Elevator.imgs[Map1.Elevator.IF], (Map1.Elevator.X - StartX)*ScaleX, (Map1.Elevator.Y - StartY) * ScaleY, Map1.Elevator.W, Map1.Elevator.H * ScaleY);
            //Draw Coins
            for (int i = 0; i < Map1.Coins.Count; i++)
            {
                Obs pTrv = Map1.Coins[i];
                g.DrawImage(pTrv.imgs[pTrv.IF], (pTrv.X-StartX)*ScaleX, (pTrv.Y-StartY)*ScaleY, pTrv.W, pTrv.H*ScaleY);
            }
            //Draw Poitions
            for (int i = 0; i < Map1.Poition.Count; i++)
            {
                Obs pTrv = Map1.Poition[i];
                g.DrawImage(pTrv.imgs[pTrv.IF], (pTrv.X-StartX)*ScaleX, (pTrv.Y - StartY) * ScaleY, pTrv.W, pTrv.H*ScaleY);
            }
            //Draw Laser
            for (int i = 0; i < Map1.Laser.Count; i++)
            {
                Obs pTrv = Map1.Laser[i];
                g.DrawImage(pTrv.imgs[pTrv.IF], (pTrv.X - StartX)*ScaleX, pTrv.Y * ScaleY, pTrv.W, pTrv.H * ScaleY);
            }
            //Draw Plane
            if (Plane.X >= 0) g.DrawImage(Plane.img, (Plane.X - StartX) * ScaleX, Plane.Y, Plane.W, Plane.H*ScaleY);

            //Draw Enemy3
            for (int i = 0; i < Enemy2.Count; i++)
            {
                Enemy pTrv = Enemy2[i];
                g.DrawImage(pTrv.Current[pTrv.IF], (pTrv.X - StartX), pTrv.Y * ScaleY, pTrv.W, pTrv.H * ScaleY);
                for (int i2 = 0; i2 < pTrv.FireX.Count; i2++)
                {
                    int A = 1;
                    if (pTrv.FireX[i2] < pTrv.X) A = -1;
                    g.DrawImage(pTrv.Bullet, pTrv.FireX[i2]-StartX, pTrv.FireY[i2]*ScaleY, pTrv.Bullet.Width*A, pTrv.Bullet.Height*ScaleY);
                }
            }



        }
    }
}
