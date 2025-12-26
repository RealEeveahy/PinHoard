using PinHoard.model;
using PinHoard.model.save_load;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace PinHoard.util
{
    public static class BoardSaveUtility
    {
        public static readonly float saveLoadVersion = 1.3f; //increment by .1 any time the saving / loading system is updated
        public static void WriteFile(Board w_b)
        {
            string targetDirectory = CreateDirectory();

            //create data
            SerializableBoard data = new SerializableBoard(saveLoadVersion, w_b);

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(data, options); //convert the pins to json
                File.WriteAllText(System.IO.Path.Combine(targetDirectory, $"{w_b.boardName}.json"), json); //serialise the json data

                MessageBox.Show("Successfully saved board.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}.", "File save error", MessageBoxButton.OK);
                Console.WriteLine($"File could not be saved: {ex.Message}.");
            }
        }
        private static string CreateDirectory()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = System.IO.Path.Combine(currentPath, "boards"); // define the path first

            if (!Directory.Exists(directoryPath))// make board if it does not exist already
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine("Directory created at " + directoryPath);
            }
            else { Console.WriteLine("Directory " + directoryPath + " exists."); }

            return directoryPath;
        }
    }
}
