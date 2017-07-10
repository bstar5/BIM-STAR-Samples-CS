using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Mango;
using PostilApp.Apis;
using PostilApp.Entity;
using PostilApp.Models;

namespace PostilApp.ViewModels
{
    public class AddPostilViewModel : ViewModelBase
    {
        public ObservableCollection<TagInfoModel> PostilTagList { get; }
        public bool AddNewPostilFlag;
        private float[] _matrixArray;
        private bool _changeFlag;
        private string _screenshotPicPath;

        public AddPostilViewModel()
        {
            PostilTagList = new ObservableCollection<TagInfoModel>();
            AddPostil = new RelayCommand(OnAddPostil, CanAddPostil);
            CancleAddPostil = new RelayCommand(OnCancleAddPostil, CanCancleAddPostil);
            _matrixArray = new float[16];
        }

        #region 属性

        private ImageSource _viewPicture;

        /// <summary>
        /// 获取或设置ViewPicture属性
        /// </summary>
        public ImageSource ViewPicture
        {
            get { return _viewPicture; }
            set { Set("ViewPicture", ref _viewPicture, value); }
        }

        private string _postilTitle;

        /// <summary>
        /// 获取或设置PostilTitle属性
        /// </summary>
        public string PostilTitle
        {
            get { return _postilTitle; }
            set { Set("PostilTitle", ref _postilTitle, value); }
        }

        private string _postilTag;

        /// <summary>
        /// 获取或设置PostilTag属性
        /// </summary>
        public string PostilTag
        {
            get { return _postilTag; }
            set { Set("PostilTag", ref _postilTag, value); }
        }

        private bool _isPublic;

        /// <summary>
        /// 获取或设置IsPublic属性
        /// </summary>
        public bool IsPublic
        {
            get { return _isPublic; }
            set { Set("IsPublic", ref _isPublic, value); }
        }

        #endregion 属性

        #region 命令

        public RelayCommand AddPostil { get; private set; }

        private bool CanAddPostil()
        {
            if (string.IsNullOrWhiteSpace(PostilTitle))
                return false;
            if (string.IsNullOrWhiteSpace(PostilTag))
                return false;
            if (PostilTitle.Length > 20)
                return false;
            //if (PostilTag.Length > 20)
            //    return false;
            var inputTagList = PostilTag.Split('，', ',').ToList().Select(t => t.Trim()).ToList();
            if (inputTagList.Any(tag => tag.Length > 8) || inputTagList.Count > 4)
                return false;
            return !IsBusy;
        }

        public async void OnAddPostil()
        {
            var inputTagList = PostilTag.Split('，', ',').ToList().Select(t => t.Trim()).ToList();
            var tagSet = new HashSet<string>(inputTagList.Where(t => t != ""));
            if (!File.Exists(_screenshotPicPath))
                return;
            var result = await DataApi.UploadFilesAsync(new[] { _screenshotPicPath }, Mg.Get<IMgContext>().ProjectId);
            if (!result.IsOk)
            {
                Mg.Get<IMgLog>().Error("上传截图失败：" + result.Message);
                Mg.Get<IMgDialog>().ShowDesktopAlert("上传截图失败", result.Message);
                return;
            }
            var fileId = result.Data.AsList<Id>().FirstOrDefault();
            var data = new DynamicRecord
            {
                [Hubs.Postil.ProjectId] = Mg.Get<IMgContext>().ProjectId,
                [Hubs.Postil.Title] = PostilTitle,
                [Hubs.Postil.CreateTime] = DateTime.Now,
                [Hubs.Postil.CreateUser] = Mg.Get<IMgContext>().UserId,
                [Hubs.Postil.Tags] = tagSet,
                [Hubs.Postil.IsPublic] = IsPublic,
                [Hubs.Postil.FileId] = fileId,
                [Hubs.Postil.CameraMatrix] = _matrixArray
            };
            var createResult = await DataApi.AddAsync(Hubs.Postil.T, new IRecord[] { data });
            if (!createResult.IsOk)
            {
                Mg.Get<IMgLog>().Error("添加批注失败：" + createResult.Message);
                Mg.Get<IMgDialog>().ShowDesktopAlert("添加批注失败", createResult.Message);
                return;
            }
            var originalTagList = PostilTagList.Select(tag => tag.Name.Trim()).ToList();
            var tagData = (from tag in tagSet
                           where !originalTagList.Contains(tag)
                           select new DynamicRecord
                           {
                               [Hubs.Tag.ProjectId] = Mg.Get<IMgContext>().ProjectId,
                               [Hubs.Tag.Name] = tag
                           }).ToList();
            await DataApi.AddAsync(Hubs.Tag.T, tagData.AsArray<IRecord>());
            var addRecord = createResult.Data.AsList<DynamicRecord>().FirstOrDefault();
            await Mg.Get<IMgWeb>().SendWebMsgAsync("demoPostil/add", addRecord?.Id.ToString(), BoardcastType.ProjectGroup);//发送广播
            //Close();
        }

        public RelayCommand CancleAddPostil { get; private set; }

        private bool CanCancleAddPostil()
        {
            return true;
        }

        private void OnCancleAddPostil()
        {
            Close();
        }

        #endregion 命令

        #region 重载

        protected override void OnViewLoaded(bool isFirstLoaded)
        {
            base.OnViewLoaded(isFirstLoaded);
            if (!isFirstLoaded)
                return;
            if (!GetCameraMatrix())
                Close();
            if (!GetGraphicPlatformCaptureScreen())
                Close();
            LoadPostilTagData();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "PostilTag")
            {
                if (_changeFlag)
                {
                    _changeFlag = false;
                    return;
                }
                var strArray = PostilTag.Split('，', ',').ToArray();
                var set = new HashSet<string>();
                foreach (var t in strArray)
                    set.Add(t.Trim());
                foreach (var tagModel in PostilTagList)
                {
                    if (set.Contains(tagModel.Name.Trim()))
                    {
                        if (tagModel.TagIsChecked)
                            continue;
                        _changeFlag = true;
                        tagModel.TagIsChecked = true;
                    }
                    else
                    {
                        if (!tagModel.TagIsChecked)
                            continue;
                        _changeFlag = true;
                        tagModel.TagIsChecked = false;
                    }
                }
            }
        }

        protected override string GetErrorFor(string propertyName)
        {
            if (propertyName == "PostilTitle")
            {
                if (string.IsNullOrWhiteSpace(PostilTitle))
                    return "标题不能为空！";
                if (PostilTitle.Length > 20)
                    return "标题不能超过20个汉字！";
            }
            if (propertyName == "PostilTag")
            {
                if (string.IsNullOrWhiteSpace(PostilTag))
                    return "标签不能为空！";
                var inputTagList = PostilTag.Split('，', ',').ToList().Select(t => t.Trim()).ToList();
                if (inputTagList.Any(tag => tag.Length > 8))
                    return "每个小标签不能超过8个汉字！";
                if (inputTagList.Count > 4)
                    return "最多只能添加4个小标签！";
            }
            return base.GetErrorFor(propertyName);
        }

        #endregion 重载

        #region 其他方法

        /// <summary>
        /// 获取图形平台的截图
        /// </summary>
        private bool GetGraphicPlatformCaptureScreen()
        {
            var tempDir = this.GetAppResPath("PostilFile\\");
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
            _screenshotPicPath = tempDir + DateTime.Now.ToString("yyyyMMddHHmmss") + ".JPG";
            if (File.Exists(_screenshotPicPath))
                File.Delete(_screenshotPicPath);
            var r = Mg.Get<IMgService>().Invoke<PostilApp>("GraphicPlatform:CaptureScreen", _screenshotPicPath);//传入图片需要保存的路径
            if (!r.IsOk)
            {
                Mg.Get<IMgLog>().Error("获取图形平台截图失败！无法调用图形管理平台服务，需打开图形管理平台模块！");
                Mg.Get<IMgDialog>().ShowDesktopAlert("获取图形平台截图失败", "无法调用图形管理平台服务，需打开图形管理平台模块！");
                return false;
            }
            if (File.Exists(_screenshotPicPath))
            {
                ViewPicture = this.GetAppImageSource(_screenshotPicPath);
                return true;
            }
            Mg.Get<IMgLog>().Error("获取图形平台截图失败！");
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "获取图形平台截图失败！");
            return false;
        }

        /// <summary>
        /// 获取图形平台截图中摄像机位置的数据
        /// </summary>
        private bool GetCameraMatrix()
        {
            var r = Mg.Get<IMgService>().Invoke<PostilApp>("GraphicPlatform:GetCameraMatrix");
            if (!r.IsOk)
            {
                Mg.Get<IMgLog>().Error("获取图形平台照相机的位置数据失败！无法调用图形管理平台服务，需打开图形管理平台模块！");
                Mg.Get<IMgDialog>().ShowDesktopAlert("获取图形平台照相机的位置数据失败", "无法调用图形管理平台服务，需打开图形管理平台模块！");
                return false;
            }
            _matrixArray = r.Data.As<float[]>();
            return _matrixArray != null && _matrixArray.Length == 16;
        }

        /// <summary>
        /// 加载批注标签数据
        /// </summary>
        private async void LoadPostilTagData()
        {
            PostilTagList.Clear();
            var listParams = new ListParams { Search = Query.Eq(Hubs.Tag.ProjectId, Mg.Get<IMgContext>().ProjectId) };
            var result = await DataApi.GetListAsync(Hubs.Tag.T, listParams);
            if (!result.IsOk)
                return;
            var records = result.GetRecords();
            foreach (var record in records)
            {
                var name = record[Hubs.Tag.Name].As<string>();
                var model = new TagInfoModel
                {
                    Id = record.Id,
                    Name = $" {name} "
                };
                PostilTagList.Add(model);
            }
        }

        /// <summary>
        /// 删除标签事件
        /// </summary>
        /// <param name="model"></param>
        public async void OnDeleteTag(TagInfoModel model)
        {
            //var showResult = this.ShowMessage("确定删除[" + model.Name + "]标签吗？", "提示", MessageBoxButton.OKCancel);
            //if (showResult == MessageBoxResult.Cancel)
            //    return;
            var result = await DataApi.DeleteAsync(Hubs.Tag.T, new[] { model.Id });
            if (result.IsOk)
                PostilTagList.Remove(model);
        }

        /// <summary>
        /// 点击标签的事件
        /// </summary>
        /// <param name="model"></param>
        private void OnIsCheckedTag(TagInfoModel model)
        {
            var tagName = model.Name.Trim();
            if (model.TagIsChecked)
            {
                if (_changeFlag)
                {
                    _changeFlag = false;
                    return;
                }
                _changeFlag = true;
                if (string.IsNullOrEmpty(PostilTag))
                    PostilTag = tagName;
                else
                    PostilTag += "，" + tagName;
            }
            else
            {
                if (_changeFlag)
                {
                    _changeFlag = false;
                    return;
                }
                if (string.IsNullOrEmpty(PostilTag)) return;
                _changeFlag = true;
                var index = PostilTag.IndexOf(tagName, StringComparison.Ordinal);
                switch (index)
                {
                    case -1:
                        break;

                    case 0:
                        PostilTag = PostilTag.Remove(index, tagName.Length);
                        var i = PostilTag.IndexOf('，');
                        if (i != -1)
                            PostilTag = PostilTag.Remove(i, 1);
                        break;

                    default:
                        var name = "，" + tagName;
                        PostilTag = PostilTag.Remove(index - 1, name.Length);
                        break;
                }
            }
        }

        #endregion 其他方法
    }
}