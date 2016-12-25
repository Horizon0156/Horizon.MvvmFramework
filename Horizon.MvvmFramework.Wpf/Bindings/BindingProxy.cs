using System.Windows;

namespace Horizon.MvvmFramework.Wpf.Bindings
{
    /// <summary>
    /// Represents a binding proxy used to access the DataContext of an object which
    /// is not in the current visual tree, e.g. within the contextment, or a tooltip's pop-up.
    /// </summary>
    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data",
            typeof(object),
            typeof(BindingProxy),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the bound Data of the <see cref="BindingProxy"/>
        /// </summary>
        public object Data
        {
            get
            {
                return GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        /// <inheritdoc />>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}