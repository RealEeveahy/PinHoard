using PinHoard.viewmodel.menus;
using System.Windows;

namespace PinHoard
{
    public partial class CompileSetupWindow : Window
    {
        readonly Main_ViewModel viewModel;
        public CompileSetupWindow(Main_ViewModel vm)
        {
            viewModel = vm;
            InitializeComponent();

            StartButton.Click += vm.Compile;

            Build();
        }
        void Build()
        {
            for (int i = 0; i < viewModel.fileCount; i++)
            {
                viewModel.ShowSelectable(i).SetParent(SelectBoardPanel);
            }
            if (SelectBoardPanel.Children.Count > 0) MessageBox.Show("No boards to revise.");

            this.Height = 600 + (30 * viewModel.fileCount);
        }
    }
}
