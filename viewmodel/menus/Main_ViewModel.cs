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
    /// <summary>
    /// Represents the view model for the menu screen of the application.
    /// </summary>
    public class Main_ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> boardnames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<FileWidget> FileWidgets { get; set; } = new ObservableCollection<FileWidget>();
        //public int fileCount => boardnames.Count();

        readonly List<int> selectedBoards = new List<int>();

        // Due to change. Render the most recent boards instead.
        private readonly string boardPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "boards");
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
                FileWidgets.Add(new FileWidget(new FileWidget_ViewModel(file.Name)));
            }
        }
        public void UpdateSelection(string filename, bool remove = false)
        {
            int index = boardnames.IndexOf(filename);
            if (remove) selectedBoards.Remove(index);
            else selectedBoards.Add(index);
        }
        public void OpenBoard(string filename)
        {
            Board_ViewModel _board = new Board_ViewModel(new Board(filename));
            _board.ReloadMain = LoadAllBoards;
        }
        public void ModifyBoard(string filename)
        {
            string board = filename;
            string thisFullPath = Path.Combine(boardPath, board);

            SettingsWindow settingsWindow = new(board, DeleteBoard);
            settingsWindow.ShowDialog();
        }
        public void DeleteBoard(string filename)
        {
            string board = filename;
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
        public void OpenSettings(object sender, RoutedEventArgs e)
        {
            PreferencesWindow prefs = new PreferencesWindow(this);
            prefs.ShowDialog();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
