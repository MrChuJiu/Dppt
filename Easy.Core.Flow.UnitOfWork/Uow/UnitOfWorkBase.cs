using Easy.Core.Flow.AspectCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    /// <summary>
    /// 工作单元基类
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <summary>
        /// 工作单元id
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// 对外公布的工作单元对象
        /// </summary>
        public IUnitOfWork Outer { get; set; }



        /// <summary>
        /// 提交事件
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<Exception> Failed;

        /// <summary>
        /// 事件
        /// </summary>
        public event EventHandler Disposed;


        /// <summary>
        /// 工作单元创建选项
        /// </summary>
        public UnitOfWorkOptions Options { get; protected set; }

        /// <summary>
        /// 工作单元的附加选项
        /// </summary>
        public Dictionary<string, object> Items { get; set; }

        /// <summary>
        /// 是否已经释放了资源
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 是否已经启动了工作单元
        /// </summary>
        private bool _isBeginCalledBefore;

        /// <summary>
        /// 是否已经提交了工作单元
        /// </summary>
        private bool _isCompleteCalledBefore;

        /// <summary>
        /// 工作单元是否已经提交成功
        /// </summary>
        private bool _succeed;

        /// <summary>
        /// 工作单元异常
        /// </summary>
        private Exception _exception;

        /// <summary>
        /// 数据库连接字符串显示名称
        /// </summary>
        private string _connectionStringName;


        /// <summary>
        /// 构造函数
        /// </summary>
        protected UnitOfWorkBase()
        {
            Id = Guid.NewGuid().ToString("N");
            Items = new Dictionary<string, object>();
        }

        public void Begin(UnitOfWorkOptions options)
        {
            PreventMultipleBegin();


            //TODO: Do not set options like that, instead make a copy?
            Options = options;

            this.SetConnectionStringName(Options.ConnectionStringName);

            BeginUow();
        }
     
        /// <summary>
        /// 启动工作单元之前,校验工作单元是否被重复启动
        /// </summary>
        protected virtual void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new Exception($"工作单元{this.Id}已启动,不能重复启动!");
            }

            _isBeginCalledBefore = true;
        }

        /// <inheritdoc/>
        public IDisposable SetConnectionStringName(string connectionStringName)
        {
            var oldConnectionStringName = this._connectionStringName;

            this._connectionStringName = connectionStringName;

            return new DisposeAction(() =>
            {
                _connectionStringName = oldConnectionStringName;
            });
        }
        /// <summary>
        /// 启动工作单元
        /// </summary>
        protected virtual void BeginUow()
        {

        }
        /// <summary>
        /// 保存修改
        /// </summary>
        public abstract void SaveChanges();
        /// <summary>
        /// 保存修改 - 异步
        /// </summary>
        /// <returns></returns>
        public abstract Task SaveChangesAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 提交工作单元
        /// </summary>
        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }
        /// <summary>
        /// 提交工作单元 - 异步
        /// </summary>
        /// <returns></returns>
        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            PreventMultipleComplete();
            try
            {
                await CompleteUowAsync(cancellationToken);
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }
        /// <summary>
        /// 提交工作单元
        /// </summary>
        protected abstract void CompleteUow();
        /// <summary>
        /// 提交工作单元 - 异步
        /// </summary>
        protected abstract Task CompleteUowAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 触发 <see cref="Completed"/> 事件
        /// </summary>
        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this, null);
        }
        /// <summary>
        /// 触发 <see cref="Failed"/> 事件
        /// </summary>
        /// <param name="exception">异常信息</param>
        protected virtual void OnFailed(Exception exception)
        {
            Failed?.Invoke(this, exception);
        }
        /// <summary>
        /// 触发 <see cref="Disposed"/> 事件.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, null);
        }
        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
        public string GetConnectionStringName()
        {
            return this._connectionStringName;
        }
        /// <summary>
        /// 提交工作单元之前,校验工作单元是否已经被提交
        /// </summary>
        protected virtual void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new Exception($"工作单元{this.Id}已提交");
            }

            _isCompleteCalledBefore = true;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="state"></param>
        protected virtual void Dispose(bool state)
        {
            if (!_isBeginCalledBefore || IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // https://docs.microsoft.com/zh-cn/visualstudio/code-quality/ca1063?view=vs-2019
            this.Dispose(true);
        }

        /// <summary>
        /// 释放工作单元
        /// </summary>
        protected abstract void DisposeUow();
    }
}
