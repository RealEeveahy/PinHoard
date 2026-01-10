using System.Windows;

namespace PinHoard.util
{
    public static class PinHoardErrors
    {
        /// <summary>
        /// Display a messagebox detailing an error to do with a filename input by the user.
        /// </summary>
        /// <param name="issue">The full error output</param>
        public static void FilenameError(string issue)
        { MessageBox.Show($"{issue}.", "Invalid Filename", MessageBoxButton.OK, MessageBoxImage.Warning); }

        /// <summary>
        /// Display a messagebox detailing an error that occurred while attempting to save a file.
        /// </summary>
        /// <param name="issue">The full error output</param>
        public static void SaveError(string issue)
        { MessageBox.Show($"{issue}.", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        /// <summary>
        /// Display a messagebox warning that an action could not be completed because a pin was not in focus.
        /// </summary>
        /// <param name="issue">The full error output</param>
        public static void FocusError(string issue)
        {
            MessageBox.Show($"{issue} : Please select a pin.", "Focus Error",
            MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
