using PinHoard.viewmodel.menus;
using System.Windows;

namespace PinHoard
{
    public partial class QuizSetupWindow : Window
    {
        int questionCount = 10;
        readonly Main_ViewModel viewModel;
        public QuizSetupWindow(Main_ViewModel vm)
        {
            viewModel = vm;
            InitializeComponent();

            StartButton.Click += (sender, e) => { vm.Revise(questionCount, CloseThis); };

            QuestionCountBox.TextChanged += SetCount;

            Build();
        }
        void Build()
        {
            for (int i = 0; i < viewModel.fileCount; i++)
            {
                viewModel.ShowSelectable(i).SetParent(SelectBoardPanel);
            }
            if (SelectBoardPanel.Children.Count == 0) MessageBox.Show("No boards to revise.");

            this.Height = 600 + (30 * viewModel.fileCount);
        }
        void SetCount(object sender, RoutedEventArgs e) { if (!int.TryParse(QuestionCountBox.Text, out questionCount)) questionCount = 0; }
        public void CloseThis()
        {
            this.Close();
        }
    }
}
