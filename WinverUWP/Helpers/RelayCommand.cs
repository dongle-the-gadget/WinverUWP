using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinverUWP.Helpers
{
    public class RelayCommand : ICommand
    {
        private Action _action;

        public RelayCommand(Action action) => _action = action;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
            => _action.Invoke();
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> _action;

        public RelayCommand(Action<T> action) => _action = action;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => parameter is T;

        public void Execute(object parameter)
            => _action.Invoke((T)parameter);
    }
}
