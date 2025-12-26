using PinHoard.model.pins;
using System;

namespace PinHoard.model.save_load
{
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
