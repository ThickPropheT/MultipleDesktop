using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Input.Extended
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _execute;

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public DelegateCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
            => _execute();
    }
}
