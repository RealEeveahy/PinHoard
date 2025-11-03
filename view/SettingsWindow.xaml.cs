using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        string boardname = string.Empty;
        string path = string.Empty;
        public SettingsWindow(string filename, string fullPath)
        {
            InitializeComponent();
            boardname = filename;
            path = fullPath;
            this.Title = $"{boardname} Settings";
            TitleLabel.Content = $"Settings for '{boardname}'";
            RenameBox.Text = boardname;

            DeleteButton.Click += DeleteBoard;
            CloseButton.Click += CloseSettings;
            SaveChangeButton.Click += SaveChanges;
        }
        public void DeleteBoard(object sender, RoutedEventArgs e)
        {
            File.Delete(path);
            MainWindow? m = GetWindow(App.Current.MainWindow) as MainWindow;
            m?.LoadAllBoards();
            this.Close();
        }
        public void CloseSettings(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void SaveChanges(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
