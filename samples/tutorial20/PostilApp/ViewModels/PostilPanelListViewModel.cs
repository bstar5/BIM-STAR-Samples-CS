using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Mango;
using Mango.ViewModels;
using PostilApp.Apis;
using PostilApp.Entity;
using PostilApp.Models;

namespace PostilApp.ViewModels
{
    public class PostilPanelListViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PostilInfoModel> _backupPostilInfoModels;

        public PostilPanelListViewModel()
        {
            PostilInfoModels = new ObservableCollection<PostilInfoModel>();
            _backupPostilInfoModels = new ObservableCollection<PostilInfoModel>();
            Refresh = new RelayCommand(OnRefresh, CanRefresh);
            Search = OnSearchAsync;
            Mg.Get<IMgWeb>().AddWebMsgHandler(App.Instance<PostilApp>(), "demoPostil/add", DataAdd);
            Mg.Get<IMgWeb>().AddWebMsgHandler(App.Instance<PostilApp>(), "demoPostil/delete", DataDelete);
        }

        #region 属性

        private ObservableCollection<PostilInfoModel> _postilInfoModels;

        /// <summary>
        /// 批注信息的集合
        /// </summary>
        public ObservableCollection<PostilInfoModel> PostilInfoModels
        {
            get { return _postilInfoModels; }
            set { Set("PostilInfoModels", ref _postilInfoModels, value); }
        }

        private PostilInfoModel _selectedPostilInfoModel;

        /// <summary>
        /// 选中批注信息行
        /// </summary>
        public PostilInfoModel SelectedPostilInfoModel
        {
            get { return _selectedPostilInfoModel; }
            set { Set("SelectedPostilInfoModel", ref _selectedPostilInfoModel, value); }
        }

        private string _searchText;

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set { Set("SearchText", ref _searchText, value); }
        }

        #endregion 属性

        #region 命令

        public RelayCommand Refresh { get; private set; }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        private void OnRefresh()
        {
            LoadPostilInfos();
        }

        #endregion 命令

        #region 重载

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "SelectedPostilInfoModel" && SelectedPostilInfoModel?.CameraMatrix != null)
            {
                var panes = Mg.Get<IMgDocking>().GetDockingPanes();
                foreach (var paneViewModel in panes.Where(paneViewModel => paneViewModel.Header == "图形平台"))
                {
                    paneViewModel.IsActive = true;
                    break;
                }
                var r = Mg.Get<IMgService>().Invoke<PostilApp>("GraphicPlatform:SetCameraMatrix", SelectedPostilInfoModel.CameraMatrix);
                if (!r.IsOk)
                {
                    this.ShowMessage("无法调用图形平台的服务，需加载图形平台模块！", "设置图形平台摄像机的位置失败",
                        MessageBoxButton.OK);
                }
            }
        }

        protected override void OnViewLoaded(bool isFirstLoaded)
        {
            base.OnViewLoaded(isFirstLoaded);
            if (!isFirstLoaded)
                return;
            LoadPostilInfos();
        }

        #endregion 重载

        #region 其他方法

        /// <summary>
        /// 加载所有批注信息数据
        /// </summary>
        private async void LoadPostilInfos()
        {
            IsBusy = true;
            PostilInfoModels.Clear();
            _backupPostilInfoModels.Clear();
            var listParams = new ListParams
            {
                Search = Query.Eq(Hubs.Postil.ProjectId, Mg.Get<IMgContext>().ProjectId),
                Sort = new[] { new ListParams.SortProperty(Hubs.Postil.CreateTime, false) }
            };
            var result = await DataApi.GetListAsync(Hubs.Postil.T, listParams);
            if (!result.IsOk)
            {
                Mg.Get<IMgLog>().Error("获取批注信息失败" + result.Message);
                Mg.Get<IMgDialog>().ShowDesktopAlert("获取批注信息失败", result.Message);
                IsBusy = false;
                return;
            }
            var models = result.GetRecords().Select(t => t.As<PostilInfoModel>()).ToList();
            if (models.Count == 0)
            {
                IsBusy = false;
                return;
            }
            var urlDic = await Util.Data.GetPictureUrlsAsync(models.Select(t => t.FileId).ToArray());
            var userDic = await Util.Data.GetUserInfosAsync(models.Select(t => t.CreateUser).ToArray());
            foreach (var postilInfoModel in models)
            {
                postilInfoModel.ImageUrl = urlDic?[postilInfoModel.FileId];
                postilInfoModel.PostilUser = userDic?[postilInfoModel.CreateUser];
                postilInfoModel.FirstTag = $" {postilInfoModel.Tags[0]} ";
                var dt1 = Convert.ToDateTime(postilInfoModel.CreateTime);
                var dt2 = DateTime.Now;
                postilInfoModel.LastTime = DateDiff(dt1, dt2);
                PostilInfoModels.Add(postilInfoModel);
                _backupPostilInfoModels.Add(postilInfoModel);
            }
            IsBusy = false;
        }

        public Func<string, Task> Search { get; private set; }

        public async Task OnSearchAsync(string searchStr)
        {
            PostilInfoModels.Clear();
            if (string.IsNullOrEmpty(searchStr))
            {
                PostilInfoModels.AddRange(_backupPostilInfoModels);
                return;
            }
            foreach (var model in _backupPostilInfoModels)
            {
                if (ContainsString(model.Title, searchStr)
                    || ContainsString(model.FirstTag, searchStr)
                    || ContainsString(model.CreateTime.ToString("yyyy-MM-dd HH:mm"), searchStr)
                    || ContainsString(model.ToString(), searchStr))
                    PostilInfoModels.Add(model);
            }
        }

        /// <summary>
        /// 比较字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchStr"></param>
        /// <returns></returns>
        private static bool ContainsString(string str, string searchStr)
        {
            return str != null && str.Contains(searchStr, CompareOptions.IgnoreCase);
        }

        /// <summary>
        /// 计算时间的差值
        /// </summary>
        /// <param name="dateTime1">需要对比的时间</param>
        /// <param name="dateTime2">现在时间</param>
        /// <returns></returns>
        public static string DateDiff(DateTime dateTime1, DateTime dateTime2)
        {
            var ts1 = new TimeSpan(dateTime1.Ticks);
            var ts2 = new TimeSpan(dateTime2.Ticks);
            var ts = ts1.Subtract(ts2).Duration();
            if (ts.Days >= 7)
            {
                return dateTime1.ToString("yyyy-MM-dd");
            }
            if (ts.Days == 0)
            {
                if (ts.Hours != 0) return ts.Hours + "小时前";
                if (ts.Minutes == 0)
                {
                    return "刚刚";
                }
                return ts.Minutes + "分钟前";
            }
            return ts.Days + "天前";
        }

        private async void OnMouseDoubleClick()
        {
            if (SelectedPostilInfoModel == null)
                return;
            var r = Mg.Get<IMgService>().Invoke<PostilApp>("GraphicPlatform:SetCameraMatrix", SelectedPostilInfoModel.CameraMatrix);
            if (!r.IsOk)
            {
                Mg.Get<IMgLog>().Error("设置图形平台摄像机的位置失败" + r.Message);
                Mg.Get<IMgDialog>().ShowDesktopAlert("设置图形平台摄像机的位置失败", $"无法调用图形平台的服务，需打开图形平台模块！{r.Message}");
                return;
            }
            var privilege = await Mg.Get<IMgContext>().GetPrivilegeAsync(ModuleKeys.OpenCheckPostilView);
            if (!privilege)
            {
                Mg.Get<IMgLog>().Error("您没有查看批注详情的权限！");
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "您没有查看批注详情的权限！");
                return;
            }
            var index = PostilInfoModels.IndexOf(SelectedPostilInfoModel);
            var checkPostils = new CheckPostilsViewModel(index / 8, SelectedPostilInfoModel._id);
            var vm = new DialogViewModel(checkPostils)
            {
                Title = "查看批注",
                Width = 1200,
                Height = 700,
                CancelButtonVisibility = Visibility.Collapsed,
                OkButtonVisibility = Visibility.Collapsed,
                Icon = this.GetAppImageSource("Assets/查看批注.png")
            };
            Mg.Get<IMgDialog>().ShowDialog(vm);
        }

        private async void DataAdd(WebMsgEventArgs eventArgs)
        {
            await Task.Delay(300);
            var addId = eventArgs.Data.As<Id>();
            if (addId == null)
                return;
            var result = await DataApi.GetDataInfoAsync(Hubs.Postil.T, addId);
            if (!result.IsOk)
                return;
            var model = result.GetRecord().As<PostilInfoModel>();
            var urlDic = await Util.Data.GetPictureUrlsAsync(new[] { model.FileId });
            var userDic = await Util.Data.GetUserInfosAsync(new[] { model.CreateUser });
            model.ImageUrl = urlDic?[model.FileId];
            model.PostilUser = userDic?[model.CreateUser];
            model.FirstTag = $" {model.Tags[0]} ";
            model.LastTime = DateDiff(Convert.ToDateTime(model.CreateTime), DateTime.Now);
            PostilInfoModels.Insert(0, model);
            _backupPostilInfoModels.Insert(0, model);
        }

        private void DataDelete(WebMsgEventArgs eventArgs)
        {
            var deleteId = eventArgs.Data.As<Id>();
            if (deleteId == null)
                return;
            var delModel = PostilInfoModels.FirstOrDefault(t => t._id == deleteId);
            if (delModel == null)
                return;
            PostilInfoModels.Remove(delModel);
            _backupPostilInfoModels.Remove(delModel);
        }

        #endregion 其他方法
    }
}