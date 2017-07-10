using Mango;

namespace DemoApp.Models
{
    public class User : NotifyBase
    {
        private string _number;

        /// <summary>
        /// 获取或设置Number属性
        /// </summary>
        public string Number
        {
            get { return _number; }
            set { Set("Number", ref _number, value); }
        }

        private string _name;

        /// <summary>
        /// 获取或设置Name属性
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        private string _remark;

        /// <summary>
        /// 获取或设置Remark属性
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { Set("Remark", ref _remark, value); }
        }
    }
}