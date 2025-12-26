using PinHoard.viewmodel.menus;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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

            if (viewModel?.boardnames is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += Files_CollectionChanged;
            }

            Build(viewModel.boardnames);
        }
        private void Files_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataContext is Main_ViewModel vm)
                Build(vm.boardnames);
        }
        void Build(IEnumerable<string>? boards)
        {
            BoardGrid.Children.Clear();

            for (int i = 0; i < boards.Count(); i++)
            {
                viewModel.ShowFile(i).SetParent(BoardGrid);
            }
            if (BoardGrid.Children.Count > 0) NoBoardsLabel.Visibility = Visibility.Hidden;
        }
    }
}
