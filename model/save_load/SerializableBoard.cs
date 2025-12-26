using PinHoard.model.pins;
using System;
using System.Collections.Generic;

namespace PinHoard.model.save_load
{
    [Serializable]
    public class SerializableBoard
    {
        public float version { get; set; }
        public List<SerializablePin>? pins { get; set; }
        public SerializableBoard()
        {

        }
        public SerializableBoard(float version, List<SerializablePin> pinList)
        {
            this.version = version;
            this.pins = pinList;
        }
        /// <summary>
        /// 
        /// Self-Initializing constructor.
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="board"></param>
        public SerializableBoard(float version, Board board)
        {
            this.version = version;
            pins = new List<SerializablePin>();

            foreach (Pin_Model pin in board.allPins)
            {
                pins.Add(new SerializablePin(pin));
            }
        }
    }
}
