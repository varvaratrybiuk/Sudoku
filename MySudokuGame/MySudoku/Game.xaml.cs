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
    public partial class Gamexaml : Window
    {
        private readonly SudokuGame _game;
        private static bool _isSpecificButtonClicked = false;
        private BoardEditor? _boardEditor;
        private readonly SudokuLvl _level;
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public Gamexaml() : this(SudokuLvl.Normal) { }

        public Gamexaml(SudokuLvl level)
        {
            InitializeComponent();
            _level = level;
            _game = new SudokuGame(_level);
            StartStopWatch();
        }

        private void StartStopWatch()
        {
            _stopWatch.Start();
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_stopWatch.IsRunning)
            {
                var elapsed = _stopWatch.Elapsed;
                time.Content = $"{elapsed.Minutes:00}:{elapsed.Seconds:00}:{elapsed.Milliseconds / 10:00}";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _game.StartGame();
            _boardEditor = new BoardEditor(_game.Board);
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < (int)_level; i++)
            {
                for (int j = 0; j < (int)_level; j++)
                {
                    var borderWTextBox = CreateTextBoxWithBorder(i, j);
                    Field.Children.Add(borderWTextBox);
                    var textBox = borderWTextBox.Child as TextBox;
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                }
            }
        }

        private Border CreateTextBoxWithBorder(int i, int j)
        {
            var textBox = new TextBox();
            var cell = _game.GetCells().FirstOrDefault(c => c.Row == i && c.Col == j);
            var border = new Border { BorderThickness = GetThickness(i, j) };

            if (cell != null)
            {
                textBox.Text = cell.Value.ToString();
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
            double gridSize = Math.Sqrt((int)_level);
            return new Thickness(
                j % gridSize == 0 ? 1 : 0,
                i % gridSize == 0 ? 1 : 0,
                j % gridSize == gridSize - 1 ? 1 : 0,
                i % gridSize == gridSize - 1 ? 1 : 0
            );
        }

        private void Cell_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int value))
            {
                _boardEditor?.Save();
                _game.AddToBoard(new[] { Grid.GetRow(textBox), Grid.GetColumn(textBox) }, value);
                textBox.IsReadOnly = true;
            }
        }

        private void Random(object sender, RoutedEventArgs e)
        {
            var usedCells = _game.GetCells().Select(cell => new[] { cell.Row, cell.Col }).ToList();
            UseHint(new OpenRandomCell(usedCells, (int)_level));
        }

        private void Specific(object sender, RoutedEventArgs e) => _isSpecificButtonClicked = true;

        private void Cell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && _isSpecificButtonClicked && sender is TextBox textBox)
            {
                UseHint(new OpenSpecificCell(Grid.GetRow(textBox), Grid.GetColumn(textBox)));
                _isSpecificButtonClicked = false;
            }
        }

        private void UpdateTextBox(Cell cell)
        {
            var border = Field.Children.OfType<Border>().FirstOrDefault(b =>
            {
                var textBox = b.Child as TextBox;
                return textBox != null && Grid.GetRow(textBox) == cell.Row && Grid.GetColumn(textBox) == cell.Col;
            });

            if (border?.Child is TextBox textBox)
            {
                textBox.TextChanged -= Cell_TextChanged;
                textBox.IsReadOnly = true;
                textBox.Text = cell.Value.ToString();
            }
        }

        private void UseHint(IHint hint)
        {
            if (_game.OpenCell(hint))
                UpdateTextBox(_game.GetCells().Last());
        }

        private void Redirection()
        {
            var userWindow = new UserWindow();
            userWindow.Show();
            Hide();
        }

        private void CheckSudoku(object sender, RoutedEventArgs e)
        {
            CheckCells();
            if (_game.FullBoard())
            {
                _stopWatch.Stop();
                RatingGenerator.GetInstance().WriteToFile(_level.ToString(), time.Content.ToString());
                MessageBox.Show("Good Job!", "Win", MessageBoxButton.OK, MessageBoxImage.Information);
                Redirection();
            }
        }

        private void CheckCells()
        {
            foreach (var border in Field.Children.OfType<Border>())
            {
                if (border.Child is TextBox cell && cell.Name != "Use")
                {
                    int row = Grid.GetRow(cell);
                    int col = Grid.GetColumn(cell);

                    if (int.TryParse(cell.Text, out int value) && !_game.Check(new[] { row, col }, value))
                    {
                        cell.Text = "";
                        cell.IsReadOnly = false;
                    }
                    else
                    {
                        cell.FontWeight = FontWeights.Bold;
                    }

                    _game.GetDraft().Clear();
                    _boardEditor?.Clear();
                }
            }
        }

        private void UndoStep(object sender, RoutedEventArgs e)
        {
            var lastCell = _game.Board.Draft.LastOrDefault();
            _boardEditor?.Undo();

            foreach (var border in Field.Children.OfType<Border>())
            {
                if (border.Child is TextBox cell && cell.Name != "Use" && lastCell != null)
                {
                    int row = Grid.GetRow(cell);
                    int col = Grid.GetColumn(cell);

                    if (!_game.Board.Draft.Contains(lastCell) && row == lastCell.Row && col == lastCell.Col)
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
                _stopWatch.Stop();
                Redirection();
            }
        }
    }
}
