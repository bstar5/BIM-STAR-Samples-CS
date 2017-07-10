# 控件RadGridView使用

之前已经简单介绍了`RadGridView`列表控件的使用，下面详细说明一下。

- `View`层代码如下所示。

  ```html
  <telerik:RadGridView Grid.Column="1" AutoGenerateColumns="False" SelectionMode="Single" ShowGroupPanel="False" IsFilteringAllowed="True" RowIndicatorVisibility="Collapsed" CanUserFreezeColumns="False" IsReadOnly="False" CanUserSortColumns="True" Margin="0,10,10,10"
  ItemsSource="{Binding UserSource}" SelectedItem="{Binding SelectedUserItem}">
       <telerik:RadGridView.Columns>
            <telerik:GridViewDataColumn Header="序号" DataMemberBinding="{Binding Number}" />
            <telerik:GridViewDataColumn Header="姓名" DataMemberBinding="{Binding Name}" />
            <telerik:GridViewDataColumn Header="备注" DataMemberBinding="{Binding Remark}" />
        </telerik:RadGridView.Columns>
  </telerik:RadGridView>
  ```

  `RadGridView`控件的数据源属性是`ItemsSource`，可以绑定对应的`ViewModel`层的某个数据集。属性`SelectedItem`则是用户当前鼠标所选中的某一行，通过绑定可以在后台操作这个对象。`telerik:GridViewDataColumn`则是设置每一列，`Header`设置的是对应的列名，`DataMemberBinding`设置的是需要显示数据集的类型的属性名。

  `RadGridView`控件的属性`IsFilteringAllowed`允许有自带的列筛选功能，`IsReadOnly`单元格是否可编辑辑，`CanUserSortColumns`是否允许用户点击列名进行排序等等，要了解更多属性请去官网查看`RadGridView`控件的文档。


- `ViewModel`层代码如下所示。`UserSource`是一种`User`类型的集合，有时候界面绑定的数据集可能会在后台进行增删改，这个时候数据集类型应该使用`ObservableCollection`，它会根据集合的变化在界面上显示相应的变化的。`SelectedUserItem`则对应的是用户点击选择的某一行对象。每当有属性发生变化的时候，都会进入`OnPropertyChanged()`方法，因此可以在这个方法里根据`SelectedUserItem`的变化写需要处理的逻辑代码。

  ```C#
  using System.Collections.ObjectModel;
  using DemoApp.Models;
  using Mango;

  namespace DemoApp.ViewModels
  {
      public class FirstViewModel : ViewModelBase
      {
          public FirstViewModel()
          {
              UserSource = new ObservableCollection<User>
              {
                  new User{Number = "1",Name="小茗",Remark="-"},
                  new User{Number = "2",Name="冷冷",Remark="-"},
                  new User{Number = "3",Name="暖暖",Remark="-"}
              };
          }

          private ObservableCollection<User> _userSource;

          /// <summary>
          /// RadGridView的数据源
          /// </summary>
          public ObservableCollection<User> UserSource
          {
              get { return _userSource; }
              set { Set("UserSource", ref _userSource, value); }
          }

          private User _selectedUserItem;

          /// <summary>
          /// 获取或设置SelectedUserItem属性
          /// </summary>
          public User SelectedUserItem
          {
              get { return _selectedUserItem; }
              set { Set("SelectedUserItem", ref _selectedUserItem, value); }
          }

          private string _textBlockTest;

          /// <summary>
          /// 获取或设置TextBlockTest属性
          /// </summary>
          public string TextBlockTest
          {
              get { return _textBlockTest; }
              set { Set("TextBlockTest", ref _textBlockTest, value); }
          }

          protected override void OnPropertyChanged(string propertyName)
          {
              base.OnPropertyChanged(propertyName);
              if (propertyName == "SelectedUserItem" && SelectedUserItem != null)
              {
                  TextBlockTest = $"您刚刚选择了{SelectedUserItem.Name}!";
              }
          }
      }
  }
  ```

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。