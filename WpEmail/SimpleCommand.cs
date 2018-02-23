namespace WpfEmail
{
    using System;
    using System.Windows.Input;

    class SimpleCommand : ICommand
    {
        private Action<object> _execute;

        private Predicate<object> _canExecute;

        private event EventHandler _canExecuteChangedInternal;

        public SimpleCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public SimpleCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                _canExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                _canExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) == true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = _canExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            _canExecute = _ => false;
            _execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
