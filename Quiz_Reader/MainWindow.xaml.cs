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
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;
using Quiz_Generator.Models;
using Quiz_Generator;
using System.Windows.Threading;
using System.Timers;

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




        public MainWindow()
        {
            InitializeComponent();
            _aese = new AESEncryption();
            _quiz = new Quiz();
            DecryptQuiz();
            start();
        }
        
        private void start()
        {
            text_block.Text = "Aby rozpocząć Quiz kliknij przycisk: rozpocznij";
            button_0.Visibility = (Visibility)1;
            button_1.Visibility = (Visibility)1;
            button_2.Visibility = (Visibility)1;
            button_3.Visibility = (Visibility)1;
            clock.Visibility = (Visibility)1;

            players_answers = new int[_quiz.Questions.Count()][];

            for (int i = 0; i < _quiz.Questions.Count(); i++) //nie dziala
            {
                int[] line = new int[] { 0, 0, 0, 0 };
                players_answers[i] = line;
            }

            sec = 0;
            min = 0;

            summary_text_block.Visibility = (Visibility)1;

            start_button.IsEnabled = true;
            start_button.Visibility = 0;
            end_button.IsEnabled = false;
            end_button.Visibility = (Visibility)1;
            end_summary_button.IsEnabled = false;
            end_summary_button.Visibility = (Visibility)1;
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
            button_0.Content = _quiz.Questions[0].Answers[0].Content;
            button_1.Content = _quiz.Questions[0].Answers[1].Content;
            button_2.Content = _quiz.Questions[0].Answers[2].Content;
            button_3.Content = _quiz.Questions[0].Answers[3].Content;
            button_0.Visibility = 0;
            button_1.Visibility = 0;
            button_2.Visibility = 0;
            button_3.Visibility = 0;

            start_button.IsEnabled = false;
            start_button.Visibility = (Visibility)1;
            end_button.IsEnabled = true;
            end_button.Visibility = 0;
            question_record = 0;

            clock.Visibility = 0;
            timer(true);
        }

        //timer co mnozy czas po zakoncz xdddddd
        private void timer(bool start_stop)
        {
            DispatcherTimer disTmr = new DispatcherTimer();
            disTmr.Tick += new EventHandler(disTmr_Tick);
            disTmr.Interval = new TimeSpan(0, 0, 1);
            disTmr.Start();
            if (!start_stop)
            {
                disTmr.Stop();
            }
        }

        public void disTmr_Tick(object sender, EventArgs e)
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
            start_button.IsEnabled = false;
            start_button.Visibility = (Visibility);
            end_button.IsEnabled = true;
            end_button.Visibility = 0;
            end_summary_button.IsEnabled = true;
            end_summary_button.Visibility = 0;

            button_0.Visibility = (Visibility)1;
            button_1.Visibility = (Visibility)1;
            button_2.Visibility = (Visibility)1;
            button_3.Visibility = (Visibility)1;
            prev_button.Visibility = (Visibility)1;
            next_button.Visibility = (Visibility)1;

            clock.Visibility = (Visibility)1;
            timer(false);
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
            button_0.IsChecked = false;
            button_1.IsChecked = false;
            button_2.IsChecked = false;
            button_3.IsChecked = false;
            button_0.Foreground = Brushes.Black;
            button_1.Foreground = Brushes.Black;
            button_2.Foreground = Brushes.Black;
            button_3.Foreground = Brushes.Black;
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
            button_0.Content = _quiz.Questions[question_record].Answers[0].Content;
            button_1.Content = _quiz.Questions[question_record].Answers[1].Content;
            button_2.Content = _quiz.Questions[question_record].Answers[2].Content;
            button_3.Content = _quiz.Questions[question_record].Answers[3].Content;
        }

        private void prev_button_Click(object sender, RoutedEventArgs e)
        {
            question_record -= 1;
            uncheck();
            isArrow();
            text_block.Text = _quiz.Questions[question_record].Name;
            button_0.Content = _quiz.Questions[question_record].Answers[0].Content;
            button_1.Content = _quiz.Questions[question_record].Answers[1].Content;
            button_2.Content = _quiz.Questions[question_record].Answers[2].Content;
            button_3.Content = _quiz.Questions[question_record].Answers[3].Content;
        }
        private void summary()
        {
            question_record = -1;
            isArrow();
            text_block.Text = "Poprawne odpowiedzi: \n";
            text_block.Text += "Błędne odpowiedzi: \n";
            text_block.Text += "Czas: " + time;


        }

        private void end_summary_button_Click(object sender, RoutedEventArgs e)
        {
            start();  
        }
    }
}
