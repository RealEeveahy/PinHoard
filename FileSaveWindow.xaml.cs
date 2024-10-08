using System;
using System.Collections.Generic;
using System.Linq;
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
        private BoardWindow parentWindow;
        public FileSaveWindow(BoardWindow board)
        {
            InitializeComponent();

            parentWindow = board;
            
            SaveButton.Click += JustSave;
            SaveCloseButton.Click += SaveClose;
            CancelButton.Click += CancelSave;
        }
        public void JustSave(object sender, RoutedEventArgs e)
        {
            ValidateFilename(false);
        }
        public void SaveClose(object sender, RoutedEventArgs e)
        {
            ValidateFilename(true);
        }
        public void ValidateFilename(bool close)
        { 
            if (string.IsNullOrEmpty(FilenameEntry.Text) || string.IsNullOrWhiteSpace(FilenameEntry.Text))
            {
                ErrorMessage("Filename cannot be empty.");
                return;
            }
            //Regex rg = new Regex(@"^[a-zA-Z0-9 . _ -]*$");
            Regex rg = new Regex(@"^[\w\-. ]*$");
            if (rg.IsMatch(FilenameEntry.Text))
            {
                parentWindow.boardName = FilenameEntry.Text;
                this.Close();
                if(close) { parentWindow.closeAfterSave = true; }
            }
            else
            {
                ErrorMessage("An invalid character was entered.");
                return;
            }
        }
        void ErrorMessage(string issue)
        {
            MessageBox.Show($"{issue}.", "Invalid Filename", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        void CancelSave(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
