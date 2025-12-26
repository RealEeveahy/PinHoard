using PinHoard.model.quiz;
using PinHoard.util;
using System.Windows.Media;

namespace PinHoard.viewmodel.question_vms
{
    public class Result_ViewModel
    {
        private readonly string _userAnswer;
        private readonly IQuizQuestion quizQuestion;
        public string CorrectAnswer => "Correct Answer: " + quizQuestion.answer;
        public string UserAnswer => _userAnswer;
        public string Question => quizQuestion.topic;
        public SolidColorBrush ResultColour =>
            PinHoardHelpers.ValidateQuizResponse(quizQuestion.answer, _userAnswer) ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.IndianRed);
        public string Indicator => PinHoardHelpers.ValidateQuizResponse(quizQuestion.answer, _userAnswer) ? "✔️" : "❌";

        public Result_ViewModel(IQuizQuestion q, string userAnswer)
        {
            quizQuestion = q;
            _userAnswer = userAnswer;
        }
    }
}
