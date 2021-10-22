using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Core.Dppt.Threading
{
    public static class TaskCache
    {
        public static Task<bool> TrueResult { get; }
        public static Task<bool> FalseResult { get; }

        static TaskCache()
        {
            TrueResult = Task.FromResult(true);
            FalseResult = Task.FromResult(false);
        }
    }
}
