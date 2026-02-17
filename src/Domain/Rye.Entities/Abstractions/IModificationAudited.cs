using System;

namespace Rye.Entities
{
    public interface IModificationAudited
    {
        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改用户Id
        /// </summary>
        long? LastModifierUserId { get; set; }

        /// <summary>
        /// 最后修改用户名称
        /// </summary>
        string LastModifierFullName { get; set; }
    }
}
