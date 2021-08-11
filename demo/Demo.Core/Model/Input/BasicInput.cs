
using System.ComponentModel.DataAnnotations;

namespace Demo.Model.Input
{
    public class BasicInput
    {
        ///// <summary>
        ///// App Key
        ///// </summary>
        //[Required(ErrorMessage = "INVALID_PARAMS")]
        //public string AppKey { get; set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public int ClientType { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public string Version { get; set; }
    }
}
