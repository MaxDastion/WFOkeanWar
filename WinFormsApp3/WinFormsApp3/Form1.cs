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
            tcpClient.ConnectAsync(IPAddress.Parse("192.168.89.189"), 9010);
            WhoMoveFirst();

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

            bool nigga= Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(str);

            if (nigga == true)
            {
                move = true;
                foreach (Button item in tableLayoutPanel2.Controls)
                {
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
            Point point = new Point();
            point.X = tableLayoutPanel3.GetColumn((Control)sende);
            point.Y = tableLayoutPanel3.GetRow((Control)sende);
            if (func.FN(point))
            {
                Button c = (Button)tableLayoutPanel3.GetControlFromPosition(point.X, point.Y);
                c.BackColor = Color.Black;
                c.Enabled = false;
                if (func.indexShipa == 10)
                {
                    foreach (Button item in tableLayoutPanel3.Controls)
                    {
                        item.Enabled = false;
                    }

                }
            }
        }

        void AttackMouseClick(object sende, MouseEventArgs e)
        {
            Point point = new Point();
            point.X = tableLayoutPanel2.GetColumn((Control)sende);
            point.Y = tableLayoutPanel2.GetRow((Control)sende);
            NetworkStream stream = tcpClient.GetStream();

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(point);
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


            Button button = (Button)tableLayoutPanel2.GetControlFromPosition(point.X, point.Y);
            button.Enabled = false;
            if (HitOrNot == true)
            {
                button.BackColor = Color.Black;
            }
            else
            {
                button.BackColor = Color.Red;
            }


            WhoMove(HitOrNot);


            
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
                    foreach (Button item in tableLayoutPanel2.Controls)
                    {
                        item.Enabled = false;
                    }
                }
                else
                {
                    move = true;
                    foreach (Button item in tableLayoutPanel2.Controls)
                    {
                        item.Enabled = true;
                    }
                }
            }
        }
        public void EnemyMove(TcpClient client)
        {
            var stream = client.GetStream();
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
            }

            string jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(HitOrNot);

            if (jsonUser != null)
            {
                jsonUser += '\0';
                byte[] bytee = Encoding.UTF8.GetBytes(jsonUser);
                stream.WriteAsync(bytee);
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
                if (ships[indexShipa].count > 2)
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
                MessageBox.Show("Вы ввели неверную позицию2");
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
