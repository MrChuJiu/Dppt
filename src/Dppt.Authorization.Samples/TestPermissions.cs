using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dppt.Authorization.Samples
{
    public class TestPermissions
    {
        public const string GroupName = "MainDataManagement";

        public static class Company
        {
            public const string Default = GroupName + ".Company";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Check = Default + ".Check";
            public const string UnCheck = Default + ".UnCheck";
            public const string BatchCheck = Default + ".BatchCheck";
            public const string BatchUnCheck = Default + ".BatchUnCheck";
        }
    }
}
