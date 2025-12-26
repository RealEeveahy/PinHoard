using PinHoard.model.pins;
using PinHoard.util;
using System.Collections.Generic;
using System.Windows;

namespace PinHoard.model.save_load
{
    public class LegacyLoad
    //tool for loading older versions,
    //supporting files created with a legacy save code
    {
        public Board loading;
        readonly int[] hwArray = new int[2];
        public LegacyLoad(float version, v1_2SaveData data, Board b)
        {
            loading = b;
            hwArray[0] = b.pinHeight;
            hwArray[1] = b.pinWidth;

            if (version == 1.0f) LoadVersion1_0(data);
            else if (version == 1.1f) LoadVersion1_1(data);
            else if (version == 1.2f) LoadVersion1_2(data);
            else MessageBox.Show($"Failed to load file. Loaded in version {version}.", "Version Interpreter Error", MessageBoxButton.OK);
        }
        public void LoadVersion1_0(v1_2SaveData data)
        //regular vs definition distinction was made by checking a set of two strings,
        //if the first was empty, then it was a regular pin.
        //note - i do not have any files that follow this format any more, but am keeping it anyway
        {
            List<string>? contents = data.myPins;
            for (int i = 0; i < contents.Count; i += 2)
            {
                if (string.IsNullOrEmpty(contents[i])) //tuple is a regular pin
                {
                    Pin_Model newPin = new Pin_Model(new List<ComponentBase>
                    {
                        new PinContent(0, 120, contents[i + 1])
                    });
                    //Board.PinGrid.Children.Add(newPin.NoteGrid);

                    loading.allPins.Add(newPin);
                }
                else //tuple is a definition
                {
                    Pin_Model newDefinition = new Pin_Model(new List<ComponentBase>
                    {
                        new TitleComponent(0, 120, contents[i]),
                        new PinContent(1, 120, contents[i + 1])
                    });
                    loading.allPins.Add(newDefinition);
                }
            }
        }
        public void LoadVersion1_1(v1_2SaveData data)
        /// pins were split into three distinct types based on the number of strings in a list,
        /// one string = regular, two = definition, three or more = list
        /// this of course meant that a list with one point became a definition.
        /// used for the majority of my second uni semester
        {
            List<v1_2PinObject>? contents = data.myPinObjects;

            if (contents != null)
            {
                foreach (v1_2PinObject pdo in contents)
                {
                    if (pdo.stringList != null)
                    {
                        if (pdo.stringList.Count == 1)
                        {
                            Pin_Model newPin = new Pin_Model(new List<ComponentBase>
                            {
                                new PinContent(0, 120, pdo.stringList[0])
                            });

                            loading.allPins.Add(newPin);
                        }
                        else if (pdo.stringList.Count == 2)
                        {
                            Pin_Model newDefinition = new Pin_Model(new List<ComponentBase>
                            {
                                new TitleComponent(0, 120, pdo.stringList[0]),
                                new PinContent(1, 120, pdo.stringList[1])
                            });
                            loading.allPins.Add(newDefinition);
                        }
                        else if (pdo.stringList.Count > 2)
                        {
                            //get and remove the title string from the list
                            string t = pdo.stringList[0];
                            pdo.stringList.RemoveAt(0);

                            List<ComponentBase> components = new List<ComponentBase>{
                                new TitleComponent(0, 120, t)
                            };
                            components.AddRange(PinHoardHelpers.ConvertStringListToComponents(pdo.stringList));

                            Pin_Model newMulti = new Pin_Model(components);

                            loading.allPins.Add(newMulti);
                        }
                    }
                }
            }
        }

        public void LoadVersion1_2(v1_2SaveData data)
        /// Version was updated here because classes and attribute names were modified to be more concise, 
        /// and json data needs to read from the existing attribute names
        {
            List<v1_2PinObject>? pinObjects = data.myPinObjects;
            if (pinObjects == null) return;

            foreach (v1_2PinObject po in pinObjects)
            {
                if (po.myComponentObjects == null) return;
                List<SerializableComponent> componentObjects = po.myComponentObjects;
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
                    else deserializedComponents.Add(new PinContent(componentObjects.Count, 120, co.stringContent)); // add as default if format not recognised
                }

                Pin_Model newPin = new Pin_Model(deserializedComponents, po.bgColour);

                loading.allPins.Add(newPin);
            }
        }
    }

    public class v1_2SaveData
    {
        public float version { get; set; }
        public List<v1_2PinObject> myPinObjects { get; set; }
        public List<string>? myPins { get; set; } // deprecated, v1_0
    }
    public class v1_2PinObject
    {
        readonly int index;
        public List<SerializableComponent> myComponentObjects { get; set; }
        public string bgColour { get; set; }
        public List<string>? stringList { get; set; } // deprecated, v1_1
    }

}
