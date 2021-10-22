using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Dppt.Authorization.Abstractions.Permissions.PermissionValue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dppt.Authorization.Permissions
{
    public class PermissionValueProviderManager : IPermissionValueProviderManager
    {
        public IReadOnlyList<IPermissionValueProvider> ValueProviders => _lazyProviders.Value;
        private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

        protected AbpPermissionOptions Options { get; }

        public PermissionValueProviderManager(
            IServiceProvider serviceProvider,
            IOptions<AbpPermissionOptions> options)
        {
            Options = options.Value;

            _lazyProviders = new Lazy<List<IPermissionValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as IPermissionValueProvider)
                    .ToList(),
                true
            );
        }
    }
}
