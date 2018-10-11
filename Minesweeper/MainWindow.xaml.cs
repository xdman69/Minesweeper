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
        public MainWindow()
        {
            InitializeComponent();
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
            if(!string.IsNullOrWhiteSpace(Rows.Text) && !string.IsNullOrWhiteSpace(Columns.Text)) {
                int _rows = Int32.Parse(Rows.Text);
                int _columns = Int32.Parse(Columns.Text);

                grid.Children.Clear();
                Generate(_rows, _columns);
            }
        }

        public void Generate(int rows, int columns)
        {
            for(int i = 0; i < rows; i++)
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
            Application.Current.MainWindow.Height = 0;
            Application.Current.MainWindow.Width = 0;

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\76px-Minesweeper_unopened_square.svg.png");
            logo.EndInit();

            Random _rnd = new Random();

            for (int i = 0; i < rows; i++)
            {
                for (int o = 0; o < columns; o++) {
                    {
                        int _random = _rnd.Next(1, 5);
                        Image rect = new Image();
                        Grid.SetColumn(rect, i);
                        Grid.SetRow(rect, o);
                        rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                        rect.VerticalAlignment = VerticalAlignment.Stretch;
                        if (_random == 1)
                        {
                            rect.Uid = "mine";
                        } else
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
                Application.Current.MainWindow.Height += 40;
                Application.Current.MainWindow.Width += 40;
            }

            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public void Check(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\76px-Minesweeper_unopened_square.svg.png");
            logo.EndInit();

            BitmapImage blank = new BitmapImage();
            blank.BeginInit();
            blank.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\2000px-Minesweeper_0.svg.png");
            blank.EndInit();

            BitmapImage mine = new BitmapImage();
            mine.BeginInit();
            mine.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\a56bd269f247d1b7cca22b0f0e912eef.jpg");
            mine.EndInit();

            BitmapImage mineClicked = new BitmapImage();
            mineClicked.BeginInit();
            mineClicked.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\mineExplode.jpg");
            mineClicked.EndInit();

            BitmapImage mineDisabled = new BitmapImage();
            mineDisabled.BeginInit();
            mineDisabled.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\mineDisabled.jpg");
            mineDisabled.EndInit();

            string _uid = ((Image)sender).Uid;
            if (_uid != "mineDisabled" || _uid != "blankDisabled")
            {
                if(_uid == "blank")
                {
                    ((Image)sender).Source = blank;
                    UIElement element = (UIElement)InputHitTest(e.GetPosition(grid));
                    int _row = Grid.GetRow(element);
                    int _column = Grid.GetColumn(element);

                    Console.WriteLine(_row);
                    Console.WriteLine(_column);

                    foreach (Image child in grid.Children)
                    {
                        if (Grid.GetRow(child) == _row + 1 && Grid.GetColumn(child) == _column)
                        {
                            if(child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }

                        else if (Grid.GetRow(child) == _row - 1 && Grid.GetColumn(child) == _column)
                        {
                            if (child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }

                        else if (Grid.GetColumn(child) == _column + 1 && Grid.GetRow(child) == _row)
                        {
                            if (child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }

                        else if (Grid.GetColumn(child) == _column - 1 && Grid.GetRow(child) == _row)
                        {
                            if (child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }

                        else if (Grid.GetColumn(child) == _column + 1 && Grid.GetRow(child) == _row + 1)
                        {
                            if (child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }

                        else if (Grid.GetColumn(child) == _column - 1 && Grid.GetRow(child) == _row - 1)
                        {
                            if (child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }

                        else if (Grid.GetColumn(child) == _column - 1 && Grid.GetRow(child) == _row + 1)
                        {
                            if (child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }

                        else if (Grid.GetColumn(child) == _column + 1 && Grid.GetRow(child) == _row - 1)
                        {
                            if (child.Uid == "blank")
                            {
                                child.Source = blank;
                            }
                        }
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
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\76px-Minesweeper_unopened_square.svg.png");
            logo.EndInit();

            BitmapImage marked = new BitmapImage();
            marked.BeginInit();
            marked.UriSource = new Uri(@"\\data.sps-prosek.local\valesja15\Obrázky\2000px-Minesweeper_flag.svg.png");
            marked.EndInit();

            string _uid = ((Image)sender).Uid;
            if (_uid == "mine") {
                ((Image)sender).Uid = "mineDisabled";
                ((Image)sender).Source = marked;
            }

            else if (_uid == "blank")
            {
                ((Image)sender).Uid = "blankDisabled";
                ((Image)sender).Source = marked;
            }

            else if(_uid == "mineDisabled")
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
    }
}
