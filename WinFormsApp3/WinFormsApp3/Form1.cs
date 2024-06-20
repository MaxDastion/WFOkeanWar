namespace WinFormsApp3
{



    public partial class Form1 : Form
    {

        Func func = new Func();
        public Form1()
        {
            InitializeComponent();
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

        }

        void Rempfn(object sende, EventArgs e)
        {
            Point point = new Point();
            point.X = tableLayoutPanel3.GetColumn((Control)sende);
            point.Y = tableLayoutPanel3.GetRow((Control)sende);
            if (func.FN(point))
            {
                Control c = tableLayoutPanel3.GetControlFromPosition(point.X, point.Y);
                c.BackColor = Color.Black;
            }
        }
        void Smouth(object sende, EventArgs e)
        {


        }
    }



    class Func
    {
        public List<Ship> ships = new List<Ship>();
        private int indexShipa = 0;

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