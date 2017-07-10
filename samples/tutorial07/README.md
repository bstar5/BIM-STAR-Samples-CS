# 命令的使用

- 命令主要分为两种，内部命令和全局命令。


- 内部命令：为`ViewModel`内的命令，只在对应的`View`上绑定有效。
- 全局命令：在应用的任何地方都可以定义，在其他应用中也可以获取该命令，主要用于提供命令给其他应用或其他不能通过内部命令完成的地方使用。

## 命令的定义及使用

所有的命令都继承自`ICommand`；其具体的实现主要有两种`RelayCommand`，`AsyncCommand`：

- `RelayCommand`：

  ```C#
  ICommand cmd = new RelayCommand(OnAction, CanAction);
  //ICommand cmd = new RelayCommand(OnAction);
  private void OnAction(){}
  private bool CanAction(){
    return true;
  }
  ```

  **参数说明**：`OnAction` 为触发该命令后执行的方法，该方法可以为无参数方法或者为包含一个`object`参数的方法 ；`CanAction`为判断该命令是否可用的方法，该方法的返回值为`bool`类型；两个方法都不能是返回Task的方法。

  由于`CanAction`有返回值，所以不能是异步方法。

- `AsyncCommand`: 该命令是对`RelayCommand`的一个封装，使得`CanAction`能够返回`Task<bool>`类型，其包含`RelayCommand`的属性，名为`Command`，通过`Command`就可以使用其委托的方法；

  ```C#
  AsyncCommand cmd = new AsyncCommand(OnAction, CanActionAsync);
  private void OnAction(){}
  private async Task<bool> CanActionAsync(){
    return true;
  }
  ```

**提示**：有时按钮的点击命令需要先判断该按钮是否可以点击，该判断方法里可能需要通过访问API才能知道该按钮是否可点击，因此需要花费一定的时间，这个时候就需要用到异步命令`AsyncCommand` 来实现。比如在获取权限的时候需要使用异步的方法去判断此时该命令的是否可用。

## 注册/获取全局命令

​	系统通过`Mg.Get<IMgCommand>()` 来管理全局的命令，位于`Mango`中，通过`Register`方法可以注册命令，注册全局命令的应用卸载时，全局命令也会相应的被卸载。全局命令一般在应用的启动方法`OnStartupAsync`中进行注册。

- 注册命令

  ```c#
  Mg.Get<IMgCommand>().Register(this, new CommandInfo("CommandName", new RelayCommand(Methor)));
  ```

  **参数说明**：`Register`的第一个参数是`应用App`对象，第二个参数需要提供`CommandInfo`类型的对象； `CommandInfo`的构造函数中，`CommandName`是命令的唯一标识名，`Methor`是执行方法的方法名；

  **注意**：异步全局命令如下所示。`CanMethor`是一个异步的返回`Task<bool>`类型的方法。

  ```C#
  Mg.Get<IMgCommand>().Register(this, new CommandInfo("CommandName", new AsyncCommand(Methor, CanMethor)));
  ```
  **参数说明**：`CommandName`为全局命令的唯一标识名。

- 获取全局命令

  `GlobalCommand`和`GlobalCommandAsync`绑定的是对应的`View`界面的按钮的点击事件。

```C#
public AsyncCommand GlobalCommandAsync { get; private set; }
public RelayCommand GlobalCommand { get; private set; }

public FirstViewModel()
{
    var cmd = Mg.Get<IMgCommand>();
    GlobalCommand = cmd.GetCommandInfo("AlertMessage").Command as RelayCommand;
    GlobalCommandAsync = cmd.GetCommandInfo("AlertMessageAsync").Command as AsyncCommand;
}
```

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。