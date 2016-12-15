using System.Windows;
using System.Windows.Interactivity;

namespace Horizon.MvvmFramework.Wpf.Behaviors
{
    /// <summary>
    /// Behavior which sets the focus initially to the associated objects, after it has been loaded.
    /// </summary>
    public class InitialFocusBehavior : Behavior<FrameworkElement>
    {
        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += EnsureInitialFocusIsSetProperly;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= EnsureInitialFocusIsSetProperly;

            base.OnDetaching();
        }

        private void EnsureInitialFocusIsSetProperly(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Focus();
            AssociatedObject.Loaded -= EnsureInitialFocusIsSetProperly;
        }
    }
}