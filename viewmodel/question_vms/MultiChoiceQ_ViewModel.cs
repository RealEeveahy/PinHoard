using PinHoard.model.quiz;

namespace PinHoard.viewmodel.question_vms
{
    class MultiChoiceQ_ViewModel : qvm_base
    {
        private string _answer = string.Empty;
        public string QuestionText => questionModel.topic;
        public string PromptText => questionModel.prompt;
        public string MC_1 => ((MultiChoiceQ_Model)questionModel).answer;
        public string MC_2 => ((MultiChoiceQ_Model)questionModel).incorrect1;
        public string MC_3 => ((MultiChoiceQ_Model)questionModel).incorrect2;
        public override string UserAnswer
        {
            get => _answer;
            set { _answer = value; }
        }
        public MultiChoiceQ_ViewModel(IQuizQuestion q) : base(q) { }
    }
}
