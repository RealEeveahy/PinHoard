using PinHoard.model.pins;
using PinHoard.model.quiz;
using PinHoard.util;
using PinHoard.view.quiz;
using PinHoard.viewmodel.question_vms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PinHoard.viewmodel
{
    public class Quiz_ViewModel : INotifyPropertyChanged
    {
        readonly Quiz_Model model;
        List<Pin_Model> validPins = new List<Pin_Model>();

        public List<IQuizQuestion> questionQueue = new List<IQuizQuestion>();

        public IQuizQuestion CurrentQuestion;
        private qvm_base? _currentQuestionViewModel;
        public qvm_base? CurrentQuestionViewModel
        {
            get => _currentQuestionViewModel;
            set
            {
                if (_currentQuestionViewModel == value) return;
                _currentQuestionViewModel = value;
                OnPropertyChanged(nameof(CurrentQuestionViewModel));
            }
        }

        public Dictionary<IQuizQuestion, string> answerDictionary = new Dictionary<IQuizQuestion, string>();
        int _progress = 0;
        public int progress
        {
            get => _progress;
            set
            {
                _progress = value;
                UpdateQuestion();
                OnPropertyChanged(nameof(QuestionProgress));
                OnPropertyChanged(nameof(SubmitButtonContent));
                OnPropertyChanged(nameof(RetryVisibility));
            }
        }
        public bool Complete => progress >= questionQueue.Count;
        public string SubmitButtonContent => Complete ? "Finish" : "Submit";
        public string QuestionProgress => $"{progress}/{questionQueue.Count}";
        public Visibility RetryVisibility => Complete ? Visibility.Visible : Visibility.Collapsed;
        public Quiz_ViewModel(Quiz_Model model)
        {
            this.model = model;

            foreach (Pin_Model pin in model.source.allPins)
            {
                if (pin.componentList.Count < 2) continue;
                if (pin.componentList[0].format != "title") continue;
                if (pin.componentList[1].format != "content") continue;
                validPins.Add(pin);
            }

            if (validPins.Count < 3)
            {
                MessageBox.Show("The selected boards do not contain enough valid pins for a quiz. Please ensure that the selected boards contain pins with titles and at least one other component.");
                return;
            }
            questionQueue = QuizBuilder.GenerateQueue(model.questionCount, validPins);

            UpdateQuestion();

            _ = new QuizShell(this);
        }
        public void SubmitAnswer(string answer, Window window)
        {
            if (Complete && CurrentQuestionViewModel is ResultScreen_ViewModel)
            {
                // close the window only when the quiz is complete AND the results have been shown
                window.Close();
            }
            if (CurrentQuestion == null) return;
            answerDictionary[CurrentQuestion] = answer;

            progress++;
        }
        void UpdateQuestion()
        {
            if (progress < questionQueue.Count) CurrentQuestion = questionQueue[progress];

            CurrentQuestionViewModel = CreateVM(CurrentQuestion);
        }
        public void ResetQuiz()
        {
            questionQueue = QuizBuilder.GenerateQueue(model.questionCount, validPins);
            answerDictionary.Clear();
            progress = 0;
        }
        qvm_base CreateVM(IQuizQuestion question_model)
        {
            if (Complete)
            {
                // straight to results screen if all questions answered
                return new ResultScreen_ViewModel(CurrentQuestion, answerDictionary);
            }
            if (question_model is MultiChoiceQ_Model mcq)
            {
                return new MultiChoiceQ_ViewModel(question_model);
            }
            else if (question_model is FreeTextQ_Model ftq)
            {
                return new FreeTextQ_ViewModel(question_model);
            }
            else throw new NotImplementedException("Invalid question type was provided");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
