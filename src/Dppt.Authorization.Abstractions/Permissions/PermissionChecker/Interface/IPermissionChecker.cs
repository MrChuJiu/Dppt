using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions.PermissionChecker.Interface
{
    /// <summary>
    /// 授权许可
    /// </summary>
    public interface IPermissionChecker
    {
        Task<bool> IsGrantedAsync([NotNull] string name);

        Task<bool> IsGrantedAsync([CanBeNull] ClaimsPrincipal claimsPrincipal, [NotNull] string name);

    }
}
