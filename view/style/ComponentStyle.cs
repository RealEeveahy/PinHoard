namespace PinHoard.style
{
    public class ComponentStyle
    {
        public string styleName;
        public int fontSize;
        public string backgroundColour; //as a hex value
        public string foregroundColour;
        public ComponentStyle(string styleName, int fontSize, string backgroundColour, string foregroundColour)
        {
            this.styleName = styleName;
            this.fontSize = fontSize;
            this.backgroundColour = backgroundColour;
            this.foregroundColour = foregroundColour;
        }
    }
}