using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using static PinHoard.BoardWindow;
using System.Windows.Media;
using System.Windows.Shapes;
using System.DirectoryServices.ActiveDirectory;
using PinHoard.style;

namespace PinHoard.model.pins
{
    // 13/09/2025. Im not really sure what this is for. the styles could probably just be moved into the components?
    static internal class PinLib
    {
        public static List<Type> componentLibrary = new List<Type>() 
        { 
            typeof(PinContent),
            typeof(TitleComponent),
            typeof(ListComponent),
            typeof(ListPoint)
        };

        public static List<ComponentStyle> styleLibrary = new List<ComponentStyle>()
        {
            new ComponentStyle("Default (Content)", 12, "#FFFCF8F3", "#FF000000"), 
            new ComponentStyle("Code", 12, "#FF000000", "#FFFCF8F3"), //optional style for content to look like a code block
            new ComponentStyle("Default (Title)", 14, "#FFFCF8F3", "#FF000000"),
            new ComponentStyle("Default (List)", 12, "#FFFCF8F3", "#FF000000")
        };

        public static Dictionary<string, ComponentStyle> styleDictionary = new Dictionary<string, ComponentStyle>();

        static PinLib()
        {
            foreach(ComponentStyle cs in styleLibrary)
            {
                styleDictionary[cs.styleName] = cs;
            }
        }
    }
    public interface PinComponent
    {
        public StackPanel wrapper { get; set; }
        public string format { get; set; }
        public string style { get; set; }
        public int lines { get; set; }
        public int GetHeight(); // expect different components to have unique objects that may define the overall height. use this to get them.
        public int GetOrder(); // order of the component is subject to change and thus should be dynamically accessible.
        public int GetLines(); // universally accessible such that a listbox is responsible for its own children
    
    }

    public class TextChangeManager
    {
        PinComponent component;
        TextBox target;
        BasePin parent;
        public TextChangeManager(PinComponent parentComponent, TextBox target, BasePin parent)
        {
            component = parentComponent;
            this.target = target;
            this.parent = parent;
        }
        public void HandleTextChange(object sender, TextChangedEventArgs e)
        {
            target.TextChanged -= HandleTextChange;
            int orderInPin = component.GetOrder();

            parent.rawStringList[orderInPin] = (target.Text.Replace("\n", string.Empty), component.format);
            StringBuilder newText = new StringBuilder();
            int charCount = 0;
            int lastWordIndex = 0;
            int newLineCount = 1;

            foreach (char c in parent.rawStringList[orderInPin].Item1)
            {
                newText.Append(c);
                if (c != '.' || c != ',') charCount++;
                if (c == ' ') lastWordIndex = newText.Length;

                if (charCount % 16 == 0)
                {
                    newText.Insert(lastWordIndex, "\n");
                    newLineCount++;
                }
            }
            int caretPos = target.CaretIndex;
            if (component.lines != newLineCount) //if the no. of lines before changing text is different after, recalculate the pin height
            {
                component.lines = newLineCount;
                parent.PinResize();
            }

            target.Text = newText.ToString();

            target.CaretIndex = Math.Min(caretPos + (target.Text.Length - parent.rawStringList[orderInPin].Item1.Length), target.Text.Length);

            target.TextChanged += HandleTextChange;
        }
    }
}
