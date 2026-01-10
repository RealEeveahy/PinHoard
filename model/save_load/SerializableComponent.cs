using PinHoard.model.pins;
using System;

namespace PinHoard.model.save_load
{
    /// <summary>
    /// Represents a component model in serializable form, as only its content and format.
    /// </summary>
    [Serializable]
    public class SerializableComponent
    {
        public string stringContent { get; set; }
        public string format { get; set; }
        public SerializableComponent()
        {

        }
        public SerializableComponent(string content, string format)
        {
            stringContent = content;
            this.format = format;
        }
        public SerializableComponent(ComponentBase component)
        {
            stringContent = component.GetContent();
            format = component.format;
        }
    }
}
