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
                BoardGrid.Children.Add(newFW.WidgetGrid);
                files++;
            }
            if (files > 0) NoBoardsLabel.Visibility = Visibility.Hidden;
        }
        private void NewBoardClicked(object sender, RoutedEventArgs e)
        {
            BoardWindow boardWindow = new BoardWindow(false);
            boardWindow.Show();
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
        public class FileWidget
        {
            public Grid WidgetGrid;
            public Border WidgetBorder;
            public Label WidgetLabel;
            public Button OpenBoard;
            public Button BoardOptions;
            public string fName { get; set; }
            public Grid WidgetParent;
            public FileWidget(Grid parent, string fn, int index, string fullPath)
            {
                WidgetParent = parent;
                fName = fn;

                //create a grid
                WidgetGrid = new Grid();
                WidgetGrid.Height = 60;
                WidgetGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3"));
                WidgetGrid.VerticalAlignment = VerticalAlignment.Top;
                WidgetGrid.Margin = new Thickness(20, index * 60 + (20 + index * 20), 20, 0);

                //create border
                WidgetBorder = new Border();
                WidgetBorder.HorizontalAlignment = HorizontalAlignment.Center;
                WidgetBorder.VerticalAlignment = VerticalAlignment.Top;
                WidgetBorder.CornerRadius = new CornerRadius(10, 10, 10, 10);
                WidgetBorder.BorderThickness = new Thickness(1); 

                WidgetLabel = new Label();
                WidgetLabel.HorizontalAlignment = HorizontalAlignment.Left;
                WidgetLabel.VerticalAlignment = VerticalAlignment.Center;
                WidgetLabel.Margin = new Thickness(20, 0, 0, 0);
                WidgetLabel.Content = fn;

                OpenBoard = new Button();
                OpenBoard.HorizontalAlignment = HorizontalAlignment.Right;
                OpenBoard.VerticalAlignment = VerticalAlignment.Center;
                OpenBoard.Height = 40;
                OpenBoard.Width = 60;
                OpenBoard.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF69176"));
                OpenBoard.BorderBrush = null;
                OpenBoard.Margin = new Thickness(0, 0, 20, 0);
                OpenBoard.Content = "Open";

                BoardOptions = new Button();
                BoardOptions.HorizontalAlignment = HorizontalAlignment.Right;
                BoardOptions.VerticalAlignment = VerticalAlignment.Center;
                BoardOptions.Height = 40;
                BoardOptions.Width = 40;
                BoardOptions.Margin = new Thickness(0, 0, 100, 0);

                Grid optionsSubGrid = new Grid();
                Image optionIcon = new Image();
                optionIcon.Source = new BitmapImage(new Uri("images/miniSettingGraphic.png", UriKind.Relative));
                optionIcon.Stretch = Stretch.Fill;
                optionsSubGrid.Children.Add(optionIcon);
                BoardOptions.Content = (optionsSubGrid);

                //add widget content to grid
                WidgetGrid.Children.Add(WidgetBorder);
                WidgetGrid.Children.Add(WidgetLabel);
                WidgetGrid.Children.Add(OpenBoard);
                WidgetGrid.Children.Add(BoardOptions);

                OpenBoard.Click += (sender, e) =>
                {
                    BoardWindow boardWindow = new BoardWindow(false);
                    boardWindow.Show();
                    boardWindow.LoadAllPins(fn);
                };
                BoardOptions.Click += (sender, e) =>
                {
                    SettingsWindow settingsWindow = new SettingsWindow(fn, fullPath);
                    settingsWindow.ShowDialog();
                };
            }
        }
    }
}
