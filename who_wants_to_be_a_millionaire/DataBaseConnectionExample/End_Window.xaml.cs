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
using who_wants_to_be_a_millionaire;

namespace Millionaire
{
    /// <summary>
    /// Interaction logic for End_Window.xaml
    /// </summary>
    public partial class End_Window : Window
    {
        public End_Window()
        {
            InitializeComponent();

            var main = (MainWindow)Application.Current.MainWindow;

            if (main.FinalScore == "Game over, you lost!")
            {
                result_label.Foreground = System.Windows.Media.Brushes.Red;
                result_label.Content = main.FinalScore;
            }

            else if (main.FinalScore == "won")
            {
                result_label.Foreground = System.Windows.Media.Brushes.Green;
            }

            else
            {
                int index = main.FinalScore.IndexOf('.') + 1;
                main.FinalScore = main.FinalScore.Substring(index).TrimStart();
                result_label.Foreground = System.Windows.Media.Brushes.Green;
                result_label.Content = "You won "+main.FinalScore;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page1 pg = new Page1(); //uue lehe loomine
            this.Content = pg;
        }
    }
}
