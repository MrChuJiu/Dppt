using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Core.Flow.UnitOfWork.Uow.Handles
{
    /// <summary>
    /// 调用 Complete()/CompleteAsync() 会将 _isCompleteCalled 置为 true，然后在 Dispose() 
    /// 方法内会进行检测，为 faslse 的话直接抛出异常。可以看到在 InnerUnitOfWorkCompleteHandle 
    /// 内部并不会真正地调用 DbContext.SaveChanges() 进行数据保存
    /// </summary>
    public class InnerUnitOfWorkCompleteHandle : IUnitOfWorkCompleteHandle
    {
        public const string DidNotCallCompleteMethodExceptionMessage = "Did not call Complete method of a unit of work.";

        private volatile bool _isCompleteCalled;
        private volatile bool _isDisposed;

        public void Complete()
        {
            _isCompleteCalled = true;
        }

        public Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            _isCompleteCalled = true;
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool state)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (!_isCompleteCalled)
            {
                if (HasException())
                {
                    return;
                }

                throw new Exception(DidNotCallCompleteMethodExceptionMessage);
            }
        }

        private static bool HasException()
        {
            try
            {
                return Marshal.GetExceptionCode() != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
