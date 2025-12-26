using PinHoard.model.pins;
using PinHoard.viewmodel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PinHoard.view.notetaking
{
    /// <summary>
    /// Interaction logic for Pin_View.xaml
    /// </summary>
    public partial class Pin_View : UserControl
    {
        public Pin_View()
        {
            InitializeComponent();
        }
        public Pin_View(Pin_ViewModel vm) : this()
        {
            DataContext = vm;
            if (vm?.model?.componentList is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += Components_CollectionChanged;
            }

            Build(vm?.model?.componentList);
        }
        private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataContext is Pin_ViewModel vm)
                Build(vm.model.componentList);
        }
        private void Build(IEnumerable<ComponentBase>? components)
        {
            MainStack.Children.Clear();
            if (components == null) return;

            foreach (ComponentBase component in components)
            {
                component.SetParent(MainStack);
            }
        }
        private void Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is Pin_ViewModel vm)
            {
                vm.Focus(null);
            }
        }
        private void Bind()
        {
            BindingOperations.SetBinding(this, FrameworkElement.HeightProperty, new Binding("height") { Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(this, UIElement.VisibilityProperty, new Binding("NoteVisibility") { Mode = BindingMode.OneWay });
        }
    }
}
