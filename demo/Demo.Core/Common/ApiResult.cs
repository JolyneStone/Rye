using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Core.Common
{
    public class ApiResult
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public static ApiResult Create(int code, string message)
        {
            return new ApiResult()
            {
                Code = (int)code,
                Message = message
            };
        }
    }

    public class ApiResult<T>: ApiResult
    {
        public T Data { get; set; }

        public static ApiResult<T> Create(int code, T data, string message)
        {
            return new ApiResult<T>()
            {
                Code = (int)code,
                Message = message,
                Data = data
            };
        }
    }
}
