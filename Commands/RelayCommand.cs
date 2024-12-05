using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HobbyListWPF.Commands
{
    // Generic class that maps out commands to be executed (or not) from button clicks
    internal class RelayCommand : ICommand
    {
        // Delegate _execute takes an (method as an) object as a parameter
        private readonly Action<object?> _execute;
        // Delegate _canExecute takes an object and returns a bool
        private readonly Func<object?, bool>? _canExecute;
        // Event to be raised depending on the state of the app
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Constructor for this class takes execute as an argument and caExecute as an optional argument
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // 
        public bool CanExecute(object? parameter) => _canExecute is null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);
    }
}