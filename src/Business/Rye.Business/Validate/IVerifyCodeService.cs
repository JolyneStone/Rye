using System.Drawing;

namespace Rye.Business.Validate
{
    public interface IVerifyCodeService
    {
        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name = "id" > 验证码编号 </ param >
        /// <param name="code">要校验的验证码</param>
        /// <param name="removeIfSuccess">验证成功时是否移除</param>
        /// <returns></returns>
        bool CheckCode(string id, string code, bool removeIfSuccess = true);

        /// <summary>
        /// 设置验证码到缓存中中
        /// </summary>
        void SetCode(string code, out string id);

        /// <summary>
        /// 将图片序列化成字符串
        /// </summary>
        string GetImageString(Image image, string id);
    }
}
