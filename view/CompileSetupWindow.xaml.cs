using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PinHoard.model;

namespace PinHoard
{
    public partial class CompileSetupWindow : Window
    {
        public List<BoardWidget> widgets = new List<BoardWidget>();
        public CompileSetupWindow(List<string> filenames)
        {
            InitializeComponent();

            if (filenames.Count == 0)
            {
                MessageBox.Show("No boards to revise.");
            }
            else
            {
                int i = 0;
                foreach (string filename in filenames)
                {
                    BoardWidget newWidget = new BoardWidget(filename);

                    SelectBoardPanel.Children.Add(newWidget);
                    widgets.Add(newWidget);

                    i++;
                    if (this.Height < 600) this.Height += 30;
                }
            }

            StartButton.Click += CheckSelected;
        }
        public void CheckSelected(object sender, RoutedEventArgs e)
        {
            List<string> filenames = new List<string>();
            foreach(BoardWidget widget in widgets)
            {
                if (widget.selected)
                {
                    filenames.Add(widget.filename);
                }
            }
            if(filenames.Count > 0) 
            {
                //create a new window for the quiz
                Board compiledBoard = new Board(filenames);
                this.Hide();
            }
            else if(filenames.Count == 0)
            {
                MessageBox.Show("Please select one or more boards to compile.");
            }
        }
    }
}
