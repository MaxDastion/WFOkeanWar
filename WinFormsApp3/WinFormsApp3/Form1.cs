﻿using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace WinFormsApp3
{



    public partial class Form1 : Form
    {
        TcpClient tcpClient = new TcpClient();
        bool move = new bool();

        ShipsPlacemant func = new ShipsPlacemant();
        string server = string.Empty; int port;
        public Form1()
        {
            InitializeComponent();
            tcpClient.ConnectAsync(IPAddress.Parse("127.0.0.1"), 9010);

            foreach (Button item in tableLayoutPanel3.Controls)
            {
                item.MouseClick += new MouseEventHandler(Rempfn);
            }
            for (int i = 0; i < 10; i++)
            {
                if (i <= 3)
                {
                    OneShip ship = new OneShip();
                    func.ships.Add(ship);
                }
                else if (i <= 6 && i >= 4)
                {
                    DoubleShip ship = new DoubleShip();
                    func.ships.Add(ship);
                }
                else if (i < 9 && i >= 7)
                {
                    TripleShip ship = new TripleShip();
                    func.ships.Add(ship);
                }
                else
                {
                    FourthShip ship = new FourthShip();
                    func.ships.Add(ship);
                }
            }
            foreach (Button item in tableLayoutPanel2.Controls)
            {
                item.Enabled = false;
                item.MouseClick += new MouseEventHandler(AttackMouseClick);
            }
        }


        private void MoveDistributor()
        {
            while (true)
            {
                if (move == true)
                {
                    foreach (Button item in tableLayoutPanel2.Controls)
                    {
                        if (item.BackColor != Color.Red  )
                                if (item.BackColor != Color.LightGreen)
                                    item.Enabled = true;
                        

                        
                    }
                    break;
                }
                else
                {
                    foreach (Button item in tableLayoutPanel2.Controls)
                    {
                        item.Enabled = false;
                    }
                    EnemyMove();
                }
            }

        }



        void WhoMoveFirst()
        {
            var stream = tcpClient.GetStream();
            List<byte> bytes = new List<byte>();
            int bytesRead = 0;

            while ((bytesRead = stream.ReadByte()) != '\0')
            {
                bytes.Add((byte)bytesRead);
            }
            bytes.Add((byte)'\0');

            string str = Encoding.UTF8.GetString(bytes.ToArray());

            bool nigga = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(str);

            if (nigga == true)
            {
                move = true;
                foreach (Button item in tableLayoutPanel2.Controls)
                {
                    if (item.BackColor != Color.Red || item.BackColor != Color.LightGreen)
                        item.Enabled = true;
                }
            }
            else
            {
                move = false;
                foreach (Button item in tableLayoutPanel2.Controls)
                {
                    item.Enabled = false;
                }
            }
        }

        void Rempfn(object sende, EventArgs e)
        {
            int indexShipaTemp = -1;
            Point point = new Point();
            point.X = tableLayoutPanel3.GetColumn((Control)sende);
            point.Y = tableLayoutPanel3.GetRow((Control)sende);
            if (func.FN(point))
            {
                Button c = (Button)tableLayoutPanel3.GetControlFromPosition(point.X, point.Y);
                c.BackColor = Color.Black;
                c.Enabled = false;
                if (indexShipaTemp != func.indexShipa - 1)
                {
                    indexShipaTemp = func.indexShipa - 1;
                    checke(func.ships[indexShipaTemp]);
                }
                if (func.indexShipa >= 0 && func.indexShipa <= 3)
                {
                    label1.Text = "Сейчас вы устанавливате единичный корабль";
                }
                else if (func.indexShipa >= 4 && func.indexShipa <= 6)
                {
                    label1.Text = "Сейчас вы устанавливате двойной корабль";
                }
                else if (func.indexShipa == 7 || func.indexShipa == 8)
                {
                    label1.Text = "Сейчас вы устанавливате тройной корабль";
                }
                else if (func.indexShipa == 9)
                {
                    label1.Text = "Сейчас вы устанавливаете четверной корабль";
                }
                else if (func.indexShipa == 10)
                {
                    label1.Text = string.Empty;
                    foreach (Button item in tableLayoutPanel3.Controls)
                    {
                        item.Enabled = false;
                    }
                    WhoMoveFirst();
                    MoveDistributor();
                }
            }
        }

        void AttackMouseClick(object sende, MouseEventArgs e)
        {
            Point point = new Point();
            point.X = tableLayoutPanel2.GetColumn((Control)sende);
            point.Y = tableLayoutPanel2.GetRow((Control)sende);
            NetworkStream stream = tcpClient.GetStream();

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(point) + '\0';
            byte[] data = Encoding.UTF8.GetBytes(json);

            stream.WriteAsync(data);

            List<byte> bytes = new List<byte>();
            int bytesRead = 0;

            while ((bytesRead = stream.ReadByte()) != '\0')
            {
                bytes.Add((byte)bytesRead);
            }
            bytes.Add((byte)'\0');

            string str = Encoding.UTF8.GetString(bytes.ToArray());

            bool HitOrNot = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(str);
            Button button = (Button)sende;

            if (HitOrNot == true)
            {
                button.BackColor = Color.Red;
                button.Enabled= false;
            }
            else
            {
                button.BackColor = Color.LightGreen;
                button.Enabled = false;
            }

            WhoMove(HitOrNot);
            MoveDistributor();
        }



        private void WhoMove(bool HitOrNot)
        {
            if (HitOrNot == true)
            {

            }
            else
            {
                if (move == true)
                {
                    move = false;
                }
                else
                {
                    move = true;
                }
            }
        }

        public async void EnemyMove()
        {
            var stream = tcpClient.GetStream();
            List<byte> bytes = new List<byte>();
            int bytesRead = 0;

            while ((bytesRead = stream.ReadByte()) != '\0')
            {
                bytes.Add((byte)bytesRead);
            }
            bytes.Add((byte)'\0');

            string str = Encoding.UTF8.GetString(bytes.ToArray());
            Point point = Newtonsoft.Json.JsonConvert.DeserializeObject<Point>(str);

            Button c = (Button)tableLayoutPanel3.GetControlFromPosition(point.X, point.Y);
            bool HitOrNot = false;
            if (c.BackColor == Color.Black)
            {
                HitOrNot = true;
                c.BackColor = Color.Red;
            }
            else
            {
                c.BackColor = Color.LightGreen;
            }

            string jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(HitOrNot);

            if (jsonUser != null)
            {
                jsonUser += '\0';
                byte[] bytee = Encoding.UTF8.GetBytes(jsonUser);
                stream.WriteAsync(bytee);
            }

            WhoMove(HitOrNot);
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {

        }
        private void checke(Ship ship)
        {
            int x = 20;
            int y = 20;

            for (int i = 0; i < ship.buttons.Count; i++)
            {
                if (ship.buttons[i].X - 1 < x)
                {
                    x = ship.buttons[i].X - 1;
                }

                if (ship.buttons[i].Y - 1 < y)
                {
                    y = ship.buttons[i].Y - 1;
                }
            }
            int xcount = 0;
            int ycount = 0;
            if (ship.direction == Ship.ShipDirection.Horizontal) { xcount = ship.buttons.Count + 2; ycount = 3; }
            else { ycount = ship.buttons.Count + 2; xcount = 3; }

            for (int i = y; i < y + ycount; i++)
            {
                for (int j = x; j < x + xcount; j++)
                {
                    if (i < 0 || j < 0 || j >= 10 || i >= 10)
                    {
                        continue;
                    }

                    Button button = (Button)tableLayoutPanel3.GetControlFromPosition(j, i);
                    button.Enabled = false;
                }
            }

        }

    }





    class ShipsPlacemant
    {
        public List<Ship> ships = new List<Ship>();
        public int indexShipa = 0;

        public bool FN(Point point)
        {
            
            if (ships[indexShipa].buttons.Count == 0)
            {
                ships[indexShipa].buttons.Add(point);
            }
            else if (check(point))
            {
                if (ships[indexShipa].count >= 2)
                {
                    if (ships[indexShipa].buttons.Count == 1)
                    {
                        for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
                        {
                            if ((point.X == ships[indexShipa].buttons[i].X + 1) && (point.Y == ships[indexShipa].buttons[i].Y))
                            {
                                ships[indexShipa].direction = Ship.ShipDirection.Horizontal; break;
                            }
                            if ((point.X == ships[indexShipa].buttons[i].X - 1) && (point.Y == ships[indexShipa].buttons[i].Y))
                            {
                                ships[indexShipa].direction = Ship.ShipDirection.Horizontal; break;

                            }
                            if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y - 1))
                            {
                                ships[indexShipa].direction = Ship.ShipDirection.Vertical; break;
                            }
                            if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y + 1))
                            {
                                ships[indexShipa].direction = Ship.ShipDirection.Vertical; break;
                            }
                        }
                    }
                    else
                    {
                        if (checkDirection(point))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Вы ввели неверную позицию");
                            return false;
                        }
                    }
                }
                ships[indexShipa].buttons.Add(point);
            }
            else
            {
                MessageBox.Show("Вы ввели неверную позицию 2");
                return false;
            }



            if (ships[indexShipa].buttons.Count == ships[indexShipa].count)
            {
                MessageBox.Show("Корабль успешно установлен");
                indexShipa++;
                if (indexShipa >= ships.Count)
                {
                    return true;
                }
            }
            return true;
        }

        public bool checkDirection(Point point)
        {

            if (ships[indexShipa].direction == Ship.ShipDirection.Vertical)
            {
                for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
                {

                    if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y + 1))
                    {
                        return true;
                    }
                    if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y - 1))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
                {


                    if ((point.X + 1 == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                    {
                        return true;
                    }
                    if ((point.X - 1 == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                    {
                        return true;
                    }

                }
            }
            return false;

        }

        public bool check(Point point)
        {
            for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
            {
                if ((point.X + 1 == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                {
                    return true;
                }
                if ((point.X - 1 == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                {
                    return true;
                }
                if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y + 1))
                {
                    return true;
                }
                if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y - 1))
                {
                    return true;
                }
            }


            return false;
        }

        



    }

    




    #region Ships        
    class Ship
    {
        public List<Point> buttons = new List<Point>();
        public int count;
        public int damage = 0;
        public void Add(Point but)
        {
            buttons.Add(but);
        }
        public enum ShipDirection { Vertical, Horizontal };
        public ShipDirection direction = ShipDirection.Vertical;
    }


    class OneShip : Ship
    {
        public OneShip(int count = 1) { this.count = count; }
    }

    class DoubleShip : Ship
    {
        public DoubleShip(int count = 2) { this.count = count; }
    }

    class TripleShip : Ship
    {
        public TripleShip(int count = 3) { this.count = count; }
    }

    class FourthShip : Ship
    {
        public FourthShip(int count = 4) { this.count = count; }
    }
    #endregion



}
