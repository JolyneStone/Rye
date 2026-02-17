using System;
using System.Collections.Generic;
using System.Text;

namespace Rye.Entities
{
    public interface IDeleted
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
