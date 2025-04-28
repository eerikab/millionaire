using DataBaseConnectionExample;
using Millionaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
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

namespace who_wants_to_be_a_millionaire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string FinalScore { get; set; } = "0";
        public MainWindow()
        {
            InitializeComponent();
            actual_rules.Visibility = Visibility.Collapsed;
            actual_rules_Copy.Visibility = Visibility.Collapsed;
        }

        private void open_new_page(object sender, RoutedEventArgs e)
        {
            Page1 pg = new Page1(this); //uue lehe loomine
            this.Content = pg;
        }

        private void rules_MouseEnter(object sender, MouseEventArgs e)
        {
            actual_rules.Visibility = Visibility.Visible;
        }

        private void rules_MouseLeave(object sender, MouseEventArgs e)
        {
            actual_rules.Visibility = Visibility.Collapsed;
        }

        private void reeglid_MouseEnter(object sender, MouseEventArgs e)
        {
            actual_rules_Copy.Visibility = Visibility.Visible;
        }

        private void reeglid_MouseLeave(object sender, MouseEventArgs e)
        {
            actual_rules_Copy.Visibility = Visibility.Collapsed;
        }
    }
}
