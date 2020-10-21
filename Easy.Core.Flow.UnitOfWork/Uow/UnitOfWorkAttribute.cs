using System;
using System.Transactions;
using SIsolationLevel = System.Transactions.IsolationLevel;


namespace Easy.Core.Flow.UnitOfWork.Uow
{
    /// <summary>
    /// 该方法用于在中间件中被拦截 来开启工作单元
    /// 如果在调用此方法之前已经有一个工作单元，则此属性无效，如果是，则使用相同的事务.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class UnitOfWorkAttribute: Attribute
    {
        /// <summary>
        /// 事务范围
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }
        /// <summary>
        /// 工作单元事务级别配置,为空则使用默认值
        /// </summary>
        public SIsolationLevel? IsolationLevel { get; set; }
        /// <summary>
        /// 事务超时时间
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        /// <summary>
        /// 是否开启事务
        /// </summary>
        public bool? IsTransactional { get; set; }
        /// <summary>
        /// 是否禁用工作单元
        /// </summary>
        public bool IsDisabled { get; set; }


        /// <summary>
        /// 传入链接字符串穿的名称
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public virtual UnitOfWorkOptions CreateOptions(string connectionStringName = null)
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout,
                Scope = Scope ?? TransactionScopeOption.Required,
                ConnectionStringName = string.IsNullOrWhiteSpace(connectionStringName) ? RivenUnitOfWorkConsts.DefaultConnectionStringName : connectionStringName
            };
        }

    }
}
