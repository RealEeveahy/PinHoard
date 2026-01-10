using System;
using System.Windows;

namespace PinHoard
{
    /// <summary>
    /// Represents a temporary window that prompts the user to enter a filename when a new file is being created
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
