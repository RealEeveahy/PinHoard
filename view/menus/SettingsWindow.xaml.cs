using System;
using System.Windows;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
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
