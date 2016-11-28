using Horizon.MvvmFramework.Exceptions;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace Horizon.MvvmFramework.Commands
{
    internal class AsyncCommand<T> : CommandBase
    {
        [CanBeNull]
        private readonly Func<T, bool> _canExecute;

        [NotNull]
        private readonly Func<T, Task> _executeAsync;

        public AsyncCommand([NotNull] Func<T, Task> executeAsync, [CanBeNull] Func<T, bool> canExecute = null)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || IsParameterValid(parameter) && _canExecute.Invoke((T)parameter);
        }

        public override async void Execute(object parameter)
        {
            Throw.IfOperationIsInvalid(isOperationInvalid: !CanExecute(parameter), message: "The command can not execute");
            Throw.IfOperationIsInvalid(isOperationInvalid: !IsParameterValid(parameter), message: $"The parameter is not of the registered type: {typeof(T).Name}.");

            await _executeAsync
                .Invoke((T)parameter)
                .ConfigureAwait(false);
        }

        private bool IsParameterValid(object parameter)
        {
            return parameter is T;
        }
    }
}