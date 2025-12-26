using System;
using System.Windows;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FileSaveWindow : Window
    {
        public FileSaveWindow(Action<string> returnFilename)
        {
            InitializeComponent();

            SaveButton.Click += (sender, e) => { returnFilename(FilenameEntry.Text); this.Close(); };
            SaveCloseButton.Click += (sender, e) => { returnFilename(FilenameEntry.Text); this.Close(); }; // fix
            CancelButton.Click += (sender, e) => { this.Close(); };
        }
    }
}
