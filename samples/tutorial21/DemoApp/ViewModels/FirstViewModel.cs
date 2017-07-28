using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using DemoApp.Models;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private float[] _cameraMatrix;
        private uint _graphicPlatformImageKey;
        private readonly ThumbtackMgnt _mgnt;

        public FirstViewModel()
        {
            _mgnt = new ThumbtackMgnt();
            CaptureScreen = new RelayCommand(OnCaptureScreen, CanCaptureScreen);
            GetCameraMatrix = new RelayCommand(OnGetCameraMatrix, CanGetCameraMatrix);
            SetCameraMatrix = new RelayCommand(OnSetCameraMatrix, CanSetCameraMatrix);
            InsertImage = new RelayCommand(OnInsertImage, CanInsertImage);
            RemoveImage = new RelayCommand(OnRemoveImage, CanRemoveImage);
            LocateNodes = new RelayCommand(OnLocateNodes, CanLocateNodes);
            OutstandSelectedNodes = new RelayCommand(OnOutstandSelectedNodes, CanOutstandSelectedNodes);
            PauseRender = new RelayCommand(OnPauseRender, CanPauseRender);
            SelectNodes = new RelayCommand(OnSelectNodes, CanSelectNodes);
            SelectPoint = new RelayCommand(OnSelectPoint, CanSelectPoint);
            GoToRemindMode = new RelayCommand(OnGoToRemindMode, CanGoToRemindMode);
            GoToDefaultMode = new RelayCommand(OnGoToDefaultMode, CanGoToDefaultMode);
            GetTargetCenter = new RelayCommand(OnGetTargetCenter, CanGetTargetCenter);
            ShowThumbtack = new RelayCommand(OnShowThumbtack, CanShowThumbtack);
            RemoveThumbtack = new RelayCommand(OnRemoveThumbtack, CanRemoveThumbtack);
        }

        #region 截图

        public RelayCommand CaptureScreen { get; private set; }

        private bool CanCaptureScreen()
        {
            return true;
        }

        private async void OnCaptureScreen()
        {
            var filePath = "D://123.jpg";
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:CaptureScreen", filePath);
            await Task.Delay(200);
            if (File.Exists(filePath))
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"截图成功，图片保存路径为:{filePath}。");
        }

        #endregion 截图

        #region 获取摄像机位置矩阵

        public RelayCommand GetCameraMatrix { get; private set; }

        private bool CanGetCameraMatrix()
        {
            return true;
        }

        private void OnGetCameraMatrix()
        {
            var result = Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:GetCameraMatrix");
            if (result.IsOk)
            {
                var data = result.Data.As<float[]>();
                _cameraMatrix = data;
                var str = data.Aggregate("", (current, t) => current + (t + ",")).TrimEnd(',');
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"摄像机位置矩阵为:{str}。");
            }
            else
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", result.Message);
        }

        #endregion 获取摄像机位置矩阵

        #region 设置摄像机位置矩阵

        public RelayCommand SetCameraMatrix { get; private set; }

        private bool CanSetCameraMatrix()
        {
            return _cameraMatrix != null && _cameraMatrix.Length == 16;
        }

        private void OnSetCameraMatrix()
        {
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:SetCameraMatrix", _cameraMatrix);
        }

        #endregion 设置摄像机位置矩阵

        #region 插入图片

        public RelayCommand InsertImage { get; private set; }

        private bool CanInsertImage()
        {
            return true;
        }

        private void OnInsertImage()
        {
            var param = new
            {
                ImagePath = this.GetAppResPath("Assets\\logo.jpg"), //string:	图片路径，必填*
                HorizontalAlignment = "Left", //string:	水平对齐，取值为：Left;Center;Right 默认为"Left"
                VerticalAlignment = "Top", //string:	竖向对齐，取值为：Top;Center;Bottom 默认为"Top"
                Left = 0f, //float:	左边距，默认为0f
                Right = 0f, //float:	右边距，默认为0f
                Top = 0f, //float:	上边距，默认为0f
                Bottom = 0f, //float:	下边距，默认为0f
                Width = float.NaN, //float:	图片宽度，默认为float.NaN，表示图片的像素宽度
                Height = float.NaN, //float:	图片高度，默认为float.NaN，表示图片的像素高度
                Opacity = 1f //float: 	图片透明度，默认为1f
            };
            var result = Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:InsertImage", param);
            if (result.IsOk)
                _graphicPlatformImageKey = result.Data.As<uint>(); //图片标识
            else
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", result.Message);
        }

        #endregion 插入图片

        #region 移除图片

        public RelayCommand RemoveImage { get; private set; }

        private bool CanRemoveImage()
        {
            return _graphicPlatformImageKey != 0;
        }

        private void OnRemoveImage()
        {
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:RemoveImage", _graphicPlatformImageKey);
        }

        #endregion 移除图片

        #region 定位到指定节点

        public RelayCommand LocateNodes { get; private set; }

        private bool CanLocateNodes()
        {
            return true;
        }

        private void OnLocateNodes()
        {
            var selNodes = Mg.Get<IMgScene>().SelectedNodes;
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:LocateNodes", selNodes);
        }

        #endregion 定位到指定节点

        #region 突出显示选中的节点

        public RelayCommand OutstandSelectedNodes { get; private set; }

        private bool CanOutstandSelectedNodes()
        {
            return true;
        }

        private void OnOutstandSelectedNodes()
        {
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:OutstandSelectedNodes");
        }

        #endregion 突出显示选中的节点

        #region 让图形平台暂停渲染，当有鼠标操作时，图形平台会自动恢复渲染

        public RelayCommand PauseRender { get; private set; }

        private bool CanPauseRender()
        {
            return true;
        }

        private void OnPauseRender()
        {
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:PauseRender");
        }

        #endregion 让图形平台暂停渲染，当有鼠标操作时，图形平台会自动恢复渲染

        #region 进入到选择构件模式

        public RelayCommand SelectNodes { get; private set; }

        private bool CanSelectNodes()
        {
            return true;
        }

        private async void OnSelectNodes()
        {
            IEnumerable<ISpatialNode> selNodes = null;
            var result = await Mg.Get<IMgService>().InvokeAsync<DemoApp>("GraphicPlatform:SelectNodes");
            if (result.IsOk)
                selNodes = result.Data.AsArray<ISpatialNode>();
        }

        #endregion 进入到选择构件模式

        #region 进入到选择点模式

        public RelayCommand SelectPoint { get; private set; }

        private bool CanSelectPoint()
        {
            return true;
        }

        private async void OnSelectPoint()
        {
            Tuple<ISpatialNode, Point3D> tuple = null;
            var result = await Mg.Get<IMgService>().InvokeAsync<DemoApp>("GraphicPlatform:SelectPoint", null);
            if (result.IsOk)
                tuple = result.Data.As<Tuple<ISpatialNode, Point3D>>();
        }

        #endregion 进入到选择点模式

        #region 进入到提醒模式

        public RelayCommand GoToRemindMode { get; private set; }

        private bool CanGoToRemindMode()
        {
            return true;
        }

        private void OnGoToRemindMode()
        {
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:GoToRemindMode");
        }

        #endregion 进入到提醒模式

        #region 进入到默认模式

        public RelayCommand GoToDefaultMode { get; private set; }

        private bool CanGoToDefaultMode()
        {
            return true;
        }

        private void OnGoToDefaultMode()
        {
            Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:GoToDefaultMode");
        }

        #endregion 进入到默认模式

        #region 获取图形平台的目标点

        public RelayCommand GetTargetCenter { get; private set; }

        private bool CanGetTargetCenter()
        {
            return true;
        }

        private void OnGetTargetCenter()
        {
            var result = Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:GetTargetCenter");
            if (result.IsOk)
            {
                var pos = result.Data.As<Point3D>();
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"目标点的X:{pos.X},Y:{pos.Y},Z:{pos.Z}");
                _mgnt.ThumbtackInfoList.Add(new ThumbtackInfo
                {
                    X = pos.X.As<float>(),
                    Y = pos.Y.As<float>(),
                    Z = pos.Z.As<float>(),
                    Key = Guid.NewGuid().ToString(),
                    ImagePath = this.GetAppResPath("Assets/safety.png")
                });
            }
            else
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", result.Message);
        }

        #endregion 获取图形平台的目标点

        #region 显示图钉

        public RelayCommand ShowThumbtack { get; private set; }

        private bool CanShowThumbtack()
        {
            return true;
        }

        private void OnShowThumbtack()
        {
            if (_mgnt.ThumbtackInfoList.Count == 0)
            {
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "请先获取目标点的坐标！");
                return;
            }
            foreach (var thumbtackInfo in _mgnt.ThumbtackInfoList)
                _mgnt.ShowThumbtack(thumbtackInfo);
        }

        #endregion 显示图钉

        #region 移除图钉

        public RelayCommand RemoveThumbtack { get; private set; }

        private bool CanRemoveThumbtack()
        {
            return true;
        }

        private void OnRemoveThumbtack()
        {
            foreach (var thumbtackInfo in _mgnt.ThumbtackInfoList)
                _mgnt.RemoveThumbtack(thumbtackInfo);
        }

        #endregion 移除图钉
    }
}