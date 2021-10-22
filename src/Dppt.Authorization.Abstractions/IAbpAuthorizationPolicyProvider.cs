using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dppt.Authorization.Abstractions
{
    public interface IAbpAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        Task<List<string>> GetPoliciesNamesAsync();
    }
}
