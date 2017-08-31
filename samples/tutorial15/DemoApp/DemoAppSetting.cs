using Mango;

namespace DemoApp
{
    public class DemoAppSetting : SettingBase
    {
        /// <summary>
        /// 全局唯一对象
        /// </summary>
        public static DemoAppSetting Ins => Get<DemoAppSetting, DemoApp>();

        private DemoAppSetting(App app) : base(app)
        {
            SetDefaut("AlwaysShow", true);//设置默认值
            SetDefaut("Name", "深圳筑星科技有限公司");//设置默认值
        }

        public bool AlwaysShow
        {
            get { return Get<bool>("AlwaysShow"); }
            set { Set("AlwaysShow", value); }
        }

        public string Name
        {
            get { return Get<string>("Name"); }
            set { Set("Name", value); }
        }
    }
}