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
DeleteCommand = commandFactory.CreateCommand(DeleteItem, CanDeleteItem);

public object SelectedItem
{
	get
	{
		return _selectedItem;
	}
	set
	{
		SetProperty(ref _selectedItem, value);
		DeleteCommand.NotifyChange();
	}

}

private void DeleteItem()
{
	Items.Remove(SelectedItem);
}

private bool CanDeleteItem()
{
	return SelectedItem != null;
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
The service namespace includes a messenger service used to establish a bi-directional communication between models, views or view-models without violating the MVVM pattern. Using MVVM straight forwards it's a challenge to fire up sub-dialogs, setting the focus, or simply showing a message box while separating the View from the ViewModel and keeping the code unit testable.

The `IMessenger` is an independent component which can be passed to any component using dependency injection. Afterwards a component can sent a message, which another component is able to handle to. Using this messenger pattern, a ViewModel may send send a message or an instance of new ViewModel which should open a sub-dialog. The View or a supervising component uses the `IMessenger` to register a handle for these specific message types and handle them properly.  

```C#
public class Message
{
	public string Message { get; set; }
}

public class DialogViewModel : ViewModel
{
	public DialogViewModel(IMessenger messenger)
	{
		_messenger = messenger;
	}
	
	public void ShowMessage()
	{
		var message = new Message { Message = "Hi from the ViewModel" };
		_messenger.Send(messange);
	}
}

public class View : Window
{
	public View(IMessenger messenger)
	{
		InitializeComponents();
		
		_messenger = messenger;
		_messenger.Register<Message>(ShowMessage);
	}
	
	private void ShowMessage(Message message)
	{
		// View specific code
		MessageBox.Show(message.Message);
		
		// Or use custom dialogs
		var messageWindow = new MessageWindow();
		messageWindow.DataContext = message;
		messageWindow.Show();
	}
}
```
It's up to you which operations will be delegated to other components while simply sending a message ;) Keep in mind that all components have to use the same instance of an `IMessenger` to establish a communication. The framework provides an implementaions of an `IMessenger` called `MessageHub`. You can use the default instance or use an IOC container to register a singleton.
```c#
// Provide the default instance
var view = new View(MessageHub.Default);
var viewModel = new DialogViewMode(MessageHub.Default);

// Using an IOC container, SimpleInjector for example
// https://simpleinjector.org/index.html
var container = new SimpleInjector.Container();
container.RegisterSingleton<IMessenger>(() => new MessageHub());
var view = _container.GetInstance<View>();
var viewMode = _container.GetInstance<DialogViewModel>();
```
### WPF extension package
The WPF NuGet packages of this framework provides a couple of extensions and behaviours especially written for Microsoft's WPF framework. Therefore, this packages isn't platform independent.

#### Behaviors
##### `ViewModelBehavior`
The ViewModel behavior can be attached to any window. An attached window will handle the ViewModel of the window as it's supposed to be used with this framework. This means, the Window triggers an IActivable component if the window gets activated. In addition the window will close automatically if the ViewModel requests a closure using the `OnClosureRequested` method.
Nevertheless, the DataContext of the Window has to be set properly. Please have a look at the *Samples* to see the setup and registration of a MVVM application using this behavior.
##### `AutoScrollBehavior`
Implements an auto scrolling for a scroll viewer. If the layout changes, the view will scroll to the last element.
##### `InputRestrictionBehavior`
Restricts the input to a textbox by validating the entered text using a regular expression. The follwing example shows a restriction applied to a textbox so the user is only able to enter a valid _hh:mm_ time representation.
```xml
<TextBox Width="45"
         Margin="5"
         VerticalContentAlignment="Center"
         Text="{Binding Time,
                        Converter={StaticResource TimeToStringConverter},
                        Mode=TwoWay,
                        UpdateSourceTrigger=Explicit}">
	<i:Interaction.Behaviors>
	    <behaviors:InputRestrictionBehavior InputExpression="^[0-2]?[0-9]?:?[0-5]?[0-9]?$" />
        </i:Interaction.Behaviors>
</TextBox>
```
##### `MinimizeToTrayBehavior`
The `MinimizeToTrayBehavior` behavior automatically sends the application to the notification area if the window, where this behavior is attached to minimizes.
#### Extensions
##### `Application.InjectResourceDictionary(...)`
Using the `InjectResourceDictionary` extension a resource dictionary can be loaded from code using a single line expression. This is very useful if you write your own application bootstrapper instead of using the empty App.xaml and the corresponding code behind file.
```c#
var app = new Application();
app.InjectResourceDictionary("MahApps.Metro", "Styles/Controls.xaml");
app.InjectResourceDictionary("MahApps.Metro", "Styles/Fonts.xaml");
app.InjectResourceDictionary("MahApps.Metro", "Styles/Colors.xaml")

// Setup windows and VMs...

app.Run();
```
