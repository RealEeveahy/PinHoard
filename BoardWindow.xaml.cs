using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.Json;
using System.IO;
using System.Windows.Shapes;
using System.Reflection.Metadata;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        public string boardName = string.Empty; //when a new board is created it has no name. this will be updated on load
        public bool readOnly = false;
        public bool closeAfterSave = false; // changed by the fileSaveWindow
        public List<BasePin> allPins = new List<BasePin>();
        public BasePin? focusedPin;

        private FileSaveWindow? mySaveWindow = null;
        public int pinsInBoard = 0, subPinCount = 0;
        public int pinWidth = 120, pinHeight = 120; //default size for pins

        readonly float saveLoadVersion = 1.2f; //increment by .1 any time the saving / loading system is updated

        public double[] windowDimensions = new double[2] {800,700};
        public Dictionary<Vector2, object> CoordForPinDict = new Dictionary<Vector2, object>();
        public BoardWindow(bool r_o)
        {
            InitializeComponent();
            this.Title = "New Board";
            this.SizeChanged += OnWindowResize;

            FilterConfirmButton.Click += FilterClicked;
            if (!r_o) // disable all the buttons if read only
            {
                NewEmptyButton.MouseEnter += PopoutToolbar;
                NewContentButton.MouseEnter += PopoutToolbar;

                NewEmptyButton.Click += NewPinClicked;
                NewPinButton.Click += NewPinClicked;
                NewDefinitionButton.Click += NewPinClicked;
                NewListButton.Click += NewPinClicked;

                NewContentButton.Click += NewComponentClicked;
                NewTitleButton.Click += NewComponentClicked;
                NewBulletButton.Click += NewComponentClicked;

                SaveBoardButton.Click += SaveAllPins;
            }
        }

        private void NewComponentClicked(object sender, RoutedEventArgs e)
        {
            if (focusedPin == null)
            {
                MessageBox.Show($"Failed to add component: Please select a pin.", "Component Error", MessageBoxButton.OK);
                return;
            }

                List<string> sL = new List<string> { "New component." };
            focusedPin.InitComponent(((Button)sender).Tag.ToString(), "New component.", sL); 
            //uses both parameters temporarily because types are differentiated by the first field
        }

        private void NewPinClicked(object sender, RoutedEventArgs e) //functional - change soon
        {
            string pinType = ((Button)sender).Tag.ToString();
            int[] hwArray = new int[2];
            hwArray[0] = pinHeight;
            hwArray[1] = pinWidth;

            BasePin thisPin = new BasePin(PinGrid, this, pinsInBoard, hwArray, windowDimensions);
            if (pinType == "empty")
            {
                //probably not necessary. just do default if tag is empty
            }
            else if (pinType == "single")
            {
                thisPin.InitComponent("content", "This is a pin!", null);
            }
            else if(pinType == "double")
            {
                thisPin.InitComponent("title", "This is a title.", null);
                thisPin.InitComponent("content", "And this is it's description!", null);
            }
            else if(pinType == "multi")
            {
                thisPin.InitComponent("title", "Title.", null);
                List<string> sL = new List<string> { "This is my first point", "This is my second point", "And here's a third!" };
                thisPin.InitComponent("list", null, sL);
            }

            if (thisPin == null) return;

            allPins.Add(thisPin);
            PinGrid.Children.Add(thisPin.NoteGrid);
            focusedPin = thisPin; // a new pin will always be in focus for convenience
            pinsInBoard++;
        }
        public void OnWindowResize(object sender, SizeChangedEventArgs e)
        {
            windowDimensions[0] = e.NewSize.Width;
            windowDimensions[1] = e.NewSize.Height;

            RecalculateAllPositions();
        }
        public void FilterClicked(object sender, RoutedEventArgs e)
        {
            foreach (BasePin bp in allPins) { bp.NoteGrid.Visibility = Visibility.Visible; }
            string searchTerm = FilterEntry.Text;
            if(searchTerm == string.Empty) return;

            subPinCount = 0;

            foreach(BasePin bp in allPins)
            {
                bool matchFound = false;
                foreach (string s in bp.rawStringList)
                {
                    if (s.ToLower().Contains(searchTerm.ToLower()))
                    {
                        matchFound = true;
                        bp.NoteGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BB6666"));
                    }
                }
                if (!matchFound)
                {
                    bp.NoteGrid.Visibility = Visibility.Collapsed;
                }
                subPinCount += matchFound ? 1 : 0;
            }
        }
        public void RecalculateAllPositions()
        {
            foreach(BasePin p in allPins)
            {
                p.CalculateMyPosition(windowDimensions);
                p.NoteGrid.Margin = new Thickness(p.myPosition[0], p.myPosition[1], 0, 0);
            }
        }
        public void PopoutToolbar(object sender, RoutedEventArgs e)
        {
            Grid popout = null;
            if (((Button)sender).Tag.ToString() == "empty")
                popout = PresetPopoutMenu;
            else popout = ComponentPopoutMenu;

            popout.Visibility = Visibility.Visible;
            popout.MouseLeave += ClosePopout;
        }
        public void ClosePopout(object sender, RoutedEventArgs e)
        {
            ((Grid)sender).Visibility = Visibility.Collapsed;
            ((Grid)sender).MouseLeave -= ClosePopout;
        }
        public void LoadAllPins(string loadBoard = "board")
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = System.IO.Path.Combine(currentPath, "boards");
            string filePath = System.IO.Path.Combine(directoryPath, $"{loadBoard}.json");

            boardName = loadBoard;

            int[] hwArray = new int[2];
            hwArray[0] = pinHeight;
            hwArray[1] = pinWidth;

            try
            {
                string json = File.ReadAllText(filePath);
                SaveData? data = JsonSerializer.Deserialize<SaveData>(json);

                if (data != null)
                {
                    float fileVersion = data.version;

                    if (fileVersion == 1.2f) //file is formatted appropriately 
                    {
                        //put the new load code here when save is changed
                    }
                    else //file is using deprecated format. place old code in LegacyLoad.cs each version change
                    {
                        LegacyLoad nLL = new LegacyLoad(fileVersion, data, this);
                    }
                    this.Title = boardName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load file: {ex.Message}.", "File load error", MessageBoxButton.OK);
            }
        }
        public void SaveAllPins(object sender, RoutedEventArgs e)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = System.IO.Path.Combine(currentPath, "boards"); // define the path first
            if (!Directory.Exists(directoryPath))// make board if it does not exist already
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine("Directory created at " + directoryPath);
            }
            else { Console.WriteLine("Directory "+ directoryPath +" exists."); }

            List<PinDataObject> pinDataList = new List<PinDataObject>();
            foreach (BasePin bp in allPins)
            {
                List<string> contents = new List<string>();
                foreach (string s in bp.rawStringList) contents.Add(s);
                PinDataObject thisData = new PinDataObject
                {
                    index = bp.orderInBoard,
                    stringList = contents
                };
                pinDataList.Add(thisData);
            }
            try
            {
                if (boardName == string.Empty) //when opened as a new board, prompt for a file name
                {
                    mySaveWindow = new FileSaveWindow(this);
                    mySaveWindow.ShowDialog();
                }
                if (boardName != string.Empty) //if a file name was not entered, do not save new
                {
                    SaveData data = new SaveData
                    {
                        version = saveLoadVersion,
                        myPinObjects = pinDataList
                    };

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(data, options); //convert the pins to json
                    File.WriteAllText(System.IO.Path.Combine(directoryPath, $"{boardName}.json"), json); //serialise the json data

                    MessageBox.Show("Successfully saved board.");
                    MainWindow? m = GetWindow(App.Current.MainWindow) as MainWindow;
                    m?.LoadAllBoards();
                }

                if (closeAfterSave) this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}.", "File save error", MessageBoxButton.OK);
                Console.WriteLine($"File could not be saved: {ex.Message}.");
            }
        }
        public class SaveData //object that is serialzed into json
        {
            public float version { get; set; }
            public List<PinDataObject>? myPinObjects { get; set; }
            public List<string>? myPins { get; set; } //deprecated
        }
        public class PinDataObject
        {
            public int index;
            public List<string>? stringList { get; set; }
            public string bgColour { get; set; }
        }
        public class BasePin
        {
            public BoardWindow myManager;
            public Grid NoteGrid { get; private set; }
            public Image PinImage {  get; private set; }
            public Border PinBorder { get; private set; }
            StackPanel MainStack = new StackPanel();
            public List<string> rawStringList = new List<string>();
            public List<PinComponent> componentList = new List<PinComponent>();
            public int orderInBoard; //irrelevant when filtering, find a new way to assign positions
            public int width, height;
            public int[] myPosition = new int[2]; //ACTUAL position
            public int totalLines = 1;
            public bool isBeingDragged;
            public bool hidden;
            public Point startPoint;
            public Grid NoteParent;
            public string? type;
            public Vector2 xyCoord = new Vector2(); //coordinate position

            public string bgColour = string.Empty;
            public BasePin(Grid parent, BoardWindow manager, int index, int[] hwArray, double[] windowSize, string colour = "#FFFCF8F3")
            {
                NoteParent = parent;
                width = hwArray[1];
                height = hwArray[0];
                orderInBoard = index;
                myManager = manager;
                bgColour = colour;
                CalculateMyPosition(windowSize);

                //create grid
                NoteGrid = new Grid();
                NoteGrid.Height = hwArray[0];
                NoteGrid.Width = hwArray[1];
                NoteGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colour));
                NoteGrid.HorizontalAlignment = HorizontalAlignment.Left;
                NoteGrid.VerticalAlignment = VerticalAlignment.Top;
                NoteGrid.Margin = new Thickness(myPosition[0], myPosition[1], 0, 0);
                
                //create border
                PinBorder = new Border();
                PinBorder.HorizontalAlignment = HorizontalAlignment.Center;
                PinBorder.VerticalAlignment = VerticalAlignment.Center;

                // Create Image
                PinImage = new Image();
                PinImage.Source = new BitmapImage(new Uri("images/pushpingraphic.png", UriKind.Relative));
                PinImage.Stretch = Stretch.Fill;
                PinImage.Height = 10;
                PinImage.Width = 10;
                PinImage.HorizontalAlignment = HorizontalAlignment.Center;
                PinImage.VerticalAlignment = VerticalAlignment.Top;
                PinImage.Margin = new Thickness(0,2,0,0); 

                //Add border and content to the grid
                NoteGrid.Children.Add(PinBorder);
                NoteGrid.Children.Add(PinImage);

                PinBorder.Child = MainStack;
                PinBorder.MouseDown += (sender, e) => { myManager.focusedPin = this; };
            }
            public void InitComponent(string name, string content, List<string> contentList)
            {
                PinComponent newComponent = null;
                if(name == "content")//TEMPORARY**** FIND A MORE ROBUST WAY
                {
                    newComponent = new PinContent(componentList.Count, this, width-10, content);
                    componentList.Add(newComponent);
                }
                else if(name == "title")
                {
                    newComponent = new TitleBox(componentList.Count, this, width-10, content);
                    componentList.Add(newComponent);
                }
                else if(name == "list")
                {
                    newComponent = new ListBox(contentList, componentList.Count, this, width - 10);
                }
                MainStack.Children.Add(newComponent.wrapper);

                PinResize();
            }
            public void CalculateMyPosition(double[] windowSize) // calls when the window size is changed and once on init
            {
                int myX = 0; //this pin's column
                int myY = 0; //this pin's row
                int maxWidth = (int)windowSize[0] - 95; // the width of the note panel, window size minus toolbar size

                int xTotal = (orderInBoard * width + (orderInBoard * 5)); // the total distance (in a straight line) of this note from the origin

                while(xTotal >= maxWidth)  
                {
                    // while the xTotal exceeds the length of a single row
                    // subtract that length, and shift the pin to the next row
                    myY += 1;
                    xTotal -= maxWidth;
                }
                while(xTotal > (width + 5))
                {
                    // while the xTotal exceeds the width of a pin + the offset distance
                    // subtract that length, and shift the pin over a column
                    myX += 1;
                    xTotal -= (width + 5);
                }
                int heightSum = 0; //read from the dictionary entry just above?
                for (int i = 0; i < myY; i++)
                {
                    if(myY > 0)
                    {
                        Vector2 aboveInColumn = new Vector2(myX, i);
                        if(!myManager.CoordForPinDict.ContainsKey(aboveInColumn))
                        {
                            heightSum += 120;
                        }
                        else
                        {
                            BasePin aboveP = (BasePin)myManager.CoordForPinDict[aboveInColumn];
                            heightSum += aboveP.height;
                        }
                    }
                }

                xyCoord[0] = myX; xyCoord[1] = myY;
                myManager.CoordForPinDict[xyCoord] = this;

                myPosition[0] = 85 + ((myX * width) + (myX * 5)); // x position,
                myPosition[1] = 5 + ((heightSum) + (myY * 5)); // y position
            }
            public void PinResize()
            {
                totalLines = 0;
                foreach (PinComponent pc in componentList) totalLines += pc.lines;
                if (totalLines <= 4) height = 120;
                else height = 120 + ((totalLines - 4) * 15);

                NoteGrid.Height = height;
                myManager.RecalculateAllPositions();
            }
        }
    }
}
