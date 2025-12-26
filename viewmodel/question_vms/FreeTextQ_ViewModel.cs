using PinHoard.model.quiz;

namespace PinHoard.viewmodel.question_vms
{
    class FreeTextQ_ViewModel : qvm_base
    {
        private string _answer = string.Empty;
        public string QuestionText => questionModel.topic;
        public string PromptText => questionModel.prompt;
        public override string UserAnswer
        {
            get => _answer;
            set { _answer = value; }
        }
        public FreeTextQ_ViewModel(IQuizQuestion q) : base(q) { }
    }
}
