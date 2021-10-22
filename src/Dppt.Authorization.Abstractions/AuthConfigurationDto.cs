using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Authorization.Abstractions
{
    public class AuthConfigurationDto
    {

        public Dictionary<string, bool> Policies { get; set; }

        public Dictionary<string, bool> GrantedPolicies { get; set; }

        public AuthConfigurationDto()
        {
            Policies = new Dictionary<string, bool>();
            GrantedPolicies = new Dictionary<string, bool>();
        }
    }
}
