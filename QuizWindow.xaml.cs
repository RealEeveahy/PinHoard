using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static PinHoard.BoardWindow;
using System.Text.Json;

namespace PinHoard
{
    public partial class QuizWindow : Window
    {
        public int questionProgress = 1;
        public int totalQuestions;
        public int correctAnswers = 0;
        public string selectedAnswer = "";
        public string correctString = "";
        public Dictionary<string,string> titleForContentDict = new Dictionary<string,string>();
        public Dictionary<Button, TextBlock> buttonChildContentDict = new Dictionary<Button, TextBlock>();
        private List<Action> quizFunctions = new List<Action>();
        public QuizWindow(List<string> filenames, int questions)
        { 
            InitializeComponent();
            quizFunctions.Add(GuessFromDefinitionMultiChoice);
            quizFunctions.Add(GuessFromDefinitionTextEntry);
            quizFunctions.Add(GuessFromTermMultiChoice);

            totalQuestions = questions;
            SubmitButton.Click += CheckResponse;

            //populate the dictionary so the text content of buttons can be grabbed
            buttonChildContentDict[GFT_Button1] = GFT_Answer1;
            buttonChildContentDict[GFT_Button2] = GFT_Answer2;
            buttonChildContentDict[GFT_Button3] = GFT_Answer3;

            buttonChildContentDict[GFD_Button1] = GFD_Answer1;
            buttonChildContentDict[GFD_Button2] = GFD_Answer2;
            buttonChildContentDict[GFD_Button3] = GFD_Answer3;

            GFT_Button1.Click += AnswerSelected;
            GFT_Button2.Click += AnswerSelected;
            GFT_Button3.Click += AnswerSelected;

            GFD_Box.TextChanged += AnswerEntered;

            GFD_Button1.Click += AnswerSelected;
            GFD_Button2.Click += AnswerSelected;
            GFD_Button3.Click += AnswerSelected;

            LoadContents(filenames);

            NextQuestion();
        }
        void LoadContents(List<string> filenames)
        {
            //get the current path
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = System.IO.Path.Combine(currentPath, "boards");

            //process definitions
            foreach (string filename in filenames)
            {
                string filePath = System.IO.Path.Combine(directoryPath, filename);
                string json = File.ReadAllText(filePath);
                SaveData? data = JsonSerializer.Deserialize<SaveData>(json);

                if (data != null)
                {
                    List<PinDataObject>? contents = data.myPinObjects;
                    if (contents != null)
                    {
                        foreach (PinDataObject obj in contents)
                        {
                            if (obj.stringList?.Count == 2)
                            {
                                titleForContentDict[obj.stringList[0]] = obj.stringList[1];
                            }
                        }
                    }
                }
            }
        }
        void NextQuestion()
        {
            //update the question counter
            ProgressLabel.Content = questionProgress.ToString() + "/" + totalQuestions.ToString();
            //set the selected button back to grey
            ClearAllButtonColour();
            //disable all grids
            GuessFromTermGrid.Visibility = Visibility.Hidden;
            GuessFromDefMultiChoiceGrid.Visibility = Visibility.Hidden;
            GuessFromDefTextEntryGrid.Visibility = Visibility.Hidden;

            if (questionProgress <  totalQuestions + 1)
            {
                //decide the next question type
                Random r = new Random();
                quizFunctions[r.Next(quizFunctions.Count)]();
            }
            else
            {
                //display a results screen
                ResultsGrid.Visibility = Visibility.Visible;
                ProgressLabel.Visibility = Visibility.Hidden;
                SubmitButton.Visibility = Visibility.Hidden;

                FinalScoreLabel.Content = correctAnswers.ToString() + "/" + totalQuestions.ToString();

                //display a message about the users score
                float scoreAsPercent = (float)correctAnswers / (float)totalQuestions;
                if (scoreAsPercent < 0.25) SnarkyLabel.Content = "Total failure!";
                else if (scoreAsPercent < 0.5) SnarkyLabel.Content = "Might wanna study harder!";
                else if (scoreAsPercent < 0.7) SnarkyLabel.Content = "Almost good enough!";
                else if (scoreAsPercent < 1) SnarkyLabel.Content = "Great performance!";
                else if (scoreAsPercent == 1) SnarkyLabel.Content = "Perfect Score!";
            }
        }
        void GuessFromTermMultiChoice()
        {
            //enable the necessary grid
            GuessFromTermGrid.Visibility = Visibility.Visible;
            Random r = new Random();
            string chosenTerm = titleForContentDict.ElementAt(r.Next(titleForContentDict.Count)).Key;

            TermLabel.Text = chosenTerm;
            correctString = titleForContentDict[chosenTerm];

            string[] givenResponses = new string[3];
            for(int i = 0; i < 2; i++)
            {
                givenResponses[i] = titleForContentDict.ElementAt(r.Next(titleForContentDict.Count)).Value;
            }
            givenResponses[2] = correctString;

            //shuffle the given responses list
            givenResponses = ShuffleArray(givenResponses); 

            GFT_Answer1.Text = givenResponses[0];
            GFT_Answer2.Text = givenResponses[1];
            GFT_Answer3.Text = givenResponses[2];
        }
        void GuessFromDefinitionMultiChoice()
        {
            //enable the necessary grid
            GuessFromDefMultiChoiceGrid.Visibility = Visibility.Visible;
            Random r = new Random();
            correctString = titleForContentDict.ElementAt(r.Next(titleForContentDict.Count)).Key;

            string chosenDefinition = '"' + titleForContentDict[correctString].Replace(".", string.Empty) + '"';
            DefinitionLabel.Text = chosenDefinition;

            string[] givenResponses = GenerateGivenResponses(r, correctString);

            //shuffle the given responses list
            givenResponses = ShuffleArray(givenResponses);

            GFD_Answer1.Text = givenResponses[0];
            GFD_Answer2.Text = givenResponses[1];
            GFD_Answer3.Text = givenResponses[2];
        }
        void GuessFromDefinitionTextEntry()
        {
            //enable the necessary grid
            GuessFromDefTextEntryGrid.Visibility = Visibility.Visible;
            Random r = new Random();
            correctString = titleForContentDict.ElementAt(r.Next(titleForContentDict.Count)).Key;

            string chosenDefinition = '"' + titleForContentDict[correctString].Replace(".", string.Empty) + '"';
            DefinitionLabel2.Text = chosenDefinition;

            GFD_Box.Text = "";
        }
        string[] GenerateGivenResponses(Random r, string correctString)
        {
            string[] givenResponses = new string[3];
            for (int i = 0; i < 2; i++)
            {
                givenResponses[i] = titleForContentDict.ElementAt(r.Next(titleForContentDict.Count)).Key;
            }
            givenResponses[2] = correctString;

            return givenResponses;
        }
        string[] ShuffleArray(string[] unshuffled)
        {
            string[] shuffled = new string[unshuffled.Length];
            List<int> positions = new List<int>{ 0, 1, 2 };
            Random r = new Random();

            int i = 0;
            while(positions.Count > 0)
            {
                int rngIndex = r.Next(positions.Count);
                shuffled[positions[rngIndex]]= unshuffled[i];
                positions.RemoveAt(rngIndex);
                i++;
            }

            return shuffled;
        }
        public void AnswerSelected(object sender, RoutedEventArgs e)
        {
            ClearAllButtonColour();

            Button thisButton = (Button)sender;
            thisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6495ED"));

            TextBlock buttonChild = buttonChildContentDict[thisButton];
            selectedAnswer = buttonChild.Text;
        }
        public void AnswerEntered(object sender, RoutedEventArgs e)
        {
            TextBox thisTB = (TextBox)sender;
            selectedAnswer = thisTB.Text;
        }
        void ClearAllButtonColour()
        {
            SolidColorBrush grey = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            foreach (Button b in buttonChildContentDict.Keys)
            {
                b.Background = grey;
            }
        }
        void CheckResponse(object sender, RoutedEventArgs e)
        {
            if(selectedAnswer != null && selectedAnswer != "") 
            {
                if(selectedAnswer.ToLower() == correctString.ToLower())
                {
                    correctAnswers++;
                }

                questionProgress++;
                selectedAnswer = "";
                NextQuestion();
            }
        }
    }
}
