using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Minesweeper
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static BitmapImage blank = new BitmapImage();
        static BitmapImage logo = new BitmapImage();
        static BitmapImage mine = new BitmapImage();
        static BitmapImage mineClicked = new BitmapImage();
        static BitmapImage mineDisabled = new BitmapImage();
        static BitmapImage marked = new BitmapImage();
        static BitmapImage box1 = new BitmapImage();
        static BitmapImage box2 = new BitmapImage();
        static BitmapImage box3 = new BitmapImage();
        static BitmapImage box4 = new BitmapImage();
        static BitmapImage box5 = new BitmapImage();
        static BitmapImage box6 = new BitmapImage();
        static BitmapImage box7 = new BitmapImage();
        static BitmapImage box8 = new BitmapImage();

        static bool running = false;

        public MainWindow()
        {
            InitializeComponent();

            logo.BeginInit();
            logo.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\2000px-Minesweeper_unopened_square.svg.png");
            logo.EndInit();

            blank.BeginInit();
            blank.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\76px-Minesweeper_0.svg.png");
            blank.EndInit();

            mine.BeginInit();
            mine.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\mine.png");
            mine.EndInit();

            mineClicked.BeginInit();
            mineClicked.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\mineExploded.png");
            mineClicked.EndInit();

            mineDisabled.BeginInit();
            mineDisabled.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\flagIncorrect.png");
            mineDisabled.EndInit();

            marked.BeginInit();
            marked.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\2000px-Minesweeper_flag.svg.png");
            marked.EndInit();

            box1.BeginInit();
            box1.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box1.png");
            box1.EndInit();

            box2.BeginInit();
            box2.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box2.png");
            box2.EndInit();

            box3.BeginInit();
            box3.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box3.png");
            box3.EndInit();

            box4.BeginInit();
            box4.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box4.png");
            box4.EndInit();

            box5.BeginInit();
            box5.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box5.png");
            box5.EndInit();

            box6.BeginInit();
            box6.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box6.png");
            box6.EndInit();

            box7.BeginInit();
            box7.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box7.png");
            box7.EndInit();

            box8.BeginInit();
            box8.UriSource = new Uri(@"C:\Users\user\source\repos\Minesweeper-master\Minesweeper\Resources\box8.png");
            box8.EndInit();

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void DeleteElement(UIElement e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Rows.Text) && !string.IsNullOrWhiteSpace(Columns.Text))
            {
                int _rows = Int32.Parse(Rows.Text);
                int _columns = Int32.Parse(Columns.Text);

                grid.Children.Clear();
                Generate(_rows, _columns);
            }
        }

        public void Generate(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                RowDefinition _rowDef = new RowDefinition();
                grid.RowDefinitions.Add(_rowDef);
            }

            for (int i = 0; i < columns; i++)
            {
                ColumnDefinition _colDef = new ColumnDefinition();
                grid.ColumnDefinitions.Add(_colDef);
            }

            Console.WriteLine("COLUMNS " + grid.ColumnDefinitions.Count);
            Console.WriteLine("ROWS " + grid.RowDefinitions.Count);

            grid.Width = 0;
            grid.Height = 0;
            System.Windows.Application.Current.MainWindow.Height = 0;
            System.Windows.Application.Current.MainWindow.Width = 0;

            Random _rnd = new Random();

            for (int i = 0; i < rows; i++)
            {
                for (int o = 0; o < columns; o++)
                {
                    {
                        int _random = _rnd.Next(1, 5);
                        Image rect = new Image();
                        Grid.SetColumn(rect, i);
                        Grid.SetRow(rect, o);
                        rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        rect.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                        if (_random == 1)
                        {
                            rect.Uid = "mine";
                        }
                        else
                        {
                            rect.Uid = "blank";
                        }
                        rect.MouseLeftButtonDown += new MouseButtonEventHandler(Check);
                        rect.MouseRightButtonDown += new MouseButtonEventHandler(RightCheck);
                        rect.Source = logo;
                        grid.Children.Add(rect);
                    }
                }
                grid.Width += 40;
                grid.Height += 40;
                System.Windows.Application.Current.MainWindow.Height += 40;
                System.Windows.Application.Current.MainWindow.Width += 40;
            }

            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        public void Check(object sender, MouseButtonEventArgs e)
        {
            string _uid = ((Image)sender).Uid;
            if (_uid != "mineDisabled" || _uid != "blankDisabled")
            {
                if (_uid == "blank")
                {
                    int _row;
                    int _column;
                    if (((Image)sender).Source != blank)
                    {
                        ((Image)sender).Source = blank;
                        _row = Grid.GetRow(((Image)sender));
                        _column = Grid.GetColumn(((Image)sender));

                    }
                    else
                    {
                        _row = Grid.GetRow(((Image)sender));
                        _column = Grid.GetColumn(((Image)sender));
                    }

                    Console.WriteLine(_row);
                    Console.WriteLine(_column);

                    List<Image> tiles = new List<Image>();

                    for (int i = 0; i < 8; i++)
                    {
                        GetGridElement(grid, _row, _column);
                    }
                }

                if (_uid == "mine")
                {
                    Console.WriteLine("You Lost");
                    foreach (Image i in grid.Children)
                    {
                        if (i.Uid == "mine")
                        {
                            i.Source = mine;
                            if (i == ((Image)sender))
                            {
                                i.Source = mineClicked;
                            }
                        }

                        else if (i.Uid == "mineDisabled")
                        {
                            i.Source = mineDisabled;
                        }

                        else if (i.Uid == "blankDisabled")
                        {
                            i.Source = logo;
                        }
                    }
                }
            }
        }

        public void RightCheck(object sender, MouseButtonEventArgs e)
        {

            string _uid = ((Image)sender).Uid;
            if (_uid == "mine")
            {
                ((Image)sender).Uid = "mineDisabled";
                ((Image)sender).Source = marked;
            }

            else if (_uid == "blank")
            {
                ((Image)sender).Uid = "blankDisabled";
                ((Image)sender).Source = marked;
            }

            else if (_uid == "mineDisabled")
            {
                ((Image)sender).Uid = "mine";
                ((Image)sender).Source = logo;
            }

            else if (_uid == "blankDisabled")
            {
                ((Image)sender).Uid = "blank";
                ((Image)sender).Source = logo;
            }

        }

        int countBombs(int r, int c)
        {
            int bombCounter = 0;

            for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement e = grid.Children[i];
                Image img = (Image)e;
                if (Grid.GetRow(e) == r + 1 && Grid.GetColumn(e) == c)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
                else if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c + 1)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
                else if (Grid.GetRow(e) == r - 1 && Grid.GetColumn(e) == c)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
                else if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c - 1)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
                else if (Grid.GetRow(e) == r + 1 && Grid.GetColumn(e) == c + 1)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
                else if (Grid.GetRow(e) == r - 1 && Grid.GetColumn(e) == c - 1)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
                else if (Grid.GetRow(e) == r + 1 && Grid.GetColumn(e) == c - 1)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
                else if (Grid.GetRow(e) == r - 1 && Grid.GetColumn(e) == c + 1)
                {
                    if (e.Uid == "mine")
                    {
                        bombCounter++;
                    }
                }
            }

            return bombCounter;
        }

        void GetGridElement(Grid g, int r, int c)
        {
            List<Image> imgList = new List<Image>();

            int bombCounter = 0;

            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                Image img = (Image)e;
                if (Grid.GetRow(e) == r + 1 && Grid.GetColumn(e) == c)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }
                else if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c + 1)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }
                else if (Grid.GetRow(e) == r - 1 && Grid.GetColumn(e) == c)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }
                else if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c - 1)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }
                else if (Grid.GetRow(e) == r + 1 && Grid.GetColumn(e) == c + 1)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }
                else if (Grid.GetRow(e) == r - 1 && Grid.GetColumn(e) == c - 1)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }
                else if (Grid.GetRow(e) == r + 1 && Grid.GetColumn(e) == c - 1)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }
                else if (Grid.GetRow(e) == r - 1 && Grid.GetColumn(e) == c + 1)
                {
                    if (e.Uid == "blank")
                    {
                        bombCounter = countBombs(Grid.GetRow(e), Grid.GetColumn(e));
                        imgList.Add(img);
                    }
                }

                if (bombCounter > 0)
                {
                    if (bombCounter == 1)
                    {
                        img.Source = box1;
                    }
                    else if (bombCounter == 2)
                    {
                        img.Source = box2;
                    }
                    else if (bombCounter == 3)
                    {
                        img.Source = box3;
                    }
                    else if (bombCounter == 4)
                    {
                        img.Source = box4;
                    }
                    else if (bombCounter == 5)
                    {
                        img.Source = box5;
                    }
                    else if (bombCounter == 6)
                    {
                        img.Source = box6;
                    }
                    else if (bombCounter == 7)
                    {
                        img.Source = box7;
                    }
                    else if (bombCounter == 8)
                    {
                        img.Source = box8;
                    }
                }

                bombCounter = 0;
            }

            if (!running)
            {
                running = true;
                foreach (Image tile in imgList)
                {
                    GetGridElement(grid, Grid.GetRow(tile), Grid.GetColumn(tile));
                    if (imgList.IndexOf(tile) == imgList.Count - 1)
                    {
                        running = false;
                    }
                }
            }
        }
    }
}
