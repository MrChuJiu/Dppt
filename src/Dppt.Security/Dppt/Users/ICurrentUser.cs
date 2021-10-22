using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Security.Users
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        [CanBeNull]
        Guid? Id { get; }

        [CanBeNull]
        string UserName { get; }

        [CanBeNull]
        string Name { get; }

        [CanBeNull]
        string SurName { get; }

        [CanBeNull]
        string PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        [CanBeNull]
        string Email { get; }

        bool EmailVerified { get; }

        [NotNull]
        string[] Roles { get; }

        [CanBeNull]
        Claim FindClaim(string claimType);

        [NotNull]
        Claim[] FindClaims(string claimType);

        [NotNull]
        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}
