using System;
using System.Threading.Tasks;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            OpenPropertyPane = new RelayCommand(OnOpenPropertyPane, CanOpenPropertyPane);
            RegisterService = new RelayCommand(OnRegisterService, CanRegisterService);
            DemoAppShow = new RelayCommand(OnDemoAppShow, CanDemoAppShow);
            DemoAppHide = new RelayCommand(OnDemoAppHide, CanDemoAppHide);
            RegisterSlot = new RelayCommand(OnRegisterSlot, CanRegisterSlot);
            RegisterPlug = new RelayCommand(OnRegisterPlug, CanRegisterPlug);
            ExecutePlugs = new RelayCommand(OnExecutePlugs, CanExecutePlugs);
        }

        /// <summary>
        /// 调用->属性应用的打开属性面板服务
        /// </summary>
        public RelayCommand OpenPropertyPane { get; private set; }

        private bool CanOpenPropertyPane()
        {
            return true;
        }

        private void OnOpenPropertyPane()
        {
            Mg.Get<IMgService>().Invoke("PropertyExplorer:Show");
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public RelayCommand RegisterService { get; private set; }

        private bool CanRegisterService()
        {
            return true;
        }

        private void OnRegisterService()
        {
            Mg.Get<IMgService>().Register(App.Instance<DemoApp>(),
                 //注册相关函数
                 new Service("DemoApp:Show", () =>
                 {
                     this.ShowMessage("您调用了DemoApp:Show服务！");
                 }),
                 new Service("DemoApp:Hide", () =>
                 {
                     this.ShowMessage("您调用了DemoApp:Hide服务！");
                 }));
        }

        public RelayCommand DemoAppShow { get; private set; }

        private bool CanDemoAppShow()
        {
            return true;
        }

        private async void OnDemoAppShow()
        {
            var exist = Mg.Get<IMgService>().IsExist("DemoApp:Show");
            if (exist)
            {
                var r = Mg.Get<IMgService>().Invoke("DemoApp:Show");
                if (!r.IsOk)
                {
                    Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "调用DemoApp:Show服务失败！");
                }
            }
            else
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "没有找到DemoApp:Show服务！");
            var rr = await Mg.Get<IMgService>().InvokeAsync("DemoApp:Show");
        }

        public RelayCommand DemoAppHide { get; private set; }

        private bool CanDemoAppHide()
        {
            return true;
        }

        private void OnDemoAppHide()
        {
            var exist = Mg.Get<IMgService>().IsExist("DemoApp:Hide");
            if (exist)
            {
                var r = Mg.Get<IMgService>().Invoke("DemoApp:Hide");
                if (!r.IsOk)
                {
                    Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "调用DemoApp:Hide服务失败！");
                }
            }
            else
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "没有找到DemoApp:Hide服务！");
        }

        /// <summary>
        /// 注册插槽
        /// </summary>
        public RelayCommand RegisterSlot { get; private set; }

        private bool CanRegisterSlot()
        {
            return true;
        }

        private void OnRegisterSlot()
        {
            //注册插槽
            Mg.Get<IMgSlot>().Register(App.Instance<DemoApp>(),
                       new Slot("DemoApp:Startup") { Description = "DemoApp应用启动时其它应用需要执行的操作" },
                       new Slot("DemoApp:Exited") { Description = "DemoApp应用卸载时其它应用需要执行的操作" });
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "注册了DemoApp:Startup和DemoApp:Exited两个插槽！");
        }

        /// <summary>
        /// 注册某个插槽里的插头
        /// </summary>
        public RelayCommand RegisterPlug { get; private set; }

        private bool CanRegisterPlug()
        {
            return true;
        }

        private void OnRegisterPlug()
        {
            var data = new Func<IRecord, Task>(PlugAction);
            Mg.Get<IMgSlot>().PushPlugs(App.Instance<DemoApp>(), new Plug("FirstPlug", "DemoApp:Startup", data));
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "注册了插槽DemoApp:Startup里的插头FirstPlug！");
        }

        private async Task PlugAction(IRecord data)
        {
            await Task.Delay(100);
            var who = data["Name"].As<string>();
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"Hi,{who}！");
        }

        /// <summary>
        /// 执行某个插槽里的插头
        /// </summary>
        public RelayCommand ExecutePlugs { get; private set; }

        private bool CanExecutePlugs()
        {
            return true;
        }

        private void OnExecutePlugs()
        {
            //获取插槽中所有插头
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
        }
    }
}