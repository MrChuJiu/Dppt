using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Domain.Entities
{
    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Id
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// 是否为临时对象
        /// </summary>
        /// <returns></returns>
        bool IsTransient();

        /// <summary>
        /// 实体比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool EntityEquals(object obj);
    }

    /// <summary>
    /// 实体接口,主键为long
    /// </summary>
    public interface IEntity : IEntity<long>
    {

    }
}
