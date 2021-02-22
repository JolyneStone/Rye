using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Rye
{
    /// <summary>
    /// 身份证操作类
    /// </summary>
    public class IDCardHelper
    {
        /// 验证身份证结果
        /// </summary>
        /// <param name="idcard">身份证号码</param>
        /// <returns>正确的时候返回string.Empty</returns>
        public static string Validate(string idcard)
        {
            if (idcard.Length != 18 && idcard.Length != 15)
            {
                return "身份证明号码必须是15或者18位！";
            }

            Regex rg = new Regex(@"^\d{17}(\d|X)$");
            if (!rg.Match(idcard).Success)
            {
                return "身份证号码必须为数字或者X！";
            }
            if (idcard.Length == 15)
            {
                idcard = IdCard15To18(idcard);
            }
            else if (idcard.Length == 18)
            {
                int ll_sum = 0, tmp = 0;
                for (int i = 0; i < 17; i++)
                {
                    tmp = int.Parse(idcard.Substring(i, 1));
                    ll_sum += tmp * li_quan[i];
                }
                ll_sum = ll_sum % 11;
                if (idcard.Substring(17, 1) != ls_jy[ll_sum])
                {
                    return "身份证号码最后一位应该是" + ls_jy[ll_sum] + "！";
                }
            }
            try
            {
                DateTime.Parse(idcard.Substring(6, 4) + "-" + idcard.Substring(10, 2) + "-" + idcard.Substring(12, 2));
            }
            catch
            {
                return "非法生日";
            }
            return string.Empty;
        }



        private static readonly string[] ls_jy = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
        private static readonly int[] li_quan = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };

        /// <summary>
        /// 15位身份证明号码转化成18位用来编码
        /// </summary>
        /// <param name="idcard">15位的身份证号码</param>
        /// <returns></returns>
        public static string IdCard15To18(string idcard)
        {

            if (idcard == null || idcard.Length != 15)
            {
                return idcard;
            }
            else
            {
                string result = string.Empty;
                int ll_sum = 0, tmp = 0;
                result = idcard.Substring(0, 6) + "19" + idcard.Substring(6, 9);
                for (int i = 0; i < 17; i++)
                {
                    tmp = int.Parse(result.Substring(i, 1));
                    ll_sum += tmp * li_quan[i];
                }
                ll_sum = ll_sum % 11;
                result += ls_jy[ll_sum];
                return result;

            }
        }

        /// <summary>
        /// 获取身份证对应省份的区划
        /// </summary>
        /// <param name="id">身份证</param>
        /// <returns>头两位+4个0</returns>
        public static string GetProvince(string id)
        {
            return id.Substring(0, 2) + "0000";
        }

        /// <summary>
        /// 获取身份证对应县市的区划
        /// </summary>
        /// <param name="id">身份证</param>
        /// <returns>头4位+2个0</returns>
        public static string GetCity(string id)
        {
            return id.Substring(0, 4) + "00";
        }

        /// <summary>
        /// 获取身份证对应地区的区划
        /// </summary>
        /// <param name="id">身份证</param>
        /// <returns>头6位</returns>
        public static string GetArea(string id)
        {
            return id.Substring(0, 6);
        }

        /// <summary>
        /// 根据身份证判断是否男女
        /// </summary>
        /// <param name="id">身份证号码</param>
        /// <returns>返回"男"或者"女"</returns>
        public static string GetSexName(string id)
        {
            int sexStr = 0;
            if (id.Length == 15)
            {
                sexStr = Convert.ToInt32(id.Substring(14, 1));
            }
            else if (id.Length == 18)
            {
                sexStr = Convert.ToInt32(id.Substring(16, 1));
            }
            else
            {
                throw new ArgumentException("身份证号码不是15或者18位！");
            }
            return sexStr % 2 == 0 ? "女" : "男";
        }

        /// <summary>
        /// 根据身份证获取出生年月
        /// </summary>
        /// <param name="id">身份证号码</param>
        /// <returns>出生年月</returns>
        public static DateTime GetBirthday(string id)
        {
            string result = string.Empty;
            if (id.Length == 15)
            {
                result = "19" + id.Substring(6, 2) + "-" + id.Substring(8, 2) + "-" + id.Substring(10, 2);
            }
            else if (id.Length == 18)
            {
                result = id.Substring(6, 4) + "-" + id.Substring(10, 2) + "-" + id.Substring(12, 2);
            }
            else
            {
                throw new ArgumentException("身份证号码不是15或者18位！");
            }
            return Convert.ToDateTime(result);
        }

    }
}
