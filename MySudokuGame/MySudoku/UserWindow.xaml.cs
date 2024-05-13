using Rating;
using Solver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public readonly Dictionary<string, SudokuLvl> Levels = new Dictionary<string, SudokuLvl>()
            {
                { "Easy", SudokuLvl.Easy },
                { "Normal", SudokuLvl.Normal },
                { "Hard", SudokuLvl.Hard }
            };
        public UserWindow()
        {
            InitializeComponent();
            lvls.ItemsSource = Levels.Keys;
            lvls.SelectedIndex = 0;
     
        }
      
        private void Start(object sender, RoutedEventArgs e)
        {
            if(lvls.SelectedValue is string)
            {
                string choosedlvl = (string)lvls.SelectedValue;
                var Game = new Gamexaml(Levels[choosedlvl]);
                Game.Show();
                Hide();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void lvls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvls.SelectedValue is string)
            {
                string choosedlvl = (string)lvls.SelectedValue;
                var ratingTable = new RatingGenerator().GenerateDataTable(choosedlvl);
                rating.ItemsSource = ratingTable.DefaultView;
            }
          
        }
    }
}
