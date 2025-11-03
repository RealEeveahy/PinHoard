using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Reflection.Metadata;
using PinHoard.model.pins;
using PinHoard.viewmodel;
using System.Collections.Specialized;
using PinHoard.view;

namespace PinHoard
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        BoardViewModel boardVM;
        public bool closeAfterSave = false; // changed by the fileSaveWindow

        private FileSaveWindow? mySaveWindow = null;
        public BoardWindow(bool r_o, BoardViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            boardVM = vm;
            this.Title = vm.boardName;
            this.SizeChanged += OnWindowResize;

            boardVM.allPins.CollectionChanged += PinsChanged;

            MainGrid.Children.Add(new ColourPicker(boardVM)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(170,30,0,0)
            });
            FilterConfirmButton.Click += SetFilter;
            DebugButton.Click += vm.ToggleDebug;
            if (!r_o) // disable all the buttons if read only
            {
                NewEmptyButton.MouseEnter +=  PopoutToolbar;
                NewContentButton.MouseEnter += PopoutToolbar;

                NewEmptyButton.Click += vm.NewPinClicked;
                NewPinButton.Click += vm.NewPinClicked;
                NewDefinitionButton.Click += vm.NewPinClicked;
                NewListButton.Click += vm.NewPinClicked;

                NewContentButton.Click += vm.NewComponentClicked;
                NewTitleButton.Click += vm.NewComponentClicked;
                NewBulletButton.Click += vm.NewComponentClicked;

                SaveBoardButton.Click += vm.SaveBoard;
            }
        }
        /// <summary>
        /// Called on changes to the boards view such as new pins, components, or size changes
        /// </summary>
        void PinsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (BasePin newPin in e.NewItems)
                    PinAdded(newPin);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BasePin existingPin in e.OldItems)
                    PinRemoved(existingPin);
            }
        }
        void PinAdded(BasePin pin)
        {
            PinGrid.Children.Add(pin.Top());
        }
        void PinRemoved(BasePin pin)
        {
            PinGrid.Children.Remove(pin.Top());
        }
        void SwitchCursor()
        {
            PinGrid.Cursor = boardVM.debugging ? Cursors.Help : Cursors.Arrow;
        }
        private void SetFilter(object sender, RoutedEventArgs e)
        {
            boardVM.FilterBoard(FilterEntry.Text);
        }
        public void OnWindowResize(object sender, SizeChangedEventArgs e)
        {
            boardVM.windowDimensions[0] = e.NewSize.Width;
            boardVM.windowDimensions[1] = e.NewSize.Height;

            boardVM.RecalculateAllPositions();
        }
        public void PopoutToolbar(object sender, RoutedEventArgs e)
        {
            Grid popout = null;
            if (((Button)sender).Tag.ToString() == "empty")
                popout = PresetPopoutMenu;
            else popout = ComponentPopoutMenu;

            popout.Visibility = Visibility.Visible;
            popout.MouseLeave += ClosePopout;
        }
        public void ClosePopout(object sender, RoutedEventArgs e)
        {
            ((Grid)sender).Visibility = Visibility.Collapsed;
            ((Grid)sender).MouseLeave -= ClosePopout;
        }
    }
}
