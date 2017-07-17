using System.Collections.Generic;
using System.Web.Routing;

namespace Template.Engine.MVC
{
    public static class HtmlAttributesExtensions
    {
        public static IDictionary<string, object> AddHtmlAttribute(this object obj, string name, object value, bool condition)
        {
            var items = !condition ? new RouteValueDictionary(obj) : new RouteValueDictionary(obj) { { name, value } };
            return UnderlineToDashInDictionaryKeys(items);
        }

        public static IDictionary<string, object> AddHtmlAttribute(this IDictionary<string, object> dictSource, string name, object value, bool condition)
        {
            if (!condition)
                return dictSource;

            dictSource.Add(name, value);
            return UnderlineToDashInDictionaryKeys(dictSource);
        }

        private static IDictionary<string, object> UnderlineToDashInDictionaryKeys(IDictionary<string, object> items)
        {
            var newItems = new RouteValueDictionary();
            foreach (var item in items)
            {
                newItems.Add(item.Key.Replace("_", "-"), item.Value);
            }
            return newItems;
        }
    }
}
