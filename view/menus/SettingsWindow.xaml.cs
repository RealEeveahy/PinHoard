using System;
using System.Windows;

namespace PinHoard
{
    /// <summary>
    /// Represents the settings window for a given file
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(string filename, int index, Action<int> delete)
        {
            InitializeComponent();
            this.Title = $"{filename} Settings";
            TitleLabel.Content = $"Settings for '{filename}'";
            RenameBox.Text = filename;

            DeleteButton.Click += (sender, e) =>
            {
                delete(index);
                this.Close();
            };
            CloseButton.Click += (sender, e) => { this.Close(); };
            SaveChangeButton.Click += (sender, e) => { this.Close(); }; //change
        }
    }
}
