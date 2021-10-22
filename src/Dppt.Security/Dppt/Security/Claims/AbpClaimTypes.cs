using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Security.Claims
{
    /// <summary>
    /// 用于获取特定于ABP的声明类型名称。
    /// </summary>
    public static class AbpClaimTypes
    {
        /// <summary>
        /// Default: <see cref="ClaimTypes.Name"/>
        /// </summary>
        public static string UserName { get; set; } = ClaimTypes.Name;

        /// <summary>
        /// Default: <see cref="ClaimTypes.GivenName"/>
        /// </summary>
        public static string Name { get; set; } = ClaimTypes.GivenName;

        /// <summary>
        /// Default: <see cref="ClaimTypes.Surname"/>
        /// </summary>
        public static string SurName { get; set; } = ClaimTypes.Surname;

        /// <summary>
        /// Default: <see cref="ClaimTypes.NameIdentifier"/>
        /// </summary>
        public static string UserId { get; set; } = ClaimTypes.NameIdentifier;

        /// <summary>
        /// Default: <see cref="ClaimTypes.Role"/>
        /// </summary>
        public static string Role { get; set; } = ClaimTypes.Role;

        /// <summary>
        /// Default: <see cref="ClaimTypes.Email"/>
        /// </summary>
        public static string Email { get; set; } = ClaimTypes.Email;

        /// <summary>
        /// Default: "email_verified".
        /// </summary>
        public static string EmailVerified { get; set; } = "email_verified";

        /// <summary>
        /// Default: "phone_number".
        /// </summary>
        public static string PhoneNumber { get; set; } = "phone_number";

        /// <summary>
        /// Default: "phone_number_verified".
        /// </summary>
        public static string PhoneNumberVerified { get; set; } = "phone_number_verified";
    }
}
