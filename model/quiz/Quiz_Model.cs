using System.Collections.Generic;

namespace PinHoard.model.quiz
{
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
