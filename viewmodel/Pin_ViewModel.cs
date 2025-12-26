using PinHoard.model.pins;
using PinHoard.util;
using PinHoard.view.notetaking;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinHoard.viewmodel
{
    public class Pin_ViewModel : INotifyPropertyChanged
    {
        public Pin_Model model;
        public Pin_View view;
        public int[] myPosition = new int[2]; //ACTUAL position
        private double _height = 120;
        public double height
        {
            get => _height;
            set
            {
                if (Math.Abs(_height - value) > double.Epsilon)
                {
                    _height = value;
                    OnPropertyChanged("height");
                }
            }
        }
        public double width { get; set; } = 120;
        // Cached brush to avoid allocating a new brush on every get
        private SolidColorBrush? _backgroundBrush;
        public SolidColorBrush BackgroundColour
        {
            get
            {
                if (_backgroundBrush == null)
                {
                    _backgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.bgColour));
                }
                return _backgroundBrush;
            }
            set
            {
                _backgroundBrush = value;
                var c = value.Color;
                model.bgColour = $"#{c.R:X2}{c.G:X2}{c.B:X2}";
                OnPropertyChanged(nameof(BackgroundColour));
            }
        }
        public Visibility NoteVisibility { get; set; }
        public int orderInBoard; //irrelevant when filtering, find a new way to assign positions
        private readonly Action<Pin_ViewModel> FocusSelf;
        private bool ActiveList;

        public Pin_ViewModel(Pin_Model model, Action<Pin_ViewModel> _focus, PinMatrix container)
        {
            this.model = model;
            NoteVisibility = Visibility.Visible;

            foreach(ComponentBase component in model.componentList) component.Focused += Focus;

            view = new Pin_View(this);
            container.Add(view);
            PinResize();

            this.FocusSelf = _focus;
        }
        public void UpdateColour(string newColour_hexCode)
        {
            BackgroundColour = new SolidColorBrush((Color)ColorConverter.ConvertFromString(newColour_hexCode));
        }
        public void Focus(ComponentBase component)
        {
            FocusSelf(this);
        }
        public void Update(ComponentBase component)
        {

        }
        public void SetVisible(bool isVisible)
        {
            NoteVisibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(NoteVisibility));
        }
        public void ShowDebugInfo()
        {
            string componentsAsString = string.Empty;
            foreach ((string, string) sPair in model.rawStringList())
            {
                componentsAsString += $"\u0009'{sPair.Item1}', ({sPair.Item2}) \n";
            }
            MessageBox.Show("Information about this Pin: \n" +
                $"Order  {orderInBoard}\n" +
                $"Logical Dimensions    {width} x {height}\n" +
                //$"Physical Dimensions   {NoteGrid.ActualWidth} x {NoteGrid.ActualHeight}\n" +
                $"Colour    {model.bgColour}\n" +
                $"Components    \n{componentsAsString}", "Mae's debug tool");
        }
        public void InitComponent(string format)
        {
            if (format == "content")
            {
                PinContent component = new PinContent(model.componentCount, (int)width, "New Component");
                component.Focused += Focus;
                model.AddComponent(component);
                ActiveList = false;
            }
            else if (format == "title")
            {
                TitleComponent component = new TitleComponent(model.componentCount, (int)width, "New Component");
                component.Focused += Focus;
                model.AddComponent(component);
                ActiveList = false;
            }
            else if (format == "list")
            {
                if (model.componentCount == 0 || !ActiveList)
                {
                    ListPoint component = new ListPoint(model.componentCount, (int)width, "New Component");
                    component.Focused += Focus;
                    model.AddComponent(component);
                    ActiveList = true;
                }
                else if (ActiveList)
                {
                    // use the existing last created list
                    ListPoint component = new ListPoint(model.componentCount, (int)width, "New Component");
                    component.Focused += Focus;
                    model.AddComponent(component);
                }
            }
            else { MessageBox.Show("Format not recognised.", "Component Initialisation Error", MessageBoxButton.OK, MessageBoxImage.Error); }
            PinResize();
        }
        public void PinResize()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                double newHeight = 0;
                foreach (ComponentBase component in model.componentList)
                {
                    //if (pc.wrapper.Height < 0) return;
                    newHeight += component.GetHeight();
                }
                newHeight += 40; //add 20 to each end

                if (newHeight < 120) newHeight = 120;
                height = newHeight;
            });
        }
        public void SetParent(Panel parent)
        {
            parent.Children.Add(view.NoteGrid);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}