using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard.util
{
    public abstract class CompositeWidget
    {
        protected Grid wrapper { get; set; } = new Grid();
        public void SetParent(Panel parent)
        {
            parent.Children.Add(wrapper);
        }
        public virtual void Build(List<UIElement> children)
        {
            foreach (UIElement child in children) wrapper.Children.Add(child);
        }
    }
}
