using PinHoard.model.pins;
using PinHoard.model.save_load;
using PinHoard.util;
using System.Collections.Generic;

namespace PinHoard.model
{
    public class Board
    {
        public string boardName = string.Empty; //when a new board is created it has no name. this will be updated on load
        public float version;
        public List<Pin_Model> allPins = new List<Pin_Model>();
        public int pinsInBoard => allPins.Count;

        public int pinWidth = 120, pinHeight = 120; //default size for pins
        public Board() // new board constructor
        {

        }
        public Board(string boardName)
        {
            LoadData data = new LoadData(boardName, this);
            this.boardName = PinHoardHelpers.CutExtension(boardName);
        }
        public Board(List<string> boardnames)
        {
            foreach (string s in boardnames)
            {
                LoadData data = new LoadData(s, this, false);
            }
            boardName = boardnames.Count.ToString() + " Boards";
        }
    }
}