using Hints;
using Rating;
using Solver;
using SudokuComponents;
using SudokuComponents.Memento;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MySudoku
{
    /// <summary>
    /// Interaction logic for Gamexaml.xaml
    /// </summary>
    public partial class Gamexaml : Window
    {
        private SudokuGame game;
        private static bool ButtonSpec = false;
        private BoardEditor? BoardEditor;
        private SudokuLvl _lvl = SudokuLvl.Normal;
        Stopwatch stopWatch = new Stopwatch();
        DispatcherTimer dt = new DispatcherTimer();

        public Gamexaml()
        {
            InitializeComponent();
            game = new SudokuGame(SudokuLvl.Normal);
            StartStopWatch();
        }
        public Gamexaml(SudokuLvl lvl)
        {
            InitializeComponent();
            _lvl = lvl;
            game = new SudokuGame(_lvl);
            StartStopWatch();
        }
        private  void StartStopWatch()
        {
            stopWatch.Start();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dt.Start();
        }
        void dt_Tick(object? sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                time.Content = string.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game.StartGame();
            BoardEditor = new BoardEditor(game.board);
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < (int)_lvl; i++)
            {
                for (int j = 0; j < (int)_lvl; j++)
                {
                    Border borderWTextBox = CreateTextBoxWithBorder(i, j);
                    Field.Children.Add(borderWTextBox);
                    var textBox = borderWTextBox.Child as TextBox;
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                }
            }
        }

        private Border CreateTextBoxWithBorder(int i, int j)
        {
            TextBox textBox = new TextBox();
            Cell? cell = game.GetCells().FirstOrDefault(c => c.row == i && c.col == j);
            var border = new Border()
            {
                BorderThickness = GetThickness(i, j)
            };
            if (cell != null)
            {
                textBox.Text = cell.value.ToString();
                textBox.IsReadOnly = true;
                textBox.Name = "Use";
            }
            else
            {
                textBox.TextChanged += Cell_TextChanged;
                textBox.PreviewMouseDown += Cell_PreviewMouseDown;
            }
            border.Child = textBox;
            return border;
        }

        private Thickness GetThickness(int i, int j)
        {
            var top = i % Math.Sqrt((int)_lvl) == 0 ? 1 : 0;
            var bottom = i % Math.Sqrt((int)_lvl) == Math.Sqrt((int)_lvl) - 1 ? 1 : 0;
            var left = j % Math.Sqrt((int)_lvl) == 0 ? 1 : 0;
            var right = j % Math.Sqrt((int)_lvl) == Math.Sqrt((int)_lvl) - 1 ? 1 : 0;
            return new Thickness(left, top, right, bottom);
        }

        private void Cell_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null && int.TryParse(textBox.Text, out int value))
            {
                BoardEditor?.Save();
                game.AddToBoard([Grid.GetRow(textBox), Grid.GetColumn(textBox)], value);
                textBox.IsReadOnly = true;
            }
        }

        private void Random(object sender, RoutedEventArgs e) => UsedHint(new OpenRandomCell((int)_lvl));

        private void Specific(object sender, RoutedEventArgs e) => ButtonSpec = true;

        private void Cell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && ButtonSpec && sender is TextBox textBox)
            {
                UsedHint(new OpenSpecificCell(Grid.GetRow(textBox), Grid.GetColumn(textBox)));
                ButtonSpec = false;
            }
        }
       

        private void UpdateTextBox(Cell cell)
        {
            var border = Field.Children.OfType<Border>().FirstOrDefault(b =>
            {
                TextBox? textBox = b.Child as TextBox;
                return textBox != null && Grid.GetRow(textBox) == cell.row && Grid.GetColumn(textBox) == cell.col;
            });
            TextBox? textBox = border?.Child as TextBox;
            if (textBox != null)
            {
                textBox.TextChanged -= Cell_TextChanged;
                textBox.IsReadOnly = true;
                textBox.Text = cell.value.ToString();
            }
        }

        private void UsedHint(IHint hint)
        {
            if (game.OpenCell(hint))
                UpdateTextBox(game.GetCells().Last());
        }
        private void Redirection()
        {
            var userWindow = new UserWindow();
            userWindow.Show();
            Hide();
        }
        private void CheckSudoku(object sender, RoutedEventArgs e)
        {
            foreach (var border in Field.Children.OfType<Border>())
            {
                TextBox? cell = border.Child as TextBox;
                if (cell?.Name != "Use")
                {
                    int row = Grid.GetRow(cell);
                    int column = Grid.GetColumn(cell);
                    if (int.TryParse(cell?.Text, out int value))
                    {
                        if (!game.Check([row, column], value))
                        {
                            cell.Text = "";
                            cell.IsReadOnly = false;
                        }
                        else
                            cell.FontWeight = FontWeights.Bold;
                        game.GetDraft().Clear();
                        BoardEditor?.Clear();
                    }
                }
            }
            if (game.FullBoard())
            {
                stopWatch.Stop();
                new RatingGenerator().WriteToFile(_lvl.ToString(), time.Content.ToString());
                MessageBox.Show("Good Job!", "Win", MessageBoxButton.OK, MessageBoxImage.Information);
                Redirection();
            }
        }

        private void UndoStep(object sender, RoutedEventArgs e)
        {
            var oldCell = game.board.Draft.LastOrDefault();
            BoardEditor?.Undo();
            foreach (var border in Field.Children.OfType<Border>())
            {
                TextBox? cell = border.Child as TextBox;
                if (cell?.Name != "Use" && oldCell != null)
                {
                    int row = Grid.GetRow(cell);
                    int column = Grid.GetColumn(cell);
                    if (!game.board.Draft.Contains(oldCell) && row == oldCell.row && column == oldCell.col)
                    {
                        cell.Text = "";
                        cell.IsReadOnly = false;
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
           var result = MessageBox.Show("Are you sure?", "Stop game", MessageBoxButton.YesNo, MessageBoxImage.None);
            if (result == MessageBoxResult.Yes)
            {
                stopWatch.Stop();
                Redirection();
            } 
        }
    }
}
