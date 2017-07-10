using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Mango;
using PostilApp.Apis;
using PostilApp.Entity;
using PostilApp.Models;

namespace PostilApp.ViewModels
{
    public class CheckPostilsViewModel : ViewModelBase
    {
        private readonly List<PostilInfoModel> _listPostilInfoModel;
        private int _currentPageIndex;
        public bool DeletePostilFlag;
        private bool _canNextPage;
        private Id _selectedPostilId;
        private bool _isSelfDelPostil;

        public CheckPostilsViewModel()
        {
            _currentPageIndex = 0;
            _listPostilInfoModel = new List<PostilInfoModel>();
            ViewPictureModels = new ObservableCollection<PictureModel>();
            ListTag = new ObservableCollection<TagInfoModel>();
            PreviousPicture = new RelayCommand(OnPreviousPicture, CanPreviousPicture);
            NextPicture = new RelayCommand(OnNextPicture, CanNextPicture);
            Refresh = new RelayCommand(OnRefresh, CanRefresh);
            ImageStretch = Stretch.UniformToFill;
            Mg.Get<IMgWeb>().AddWebMsgHandler(App.Instance<PostilApp>(), "demoPostil/add", DataAdd);//接收广播
            Mg.Get<IMgWeb>().AddWebMsgHandler(App.Instance<PostilApp>(), "demoPostil/delete", DataDelete);//接收广播
        }

        public CheckPostilsViewModel(int page, Id selectedPostilId) : this()
        {
            _currentPageIndex = page;
            _selectedPostilId = selectedPostilId;
        }

        #region 属性

        private string _postilTitle;

        /// <summary>
        /// 获取或设置PostilTitle属性
        /// </summary>
        public string PostilTitle
        {
            get { return _postilTitle; }
            set { Set("PostilTitle", ref _postilTitle, value); }
        }

        private DateTime _postilTime;

        /// <summary>
        /// 获取或设置PostilTime属性
        /// </summary>
        public DateTime PostilTime
        {
            get { return _postilTime; }
            set { Set("PostilTime", ref _postilTime, value); }
        }

        private ObservableCollection<TagInfoModel> _listTag;

        /// <summary>
        /// 获取或设置ListTag属性
        /// </summary>
        public ObservableCollection<TagInfoModel> ListTag
        {
            get { return _listTag; }
            set { Set("ListTag", ref _listTag, value); }
        }

        private string _publicOrPrivate;

        /// <summary>
        /// 获取或设置PublicOrPrivate属性
        /// </summary>
        public string PublicOrPrivate
        {
            get { return _publicOrPrivate; }
            set { Set("PublicOrPrivate", ref _publicOrPrivate, value); }
        }

        private string _postilFounder;

        /// <summary>
        /// 获取或设置PostilFounder属性
        /// </summary>
        public string PostilFounder
        {
            get { return _postilFounder; }
            set { Set("PostilFounder", ref _postilFounder, value); }
        }

        /// <summary>
        /// 控制界面大图片的显示
        /// </summary>
        private string _bigPictureUrl;

        public string BigPictureUrl
        {
            get { return _bigPictureUrl; }
            set { Set("BigPictureUrl", ref _bigPictureUrl, value); }
        }

        private ObservableCollection<PictureModel> _viewPictureModels;

        /// <summary>
        /// 图片的集合
        /// </summary>
        public ObservableCollection<PictureModel> ViewPictureModels
        {
            get { return _viewPictureModels; }
            set { Set("ViewPictureModels", ref _viewPictureModels, value); }
        }

        private PictureModel _selectedViewPicModel;

        /// <summary>
        /// 选择的图片
        /// </summary>
        public PictureModel SelectedViewPicModel
        {
            get { return _selectedViewPicModel; }
            set { Set("SelectedViewPicModel", ref _selectedViewPicModel, value); }
        }

        private Stretch _imageStretch;

        /// <summary>
        /// 获取或设置ImageStretch属性
        /// </summary>
        public Stretch ImageStretch
        {
            get { return _imageStretch; }
            set { Set("ImageStretch", ref _imageStretch, value); }
        }

        #endregion 属性

        #region 命令

        public RelayCommand PreviousPicture { get; private set; }

        private bool CanPreviousPicture()
        {
            if (IsBusy || SelectedViewPicModel == null)
                return false;
            if (_currentPageIndex == 0 && SelectedViewPicModel.SelectedPicIndex == 0)
                return false;
            return true;
        }

        private void OnPreviousPicture()
        {
            if (!CanPreviousPicture())
                return;
            var index = SelectedViewPicModel.SelectedPicIndex;
            if (index == 0 && _currentPageIndex != 0)
                LoadPostilInfosAsync(--_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.LastOrDefault());
            else
                SelectedViewPicModel = ViewPictureModels.ElementAt(index - 1);
        }

        public RelayCommand NextPicture { get; private set; }

        private bool CanNextPicture()
        {
            if (IsBusy || SelectedViewPicModel == null)
                return false;
            var index = SelectedViewPicModel.SelectedPicIndex;
            if (index < 7 && index == ViewPictureModels.Count - 1)
                return false;
            if (index == 7 && !_canNextPage)
                return false;
            return true;
        }

        private void OnNextPicture()
        {
            if (!CanNextPicture())
                return;
            var index = SelectedViewPicModel.SelectedPicIndex;
            if (index != ViewPictureModels.Count - 1)
                SelectedViewPicModel = ViewPictureModels.ElementAt(index + 1);
            else
                LoadPostilInfosAsync(++_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.FirstOrDefault());
        }

        public RelayCommand Refresh { get; private set; }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        private void OnRefresh()
        {
            var selectId = SelectedViewPicModel?.PostilId;
            SelectedViewPicModel = null;
            LoadPostilInfosAsync(_currentPageIndex, () =>
            {
                SelectedViewPicModel = ViewPictureModels.FirstOrDefault(t => t.PostilId == selectId) ?? ViewPictureModels.FirstOrDefault();
            });
        }

        #endregion 命令

        #region 重载

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "SelectedViewPicModel")
            {
                if (SelectedViewPicModel != null)
                {
                    BigPictureUrl = SelectedViewPicModel.ImageUrl;
                    var model = _listPostilInfoModel.FirstOrDefault(t => t._id == SelectedViewPicModel.PostilId);
                    if (model == null)
                        return;
                    PostilTitle = model.Title;
                    PostilTime = model.CreateTime;
                    PostilFounder = model.PostilUser;
                    PublicOrPrivate = model.IsPublic ? "公开" : "不公开";
                    ListTag.Clear();
                    foreach (var tag in model.Tags)
                        ListTag.Add(new TagInfoModel { Name = $" {tag} " });
                }
                else
                {
                    BigPictureUrl = null;
                    PostilTitle = "";
                    PostilTime = DateTime.MinValue;
                    PostilFounder = "";
                    PublicOrPrivate = "";
                    ListTag.Clear();
                }
            }
        }

        protected override void OnViewLoaded(bool isFirstLoaded)
        {
            base.OnViewLoaded(isFirstLoaded);
            if (!isFirstLoaded)
                return;
            if (_selectedPostilId != null)
                LoadPostilInfosAsync(_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.FirstOrDefault(t => t.PostilId == _selectedPostilId));
            else
            {
                var pane = Mg.Get<IMgDocking>().GetPane(typeof(PostilPanelListViewModel));
                var panelListVm = pane?.Content as PostilPanelListViewModel;
                if (panelListVm?.SelectedPostilInfoModel == null || panelListVm.PostilInfoModels.Count == 0)
                    LoadPostilInfosAsync(0);
                else
                {
                    var index = panelListVm.PostilInfoModels.IndexOf(panelListVm.SelectedPostilInfoModel);
                    if (index == -1)
                        _currentPageIndex = 0;
                    else
                        _currentPageIndex = index / 8;
                    _selectedPostilId = panelListVm.SelectedPostilInfoModel._id;
                    LoadPostilInfosAsync(_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.FirstOrDefault(t => t.PostilId == _selectedPostilId));
                }
            }
        }

        #endregion 重载

        #region 其他方法

        /// <summary>
        /// 读取下一页批注是否还有数据
        /// </summary>
        private async void LoadNextPageCountAsync()
        {
            var listParams = new ListParams
            {
                Search = Query.Eq(Hubs.Postil.ProjectId, Mg.Get<IMgContext>().ProjectId),
                Page = new ListParams.PageInfo(_currentPageIndex + 1, 8),
                Sort = new[] { new ListParams.SortProperty(Hubs.Postil.CreateTime, true) },
                Map = new[] { Id.Name }
            };
            var result = await DataApi.GetListAsync(Hubs.Postil.T, listParams);
            if (!result.IsOk)
            {
                _canNextPage = false;
                return;
            }
            _canNextPage = result.GetRecords().Count > 0;
        }

        /// <summary>
        /// 加载指定页的批注信息
        /// </summary>
        private async void LoadPostilInfosAsync(int pageIndex, Action action = null)
        {
            IsBusy = true;
            var listParams = new ListParams
            {
                Search = Query.Eq(Hubs.Postil.ProjectId, Mg.Get<IMgContext>().ProjectId),
                Page = new ListParams.PageInfo(pageIndex, 8),
                Sort = new[] { new ListParams.SortProperty(Hubs.Postil.CreateTime, false) }
            };
            var dataResult = await DataApi.GetListAsync(Hubs.Postil.T, listParams);
            _listPostilInfoModel.Clear();
            if (!dataResult.IsOk)
            {
                Mg.Get<IMgLog>().Error("获取批注信息失败" + dataResult.Message);
                Mg.Get<IMgDialog>().ShowDesktopAlert("获取批注信息失败", dataResult.Message);
                IsBusy = false;
                return;
            }
            var models = dataResult.GetRecords().Select(t => t.As<PostilInfoModel>()).ToList();
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
                _listPostilInfoModel.Add(postilInfoModel);
            }
            ViewPictureModels.Clear();
            for (var index = 0; index < models.Count; index++)
            {
                var model = models[index];
                ViewPictureModels.Add(new PictureModel
                {
                    SelectedPicIndex = index,
                    PostilId = model._id,
                    ImageUrl = model.ImageUrl
                });
            }
            SelectedViewPicModel = ViewPictureModels.FirstOrDefault();
            action?.Invoke();
            LoadNextPageCountAsync();
            IsBusy = false;
        }

        /// <summary>
        /// 删除某条批注事件
        /// </summary>
        /// <param name="delPicModel"></param>
        private async void OnDeletePostilItem(PictureModel delPicModel)
        {
            if (delPicModel == null)
                return;
            if (this.ShowMessage("删除后将无法恢复信息，确定删除这条批注吗？", "警告", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;
            var index = delPicModel.SelectedPicIndex;
            var deleteId = delPicModel.PostilId;
            var result = await DataApi.DeleteAsync(Hubs.Postil.T, new[] { deleteId });
            if (!result.IsOk)
            {
                Mg.Get<IMgLog>().Error("删除批注信息失败" + result.Message);
                Mg.Get<IMgDialog>().ShowDesktopAlert("删除批注信息失败", result.Message);
                return;
            }
            if (ViewPictureModels.Count == 8)
            {
                if (SelectedViewPicModel.PostilId == deleteId)
                    LoadPostilInfosAsync(_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.ElementAtOrDefault(index) ?? ViewPictureModels.LastOrDefault());
                else
                {
                    var selectId = SelectedViewPicModel.PostilId;
                    LoadPostilInfosAsync(_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.FirstOrDefault(t => t.PostilId == selectId) ?? ViewPictureModels.FirstOrDefault());
                }
            }
            else
            {
                var selectId = SelectedViewPicModel.PostilId;
                ViewPictureModels.Remove(delPicModel);
                for (var i = 0; i < ViewPictureModels.Count; i++)//重新给索引
                    ViewPictureModels[i].SelectedPicIndex = i;
                _listPostilInfoModel.Remove(_listPostilInfoModel.FirstOrDefault(t => t._id == deleteId));
                if (deleteId == selectId)
                    SelectedViewPicModel = ViewPictureModels.ElementAtOrDefault(index) ?? ViewPictureModels.LastOrDefault();
                if (ViewPictureModels.Count == 0 && _currentPageIndex != 0)
                    LoadPostilInfosAsync(--_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.LastOrDefault());
            }
            _isSelfDelPostil = true;
            await Mg.Get<IMgWeb>().SendWebMsgAsync("demoPostil/delete", deleteId.ToString(), BoardcastType.ProjectGroup);//发送广播
        }

        /// <summary>
        /// 批注增加的广播信息处理事件
        /// </summary>
        /// <param name="eventArgs"></param>
        private void DataAdd(WebMsgEventArgs eventArgs)
        {
            var index = SelectedViewPicModel?.SelectedPicIndex ?? 0;
            if (_currentPageIndex == 0)//最新的只会添加在前面
                LoadPostilInfosAsync(_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.ElementAtOrDefault(index) ?? ViewPictureModels.LastOrDefault());
        }

        private void DataDelete(WebMsgEventArgs eventArgs)
        {
            if (_isSelfDelPostil)
            {
                _isSelfDelPostil = false;
                return;
            }
            var deleteId = eventArgs.Data.As<Id>();
            if (deleteId == null)
                return;
            var model = ViewPictureModels.FirstOrDefault(t => t.PostilId == deleteId);
            if (model == null)
                return;
            if (ViewPictureModels.Count == 8)
            {
                if (SelectedViewPicModel.PostilId == deleteId)
                    LoadPostilInfosAsync(_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.ElementAtOrDefault(model.SelectedPicIndex) ?? ViewPictureModels.LastOrDefault());
                else
                {
                    var selectId = SelectedViewPicModel.PostilId;
                    LoadPostilInfosAsync(_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.FirstOrDefault(t => t.PostilId == selectId) ?? ViewPictureModels.FirstOrDefault());
                }
            }
            else
            {
                ViewPictureModels.Remove(model);
                for (var i = 0; i < ViewPictureModels.Count; i++)//重新给索引
                    ViewPictureModels[i].SelectedPicIndex = i;
                _listPostilInfoModel.Remove(_listPostilInfoModel.FirstOrDefault(t => t._id == deleteId));
                if (deleteId == SelectedViewPicModel.PostilId)
                    SelectedViewPicModel = ViewPictureModels.ElementAtOrDefault(model.SelectedPicIndex) ?? ViewPictureModels.LastOrDefault();
                if (ViewPictureModels.Count == 0 && _currentPageIndex != 0)
                    LoadPostilInfosAsync(--_currentPageIndex, () => SelectedViewPicModel = ViewPictureModels.LastOrDefault());
            }
        }

        #endregion 其他方法
    }
}