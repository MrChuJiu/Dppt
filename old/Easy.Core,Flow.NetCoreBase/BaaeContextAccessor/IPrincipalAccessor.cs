using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Easy.Core_Flow.NetCoreBase.BaaeContextAccessor
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
