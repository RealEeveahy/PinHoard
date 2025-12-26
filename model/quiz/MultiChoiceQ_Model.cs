namespace PinHoard.model.quiz
{
    class MultiChoiceQ_Model : IQuizQuestion
    {
        public string topic { get; set; }
        public string prompt { get; set; }
        public string answer { get; set; }
        public string incorrect1 { get; set; }
        public string incorrect2 { get; set; }
        public MultiChoiceQ_Model(string topic, string prompt, string answer, string incorrect1, string incorrect2)
        {
            this.topic = topic;
            this.prompt = prompt;
            this.answer = answer;
            this.incorrect1 = incorrect1;
            this.incorrect2 = incorrect2;
        }
    }
}
