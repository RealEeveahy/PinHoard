using PinHoard.model.quiz;

namespace PinHoard.viewmodel.question_vms
{
    /// <summary>
    /// The abstract base class for all quiz question viewmodels
    /// </summary>
    /// <remarks>
    /// Exists because each specific question type needs to implement its own view, 
    /// and by extension its own viewmodel for data binding
    /// while also being usable in the Quiz_ViewModel's CurrentQuestionViewModel property
    /// </remarks>
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
