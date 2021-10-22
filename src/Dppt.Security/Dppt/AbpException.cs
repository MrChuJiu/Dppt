using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.Security.Dppt
{
    /// <summary>
    /// Abp系统为特定于Abp的异常抛出的基本异常类型。
    /// </summary>
    public class AbpException : Exception
    {
        public AbpException()
        {

        }

        public AbpException(string message)
            : base(message)
        {

        }

        public AbpException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public AbpException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}
