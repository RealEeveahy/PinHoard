using PinHoard.viewmodel;
using System.Windows;

namespace PinHoard.view.quiz
{
    /// <summary>
    /// Interaction logic for QuizShell.xaml
    /// </summary>
    public partial class QuizShell : Window
    {
        readonly Quiz_ViewModel vm;
        public QuizShell(Quiz_ViewModel vm)
        {
            DataContext = vm;
            this.vm = vm;
            InitializeComponent();

            SubmitButton.Click += (sender, e) => Submit();
            RetryButton.Click += (sender, e) => vm.ResetQuiz();

            this.Show();
        }
        public void Submit()
        {
            if (vm.CurrentQuestionViewModel.UserAnswer != string.Empty)
                vm.SubmitAnswer(vm.CurrentQuestionViewModel.UserAnswer, this);
        }
    }
}
