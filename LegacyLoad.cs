using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static PinHoard.BoardWindow;

namespace PinHoard
{
    internal class LegacyLoad 
        //tool for loading older versions,
        //supporting files created with a legacy save code
    {
        public BoardWindow Board;
        int[] hwArray = new int[2];
        public LegacyLoad(float version, SaveData data, BoardWindow b)
        {
            Board = b;
            hwArray[0] = b.pinHeight;
            hwArray[1] = b.pinWidth;

            if (version == 1.0f) LoadVersion1_0(data);
            else if (version == 1.1f) LoadVersion1_1(data);
            else MessageBox.Show($"Failed to load file. Loaded in version {version}.", "Version Interpreter Error", MessageBoxButton.OK);
        }
        public void LoadVersion1_0(SaveData data) 
            //regular vs definition distinction was made by checking a set of two strings,
            //if the first was empty, then it was a regular pin.
            //note - i do not have any files that follow this format any more, but am keeping it anyway
        {
            List<string>? contents = data.myPins;
            for (int i = 0; i < contents.Count; i += 2)
            {
                if (string.IsNullOrEmpty(contents[i])) //tuple is a regular pin
                {
                    BasePin newPin = new BasePin(Board.PinGrid, Board, Board.pinsInBoard, hwArray, Board.windowDimensions);
                    newPin.InitComponent("content", contents[i + 1], null);
                    Board.PinGrid.Children.Add(newPin.NoteGrid);

                    Board.allPins.Add(newPin);
                }
                else //tuple is a definition
                {
                    BasePin newDefinition = new BasePin(Board.PinGrid, Board,
                        Board.pinsInBoard, hwArray, Board.windowDimensions);
                    newDefinition.InitComponent("title", contents[i], null);
                    newDefinition.InitComponent("content", contents[i + 1], null);
                    Board.PinGrid.Children.Add(newDefinition.NoteGrid);

                    Board.allPins.Add(newDefinition);
                }
                Board.pinsInBoard++;
            }
        }
        public void LoadVersion1_1(SaveData data)
            //pins were split into three distinct types based on the number of strings in a list,
            //one string = regular, two = definition, three or more = list
            //this of course meant that a list with one point became a definition.
            //used for the majority of my second uni semester
        {
            List<PinDataObject>? contents = data.myPinObjects;

            if (contents != null)
            {
                foreach (PinDataObject pdo in contents)
                {
                    if (pdo.stringList != null)
                    {
                        if (pdo.stringList.Count == 1)
                        {
                            BasePin newPin = new BasePin(Board.PinGrid, Board, Board.pinsInBoard, hwArray, Board.windowDimensions);
                            newPin.InitComponent("content", pdo.stringList[0], null);

                            Board.PinGrid.Children.Add(newPin.NoteGrid);
                            Board.allPins.Add(newPin);
                        }
                        else if(pdo.stringList.Count == 2)
                        {
                            BasePin newDefinition = new BasePin(Board.PinGrid, Board, Board.pinsInBoard, hwArray, Board.windowDimensions);
                            newDefinition.InitComponent("title", pdo.stringList[0], null);
                            newDefinition.InitComponent("content", pdo.stringList[1], null);

                            Board.PinGrid.Children.Add(newDefinition.NoteGrid);
                            Board.allPins.Add(newDefinition);
                        }
                        else if(pdo.stringList.Count > 2)
                        {
                            //get and remove the title string from the list
                            string t = pdo.stringList[0];
                            pdo.stringList.RemoveAt(0);

                            BasePin newMulti = new BasePin(Board.PinGrid, Board, Board.pinsInBoard, hwArray, Board.windowDimensions);
                            newMulti.InitComponent("title", t, null);
                            newMulti.InitComponent("list", null, pdo.stringList);//temp

                            Board.PinGrid.Children.Add(newMulti.NoteGrid);
                            Board.allPins.Add(newMulti);
                        }
                        Board.pinsInBoard++;
                    }
                }
            }
        }
    }
}

