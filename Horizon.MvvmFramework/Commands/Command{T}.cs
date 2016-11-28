using Horizon.MvvmFramework.Exceptions;
using JetBrains.Annotations;
using System;

namespace Horizon.MvvmFramework.Commands
{
    internal class Command<T> : CommandBase
    {
        [CanBeNull]
        private readonly Func<T, bool> _canExecute;

        [NotNull]
        private readonly Action<T> _execute;

        public Command([NotNull] Action<T> execute, [CanBeNull] Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || IsParameterValid(parameter) && _canExecute.Invoke((T)parameter);
        }

        public override void Execute(object parameter)
        {
            Throw.IfOperationIsInvalid(isOperationInvalid: !CanExecute(parameter), message: "The command can not execute");
            Throw.IfOperationIsInvalid(isOperationInvalid: !IsParameterValid(parameter), message: $"The parameter is not of the registered type: {typeof(T).Name}.");

            _execute.Invoke((T)parameter);
        }

        private bool IsParameterValid(object parameter)
        {
            return parameter is T;
        }
    }
}