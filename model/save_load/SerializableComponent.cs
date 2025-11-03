using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinHoard.model.save_load
{
    public class SerializableComponent
    {
        public string? stringContent { get; set; }
        public string? format { get; set; }
        public List<string>? contentList { get; set; }
        public SerializableComponent(string stringContent, string format)
        {
            this.stringContent = stringContent;
            this.format = format;
        }
        public SerializableComponent(List<string> strings)
        {
            contentList = strings;
            format = "list";
        }
    }
}
