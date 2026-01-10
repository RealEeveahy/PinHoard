namespace PinHoard.model.quiz
{
    /// <summary>
    /// Defines the contract for any question in a quiz.
    /// </summary>
    public interface IQuizQuestion
    {
        /// <summary>
        /// The subject of the question - may be a term, or definition, depending on context.
        /// </summary>
        string topic { get; set; }
        /// <summary>
        /// The text that represents the question being asked as a bridge between topic and answer.
        /// </summary>
        /// <example>"Is best described as:"</example>
        string prompt { get; set; }
        /// <summary>
        /// The corresponding response to the topic.
        /// </summary>
        string answer { get; set; }
    }
}
