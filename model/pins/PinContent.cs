using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinHoard.model.pins
{
    internal class PinContent : PinComponent //pin component for displaying plain text - default
    {
        private BasePin parentPin;
        public TextBox tb { get; private set; }
        public StackPanel wrapper { get; set; } = new StackPanel();
        int orderInPin;
        public string format { get; set; } = "content";
        public string style { get; set; } = "default";
        public int lines { get; set; } = 1;
        public TextChangeManager textChangeManager;
        public PinContent(int order, BasePin parent, int width, string content = "This is a note!")
        {
            parentPin = parent;
            orderInPin = order;
            parentPin.rawStringList.Insert(order, (content, format));
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
            tb.GotFocus += (sender, e) => { parentPin.GotFocus(); };
        }
        public int GetHeight()
        {
            return (int)tb.ActualHeight;
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
