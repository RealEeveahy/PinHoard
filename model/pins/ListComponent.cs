using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PinHoard.model.pins
{
    public class ListComponent : PinComponent
    {
        private BasePin parentPin;
        public List<ListPoint> Children = new();
        public StackPanel wrapper { get; set; } = new StackPanel();
        public string format { get; set; } = "list";
        public string style { get; set; } = "default";
        public int lines { get; set; } = 0;
        private int myWidth;
        public ListComponent(List<string> contents, int startOrder, BasePin parent, int width)
        {
            parentPin = parent;
            myWidth = width;

            List<ListPoint> points = StringsToPoints(contents, startOrder);
        }
        public int GetHeight()
        {
            return 0;
        }
        public int GetOrder()
        {
            return 0; //should not need to access for this component
        }
        public int GetLines()
        {
            int L = 0;
            foreach (ListPoint point in Children)
            {
                L += point.GetLines();
            }
            return L;
        }

        List<ListPoint> StringsToPoints(List<string> strings, int startOrder) //method for quickly turning a list into several points
        {
            var list = new List<ListPoint>();
            int i = 0;
            foreach (string s in strings)
            {
                ListPoint newPoint = new ListPoint(s, startOrder + i, parentPin, myWidth, this);
                list.Add(newPoint);
                i++;

                //parentPin.componentList.Add(newPoint);
                Children.Add(newPoint);
            }

            return list;
        }
    }
}
