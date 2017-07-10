using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mango;

namespace PostilApp.Apis
{
    public static class DataApi
    {
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="name">数据集名称</param>
        /// <param name="fieldNames">属性名集合</param>
        /// <param name="isUnique">是否是唯一键</param>
        /// <returns></returns>
        public static async Task<Result> CreateIndexAsync(string name, string[] fieldNames, bool isUnique = false)
        {
            var result = await Mg.Get<IMgWeb>().PostAsync<PostilApp>("v1/data/create_index", new
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
        /// <param name="name">数据集名称</param>
        /// <param name="listParams">集合的筛选、分页、排序、映射参数</param>
        /// <returns></returns>
        public static async Task<Result> GetListAsync(string name, ListParams listParams = null)
        {
            var result = await Mg.Get<IMgWeb>().GetResultAsync<PostilApp>("v1/data/list", new
            {
                Name = name,
                ListParams = listParams
            });
            return result;
        }

        /// <summary>
        /// 获取自定义数据详情
        /// </summary>
        /// <param name="name">数据集名称</param>
        /// <param name="id">自定义数据的Id</param>
        /// <param name="map">返回的属性映射集合，如果为空，则返回所有属性</param>
        /// <returns></returns>
        public static async Task<Result> GetDataInfoAsync(string name, Id id, string[] map = null)
        {
            var result = await Mg.Get<IMgWeb>().GetResultAsync<PostilApp>("v1/data/info", new
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
        /// <param name="name">数据集名称</param>
        /// <param name="data">自定义数据内容，数组格式</param>
        /// <returns></returns>
        public static async Task<Result> AddAsync(string name, IRecord[] data)
        {
            var result = await Mg.Get<IMgWeb>().PostAsync<PostilApp>("v1/data/add", new
            {
                Name = name,
                Data = data
            });
            return result;
        }

        /// <summary>
        /// 删除自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称</param>
        /// <param name="ids">自定义数据的Id集合</param>
        /// <returns></returns>
        public static async Task<Result> DeleteAsync(string name, Id[] ids)
        {
            var result = await Mg.Get<IMgWeb>().PostAsync<PostilApp>("v1/data/delete", new
            {
                Name = name,
                Ids = ids
            });
            return result;
        }

        /// <summary>
        /// 修改自定义数据（批量）
        /// </summary>
        /// <param name="name">数据集名称</param>
        /// <param name="data">修改的自定义数据内容集合</param>
        /// <returns></returns>
        public static async Task<Result> UpdateAsync(string name, IRecord[] data)
        {
            var result = await Mg.Get<IMgWeb>().PostAsync<PostilApp>("v1/data/update", new
            {
                Name = name,
                Data = data
            });
            return result;
        }

        /// <summary>
        /// 上传文件（批量）
        /// </summary>
        /// <param name="filePaths">文件路径</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public static async Task<Result> UploadFilesAsync(string[] filePaths, Id projectId = null)
        {
            var streamInfos = new List<StreamInfo>();
            foreach (var filePath in filePaths)
            {
                if (!File.Exists(filePath))
                    continue;
                var fileName = Path.GetFileName(filePath);
                var name = Path.GetFileNameWithoutExtension(filePath);
                using (var stream = new FileStream(filePath, FileMode.Open))
                    streamInfos.Add(new StreamInfo { FileName = fileName, Name = name, Stream = stream });
            }
            var result = await Mg.Get<IMgWeb>().PostAsync<PostilApp>("v1/data/upload_files", new
            {
                ProjectId = projectId
            }, streamInfos.ToArray());
            return result;
        }

        /// <summary>
        /// 删除文件（批量）
        /// </summary>
        /// <param name="ids">需要删除的文件Id集合</param>
        /// <returns></returns>
        public static async Task<Result> DeleteFilesAsync(Id[] ids)
        {
            var result = await Mg.Get<IMgWeb>().PostAsync<PostilApp>("v1/data/delete_files", new
            {
                Ids = ids
            });
            return result;
        }

        /// <summary>
        /// 获取文件URL（批量）
        /// </summary>
        /// <param name="ids">需要获取URL的文件Id集合</param>
        /// <returns></returns>
        public static async Task<Result> GetFilesUrlAsync(Id[] ids)
        {
            var result = await Mg.Get<IMgWeb>().GetResultAsync<PostilApp>("v1/data/get_files_url", new
            {
                Ids = ids
            });
            return result;
        }
    }
}