using PinHoard.model.pins;
using System;
using System.Collections.Generic;

namespace PinHoard.model.save_load
{
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
        /// <param name="pin"></param>
        public SerializablePin(Pin_Model pin)
        {
            components = new List<SerializableComponent>();
            foreach (ComponentBase component in pin.componentList) components.Add(new SerializableComponent(component));
            this.bgColour_hexCode = pin.bgColour;
        }
    }
}
