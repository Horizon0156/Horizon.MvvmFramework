using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.MvvmFramework.Exceptions;

namespace Horizon.MvvmFramework.Commands
{
    /// <summary>
    /// Represents a command factory.
    /// </summary>
    public class CommandFactory : ICommandFactory
    {
        /// <inheritdoc/>
        public INotifiableCommand CreateCommand(Action execute, Func<bool> canExecute)
        {
            Throw.IfArgumentIsNull(execute, nameof(execute));

            return new Command(execute, canExecute);
        }

        /// <inheritdoc/>
        public INotifiableCommand CreateCommand<T>(Action<T> execute, Func<T, bool> canExecute)
        {
            Throw.IfArgumentIsNull(execute, nameof(execute));

            return new Command<T>(execute, canExecute);
        }

        /// <inheritdoc/>
        public INotifiableCommand CreateAyncCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            Throw.IfArgumentIsNull(executeAsync, nameof(executeAsync));

            return new AsyncCommand(executeAsync, canExecute);
        }

        /// <inheritdoc/>
        public INotifiableCommand CreateAyncCommand<T>(Func<T, Task> executeAsync, Func<T, bool> canExecute = null)
        {
            Throw.IfArgumentIsNull(executeAsync, nameof(executeAsync));

            return new AsyncCommand<T>(executeAsync, canExecute);
        }
    }
}