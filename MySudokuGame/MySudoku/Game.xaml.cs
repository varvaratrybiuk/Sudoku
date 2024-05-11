using Hints;
using Solver;
using SudokuComponents;
using System;
using System.Collections.Generic;
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

namespace MySudoku
{
    /// <summary>
    /// Interaction logic for Gamexaml.xaml
    /// </summary>
    public partial class Gamexaml : Window
    {
        private SudokuGame game;
        private static bool ButtonSpec = false;
        public Gamexaml()
        {
            InitializeComponent();
            game = new SudokuGame(SudokuLvl.Normal);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game.StartGame();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = CreateTextBox(i, j);
                    Field.Children.Add(textBox);
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                }
            }
        }

        private TextBox CreateTextBox(int i, int j)
        {
            TextBox textBox = new TextBox();
            Cell cell = game.GetCells()[i];
            if (cell != null && cell.row == i && cell.col == j)
            {
                textBox.Text = cell.value.ToString();
                textBox.IsReadOnly = true;
            }
            else
            {
                textBox.TextChanged += Cell_TextChanged;
                textBox.PreviewMouseDown += Cell_PreviewMouseDown;
            }
            return textBox;
        }

        private void Cell_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null && int.TryParse(textBox.Text, out int value))
            {
                game.AddToBoard([Grid.GetRow(textBox), Grid.GetColumn(textBox)], value);
                textBox.IsReadOnly = true;
            }
        }

        private void Random(object sender, RoutedEventArgs e) => UsedHint(new OpenRandomCell(9));

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
            TextBox? textBox = Field.Children.OfType<TextBox>().FirstOrDefault(t => Grid.GetRow(t) == cell.row && Grid.GetColumn(t) == cell.col);
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
            {
                UpdateTextBox(game.GetCells().Last());
            }
        }
        private void CheckSudoku(object sender, RoutedEventArgs e)
        {

        }

        private void UndoStep(object sender, RoutedEventArgs e)
        {

        }

      
    }
}
