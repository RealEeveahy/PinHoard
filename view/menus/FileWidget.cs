using PinHoard.util;
using PinHoard.viewmodel.menus;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PinHoard.view.menus
{
    public class FileWidget : CompositeWidget
    {
        public string fName { get; set; }
        public FileWidget(string fn, int index, Main_ViewModel vm)
        {
            fName = fn;

            //create a grid
            wrapper.Height = 60;
            wrapper.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3"));
            wrapper.VerticalAlignment = VerticalAlignment.Top;
            wrapper.Margin = new Thickness(20, 20, 20, 0);

            //create border
            Border WidgetBorder = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                CornerRadius = new CornerRadius(10, 10, 10, 10),
                BorderThickness = new Thickness(1)
            };

            Label WidgetLabel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 0, 0, 0),
                Content = fn
            };

            Button OpenBoard = new()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 40,
                Width = 60,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF69176")),
                BorderBrush = null,
                Margin = new Thickness(0, 0, 20, 0),
                Content = "Open"
            };
            OpenBoard.Click += (sender, e) => { vm.OpenBoard(index); };

            Image optionIcon = new()
            {
                Source = new BitmapImage(new Uri("images/miniSettingGraphic.png", UriKind.Relative)),
                Stretch = Stretch.Fill
            };
            Grid optionsSubGrid = new();
            optionsSubGrid.Children.Add(optionIcon);

            Button BoardOptions = new()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 40,
                Width = 40,
                Margin = new Thickness(0, 0, 100, 0),
                Content = (optionsSubGrid)
            };
            BoardOptions.Click += (sender, e) => { vm.ModifyBoard(index); };

            //add widget content to grid
            Build(new List<UIElement>() { WidgetBorder, WidgetLabel, OpenBoard, BoardOptions });
        }
    }
}
