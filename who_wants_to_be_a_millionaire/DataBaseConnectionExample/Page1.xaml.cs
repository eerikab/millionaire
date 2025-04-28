using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataBaseConnectionExample;
using who_wants_to_be_a_millionaire;

namespace Millionaire
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private readonly MainWindow _main;
        private readonly List<List<string>> _questions;
        private readonly List<(string Key, Button Btn)> _answerButtons;
        private readonly Random _random = new Random();

        private int _level = 1;
        private bool _canAnswer = true;
        private string _correctAnswer = string.Empty;
        private string _guaranteed = "0 €";

        private void Chose_A(object sender, RoutedEventArgs e) => _ = HandleAnswerAsync("a");
        private void Chose_B(object sender, RoutedEventArgs e) => _ = HandleAnswerAsync("b");
        private void Chose_C(object sender, RoutedEventArgs e) => _ = HandleAnswerAsync("c");
        private void Chose_D(object sender, RoutedEventArgs e) => _ = HandleAnswerAsync("d");

        public Page1(MainWindow main)
        {
            InitializeComponent();

            _main = main ?? throw new ArgumentNullException(nameof(main));
            var db = new DbRepository();
            _questions = db.GetQuestions();

            // Map answer keys to buttons
            _answerButtons = new List<(string, Button)>
            {
                ("a", Answer_A),
                ("b", Answer_B),
                ("c", Answer_C),
                ("d", Answer_D)
            };

            LoadQuestion();
        }

        private void LoadQuestion()
        {
            // Reset lifelines and state
            _canAnswer = true;
            Pick5050.Clear();

            // Select a random question for current level
            int start = (_level - 1) * 5;
            int idx = _random.Next(start, Math.Min(start + 5, _questions.Count));
            var q = _questions[idx];

            Question_label.Content = q[0];
            for (int i = 0; i < _answerButtons.Count; i++)
            {
                var (key, btn) = _answerButtons[i];
                btn.Content = $"{key.ToUpper()}: {q[i + 1]}";
                btn.Tag = key;
                btn.IsEnabled = true;
            }

            _correctAnswer = q[5];
        }

        private async Task HandleAnswerAsync(string selected)
        {
            if (!_canAnswer) return;
            _canAnswer = false;

            if (selected == _correctAnswer)
                await ProcessCorrectAsync();
            else
                await ProcessIncorrectAsync();
        }

        private async Task ProcessCorrectAsync()
        {
            ShowResult("Correct answer!", Brushes.Green);

            // Update money labels
            var moneyLabel = FindName($"money_label_{_level}") as Label;
            if (moneyLabel != null)
                HighlightLabel(moneyLabel, _level);

            var prevLabel = FindName($"money_label_{_level - 1}") as Label;
            if (prevLabel != null)
                UnhighlightLabel(prevLabel, _level - 1);

            _main.FinalScore = moneyLabel?.Content as string;
            if (_level % 5 == 0)
                _guaranteed = _main.FinalScore;

            await Task.Delay(1000);
            ClearResult();

            if (_level == 15)
            {
                _main.FinalScore = "won";
                new End_Window().Show();
                _main.Hide();
                return;
            }

            _level++;
            LoadQuestion();
        }

        private async Task ProcessIncorrectAsync()
        {
            _main.FinalScore = _guaranteed;
            ShowResult("Wrong answer!", Brushes.Red);
            await Task.Delay(3000);

            new End_Window().Show();
            _main.Hide();
        }

        private void ShowResult(string text, Brush color)
        {
            Result_label.Content = text;
            Result_label.Foreground = color;
        }

        private void ClearResult() => Result_label.Content = string.Empty;

        private void HighlightLabel(Label lbl, int lvl)
        {
            lbl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFAE00"));
            lbl.Foreground = (lvl % 5 == 0) ? Brushes.White : Brushes.Black;
        }

        private void UnhighlightLabel(Label lbl, int lvl)
        {
            lbl.Background = Brushes.Transparent;
            lbl.Foreground = (lvl % 5 == 1)
                ? Brushes.White
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF6E00"));
        }

        // Answer clicked
        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string key)
                _ = HandleAnswerAsync(key);
        }

        // Lifelines
        private readonly List<(string Key, Button Btn)> Pick5050 = new List<(string, Button)>();

        private void Click_lifeline_1(object sender, RoutedEventArgs e)
        {
            var wrongs = _answerButtons.Where(x => x.Key != _correctAnswer).ToList();
            var keep = wrongs[_random.Next(wrongs.Count)];
            foreach (var w in wrongs)
                if (w.Key != keep.Key)
                {
                    w.Btn.Content = string.Empty;
                    Pick5050.Add(w);
                }
            (sender as Button).IsEnabled = false;
        }

        private void Click_lifeline_2(object sender, RoutedEventArgs e)
        {
            var wrongs = _answerButtons.Where(x => x.Key != _correctAnswer && !Pick5050.Any(p => p.Key == x.Key)).ToList();
            var rem = wrongs[_random.Next(wrongs.Count)];
            rem.Btn.Content = string.Empty;
            Pick5050.Add(rem);
            (sender as Button).IsEnabled = false;
        }

        private void Click_lifeline_3(object sender, RoutedEventArgs e)
        {
            var choice = _answerButtons[_random.Next(_answerButtons.Count)];
            choice.Btn.Content += " (friend pick)";
            (sender as Button).IsEnabled = false;
        }

        private void Click_lifeline_4(object sender, RoutedEventArgs e)
        {
            foreach (var (key, btn) in _answerButtons)
            {
                int percent = _random.Next(key == _correctAnswer ? 40 : 0,
                                            key == _correctAnswer ? 101 : 61);
                btn.Content += $" {percent}%";
            }
            (sender as Button).IsEnabled = false;
        }

        private void Click_cashout_button(object sender, RoutedEventArgs e)
        {
            if (_main.FinalScore != "0 €")
            {
                new End_Window().Show();
                _main.Hide();
            }
        }
    }
}
