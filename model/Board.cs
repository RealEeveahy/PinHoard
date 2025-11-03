using PinHoard.model.pins;
using PinHoard.model.save_load;
using PinHoard.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PinHoard.model
{
    public class Board
    {
        public string boardName = string.Empty; //when a new board is created it has no name. this will be updated on load
        public List<BasePin> allPins = new List<BasePin>();
        public int pinsInBoard
        {
            get { return allPins.Count; }
        }
        public int pinWidth = 120, pinHeight = 120; //default size for pins
        public Board(bool read_only = false) // new board constructor
        {
            BoardViewModel vm = new BoardViewModel(this, read_only);
        }
        public Board(string boardName, bool read_only = false)
        {
            LoadData data = new LoadData(boardName);
            data.GetWithUI(new BoardViewModel(this, read_only));
        }
        public Board(List<string> boardnames)
        {
            BoardViewModel vm = new(this, true);
            foreach(string s in boardnames)
            {
                LoadData data = new LoadData((s.Substring(0, s.Length - 5)));
                data.GetWithUI(vm);
            }
            vm.boardName = boardnames.Count.ToString() + " Boards";
        }
    }
}
