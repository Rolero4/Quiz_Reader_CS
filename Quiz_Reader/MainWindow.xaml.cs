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
        public List<List<bool>> players_answers = new List<List<bool>>();


        public MainWindow()
        {
            InitializeComponent();
            _aese = new AESEncryption();
            _quiz = new Quiz();
            DecryptQuiz();
            
            text_block.Text = "Aby rozpocząć Quiz kliknij przycisk: rozpocznij";
            button_1.Visibility = (Visibility)1;
            button_2.Visibility = (Visibility)1;
            button_3.Visibility = (Visibility)1;
            button_4.Visibility = (Visibility)1;



            end_button.IsEnabled = false;
            end_button.Visibility = (Visibility)1;
            prev_button.Visibility = (Visibility)1;
            next_button.Visibility = (Visibility)1;
        }

        public void isArrow(int question_number_in_list, int total_question)
        {
            if(question_number_in_list == 0)
            {
                prev_button.Visibility = (Visibility)1;
            }
            else
            {
                prev_button.Visibility = 0;
            }
            if(question_number_in_list + 1 == total_question)
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
            button_1.IsChecked = false;
            button_2.IsChecked = false;
            button_3.IsChecked = false;
            button_4.IsChecked = false;
            button_1.Foreground = Brushes.Black;
            button_2.Foreground = Brushes.Black;
            button_3.Foreground = Brushes.Black;
            button_4.Foreground = Brushes.Black;
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
            isArrow(0, _quiz.Questions.Count());
            text_block.Text = _quiz.Questions[0].Name;
            button_1.Content = _quiz.Questions[0].Answers[0].Content;
            button_2.Content = _quiz.Questions[0].Answers[1].Content;
            button_3.Content = _quiz.Questions[0].Answers[2].Content;
            button_4.Content = _quiz.Questions[0].Answers[3].Content;
            button_1.Visibility = 0;
            button_2.Visibility = 0;
            button_3.Visibility = 0;
            button_4.Visibility = 0;
            start_button.IsEnabled = false;
            start_button.Visibility = (Visibility)1;
            end_button.IsEnabled = true;
            end_button.Visibility = 0;
            question_record = 0;
        }

        private void button_answer_Checked(object sender, RoutedEventArgs e)
        {
            var check = sender as CheckBox;
            if (check.Foreground == Brushes.Black)
            {
                check.Foreground = Brushes.Yellow;
                
            }
            else
            {
                check.Foreground = Brushes.Black;
            }
        }

        private void to_answers(string Button)
        {

        }


        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            question_record += 1;
            uncheck();
            isArrow(question_record, _quiz.Questions.Count());
            text_block.Text = _quiz.Questions[question_record].Name;
            button_1.Content = _quiz.Questions[question_record].Answers[0].Content;
            button_2.Content = _quiz.Questions[question_record].Answers[1].Content;
            button_3.Content = _quiz.Questions[question_record].Answers[2].Content;
            button_4.Content = _quiz.Questions[question_record].Answers[3].Content;
        }

        private void prev_button_Click(object sender, RoutedEventArgs e)
        {
            question_record -= 1;
            uncheck();
            isArrow(question_record, _quiz.Questions.Count());
            text_block.Text = _quiz.Questions[question_record].Name;
            button_1.Content = _quiz.Questions[question_record].Answers[0].Content;
            button_2.Content = _quiz.Questions[question_record].Answers[1].Content;
            button_3.Content = _quiz.Questions[question_record].Answers[2].Content;
            button_4.Content = _quiz.Questions[question_record].Answers[3].Content;
        }

        private void end_button_Click(object sender, RoutedEventArgs e)
        {
            isArrow(0, 0);
            text_block.Text = "Aby rozpocząć Quiz kliknij przycisk: rozpocznij";

            start_button.IsEnabled = true;
            start_button.Visibility = 0;
            end_button.IsEnabled = false;
            end_button.Visibility = (Visibility)1;

            button_1.Visibility = (Visibility)1;
            button_2.Visibility = (Visibility)1;
            button_3.Visibility = (Visibility)1;
            button_4.Visibility = (Visibility)1;
            prev_button.Visibility = (Visibility)1;
            next_button.Visibility = (Visibility)1;
            question_record = 0;
        }
    }
}
