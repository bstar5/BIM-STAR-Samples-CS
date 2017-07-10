using System.Threading.Tasks;
using Mango;

namespace DemoApp.Apis
{
    public static class CustomDataApi
    {
        private static readonly IMgWeb Web = Mg.Get<IMgWeb>();

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="fieldNames">属性名(字段名)集合</param>
        /// <param name="isUnique">是否是唯一键</param>
        /// <returns></returns>
        public static async Task<Result> CreateIndexAsync(string name, string[] fieldNames, bool isUnique = false)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/create_index", new
            {
                Name = name,
                FieldNames = fieldNames,
                IsUnique = isUnique
            });
            return result;
        }

        /// <summary>
        /// 获取自定义数据列表（筛选、分页、排序、映射）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="listParams">集合的筛选、分页、排序、映射参数</param>
        /// <returns></returns>
        public static async Task<Result> GetCustomDataListAsync(string name, ListParams listParams = null)
        {
            var result = await Web.GetResultAsync<DemoApp>("v1/data/list", new
            {
                Name = name,
                ListParams = listParams
            });
            return result;
        }

        /// <summary>
        /// 获取自定义数据详情（映射）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="id">自定义数据的Id</param>
        /// <param name="map">返回的属性映射数组</param>
        /// <returns></returns>
        public static async Task<Result> GetCustomDataInfoAsync(string name, Id id, string[] map = null)
        {
            var result = await Web.GetResultAsync<DemoApp>("v1/data/info", new
            {
                Name = name,
                Id = id,
                Map = map
            });
            return result;
        }

        /// <summary>
        /// 添加自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="data">自定义数据内容</param>
        /// <returns></returns>
        public static async Task<Result> AddCustomDataAsync(string name, IRecord[] data)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/add", new
            {
                Name = name,
                Data = data
            });
            return result;
        }

        /// <summary>
        /// 删除自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="ids">自定义数据的Id集合</param>
        /// <returns></returns>
        public static async Task<Result> DeleteCustomDataAsync(string name, Id[] ids)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/delete", new
            {
                Name = name,
                Ids = ids
            });
            return result;
        }

        /// <summary>
        /// 修改自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称(表名)</param>
        /// <param name="data">修改的自定义数据内容集合</param>
        /// <returns></returns>
        public static async Task<Result> UpdateCustomDataAsync(string name, IRecord[] data)
        {
            var result = await Web.PostAsync<DemoApp>("v1/data/update", new
            {
                Name = name,
                Data = data
            });
            return result;
        }
    }
}