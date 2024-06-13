namespace WinFormsApp1
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

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
            }                
            else if (check(point))
            {
                if (ships[indexShipa].count > 2)
                {
                    if (ships[indexShipa].buttons.Count == 1)
                    {
                        for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
                        {
                            if ((point.X++ == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                            {
                                ships[indexShipa].direction = Ship.ShipDirection.Horizontal; break;
                            }
                            if ((point.X-- == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                            {
                                ships[indexShipa].direction = Ship.ShipDirection.Horizontal; break;

                            }
                            if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y++ == ships[indexShipa].buttons[i].Y))
                            {
                                ships[indexShipa].direction = Ship.ShipDirection.Vertical; break;
                            }
                            if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y-- == ships[indexShipa].buttons[i].Y))
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
                        }
                    }
                }
                ships[indexShipa].buttons.Add(point);
            }
            else 
            {
                MessageBox.Show("Вы ввели неверную позицию");   
            }



            if (ships[indexShipa].buttons.Count == ships[indexShipa].count)
            {
                indexShipa++;
                if (indexShipa >= ships.Count)
                {
                    return;
                }
            }



        }

        public bool checkDirection(Point point)
        {
            if (ships[indexShipa].direction == Ship.ShipDirection.Vertical)
            {
                for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
                {

                    if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y++ == ships[indexShipa].buttons[i].Y))
                    {
                        return true;
                    }
                    if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y-- == ships[indexShipa].buttons[i].Y))
                    {
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
                {


                    if ((point.X++ == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                    {
                        return true;
                    }
                    if ((point.X-- == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        } 

        public bool check(Point point)
        {
            for (int i = 0; i < ships[indexShipa].buttons.Count; i++)
            {
                if ((point.X++ == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                {
                    return true;
                }
                if ((point.X-- == ships[indexShipa].buttons[i].X) && (point.Y == ships[indexShipa].buttons[i].Y))
                {
                    return true;
                }
                if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y++ == ships[indexShipa].buttons[i].Y))
                {
                    return true;
                }
                if ((point.X == ships[indexShipa].buttons[i].X) && (point.Y-- == ships[indexShipa].buttons[i].Y))
                {
                    return true;
                }
            }


            return false;
        }
    }

}







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
    public FourthShip(int count = 4 ) { this.count = count; }
}