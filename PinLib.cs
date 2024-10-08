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
        
    }
    public class PinContent : PinComponent //pin component for displaying plain text - default
    {
        private Pin parentPin;
        public TextBox tb { get; private set; }
        public PinContent(string content, Pin parent, int width) 
        {
            parentPin = parent;
            tb = new TextBox
            {
                Text = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = width,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3")),
                BorderThickness = new Thickness(0, 0, 0, 0)
            };

            tb.TextChanged += TextChangeHandler;
            TextChangeHandler(this, null);
        }
        private void TextChangeHandler(object sender, TextChangedEventArgs e)
        {
            tb.TextChanged -= TextChangeHandler;

            parentPin.rawPinContent = tb.Text.Replace("\n", string.Empty);
            StringBuilder newText = new StringBuilder();
            int charCount = 0;
            int lastWordIndex = 0;
            int lineCount = 1;

            foreach (char c in parentPin.rawPinContent)
            {
                newText.Append(c);
                charCount++;
                if (c == ' ') lastWordIndex = newText.Length;

                if (charCount % 16 == 0)
                {
                    newText.Insert(lastWordIndex, "\n");
                    lineCount++;
                }
            }
            int caretPos = tb.CaretIndex;
            if (parentPin.lines != lineCount) //if the no. of lines before changing text is different after, recalculate the pin height
            {
                if (parentPin.type != null) parentPin.PinResize(parentPin.type, lineCount);
            }

            tb.Text = newText.ToString();

            tb.CaretIndex = Math.Min(caretPos + (tb.Text.Length - parentPin.rawPinContent.Length), tb.Text.Length);

            tb.TextChanged += TextChangeHandler;
        }
    }
    public class PointBox : PinComponent //pin component for dot points
    {
        int orderInPin = 0;
        private MultiPin parentPin;
        public StackPanel wrapper { get; private set; }
        public StackPanel thisPoint { get; private set; }
        public Ellipse bulletPoint { get; private set; }
        public TextBox thisContent { get; private set; }
        public PointBox(string content, int order, MultiPin parent, int width)
        {
            orderInPin = order;
            parentPin = parent;
            wrapper = new StackPanel();
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

            thisContent.TextChanged += TextChangeHandler;
            parentPin.rawContentList.Add(content);
        }
        private void TextChangeHandler(object sender, TextChangedEventArgs e)
        {
            if (thisContent != null)
            {
                thisContent.TextChanged -= TextChangeHandler;

                parentPin.rawContentList[orderInPin] = thisContent.Text.Replace("\n", string.Empty);
                StringBuilder newText = new StringBuilder();
                int charCount = 0;
                int lastWordIndex = 0;

                foreach (char c in parentPin.rawContentList[orderInPin])
                {
                    newText.Append(c);
                    charCount++;
                    if (c == ' ') lastWordIndex = newText.Length;

                    if (charCount % 12 == 0)
                    {
                        newText.Insert(lastWordIndex, "\n");
                    }
                }
                int caretPos = thisContent.CaretIndex;

                thisContent.Text = newText.ToString();

                thisContent.CaretIndex = Math.Min(caretPos + (thisContent.Text.Length - parentPin.rawContentList[orderInPin].Length), thisContent.Text.Length);

                thisContent.TextChanged += TextChangeHandler;
            }
        }
    }
}
