using PinHoard.viewmodel.menus;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard
{
    /// <summary>
    /// Represents a temporary window that provides the ability to select multiple boards for compilation
    /// </summary>
    public partial class CompileSetupWindow : Window
    {
        public CompileSetupWindow(Main_ViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            StartButton.Click += vm.Compile;
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
