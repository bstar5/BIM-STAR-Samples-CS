using Mango;

namespace PostilApp.Models
{
    public class TagInfoModel : NotifyBase
    {
        private Id _id;

        /// <summary>
        /// 标签的Id
        /// </summary>
        public Id Id
        {
            get { return _id; }
            set { Set("Id", ref _id, value); }
        }

        private string _name;

        /// <summary>
        /// 标签名字
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        private bool _tagIsChecked;

        /// <summary>
        /// 获取标签选中的状态
        /// </summary>
        public bool TagIsChecked
        {
            get { return _tagIsChecked; }
            set { Set("TagIsChecked", ref _tagIsChecked, value); }
        }

        private bool _changeFlag;

        /// <summary>
        /// 获取ChangeFlag
        /// </summary>
        public bool ChangeFlag
        {
            get { return _changeFlag; }
            set { Set("ChangeFlag", ref _changeFlag, value); }
        }
    }
}