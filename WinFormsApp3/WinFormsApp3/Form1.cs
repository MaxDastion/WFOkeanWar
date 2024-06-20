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
                else if (i <= 9 && i >= 7)
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

        void Rempfn(object sende, EventArgs e) {
            Point point = new Point();
          point.X = tableLayoutPanel3.GetColumn((Control)sende);
            point.Y = tableLayoutPanel3.GetRow((Control)sende);
            func.FN(point);
        }
        void Smouth(object sende, EventArgs e)
        {


        }
    }



    class Func
    {
        public List<Ship> ships = new List<Ship>();
        private int indexShipa = 0;




        public void FN(Point point)
        {

            if (ships[indexShipa].count == 1)
            {
                ships[indexShipa].buttons.Add(point);
                MessageBox.Show("This dileshes");
            }
            else if (check(point))
            {
                if (ships[indexShipa].count >= 2)
                {
                    for (int f = 0; f < indexShipa; f++)
                    {
                        if (ships[f].count == 1)
                        {
                            for (int i = 0; i < ships[f].count; i++)
                            {
                                if ((point.X+1 == ships[f].buttons[i].X) && (point.Y == ships[f].buttons[i].Y))
                                {
                                    ships[indexShipa].direction = Ship.ShipDirection.Horizontal; break;continue;
                                }
                                if ((point.X-1 == ships[f].buttons[i].X) && (point.Y == ships[f].buttons[i].Y))
                                {
                                    ships[indexShipa].direction = Ship.ShipDirection.Horizontal; break;continue;
                                }
                                if ((point.X == ships[f].buttons[i].X) && (point.Y+1 == ships[f].buttons[i].Y))
                                {
                                    ships[indexShipa].direction = Ship.ShipDirection.Vertical; break;continue;
                                }
                                if ((point.X == ships[f].buttons[i].X) && (point.Y-1 == ships[f].buttons[i].Y))
                                {
                                    ships[indexShipa].direction = Ship.ShipDirection.Vertical; break;continue;
                                }
                            }
                        }
                    }
                            MessageBox.Show("Im ok comrads))");
                }
                else
                {
                    if (!checkDirection(point))
                    {
                        MessageBox.Show("Fuck you");
                    }

                }
                
                ships[indexShipa].buttons.Add(point);
            }
            else
            {
                MessageBox.Show("Cold down");
            }



            if (ships[indexShipa].buttons.Count == ships[indexShipa].count)
            {
                MessageBox.Show(":)");
                indexShipa++;
                if (indexShipa >= ships.Count)
                {
                    return;
                }
            }



        }

        public bool checkDirection(Point point)
        {
            for (int f = 0; f < indexShipa; f++)
            {


                if (ships[f].direction == Ship.ShipDirection.Vertical)
                {
                    for (int i = 0; i < ships[f].buttons.Count; i++)
                    {

                        if ((point.X == ships[f].buttons[i].X) && (point.Y+1 == ships[f].buttons[i].Y))
                        {
                            return true;
                        }
                        if ((point.X == ships[f].buttons[i].X) && (point.Y-1 == ships[f].buttons[i].Y))
                        {
                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    for (int i = 0; i < ships[f].buttons.Count; i++)
                    {


                        if ((point.X+1 == ships[f].buttons[i].X) && (point.Y == ships[f].buttons[i].Y))
                        {
                            return true;
                        }
                        if ((point.X-1 == ships[f].buttons[i].X) && (point.Y == ships[f].buttons[i].Y))
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
                return false;
            
        }

        public bool check(Point point)
        {
            int temp = 0;
            for (int f = 0; f < indexShipa; f++)
            {


                for (int i = 0; i < ships[f].buttons.Count; i++)
                {
                    if ((point.X+1 != ships[f].buttons[i].X) && (point.Y != ships[f].buttons[i].Y))
                    {
                        temp++;
                    }
                    if ((point.X-1 != ships[f].buttons[i].X) && (point.Y != ships[f].buttons[i].Y))
                    {
                        temp++;
                    }
                    if ((point.X != ships[f].buttons[i].X) && (point.Y + 1 != ships[f].buttons[i].Y))
                    {

                        temp++;
                    }
                    if ((point.X != ships[f].buttons[i].X) && (point.Y - 1 != ships[f].buttons[i].Y))
                    {
                            temp++;
                    }
                }
                if (temp != 4)
                {
                    return true;
                }
                else
                {
                    return false;
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