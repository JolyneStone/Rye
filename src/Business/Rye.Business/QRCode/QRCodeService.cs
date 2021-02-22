using QRCoder;

using System;
using System.Drawing;
using System.IO;

namespace Rye.Business.QRCode
{
    public class QRCodeService: IQRCodeService
    {
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="codeNumber">要生成二维码的字符串</param>     
        /// <param name="size">大小尺寸</param>
        /// <returns>二维码图片</returns>
        public Bitmap CreateQRCode(string codeNumber, int size = 200)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeNumber, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(size / 49);
            return qrCodeImage;
        }
        /// <summary>
        /// 将二维码画到图片上去
        /// </summary>
        /// <param name="imgPath">图片地址</param>
        /// <param name="qrInfo">二维码信息</param>
        /// <param name="y">需要画图的y轴位置</param>
        /// <param name="x">需要画图的x轴位置</param>
        /// <param name="imgSize">图片大小</param>
        /// <returns>图片二进制文件</returns>
        public byte[] DrawQRcodeToImg(string imgName, string qrInfo, int top, int left, int imgSize = 200)
        {
            if (left < 0)
            {
                throw new ArgumentException("不能为负数", nameof(left));
            }
            if (top < 0)
            {
                throw new ArgumentException("不能为负数", nameof(top));
            }
            Bitmap qrCodeImg = CreateQRCode(qrInfo, imgSize);
            Bitmap img = new Bitmap(imgName);
            if (img.Width < left + qrCodeImg.Width)
            {
                throw new ArgumentException("二维码将超出宽度范围", nameof(left));
            }
            if (img.Height < top + qrCodeImg.Height)
            {
                throw new ArgumentException("二维码将超出高度范围", nameof(top));
            }
            var whiteBroder = (imgSize % 49) / 2;
            //画上白色边框
            if (whiteBroder > 0)
            {
                var newImg = new Bitmap(imgSize, imgSize);
                for (int i = whiteBroder; i < qrCodeImg.Width + whiteBroder; i++)
                {
                    for (int j = whiteBroder; j < qrCodeImg.Height + whiteBroder; j++)
                    {
                        var color = Color.White;
                        if (i >= whiteBroder && i < qrCodeImg.Width + whiteBroder && j >= whiteBroder & j < qrCodeImg.Height + whiteBroder)
                        {
                            color = qrCodeImg.GetPixel(i - whiteBroder, j - whiteBroder);
                        }
                        newImg.SetPixel(i, j, color);
                    }
                }
                qrCodeImg = newImg;
            }
            //开始画图 将二维码画到指定的位置
            for (int i = 0 + left; i < left + qrCodeImg.Width; i++)
            {
                for (int j = 0 + top; j < top + qrCodeImg.Height; j++)
                {
                    var color = qrCodeImg.GetPixel(i - left, j - top);
                    img.SetPixel(i, j, color);
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
