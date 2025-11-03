using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PinHoard.viewmodel
{
    public class FileSaveViewModel
    {
        public string newBoardName;
        public bool closeAfterSave;
        FileSaveWindow view;
        public FileSaveViewModel()
        {
            view = new FileSaveWindow(this);
            view.ShowDialog();
        }
        public FileSaveViewModel(string name)
        {
            newBoardName = name;
        }
        public string ValidateFilename(string entered, bool close)
        {
            if (string.IsNullOrEmpty(entered) || string.IsNullOrWhiteSpace(entered))
            {
                return("Filename cannot be empty.");
            }
            //Regex rg = new Regex(@"^[a-zA-Z0-9 . _ -]*$");
            Regex rg = new Regex(@"^[\w\-. ]*$");
            if (rg.IsMatch(entered))
            {
                // valid file name

                newBoardName = entered;
                view.Close();
                return "";
            }
            else
            {
                return("An invalid character was entered.");
            }
        }
    }
}
