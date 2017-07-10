using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango;
using PostilApp.Apis;

namespace PostilApp.Util
{
    public static class Data
    {
        /// <summary>
        /// 加载图片数据
        /// </summary>
        public static async Task<Dictionary<Id, string>> GetPictureUrlsAsync(Id[] fileIds)
        {
            var result = await DataApi.GetFilesUrlAsync(fileIds);
            if (!result.IsOk)
            {
                Mg.Get<IMgLog>().Error("获取批注的图片数据失败" + result.Message);
                Mg.Get<IMgDialog>().ShowDesktopAlert("获取批注的图片数据失败", result.Message);
                return null;
            }
            return result.Data.As<Dictionary<Id, string>>();
        }

        /// <summary>
        /// 加载用户真实姓名数据
        /// </summary>
        public static async Task<Dictionary<Id, string>> GetUserInfosAsync(Id[] userIds)
        {
            var listParamas = new ListParams
            {
                Search = Query.In(Id.Name, userIds),
                Map = new[] { Id.Name, "RealName" }
            };
            var result = await UserApi.GetUserInfoListAsync(listParamas);
            if (!result.IsOk)
            {
                Mg.Get<IMgLog>().Error($"获取用户的头像数据失败：{result.Message}");
                Mg.Get<IMgDialog>().ShowDesktopAlert("获取用户的头像数据失败", result.Message);
                return null;
            }
            var items = result.GetRecords();
            return items.ToDictionary(item => item[Id.Name].As<Id>(), item => item["RealName"].As<string>());
        }
    }
}