using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PinHoard.model.pins
{
    public class Pin_Model
    {
        public ObservableCollection<ComponentBase> componentList { get; } = new ObservableCollection<ComponentBase>();
        public string bgColour = string.Empty;
        public int componentCount => componentList.Count;

        // Default constructor to generate an empty pin
        public Pin_Model()
        {
            bgColour = "#FFFCF8F3";
        }
        // 'New' pin created by the user
        public Pin_Model(List<ComponentBase> components)
        {
            bgColour = "#FFFCF8F3";
            foreach (ComponentBase component in components)
                AddComponent(component);
        }
        // Pin loaded from file
        public Pin_Model(List<ComponentBase> components, string bgColour)
        {
            this.bgColour = bgColour;
            foreach (ComponentBase component in components)
                AddComponent(component);
        }

        public void AddComponent(ComponentBase newComponent)
        {
            componentList.Add(newComponent);

            // add new component to the raw list
        }
        public bool ContainsFilterTerm(string term)
        {
            bool match = false;
            foreach (ComponentBase component in componentList)
            {
                if (component.GetContent().ToLower().Contains(term.ToLower()))
                    match = true;
            }
            return match;
        }
        public List<(string, string)> rawStringList()
        {
            List<(string c, string f)> rawComponents = new List<(string c, string f)>(); //content, format

            foreach (ComponentBase component in componentList)
                rawComponents.Add((component.GetContent(), component.format));

            return rawComponents;
        }
        public bool IsNewList => componentList.Last() is ListPoint ? false : true;
    }
}
