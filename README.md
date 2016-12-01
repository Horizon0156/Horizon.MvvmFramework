![logo](https://raw.githubusercontent.com/Horizon0156/Horizon.MvvmFramework/master/Icon.png)
# Horizon.MvvmFramework
The MvvmFramework is a simple and lightweight framework supposed to speed-up the development of MVVM (Model-View-ViewModel) applications as well as to reduce code of common operations such as change notifications.

The main component is build as a portable class library so it can be used "view" independently. The Wpf submodule of this framework contains WPF specific behaviors and extensions. This framework was extracted from a working productive application. It will be extendend if a code snippet is worth to be added and might be helpful for common use cases. This project is a private project written during leasure time, published in hopes that it will be useful for other projects too. Last but not least, feel free to extend the framework :)

## Installation
Simply use this framework by referencing one of the following NuGet packages.
* https://www.nuget.org/packages/Horizon.MvvmFramework/
* https://www.nuget.org/packages/Horizon.MvvmFramework.Wpf/

## Documentation
This sections provides a short overview of the framework components and how to use them. 
### Components
#### `ObserveableObject` : `INotifyPropertyChange`
An observeable object is an object which provides change notifications for its properties. There's also a helper Method `SetPropety` which allows a simple way for setting a property and notify a change if the value has changed in a single call.
```c#
public class TestObject : ObserveableObject
{
    private string _stringProperty;
    public string StringProperty	
    {
        get
        {
            return _stringProperty;
        }
        set
        {
            SetProperty(ref _stringProperty, Value);
        }
    }
}
```

#### `ViewModel` : `ObserveableObject`
The view model is an observeable object which provides an additional `ClosureRequested`Event used to handle a closure request from the models domain logic. The view might subscribe to this event and handle the closure independently. A call to `OnClosureRequested` will then inform the View properly.

```c#
public class TestModel : ViewModel
{
    public void SaveAndCloseDialog()	
    {
    	
        SaveChanges();
        OnClosureRequested();
    }
}
```

####  `IActivateable`
The `IActivateable` interface is used to identify activateable components. An activateable component implements 
```c#
/// <summary>
/// Activates the component asynchronously.
/// </summary>
/// <param name="isInitialActivation"> Flag indicating wheather this activation is an initial activation or a reactivation. </param>
/// <returns> The operational Task. </returns>
Task ActivateAsync(bool isInitialActivation);
```
which might be called from the View after it was activated. The WPF module of this framework provides a behavior which automatically connects the `ClosureEvent` to the DataContext and triggers the activation if the window gets activated.  
### Commands
#### `ICommandFactory`
The `ICommandFactory` allows the creation of various `ICommand`, respectively `INotifiableCommand` implementations. There are methods for creating synchronous command operations as well as asnychronous command operations.
```c#
/// <summary>
/// Creates an asynchronous command which executes the given action.
/// </summary>
/// <param name="executeAsync"> Asynchronous action which will be executed by this command. </param>
/// <param name="canExecute"> Delegate which determines wheather the command can be executed. </param>
/// <returns> The command. </returns>
/// <exception cref="ArgumentNullException">If the execution action is not set. </exception>
INotifiableCommand CreateAyncCommand([NotNull] Func<Task> executeAsync, [CanBeNull] Func<bool> canExecute = null);

/// <summary>
/// Creates an asynchronous command which executes the given action.
/// </summary>
/// <param name="executeAsync"> Asynchronous action which will be executed by this command. </param>
/// <param name="canExecute"> Delegate which determines wheather the command can be executed. </param>
/// <typeparam name="T"> The type of the parameter passsed to the command. </typeparam>
/// <returns> The command. </returns>
/// <exception cref="ArgumentNullException">If the execution action is not set. </exception>
INotifiableCommand CreateAyncCommand<T>([NotNull] Func<T, Task> executeAsync, [CanBeNull] Func<T, bool> canExecute = null);
```
#### `INotifiableCommand`:`ICommand`
The `INotifiableCommand` command is an `ICommand` which triggers change notifications on demand. The developer has to decide when a command might get available
and should call `NotifyChange()` to re-evaluate the execution restriciton. This design prevents unnecessary command updates like  WPFs `CommandManager` would trigger.
#### `CommandFactory`:`ICommandFactory`
The command factory implements an `ICommandFactory` and handles the initialization of the hidden command implementations itself. Using the factory a command can be created by defining the corresponding execution action and delegate used to determine the enabled state of the command.
```c#
ICommandFactory commandFactory = new CommandFactory();
SaveCommand = commandFactory.CreateCommand(SaveChanges, CanSaveChanges);

private void SaveChanges()
{
	// Any save logic
	File.WriteAllBytes(_path, _blob);
}

private bool CanSaveChanges()
{
	// Any save restriciton
	return dataModel.HasChanges();
}
``` 
### Collections
####`AttentiveCollection<T>` : `ObservableCollection<T>`
The attentive collection extends an `ObservableCollection<T>` and provides notifications for changes of inner elements too. That means if any property of object ``T`` changes its value, an `InnerElementChanged` event will be fired, assumed that the property fires a change notification itself.
```c#
public class Product : ObserveableObject
{
	private decimal _price;
	public decimal Price
	{
		get
		{
			return _price;
		}
		set
		{
			SetProperty(ref _price, Value);
		}
	}
}

public class ProductManagement : ViewModel
{
	public AttentiveCollection<Product> Products;

	public ProductManagement()
	{
		Products = new AttentiveCollection<Product>(...);
		Products.InnerElementChanged += (s, e) => (Console.WriteLine($"Product {s} has changed its price"));
	}
}

``` 
### Exceptions
The Exceptions namespace provides a helper called `Throw`. Using the `Throw` class, common parameter or argument validations can be inserted in your code without using a couple of `if` statements at the beginning of your method. Within the current release, the following Validations are included:
```c#
/// <summary>
/// Throws an exception if the provided argument is null.
/// </summary>
/// <param name="argument"> The argument which will be checked. </param>
/// <param name="argumentName"> The name of the argument, which will be checked. </param>
/// <exception cref="ArgumentNullException">If the provided argument is null. </exception>
public static void IfArgumentIsNull(object argument, [CanBeNull] string argumentName)`;

/// <summary>
/// Throws an exception if an argument is not valid.
/// </summary>
/// <param name="isArgumentValid"> Flag wheather the argument is valid. </param>
/// <param name="message"> The exception message, why the argument is not valid. </param>
/// <param name="argumentName"> The name of the argument, which was checked. </param>
/// <exception cref="ArgumentNullException">If the provided message is null. </exception>
/// /// <exception cref="ArgumentException">If the argument is not valid. </exception>
public static void IfArgumentIsNotValid(bool isArgumentValid, [NotNull] string message, [CanBeNull] string argumentName);

/// <summary>
/// Throws an exception if an operation is invalid.
/// </summary>
/// <param name="isOperationInvalid"> Flag wheather the operation is invalid. </param>
/// <param name="message"> The message, used to point out the invalid operation. </param>
/// <exception cref="ArgumentNullException">If the provided message is null. </exception>
/// /// <exception cref="InvalidOperationException">If the operation is not valid. </exception>
public static void IfOperationIsInvalid(bool isOperationInvalid, [NotNull] string message);
```

Example usage:

```c#
public void DoSomething(object importantParameter)
{
	Throw.IfArgumentIsNull(importantParameter, nameof(importantParameter));
}

public void DoSomethingWithPositiveNumbers(int number)
{
	Throw.IfArgumentIsNotValid(number < 0, "Number has to be positive", nameof(number));
}
```

### Extensions
Currently the Extensions namespace provides a Task extension used to execute an action if the calling task faults. This extension is very useful if you want to prevent an application to fault up in an unkno
wn state. There might be a point in your application when you have to use 'async void', because you can not return an operational Task. If your asynchronous method does not handle all possible exceptions, the 
`OnUnobservedException` task extension catches any unhandled exception and continues with the given action. It improves the readability of your code, since you don't have to write your `try`/`catch` blocks over and over again.

```c#
public void LoadData()
{
	LoadDataAsync().OnUnobservedException(LogExceptionAndTerminateApplication);
}
```

### Services
TBD

### WPF Behaviors
TBD

### WPF Extensions
TBD

## Example: Hello World Application
TBD
