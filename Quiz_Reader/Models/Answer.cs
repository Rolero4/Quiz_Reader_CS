using System;
using System.Xml.Serialization;

namespace Quiz_Generator.Models
{
    [Serializable]
    public class Answer
    {
        private string _content;
        private bool _correct;

        [XmlAttribute("Content")]
        public string Content
        {
            get { return _content; }
            set { _content = value.ToString(); }
        }

        [XmlAttribute("Correct")]
        public bool Correct
        {
            get { return _correct; }
            set { _correct = value; }
        }

        public override string ToString()
        {
            return _content;
        }
    }
}
