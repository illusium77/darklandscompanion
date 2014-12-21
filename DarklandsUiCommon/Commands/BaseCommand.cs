using System;
using System.Windows.Input;

namespace DarklandsUiCommon.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Refresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
    }
}
