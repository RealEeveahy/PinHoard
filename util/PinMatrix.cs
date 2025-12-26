using PinHoard.view.notetaking;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinHoard.util
{
    public class PinMatrix : WrapPanel
    {
        public List<MatrixRow> rows = new List<MatrixRow>();
        public PinMatrix()
        {
            //this.Orientation = Orientation.Horizontal;
            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF9F0E0"));
        }
        public void Add(Pin_View item)
        {
            this.Children.Add(item);
            //if(rows.Count > 0)
            //{
            //    FrameworkElement parent = (FrameworkElement)LogicalTreeHelper.GetParent(this);

            //    if (rows.Last().WillOverflow(item.Width, parent.Width))
            //    {
            //        MatrixRow row = new MatrixRow();
            //        row.Add(item);
            //        rows.Add(row);
            //        this.Children.Add(row);
            //    }
            //    else
            //    {
            //        rows.Last().Add(item);
            //    }
            //}
            //else
            //{
            //    MatrixRow row = new MatrixRow();
            //    row.Add(item);
            //    rows.Add(row);
            //    this.Children.Add(row);
            //}
        }
    }

    public class MatrixRow : StackPanel, ICollection<Pin_View>
    {
        public List<Pin_View> childList = new List<Pin_View>();
        public MatrixRow()
        {
            this.Orientation = Orientation.Horizontal;
            this.Margin = new Thickness(5);
        }
        public void SetHeight()
        {
            double h = this.Height;
            foreach (Pin_View pin in childList) h = pin.Height > h ? pin.Height : h;
        }

        private double InnerWidth()
        {
            double total = 0;
            foreach (Pin_View pin in childList) total += pin.Width;
            return total;
        }

        public bool WillOverflow(double addWidth, double MaxWidth)
        {
            if (InnerWidth() + addWidth > MaxWidth) return true;
            else return false;
        }

        public int Count => ((ICollection<Pin_View>)childList).Count;

        public bool IsReadOnly => ((ICollection<Pin_View>)childList).IsReadOnly;

        public void Add(Pin_View item)
        {
            ((ICollection<Pin_View>)childList).Add(item);
            this.Children.Add(item);
            SetHeight();
        }

        public void Clear()
        {
            ((ICollection<Pin_View>)childList).Clear();
            this.Children.Clear();
            SetHeight();
        }

        public bool Contains(Pin_View item)
        {
            return ((ICollection<Pin_View>)childList).Contains(item);
        }

        public void CopyTo(Pin_View[] array, int arrayIndex)
        {
            ((ICollection<Pin_View>)childList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Pin_View> GetEnumerator()
        {
            return ((IEnumerable<Pin_View>)childList).GetEnumerator();
        }

        public bool Remove(Pin_View item)
        {
            if (((ICollection<Pin_View>)childList).Remove(item))
            {
                this.Children.Remove(item);
                SetHeight();
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)childList).GetEnumerator();
        }
    }
}
