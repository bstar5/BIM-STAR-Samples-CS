using Mango;
using Mango.ViewModels;

namespace DemoApp.ViewModels.RibbonTabs
{
    public class ViewGroupViewModel : RibbonGroupViewModel
    {
        public ViewGroupViewModel()
            : base("RibbonTabs/ViewGroup.xml", "Assets")//第一个参数为xml文件的路径，第二个参数为xml文件里图片资源的路径
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("ShowWindow", OnShowWindow);//注册xml文件里的按钮点击事件
        }

        private void OnShowWindow()
        {
            var firstVm = new FirstViewModel();
            var vm = new DialogViewModel(firstVm)
            {
                Title = "窗口标题",
                Width = 500,
                Height = 500
            };
            var result = Mg.Get<IMgDialog>().ShowDialog(vm);
            if (result == CloseResult.Ok)
            {
                //点击窗口的确定按钮后需要执行的代码
            }
        }
    }
}