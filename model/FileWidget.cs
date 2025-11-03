using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PinHoard.model
{
    public class FileWidget : Grid
    {
        public Border WidgetBorder;
        public Label WidgetLabel;
        public Button OpenBoard;
        public Button BoardOptions;
        public string fName { get; set; }
        public Grid WidgetParent;
        public FileWidget(Grid parent, string fn, int index, string fullPath)
        {
            WidgetParent = parent;
            fName = fn;

            //create a grid
            this.Height = 60;
            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFCF8F3"));
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Margin = new Thickness(20, index * 60 + (20 + index * 20), 20, 0);

            //create border
            WidgetBorder = new Border();
            WidgetBorder.HorizontalAlignment = HorizontalAlignment.Center;
            WidgetBorder.VerticalAlignment = VerticalAlignment.Top;
            WidgetBorder.CornerRadius = new CornerRadius(10, 10, 10, 10);
            WidgetBorder.BorderThickness = new Thickness(1);

            WidgetLabel = new Label();
            WidgetLabel.HorizontalAlignment = HorizontalAlignment.Left;
            WidgetLabel.VerticalAlignment = VerticalAlignment.Center;
            WidgetLabel.Margin = new Thickness(20, 0, 0, 0);
            WidgetLabel.Content = fn;

            OpenBoard = new Button();
            OpenBoard.HorizontalAlignment = HorizontalAlignment.Right;
            OpenBoard.VerticalAlignment = VerticalAlignment.Center;
            OpenBoard.Height = 40;
            OpenBoard.Width = 60;
            OpenBoard.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF69176"));
            OpenBoard.BorderBrush = null;
            OpenBoard.Margin = new Thickness(0, 0, 20, 0);
            OpenBoard.Content = "Open";

            BoardOptions = new Button();
            BoardOptions.HorizontalAlignment = HorizontalAlignment.Right;
            BoardOptions.VerticalAlignment = VerticalAlignment.Center;
            BoardOptions.Height = 40;
            BoardOptions.Width = 40;
            BoardOptions.Margin = new Thickness(0, 0, 100, 0);

            Grid optionsSubGrid = new Grid();
            Image optionIcon = new Image();
            optionIcon.Source = new BitmapImage(new Uri("images/miniSettingGraphic.png", UriKind.Relative));
            optionIcon.Stretch = Stretch.Fill;
            optionsSubGrid.Children.Add(optionIcon);
            BoardOptions.Content = (optionsSubGrid);

            //add widget content to grid
            this.Children.Add(WidgetBorder);
            this.Children.Add(WidgetLabel);
            this.Children.Add(OpenBoard);
            this.Children.Add(BoardOptions);

            OpenBoard.Click += (sender, e) =>
            {
                Board board = new Board(fn);
                //foreach (BasePin bp in boardWindow.allPins) //one final attempt to have all pins correct size
                //{
                //    bp.PinResize();
                //}
            };
            BoardOptions.Click += (sender, e) =>
            {
                SettingsWindow settingsWindow = new SettingsWindow(fn, fullPath);
                settingsWindow.ShowDialog();
            };
        }
    }
}
