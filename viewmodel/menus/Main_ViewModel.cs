using PinHoard.model;
using PinHoard.model.quiz;
using PinHoard.util;
using PinHoard.view.menus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PinHoard.viewmodel.menus
{
    public class Main_ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> boardnames = new ObservableCollection<string>();
        public int fileCount => boardnames.Count();

        readonly List<int> selectedBoards = new List<int>();

        // Due to change. Render the most recent boards instead.
        private readonly string boardPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "boards");
        public FileWidget ShowFile(int index) { return new FileWidget(PinHoardHelpers.CutExtension(boardnames[index]), index, this); }
        public BoardSelectWidget ShowSelectable(int index) { return new BoardSelectWidget(PinHoardHelpers.CutExtension(boardnames[index]), index, UpdateSelection); }
        public Main_ViewModel()
        {
            LoadAllBoards();
        }
        public void LoadAllBoards()
        {
            boardnames.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(boardPath);

            foreach (var file in directoryInfo.GetFiles("*.json"))
            {
                boardnames.Add(file.Name);
            }
        }
        public void UpdateSelection(int index, bool remove = false)
        {
            if (remove) selectedBoards.Remove(index);
            else selectedBoards.Add(index);
        }
        public void OpenBoard(int index)
        {
            Board_ViewModel _board = new Board_ViewModel(new Board(boardnames[index]));
            _board.ReloadMain = LoadAllBoards;
        }
        public void ModifyBoard(int index)
        {
            string board = boardnames[index];
            string thisFullPath = Path.Combine(boardPath, board);

            SettingsWindow settingsWindow = new(board, index, DeleteBoard);
            settingsWindow.ShowDialog();
        }
        public void DeleteBoard(int index)
        {
            string board = boardnames[index];
            string fullPath = Path.Combine(boardPath, board);

            File.Delete(fullPath);
            LoadAllBoards();
        }
        public void NewBoard(object sender, RoutedEventArgs e)
        {
            Board_ViewModel newBoard = new(false);
            newBoard.ReloadMain = LoadAllBoards;
        }
        public void ConfigureRevision(object sender, RoutedEventArgs e)
        {
            selectedBoards.Clear();

            QuizSetupWindow quizSetup = new QuizSetupWindow(this);
            quizSetup.ShowDialog();
        }
        public void Revise(int questionCount, Action EndConfig)
        {
            if (selectedBoards.Count < 1)
            {
                MessageBox.Show("Please select one or more boards to compile.");
                return;
            }
            if (questionCount < 1)
            {
                MessageBox.Show("Please enter a valid number of questions.");
                return;
            }

            // TODO: Code to start quiz here
            List<string> filenames = new List<string>();
            foreach (int i in selectedBoards)
                filenames.Add(boardnames[i]);

            Quiz_ViewModel quiz_vm = new Quiz_ViewModel(new Quiz_Model(questionCount, filenames));
            EndConfig();
        }
        public void ConfigureCompilation(object sender, RoutedEventArgs e)
        {
            selectedBoards.Clear();

            CompileSetupWindow compileSetup = new CompileSetupWindow(this);
            compileSetup.ShowDialog();
        }
        public void Compile(object sender, RoutedEventArgs e)
        {
            if (selectedBoards.Count < 1)
            {
                MessageBox.Show("Please select one or more boards to compile.");
                return;
            }

            List<string> filenames = new List<string>();
            foreach (int i in selectedBoards)
                filenames.Add(boardnames[i]);

            Board_ViewModel _board = new Board_ViewModel(new Board(filenames), true);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
