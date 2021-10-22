using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Dppt.Authorization.Samples
{
    public class AddDpptHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }
        /// <summary>
        /// 验证在 SignInAsync 中颁发的证书，并返回一个 AuthenticateResult 对象，表示用户的身份。
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = Context.Request.Cookies["dpptCookie"];
            if (string.IsNullOrEmpty(cookie))
            {
                return AuthenticateResult.NoResult();
            }
            
            var properties = JsonSerializer.Deserialize<AuthenticationProperties>(cookie);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(), properties, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        /// <summary>
        /// 返回一个需要认证的标识来提示用户登录，通常会返回一个 401 状态码。
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            Context.Response.Redirect("/login");
            return Task.CompletedTask;
        }
        /// <summary>
        /// 禁上访问，表示用户权限不足，通常会返回一个 403 状态码。
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
        /// <summary>
        /// 退出登录，如清除Coookie等。
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            Context.Response.Cookies.Delete("dpptCookie");
            return Task.CompletedTask;
        }
        /// <summary>
        ///  用户登录成功后颁发一个证书（加密的用户凭证），用来标识用户的身份。
        /// </summary>
        /// <param name="user">表示证书的主体，在基于声明的认证中，用来标识一个人的身份（如：姓名，邮箱等等）</param>
        /// <param name="properties">用来表示证书颁发的相关信息，如颁发时间，过期时间，重定向地址等</param>
        /// <returns></returns>
        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            // AuthenticationTicket看成是一个经过认证后颁发的证书
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);
            Context.Response.Cookies.Append("dpptCookie", JsonSerializer.Serialize(properties));
            return Task.CompletedTask;
        }
    }
}
