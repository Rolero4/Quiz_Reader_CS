using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Quiz_Generator.Models
{
    [Serializable]
    [XmlRoot("Quiz")]
    public class Quiz
    {
        private string _name;
        //private bool _randomOrderQuestions;
        private bool _randomOrderAnswers;
        private List<Question> _questions;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value.ToString(); }
        }

        [XmlAttribute("RandomOrderAnswers")]
        public bool RandomOrderAnswers
        {
            get { return _randomOrderAnswers; }
            set { _randomOrderAnswers = value; }
        }

        [XmlArray("Questions")]
        public List<Question> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }
    }
}
