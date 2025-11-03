using Microsoft.VisualBasic;
using PinHoard.model.save_load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PinHoard.model.quiz
{
    public class QuizModel
    {
        public int questionProgress = 1;
        public int correctAnswers = 0;
        public string selectedAnswer = "";
        Random r = new Random();
        List<QA_Pair> qA_Pairs = new List<QA_Pair>();
        public Dictionary<string, string> titleForContentDict = new Dictionary<string, string>();
        private List<Action> quizFunctions = new List<Action>();
        private List<IQuizQuestion> questionQueue = new List<IQuizQuestion>();

        public QuizWindow view;
        public QuizModel(List<string> filenames, int maxQuestions)
        {
            view = new QuizWindow();

            Load(filenames);

            // Build the question queue
            for(int i = 0; i < maxQuestions; i++)
            {
                questionQueue.Append(GetQuestion());
            }

            view.Show();
        }
        void Load(List<string> filenames)
        {
            List<SerializablePin> objects = new List<SerializablePin>();
                
            foreach(string s in filenames)
            {
                LoadData data = new(s);
                objects.Concat(data.GetDataOnly());
            }

            if (objects != null)
            {
                foreach (SerializablePin obj in objects)
                {
                    if (obj.stringList?.Count == 2)
                    {
                        qA_Pairs.Add(new QA_Pair(obj.stringList[0], obj.stringList[1]));
                    }
                }
            }
        }
        IQuizQuestion GetQuestion()
        {
            int rand = r.Next(2);

            if (rand == 0)
            {
                QA_Pair correctResponse = qA_Pairs.ElementAt(r.Next(qA_Pairs.Count));

                QA_Pair[] incorrectResponses = new QA_Pair[2];
                for (int i = 0; i < 2; i++)
                {
                    incorrectResponses[i] = qA_Pairs.ElementAt(r.Next(qA_Pairs.Count));
                }
                return new MultiChoice_Q(correctResponse.q, correctResponse.a, incorrectResponses[0].a, incorrectResponses[1].a);
            }
            else if (rand == 1)
            {
                QA_Pair correctResponse = qA_Pairs.ElementAt(r.Next(qA_Pairs.Count));

                QA_Pair[] incorrectResponses = new QA_Pair[2];
                for (int i = 0; i < 2; i++)
                {
                    incorrectResponses[i] = qA_Pairs.ElementAt(r.Next(qA_Pairs.Count));
                }
                return new MultiChoice_Q(correctResponse.a, correctResponse.q, incorrectResponses[0].q, incorrectResponses[1].q);
            }
            else
            {
                QA_Pair correctResponse = qA_Pairs.ElementAt(r.Next(qA_Pairs.Count));
                return new TypeGuess_Q(correctResponse.q, correctResponse.a);
            }
        }
        QA_Pair[] ShuffleArray(QA_Pair[] unshuffled)
        {
            QA_Pair[] shuffled = new QA_Pair[unshuffled.Length];
            List<int> positions = new List<int> { 0, 1, 2 };
            Random r = new Random();

            int i = 0;
            while (positions.Count > 0)
            {
                int rngIndex = r.Next(positions.Count);
                shuffled[positions[rngIndex]] = unshuffled[i];
                positions.RemoveAt(rngIndex);
                i++;
            }

            return shuffled;
        }
    }
}
