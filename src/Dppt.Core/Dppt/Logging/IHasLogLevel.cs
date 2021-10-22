using Microsoft.Extensions.Logging;

namespace Dppt.Core.Logging
{
    public interface IHasLogLevel
    {
        /// <summary>
        /// Log severity.
        /// </summary>
        LogLevel LogLevel { get; set; }
    }
}
