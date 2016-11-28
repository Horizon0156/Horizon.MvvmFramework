using System.Windows.Input;

namespace Horizon.MvvmFramework.Commands
{
    /// <summary>
    /// Interface for a <see cref="ICommand"/> which triggers change notifications on demand.
    /// </summary>
    public interface INotifiableCommand : ICommand
    {
        /// <summary>
        /// Notifies that the execution restriction of this <see cref="ICommand"/> might have changed.
        /// </summary>
        void NotifyChange();
    }
}