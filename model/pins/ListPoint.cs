using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PinHoard.model.pins
{
    public class ListPoint : PinComponent //pin component for dot points
    {
        private BasePin parentPin;
        public StackPanel wrapper { get; set; }
        public StackPanel thisPoint { get; private set; }
        public Ellipse bulletPoint { get; private set; }
        public TextBox thisContent { get; private set; }
        public string format { get; set; } = "list";
        public string style { get; set; } = "default";
        public int lines { get; set; } = 1;
        int orderInPin = 0;
        TextChangeManager textChangeManager;
        public ListPoint(string content, int order, BasePin parent, int width, ListComponent container)
        {
            orderInPin = order;
            parentPin = parent;
            wrapper = container.wrapper;
            parentPin.rawStringList.Insert(order, (content, format));
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

            thisContent.GotFocus += (sender, e) => { parentPin.GotFocus(); };
        }
        public int GetHeight()
        {
            return (int)thisContent.ActualHeight;
        }
        public int GetOrder()
        {
            return orderInPin;
        }
        public int GetLines()
        {
            return lines;
        }
    }
}
