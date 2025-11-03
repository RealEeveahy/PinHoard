using PinHoard.model.pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PinHoard.viewmodel;

namespace PinHoard.model.save_load
{
    internal class LegacyLoad
    //tool for loading older versions,
    //supporting files created with a legacy save code
    {
        public BoardViewModel board;
        int[] hwArray = new int[2];
        public LegacyLoad(float version, SerializableBoard data, BoardViewModel b)
        {
            board = b;
            hwArray[0] = b.model.pinHeight;
            hwArray[1] = b.model.pinWidth;

            if (version == 1.0f) LoadVersion1_0(data);
            else if (version == 1.1f) LoadVersion1_1(data);
            else MessageBox.Show($"Failed to load file. Loaded in version {version}.", "Version Interpreter Error", MessageBoxButton.OK);
        }
        public void LoadVersion1_0(SerializableBoard data)
        //regular vs definition distinction was made by checking a set of two strings,
        //if the first was empty, then it was a regular pin.
        //note - i do not have any files that follow this format any more, but am keeping it anyway
        {
            List<string>? contents = data.myPins;
            for (int i = 0; i < contents.Count; i += 2)
            {
                BasePin newPin = new BasePin(board, board.allPins.Count, hwArray);
                if (string.IsNullOrEmpty(contents[i])) //tuple is a regular pin
                {
                    newPin.InitComponent(new PinContent(newPin.componentList.Count, newPin, newPin.width-10, contents[i + 1]));
                }
                else //tuple is a definition
                {
                    newPin.InitComponent(new TitleComponent(newPin.componentList.Count, newPin, newPin.width - 10, contents[i]));
                    newPin.InitComponent(new PinContent(newPin.componentList.Count, newPin, newPin.width - 10, contents[i + 1]));
                }
                board.allPins.Add(newPin);
            }
        }
        public void LoadVersion1_1(SerializableBoard data)
        //pins were split into three distinct types based on the number of strings in a list,
        //one string = regular, two = definition, three or more = list
        //this of course meant that a list with one point became a definition.
        //used for the majority of my second uni semester
        {
            List<SerializablePin>? contents = data.myPinObjects;

            if (contents != null)
            {
                foreach (SerializablePin pdo in contents)
                {
                    if (pdo.stringList != null)
                    {
                        BasePin newPin = new BasePin(board, board.allPins.Count, hwArray);
                        if (pdo.stringList.Count == 1)
                        {
                            newPin.InitComponent(new PinContent(newPin.componentList.Count, newPin, newPin.width - 10, pdo.stringList[0]));
                        }
                        else if (pdo.stringList.Count == 2)
                        {
                            newPin.InitComponent(new TitleComponent(newPin.componentList.Count, newPin, newPin.width - 10, pdo.stringList[0]));
                            newPin.InitComponent(new PinContent(newPin.componentList.Count, newPin, newPin.width - 10, pdo.stringList[1]));
                        }
                        else if (pdo.stringList.Count > 2)
                        {
                            //get and remove the title string from the list
                            string t = pdo.stringList[0];
                            pdo.stringList.RemoveAt(0);

                            newPin.InitComponent(new TitleComponent(newPin.componentList.Count, newPin, newPin.width - 10, pdo.stringList[0]));
                            newPin.InitComponent(new ListComponent(pdo.stringList, newPin.componentList.Count, newPin, newPin.width - 10));
                        }
                        board.allPins.Add(newPin);
                    }
                }
            }
        }
    }
}

