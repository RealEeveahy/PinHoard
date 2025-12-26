using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PinHoard.view.notetaking
{
    /// <summary>
    /// Interaction logic for ColourPickerView.xaml
    /// </summary>
    public partial class ColourPickerView : UserControl
    {
        public ColourPickerView()
        {
            InitializeComponent();
        }

        // Target DP with change callback so the internal Popup always gets the latest element
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(UIElement), typeof(ColourPickerView),
                new PropertyMetadata(null, OnTargetChanged));

        public UIElement? Target
        {
            get => (UIElement?)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColourPickerView cpv)
            {
                cpv.ColourPickerPopout.PlacementTarget = e.NewValue as UIElement;
            }
        }

        // Expose IsOpen state so ancestor/VM can toggle popup
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(ColourPickState), typeof(bool), typeof(ColourPickerView),
                new PropertyMetadata(false, OnStateChanged));

        public bool ColourPickState
        {
            get => (bool)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColourPickerView cpv)
            {
                cpv.ColourPickerPopout.IsOpen = (bool)e.NewValue;
            }
        }
    }
}
