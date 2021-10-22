using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Core_Flow.NetCoreBase.BaaeContextAccessor
{
    public interface IClaimsAccessor
    {
        /// <summary>
        /// 登录用户ID
        /// </summary>
        int? ApiUserId { get; }

        /// <summary>
        /// 用户角色Id
        /// </summary>
        string RoleIds { get; }
    }
}
