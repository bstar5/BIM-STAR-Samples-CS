# 应用数据存取

​	应用可以创建相应的自定义表去管理自己的数据。

## 通过API对自定义表进行增删改查

1、在项目下创建文件夹`Apis`，文件夹下面创建文件`CustomDataApi.cs`，代码内容如下所示。这个类封装了一些访问自定义数据API的方法。更多的请参考`自定义数据管理API`文档，然后自己封装方法。值得注意的是创建索引的方法，当调用一次API，传入的`fieldNames`如果有多个字段，则这些字段将作为联合索引。

```C#
using System.Threading.Tasks;
using Mango;

namespace DemoApp.Apis
{
    public static class CustomDataApi
    {
        private static readonly IMgWeb Web = Mg.Get<IMgWeb>();

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="fieldNames">属性名(字段名)集合</param>
        /// <param name="isUnique">是否是唯一键</param>
        /// <returns></returns>
        public static async Task<Result> CreateIndexAsync(string name, string[] fieldNames, bool isUnique = false)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/create_index", new
            {
                Name = name,
                FieldNames = fieldNames,
                IsUnique = isUnique
            });
            return result;
        }

        /// <summary>
        /// 获取自定义数据列表（筛选、分页、排序、映射）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="listParams">集合的筛选、分页、排序、映射参数</param>
        /// <returns></returns>
        public static async Task<Result> GetCustomDataListAsync(string name, ListParams listParams = null)
        {
            var result = await Web.GetResultAsync<DemoApp>("v1/data/list", new
            {
                Name = name,
                ListParams = listParams
            });
            return result;
        }

        /// <summary>
        /// 获取自定义数据详情（映射）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="id">自定义数据的Id</param>
        /// <param name="map">返回的属性映射数组</param>
        /// <returns></returns>
        public static async Task<Result> GetCustomDataInfoAsync(string name, Id id, string[] map = null)
        {
            var result = await Web.GetResultAsync<DemoApp>("v1/data/info", new
            {
                Name = name,
                Id = id,
                Map = map
            });
            return result;
        }

        /// <summary>
        /// 添加自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="data">自定义数据内容</param>
        /// <returns></returns>
        public static async Task<Result> AddCustomDataAsync(string name, IRecord[] data)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/add", new
            {
                Name = name,
                Data = data
            });
            return result;
        }

        /// <summary>
        /// 删除自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="ids">自定义数据的Id集合</param>
        /// <returns></returns>
        public static async Task<Result> DeleteCustomDataAsync(string name, Id[] ids)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/delete", new
            {
                Name = name,
                Ids = ids
            });
            return result;
        }

        /// <summary>
        /// 修改自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="data">修改的自定义数据内容集合</param>
        /// <returns></returns>
        public static async Task<Result> UpdateCustomDataAsync(string name, IRecord[] data)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/update", new
            {
                Name = name,
                Data = data
            });
            return result;
        }
```

2、在对应的`ViewModel`层中，通过API获取数据如下所示。方法`AddCustomDataAsync`添加自定义数据需要传入两个参数，第一个`_tableName`是新建表的表名，第二个参数是需要添加到自定义表中的数据。`IRecord`是每一条记录，`DynamicRecord`是继承`IRecord`，`DynamicRecord`是一个字典，保存自定义字段和字段对应的数据。添加自定义数据成功后，就可以通过方法`GetCustomDataListAsync`获取刚刚添加的自定义数据，然后把得到的数据转成界面绑定的实体类。具体的可以看示例源码。

```C#
private async void Refresh()
{
     var data = new List<IRecord>
     {
           new DynamicRecord{{"Number", 1},{"Name", "小茗"},{"Remark", "-"}},
           new DynamicRecord{{"Number", 2},{"Name", "冷冷"},{"Remark", "-"}},
    };
    var result = await CustomDataApi.AddCustomDataAsync(_tableName, data.ToArray());
    if (!result.IsOk)
    {
        Mg.Get<IMgDialog>().ShowDesktopAlert("添加学生数据到自定义新表失败", result.Message);
        return;
    }
    var result = await CustomDataApi.GetCustomDataListAsync(_tableName);
    if (!result.IsOk)
        return;
    UserSource.Clear();
    var records = result.GetRecords();
    foreach (var record in records)
    {
       UserSource.Add(record.As<User>());
   }
}
```

**PS：**在修改数据表的时候，可以添加新的字段信息，比如`_tableName`表中的记录都没有字段`Address`，可以通过API的修改方法把这个字段添加进去。不过应该兼容旧数据。

## 定义与表对应的实体类及其使用

1、尽管框架中统一采用`DynamicRecord`来表示一条记录，但还是建议定义实体类作为临时变量使用，有如下好处：智能感知提示，方便传参，避免写错字符串导致访问数据失败，下面是`User`实体类代码。

```C#
public class User
{
    public Id _id { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public string Remark { get; set; }
}
```

2、实体类通常作为临时变量使用，`IDynamicRecord`可通过框架提供的`As<T>()`转为实体类。因为每条记录在数据库中都有一个`_id`，这个字段的数据在修改记录数据的时候会需要，因此也需要保存在实体类里，访问API返回的数据在`result.Data`中，可以通过`result.Data.As<类型>()`转换，也可以通过`result.GetRecords()`或者`result.GetRecord()`，取决于API文档中的返回数据类型说明。然后对每一条记录通过`record.As<User>()`转换为界面绑定的`User`实体类，因为`record`里的字段对应`User`里的字段，所以可以这样做。

```c#
var result = await CustomDataApi.GetCustomDataListAsync(_tableName);
if (!result.IsOk)
   return;
UserSource.Clear();
var records = result.GetRecords();
foreach (var record in records)
{
   UserSource.Add(record.As<User>());
}
```

**PS:**更多的自定义数据的API操作，请参考`自定义数据管理API`文档，里面还有针对**树**类型数据的API。

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。当第一次访问添加自定义数据api的时候，可能会失败，这可能是因为你想要创建的表名已经被其他用户所使用了，这个时候就需要改一下传入的表名参数了，建议`公司名 + 表名`作为name的值传入。