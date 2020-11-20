using Easy.Core_Flow.NetCoreBase.BaaeContextAccessor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Core_Flow.NetCoreBase.Service
{
    public class ServiceBase
    {
        /// <summary>
        /// 身份信息
        /// </summary>
        protected IClaimsAccessor Claims { get; set; }

        /// <summary>
        /// cotr
        /// </summary>
        protected ServiceBase()
        {
            Claims = ServiceProviderInstance.Instance.GetRequiredService<IClaimsAccessor>();
        }
    }
}
