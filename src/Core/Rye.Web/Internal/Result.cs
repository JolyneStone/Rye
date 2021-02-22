using System.Net;

namespace Rye.Web.Internal
{
    internal class Result
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public static Result Create(HttpStatusCode statusCode, string message)
        {
            return new Result()
            {
                StatusCode = (int)statusCode,
                Message = message
            };
        }
    }
}
