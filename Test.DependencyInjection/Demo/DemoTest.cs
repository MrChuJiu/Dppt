using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy.Core.Flow.DependencyInjection;

namespace Test.DependencyInjection.Demo
{
    public class DemoTest : IDemoTest, IScopedDependency
    {
        public void HelloWorkd()
        {
            Console.WriteLine("DemoTest DemoTest DemoTest DemoTest");
        }
    }
}
