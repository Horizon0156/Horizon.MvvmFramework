using System.Windows;
using System.Windows.Interactivity;
using JetBrains.Annotations;

namespace Horizon.MvvmFramework.Wpf.Behaviors
{
    /// <summary>
    /// Behavior which sets the focus initially to the associated objects, after it has been loaded.
    /// </summary>
    public class InitialFocusBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Dependency property for the <see cref="FallbackElement"/>
        /// </summary>
        public static DependencyProperty FallbackElementProperty = DependencyProperty.Register(
            "FallbackElement",
            typeof(FrameworkElement),
            typeof(InitialFocusBehavior),
            new FrameworkPropertyMetadata(defaultValue: null));

        /// <summary>
        /// Gets or sets a fallback element which receives the Focus if the attached element can not 
        /// receive the initial focus.
        /// </summary>
        [CanBeNull]
        public FrameworkElement FallbackElement
        {
            get
            {
                return (FrameworkElement) GetValue(FallbackElementProperty);
            }
            set
            {
                SetValue(FallbackElementProperty, value);
            }
        }

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
            if (CanAssociatedObjectBeFocused())
            {
                AssociatedObject.Focus();
            }
            else
            {
                FallbackElement?.Focus();
            }
            AssociatedObject.Loaded -= EnsureInitialFocusIsSetProperly;
        }

        private bool CanAssociatedObjectBeFocused()
        {
            return AssociatedObject.Focusable && AssociatedObject.IsEnabled && AssociatedObject.IsVisible;
        }
    }
}