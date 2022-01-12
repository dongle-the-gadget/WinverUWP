using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinverUWP.Helpers;

namespace WinverUWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isEditing;

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                Set(ref _isEditing, value);
                OnPropertyChanged(nameof(IsNotEditing));
            }
        }

        public bool IsNotEditing => !_isEditing;

        public ICommand ToggleEditing => new RelayCommand(() =>
        {
            IsEditing = !IsEditing;
        });
    }
}
