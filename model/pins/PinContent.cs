using PinHoard.model.save_load;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinHoard.model.pins
{
    internal class PinContent : ComponentBase
    {
        public PinContent(int order, int width = 120, string content = "This is a note!")
        {
            orderInPin = order;
            format = "content";
            style = "default";

            userInput = new TextBox
            {
                Text = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = width,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3")),
                BorderThickness = new Thickness(0),
                TextWrapping = TextWrapping.Wrap
            };

            Build(new List<UIElement> { userInput });

            userInput.TextChanged += (sender, e) => RaiseChanged();
            userInput.GotFocus += (sender, e) => RaiseFocused();
        }
        public override SerializableComponent GetSerializable() => new SerializableComponent(GetContent(), format);
    }
}
