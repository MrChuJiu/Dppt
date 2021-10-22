﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Authorization.Abstractions.Permissions.PermissionValue
{
    public interface IPermissionValueProviderManager
    {
        IReadOnlyList<IPermissionValueProvider> ValueProviders { get; }
    }
}
