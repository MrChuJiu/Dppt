using Easy.Core_Flow.NetCoreBase.BaaeContextAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Core_Flow.NetCoreBase.Service
{
    public class TestService: ServiceBase,ITestService
    {
        public string AddProduct()
        {
            return Claims.RoleIds;
        }
    }
}
