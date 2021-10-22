using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.PermissionValue;
using Dppt.Core.Dppt.Collections;

namespace Dppt.Authorization.Abstractions.Permissions.Permission
{
    public class AbpPermissionOptions
    {
        public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<IPermissionValueProvider> ValueProviders { get; }

        //public ITypeList<IPermissionStateProvider> GlobalStateProviders { get; }

        public AbpPermissionOptions()
        {
            DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();
            ValueProviders = new TypeList<IPermissionValueProvider>();
            //GlobalStateProviders = new TypeList<IPermissionStateProvider>();
        }
    }
}
