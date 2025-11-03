using PinHoard.service;
using PinHoard.view;
using PinHoard.viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard.model.pins
{
    public partial class BasePin
    {
        public BoardViewModel board;
        private PinView view;

        public int orderInBoard; //irrelevant when filtering, find a new way to assign positions
        public int totalLines = 1;
        //public bool isBeingDragged;
        public bool readOnly = false, debugging = false;
        public Vector2 xyCoord = new Vector2(); //coordinate position

        public double trueWidth, trueHeight;
        public List<(string, string)> rawStringList = new List<(string c, string f)>(); //content, format

        public PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<PinComponent> componentList = new ObservableCollection<PinComponent>();
        public int width;
        private int _height;
        public int height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }

        private int[] _position = new int[2];//ACTUAL position
        public int[] position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _colour = "#FFFCF8F3";
        public string colour
        {
            get => _colour;
            set
            {
                if (_colour != value)
                {
                    _colour = value;
                    OnPropertyChanged();
                }
            }
        }
        private Visibility _visibility = Visibility.Visible;
        public Visibility visibility
        {
            get => _visibility;
            set
            {
                if(_visibility != value)
                {
                    _visibility = value;
                    OnPropertyChanged();
                }
            }
        }
        public BasePin(BoardViewModel parent, int index, int[] hwArray, string type = "", string colour = "#FFFCF8F3")
        {
            board = parent;
            width = hwArray[1];
            height = hwArray[0];
            orderInBoard = index;
            this.colour = colour;

            // the pin temporarily knows about its own view until i have a different way for the widget to be referenced that
            // will allow it to be added as a child in the board - otherwise, the pin does not directly access the view for any reason
            view = new PinView(this);

            /// Initialize the pin, will create an empty pin if no style is given
            if (type == "single")
            {
                InitComponent(new PinContent(componentList.Count, this, width - 10, "This is a pin!"));;
            }
            else if (type == "double")
            {
                InitComponent(new TitleComponent(componentList.Count, this, width - 10, "This is a title!"));
                InitComponent(new PinContent(componentList.Count, this, width - 10, "And this is it's description!"));
            }
            else if (type == "multi")
            {
                InitComponent(new TitleComponent(componentList.Count, this, width-10, "Title."));
                InitComponent(new ListComponent(
                    new List<string> {
                        "This is my first point",
                        "This is my second point",
                        "And here's a third!"
                        },
                    componentList.Count,
                    this,
                    width - 10
                    )
                );
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public void InitComponent(PinComponent component)
        {
            componentList.Add(component);
            PinResize();
        }
        public void GotFocus()
        {
            board.SwitchFocus(this);
        }
        public void SetPosition(int[] newPos) { position = newPos; }
        public void PinResize()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                totalLines = 0;
                foreach (PinComponent pc in componentList) totalLines += pc.lines;
                //if (totalLines <= 4) height = 120;
                //else height = 120 + ((totalLines - 4) * 15);
                height = 0;
                foreach (PinComponent pc in componentList)
                {
                    //if (pc.wrapper.Height < 0) return;
                    height += pc.GetHeight();
                }
                height += 40; //add 20 to each end

                if (height < 120) height = 120;
                board.RecalculateAllPositions();
            });
        }
        internal string GetDebugInfo()
        {
            StringBuilder debug = new StringBuilder();

            debug.Append("Information about this Pin: \n" +
                $"Order  {orderInBoard}\n" +
                $"Logical Dimensions    {width} x {height}\n" +
                $"Physical Dimensions   {trueWidth} x {trueHeight}\n" +
                $"Colour    {colour}\n" +
                $"Components    \n");
            foreach ((string, string) sPair in rawStringList)
            {
                debug.Append($"\u0009'{sPair.Item1}', ({sPair.Item2}) \n");
            }

            return debug.ToString();
        }
        public Grid Top() { return view; }
    }
}
