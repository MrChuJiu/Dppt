using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Dppt.Authorization.Abstractions.Permissions.PermissionValue;
using Dppt.Core.Dppt.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dppt.Authorization.Abstractions.Permissions
{
    public class NullPermissionStore : IPermissionStore
    {
        public ILogger<NullPermissionStore> Logger { get; set; }

        public NullPermissionStore()
        {
            Logger = NullLogger<NullPermissionStore>.Instance;
        }

        public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return TaskCache.FalseResult;
        }
    }
}
