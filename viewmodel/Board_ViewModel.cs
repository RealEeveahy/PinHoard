using PinHoard.model;
using PinHoard.model.pins;
using PinHoard.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PinHoard.viewmodel
{
    public class Board_ViewModel : INotifyPropertyChanged
    {
        public Board model;
        public List<Board> boards;

        public PinMatrix pinContainer = new PinMatrix();

        public List<Pin_ViewModel> allPinVMs = new List<Pin_ViewModel>(); // generally try not to use.
        public List<Pin_ViewModel> filteredPinList = new List<Pin_ViewModel>();

        private Pin_ViewModel? _focusedPin = null;
        public Pin_ViewModel? focusedPin
        {
            get => _focusedPin;
            set
            {
                if (_focusedPin == value) return;
                _focusedPin = value;
                OnPropertyChanged(nameof(FocusedColour));
            }
        }
        public SolidColorBrush FocusedColour => focusedPin != null ?
            focusedPin.BackgroundColour :
            new SolidColorBrush(Colors.LightGray);

        public ColourPicker_ViewModel ColourPicker { get; }
        public bool ColourPickIsOpen = false; 

        public bool readOnly = false, debugging = false;
        
        public Action ReloadMain;

        readonly bool filtered; //temporary

        public double[] windowDimensions = new double[2] { 800, 700 };
        //private Size defaultPinSize = new Size(120, 120);

        public string displayName =>
            model != null ? (!string.IsNullOrEmpty(model.boardName) ? $"{model.boardName} [{model.version}]" : "New Board") : boards.Count.ToString() + " Boards";
        public Board_ViewModel(bool read_only = false)
        {
            readOnly = read_only;
            model = new Board();

            ColourPicker = new ColourPicker_ViewModel(this);

            new BoardWindow(this).Show();
        }
        public Board_ViewModel(Board from_file, bool read_only = false)
        {
            readOnly = read_only;
            model = from_file;
            LoadPins();

            ColourPicker = new ColourPicker_ViewModel(this);

            new BoardWindow(this).Show();
        }
        public void Build()
        {

        }
        public void LoadPins()
        {
            foreach (Pin_Model pin in model.allPins)
            {
                Pin_ViewModel _pin = new(pin, SetFocus, pinContainer);
                allPinVMs.Add(_pin);
            }
        }
        public void AddPinPrefab(string pinType)
        {
            Pin_Model newModel = new Pin_Model();
            if (pinType == "single")
            {
                newModel.AddComponent(new PinContent(0, 120));
            }
            else if (pinType == "double")
            {
                newModel.AddComponent(new TitleComponent(0, 120));
                newModel.AddComponent(new PinContent(1, 120));
            }
            else if (pinType == "multi")
            {
                List<string> sL = new List<string> { "This is my first point", "This is my second point", "And here's a third!" };
                List<ComponentBase> components = new List<ComponentBase> { new TitleComponent(0, 120) };
                components.AddRange(PinHoardHelpers.ConvertStringListToComponents(sL));

                newModel = new Pin_Model(components);
            }

            Pin_ViewModel newPin = new(newModel, SetFocus, pinContainer);

            focusedPin = newPin; // a new pin will always be in focus for convenience
            RegisterPin(newPin);
        }
        public void AddComponentToFocus(string componentType)
        {
            if (focusedPin != null) focusedPin.InitComponent(componentType);
            else PinHoardErrors.FocusError("Failed to add component");
        }
        public void SubmitColour(int r, int g, int b)
        {
            if (focusedPin != null)
            {
                focusedPin.BackgroundColour = new SolidColorBrush((Color)ColorConverter.ConvertFromString($"#{r:X2}{g:X2}{b:X2}"));
                OnPropertyChanged(nameof(FocusedColour));
            }
            else PinHoardErrors.FocusError("Failed to update colour");
        }
        public void RegisterPin(Pin_ViewModel pin)
        {
            model.allPins.Add(pin.model);
            allPinVMs.Add(pin);
        }
        public void FilterBoard(string searchTerm)
        {
            foreach (Pin_ViewModel pin in allPinVMs) { pin.SetVisible(true); }
            filteredPinList.Clear();
            if (searchTerm == string.Empty) return;

            foreach (Pin_ViewModel pin in allPinVMs)
            {
                if (pin.model.ContainsFilterTerm(searchTerm))
                    filteredPinList.Add(pin);
                else
                    pin.SetVisible(false);
            }
        }
        public void SizeChanged(double[] newSize)
        {
            windowDimensions = newSize;
            Build();
        }
        public Cursor ToggleDebug()
        {
            debugging ^= true;
            return debugging ? Cursors.Help : Cursors.Arrow;
        }
        public void DebugFocused() 
        { 
            if(focusedPin != null)
                focusedPin.ShowDebugInfo();
            else
                PinHoardErrors.FocusError("Failed to initiate debug");
        }
        public void SetFocus(Pin_ViewModel pin) { focusedPin = pin; }
        public void Save()
        {
            if (model != null)
            {
                if (model.boardName == string.Empty)
                {
                    FileSaveWindow getFileName = new FileSaveWindow(SetFilename);
                    getFileName.ShowDialog();
                }
                else
                {
                    BoardSaveUtility.WriteFile(model);
                    ReloadMain();
                }
            }
            else PinHoardErrors.SaveError("No board model to save.");
        }
        public void SetFilename(string newFilename)
        {
            if (PinHoardHelpers.IsValidFilename(newFilename))
            {
                model.boardName = newFilename;
                Save();
            }
            else PinHoardErrors.FilenameError("An invalid character was entered or the file name was left blank.");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
