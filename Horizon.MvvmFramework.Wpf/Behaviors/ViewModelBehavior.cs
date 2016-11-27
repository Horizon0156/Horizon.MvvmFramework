using Horizon.MvvmFramework.Components;
using JetBrains.Annotations;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Horizon.MvvmFramework.Wpf.Behaviors
{
    /// <summary>
    /// Behavior which initializes a  DataContext of a window.
    /// </summary>
    public sealed class ViewModelBehavior : Behavior<Window>
    {
        private bool _dataContextActivated;

        /// <summary>
        /// Event will be called if an unhandled exception occurs during initialization or activation.
        /// </summary>
        public event EventHandler<UnhandledExceptionEventArgs> UnhandledException;

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Activated += ActivateDataContext;
            AssociatedObject.DataContextChanged += HandleDataContextExchange;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            AssociatedObject.Activated -= ActivateDataContext;

            base.OnDetaching();
        }

        private async void ActivateDataContext(object sender, EventArgs eventArgs)
        {
            var initializeableDataContext = AssociatedObject.DataContext as IActivateable;

            if (initializeableDataContext != null)
            {
                try
                {
                    await initializeableDataContext
                        .ActivateAsync(isInitialActivation: !_dataContextActivated)
                        .ConfigureAwait(true);
                    _dataContextActivated = true;
                }
                catch (Exception ex)
                {
                    OnUnhandledException(new UnhandledExceptionEventArgs(ex, isTerminating: false));
                }
            }
        }

        private void AttachClosureFunctionality(object dataContext)
        {
            var viewModel = dataContext as ViewModel;

            if (viewModel != null)
            {
                viewModel.ClosureRequested += CloseWindow;
            }
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            AssociatedObject.Close();
        }

        private void DetachClosureFunctionality(object dataContext)
        {
            var viewModel = dataContext as ViewModel;

            if (viewModel != null)
            {
                viewModel.ClosureRequested -= CloseWindow;
            }
        }

        private void HandleDataContextExchange([NotNull] object sender, DependencyPropertyChangedEventArgs e)
        {
            DetachClosureFunctionality(e.OldValue);
            AttachClosureFunctionality(e.NewValue);
            _dataContextActivated = false;
        }

        private void OnUnhandledException(UnhandledExceptionEventArgs e)
        {
            UnhandledException?.Invoke(this, e);
        }
    }
}