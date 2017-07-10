using System.Xml.Linq;
using Mango;

namespace PostilApp
{
    public static class LocalConfig
    {
        static LocalConfig()
        {
            var root = XDocument.Load(typeof(LocalConfig).GetAppResPath("_config.xml")).Root;
            InsertTabName = root?.Attribute("InsertTabName")?.Value;
        }

        public static string InsertTabName { get; }
    }
}