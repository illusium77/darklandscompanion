using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsUiCommon.Commands
{
    public class UiCommand : BaseCommand
    {
        private Action m_execute;
        private Func<bool> m_canExecute;

        public UiCommand(Action execute, Func<bool> canExecute = null)
        {
            m_execute = execute;
            m_canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (m_canExecute != null)
            {
                return m_canExecute();
            }

            return true;
        }

        public override void Execute(object parameter)
        {
            if (m_execute != null)
            {
                m_execute();
            }
        }
    }
}
