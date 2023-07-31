using System;
using System.Windows.Input;

namespace TFT_TEAM_BUILDER.Core
{
    public class Commands : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Commands(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            execute = Execute;
            canExecute = CanExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}