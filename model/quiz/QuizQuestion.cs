using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinHoard.model.quiz
{
    internal interface IQuizQuestion
    {
        string question { get; set; }
        string correctAnswer { get; set; }
        string BuildQuestion(string topic);
    }

    class MultiChoice_Q : IQuizQuestion
    {
        public string question { get; set; }
        public string correctAnswer { get; set; }
        public string incorrect1 { get; set; }
        public string incorrect2 { get; set; }

        public MultiChoice_Q(string topic, string correctAnswer, string incorrect1, string incorrect2)
        {
            question = BuildQuestion(topic);
            this.correctAnswer = correctAnswer;
            this.incorrect1 = incorrect1;
            this.incorrect2 = incorrect2;
        }

        public string BuildQuestion(string topic)
        {
            return "'" + topic + "' best describes:";
        }
    }

    class TypeGuess_Q : IQuizQuestion
    {
        public string question { get; set; }
        public string correctAnswer { get; set; }
        public TypeGuess_Q(string topic, string correctAnswer)
        {
            question = BuildQuestion(topic);
            this.correctAnswer = correctAnswer;
        }
        public string BuildQuestion(string topic)
        {
            return "'" + topic + "' best describes:";
        }
    }
}
