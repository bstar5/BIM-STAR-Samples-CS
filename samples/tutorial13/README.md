# 框架控件

下面介绍，框架中的基本控件的使用。首先，添加引用`Mango.dll`和`Mango.Wpf.dll`。

## `MetroProgressBar`	控件

`MetroProgressBar`为进度条控件，多用在大量数据加载的时候，呈现出一个正在加载数据的状态，给予用户比较良好的体验。像`RadGridView`控件通常需要通过API获取数据，这个过程就需要等待，这时候就需要使用到进度条。

1.首先，在`View`层，前端代码中添加`Resources`中添加`converters:BoolToVisibilityCVT`，这个是一个转换器，将布尔类型的值转换为对应的控件可见值，代码如下。

```xml
<core:ViewBase.Resources>
    <converters:BoolToVisibilityCvt x:Key="BoolToVisibilityCvt" />
</core:ViewBase.Resources>
```

2.然后，在`View`层的控件`RadGridView`上方添加`MetroProgressBar`控件代码，代码如下所示。进度条属性`Visibility`绑定后台的`IsBusy`属性，通过转换器将`IsBusy`的布尔值转换成对应的`Visibility`类型值。

```xml
<controls:MetroProgressBar Grid.Row="1" VerticalAlignment="Bottom" VerticalContentAlignment="Bottom"
        				IsIndeterminate="True" Visibility="{Binding IsBusy, Converter={StaticResource BoolToVisibilityCvt}}" />
```

3.由于对应的`ViewModel`层继承了`ViewModelBase`类，因此它具有`IsBusy`通知属性，它是布尔值类型，可以通过设置`IsBusy`的值`true`或`false`来控制进度条的显示与隐藏。代码如下所示。

```c#
IsBusy = true;//进度条显示
await Task.Delay(5000);//延迟5秒,模拟网络延迟
UserSource.AddRange(new[] {
                new User { Number = "1", Name = "小茗", Remark = "-" },
                new User { Number = "2", Name = "冷冷", Remark = "-" },
                new User { Number = "3", Name = "暖暖", Remark = "-" }
            });
IsBusy = false;//进度条隐藏
```

## `SearchBox`	控件

对于一些表格，通常会用到搜索功能。下面，介绍如何使用`SearchBox`控件。

1.首先，在`View`层，添加`PromptBox`控件，代码如下所示。`SearchFunc`绑定的是后台的一个搜索方法。

```xml
<controls:SearchBox Grid.Row="1" Margin="10,10,10,0" MinWidth="200" SearchFunc="{Binding Search}" />
```

2.然后，在`ViewModel`层，添加如下代码。

```c#
private List<User> _bakUserSource;

public Func<string, Task> Search { get; private set; }

private async Task OnSearchAsync(string searchStr)
{
    if (string.IsNullOrEmpty(searchStr))
    {
        UserSource = new ObservableCollection<User>();
        UserSource.AddRange(_bakUserSource);
    }
    var searchItems = await Task.Run(() =>
    {
        var items = new ObservableCollection<User>();
        if (searchStr == null) return items;
        searchStr = searchStr.ToLower().Trim();
        foreach (var item in _bakUserSource)
        {
            if (ContainsString(item.Number + "", searchStr)
                || ContainsString(item.Name, searchStr)
                || ContainsString(item.Remark, searchStr)
                )
            {
                items.Add(item);
            }
        }
        return items;
    });
    UserSource = new ObservableCollection<User>(searchItems);
}

private static bool ContainsString(string str, string searchStr)
{
    return str != null && str.Contains(searchStr, CompareOptions.IgnoreCase);
}
```

3.之后在`ViewModel`层类构造函数中，添加如下代码：

```c#
Search = OnSearchAsync;
```

4.到此，实现了`SearchBox`控件的显示、绑定及功能实现。

## `PromptBox`	控件

1.首先，在`View`层，添加`PromptBox`控件，代码如下所示。当属性`Text`里的值为空时，则显示属性`Placeholder`里的文本内容。

```xml
<controls:PromptBox  Name="PromptBox" Placeholder="请填写您的名字" Text="{Binding YourName}"/>
```

2.然后，在`ViewModel`层，添加`YourName`通知属性，如下所示。

```c#
private string _yourName;

/// <summary>
/// PromptBox绑定的文本
/// </summary>
public string YourName
{
      get { return _yourName; }
      set { Set("YourName", ref _yourName, value); }
}
```

3.这样，实现了`PromptBox`的数据绑定。

## `ValidationTip`	控件

1.当希望给用户输入的数据进行验证时，则需要用到`ValidationTip`控件。这里将配合`PromptBox`控件一起使用。首先，在`View`层，添加`ValidationTip`控件，代码如下所示。

```xml
<Grid Margin="10">
     <Grid.ColumnDefinitions>
           <ColumnDefinition Width="Auto" />
           <ColumnDefinition Width="Auto" />
      </Grid.ColumnDefinitions>
      <controls:PromptBox  Name="PromptBox" Grid.Column="0" Placeholder="请填写您的名字" Text="{Binding YourName,Mode=TwoWay,UpdateSourceTrigger=PropertyChanged,NotifyOnValidationError=True,ValidatesOnDataErrors=True}"/>
      <controls:ValidationTip Grid.Column="2" Width="16" Height="16" ValidationElement="{Binding ElementName=PromptBox}" HorizontalAlignment="Left" />
</Grid>
```

2.`ValidationTip`控件的属性`ValidationElement`对应要验证的控件`Name`属性，即`PromptBox`控件的`Name`的值（`PromptBox`）。要想根据`PromptBox`控件的输入值的每次变化进行验证，还需要给`PromptBox`控件的属性`Text`添加如上的代码，这样的话，`Text`绑定的值发生变化的时候，验证控件会收到验证通知。

3.然后，在`ViewModel`层，`Title`属性添加验证规则，代码如下所示，这是最简单的一种验证方式。这样，就可以对`Title`进行验证了。

```c#
private string _yourName;
[Required(ErrorMessage = "不能为空！")]
public string YourName
{
    get { return _yourName; }
    set { Set("YourName", ref _yourName, value); }
}
```

4.框架里提供了自定义的验证的方法。代码如下所示。`propertyName`会传入一些发生变化的属性值，当指定的通知属性发生变化时，可以对其进行判断分析，从而用`return `返回一些需要显示的验证结果语。

```C#
 protected override string GetErrorFor(string propertyName)
{
     if (propertyName == "YourName")
     {
          if (string.IsNullOrEmpty(YourName))
              return "填写的名字不能空！";
     }
     return base.GetErrorFor(propertyName);
}
```

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。