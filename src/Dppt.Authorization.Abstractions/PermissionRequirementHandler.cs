using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.PermissionChecker.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Dppt.Authorization.Abstractions
{
    public  class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {

        private readonly IPermissionChecker _permissionChecker;

        public PermissionRequirementHandler(IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            //这里通过权限检查器来确定当前用户是否拥有某个权限。
            //如果当前用户拥有某个权限，则通过 Contxt.Succeed() 通过授权验证。
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.PermissionName))
            {
                context.Succeed(requirement);
            }
        }
    }
}
