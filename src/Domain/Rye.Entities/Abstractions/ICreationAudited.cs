using System;
using System.Collections.Generic;
using System.Text;

namespace Rye.Entities
{
    public interface ICreationAudited
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建用户名称
        /// </summary>
        string CreatorFullName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
