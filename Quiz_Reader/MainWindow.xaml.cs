using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Serialization;
using Quiz_Generator;
using Quiz_Generator.Models;

namespace Quiz_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Quiz _quiz;
        private readonly string _key = "SgUkXp2s5v8y/B?E(H+MbQeThWmYq3t6";
        private AESEncryption _aese;
        public int question_record;
        public int[][] players_answers;
        public int sec;
        public int min;
        public string time = "";
        DispatcherTimer disTmr = new DispatcherTimer();




        public MainWindow()
        {
            InitializeComponent();
            _aese = new AESEncryption();
            _quiz = new Quiz();
            DecryptQuiz();
            start();
            disTmr.Tick += new EventHandler(disTmr_Tick);
            disTmr.Interval = TimeSpan.FromSeconds(1);
            disTmr.Start();
            disTmr.IsEnabled = false;
        }
        private void shuffle_quiz()
        {
            for (int i = _quiz.Questions.Count() - 1; i >= 0; i--)
            {
                Random random = new Random();
                int random_int = random.Next(i);
                var question = _quiz.Questions[random_int];
                _quiz.Questions[random_int] = _quiz.Questions[i];
                _quiz.Questions[i] = question;
            }
        }
        private void shuffle_answers()
        {
            for (int i = 0; i < _quiz.Questions.Count() - 1; i++)
            {
                for (int j = 3; j >= 0; j--)
                {
                    Random random = new Random();
                    int random_int = random.Next(j);
                    var answer = _quiz.Questions[i].Answers[random_int];
                    _quiz.Questions[i].Answers[random_int] = _quiz.Questions[i].Answers[j];
                    _quiz.Questions[i].Answers[j] = answer;
                }
            }
        }
        private void start()
        {
            text_block.Text = "Aby rozpocząć Quiz kliknij przycisk: rozpocznij";
            vis_check(false);
            clock.Visibility = (Visibility)1;

            shuffle_quiz();
            if (_quiz.RandomOrderAnswers)
            {
                shuffle_answers();
            }

            players_answers = new int[_quiz.Questions.Count()][];

            for (int i = 0; i < _quiz.Questions.Count(); i++)
            {
                int[] line = new int[] { 0, 0, 0, 0 };
                players_answers[i] = line;
            }

            uncheck();

            time = "0:00";
            sec = 0;
            min = 0;

            summary_text.Visibility = (Visibility)1;

            vis_func(0);
            prev_button.Visibility = (Visibility)1;
            next_button.Visibility = (Visibility)1;
        }
        private void DecryptQuiz()
        {
            LoadQuizXML();
            _quiz.Name = _aese.DecryptString(_key, _quiz.Name);
            foreach (var question in _quiz.Questions)
            {
                question.Name = _aese.DecryptString(_key, question.Name);
                foreach (var answer in question.Answers)
                {
                    answer.Content = _aese.DecryptString(_key, answer.Content);
                }
            }
        }
        private void LoadQuizXML()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Quiz));
            using (FileStream fileStream = new FileStream("Data/Quiz.xml", FileMode.Open))
            {
                _quiz = (Quiz)xs.Deserialize(fileStream);
            }
        }
        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            isArrow();
            text_block.Text = _quiz.Questions[0].Name;
            question_record = 0;
            button_content();
            vis_check(true);


            vis_func(1);
            question_record = 0;

            timer_start();
        }
        private void timer_start()
        {
            disTmr.IsEnabled = true;
            clock.Visibility = 0;
            clock.Text = time;
        }
        private void timer_stop()
        {
            disTmr.IsEnabled = false;
        }
        private void disTmr_Tick(object sender, EventArgs e)
        {
            sec++;
            if (sec < 10)
                time = min.ToString() + ":0" + sec.ToString();
            else if (sec >= 10 && sec < 60)
            {
                time = min.ToString() + ":" + sec.ToString();
            }
            else
            {
                min++;
                sec = 0;
            }
            clock.Text = time;
        }
        private void end_button_Click(object sender, RoutedEventArgs e)
        {
            isArrow();


            vis_check(false);
            vis_func(2);
            prev_button.Visibility = (Visibility)1;
            next_button.Visibility = (Visibility)1;

            clock.Visibility = (Visibility)1;
            timer_stop();
            summary();
        }
        public void isArrow()
        {

            if (question_record <= 0)
            {
                prev_button.Visibility = (Visibility)1;
            }
            else
            {
                prev_button.Visibility = 0;
            }
            if (question_record + 1 == _quiz.Questions.Count())
            {
                next_button.Visibility = (Visibility)1;
            }
            else
            {
                next_button.Visibility = 0;

            }
        }
        private void uncheck()
        {
            if (players_answers[question_record][0] == 1)
            {
                button_0.IsChecked = true;
                button_0.Foreground = Brushes.Yellow;
            }
            else
            {
                button_0.IsChecked = false;
                button_0.Foreground = Brushes.Black;
            }
            if (players_answers[question_record][1] == 1)
            {
                button_1.IsChecked = true;
                button_1.Foreground = Brushes.Yellow;
            }
            else
            {
                button_1.IsChecked = false;
                button_1.Foreground = Brushes.Black;
            }
            if (players_answers[question_record][2] == 1)
            {
                button_2.IsChecked = true;
                button_2.Foreground = Brushes.Yellow;
            }
            else
            {
                button_2.IsChecked = false;
                button_2.Foreground = Brushes.Black;
            }
            if (players_answers[question_record][3] == 1)
            {
                button_3.IsChecked = true;
                button_3.Foreground = Brushes.Yellow;
            }
            else
            {
                button_3.IsChecked = false;
                button_3.Foreground = Brushes.Black;
            }
        }
        private void button_answer_Checked(object sender, RoutedEventArgs e)
        {
            var checked_button = sender as CheckBox;
            string number_of_answer = checked_button.Name.Substring(button_1.Name.Length - 1);
            if (checked_button.Foreground == Brushes.Black)
            {
                checked_button.Foreground = Brushes.Yellow;
                to_answers(number_of_answer);
            }
            else
            {
                checked_button.Foreground = Brushes.Black;
                delete_answer(number_of_answer);
            }
        }
        private void to_answers(string number_of_button)
        {
            int index = int.Parse(number_of_button);
            players_answers[question_record].SetValue(1, index);
        }
        private void delete_answer(string number_of_button)
        {
            int index = int.Parse(number_of_button);
            players_answers[question_record].SetValue(0, index);
        }
        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            question_record += 1;
            uncheck();
            isArrow();
            text_block.Text = _quiz.Questions[question_record].Name;
            button_content();
        }
        private void prev_button_Click(object sender, RoutedEventArgs e)
        {
            question_record -= 1;
            uncheck();
            isArrow();
            text_block.Text = _quiz.Questions[question_record].Name;
            button_content();
        }
        private void summary()
        {
            question_record = 0;

            var cor_incor = stats();

            text_block.Text = "Poprawne odpowiedzi: " + cor_incor[0].ToString() + "\n";
            text_block.Text += "Błędne/Brak odpowiedzi: " + cor_incor[1].ToString() + " \n";
            text_block.Text += "Czas: " + time;

            summary_text.Visibility = 0;
            Scroll.Visibility = 0;
            summary_text.Text = " ";
            for (int i = 0; i < _quiz.Questions.Count(); i++)
            {
                string current = "  ";
                for (int j = 0; j < 4; j++)
                {
                    if (players_answers[i][j] == 1)
                    {
                        current += _quiz.Questions[i].Answers[j].Content + ", ";
                    }
                }

                string correct = "  ";
                for (int j = 0; j < 4; j++)
                {
                    if (_quiz.Questions[i].Answers[j].Correct)
                    {
                        correct += _quiz.Questions[i].Answers[j].Content + ", ";
                    }
                }

                summary_text.Text += "Pytanie " + (i + 1).ToString() + ": " + _quiz.Questions[i].Name + "\n";
                summary_text.Text += "Twoje odpowiedzi: " + current.Remove(current.Length - 2) + "\n";
                summary_text.Text += "Poprawne odpowiedzi:" + correct.Remove(correct.Length - 2) + "\n";
                summary_text.Text += "\n";
            }
        }
        private int[] stats()
        {
            int correct_n = 0;
            int incorrect = 0;
            for (int i = 0; i < _quiz.Questions.Count(); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (players_answers[i][j] == 1 && _quiz.Questions[i].Answers[j].Correct)
                    {
                        correct_n++;
                    }
                    else if (players_answers[i][j] == 0 && _quiz.Questions[i].Answers[j].Correct //brak odp, która była dobra
                        || players_answers[i][j] == 1 && _quiz.Questions[i].Answers[j].Correct)  // zaznaczenie błędnej
                    {
                        incorrect++;
                    }
                }
            }
            int[] ans = { correct_n, incorrect };
            return ans;
        }
        private void end_summary_button_Click(object sender, RoutedEventArgs e)
        {
            start();
        }
        private void vis_check(bool check)
        {
            if (!check)
            {
                button_0.Visibility = (Visibility)1;
                button_1.Visibility = (Visibility)1;
                button_2.Visibility = (Visibility)1;
                button_3.Visibility = (Visibility)1;
            }
            else
            {
                button_0.Visibility = 0;
                button_1.Visibility = 0;
                button_2.Visibility = 0;
                button_3.Visibility = 0;
            }
        }
        private void button_content()
        {
            button_0.Content = _quiz.Questions[question_record].Answers[0].Content;
            button_1.Content = _quiz.Questions[question_record].Answers[1].Content;
            button_2.Content = _quiz.Questions[question_record].Answers[2].Content;
            button_3.Content = _quiz.Questions[question_record].Answers[3].Content;
        }
        private void vis_func(int func)
        {
            switch (func)
            {
                case 0:
                    start_button.IsEnabled = true;
                    start_button.Visibility = 0;
                    end_button.IsEnabled = false;
                    end_button.Visibility = (Visibility)1;
                    end_summary_button.IsEnabled = false;
                    end_summary_button.Visibility = (Visibility)1;
                    break;

                case 1:
                    start_button.IsEnabled = false;
                    start_button.Visibility = (Visibility)1;
                    end_button.IsEnabled = true;
                    end_button.Visibility = 0;
                    end_summary_button.IsEnabled = false;
                    end_summary_button.Visibility = (Visibility)1;
                    break;

                case 2:
                    start_button.IsEnabled = false;
                    start_button.Visibility = (Visibility)1;
                    end_button.IsEnabled = false;
                    end_button.Visibility = (Visibility)1;
                    end_summary_button.IsEnabled = true;
                    end_summary_button.Visibility = 0;
                    break;
            }
        }

    }
}
