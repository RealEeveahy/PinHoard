using PinHoard.model.pins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm;
using System.ComponentModel;

namespace PinHoard.view
{
    public class PinView : Grid
    {
        public Image PinImage { get; private set; }
        public Border PinBorder { get; private set; }
        StackPanel MainStack = new StackPanel();
        BasePin model { get; set; }
        public PinView(BasePin model)
        {
            DataContext = model;
            this.model = model;

            //create grid
            Height = model.height;
            Width = model.width;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.colour));
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            UpdatePosition();

            //create border
            PinBorder = new Border();
            PinBorder.HorizontalAlignment = HorizontalAlignment.Center;
            PinBorder.VerticalAlignment = VerticalAlignment.Center;
            PinBorder.IsHitTestVisible = true;

            // Create Image
            PinImage = new Image();
            PinImage.Source = new BitmapImage(new Uri("images/pushpingraphic.png", UriKind.Relative));
            PinImage.Stretch = Stretch.Fill;
            PinImage.Height = 10;
            PinImage.Width = 10;
            PinImage.HorizontalAlignment = HorizontalAlignment.Center;
            PinImage.VerticalAlignment = VerticalAlignment.Top;
            PinImage.Margin = new Thickness(0, 2, 0, 0);

            //Add border and content to the grid
            Children.Add(PinBorder);
            Children.Add(PinImage);

            model.componentList.CollectionChanged += ComponentsChanged;
            model.PropertyChanged += PropertyChanged;
            model.trueWidth = ActualWidth;

            PinBorder.Child = MainStack;
            MouseDown += (sender, e) =>
            {
                model.GotFocus();

                /// Change this. weird. just get

                // add debug click
                //if (model.board.view.debugging) model.board.DebugBox(model.GetDebugInfo());
            };
        }
        void PropertyChanged(object obj, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BasePin.colour)) ColourChanged();
            else if (e.PropertyName == nameof(BasePin.height)) HeightChanged();
            else if (e.PropertyName == nameof(BasePin.position)) UpdatePosition();
            else if (e.PropertyName == nameof(BasePin.visibility)) VisibilityChanged();
        }
        void ColourChanged() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.colour)); }
        void HeightChanged()
        {
            UpdateLayout();
            Height = model.height;
            model.trueHeight = ActualHeight;
        }
        void UpdatePosition() { Margin = new Thickness(model.position[0], model.position[1], 0, 0); }
        void VisibilityChanged() { Visibility = model.visibility; }
        void ComponentsChanged(object obj, NotifyCollectionChangedEventArgs e)
        {
            MainStack.Children.Clear();
            foreach(PinComponent pc in model.componentList)
            {
                AddToStack(pc.wrapper);
            }
        }
        void AddToStack(StackPanel component)
        {
            MainStack.Children.Add(component);
        }
    }
}
