using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using static PinHoard.BoardWindow;
using System.Windows.Media;
using System.Windows.Shapes;
using System.DirectoryServices.ActiveDirectory;

namespace PinHoard
{
    //Script for holding repeated classes
    internal class PinLib
    {

    }

    public class BoardWidget //widget for displaying filenames with checkboxes
    {
        public Grid WidgetGrid = new Grid();
        public CheckBox BoardCheck = new CheckBox();
        public Label BoardNameLabel = new Label();
        public bool selected = false;
        public string myFilename = string.Empty;
        public BoardWidget(string fn)
        {
            myFilename = fn;

            WidgetGrid.Height = 30;
            BoardCheck.VerticalAlignment = VerticalAlignment.Center;
            BoardCheck.HorizontalAlignment = HorizontalAlignment.Left;

            string nameWithoutExtension = fn.Substring(0, fn.Length - 5);

            BoardNameLabel.VerticalAlignment = VerticalAlignment.Center;
            BoardNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
            BoardNameLabel.Margin = new Thickness(30, 0, 0, 0);
            BoardNameLabel.Content = nameWithoutExtension;

            WidgetGrid.Children.Add(BoardCheck);
            WidgetGrid.Children.Add(BoardNameLabel);

            BoardCheck.Checked += (sender, e) =>
            {
                selected = (BoardCheck.IsChecked ?? false);
            };
            BoardCheck.Unchecked += (sender, e) =>
            {
                selected = (BoardCheck.IsChecked ?? false);
            };
        }
    }
    public interface PinComponent
    {
        public StackPanel wrapper { get; set; }
        public string format { get; set; }
        public int lines { get; set; }
        public int GetHeight(); // expect different components to have unique objects that may define the overall height. use this to get them.
        public int GetOrder(); // order of the component is subject to change and thus should be dynamically accessible.
    }
    public class PinContent : PinComponent //pin component for displaying plain text - default
    {
        private BasePin parentPin;
        public TextBox tb { get; private set; }
        public StackPanel wrapper { get; set; } = new StackPanel();
        int orderInPin;
        public string format { get; set; } = "content";
        public int lines { get; set; } = 1;
        public TextChangeManager textChangeManager;
        public PinContent(int order, BasePin parent, int width, string content = "This is a note!") 
        {
            parentPin = parent;
            orderInPin = order;
            parentPin.rawStringList.Insert(order,(content, format));
            tb = new TextBox
            {
                Text = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = width,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3")),
                BorderThickness = new Thickness(0, 0, 0, 0)
            };

            textChangeManager = new TextChangeManager(this, tb, parentPin);
            tb.TextChanged += textChangeManager.HandleTextChange;
            textChangeManager.HandleTextChange(this, null);

            wrapper.Children.Add(tb);
            tb.GotFocus += (sender, e) => { parentPin.myManager.focusedPin = parentPin; };
        }
        public int GetHeight()
        {
            return (int)tb.ActualHeight;
        }
        public int GetOrder()
        {
            return orderInPin;
        }
    }
    public class TitleBox : PinComponent //pin component that adds a title
    {
        private BasePin parentPin;
        public TextBox tb { get; private set; }
        public StackPanel wrapper { get; set; } = new StackPanel();
        int orderInPin;
        public string format { get; set; } = "title";
        public int lines { get; set; } = 1;
        TextChangeManager textChangeManager;
        public TitleBox(int order, BasePin parent, int width, string content = "This is a title.")
        {
            parentPin = parent;
            orderInPin = order;
            parentPin.rawStringList.Insert(order, (content, format)); //should be changed so title always has an order of 0
            tb = new TextBox()
            {
                Text = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                MinWidth = width,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3")),
                BorderThickness = new Thickness(0, 0, 0, 2),
                Margin = new Thickness(0, 6, 0, 0),
                FontSize = 14
            };

            textChangeManager = new TextChangeManager(this, tb, parentPin);
            tb.TextChanged += textChangeManager.HandleTextChange;
            textChangeManager.HandleTextChange(this, null);


            wrapper.Children.Add(tb);
            tb.GotFocus += (sender, e) => { parentPin.myManager.focusedPin = parentPin; };
        }
        public int GetHeight()
        {
            return (int)tb.ActualHeight;
        }
        public int GetOrder()
        {
            return orderInPin;
        }
    }
    public class ListBox : PinComponent
    {
        private BasePin parentPin;
        public StackPanel wrapper { get; set; } = new StackPanel();
        public string format { get; set; } = "list";
        public int lines { get; set; } = 0;
        private int myWidth;
        public ListBox(List<string> contents, int startOrder, BasePin parent, int width)
        {
            parentPin = parent;
            myWidth = width;

            List<PointBox> points = StringsToPoints(contents, startOrder);
        }
        public int GetHeight()
        {
            return 0;
        }
        public int GetOrder()
        {
            return 0; //should not need to access for this component
        }
        List<PointBox> StringsToPoints(List<string> strings, int startOrder) //method for quickly turning a list into several points
        {
            var list = new List<PointBox>();
            int i = 0;
            foreach (string s in strings)
            {
                PointBox newPoint = new PointBox(s, startOrder + i, parentPin, myWidth, this);
                list.Add(newPoint);
                i++;

                parentPin.componentList.Add(newPoint);
            }

            return list; 
        }
    }
    public class PointBox : PinComponent //pin component for dot points
    {
        private BasePin parentPin;
        public StackPanel wrapper { get; set; }
        public StackPanel thisPoint { get; private set; }
        public Ellipse bulletPoint { get; private set; }
        public TextBox thisContent { get; private set; }
        public string format { get; set; } = "list";
        public int lines { get; set; } = 1;
        int orderInPin = 0;
        TextChangeManager textChangeManager;
        public PointBox(string content, int order, BasePin parent, int width, ListBox container)
        {
            orderInPin = order;
            parentPin = parent;
            wrapper = container.wrapper;
            parentPin.rawStringList.Insert(order, (content,format));
            thisPoint = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            bulletPoint = new Ellipse
            {
                Width = 5,
                Height = 5,
                Fill = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 5, 0)
            };
            thisContent = new TextBox
            {
                Text = content,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = width - 10,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3")),
                BorderThickness = new Thickness(0, 0, 0, 0)
            };
            thisPoint.Children.Add(bulletPoint);
            thisPoint.Children.Add(thisContent);
            wrapper.Children.Add(thisPoint);

            textChangeManager = new TextChangeManager(this, thisContent, parentPin);
            thisContent.TextChanged += textChangeManager.HandleTextChange;
            textChangeManager.HandleTextChange(this, null);

            thisContent.GotFocus += (sender, e) => { parentPin.myManager.focusedPin = parentPin; };
        }
        public int GetHeight()
        {
            return (int)thisContent.ActualHeight;
        }
        public int GetOrder()
        {
            return orderInPin;
        }
    }
    public class TextChangeManager
    {
        PinComponent component;
        TextBox target;
        BasePin parent;
        public TextChangeManager(PinComponent parentComponent, TextBox target, BasePin parent)
        {
            this.component = parentComponent;
            this.target = target;
            this.parent = parent;
        }
        public void HandleTextChange(object sender, TextChangedEventArgs e)
        {
            target.TextChanged -= this.HandleTextChange;
            int orderInPin = component.GetOrder();

            parent.rawStringList[orderInPin] = (target.Text.Replace("\n", string.Empty), component.format);
            StringBuilder newText = new StringBuilder();
            int charCount = 0;
            int lastWordIndex = 0;
            int newLineCount = 1;

            foreach (char c in parent.rawStringList[orderInPin].Item1)
            {
                newText.Append(c);
                if (c != '.' || c != ',') charCount++;
                if (c == ' ') lastWordIndex = newText.Length;

                if (charCount % 16 == 0)
                {
                    newText.Insert(lastWordIndex, "\n");
                    newLineCount++;
                }
            }
            int caretPos = target.CaretIndex;
            if (component.lines != newLineCount) //if the no. of lines before changing text is different after, recalculate the pin height
            {
                component.lines = newLineCount;
                parent.PinResize();
            }

            target.Text = newText.ToString();

            target.CaretIndex = Math.Min(caretPos + (target.Text.Length - parent.rawStringList[orderInPin].Item1.Length), target.Text.Length);

            target.TextChanged += this.HandleTextChange;
        }
    }
}
