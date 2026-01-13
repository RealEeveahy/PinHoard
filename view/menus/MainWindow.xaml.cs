using PinHoard.view.menus;
using PinHoard.viewmodel.menus;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace PinHoard
{
    /// <summary>
    /// Represents the main application window containing the most recently accessed boards, 
    /// as well as options to start quizzes and compilations
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Main_ViewModel viewModel;
        public MainWindow()
        {
            viewModel = new();
            DataContext = viewModel;
            InitializeComponent();

            NewBoardButton.Click += viewModel.NewBoard;
            NewTool.Click += viewModel.NewBoard;
            QuizButton.Click += viewModel.ConfigureRevision;
            ReviseTool.Click += viewModel.ConfigureRevision;
            CompileTool.Click += viewModel.ConfigureCompilation;
            SettingsTool.Click += viewModel.OpenSettings;
        }
        private void FileWidget_OpenRequested(object sender, RoutedEventArgs e)
        {
            var widget = e.OriginalSource as FileWidget;
            if (widget != null)
            {
                var vm = widget.DataContext as FileWidget_ViewModel;
                viewModel.OpenBoard(vm.filename);
            }
        }
        private void FileWidget_SettingsRequest(object sender, RoutedEventArgs e)
        {
            var widget = e.OriginalSource as FileWidget;
            if (widget != null)
            {
                var vm = widget.DataContext as FileWidget_ViewModel;
                viewModel.ModifyBoard(vm.filename);
            }
        }
    }
}
