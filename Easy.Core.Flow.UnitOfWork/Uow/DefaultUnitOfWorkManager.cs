using Easy.Core.Flow.UnitOfWork.Uow.Handles;
using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    public class DefaultUnitOfWorkManager : IUnitOfWorkManager
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public IActiveUnitOfWork Current => _currentUnitOfWorkProvider.Current;
        public DefaultUnitOfWorkManager(
            IServiceProvider serviceProvider,
             ICurrentUnitOfWorkProvider currentUnitOfWorkProvider
            )
        {
            _serviceProvider = serviceProvider;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }
        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }
        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            // 获取当前的外部工作单元
            var outerUow = _currentUnitOfWorkProvider.Current;

            // 如果已经存在有外部工作单元，则直接构建一个内部工作单元
            if (options.Scope == TransactionScopeOption.Required && outerUow != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }

            // 不存在外部工作单元，则从 IOC 容器当中获取一个新的出来
            var uow = _serviceProvider.GetService<IUnitOfWork>();


            // 绑定外部工作单元的事件
            uow.Completed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Failed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Disposed += (sender, args) =>
            {
                uow.Dispose();
            };

            uow.Begin(options);
            // 从外部UOW继承connectionStringName todo 理解为嵌套工作单元
            if (outerUow != null)
            {
                uow.SetConnectionStringName(outerUow.GetConnectionStringName());
            }
            // 设置当前的外部工作单元为刚刚初始化的工作单元
            _currentUnitOfWorkProvider.Current = uow;


            return uow;
        }
    }
}
