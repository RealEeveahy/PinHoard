using PinHoard.model.pins;
using PinHoard.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace PinHoard.model.save_load
{
    /// <summary>
    /// Extracts Pin_Model instances from a file and compiles them into a given Board instance.
    /// 
    /// <para>If file version is not appropriate, the task is given to LegacyLoad instead.</para>
    /// </summary>
    internal class LoadData
    {
        public LoadData(string boardname, Board loadTo, bool verbose = true)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = System.IO.Path.Combine(currentPath, "boards");
            string filePath = System.IO.Path.Combine(directoryPath, $"{boardname}");

            try
            {
                string json = File.ReadAllText(filePath);
                try
                {
                    //Attempt to load file using current save format
                    SerializableBoard? data = JsonSerializer.Deserialize<SerializableBoard>(json);
                    loadTo.version = data.version;

                    if (data.version == BoardSaveUtility.saveLoadVersion)
                    {

                        List<SerializablePin>? pins = data.pins;
                        if (pins == null) return;

                        foreach (SerializablePin pin in pins)
                        {
                            if (pin.components == null) return;
                            List<SerializableComponent> componentObjects = pin.components;
                            List<ComponentBase> deserializedComponents = new List<ComponentBase>();

                            foreach (SerializableComponent co in componentObjects)
                            {
                                //fix in the future, multiple bulletpoints should be grouped into a list.
                                //either cache here or group in the save function
                                List<string> sL = new List<string>();
                                if (co.format == "list") { sL.Add(co.stringContent); }


                                if (co.format == "content") deserializedComponents.Add(new PinContent(componentObjects.Count, 120, co.stringContent));
                                else if (co.format == "title") deserializedComponents.Add(new TitleComponent(componentObjects.Count, 120, co.stringContent));
                                else if (co.format == "list") deserializedComponents.Add(new ListPoint(componentObjects.Count, 120, co.stringContent));
                            }

                            Pin_Model newPin = new Pin_Model(deserializedComponents, pin.bgColour_hexCode);

                            loadTo.allPins.Add(newPin);
                        }
                    }
                    else
                    {
                        //file is using deprecated format. place old code in LegacyLoad.cs each version change
                        v1_2SaveData? legacyData = JsonSerializer.Deserialize<v1_2SaveData>(json);
                        if (verbose)
                            PinHoardErrors.FileWarning("A newer version of PinHoard is available. Please save the file again to update.");
                        loadTo.version = data.version;

                        LegacyLoad nLL = new LegacyLoad(loadTo.version, legacyData, loadTo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load file: {ex.Message}.", "File load error", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load file: {ex.Message}.", "File load error", MessageBoxButton.OK);
            }
        }
    }
}
