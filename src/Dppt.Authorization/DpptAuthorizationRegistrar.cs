using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Dppt.Authorization.Abstractions.Permissions.PermissionChecker.Interface;
using Dppt.Authorization.Permissions;
using Dppt.Security.Dppt.Security.Claims;
using Dppt.Security.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dppt.Authorization
{
    public static class DpptAuthorizationRegistrar
    {
        public static void AddDpptAuthorization(this IServiceCollection services,List<Type> types)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            AutoAddDefinitionProviders(services, types);

            //services.AddTransient<IPermissionValueProvider, UserPermissionValueProvider>();
            //services.AddTransient<IPermissionValueProviderManager, PermissionValueProviderManager>();

            services.AddTransient<IPermissionDefinitionContext, PermissionDefinitionContext>();
            services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();
            services.AddTransient<ICurrentPrincipalAccessor, ThreadCurrentPrincipalAccessor>();
            services.AddTransient<IPermissionChecker, PermissionChecker>();

            services.AddAuthorizationCore();
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            services.TryAddTransient<DefaultAuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationPolicyProvider, AbpAuthorizationPolicyProvider>();

        }

        private static void AutoAddDefinitionProviders(IServiceCollection services, List<Type> types)
        {
            var definitionProviders = types.Where(s => typeof(PermissionDefinitionProvider).IsAssignableFrom(s)).ToList();

            foreach (var item in definitionProviders)
            {
                services.AddTransient(item);
            }

            services.Configure<AbpPermissionOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }

    }
}
