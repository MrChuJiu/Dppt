using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.RivenUow.Database
{
    public class User
    {
        public virtual string Creator { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual string LastModifier { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual string Deleter { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string TenantName { get; set; }
    }
}
