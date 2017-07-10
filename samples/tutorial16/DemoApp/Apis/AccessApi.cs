using System.Threading.Tasks;
using Mango;

namespace DemoApp.Apis
{
    public class AccessApi
    {
        private static readonly IMgWeb Web = Mg.Get<IMgWeb>();

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="moduleKeys">The module keys.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>Task&lt;Result&gt;.</returns>
        public static async Task<Result> GetPrivilegesAsync(string[] moduleKeys, Id projectId)
        {
            var result = await Web.GetResultAsync<DemoApp>("v1/access/privilege", new
            {
                ModuleKeys = moduleKeys,
                ProjectId = projectId
            });
            return result;
        }
    }
}