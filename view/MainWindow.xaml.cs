using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using static System.Net.WebRequestMethods;
using PinHoard.model.pins;
using PinHoard.model;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> boardnames = new List<string>();
        private string boardPath;
        public MainWindow()
        {
            InitializeComponent();

            boardPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "boards");

            LoadAllBoards();
            NewBoardButton.Click += NewBoardClicked;
            NewTool.Click += NewBoardClicked;
            QuizButton.Click += ReviseButtonClicked;
            ReviseTool.Click += ReviseButtonClicked;
            CompileTool.Click += CompileButtonClicked;
        }
        public void LoadAllBoards()
        {
            BoardGrid.Children.Clear();
            boardnames.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(boardPath);

            int files = 0;

            foreach(var file in directoryInfo.GetFiles("*.json")) 
            {
                string nameWithoutExtension = file.Name.Substring(0, file.Name.Length - 5);
                string thisFullPath = Path.Combine(boardPath, file.Name);
                FileWidget newFW = new FileWidget(BoardGrid, nameWithoutExtension, files, thisFullPath);
                boardnames.Add(file.Name);
                BoardGrid.Children.Add(newFW);
                files++;
            }
            if (files > 0) NoBoardsLabel.Visibility = Visibility.Hidden;
        }
        private void NewBoardClicked(object sender, RoutedEventArgs e)
        {
            Board board = new Board(false);
        }
        private void ReviseButtonClicked(object sender, RoutedEventArgs e)
        {
            QuizSetupWindow quizSetup = new QuizSetupWindow(boardnames);
            quizSetup.Show();
        }
        private void CompileButtonClicked(object sender, RoutedEventArgs e)
        {
            CompileSetupWindow compileSetup = new CompileSetupWindow(boardnames);
            compileSetup.Show();
        }
    }
}
