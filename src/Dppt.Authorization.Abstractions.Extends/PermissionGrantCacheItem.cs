using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Authorization.Abstractions.Extends
{
    [Serializable]
    public class PermissionGrantCacheItem
    {
        private const string CacheKeyFormat = "pn:{0},pk:{1},n:{2}";

        public bool IsGranted { get; set; }

        public PermissionGrantCacheItem()
        {

        }

        public PermissionGrantCacheItem(bool isGranted)
        {
            IsGranted = isGranted;
        }

        public static string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return string.Format(CacheKeyFormat, providerName, providerKey, name);
        }
    }
}
