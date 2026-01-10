using System.Collections.Generic;

namespace PinHoard.model.quiz
{
    /// <summary>
    /// Represents the model for a quiz, containing the total number of questions and the sources from which to draw quiz content.
    /// </summary>
    public class Quiz_Model
    {
        public int questionCount;
        public Board source;
        public Quiz_Model(int questionCount, List<string> boardnames)
        {
            this.questionCount = questionCount;
            this.source = new Board(boardnames);
        }
    }
}
