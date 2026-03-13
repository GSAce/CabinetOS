using System;
using System.ComponentModel;
using System.Windows.Input;
using CabinetOS.UI;

namespace CabinetOS.UI.Controls
{
    public class RemapOverlayViewModel : INotifyPropertyChanged
    {
        private string _currentInputName;

        public string CurrentInputName
        {
            get => _currentInputName;
            set
            {
                if (_currentInputName != value)
                {
                    _currentInputName = value;
                    OnPropertyChanged(nameof(CurrentInputName));
                }
            }
        }

        public ICommand CancelRemapCommand { get; }

        public event Action OnCancel;

        public RemapOverlayViewModel()
        {
            CancelRemapCommand = new RelayCommand(ExecuteCancel);
        }

        private void ExecuteCancel()
        {
            OnCancel?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}