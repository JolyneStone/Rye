using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monica.Web.Model
{
    public class ApiGridResult<T> : ApiResult<T>
        where T: PageData<T>

    {
        public ApiGridResult()
        {
        }

        public ApiGridResult(T data, string msg, int resultCode, ReturnCodeType returnCode = ReturnCodeType.Success)
            : base(data, msg, resultCode, returnCode)
        {
        }
    }
}
