using PinHoard.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FileSaveWindow : Window
    {
        FileSaveViewModel context;
        public FileSaveWindow(FileSaveViewModel context)
        {
            InitializeComponent();
            this.context = context;

            SaveButton.Click += JustSave;
            SaveCloseButton.Click += SaveClose;
            CancelButton.Click += CancelSave;
        }
        public void JustSave(object sender, RoutedEventArgs e)
        {
            ErrorMessage(context.ValidateFilename(FilenameEntry.Text, false));
        }
        public void SaveClose(object sender, RoutedEventArgs e)
        {
            ErrorMessage(context.ValidateFilename(FilenameEntry.Text, true));
        }
        void ErrorMessage(string issue)
        {
            if(!string.IsNullOrEmpty(issue))
            MessageBox.Show($"{issue}.", "Invalid Filename", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        void CancelSave(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
