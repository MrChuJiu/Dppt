using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using SIsolationLevel = System.Transactions.IsolationLevel;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    /// <summary>
    /// 工作单元配置参数
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// 范围事务设置
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// 工作单元是否启用事务,为空则使用默认值
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// 工作单元超时配置,为空则使用默认值
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 工作单元事务级别配置,为空则使用默认值
        /// </summary>
        public SIsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// 如果在异步范围中使用工作单元，则应将此选项设置为  <see cref="TransactionScopeAsyncFlowOption.Enabled"/>
        /// </summary>
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }

        /// <summary>
        /// 范围事务是否可用
        /// </summary>
        public bool IsTransactionScopeAvailable { get; set; }

        /// <summary>
        /// 连接字符串显示名称。默认值为 Default
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> ExtraData { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnitOfWorkOptions()
        {
            this.Init();
        }

        protected virtual void Init()
        {
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;
            IsTransactionScopeAvailable = true;
            ConnectionStringName = RivenUnitOfWorkConsts.DefaultConnectionStringName;
            ExtraData = new Dictionary<string, object>();
        }
    }
}
