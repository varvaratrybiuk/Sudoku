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

        public Gamexaml()
        {
            InitializeComponent();
            game = new SudokuGame(SudokuLvl.Normal);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game.StartGame();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = new TextBox();
                    if (game.GetCells()[i] != null && game.GetCells()[i].row == i && game.GetCells()[i].col == j)
                    {
                        textBox.Text = game.GetCells()[i].value.ToString();
                        textBox.IsReadOnly = true;
                    }
                    else
                    {
                        textBox.TextChanged += Cell_TextChanged;
                       
                    }
                    Field.Children.Add(textBox);
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                }
            }
            
        }

        private void Cell_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
            {
                int row = Grid.GetRow(textBox);
                int column = Grid.GetColumn(textBox);
                if (int.TryParse(textBox.Text, out int value))
                {
                    game.AddToBoard([row, column], value);
                    textBox.IsReadOnly = true;
                }
            }
        }
      
        private void Random(object sender, RoutedEventArgs e)
        {
        }

        private void Specific(object sender, RoutedEventArgs e)
        {
        }
      

        private void CheckSudoku(object sender, RoutedEventArgs e)
        {

        }

        private void UndoStep(object sender, RoutedEventArgs e)
        {

        }

      
    }
}
