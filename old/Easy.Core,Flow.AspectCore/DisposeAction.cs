using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Easy.Core.Flow.AspectCore
{
    /// <summary>
    /// This class can be used to provide an action when
    /// Dipose method is called.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<挂起>")]
    public class DisposeAction : IDisposable
    {
        public static readonly DisposeAction Empty = new DisposeAction(null);

        private Action _action;

        /// <summary>
        /// Creates a new <see cref="DisposeAction"/> object.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed.</param>
        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            // Interlocked prevents multiple execution of the _action.
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
