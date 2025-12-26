using PinHoard.model.save_load;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PinHoard.model.pins
{
    public class ListPoint : ComponentBase
    {
        public ListPoint(int order, int width, /*ComponentContainer container,*/ string content = "This is a dot point.")
        {
            this.orderInPin = order;
            //wrapper = container.wrapper;
            format = "list";
            style = "default";

            DockPanel pointContainer = new DockPanel();

            Ellipse bulletPoint = new Ellipse
            {
                Width = 5,
                Height = 5,
                Fill = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 5, 0)
            };
            bulletPoint.SetValue(DockPanel.DockProperty, Dock.Left);

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

            pointContainer.Children.Add(bulletPoint);
            pointContainer.Children.Add(userInput);

            Build(new List<UIElement> { pointContainer });

            userInput.TextChanged += (sender, e) => RaiseChanged();
            userInput.GotFocus += (sender, e) => RaiseFocused();
        }
        public override SerializableComponent GetSerializable() => new SerializableComponent(GetContent(), format);
    }
}