using PinHoard.model.pins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinHoard.service
{
    public class FocusService : INotifyPropertyChanged
    {
        private object _focusedPin;
        public object focusedPin
        {
            get => _focusedPin;
            set
            {
                if (_focusedPin != value)
                {
                    _focusedPin = value;
                    OnPropertyChanged(nameof(focusedPin));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
