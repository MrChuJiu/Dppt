using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Authorization.Abstractions.Permissions.Permission
{
    public interface IPermissionDefinitionProvider
    {
        void PreDefine(IPermissionDefinitionContext context);

        void Define(IPermissionDefinitionContext context);

        void PostDefine(IPermissionDefinitionContext context);
    }
}
