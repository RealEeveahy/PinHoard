using PinHoard.view.style;
using PinHoard.viewmodel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        //public bool closeAfterSave = false; // changed by the fileSaveWindow

        public double[] windowDimensions = new double[2] { 800, 700 };
        private readonly Board_ViewModel viewModel;
        public BoardWindow(Board_ViewModel vm)
        {
            DataContext = vm;
            viewModel = vm;
            InitializeComponent();

            this.Title = vm.displayName;

            this.SizeChanged += (sender, e) => { vm.SizeChanged(new double[2] { e.NewSize.Width, e.NewSize.Height }); };

            ColourPickerPopout.Target = ColourPickButton;
            PinScrollViewer.Content = vm.pinContainer;

            FilterConfirmButton.Click += (sender, e) => viewModel.FilterBoard(FilterEntry.Text);
            if (!viewModel.readOnly) // disable all the buttons if read only
            {
                NewEmptyButton.MouseEnter += PopoutToolbar;
                NewContentButton.MouseEnter += PopoutToolbar;
            }
        }
        public void Build()
        {

        }
        private void ToggleDebug(object sender, RoutedEventArgs e) { viewModel.DebugFocused(); }
        private void SaveBoard(object sender, RoutedEventArgs e) { viewModel.Save(); }
        private void NewPinClicked(object sender, RoutedEventArgs e)
        {
            viewModel.AddPinPrefab(((ImageButton)sender).TagValue.ToString());
            PresetPopoutContainer.IsOpen = false;
        }
        private void NewComponentClicked(object sender, RoutedEventArgs e)
        {
            viewModel.AddComponentToFocus(((ImageButton)sender).TagValue.ToString());
            ComponentPopoutContainer.IsOpen = false;
        }
        private void ColourPickToggle(object sender, RoutedEventArgs e)
        {
            ColourPickerPopout.ColourPickState = !ColourPickerPopout.ColourPickState;
        }
        public void PopoutToolbar(object sender, RoutedEventArgs e)
        {
            Popup hoveredPopout = null;
            if (((ImageButton)sender).TagValue.ToString() == "empty")
                hoveredPopout = PresetPopoutContainer;
            else hoveredPopout = ComponentPopoutContainer;

            hoveredPopout.IsOpen = true;
            StackPanel? popoutContent = hoveredPopout.Child as StackPanel;
            if (popoutContent != null)
                popoutContent.MouseLeave += (sender, e) =>
                {
                    hoveredPopout.IsOpen = false;
                };
        }
        public void ClosePopout(object sender, RoutedEventArgs e)
        {
            //((StackPanel)sender).IsOpen = false;
            ((StackPanel)sender).MouseLeave -= ClosePopout;
        }
    }
}
