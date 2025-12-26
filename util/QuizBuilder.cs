using PinHoard.model.pins;
using PinHoard.model.quiz;
using System;
using System.Collections.Generic;

namespace PinHoard.util
{
    public static class QuizBuilder
    {
        public static List<IQuizQuestion> GenerateQueue(int count, List<Pin_Model> viableSources)
        {
            List<IQuizQuestion> questions = new List<IQuizQuestion>();

            //store a list of used indexes to avoid duplicates
            List<int> usedIndexes = new List<int>();

            Random r = new Random();

            for (int i = 0; i < count; i++)
            {
                int[] randomChoices = new int[3];

                // Clear the used list if absolutely necessary
                if (usedIndexes.Count >= viableSources.Count) usedIndexes.Clear();

                //select index 0 to represent the correct answer - ensure it is unique
                while (usedIndexes.Contains(randomChoices[0] = r.Next(viableSources.Count))) randomChoices[0] = r.Next(viableSources.Count);

                // select 2 more indexes to represent wrong answers in case they are necessary
                // does not add these indexes to the used list as they can be reused
                while ((randomChoices[1] = r.Next(viableSources.Count)) == randomChoices[0]) randomChoices[1] = r.Next(viableSources.Count);
                while ((randomChoices[2] = r.Next(viableSources.Count)) == randomChoices[0] && (randomChoices[2] == randomChoices[1])) randomChoices[2] = r.Next(viableSources.Count);

                questions.Add(GenerateQuestion(
                    viableSources[randomChoices[0]],
                    viableSources[randomChoices[1]],
                    viableSources[randomChoices[2]]
                    ));
                usedIndexes.Add(randomChoices[0]);
            }

            return questions;
        }
        static IQuizQuestion GenerateQuestion(Pin_Model pin1, Pin_Model pin2, Pin_Model pin3)
        {
            IQuizQuestion question;
            Random r = new Random();
            int questionType = r.Next(3);

            switch (questionType)
            {
                case 0:
                    // Guess from multiple definitions
                    question = new MultiChoiceQ_Model(
                        pin1.componentList[0].GetContent(),
                        "Is best described as:",
                        pin1.componentList[1].GetContent(),
                        pin2.componentList[1].GetContent(),
                        pin3.componentList[1].GetContent()
                        );
                    break;
                case 1:
                    // Guess from multiple terms
                    question = new MultiChoiceQ_Model(
                        pin1.componentList[1].GetContent(),
                        "Best describes:",
                        pin1.componentList[0].GetContent(),
                        pin2.componentList[0].GetContent(),
                        pin3.componentList[0].GetContent()
                        );
                    break;
                case 2:
                    // Guess the term in free text
                    question = new FreeTextQ_Model(
                        pin1.componentList[1].GetContent(),
                        "Best describes:",
                        pin1.componentList[0].GetContent()
                        );
                    break;
                default:
                    // Fallback to free text as it is the simplest to define
                    question = new FreeTextQ_Model(
                        pin1.componentList[1].GetContent(),
                        "Best describes:",
                        pin1.componentList[0].GetContent()
                        );
                    break;
            }

            return question;
        }
    }
}
