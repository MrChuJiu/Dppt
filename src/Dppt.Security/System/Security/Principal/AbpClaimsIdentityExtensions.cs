using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dppt.Security.Claims;

namespace System.Security.Principal
{
    public static class AbpClaimsIdentityExtensions
    {
        public static Guid? FindUserId([NotNull] this ClaimsPrincipal principal)
        {
            var userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.UserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }
            if (Guid.TryParse(userIdOrNull.Value, out Guid result))
            {
                return result;
            }
            return null;
        }

        public static Guid? FindUserId([NotNull] this IIdentity identity)
        {

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.UserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(userIdOrNull.Value);
        }
    }
}
