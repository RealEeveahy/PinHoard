using PinHoard.model;
using PinHoard.model.pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PinHoard.service
{
    static class CalculatePosition
    {
        public static RepositionInfo GetNewPinPosition(double[] windowDimensions, int subOrder, BasePin pin, BasePin? above)
        {
            int myX = 0; //this pin's column
            int myY = 0; //this pin's row
            int currentOrder = pin.orderInBoard;
            int maxWidth = (int)windowDimensions[0] - 95; // the width of the note panel, window size minus toolbar size
            if (subOrder > -1) currentOrder = subOrder;

            int xTotal = currentOrder * pin.width + currentOrder * 5; // the total distance (in a straight line) of this note from the origin

            while (xTotal >= maxWidth)
            {
                // while the xTotal exceeds the length of a single row
                // subtract that length, and shift the pin to the next row
                myY += 1;
                xTotal -= maxWidth;
            }
            while (xTotal > pin.width + 5)
            {
                // while the xTotal exceeds the width of a pin + the offset distance
                // subtract that length, and shift the pin over a column
                myX += 1;
                xTotal -= pin.width + 5;
            }
            int heightSum = 0; //read from the dictionary entry just above?
            if (above == null)
            {
                heightSum += 120;
            }
            else
            {
                heightSum += above.position[1];
                heightSum += above.height;
            }

            return new RepositionInfo(new Vector2( myX, myY ), 
                new int[] { 
                85 + myX * pin.width + myX * 5, //X
                5 + heightSum + myY * 5 //Y
                });
        }
    }
}

public class RepositionInfo
{
    public Vector2 coordinate;
    public int[] position;
    public RepositionInfo(Vector2 coordinate, int[] position)
    {
        this.coordinate = coordinate;
        this.position = position;
    }
}
