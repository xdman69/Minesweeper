using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<List<Image>> MineField = new List<List<Image>>();
        Random _rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();

            logo.BeginInit();
            logo.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\cellCovered.png");
            logo.EndInit();

            blank.BeginInit();
            blank.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box0.png");
            blank.EndInit();

            mine.BeginInit();
            mine.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\mine.png");
            mine.EndInit();

            mineClicked.BeginInit();
            mineClicked.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\mineExploded.png");
            mineClicked.EndInit();

            mineDisabled.BeginInit();
            mineDisabled.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\flagIncorrect.png");
            mineDisabled.EndInit();

            marked.BeginInit();
            marked.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\flag.png");
            marked.EndInit();

            box1.BeginInit();
            box1.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box1.png");
            box1.EndInit();

            box2.BeginInit();
            box2.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box2.png");
            box2.EndInit();

            box3.BeginInit();
            box3.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box3.png");
            box3.EndInit();

            box4.BeginInit();
            box4.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box4.png");
            box4.EndInit();

            box5.BeginInit();
            box5.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box5.png");
            box5.EndInit();

            box6.BeginInit();
            box6.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box6.png");
            box6.EndInit();

            box7.BeginInit();
            box7.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box7.png");
            box7.EndInit();

            box8.BeginInit();
            box8.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Stažené\miny\box8.png");
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



            for (int i = 0; i < rows; i++)
            {
                MineField.Add(new List<Image>());
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
                    MineField[i].Add(rect);
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
            double xd = sizeI * sizeO / (int)3;
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
                    if (MineField[i][o].Uid != "mine")
                    {
                        MineField[i][o].Uid = "mine";
                        counter++;
                        if (counter >= xd)
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < sizeI; i++)
            {
                for (int o = 0; o < sizeO; o++)
                {
                    if (MineField[i][o].Uid != "mine")
                    {
                        SetPoint(i, o);
                    }
                }
            }

            DebugMineField();
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
                if (MineField[i - 1][o - 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i - 1 >= 0)
            {
                if (MineField[i - 1][o].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (o - 1 >= 0)
            {
                if (MineField[i][o - 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i + 1 <= sizeI - 1 && o + 1 <= sizeO - 1)
            {
                if (MineField[i + 1][o + 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (o + 1 <= sizeI - 1)
            {
                if (MineField[i][o + 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i + 1 <= sizeI - 1)
            {
                if (MineField[i + 1][o].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i + 1 <= sizeI - 1 && o - 1 >= 0)
            {
                if (MineField[i + 1][o - 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (i - 1 >= 0 && o + 1 <= sizeO - 1)
            {
                if (MineField[i - 1][o + 1].Uid == "mine")
                {
                    Counter++;
                }
            }
            if (Counter > 0)
            {
                MineField[i][o].Uid = Counter.ToString();

            }
            else
            {
                MineField[i][o].Uid = "blank";
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

        public void RevealAround(Image image)
        {
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;
            int i = Grid.GetColumn(image);
            int o = Grid.GetRow(image);
            if (i - 1 >= 0)
            {
                if(MineField[i-1][o].Uid != "mine" && MineField[i - 1][o].Uid != "blankDisabled")
                {
                    Check(MineField[i - 1][o]);
                }
            }
            if (o - 1 >= 0)
            {
                if (MineField[i][o - 1].Uid != "mine" && MineField[i][o - 1].Uid != "blankDisabled")
                {
                    Check(MineField[i][o - 1]);
                }
            }
            if (o + 1 <= sizeI - 1)
            {
                if (MineField[i][o + 1].Uid != "mine" && MineField[i][o + 1].Uid != "blankDisabled")
                {
                    Check(MineField[i][o + 1]);
                }
            }
            if (i + 1 <= sizeO - 1)
            {
                if (MineField[i + 1][o].Uid != "mine" && MineField[i + 1][o].Uid != "blankDisabled")
                {
                    Check(MineField[i + 1][o]);

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
                    if (MineField[i][o].Uid == "mine")
                    {
                        MineField[i][o].Source = mine;
                    }
                    else if (MineField[i][o].Uid == "blank")
                    {
                        MineField[i][o].Source = blank;
                    }
                    else if (MineField[i][o].Uid == "mineExploded")
                    {
                        MineField[i][o].Source = mineClicked;
                    }
                    else
                    {
                        if (MineField[i][o].Uid == "1")
                        {
                            MineField[i][o].Source = box1;
                        }
                        else if (MineField[i][o].Uid == "2")
                        {
                            MineField[i][o].Source = box2;
                        }
                        else if (MineField[i][o].Uid == "3")
                        {
                            MineField[i][o].Source = box3;
                        }
                        else if (MineField[i][o].Uid == "4")
                        {
                            MineField[i][o].Source = box4;
                        }
                        else if (MineField[i][o].Uid == "5")
                        {
                            MineField[i][o].Source = box5;
                        }
                        else if (MineField[i][o].Uid == "6")
                        {
                            MineField[i][o].Source = box6;
                        }
                        else if (MineField[i][o].Uid == "7")
                        {
                            MineField[i][o].Source = box7;
                        }
                        else if (MineField[i][o].Uid == "8")
                        {
                            MineField[i][o].Source = box8;
                        }
                    }
                }
            }
        }
        public void DebugMineField()
        {
            int sizeI = grid.RowDefinitions.Count;
            int sizeO = grid.ColumnDefinitions.Count;
            for (int i = 0; i < sizeI; i++)
            {
                for (int o = 0; o < sizeO; o++)
                {
                    if (MineField[o][i].Uid == "blank")
                    {
                        Debug.Write("[ ]");
                    }
                    else if (MineField[o][i].Uid == "mine")
                    {
                        Debug.Write("[x]");
                    }
                    else
                    {
                        Debug.Write("[" + MineField[o][i].Uid + "]");
                    }
                }
                Debug.WriteLine(" ");
            }
        }
    }
 
}
