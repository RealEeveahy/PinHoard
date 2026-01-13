using PinHoard.Properties;
using PinHoard.util;
using PinHoard.view.style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PinHoard.view.menus
{
    /// <summary>
    /// Represents a window that allows the user to interact with application settings
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        public PreferencesWindow(object context)
        {
            //DataContext = context;
            InitializeComponent();

            // Fill style combobox with pre-defined style options
            List<AppStyle> styles = StyleManager.StyleLibrary.Values.ToList();
            List<string> styleNames = new List<string>();
            foreach(AppStyle style in styles) styleNames.Add(style.StyleName);

            ThemeSelector.ItemsSource = styleNames;
            ThemeSelector.SelectedIndex = Settings.Default.AppStylePref;

            // Fill window combobox with options
            List<string> windowStates = new List<string>() { "Normal", "Maximized" };
            WindowModeSelector.ItemsSource = windowStates;
            WindowModeSelector.SelectedIndex = Settings.Default.WindowMode == 0 ? 0 : 1;

            // Fill board size with defaults
            DefaultWidthBox.Text = Settings.Default.BoardWindowSize.Width.ToString();
            DefaultHeightBox.Text = Settings.Default.BoardWindowSize.Height.ToString();

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Save and update style preference
            Settings.Default.AppStylePref = ThemeSelector.SelectedIndex;
            StyleManager.ApplyStyle(ThemeSelector.SelectedIndex);

            // Save window mode preference
            int windowMode = WindowModeSelector.SelectedIndex == 0 ? 0 : 2; // make "minimised" not selectable
            Settings.Default.WindowMode = windowMode;
            Application.Current.Resources["DefaultWindowState"] = (WindowState)windowMode;

            // save window size preference
            try
            {
                System.Drawing.Size newSize = new System.Drawing.Size(int.Parse(DefaultWidthBox.Text), int.Parse(DefaultHeightBox.Text));
                Settings.Default.BoardWindowSize = newSize;
                Application.Current.Resources["DefaultBoardSize"] = new Size(newSize.Width, newSize.Height);
            }
            catch (Exception ex)
            {
                PinHoardErrors.PreferenceUpdateError("Default size for board window was invalid. Property was not updated.");
            }
                
            this.Close();
        }
    }
}
