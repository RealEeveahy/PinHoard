using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PinHoard.BoardWindow;

namespace PinHoard.model.save_load
{
    public class SerializablePin
    {
        public int index;
        public List<SerializableComponent>? components { get; set; }
        public string bgColour { get; set; }
        public List<string>? stringList { get; set; } //deprecated
        public SerializablePin(int index, List<SerializableComponent>? components, string bgColour)
        {
            this.index = index;
            this.components = components;
            this.bgColour = bgColour;
        }
        public SerializablePin(int index, string bgColour, List<string>? stringList)
        {
            this.index = index;
            this.bgColour = bgColour;
            this.stringList = stringList;
        }
    }
}
