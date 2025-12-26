namespace PinHoard.model.quiz
{
    public interface IQuizQuestion
    {
        string topic { get; set; }
        string prompt { get; set; }
        string answer { get; set; }
    }
}
