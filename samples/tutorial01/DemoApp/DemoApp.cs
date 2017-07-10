using System.Threading.Tasks;
using Mango;

namespace DemoApp
{
    public class DemoApp : App
    {
        protected override async Task OnStartupAsync()
        {
            //加载应用时，执行的代码
            this.ShowMessage("我的应用启动了！");//弹框
            await Task.Yield();
        }

        protected override void OnExited()
        {
            //卸载应用时，执行的代码
            this.ShowMessage("我的应用卸载了！");//弹框
        }
    }
}