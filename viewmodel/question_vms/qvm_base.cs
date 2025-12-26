using PinHoard.model.quiz;

namespace PinHoard.viewmodel.question_vms
{
    public abstract class qvm_base
    {
        public IQuizQuestion questionModel;
        public abstract string UserAnswer { get; set; }
        protected qvm_base(IQuizQuestion model)
        {
            questionModel = model;
        }
    }
}
