using PinHoard.viewmodel.menus;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard
{
    /// <summary>
    /// Represents a temporary window that provides the ability to select multiple boards for revision
    /// </summary>
    public partial class QuizSetupWindow : Window
    {
        int questionCount = 10;
        public QuizSetupWindow(Main_ViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            StartButton.Click += (sender, e) => { vm.Revise(questionCount, CloseThis); };

            QuestionCountBox.TextChanged += SetCount;
        }
        void SetCount(object sender, RoutedEventArgs e) { if (!int.TryParse(QuestionCountBox.Text, out questionCount)) questionCount = 0; }
        public void CloseThis()
        {
            this.Close();
        }

        private void BoardCheck_Checked(object sender, RoutedEventArgs e)
        {
            var s = (CheckBox)sender;
            ((Main_ViewModel)DataContext).UpdateSelection(s.Tag.ToString());
        }
        private void BoardCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            var s = (CheckBox)sender;
            ((Main_ViewModel)DataContext).UpdateSelection(s.Tag.ToString(),true);
        }
    }
}
