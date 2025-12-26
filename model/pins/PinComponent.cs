using PinHoard.model.save_load;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard.model.pins
{
    public interface IComponent
    {
        StackPanel wrapper { get; }
        string format { get; }
        string style { get; }
        int lines { get; }
        double GetHeight(); // expect different components to have unique objects that may define the overall height. use this to get them.
        int GetOrder(); // order of the component is subject to change and thus should be dynamically accessible.
    }
    public abstract class ComponentBase : IComponent
    {
        public StackPanel wrapper { get; set; } = new StackPanel();
        public string format { get; set; }
        public string style { get; set; }
        public int lines { get; set; } = 1;
        protected int orderInPin { get; set; }
        public TextBox userInput { get; set; } = new TextBox();
        public string GetContent() => userInput.Text;
        public double GetHeight() => userInput.ActualHeight;
        public int GetOrder() => orderInPin;
        public abstract SerializableComponent GetSerializable(); // each component must be able to return a serializable version of itself

        public event Action<ComponentBase> Focused;
        public event Action<ComponentBase> Changed;

        protected void RaiseFocused() => Focused?.Invoke(this);
        protected void RaiseChanged() => Changed?.Invoke(this);

        public void SetParent(Panel parent)
        {
            wrapper.Margin = new Thickness(4);
            parent.Children.Add(wrapper);
        }
        public virtual void Build(List<UIElement> children)
        {
            foreach (UIElement child in children) wrapper.Children.Add(child);
        }
    }
    public class ComponentContainer
    {
        public StackPanel wrapper { get; set; } = new StackPanel();
        private readonly List<ComponentBase> children = new List<ComponentBase>();
        public string style { get; set; }
        public void SetParent(Panel parent) => parent.Children.Add(wrapper);
        public void AddChild(ComponentBase child)
        {
            children.Add(child);
            wrapper.Children.Add(child.wrapper);
        }
        public int GetLines()
        {
            int totalLines = 0;
            foreach (ComponentBase child in children) totalLines += child.lines;
            return totalLines;
        }
    }
}
