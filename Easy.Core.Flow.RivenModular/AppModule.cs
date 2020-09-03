using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    public abstract class AppModule : IAppModule
    {
        public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
        {
           
        }

        public virtual void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            
        }

        public virtual void OnConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public virtual void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }

        public virtual void OnPostConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public virtual void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
           
        }

        public virtual void OnPreConfigureServices(ServiceConfigurationContext context)
        {
           
        }
    }
}
