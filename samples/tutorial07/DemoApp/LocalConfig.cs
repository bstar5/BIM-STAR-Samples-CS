using System.Xml.Linq;
using Mango;

namespace DemoApp
{
    public static class LocalConfig
    {
        static LocalConfig()
        {
            var root = XDocument.Load(typeof(LocalConfig).GetAppResPath("_config.xml")).Root;
            InsertGroupName = root.Attribute("InsertGroupName").Value;
        }

        public static string InsertGroupName { get; }
    }
}