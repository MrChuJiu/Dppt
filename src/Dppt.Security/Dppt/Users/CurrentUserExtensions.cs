using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Security.Users;
using JetBrains.Annotations;

namespace Dppt.Security.Dppt.Users
{
    public static class CurrentUserExtensions
    {
        [CanBeNull]
        public static string FindClaimValue(this ICurrentUser currentUser, string claimType)
        {
            return currentUser.FindClaim(claimType)?.Value;
        }

        public static T FindClaimValue<T>(this ICurrentUser currentUser, string claimType)
            where T : struct
        {
            var value = currentUser.FindClaimValue(claimType);
            if (value == null)
            {
                return default;
            }

            return value.To<T>();
        }

        public static Guid GetId(this ICurrentUser currentUser)
        {
            Debug.Assert(currentUser.Id != null, "currentUser.Id != null");

            return currentUser.Id.Value;
        }
    }
}
