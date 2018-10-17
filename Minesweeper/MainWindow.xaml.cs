using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
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
    /// 

    public enum DifficultySelector
    {
        Easy = 10, Medium = 5, Hard = 3
    }
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

        static int Timer;
        public int MineCount;
        public int RevealedPoints = 0;

        public List<List<Image>> GameGrid = new List<List<Image>>();
        Random _rnd = new Random();

        static int diff = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Difficulty.ItemsSource = Enum.GetValues(typeof(DifficultySelector)).Cast<DifficultySelector>();
            Difficulty.SelectedItem = DifficultySelector.Easy;
        }

        private void Difficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            diff = (int)Difficulty.SelectedValue;
        }

        public MainWindow()
        {
            InitializeComponent();

            logo.BeginInit();
            logo.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources/2000px-Minesweeper_unopened_square.svg.png");
            logo.EndInit();

            blank.BeginInit();
            blank.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\76px-Minesweeper_0.svg.png");
            blank.EndInit();

            mine.BeginInit();
            mine.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\mine.png");
            mine.EndInit();

            mineClicked.BeginInit();
            mineClicked.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\mineExploded.png");
            mineClicked.EndInit();

            mineDisabled.BeginInit();
            mineDisabled.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\flagIncorrect.png");
            mineDisabled.EndInit();

            marked.BeginInit();
            marked.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\2000px-Minesweeper_flag.svg.png");
            marked.EndInit();

            box1.BeginInit();
            box1.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box1.png");
            box1.EndInit();

            box2.BeginInit();
            box2.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box2.png");
            box2.EndInit();

            box3.BeginInit();
            box3.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box3.png");
            box3.EndInit();

            box4.BeginInit();
            box4.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box4.png");
            box4.EndInit();

            box5.BeginInit();
            box5.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box5.png");
            box5.EndInit();

            box6.BeginInit();
            box6.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box6.png");
            box6.EndInit();

            box7.BeginInit();
            box7.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box7.png");
            box7.EndInit();

            box8.BeginInit();
            box8.UriSource = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Resources\box8.png");
            box8.EndInit();

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            myTimer.Interval = 1000; // 1000 ms is one second
            myTimer.Start();

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



            for (int i = 0; i < rows; i++)
            {
                GameGrid.Add(new List<Image>());
                for (int o = 0; o < columns; o++)
                {

                    Image rect = new Image();
                    Grid.SetColumn(rect, i);
                    Grid.SetRow(rect, o);
                    rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    rect.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

                    rect.MouseLeftButtonDown += new MouseButtonEventHandler(Check);
                    rect.MouseRightButtonDown += new MouseButtonEventHandler(RightCheck);
                    rect.Source = logo;
                    GameGrid[i].Add(rect);
                    grid.Children.Add(rect);


                }
                grid.Width += 40;
                grid.Height += 40;
                System.Windows.Application.Current.MainWindow.Height += 40;
                System.Windows.Application.Current.MainWindow.Width += 40;
            }


            GenerateNumbers();
            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }
        public void GenerateNumbers()
        {

            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;
            int counter = 0;
            double xd = sizeI * sizeO / (int)diff;
            Debug.WriteLine("diff:" + diff);
            if(xd < 1)
            {
                xd = 1;
            }

            while (counter < xd)
            {
                int i = _rnd.Next(0, sizeI);
                int o = _rnd.Next(0, sizeO);
                if (counter < xd)
                {
                    if (GameGrid[i][o].Uid != "mine")
                    {
                        GameGrid[i][o].Uid = "mine";
                        counter++;
                        if (counter >= xd)
                        {
                            MineCount = (int)xd;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < sizeI; i++)
            {
                for (int o = 0; o < sizeO; o++)
                {
                    if (GameGrid[i][o].Uid != "mine")
                    {
                        SetPoint(i, o);
                    }
                }
            }

            ShowGameGrid();
        }

        public void SetPoint(int i, int o)
        {
            /*   Debug.WriteLine(i + " " + o);
               Debug.WriteLine(MineField.Count + " " + MineField[i].Count);*/
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;
            int Counter = 0;
            if (i - 1 >= 0 && o - 1 >= 0)
            {
                if (GameGrid[i - 1][o - 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i - 1 >= 0)
            {
                if (GameGrid[i - 1][o].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (o - 1 >= 0)
            {
                if (GameGrid[i][o - 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i + 1 <= sizeI - 1 && o + 1 <= sizeO - 1)
            {
                if (GameGrid[i + 1][o + 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (o + 1 <= sizeI - 1)
            {
                if (GameGrid[i][o + 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i + 1 <= sizeI - 1)
            {
                if (GameGrid[i + 1][o].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i + 1 <= sizeI - 1 && o - 1 >= 0)
            {
                if (GameGrid[i + 1][o - 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i - 1 >= 0 && o + 1 <= sizeO - 1)
            {
                if (GameGrid[i - 1][o + 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (Counter > 0)
            {
                GameGrid[i][o].Uid = Counter.ToString();

            }
            else
            {
                GameGrid[i][o].Uid = "blank";
            }
        }
        public void Check(object sender, MouseButtonEventArgs e)
        {
            Check(((Image)sender));
        }
        public void Check(Image img)
        {
            string _uid = img.Uid;
            if (_uid != "mineDisabled" || _uid != "blankDisabled")
            {
                if (_uid == "mine")
                {
                    img.Uid = "mineExploded";
                    RevealAll();
                    GameOver();
                }

                else if (_uid == "blank")
                {
                    img.Source = blank;
                    img.Uid = "blankDisabled";
                    RevealAround(img);

                }

                else
                {
                    if (_uid == "1")
                    {
                        img.Source = box1;
                        img.Uid = "blankDisabled";
                    }
                    else if (_uid == "2")
                    {
                        img.Source = box2;
                        img.Uid = "blankDisabled";
                    }
                    else if (_uid == "3")
                    {
                        img.Source = box3;
                        img.Uid = "blankDisabled";
                    }
                    else if (_uid == "4")
                    {
                        img.Source = box4;
                        img.Uid = "blankDisabled";
                    }
                    else if (_uid == "5")
                    {
                        img.Source = box5;
                        img.Uid = "blankDisabled";
                    }
                    else if (_uid == "6")
                    {
                        img.Source = box6;
                        img.Uid = "blankDisabled";
                    }
                    else if (_uid == "7")
                    {
                        img.Source = box7;
                        img.Uid = "blankDisabled";
                    }
                    else if (_uid == "8")
                    {
                        img.Source = box8;
                        img.Uid = "blankDisabled";
                    }
                }
            }
        }
        public void RightCheck(object sender, MouseButtonEventArgs e)
        {
            Image img = ((Image)sender);

            string _uid = img.Uid;
            if (_uid == "mine")
            {
                img.Uid = "mineDisabled";
                RevealedPoints++;
                img.Source = marked;
                if(RevealedPoints >= MineCount)
                {
                    GameWon();
                }
            }

            else if (_uid == "blank")
            {
                img.Uid = "blankDisabled";
                img.Source = marked;
            }

            else if (_uid == "mineDisabled")
            {
                img.Uid = "mine";
                img.Source = logo;
            }

            else if (_uid == "blankDisabled")
            {
                img.Uid = "blank";
                img.Source = logo;
            }
            else
            {
                if (_uid == "1")
                {
                    img.Source = marked;
                    img.Uid = "hidden1";
                }
                else if (_uid == "2")
                {
                    img.Source = marked;
                    img.Uid = "hidden2";
                }
                else if (_uid == "3")
                {
                    img.Source = marked;
                    img.Uid = "hidden3";
                }
                else if (_uid == "4")
                {
                    img.Source = marked;
                    img.Uid = "hidden4";
                }
                else if (_uid == "5")
                {
                    img.Source = marked;
                    img.Uid = "hidden5";
                }
                else if (_uid == "6")
                {
                    img.Source = marked;
                    img.Uid = "hidden6";
                }
                else if (_uid == "7")
                {
                    img.Source = marked;
                    img.Uid = "hidden7";
                }
                else if (_uid == "8")
                {
                    img.Source = marked;
                    img.Uid = "hidden8";
                }
                else if (_uid == "hidden1")
                {
                    img.Source = logo;
                    img.Uid = "1";
                }
                else if (_uid == "hidden2")
                {
                    img.Source = logo;
                    img.Uid = "2";
                }
                else if (_uid == "hidden3")
                {
                    img.Source = logo;
                    img.Uid = "3";
                }
                else if (_uid == "hidden4")
                {
                    img.Source = logo;
                    img.Uid = "4";
                }
                else if (_uid == "hidden5")
                {
                    img.Source = logo;
                    img.Uid = "5";
                }
                else if (_uid == "hidden6")
                {
                    img.Source = logo;
                    img.Uid = "6";
                }
                else if (_uid == "hidden7")
                {
                    img.Source = logo;
                    img.Uid = "7";
                }
                else if (_uid == "hidden8")
                {
                    img.Source = logo;
                    img.Uid = "8";
                }
            }


        }

        public void RevealAround(Image image)
        {
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;
            int i = Grid.GetColumn(image);
            int o = Grid.GetRow(image);
            if (i - 1 >= 0)
            {
                if(GameGrid[i-1][o].Uid != "mine" && GameGrid[i - 1][o].Uid != "blankDisabled")
                {
                    Check(GameGrid[i - 1][o]);
                }
            }
            if (o - 1 >= 0)
            {
                if (GameGrid[i][o - 1].Uid != "mine" && GameGrid[i][o - 1].Uid != "blankDisabled")
                {
                    Check(GameGrid[i][o - 1]);
                }
            }
            if (o + 1 <= sizeI - 1)
            {
                if (GameGrid[i][o + 1].Uid != "mine" && GameGrid[i][o + 1].Uid != "blankDisabled")
                {
                    Check(GameGrid[i][o + 1]);
                }
            }
            if (i + 1 <= sizeO - 1)
            {
                if (GameGrid[i + 1][o].Uid != "mine" && GameGrid[i + 1][o].Uid != "blankDisabled")
                {
                    Check(GameGrid[i + 1][o]);

                }
            }
        }
        public void RevealAll()
        {
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;

            for (int i = 0; i < sizeI; i++)
            {
                for (int o = 0; o < sizeO; o++)
                {
                    if (GameGrid[i][o].Uid == "mine")
                    {
                        GameGrid[i][o].Source = mine;
                    }
                    else if (GameGrid[i][o].Uid == "blank")
                    {
                        GameGrid[i][o].Source = blank;
                    }
                    else if (GameGrid[i][o].Uid == "mineExploded")
                    {
                        GameGrid[i][o].Source = mineClicked;
                    }
                    else
                    {
                        if (GameGrid[i][o].Uid == "1")
                        {
                            GameGrid[i][o].Source = box1;
                        }
                        else if (GameGrid[i][o].Uid == "2")
                        {
                            GameGrid[i][o].Source = box2;
                        }
                        else if (GameGrid[i][o].Uid == "3")
                        {
                            GameGrid[i][o].Source = box3;
                        }
                        else if (GameGrid[i][o].Uid == "4")
                        {
                            GameGrid[i][o].Source = box4;
                        }
                        else if (GameGrid[i][o].Uid == "5")
                        {
                            GameGrid[i][o].Source = box5;
                        }
                        else if (GameGrid[i][o].Uid == "6")
                        {
                            GameGrid[i][o].Source = box6;
                        }
                        else if (GameGrid[i][o].Uid == "7")
                        {
                            GameGrid[i][o].Source = box7;
                        }
                        else if (GameGrid[i][o].Uid == "8")
                        {
                            GameGrid[i][o].Source = box8;
                        }
                    }
                }
            }
        }

        public void GameOver()
        {
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;

            for (int i = 0; i < sizeI; i++)
            {
                for (int o = 0; o < sizeO; o++)
                {
                    Rectangle rect = new Rectangle();
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, o);
                    rect.Fill = new SolidColorBrush(Colors.Black);
                    rect.Opacity = 0.7;
                    grid.Children.Add(rect);
                }
            }


            Label _GOText = new Label();
            Label _TimerText = new Label();
            Button _restart = new Button();

            _GOText.Content = "YOU LOST";
            _TimerText.Content = "in " + Timer + " seconds";
            _restart.Content = "RESTART";

            _GOText.HorizontalContentAlignment = HorizontalAlignment.Center;
            _GOText.VerticalContentAlignment = VerticalAlignment.Center;

            _GOText.HorizontalAlignment = HorizontalAlignment.Center;
            _GOText.VerticalAlignment = VerticalAlignment.Center;

            _TimerText.HorizontalContentAlignment = HorizontalAlignment.Center;
            _TimerText.VerticalContentAlignment = VerticalAlignment.Center;

            _TimerText.HorizontalAlignment = HorizontalAlignment.Center;
            _TimerText.VerticalAlignment = VerticalAlignment.Center;

            _restart.HorizontalContentAlignment = HorizontalAlignment.Center;
            _restart.VerticalContentAlignment = VerticalAlignment.Center;

            _restart.HorizontalAlignment = HorizontalAlignment.Center;
            _restart.VerticalAlignment = VerticalAlignment.Center;


            _TimerText.Margin = new Thickness(0, 90, 0 , 0);
            _restart.Margin = new Thickness(0, 180, 0, 0);

            Grid.SetColumnSpan(_GOText, grid.ColumnDefinitions.Count);
            Grid.SetRowSpan(_GOText, grid.RowDefinitions.Count);

            Grid.SetColumnSpan(_TimerText, grid.ColumnDefinitions.Count);
            Grid.SetRowSpan(_TimerText, grid.RowDefinitions.Count);

            Grid.SetColumnSpan(_restart, grid.ColumnDefinitions.Count);
            Grid.SetRowSpan(_restart, grid.RowDefinitions.Count);

            _GOText.Foreground = new SolidColorBrush(Colors.Red);
            _GOText.FontFamily = new FontFamily("Segoe UI Light");
            _GOText.FontSize = 42;

            _TimerText.Foreground = new SolidColorBrush(Colors.Red);
            _TimerText.FontFamily = new FontFamily("Segoe UI Light");
            _TimerText.FontSize = 30;

            _restart.FontFamily = new FontFamily("Segoe UI Light");
            _restart.FontSize = 20;

            _restart.Click += new RoutedEventHandler(RestartGame);

            grid.Children.Add(_GOText);
            grid.Children.Add(_TimerText);
            grid.Children.Add(_restart);
        }

        public void GameWon()
        {
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;

            for (int i = 0; i < sizeI; i++)
            {
                for (int o = 0; o < sizeO; o++)
                {
                    Rectangle rect = new Rectangle();
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, o);
                    rect.Fill = new SolidColorBrush(Colors.Black);
                    rect.Opacity = 0.7;
                    grid.Children.Add(rect);
                }
            }


            Label _GOText = new Label();
            Label _TimerText = new Label();
            Button _restart = new Button();

            _GOText.Content = "YOU WON";
            _TimerText.Content = "in " + Timer + " seconds";
            _restart.Content = "RESTART";

            _GOText.HorizontalContentAlignment = HorizontalAlignment.Center;
            _GOText.VerticalContentAlignment = VerticalAlignment.Center;

            _GOText.HorizontalAlignment = HorizontalAlignment.Center;
            _GOText.VerticalAlignment = VerticalAlignment.Center;

            _TimerText.HorizontalContentAlignment = HorizontalAlignment.Center;
            _TimerText.VerticalContentAlignment = VerticalAlignment.Center;

            _TimerText.HorizontalAlignment = HorizontalAlignment.Center;
            _TimerText.VerticalAlignment = VerticalAlignment.Center;

            _restart.HorizontalContentAlignment = HorizontalAlignment.Center;
            _restart.VerticalContentAlignment = VerticalAlignment.Center;

            _restart.HorizontalAlignment = HorizontalAlignment.Center;
            _restart.VerticalAlignment = VerticalAlignment.Center;


            _TimerText.Margin = new Thickness(0, 90, 0, 0);
            _restart.Margin = new Thickness(0, 180, 0, 0);

            Grid.SetColumnSpan(_GOText, grid.ColumnDefinitions.Count);
            Grid.SetRowSpan(_GOText, grid.RowDefinitions.Count);

            Grid.SetColumnSpan(_TimerText, grid.ColumnDefinitions.Count);
            Grid.SetRowSpan(_TimerText, grid.RowDefinitions.Count);

            Grid.SetColumnSpan(_restart, grid.ColumnDefinitions.Count);
            Grid.SetRowSpan(_restart, grid.RowDefinitions.Count);

            _GOText.Foreground = new SolidColorBrush(Colors.Green);
            _GOText.FontFamily = new FontFamily("Segoe UI Light");
            _GOText.FontSize = 42;

            _TimerText.Foreground = new SolidColorBrush(Colors.Green);
            _TimerText.FontFamily = new FontFamily("Segoe UI Light");
            _TimerText.FontSize = 30;

            _restart.FontFamily = new FontFamily("Segoe UI Light");
            _restart.FontSize = 20;

            _restart.Click += new RoutedEventHandler(RestartGame);

            grid.Children.Add(_GOText);
            grid.Children.Add(_TimerText);
            grid.Children.Add(_restart);
        }
        public void ShowGameGrid()
        {
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;
            for (int i = 0; i < sizeI; i++)
            {
                for (int o = 0; o < sizeO; o++)
                {
                    if (GameGrid[o][i].Uid == "blank")
                    {
                        Debug.Write("[ ]");
                    }
                    else if (GameGrid[o][i].Uid == "mine")
                    {
                        Debug.Write("[x]");
                    }
                    else
                    {
                        Debug.Write("[" + GameGrid[o][i].Uid + "]");
                    }
                }
                Debug.WriteLine(" ");
            }
        }

        public static void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            Timer += 1;
        }

        public void RestartGame(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
