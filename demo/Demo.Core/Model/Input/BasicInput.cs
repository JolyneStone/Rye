using Rye.Business.Language;

using System.ComponentModel.DataAnnotations;

namespace Demo.Core.Model.Input
{
    public class BasicInput
    {
        /// <summary>
        /// App Key
        /// </summary>
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public string AppKey { get; set; }

        /// <summary>
        /// 语言，默认为zh-cn
        /// </summary>
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public string Lang { get; set; } = LangCode.ZHCN;

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
