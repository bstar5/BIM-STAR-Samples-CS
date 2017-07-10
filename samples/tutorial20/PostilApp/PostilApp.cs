using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mango;
using System.Linq;
using System.Text;
using Mango.ViewModels;
using PostilApp.Apis;
using PostilApp.Entity;
using PostilApp.ViewModels.RibbonTabs;

namespace PostilApp
{
    public class PostilApp : App
    {
        private RibbonTabViewModel _projectTab;
        private PostilGroupViewModel _groupVm;

        protected override async Task OnStartupAsync()
        {
            //获取RibbonTab
            _projectTab = Mg.Get<IMgRibbon>().GetRibbonTab(LocalConfig.InsertTabName);
            if (_projectTab != null)
            {
                _groupVm = new PostilGroupViewModel();
                _projectTab.Groups.Add(_groupVm);
            }
            else
                Mg.Get<IMgLog>().Warn($"没有找到名称为{LocalConfig.InsertTabName}的RibbonTab！批注管理应用无法插入相关Ribbon菜单！");
            await DataApi.CreateIndexAsync(Hubs.Postil.T, new[] { Hubs.Postil.ProjectId });
            await DataApi.CreateIndexAsync(Hubs.Tag.T, new[] { Hubs.Postil.ProjectId });
            await Task.Yield();
        }

        protected override void OnExited()
        {
            if (_groupVm != null)
            {
                _projectTab.Groups.Remove(_groupVm);//移除批注的菜单组
                _groupVm = null;
            }
        }
    }
}