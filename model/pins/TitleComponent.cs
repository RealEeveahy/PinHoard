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
    public class TitleComponent : PinComponent //pin component that adds a title
    {
        private BasePin parentPin;
        public TextBox tb { get; private set; }
        public StackPanel wrapper { get; set; } = new StackPanel();
        int orderInPin;
        public string format { get; set; } = "title";
        public string style { get; set; } = "default";
        public int lines { get; set; } = 1;
        TextChangeManager textChangeManager;
        public TitleComponent(int order, BasePin parent, int width, string content = "This is a title.")
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
