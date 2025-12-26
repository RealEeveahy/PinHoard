using PinHoard.viewmodel.question_vms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinHoard.view.quiz.questions
{
    /// <summary>
    /// Interaction logic for MultiChoiceQuestion.xaml
    /// </summary>
    public partial class MultiChoiceQuestion : UserControl
    {
        public MultiChoiceQuestion()
        {
            InitializeComponent();
            ResetAll();
        }
        void ResetAll()
        {
            // reset all buttons to deselected color
            foreach (UIElement b in Buttons.Children)
            {
                if (b is not Button) continue;

                SolidColorBrush deselected = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
                ((Button)b).Background = deselected;
            }
        }
        public void SwitchSelectedAnswer(object sender, RoutedEventArgs e)
        {
            ResetAll();

            // set selected button to selected color
            ((Button)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6495ED"));

            // update VM with selected answer
            ((MultiChoiceQ_ViewModel)DataContext).UserAnswer = ((TextBlock)((Button)sender).Content).Text.ToString() ?? string.Empty;
        }
    }
}
