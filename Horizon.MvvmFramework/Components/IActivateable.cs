using System.Threading.Tasks;

namespace Horizon.MvvmFramework.Components
{
    /// <summary>
    /// Interface for an activateable component.
    /// </summary>
    public interface IActivateable
    {
        /// <summary>
        /// Activates the component asynchronously.
        /// </summary>
        /// <param name="isInitialActivation"> Flag indicating wheather this activation is an initial activation or a reactivation. </param>
        /// <returns> The operational Task. </returns>
        Task ActivateAsync(bool isInitialActivation);
    }
}