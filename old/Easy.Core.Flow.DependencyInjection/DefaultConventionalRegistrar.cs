using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Easy.Core.Flow.DependencyInjection
{
    public class DefaultConventionalRegistrar : ConventionalRegistrarBase
    {
        static IExposedServiceTypesProvider ExposedServiceTypesProvider = new ExposedServiceTypesProvider {

            IncludeSelf = true,
            IncludeDefaults = true
        };
        public override void AddType(IServiceCollection services, Type type)
        {
            // 获取type继承的那个生命周期接口
            var lifeTime = GetLifeTimeOrNull(type);
            if (lifeTime == null)
            {
                return;
            }
            // 获取到type的注入服务类型进行注入
            var exposedServiceTypes = GetExposedServiceTypes(type);
            foreach (var exposedServiceType in exposedServiceTypes)
            {
                var serviceDescriptor = CreateServiceDescriptor(
                    type,
                    exposedServiceType,
                    exposedServiceTypes,
                    lifeTime.Value
                );

                services.Add(serviceDescriptor);
            }

        }
        protected virtual Type GetRedirectedTypeOrNull(
            Type implementationType,
            Type exposingServiceType,
            List<Type> allExposingServiceTypes)
        {
            if (allExposingServiceTypes.Count < 2)
            {
                return null;
            }

            if (exposingServiceType == implementationType)
            {
                return null;
            }

            if (allExposingServiceTypes.Contains(implementationType))
            {
                return implementationType;
            }

            return allExposingServiceTypes.FirstOrDefault(
                t => t != exposingServiceType && exposingServiceType.IsAssignableFrom(t)
            );
        }
        protected virtual ServiceDescriptor CreateServiceDescriptor(
            Type implementationType,
            Type exposingServiceType,
            List<Type> allExposingServiceTypes,
            ServiceLifetime lifeTime)
        {
            if (lifeTime.IsIn(ServiceLifetime.Singleton, ServiceLifetime.Scoped))
            {
                var redirectedType = GetRedirectedTypeOrNull(
                    implementationType,
                    exposingServiceType,
                    allExposingServiceTypes
                );

                if (redirectedType != null)
                {
                    return ServiceDescriptor.Describe(
                        exposingServiceType,
                        provider => provider.GetService(redirectedType),
                        lifeTime
                    );
                }
            }

            return ServiceDescriptor.Describe(
                exposingServiceType,
                implementationType,
                lifeTime
            );
        }
        protected virtual List<Type> GetExposedServiceTypes(Type type)
        {
            return GetExposedServices(type);
        }

        public static List<Type> GetExposedServices(Type type)
        {
            return type
                .GetCustomAttributes(true)
                .OfType<IExposedServiceTypesProvider>()
                .DefaultIfEmpty(ExposedServiceTypesProvider)
                .SelectMany(p => p.GetExposedServiceTypes(type))
                .Distinct()
                .ToList();
        }

        protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type)
        {
            return GetServiceLifetimeFromClassHierarchy(type);
        }

        protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
        {
            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return null;
        }
    }
}
