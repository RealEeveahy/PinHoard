using PinHoard.model.quiz;
using PinHoard.util;
using PinHoard.view.quiz.questions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PinHoard.viewmodel.question_vms
{
    /// <summary>
    /// 
    /// - Inherits from qvm_base so that it can be used in the same content control as the questions (simplifies xaml a lot)
    /// </summary>
    public class ResultScreen_ViewModel : qvm_base
    {
        private string _answer = "Placeholder"; // allow for non-empty answer to enable submit button
        public override string UserAnswer
        {
            get => _answer;
            set { _answer = value; }
        }
        public ObservableCollection<ResultWidget> Results { get; set; } = new ObservableCollection<ResultWidget>();
        private int _score = 0;
        public string ScoreText => $"You scored: {_score}/{Results.Count} ({ MathF.Round(((float)_score/Results.Count)*100, 2) }%)";
        public string CommentText => _comment;
        public string _comment
        {
            get
            {
                float percentage = ((float)_score / Results.Count);
                if (percentage < .25)
                    return comments[0];
                else if (percentage < .5)
                    return comments[1];
                else if (percentage < .7)
                    return comments[2];
                else if (percentage < 1)
                    return comments[3];
                else if (percentage == 1)
                    return comments[4];
                else
                    return "Error calculating comment.";
            }
        }

        private string[] comments = new string[]
        {
            "Total failure!",
            "Might wanna study harder!",
            "Almost good enough!",
            "Great performance!",
            "Perfect Score!"
        };

        public ResultScreen_ViewModel(IQuizQuestion q, Dictionary<IQuizQuestion, string> answerDict) : base(q)
        {
            foreach (var kvp in answerDict)
            {
                if(PinHoardHelpers.ValidateQuizResponse(kvp.Key.answer, kvp.Value))
                    _score++;

                Results.Add(new ResultWidget(new viewmodel.question_vms.Result_ViewModel(kvp.Key, kvp.Value)));
            }
        }
    }
}
