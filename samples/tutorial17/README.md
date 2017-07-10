# 访问WebAPI

​	应用开发过程中，一些相关数据只能通过访问服务器端提供的`WebAPI`来获取数据。具体的`WebAPI`的URL请参考对应的`API`说明文档。下面介绍如何通过`WebAPI`来获取数据。

## `Get`请求

1、通过`Mg.Get<IMgWeb>()`获取框架封装好的可访问API的对象，然后通过该对象的方法`GetResultAsync`发送`Get`请求，第一个参数是API`Url`，不用写全，仅需写版本后的路径。第二个参数是`Url`的参数及参数值。

```c#
/// <summary>
/// 获取自定义数据列表（筛选、分页、排序、映射）
/// </summary>
/// <param name="name">数据集名称(表名)</param>
/// <param name="listParams">集合的筛选、分页、排序、映射参数</param>
/// <returns></returns>
public static async Task<Result> GetCustomDataListAsync(string name, ListParams listParams = null)
{
  	var Web = Mg.Get<IMgWeb>();
    var result = await Web.GetResultAsync<DemoApp>("v1/data/list", new
    {
        Name = name,
        ListParams = listParams
    });
    return result;
}
```

2、操作API返回的数据。如果`result.IsOk`是`true`，则`result.Data`中有数据；`result.IsOk`是`false`，则`result.Data`中没有数据。可以通过`result.Data.As<类型>()`转换成对应的数据类型，也可以通过`result.GetRecords()`或者`result.GetRecord()`，取决于API文档中的返回数据类型说明。如果`record`中字段对应实体类里的属性，则可直接转换，否则一个一个的转换。

```C#
var result = await CustomDataApi.GetCustomDataListAsync(_tableName);
if (!result.IsOk)
    return;
UserSource.Clear();
var records = result.GetRecords();
foreach (var record in records)
{
    //UserSource.Add(record.As<User>());
    UserSource.Add(new User
    {
        _id = record["_id"].As<Id>(),
        Number = record["Number"].As<string>(),
        Name = record["Name"].As<string>(),
        Remark = record["Remark"].As<string>()
    });
}
```

## `Post`请求

1、通过`Mg.Get<IMgWeb>()`获取框架封装好的可访问API的对象，然后通过该对象的方法`PostAsync`发送`Post`请求，第一个参数是API`Url`，不用写全，仅需写版本后的路径。第二个参数是`Url`的参数及参数值，第三个参数是需要传送的文件流，不需要传文件时则可省略。

```c#
/// <summary>
/// 添加自定义数据（批量）
/// </summary>
/// <param name="name">数据集名称(表名)</param>
/// <param name="data">自定义数据内容</param>
/// <returns></returns>
public static async Task<Result> AddCustomDataAsync(string name, IRecord[] data)
{
  	var Web = Mg.Get<IMgWeb>();
    var result = await Web.PostAsync<DemoApp>("v1/data/add", new
    {
        Name = name,
        Data = data
    });
    return result;
}

/// <summary>
/// 传入文件流
/// </summary>
public static async Task<Result> UpdateFileAsync(string name, IRecord[] data)
{
  	var Web = Mg.Get<IMgWeb>();
    var fileName = Path.GetFileName(filePath);
    var name = Path.GetFileNameWithoutExtension(filePath);
    using (var stream = new FileStream(filePath, FileMode.Open))
    {
        var streamInfo = new StreamInfo { FileName = fileName, Name = name, Stream = stream };
        var result = await Web.PostAsync<BuildingFloorApp>("v1/data/add", new
        {
        	Name = name,
        	Data = data
        }, streamInfo);
        return result;
    }
}
```

2、操作`Post`API返回的数据的方法与`Get`API返回的数据的方法一样，这里不再详述。

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。运行示例时，可能会失败，修改_tableName的值即可。