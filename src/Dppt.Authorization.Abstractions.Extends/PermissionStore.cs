using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Microsoft.Extensions.Logging;

namespace Dppt.Authorization.Abstractions.Extends
{
    public class PermissionStore : IPermissionStore
    {

        private static Dictionary<string, PermissionGrantCacheItem> CacheDictionary = new Dictionary<string, PermissionGrantCacheItem>();
        public ILogger<PermissionStore> Logger { get; set; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }




        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
        }
        protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(
            string name,
            string providerName,
            string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);

            Logger.LogDebug($"PermissionStore.GetCacheItemAsync: {cacheKey}");

            var cacheItem = CacheDictionary[cacheKey];

            if (cacheItem != null)
            {
                Logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem;
            }

            Logger.LogDebug($"Not found in the cache: {cacheKey}");

            cacheItem = new PermissionGrantCacheItem(false);

            await SetCacheItemsAsync(providerName, providerKey, name, cacheItem);

            return cacheItem;
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }

        protected virtual async Task SetCacheItemsAsync(
            string providerName,
            string providerKey,
            string currentName,
            PermissionGrantCacheItem currentCacheItem)
        {
            var permissions = PermissionDefinitionManager.GetPermissions();

            Logger.LogDebug($"Getting all granted permissions from the repository for this provider name,key: {providerName},{providerKey}");

            var grantedPermissionsHashSet = new HashSet<string>(
                (await PermissionGrantRepository.GetListAsync(providerName, providerKey)).Select(p => p.Name)
            );

            Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

            var cacheItems = new List<KeyValuePair<string, PermissionGrantCacheItem>>();

            foreach (var permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

                cacheItems.Add(new KeyValuePair<string, PermissionGrantCacheItem>(
                    CalculateCacheKey(permission.Name, providerName, providerKey),
                    new PermissionGrantCacheItem(isGranted))
                );

                if (permission.Name == currentName)
                {
                    currentCacheItem.IsGranted = isGranted;
                }
            }

            foreach (var item in cacheItems)
            {
                CacheDictionary.Add(item.Key, item.Value);
            }

            Logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");
        }

    }
}
