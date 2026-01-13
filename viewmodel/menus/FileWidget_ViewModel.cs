using PinHoard.util;

namespace PinHoard.viewmodel.menus
{
    public class FileWidget_ViewModel
    {
        public string filename;
        public string FileDisplayName { get; set; }
        public FileWidget_ViewModel(string filename)
        {
            FileDisplayName = PinHoardHelpers.CutExtension(filename);
            this.filename = filename;
        }
    }
}
