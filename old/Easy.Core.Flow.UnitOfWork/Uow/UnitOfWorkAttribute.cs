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
        /// Required：所处的嵌套事务 属于同一个事务ID，所以可以当做一个事务看待，多个事务都需要提交事务才算整体成功。
        /// RequiresNew：嵌套事务内部使用自己的事务ID 内外互不影响
        /// Suppress：嵌套事务内部的代码就等于完全脱离了事务的限制，内外事务都不会影响到他，不管内外事务是否有提交，都相当于有提交事务的操作
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }
        /// <summary>
        /// 工作单元事务的隔离级别,为空则使用默认值
        /// 理论：
        /// 事务的特性（ACID）
        /// 1、原子性（Atomicity）
        /// 　　事物是数据库的逻辑工作单位，事务中的诸多操作要么全做要么全不做
        /// 2、一致性（Consistency）
        /// 　　事务执行结果必须是使数据库从一个一致性状态变到另一个一致性状态
        /// 3、隔离性（Isolation）
        /// 　　一个数据的执行不能被其他事务干扰
        /// 4、持续性/永久性（Durability）
        /// 　　一个事务一旦提交，它对数据库中的数据改变是永久性的
        /// 　　
        /// Serializable      设置范围锁定，在运行事务处理时，不能向所读取的记录范围内，添加新记录。
        /// RepeatableRead    为读取的记录设置锁定，直到事务处理完成为止，这样就避免了不可“无法重复的读取”的问题，但是幻影读取仍可能发生。
        /// ReadCommitted     等待其他事务处理释放对记录的写入锁定，这样就不会出现脏读取操作，这个级别为读取当前的记录设置读取锁定，
        ///                   为写入的数据设置写入锁定，直到事务处理完成，对于要读取的记录，在移动到下一个记录上时，每个记录都是未锁定的，所以可能出现“无法重复的读取操作”。
        /// Unspecified       表示，提供程序使用另一个隔离级别通道，它不同于IsolationLevel的枚举值。
        /// Snapshot          只能用于sql server2005 以后的版本，在复制修改的纪录时，这个级别会减少锁定，这样其他事务处理就可以读取旧数据，不需等待解锁。
        /// Chaos             类似于ReadUncommitted，除了执行与ReadUncommitted 值的操作外，他不能锁定更新的记录。
        /// ReadUncommitted   事务处理不会相互隔离，使用这个级别，不需要等待其他事务处理释放锁定的记录，
        ///                   可以从其他事务处理中读取未提交的数据--脏读取，这级别仅用于读取临时修改不太重要的记录，例如报表。
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
