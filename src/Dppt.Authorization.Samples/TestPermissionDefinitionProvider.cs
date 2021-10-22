using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions;
using Dppt.Authorization.Abstractions.Permissions.Permission;

namespace Dppt.Authorization.Samples
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var mainDataGroup = context.AddGroup(TestPermissions.GroupName, "主业务数据模块");

            var companyGroup = mainDataGroup.AddPermission(TestPermissions.Company.Default, "公司");
            companyGroup.AddChild(TestPermissions.Company.Create, "添加");
            companyGroup.AddChild(TestPermissions.Company.Delete, "删除");
            companyGroup.AddChild(TestPermissions.Company.Update, "修改");
        }
    }
}
