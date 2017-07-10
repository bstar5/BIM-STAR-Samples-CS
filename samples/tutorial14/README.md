# View层与ViewModel层之间消息通信

`View`层与`ViewModel`层可以相互发消息，一端发另一端接收，不过这两端必须是对应的，下面给出详细介绍。

## `string`消息的发送与接收

`View`层消息发送，`View.xaml.cs`的代码如下所示。下面是一个按钮的点击事件。`Message`对象表示的是消息对象，第一个参数是消息的名称，第二个参数是消息的内容，是`string`类型的内容。

```c#
 private void ButtonThree_OnClick(object sender, RoutedEventArgs e)
{
      //消息发送
      Messager.Send(this, new Message<string>("ViewToViewModel", "你好！"));
}
```

`ViewModel`层接收消息，代码如下所示。首先`ViewModel`类要继承接口`IHandle<string>`实现`Handle`方法，表示接收消息内容类型为`string`的消息。`message.Name`为需要接受消息的名称，`message.Value`为消息的内容。

```c#
public class FirstViewModel : ViewModelBase,IHandle<string>
{
      public void Handle(Message<string> message)
	  {
   		if (message.Name == "ViewToViewModel")//需要接受消息的名称
   		{
       //进行一些操作
       this.ShowMessage($"ViewModel：刚刚收到View发过来的string类型的通知！\r\n消息内容为:{message.Value}");
  		}
	  }
}
```

注意：上面仅写了`View`发送消息给`ViewModel`，如果是`ViewModel`发送消息给`View`层，由于`View.xaml.cs`里的类已经继承了`IHandle<string>` ，因此需要`override`去实现`Handle`方法。具体代码请看示例源码。

## `Object`消息的发送与接收

有时发送的消息可能是一个`Object`对象，所以，`string`消息不适用了。此时，需要`ViewModel`类继承接口`IHandle<Object>`，实现`Handle`方法。

`View`层消息发送，代码如下所示。

```c#
var user = new User { Number = "5", Name = "小灰", Remark = "View发送给ViewModel" };
Messager.Send(this, new Message<User>("ViewToViewModel", user));
```

`ViewModel`层接收消息，代码如下所示。`message.Name`为需要接受消息的名称，`message.Value`为消息的内容，即`User`实体。

```c#
public class FirstViewModel : ViewModelBase,IHandle<User>
{
        public void Handle(Message<User> message)
        {
            if (message.Name == "ViewToViewModel")
            {
                //进行一些操作
                UserSource.Add(message.Value);
                this.ShowMessage($"ViewModel：刚刚收到View发过来的User类型的通知！\r\n消息内容为:{message.Value.Name}，并添加到了GridView里");
            }
        }
}
```

> 其中`UserSource`为上面例子中`RadGridView`的数据源。

注意：上面仅写了`View`发送消息给`ViewModel`，如果是`ViewModel`发送消息给`View`层，`View.xaml.cs`里的类应先继承`IHandle<Object>` ，然后再去去实现`Handle`方法。具体代码请看示例源码。

## 框架的新特性` mango:Mvvm.Attach` 

对于界面控件的事件，可以直接使用`mango:Mvvm.Attach`特性，快捷进入`ViewModel`层对应的方法里。比如，当鼠标进入按钮的边界时候，发生的事件`MouseEnter`，我想在`ViewModel`层里进行处理这个事件，当然了也可以使用上面所说的发送消息通信的方法，不过下面这个方法更快捷。

`View`层代码如下所示。`mango:Mvvm.Attach="事件名1=ViewModel中的方法名1();事件名2=ViewModel中的方法名2()"` 里面的事件用分号（;）隔开。

```html
<Button mango:Mvvm.Attach="MouseEnter=MouseEnter();MouseLeave=MouseLeave()" />
```

`ViewModel`层代码如下所示。这样只要鼠标进入按钮边界就会触发`MouseEnter`事件。

```C#
public void MouseEnter()
{
     MouseToButtonText = "鼠标移进了按钮";
}

public void MouseLeave()
{
     MouseToButtonText = "鼠标离开了按钮";
}
```

有的时候事件需要传递一些参数，比如`mango:Mvvm.Attach="事件名1=Method1($this);事件名2=Method2($dataContext);事件名3=Method3($this.IsChecked)"`等等。那么接受方法的参数也就需要对应的类型才能进入方法。

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。