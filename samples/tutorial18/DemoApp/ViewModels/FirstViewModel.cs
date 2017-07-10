using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DemoApp.Apis;
using DemoApp.Models;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private readonly string _tableName;

        public FirstViewModel()
        {
            _tableName = "demo_student";
            UserSource = new ObservableCollection<User>();
            Refresh();
            AddData = new RelayCommand(OnAddData, CanAddData);
            CreateIndex = new RelayCommand(OnCreateIndex, CanCreateIndex);
            DeleteData = new RelayCommand(OnDeleteData, CanDeleteData);
            UpdateData = new RelayCommand(OnUpdateData, CanUpdateData);
        }

        private ObservableCollection<User> _userSource;

        /// <summary>
        /// RadGridView绑定的数据源
        /// </summary>
        public ObservableCollection<User> UserSource
        {
            get { return _userSource; }
            set { Set("UserSource", ref _userSource, value); }
        }

        private User _selectedUserItem;

        /// <summary>
        /// RadGridView绑定的选择项
        /// </summary>
        public User SelectedUserItem
        {
            get { return _selectedUserItem; }
            set { Set("SelectedUserItem", ref _selectedUserItem, value); }
        }

        /// <summary>
        /// 创建自定义新表的索引
        /// </summary>
        public RelayCommand CreateIndex { get; private set; }

        private bool CanCreateIndex()
        {
            return true;
        }

        private async void OnCreateIndex()
        {
            var result = await CustomDataApi.CreateIndexAsync(_tableName, new[] { "Number" }, true);
            if (!result.IsOk)
            {
                Mg.Get<IMgDialog>().ShowDesktopAlert("为自定义新表创建学号索引失败", result.Message);
                return;
            }
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "为自定义新表创建学号索引成功");
        }

        /// <summary>
        /// 往自定义新表添加数据
        /// </summary>
        public RelayCommand AddData { get; private set; }

        private bool CanAddData()
        {
            return true;
        }

        private async void OnAddData()
        {
            var data = new List<IRecord>
            {
                new DynamicRecord
                {
                    {"Number", UserSource.Count + 1},
                    {"Name", "小茗"},
                    {"Remark", "-"}
                },
                new DynamicRecord
                {
                    {"Number", UserSource.Count + 2},
                    {"Name", "冷冷"},
                    {"Remark", "-"}
                },
                new DynamicRecord
                {
                    {"Number", UserSource.Count + 3},
                    {"Name", "暖暖"},
                    {"Remark", "-"}
                }
            };
            var result = await CustomDataApi.AddCustomDataAsync(_tableName, data.ToArray());
            if (!result.IsOk)
            {
                Mg.Get<IMgDialog>().ShowDesktopAlert("添加学生数据到自定义新表失败", result.Message);
                return;
            }
            Refresh();
        }

        /// <summary>
        /// 删除自定义新表最后一条数据
        /// </summary>
        public RelayCommand DeleteData { get; private set; }

        private bool CanDeleteData()
        {
            return true;
        }

        private async void OnDeleteData()
        {
            if (UserSource.Count == 0)
                return;
            var lastUser = UserSource.Last();
            var result = await CustomDataApi.DeleteCustomDataAsync(_tableName, new[] { lastUser._id });
            if (!result.IsOk)
            {
                Mg.Get<IMgDialog>().ShowDesktopAlert("删除最后一个学生数据失败", result.Message);
                return;
            }
            Refresh();
        }

        /// <summary>
        /// 修改自定义新表最后一条数据的备注
        /// </summary>
        public RelayCommand UpdateData { get; private set; }

        private bool CanUpdateData()
        {
            return true;
        }

        private async void OnUpdateData()
        {
            if (UserSource.Count == 0)
                return;
            var lastUser = UserSource.Last();
            var record = lastUser.AsDictionary().As<DynamicRecord>();
            record["Remark"] = $"于{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}被修改了备注信息";
            var data = new List<IRecord> { record };
            var result = await CustomDataApi.UpdateCustomDataAsync(_tableName, data.ToArray());
            if (!result.IsOk)
            {
                Mg.Get<IMgDialog>().ShowDesktopAlert("修改最后一个学生的备注信息失败", result.Message);
                return;
            }
            Refresh();
        }

        /// <summary>
        /// 获取表的最新数据
        /// </summary>
        private async void Refresh()
        {
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
    }
}