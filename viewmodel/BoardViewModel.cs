using MvvmHelpers;
using PinHoard.model;
using PinHoard.model.pins;
using PinHoard.model.save_load;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static PinHoard.service.CalculatePosition;

namespace PinHoard.viewmodel
{
    public class BoardViewModel : BaseViewModel
    {
        public Board model { get; }
        BoardWindow boardView { get; }

        public string? boardName { get; set; }
        readonly float saveLoadVersion = 1.2f; //increment by .1 any time the saving / loading system is updated

        public PropertyChangedEventHandler PropertyChanged;

        private BasePin _focused;
        public BasePin focusedPin
        {
            get => _focused;
            set
            {
                if(_focused != value)
                {
                    _focused = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<BasePin> filteredPinList = new List<BasePin>();
        public ObservableCollection<BasePin> allPins = new ObservableCollection<BasePin>();
        public Dictionary<Vector2, BasePin> CoordForPinDict = new Dictionary<Vector2, BasePin>();

        public bool readOnly = false, debugging = false;
        public double[] windowDimensions = new double[2] { 800, 700 };

        public BoardViewModel(Board board, bool r_o = false) 
        {
            model = board;
            boardName = board.boardName;

            //map the viewmodel pins to those in the model
            allPins = new ObservableCollection<BasePin>(board.allPins);

            boardView = new BoardWindow(r_o, this);
            boardView.Show();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public void NewComponentClicked(object sender, RoutedEventArgs e)
        {
            if (focusedPin == null)
            {
                MessageBox.Show($"Failed to add component: Please select a pin.", "Component Error", MessageBoxButton.OK);
                return;
            }
            AddComponent(((Button)sender).Tag.ToString());
        }
        public void SwitchFocus(BasePin p)
        {
            focusedPin = p;
            if (debugging) DebugBox(p.GetDebugInfo());
        }
        public BasePin AddPin(string initType)
        {
            int[] hwArray = { model.pinHeight, model.pinWidth };

            BasePin newPin = new(
                this,
                allPins.Count,
                hwArray,
                initType
            );
            BasePin above;
            try { above = CoordForPinDict[(new Vector2(newPin.xyCoord.X, newPin.xyCoord.Y - 1))]; }
            catch (Exception ex) { above = null; }


            RepositionInfo info = GetNewPinPosition(windowDimensions, allPins.Count, newPin, above);
            CoordForPinDict[info.coordinate] = newPin;
            newPin.SetPosition(info.position);

            focusedPin = newPin;
            return newPin;
        }
        public void AddComponent(string componentType)
        {
            if (componentType == "content")
                focusedPin.InitComponent(
                    new PinContent(
                        focusedPin.componentList.Count,
                        focusedPin,
                        model.pinWidth - 10,
                        "New Component")
                    );
            else if (componentType == "title")
                focusedPin.InitComponent(
                    new TitleComponent(
                        focusedPin.componentList.Count,
                        focusedPin,
                        model.pinWidth - 10,
                        "New Component")
                    );
            else if (componentType == "list")
                focusedPin.InitComponent(
                    new ListComponent(
                        new List<string> { "New Component." },
                        focusedPin.componentList.Count,
                        focusedPin,
                        model.pinWidth - 10)
                    );
        }
        public void NewPinClicked(object sender, RoutedEventArgs e) //functional - change soon
        {
            string pinType = ((Button)sender).Tag.ToString();
            allPins.Add(AddPin(pinType));
        }

        public void FilterBoard(string searchTerm)
        {
            foreach (BasePin bp in allPins) { bp.visibility = Visibility.Visible; }
            filteredPinList.Clear();
            if (searchTerm == string.Empty) return;

            foreach (BasePin bp in allPins)
            {
                bool matchFound = false;
                foreach ((string, string) s in bp.rawStringList)
                {
                    if ((s.Item1).ToLower().Contains(searchTerm.ToLower()))
                    {
                        if (!matchFound) { filteredPinList.Add(bp); }
                        matchFound = true;
                    }
                }
                if (!matchFound)
                {
                    bp.visibility = Visibility.Collapsed;
                }
            }
            RecalculateAllPositions(true);
        }
        public void RecalculateAllPositions(bool filtered = false)
        //this method is very strange looking but works, and doesnt use a bunch of if-elses
        {
            List<BasePin> selectedList = new List<BasePin>(allPins);
            if (filtered) selectedList = filteredPinList;

            int subOrder = -1;

            foreach (BasePin p in selectedList)
            {
                if (filtered) subOrder++;

                BasePin above;
                try { above = CoordForPinDict[(new Vector2(p.xyCoord.X, p.xyCoord.Y - 1))]; }
                catch (Exception ex) { above = null; }

                RepositionInfo info = GetNewPinPosition(windowDimensions, subOrder, p, above);

                CoordForPinDict[info.coordinate] = p;
                p.SetPosition(info.position);
            }
        }
        public void ToggleDebug(object sender, RoutedEventArgs e)
        {
            debugging ^= true;
        }
        public void DebugBox(string message)
        {
            MessageBox.Show(message, "Mae's Magic Wand");
        }
        public void SaveBoard(object sender, RoutedEventArgs e)
        {
            SaveData.SaveAll(this, saveLoadVersion);
        }
        public void Close()
        {
            boardView.Close();
        }
    }
}
