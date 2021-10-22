using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Authorization.Abstractions.Permissions.Permission
{
    /// <summary>
    /// 权限提供者
    /// </summary>
    public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        public virtual void PreDefine(IPermissionDefinitionContext context)
        {

        }

        public abstract void Define(IPermissionDefinitionContext context);

        public virtual void PostDefine(IPermissionDefinitionContext context)
        {

        }
    }
}
