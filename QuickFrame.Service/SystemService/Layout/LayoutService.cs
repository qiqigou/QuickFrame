using QuickFrame.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuickFrame.Service
{
    /// <summary>
    /// 布局服务(提供列字段,描述,主键等信息)
    /// </summary>
    public class LayoutService : ILayoutService
    {
        private readonly ICache _cache;

        public LayoutService(ICache cache)
        {
            _cache = cache;
        }
        /// <summary>
        /// 获取Xml
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<MemberItem>> GetXElementAsync()
        {
            var result = await _cache.GetAsync<MemberItem[]>(CacheKey.ModelMemberItems);
            if (result == default)
            {
                var modelPath = Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.ModelName}.xml");
                var commonPath = Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.CommonName}.xml");
                var servicePath = Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.ServiceName}.xml");
                var controllerPath = Path.Combine(AppContext.BaseDirectory, $"{AssemblyOption.ControllersName}.xml");

                var modelXml = await LoadXmlAsync(modelPath);
                var commonXml = await LoadXmlAsync(commonPath);
                var serviceXml = await LoadXmlAsync(servicePath);
                var controllerXml = await LoadXmlAsync(controllerPath);

                var modelItems = SelectMemberItem(modelXml);
                var commonItems = SelectMemberItem(commonXml);
                var serviceItems = SelectMemberItem(serviceXml);
                var controllerItems = SelectMemberItem(controllerXml);

                result = modelItems.Concat(commonItems).Concat(serviceItems).Concat(controllerItems).ToArray();
                if (result.Length > 0)
                {
                    await _cache.SetAsync(CacheKey.ModelMemberItems, result);
                }
            }
            return result;

            static IEnumerable<MemberItem> SelectMemberItem(XElement xml)
            {
                var members = xml.Element("members")?.Elements("member");
                if (members == default) return Array.Empty<MemberItem>();
                return members.Select(x => new MemberItem
                {
                    Name = x?.Attribute("name")?.Value ?? string.Empty,
                    Param = x?.Elements("param")?.Select(m => new NameValueItem<string> { Name = m?.Attribute("name")?.Value ?? string.Empty, Value = m?.Value.Trim() ?? string.Empty }).ToArray() ?? Array.Empty<NameValueItem<string>>(),
                    Returns = x?.Element("returns")?.Value.Trim() ?? string.Empty,
                    Summary = x?.Element("summary")?.Value.Trim() ?? string.Empty,
                    TypeParam = x?.Elements("typeparam")?.Select(m => new NameValueItem<string> { Name = m?.Attribute("name")?.Value ?? string.Empty, Value = m?.Value.Trim() ?? string.Empty }).ToArray() ?? Array.Empty<NameValueItem<string>>(),
                });
            }
            static async Task<XElement> LoadXmlAsync(string path)
            {
                using var streamWriter = new StreamReader(path);
                return await XElement.LoadAsync(streamWriter, LoadOptions.None, TaskCancelOption.SimpleTask.Token);
            }
        }
        /// <summary>
        /// 查询视图字段名和描述
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ColumnItemOutput>> QueryViewColumnsAsync(string viewName)
        {
            var columns = await _cache.GetAsync<ColumnItemOutput[]>(string.Format(CacheKey.ColumnItems, viewName));
            if (columns == default)
            {
                var viewFullName = $"P:{AssemblyOption.ModelName}.{viewName}.";
                var members = await GetXElementAsync();
                var array = members.Where(x => x.Name.StartsWith(viewFullName)).ToArray();
                columns = array.Select(x => new ColumnItemOutput
                {
                    Desc = x.Summary,
                    Name = x.Name[viewFullName.Length..]
                }).ToArray();
                if (columns.Length > 0)
                {
                    await _cache.SetAsync(viewFullName, columns);
                }
            }
            return columns;
        }
        /// <summary>
        /// 获取全部视图
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ViewItemOutput>> QueryViewsAsync()
        {
            var views = await _cache.GetAsync<ViewItemOutput[]>(CacheKey.ViewNames);
            if (views == default)
            {
                var viewKey = $"T:{AssemblyOption.ModelName}.";
                var members = await GetXElementAsync();
                var array = members.Where(x => x.Name.StartsWith(viewKey)).ToArray();
                views = array.Select(x => new ViewItemOutput
                {
                    Desc = x.Summary,
                    Name = x.Name[viewKey.Length..],
                }).ToArray();
                if (views.Length > 0)
                {
                    await _cache.SetAsync(CacheKey.ViewNames, views);
                }
            }
            return views;
        }
    }
}
