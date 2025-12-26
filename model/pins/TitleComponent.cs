using PinHoard.model.save_load;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinHoard.model.pins
{
    internal class TitleComponent : ComponentBase
    {
        public TitleComponent(int order, int width = 120, string content = "This is a title.")
        {
            orderInPin = order;
            format = "title";
            style = "default";

            userInput = new TextBox
            {
                Text = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = width,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3")),
                BorderThickness = new Thickness(0, 0, 0, 2),
                Margin = new Thickness(0, 6, 0, 0),
                FontSize = 14
            };

            Build(new List<UIElement> { userInput });

            userInput.TextChanged += (sender, e) => RaiseChanged();
            userInput.GotFocus += (sender, e) => RaiseFocused();
        }
        public override SerializableComponent GetSerializable() => new SerializableComponent(GetContent(), format);
    }
}