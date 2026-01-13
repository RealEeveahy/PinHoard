using PinHoard.viewmodel.menus;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PinHoard.view.menus
{
    /// <summary>
    /// Interaction logic for FileWidget.xaml
    /// </summary>
    public partial class FileWidget : UserControl
    {
        public static readonly RoutedEvent OpenRequestedEvent = 
            EventManager.RegisterRoutedEvent(nameof(OpenRequested), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileWidget));
        public event RoutedEventHandler OpenRequested
        {
            add => AddHandler(OpenRequestedEvent, value);
            remove => RemoveHandler(OpenRequestedEvent, value);
        }
        public static readonly RoutedEvent SettingsRequestedEvent =
            EventManager.RegisterRoutedEvent(nameof(SettingsRequested), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileWidget));
        public event RoutedEventHandler SettingsRequested
        {
            add => AddHandler(OpenRequestedEvent, value);
            remove => RemoveHandler(OpenRequestedEvent, value);
        }
        public FileWidget(FileWidget_ViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
        void RaiseSettingsRequested() => RaiseEvent(new RoutedEventArgs(SettingsRequestedEvent, this));
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseSettingsRequested();
        }
        void RaiseOpenRequested() => RaiseEvent(new RoutedEventArgs(OpenRequestedEvent, this));
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseOpenRequested();
        }
    }
}
