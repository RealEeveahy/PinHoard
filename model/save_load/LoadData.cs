using PinHoard.model.pins;
using PinHoard.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace PinHoard.model.save_load
{
   public class LoadData
    {
        string currentPath = AppDomain.CurrentDomain.BaseDirectory;
        SerializableBoard deserialized;
        // set to change, switch to loadable custom file types

        string boardName;

        /// <summary>
        /// Object used to load a board, then store a reference to the deserialized data.
        /// After deserializing, you may fetch a list of pins (for board rendering) 
        /// or, alternatively, a list of serialize objects for quizzes (no ui attachment constructed)
        /// </summary>
        /// <param name="boardName"> the name of the board, parsed by the Main Window</param>
        public LoadData(string boardName)
        {
            string directoryPath = System.IO.Path.Combine(currentPath, "boards");
            string filePath = System.IO.Path.Combine(directoryPath, $"{boardName}.json");
            this.boardName = boardName;

            try
            {
                string json = File.ReadAllText(filePath);
                deserialized = JsonSerializer.Deserialize<SerializableBoard>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load file: {ex.Message}.", "File load error", MessageBoxButton.OK);
            }
        }
        public void GetWithUI(BoardViewModel board)
        {
            if (deserialized == null)
            {
                MessageBox.Show($"Cannot get data. No file has been loaded.", "File load error", MessageBoxButton.OK);
            }
            else
            {
                int[] hwArray = { 120, 120 };

                float fileVersion = deserialized.version;

                if (fileVersion == 1.2f) //file is formatted appropriately 
                {
                    List<SerializablePin>? pinObjects = deserialized.myPinObjects;
                    if (pinObjects == null) return;

                    foreach (SerializablePin pin in pinObjects)
                    {
                        if (pin.components == null) continue;
                        List<SerializableComponent> componentObjects = pin.components;

                        BasePin newPin = new BasePin(board, board.allPins.Count, hwArray); 
                        foreach (SerializableComponent co in componentObjects)
                        {
                            if (co.format == "content")
                            {
                                newPin.InitComponent(new PinContent(newPin.componentList.Count, newPin, newPin.width - 10, co.stringContent));
                            }
                            else if (co.format == "title")
                            {
                                newPin.InitComponent(new TitleComponent(newPin.componentList.Count, newPin, newPin.width - 10, co.stringContent));
                            }
                            else if (co.format == "list")
                            {
                                newPin.InitComponent(new ListComponent(co.contentList, newPin.componentList.Count, newPin, newPin.width - 10));
                            }
                        }
                        board.allPins.Add(newPin);
                    }
                }
                else //file is using deprecated format. place old code in LegacyLoad.cs each version change
                {
                    LegacyLoad nLL = new LegacyLoad(fileVersion, deserialized, board);
                }
                board.Title = $"{boardName} [{fileVersion}]";
            }
        }
        public List<SerializablePin> GetDataOnly()
        {
            if(deserialized == null)
            {
                MessageBox.Show($"Cannot get data. No file has been loaded.", "File load error", MessageBoxButton.OK);
                return new List<SerializablePin>();
            }
            else {
                return deserialized.myPinObjects;
            }
        }
    }
}
