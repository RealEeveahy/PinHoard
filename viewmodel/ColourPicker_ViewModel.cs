using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PinHoard.viewmodel
{
    public class ColourPicker_ViewModel : INotifyPropertyChanged
    {
        private readonly Board_ViewModel _board;
        public ColourPicker_ViewModel(Board_ViewModel board)
        {
            _board = board;
            _board.PropertyChanged += BoardUpdate;
        }
        private void BoardUpdate(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Board_ViewModel.FocusedColour) || e.PropertyName=="focusedPin")
            {
                OnPropertyChanged(nameof(R_Slider));
                OnPropertyChanged(nameof(G_Slider));
                OnPropertyChanged(nameof(B_Slider));
            }
        }
        (int r, int g, int b) ParseColour()
        {
            var hex = _board.focusedPin?.model.bgColour;
            if (string.IsNullOrEmpty(hex) || hex.Length < 7) return (0, 0, 0);
            try
            {
                int r = Convert.ToInt32(hex.Substring(1, 2), 16);
                int g = Convert.ToInt32(hex.Substring(3, 2), 16);
                int b = Convert.ToInt32(hex.Substring(5, 2), 16);
                return (r, g, b);
            }
            catch
            {
                return (0, 0, 0);
            }
        }
        public double R_Slider
        {
            get => ParseColour().r;
            set
            {
                if (_board.focusedPin == null) return;
                var (_, g, b) = ParseColour();
                int r = Math.Clamp((int)value, 0, 255);
                _board.SubmitColour(r, g, b);
                OnPropertyChanged(nameof(R_Slider));
                OnPropertyChanged(nameof(G_Slider));
                OnPropertyChanged(nameof(B_Slider));
            }
        }
        public double G_Slider
        {
            get => ParseColour().g;
            set
            {
                if (_board.focusedPin == null) return;
                var (r, _, b) = ParseColour();
                int g = Math.Clamp((int)value, 0, 255);
                _board.SubmitColour(r, g, b);
                OnPropertyChanged(nameof(R_Slider));
                OnPropertyChanged(nameof(G_Slider));
                OnPropertyChanged(nameof(B_Slider));
            }
        }
        public double B_Slider
        {
            get => ParseColour().b;
            set
            {
                if (_board.focusedPin == null) return;
                var (r, g, _) = ParseColour();
                int b = Math.Clamp((int)value, 0, 255);
                _board.SubmitColour(r, g, b);
                OnPropertyChanged(nameof(R_Slider));
                OnPropertyChanged(nameof(G_Slider));
                OnPropertyChanged(nameof(B_Slider));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
