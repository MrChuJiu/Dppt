using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Authorization.Abstractions.Permissions.PermissionValue
{
    public interface IPermissionValueProvider
    {
        string Name { get; }

        //TODO: Rename to GetResult? (CheckAsync throws exception by naming convention)
        Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);
    }
}
