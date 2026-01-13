using PinHoard.Properties;
using PinHoard.view.style;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // load and apply style preference
            int paletteIndex = Settings.Default.AppStylePref;
            StyleManager.ApplyStyle(paletteIndex);

            System.Drawing.Size prefSize = Settings.Default.BoardWindowSize;
            Current.Resources["DefaultBoardSize"] = new Size(prefSize.Width, prefSize.Height);
            Current.Resources["DefaultWindowState"] = (WindowState)Settings.Default.WindowMode;
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }

    }
}
