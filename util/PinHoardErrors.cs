using System.Windows;

namespace PinHoard.util
{
    public static class PinHoardErrors
    {
        public static void FilenameError(string issue)
        { MessageBox.Show($"{issue}.", "Invalid Filename", MessageBoxButton.OK, MessageBoxImage.Warning); }

        public static void SaveError(string issue)
        { MessageBox.Show($"{issue}.", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        public static void FocusError(string issue)
        { MessageBox.Show($"{issue} : Please select a pin.", "Focus Error", MessageBoxButton.OK); }
    }
}
