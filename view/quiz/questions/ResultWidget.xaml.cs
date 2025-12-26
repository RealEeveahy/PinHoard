using PinHoard.viewmodel.question_vms;
using System.Windows.Controls;

namespace PinHoard.view.quiz.questions
{
    /// <summary>
    /// Interaction logic for ResultWidget.xaml
    /// </summary>
    public partial class ResultWidget : UserControl
    {
        public ResultWidget(Result_ViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
