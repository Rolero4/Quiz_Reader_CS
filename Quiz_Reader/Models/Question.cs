using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Quiz_Generator.Models
{
    [Serializable]
    public class Question
    {
        private string _name;
        private List<Answer> _answers;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value.ToString(); }
        }

        [XmlArray("Answers")]
        public List<Answer> Answers
        {
            get { return _answers; }
            set { _answers = value; }
        }

        public int CountCorrectAnswers()
        {
            int sum = 0;
            foreach (Answer answer in _answers)
            {
                if (answer.Correct)
                {
                    sum++;
                }
            }
            return sum;
        }

        public override string ToString()
        {
            int count = CountCorrectAnswers();
            if (count == 1)
            {
                return $"{_name}; {count} poprawna odpowiedź";
            }
            else
            {
                return $"{_name}; {count} poprawne odpowiedzi";
            }
        }
    }
}
