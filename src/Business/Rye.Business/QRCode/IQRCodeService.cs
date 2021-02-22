using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Business.QRCode
{
    public interface IQRCodeService
    {
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="codeNumber">要生成二维码的字符串</param>     
        /// <param name="size">大小尺寸</param>
        /// <returns>二维码图片</returns>
        Bitmap CreateQRCode(string codeNumber, int size = 200);
        /// <summary>
        /// 将二维码画到图片上去
        /// </summary>
        /// <param name="imgPath">图片地址</param>
        /// <param name="qrInfo">二维码信息</param>
        /// <param name="y">需要画图的y轴位置</param>
        /// <param name="x">需要画图的x轴位置</param>
        /// <param name="imgSize">图片大小</param>
        /// <returns>图片二进制文件</returns>
        byte[] DrawQRcodeToImg(string imgName, string qrInfo, int top, int left, int imgSize = 200);
    }
}
