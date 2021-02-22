using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Model.Intput
{
    public class LoginInput
    {
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public string Account { get; set; }
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public string Password { get; set; }
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public string VerifyCode { get; set; }
        [Required(ErrorMessage = "INVALID_PARAMS")]
        public string VerifyCodeId { get; set; }
    }
}
