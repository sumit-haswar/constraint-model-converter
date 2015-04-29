using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Converter.Core
{
    public static class Extension
    {
        public static bool IsEqual(this string text, string value)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            else
            {
                return text.ToLower().Equals(value.ToLower());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this XElement xElement, string attribute)
        {
            if (string.IsNullOrWhiteSpace(attribute) == false && xElement.Attribute(attribute) != null)
            {
                return xElement.Attribute(attribute).Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool IsAttributeValue(this XElement xElement, string attribute, string value)
        {
            if (string.IsNullOrWhiteSpace(attribute) == false 
                && xElement.Attribute(attribute) != null 
                && xElement.Attribute(attribute).Value == value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }
    }
}
