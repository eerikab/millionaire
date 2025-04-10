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
        public string correctAnswer = "";
        public List<List<string>> data;
        public int level = 2;

        public Page1()
        {
            InitializeComponent();

            DbRepository dbRepository = new DbRepository();
            data = dbRepository.GetQuestions();
            SetQuestion(0);
        }

        private void SetQuestion(int question_id)
        {
            Question_label.Content = data[question_id][0]; //siia oleks vaja panna ainult küsimus
            Answer_A.Content = "A: " + data[question_id][1]; //siia oleks vaja vastus A
            Answer_B.Content = "B: " + data[question_id][2]; //siia oleks vaja vastus B
            Answer_C.Content = "C: " + data[question_id][3]; //siia oleks vaja vastus C
            Answer_D.Content = "D: " + data[question_id][4]; //siia oleks vaja vastus D

            correctAnswer = data[question_id][5];
            int level = int.Parse(data[question_id][6]);
        }

        private async void Select(string selected)
        {
            if (correctAnswer == selected)
            {
                Result_label.Content = "Correct answer!";
                Result_label.Foreground = System.Windows.Media.Brushes.Green;

                var money_label = this.FindName("money_label_" + level.ToString()) as Label;
                var previous_money_label = this.FindName("money_label_" + (level-1).ToString()) as Label;

                money_label.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFAE00"));//paneb võidu summa tausta oranžiks
                money_label.Foreground = System.Windows.Media.Brushes.White;//paneb võidusumma teksti valgeks

                previous_money_label.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));//taastab eelmise võidu summa tausta
                previous_money_label.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF6E00"));//taastab eelmise võidu summa teksti

                await Task.Delay(1000); // 1 sekund ooteaega
                Result_label.Content = "";
                level += 1;
                SetQuestion(level);//paneb uued küsimused ja vastused
            }
            else
            {
                Result_label.Content = "Game over!";
                Result_label.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void Chose_A(object sender, RoutedEventArgs e)
        {
            Select("a");
        }

        private void Chose_B(object sender, RoutedEventArgs e)
        {
            Select("b");
        }

        private void Chose_C(object sender, RoutedEventArgs e)
        {
            Select("c");
        }

        private void Chose_D(object sender, RoutedEventArgs e)
        {
            Select("d");
        }

        private void click_lifeline_1(object sender, RoutedEventArgs e)
        {

        }

        private void click_lifeline_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
