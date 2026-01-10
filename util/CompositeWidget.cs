using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard.util
{
    /// <summary>
    /// An abstract class for a widget that can be defined in code with its own functions with easy parenting access
    /// </summary>
    /// <remarks>
    /// I made this before I really understood usercontrols, so it seems a bit obsolete
    /// </remarks>
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
