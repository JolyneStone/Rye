using Monica.DataAccess;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monica.Entities.Abstractions
{
    /// <summary>
    /// 应用实体基类
    /// </summary>
    /// <typeparam name="TAppKey"></typeparam>
    public interface IEntityAppInfoBase<TAppKey> : IEntity<TAppKey> where TAppKey : IEquatable<TAppKey>
    {
        /// <summary>
        /// 应用主键
        /// </summary>
        TAppKey AppId { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>
        string AppKey { get; set; }

        /// <summary>
        /// App Secret
        /// </summary>
        string AppSecret { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        sbyte Status { get; set; }
    }
}
