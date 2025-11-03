using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace PinHoard.model
{
    public class BoardWidget : Grid //widget for displaying filenames with checkboxes
    {
        public bool selected = false;
        public string filename;
        public BoardWidget(string fn)
        {
            filename = fn;
            Height = 30;

            CheckBox BoardCheck = new();
            BoardCheck.VerticalAlignment = VerticalAlignment.Center;
            BoardCheck.HorizontalAlignment = HorizontalAlignment.Left;

            string nameWithoutExtension = fn.Substring(0, fn.Length - 5);

            Label BoardNameLabel = new();
            BoardNameLabel.VerticalAlignment = VerticalAlignment.Center;
            BoardNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
            BoardNameLabel.Margin = new Thickness(30, 0, 0, 0);
            BoardNameLabel.Content = nameWithoutExtension;

            Children.Add(BoardCheck);
            Children.Add(BoardNameLabel);

            BoardCheck.Checked += (sender, e) =>
            {
                selected = BoardCheck.IsChecked ?? false;
            };
            BoardCheck.Unchecked += (sender, e) =>
            {
                selected = BoardCheck.IsChecked ?? false;
            };
        }
    }
}
