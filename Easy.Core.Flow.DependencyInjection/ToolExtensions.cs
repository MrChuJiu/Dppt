using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Easy.Core.Flow.DependencyInjection
{
    public static class ToolExtensions
    {
        public static string Right(this string str, int len)
        {
            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }
    }
}
