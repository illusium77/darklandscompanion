using System;

namespace DarklandsUiCommon.Commands
{
    public class UiCommand : BaseCommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public UiCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }

            return true;
        }

        public override void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute();
            }
        }
    }
}