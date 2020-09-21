using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    public abstract class AppModule : IAppModule
    {
        public virtual void OnPreConfigureServices()
        {

        }

        public virtual void OnConfigureServices()
        {

        }

        public virtual void OnPostConfigureServices()
        {

        }

        public virtual void OnPreApplicationInitialization()
        {

        }

        public virtual void OnApplicationInitialization()
        {

        }

        public virtual void OnPostApplicationInitialization()
        {

        }
        public virtual void OnApplicationShutdown()
        {

        }
    }
}
