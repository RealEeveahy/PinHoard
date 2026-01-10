using PinHoard.model.pins;
using System;
using System.Collections.Generic;

namespace PinHoard.model.save_load
{
    /// <summary>
    /// Represents a pin model in serializable form, as only a background colour and list of serialized component models
    /// </summary>
    [Serializable]
    public class SerializablePin
    {
        public int index;
        public List<SerializableComponent>? components { get; set; }
        public string bgColour_hexCode { get; set; }
        public SerializablePin()
        {

        }
        public SerializablePin(List<SerializableComponent> components, string colour)
        {
            this.components = components;
            this.bgColour_hexCode = colour;
        }
        /// <summary>
        /// Self-Serializing Constructor to be used by the SerializableBoard class
        /// </summary>
        /// <param name="pin">The pin in its orignal form.</param>
        public SerializablePin(Pin_Model pin)
        {
            components = new List<SerializableComponent>();
            foreach (ComponentBase component in pin.componentList) components.Add(new SerializableComponent(component));
            this.bgColour_hexCode = pin.bgColour;
        }
    }
}
