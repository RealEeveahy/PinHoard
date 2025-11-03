using PinHoard.model.pins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using PinHoard.viewmodel;

namespace PinHoard.model.save_load
{
    public static class SaveData //object that is serialzed into json
    {
        public static void SaveAll(BoardViewModel board, float version)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = System.IO.Path.Combine(currentPath, "boards"); // define the path first

            if (!Directory.Exists(directoryPath))// make board if it does not exist already
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine("Directory created at " + directoryPath);
            }
            else { Console.WriteLine("Directory " + directoryPath + " exists."); }

            List<SerializablePin> pinDataList = new List<SerializablePin>();
            foreach (BasePin bp in board.allPins)
            {
                List<SerializableComponent> contents = new List<SerializableComponent>();
                foreach ((string, string) s in bp.rawStringList)
                {
                    SerializableComponent componentDataObject = new(s.Item1, s.Item2);
                    contents.Add(componentDataObject);
                }
                SerializablePin thisData = new SerializablePin(bp.orderInBoard, contents, bp.colour);
                pinDataList.Add(thisData);
            }
            try
            {
                FileSaveViewModel saveHelper = new FileSaveViewModel("");
                if (board.boardName == string.Empty) //when opened as a new board, prompt for a file name
                {
                    saveHelper = new FileSaveViewModel();
                }
                else
                {
                    saveHelper = new FileSaveViewModel(board.boardName);
                }
                if (saveHelper.newBoardName != string.Empty) //if a file name was not entered, do not save new
                {
                    SerializableBoard data = new SerializableBoard(version, pinDataList);

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(data, options); //convert the pins to json
                    File.WriteAllText(System.IO.Path.Combine(directoryPath, $"{saveHelper.newBoardName}.json"), json); //serialise the json data

                    MessageBox.Show("Successfully saved board.");
                    MainWindow? m = Window.GetWindow(App.Current.MainWindow) as MainWindow;
                    m?.LoadAllBoards();
                }

                if(saveHelper != null)
                    if (saveHelper.closeAfterSave) board.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}.", "File save error", MessageBoxButton.OK);
                Console.WriteLine($"File could not be saved: {ex.Message}.");
            }
        }
    }
}
