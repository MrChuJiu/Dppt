using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Easy.Core.Flow.UnitOfWork.Uow.Providers
{
    public class AsyncLocalCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider
    {
        public IUnitOfWork Current
        {
            get { return GetCurrentUow(); }
            set { SetCurrentUow(value); }
        }
        // 基于线程的本地变量存储 用于保存异步等待上下文中的共享变量的值
        // 对某个变量赋了值，而这个变量是多个线程共享的 可能会在等待期间线程被切换 导致值操作丢失
        private static readonly AsyncLocal<LocalUowWrapper> AsyncLocalUow = new AsyncLocal<LocalUowWrapper>();
        public AsyncLocalCurrentUnitOfWorkProvider()
        {

        }
        private static IUnitOfWork GetCurrentUow()
        {
            var uow = AsyncLocalUow.Value?.UnitOfWork;
            if (uow == null)
            {
                return null;
            }

            if (uow.IsDisposed)
            {
                AsyncLocalUow.Value = null;
                return null;
            }

            return uow;
        }
        private static void SetCurrentUow(IUnitOfWork value)
        {
            lock (AsyncLocalUow)
            {
                if (value == null)
                {
                    if (AsyncLocalUow.Value == null)
                    {
                        return;
                    }
                    if (AsyncLocalUow.Value.UnitOfWork?.Outer == null)
                    {
                        AsyncLocalUow.Value.UnitOfWork = null;
                        AsyncLocalUow.Value = null;
                        return;
                    }
                    AsyncLocalUow.Value.UnitOfWork = AsyncLocalUow.Value.UnitOfWork.Outer;
                }
                else
                {
                    if (AsyncLocalUow.Value?.UnitOfWork == null)
                    {
                        if (AsyncLocalUow.Value != null)
                        {
                            AsyncLocalUow.Value.UnitOfWork = value;
                        }

                        AsyncLocalUow.Value = new LocalUowWrapper(value);
                        return;
                    }
                    value.Outer = AsyncLocalUow.Value.UnitOfWork;
                    AsyncLocalUow.Value.UnitOfWork = value;
                }
            }
        }

        private class LocalUowWrapper
        {
            public IUnitOfWork UnitOfWork { get; set; }

            public LocalUowWrapper(IUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }
        }


    }
}
