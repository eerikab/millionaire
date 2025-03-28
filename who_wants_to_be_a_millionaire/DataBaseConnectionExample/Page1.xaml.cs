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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataBaseConnectionExample;

namespace Millionaire
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        
        public Page1()
        {
            InitializeComponent();

            DbRepository dbRepository = new DbRepository();
            var data = dbRepository.GetQuestions();
            string data_string = "";
            foreach (var line in data)
            {
                foreach (string key in line)
                {
                    data_string += key + ", ";
                }
                data_string += "\n";
            }

            Question_label.Content = data_string; //siia oleks vaja panna ainult küsimus
            Answer_A.Content = data_string; //siia oleks vaja vastus A
            Answer_B.Content = data_string; //siia oleks vaja vastus B
            Answer_C.Content = data_string; //siia oleks vaja vastus C
            Answer_D.Content = data_string; //siia oleks vaja vastus D

        }

        private void Chose_A(object sender, RoutedEventArgs e)
        {
            /* if (correctAnswer == "A") SIIA OLEKS VAJA ÕIGE VASTUSE TÄHTE
             {
                 Result_label.Content = "Correct answer!";
                 Result_label.Foreground = System.Windows.Media.Brushes.Green;
             }
             else
             {
                 Result_label.Content = "Game over!";
                 Result_label.Foreground = System.Windows.Media.Brushes.Red;
             } */
        }

        private void Chose_B(object sender, RoutedEventArgs e)
        {

        }

        private void Chose_C(object sender, RoutedEventArgs e)
        {

        }

        private void Chose_D(object sender, RoutedEventArgs e)
        {

        }
    }
}
