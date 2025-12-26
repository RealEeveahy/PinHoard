using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PinHoard.view.style
{
    public partial class ImageButton : UserControl
    {
        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(ImageButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        public ImageButton()
        {
            InitializeComponent();


            InnerButton.MouseEnter += (s, e) =>
            {
                // Raise MouseEnter on the UserControl itself
                this.RaiseEvent(new MouseEventArgs(e.MouseDevice, e.Timestamp) { RoutedEvent = MouseEnterEvent });

                if (MouseEnterCommand?.CanExecute(MouseEnterCommandParameter) == true)
                    MouseEnterCommand.Execute(MouseEnterCommandParameter);
            };

            InnerButton.MouseLeave += (s, e) =>
            {
                if (MouseLeaveCommand?.CanExecute(MouseLeaveCommandParameter) == true)
                    MouseLeaveCommand.Execute(MouseLeaveCommandParameter);
            };
            InnerButton.Click += (s, e) =>
            {
                RaiseEvent(new RoutedEventArgs(ClickEvent));
            };
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ImageButton));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        // NEW: MouseEnterCommand
        public static readonly DependencyProperty MouseEnterCommandProperty =
            DependencyProperty.Register("MouseEnterCommand", typeof(ICommand), typeof(ImageButton));

        public ICommand MouseEnterCommand
        {
            get => (ICommand)GetValue(MouseEnterCommandProperty);
            set => SetValue(MouseEnterCommandProperty, value);
        }

        public static readonly DependencyProperty MouseEnterCommandParameterProperty =
            DependencyProperty.Register("MouseEnterCommandParameter", typeof(object), typeof(ImageButton));

        public object MouseEnterCommandParameter
        {
            get => GetValue(MouseEnterCommandParameterProperty);
            set => SetValue(MouseEnterCommandParameterProperty, value);
        }

        // NEW: MouseLeaveCommand
        public static readonly DependencyProperty MouseLeaveCommandProperty =
            DependencyProperty.Register("MouseLeaveCommand", typeof(ICommand), typeof(ImageButton));

        public ICommand MouseLeaveCommand
        {
            get => (ICommand)GetValue(MouseLeaveCommandProperty);
            set => SetValue(MouseLeaveCommandProperty, value);
        }

        public static readonly DependencyProperty MouseLeaveCommandParameterProperty =
            DependencyProperty.Register("MouseLeaveCommandParameter", typeof(object), typeof(ImageButton));

        public object MouseLeaveCommandParameter
        {
            get => GetValue(MouseLeaveCommandParameterProperty);
            set => SetValue(MouseLeaveCommandParameterProperty, value);
        }

        // Existing dependency properties
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ImageButton));

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty TagValueProperty =
            DependencyProperty.Register("TagValue", typeof(object), typeof(ImageButton));

        public object TagValue
        {
            get => GetValue(TagValueProperty);
            set => SetValue(TagValueProperty, value);
        }
    }
}
