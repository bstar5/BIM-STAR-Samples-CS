# 服务及插槽插头

## 注册/使用服务

### 服务的注册

一般在安装应用时（`OnStartupAsync`函数中）注册服务，卸载应用时系统会自动取消该应用注册过的服务。通过`Mg.Get<IMgService>()`获取服务管理对象，然后根据服务管理对象提供的方法`Register`进行服务的注册。`Service`的第一个参数是该应用App，第二个参数是服务对象，根据服务类型的不同，传入的方法也不同。比较简单的服务如下所示。示例源码为了展示注册对比的效果，并没有在`OnStartupAsync`中进行注册，而是通过界面的一个按钮的点击进行注册。

```C#
public class DemoApp : App
{
    protected override async Task OnStartupAsync()
    {
        Mg.Get<IMgService>().Register(this,
                 //注册相关函数
                 new Service("DemoApp:Show", () =>
                 {
                     //该服务需要执行的代码
                     this.ShowMessage("您调用了DemoApp:Show服务！");
                 }),
                 new Service("DemoApp:Hide", () =>
                 {
                   	 //该服务需要执行的代码
                     this.ShowMessage("您调用了DemoApp:Hide服务！");
                 }));
        await Task.Yield();
    }
}
```

### 服务类型、参数及使用方式

1、服务类型：无参数，无返回值；一个`object`类型的参数，无返回值；无参数，返回`Result`；一个`object`类型的参数，返回`Result`；无参数，返回`Task`（异步服务）；一个`object`类型的参数，返回`Task`（异步服务）；无参数，返回`Task<Result>`（异步服务）；一个`object`类型的参数，返回`Task<Result>`（异步服务）

2、参数传递方式，当传递的参数个数比较少时使用`Tuple`进行传递，可根据实际情况使用的方式：使用`string（Json）`方式传递参数；使用匿名对象传递参数，解析参数的时候使用`param.GetPropertyValue("属性名", "默认值")`；使用字典（`Dictionary<string,object>`）的方式传递参数；使用动态对象（`dynamic）`传递参数。

3、使用服务

```c#
//调用非异步服务(传入参数)
var param = new Tuple<string,object>("string",new object());
var r  = Mg.Get<IMgService>().Invoke("DemoApp:Show",param);
if (!r.IsOk)
{
    Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "调用DemoApp:Show服务失败！");
}
//调用非异步服务(不传入参数)
var r  = Mg.Get<IMgService>().Invoke("DemoApp:Show");
//调用异步服务(不传入参数)
var r = await Mg.Get<IMgService>().InvokeAsync("DemoApp:Show");
```

**PS：**看完服务的使用，你可能会说我不清楚其它应用提供的服务需要传入哪些参数，这时你可以打开对应应用提供的说明文档。当然你创建的应用如果提供了相关服务，那就应该提供服务的说明文档，详细说明该应用提供哪些服务，参数类型等。

## 注册/使用插槽插头

### 插槽的注册

一般在安装应用时（`OnStartupAsync`函数中）注册插槽，卸载应用时会自动取消该应用注册过的服务。通过`Mg.Get<IMgSlot>()`获取槽的对象，然后通过槽对象提供的方法`Register`进行槽的注册。`Service`的第一个参数是该应用App，第二个参数是插槽对象。注册插槽代码如下所示。示例源码为了展示注册对比的效果，并没有在`OnStartupAsync`中进行注册，而是通过界面的一个按钮的点击进行注册。

```C#
public class DemoApp : App
{
    protected override async Task OnStartupAsync()
    {
        //注册插槽
        Mg.Get<IMgSlot>().Register(this,
                   new Slot("DemoApp:Startup"){ Description = "DemoApp应用启动时其它应用需要执行的操作" },
                   new Slot("DemoApp:Exited"){ Description = "DemoApp应用卸载时其它应用需要执行的操作" });
        await Task.Yield();
    }
}
```

### 插头的注册

当一个应用提供了插槽，那么其他应用的插头就可以插入到这个插槽里。`PlugAction`是插头`FirstPlug`的执行方法。

```C#
public class DemoApp : App
{
    protected override async Task OnStartupAsync()
    {
		//注册插槽DemoApp:Startup中的一个插头
		var data = new Func<IRecord, Task>(PlugAction);
		Mg.Get<IMgSlot>().PushPlugs(App.Instance<DemoApp>(), new Plug("FirstPlug", "DemoApp:Startup", data));
        await Task.Yield();
    }
}

private async Task PlugAction(IRecord data)
{
     await Task.Delay(100);
     var who = data["Name"].As<string>();
     Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"Hi,{who}！");
}
```

### 插槽和插头的使用

在遍历插槽中的插头时，可以选择插头进行执行，一定要注意插头执行方法的参数要对应得上。

```C#
//获取插槽DemoApp:Startup中所有插头
var plugs = Mg.Get<IMgSlot>().GetPlugs("DemoApp:Startup");
foreach (var plug in plugs)
{
    if (plug.Name != "FirstPlug")
        continue;
    var func = plug.Data as Func<IRecord, Task>;
    if (func == null)
    {
        Mg.Get<IMgLog>().Warn($"插槽'{plug.SlotName}'中定义的插头'{plug.Name}'没有有效的元数据！");
        continue;
    }
    var record = new DynamicRecord { { "Name", "小明" } };
    //一些数据可放在record里
    func(record);
}
```

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。