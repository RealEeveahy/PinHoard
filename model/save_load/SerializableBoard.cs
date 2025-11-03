using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinHoard.model.save_load
{
    public class SerializableBoard
    {
        public float version { get; set; }
        public List<SerializablePin>? myPinObjects { get; set; }
        public List<string>? myPins { get; set; } //deprecated
        public SerializableBoard(float version, List<SerializablePin> myPinObjects) 
        {
            this.version = version;
            this.myPinObjects = myPinObjects;
        }
        public SerializableBoard(float version, List<string> myPins)
        {
            this.version = version;
            this.myPins = myPins;
        }
    }
}
