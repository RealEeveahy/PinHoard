namespace PinHoard.model.quiz
{
    /// <summary>
    /// The model for a free text quiz question.
    /// This question type requires the user to input text as an answer.
    /// </summary>
    class FreeTextQ_Model : IQuizQuestion
    {
        public string topic { get; set; }
        public string prompt { get; set; }
        public string answer { get; set; }
        public FreeTextQ_Model(string topic, string prompt, string answer)
        {
            this.topic = topic;
            this.prompt = prompt;
            this.answer = answer;
        }
    }
}
