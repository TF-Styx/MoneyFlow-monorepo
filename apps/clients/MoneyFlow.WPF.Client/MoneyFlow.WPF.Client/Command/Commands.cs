using System.Windows.Input;

namespace MoneyFlow.WPF.Client.Command
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            T typedParameter;
            if (parameter == null)
            {
                if (typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
                {
                    return false;
                }
                typedParameter = default(T);
            }
            else if (parameter is not T)
            {
                try
                {
                    typedParameter = (T)Convert.ChangeType(parameter, typeof(T));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                typedParameter = (T)parameter;
            }

            return _canExecute(typedParameter);
        }

        public void Execute(object parameter)
        {
            T typedParameter;
            if (parameter == null)
            {
                if (typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
                {
                    if (typeof(T) == typeof(object))
                    {
                        typedParameter = (T)parameter;
                    }
                    else
                    {
                        typedParameter = (T)parameter;
                    }
                }
                else
                {
                    typedParameter = default(T);
                }
            }
            else if (!(parameter is T))
            {
                try
                {
                    typedParameter = (T)Convert.ChangeType(parameter, typeof(T));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException
                        (
                            $"Parameter is of type {parameter.GetType().Name} " +
                            $"but could not be converted to expected type {typeof(T).Name}.",
                            nameof(parameter),
                            ex
                        );
                }
            }
            else
            {
                typedParameter = (T)parameter;
            }

            _execute(typedParameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base(param => execute(), null)              
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute) : base(param => execute(), param => canExecute())
        {
        }
    }

}
