using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.DependencyInjection
{
    public interface IExposedServiceTypesProvider
    {
        Type[] GetExposedServiceTypes(Type targetType);
    }
}
