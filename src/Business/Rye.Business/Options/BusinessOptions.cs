using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Business.Options
{
    public class BusinessOptions
    {
        /// <summary>
        /// 验证码过期时间
        /// </summary>
        public int VerfiyCodeExpire { get; set; } = 5 * 60;
    }
}
