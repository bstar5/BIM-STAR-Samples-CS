using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Models;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            UserSource = new ObservableCollection<User>
            {
                new User{Number = "1",Name="小茗",Remark="-"},
                new User{Number = "2",Name="冷冷",Remark="-"},
                new User{Number = "3",Name="暖暖",Remark="-"}
            };
            AddStudent = new AsyncCommand(OnAddStudent, CanAddStudentAsync);
            ModifyStudent = new AsyncCommand(OnModifyStudent, CanModifyStudentAsync);
            DeleteStudent = new AsyncCommand(OnDeleteStudentAsync, CanDeleteStudent);
        }

        private ObservableCollection<User> _userSource;

        /// <summary>
        /// RadGridView的数据源
        /// </summary>
        public ObservableCollection<User> UserSource
        {
            get { return _userSource; }
            set { Set("UserSource", ref _userSource, value); }
        }

        private User _selectedUserItem;

        /// <summary>
        /// 获取或设置SelectedUserItem属性
        /// </summary>
        public User SelectedUserItem
        {
            get { return _selectedUserItem; }
            set { Set("SelectedUserItem", ref _selectedUserItem, value); }
        }

        /// <summary>
        /// 增加一个学生
        /// </summary>
        public AsyncCommand AddStudent { get; private set; }

        private async Task<bool> CanAddStudentAsync()
        {
            return await Mg.Get<IMgContext>().GetPrivilegeAsync(ModuleKeys.Create);
        }

        private void OnAddStudent()
        {
            var num = (UserSource.Count + 1).As<string>();
            UserSource.Add(new User
            {
                Number = num,
                Name = $"小{num}",
                Remark = "新增的"
            });
        }

        /// <summary>
        /// 修改最后一个学生的备注
        /// </summary>
        public AsyncCommand ModifyStudent { get; private set; }

        private async Task<bool> CanModifyStudentAsync()
        {
            return await Mg.Get<IMgContext>().GetPrivilegeAsync(ModuleKeys.Modify);
        }

        private void OnModifyStudent()
        {
            if (UserSource.Count == 0)
                return;
            var lastStudent = UserSource.Last();
            lastStudent.Remark = $"该同学于{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}被修改了备注";
        }

        public AsyncCommand DeleteStudent { get; private set; }

        private async Task<bool> CanDeleteStudent()
        {
            return await Mg.Get<IMgContext>().GetPrivilegeAsync(ModuleKeys.Delete);
        }

        private void OnDeleteStudentAsync()
        {
            if (UserSource.Count == 0)
                return;
            var lastStudent = UserSource.Last();
            UserSource.Remove(lastStudent);
        }
    }
}