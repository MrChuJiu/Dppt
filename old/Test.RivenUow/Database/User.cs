using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.RivenUow.Database
{
    public class User
    {
        public int UserId { get; set; }
        public virtual string Creator { get; set; }
        public virtual string TenantName { get; set; }
    }
}
