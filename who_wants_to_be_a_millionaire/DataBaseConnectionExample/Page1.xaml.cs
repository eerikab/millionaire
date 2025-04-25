using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using who_wants_to_be_a_millionaire;

namespace Millionaire
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {   
        public string correctAnswer = "";
        public List<List<string>> data;
        public int level = 1;

        public List<(string key, Button btn)> answers;
        public List<(string key, Button btn)> incorrect;

        public List<(string key, Button btn)> pick_50_50;

        public Page1()
        {
            InitializeComponent();

            DbRepository dbRepository = new DbRepository();
            data = dbRepository.GetQuestions();

            answers = new List<(string key, Button btn)>//kõik nupud koos vastuse tähisega
            {
                ("a", Answer_A),
                ("b", Answer_B),
                ("c", Answer_C),
                ("d", Answer_D)
            };
            pick_50_50 = new List<(string key, Button btn)>();

            SetQuestion(0);
        }

        private void SetQuestion(int question_id)
        {
            Question_label.Content = data[question_id][0]; //siia oleks vaja panna ainult küsimus
            Answer_A.Content = "A: " + data[question_id][1]; //siia oleks vaja vastus A
            Answer_B.Content = "B: " + data[question_id][2]; //siia oleks vaja vastus B
            Answer_C.Content = "C: " + data[question_id][3]; //siia oleks vaja vastus C
            Answer_D.Content = "D: " + data[question_id][4]; //siia oleks vaja vastus D

            correctAnswer = data[question_id][5]; //õige vastus
            int level = int.Parse(data[question_id][6]);

            incorrect = answers.Where(a => a.key != correctAnswer).ToList();//valed vastused
        }

        private async void Select(string selected)
        {
            if (correctAnswer == selected)
            {
                Result_label.Content = "Correct answer!";
                Result_label.Foreground = System.Windows.Media.Brushes.Green;

                var money_label = this.FindName("money_label_" + level.ToString()) as Label;
                var main = (MainWindow)Application.Current.MainWindow;

                if (level == 15)//lõpu õnnitlus
                {
                    main.FinalScore = "won";
                    End_Window endpage = new End_Window();
                    endpage.Show(); //lõpu ekraani kuvamine
                    Window.GetWindow(this)?.Close(); //selle page sulgemine
                    return;
                }

                var previous_money_label = this.FindName("money_label_" + (level-1).ToString()) as Label;

                if (money_label != null) //et alustades esimesest ei tuleks error
                {
                    money_label.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFAE00"));//paneb võidu summa tausta oranžiks
                    money_label.Foreground = System.Windows.Media.Brushes.White;//paneb võidusumma teksti valgeks
                }

                if (previous_money_label != null) //et alustades esimesest ei tuleks error
                {
                    previous_money_label.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));//taastab eelmise võidu summa tausta
                    previous_money_label.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF6E00"));//taastab eelmise võidu summa teksti
                }

                
                main.FinalScore = money_label.Content as string;
                await Task.Delay(1000); // 1 sekund ooteaega
                Result_label.Content = "";
                level += 1;
                SetQuestion(level);//paneb uued küsimused ja vastused
            }
            else
            {
                var main = (MainWindow)Application.Current.MainWindow;
                main.FinalScore = "Game over, you lost!";
                Result_label.Content = "Wrong answer!";
                Result_label.Foreground = System.Windows.Media.Brushes.Red;
                await Task.Delay(3000); // 3 sekund ooteaega
                End_Window endpage = new End_Window();
                endpage.Show(); //lõpu ekraani kuvamine
                Window.GetWindow(this)?.Close(); //selle page sulgemine
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

        private void Click_lifeline_1(object sender, RoutedEventArgs e)//lifeline 50/50
        {
            var rnd = new Random(); 
            var randomWrong = incorrect[rnd.Next(incorrect.Count)]; //üks juhuslik vale vastus

            foreach (var wrong in incorrect)
                {
                    if (wrong != randomWrong && !pick_50_50.Contains(wrong))
                    {
                        pick_50_50.Add(wrong);
                        wrong.btn.Content = ""; //kustutab kaks vale vastust
                    }
                }

            (sender as Button).IsEnabled = false; //deaktiveerib 50/50 nupu

        }

        private async void Click_lifeline_2(object sender, RoutedEventArgs e) //üks vale välja
        {
            while (true)
            {
                var random = new Random();
                var RandomAnswer = incorrect[random.Next(incorrect.Count)]; //valib juhusliku vastuse
                if (!pick_50_50.Contains(RandomAnswer))//et ei oleks sama, mis 50/50 lifelines
                {
                    pick_50_50.Add(RandomAnswer);
                    RandomAnswer.btn.Content = ""; //kustutab vale vastuse
                    break;
                }
                
            }

            (sender as Button).IsEnabled = false; //deaktiveerib nupu
        }

        private void Click_lifeline_3(object sender, RoutedEventArgs e) //helista sõbrale
        {
            var random = new Random();
            var RandomAnswer = answers[random.Next(answers.Count)]; //valib juhusliku vastuse

            if (RandomAnswer.btn.Content != null && !RandomAnswer.btn.Content.ToString().Contains(" (friend pick)"))
            {
                RandomAnswer.btn.Content += " (friend pick)";
            }

            (sender as Button).IsEnabled = false; //deaktiveerib nupu

        }

        private void Click_lifeline_4(object sender, RoutedEventArgs e) //küsi publikult
        {

            foreach (var i in answers)
            {
                if (i.key != correctAnswer)
                {
                    Random random = new Random();
                    int RandomNumber = random.Next(0, 60);

                    i.btn.Content += " "+RandomNumber + "%"; //paneb valedele vastustele taha protsendi 0-60
                }

                else
                {
                    Random random = new Random();
                    int RandomNumber = random.Next(40, 100); 

                    i.btn.Content += " " + RandomNumber + "%"; //paneb õigele vastuse taha protsendi 40-100
                }

            }

            (sender as Button).IsEnabled = false; //deaktiveerib nupu

        }

        private void Click_cashout_button(object sender, RoutedEventArgs e)
        {
            var main = (MainWindow)Application.Current.MainWindow;

            if (main.FinalScore != "0")
            {
                End_Window endpage = new End_Window();
                endpage.Show(); //lõpu ekraani kuvamine
                Window.GetWindow(this)?.Close(); //selle page sulgemine
            }
        }
    }
}
