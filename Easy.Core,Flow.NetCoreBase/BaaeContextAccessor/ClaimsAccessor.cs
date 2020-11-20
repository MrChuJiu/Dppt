using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Easy.Core_Flow.NetCoreBase.BaaeContextAccessor
{
    public class ClaimsAccessor : IClaimsAccessor
    {
        protected IPrincipalAccessor PrincipalAccessor { get; }

        public ClaimsAccessor(IPrincipalAccessor principalAccessor)
        {
            PrincipalAccessor = principalAccessor;
        }

        public int? ApiUserId {

            get { 
            
                var  userId = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid)?.Value;
                if (userId != null)
                {
                    int id = 0;
                    int.TryParse(userId, out id);
                    return id;
                }

                return null;
            }
        
        }

        public string RoleIds {

            get {

                var roleIds = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (string.IsNullOrWhiteSpace(roleIds))
                {
                    return string.Empty;
                }

                return roleIds;

            }
        
        }
    }
}
