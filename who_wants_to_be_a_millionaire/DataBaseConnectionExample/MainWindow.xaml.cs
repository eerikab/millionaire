using DataBaseConnectionExample;
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

namespace who_wants_to_be_a_millionaire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void open_new_window(object sender, RoutedEventArgs e)
        {
            Window2 win2 = new Window2();
            // this.Visibility = Visibility.Hidden; //peidame selle akna
            // win2.Show();

            // test andmebaasi pärimiseks
            DbRepository dbRepository = new DbRepository();
            var data = dbRepository.GetQuestions();
            string data_string = "";
            foreach (var line in data)
            {
                foreach (string key in line)
                {
                    data_string += key+", ";
                }
                data_string += "\n";
            }
            this.Content = data_string;
        }
    }
}
