using MvvmHelpers;
using MvvmHelpers.Commands;
using PinHoard.view;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PinHoard.viewmodel
{
    public class ColourPickerViewModel : BaseViewModel
    {
        public ICommand ApplyColourChange { private set; get; }
        BoardViewModel boardViewModel;

        private bool _focusChanged;
        public bool focusChanged
        {
            get => _focusChanged;
            set
            {
                if(_focusChanged != value)
                {
                    _focusChanged = value;
                    OnPropertyChanged();
                }
            }
        }

        public string r = "", g = "", b = "";

        public new PropertyChangedEventHandler PropertyChanged;
        public ColourPickerViewModel(BoardViewModel board) 
        {
            /// (hopefully) binds the updateColour function the the apply button, taking the string as an input, 
            /// therefore not requring any references to the colour picker
            ApplyColourChange = new Command<string>( (string s) => { UpdateColour(s); } );
            boardViewModel = board;
            board.PropertyChanged += SetFields;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void SetFields(object obj, PropertyChangedEventArgs e)
        {
            r = boardViewModel.focusedPin.colour.Substring(1, 2);
            g = boardViewModel.focusedPin.colour.Substring(3, 2);
            b = boardViewModel.focusedPin.colour.Substring(5, 2);
        }
        private void UpdateColour(string hexCode)
        {
            if (boardViewModel.focusedPin != null)
            {
                string colour = "#" + hexCode.ToUpper();
                if (ValidateString(colour))
                {
                    boardViewModel.focusedPin.colour = colour;
                }
                else MessageBox.Show("Colour Invalid.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Could not apply colour, no pin focused.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        bool ValidateString(string hexCode)
        {
            List<char> hexletters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F' };

            if (hexCode.Length == 7)
                for(int i = 1;  i < hexCode.Length; i++)
                {
                    char c = hexCode[i];
                    if (!System.Char.IsDigit(c) && !hexletters.Contains(c))
                    {
                        return false;
                    }
                }
            else return false;

            return true;
        }
    }
}
