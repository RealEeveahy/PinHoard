using PinHoard.util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard.view.menus
{
    public class BoardSelectWidget : CompositeWidget
    {
        public CheckBox BoardCheck = new CheckBox();
        public bool selected => BoardCheck.IsChecked ?? false;
        public BoardSelectWidget(string boardname, int index, Action<int, bool> onToggle)
        {
            wrapper.Height = 30;

            BoardCheck.VerticalAlignment = VerticalAlignment.Center;
            BoardCheck.HorizontalAlignment = HorizontalAlignment.Left;

            Label BoardNameLabel = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(30, 0, 0, 0),
                Content = boardname
            };

            BoardCheck.Checked += (sender, e) => { onToggle(index, false); };
            BoardCheck.Unchecked += (sender, e) => { onToggle(index, true); };

            Build(new List<UIElement>() { BoardCheck, BoardNameLabel });
        }
    }
}
