using System.Threading.Tasks;
using Mango;

namespace PostilApp.Apis
{
    public static class UserApi
    {
        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="listParams">集合的筛选、分页、排序、映射参数</param>
        /// <returns></returns>
        public static async Task<Result> GetUserInfoListAsync(ListParams listParams = null)
        {
            var result = await Mg.Get<IMgWeb>().GetResultAsync<PostilApp>("v1/user/list", new
            {
                ListParams = listParams
            });
            return result;
        }
    }
}