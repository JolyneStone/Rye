using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Rye.Util
{
    public class StringUtil
    {
        /// <summary>
        /// 判断是否为手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length != 11)
            {
                return false;
            }
            Regex regex = new Regex("^((13|15|16|18|17|19)\\d{9})|((14[57])\\d{8})$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 判断是否为邮箱地址
        /// </summary>
        /// <param name="emailValue"></param>
        /// <returns></returns>
        public static bool IsEmail(string emailValue)
        {
            string pattern = "^[A-Z,a-z,0-9]+([-+._][A-Z,a-z,0-9]+)*@[A-Z,a-z,0-9]+([-.][A-Z,a-z,0-9]+)*\\.[A-Z,a-z,0-9]+([-.][A-Z,a-z,0-9]+)*$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(emailValue);
        }


        /// <summary>
        /// 将一个用指定分割符分割的字符串 转化成指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="split">分隔符</param>
        /// <returns>（success：是否转化成功 list:转化的结果 ）</returns>
        public static (bool success, IEnumerable<T> list) ParseByStr<T>(string str, params char[] split) where T : struct
        {
            var arr = str.Split(split);
            var type = typeof(T);
            var list = new List<T>();

            var parse = type.GetMethod("Parse", new Type[] { typeof(string) });
            if (parse == null)
            {
                return (false, list);
            }
            foreach (var item in arr)
            {
                try
                {
                    var value = (T)parse.Invoke(null, new object[] { item });
                    list.Add(value);
                }
                catch
                {
                    return (false, list);
                }
            }
            return (true, list);
        }


        /// <summary>
        /// 获得随机验证码数字
        /// </summary>
        /// <returns></returns>
        public static string GetSmsCodeOnlyNum(int size = 4)
        {
            string max = "";
            string replace = "";
            for (int i = 0; i < size; i++)
            {
                max += "9";
                replace += "0";
            }

            System.Random rdm = new System.Random();
            return rdm.Next(0, int.Parse(max)).ToString(replace);
        }

        /// <summary>
        /// 获得随机验证码数字包含字母
        /// </summary>
        /// <returns></returns>
        public static string GetSmsCode(int size = 4)
        {
            string chars = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            string pwd;
            pwd = "";
            Random randrom = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < size; i++)
            {
                pwd += chars[randrom.Next(chars.Length)];
            }
            return pwd;
        }



        /// <summary>
        /// 获取随机密码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomPwd(int length = 8)
        {
            string chars = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            string pwd;
            while (true)
            {
                pwd = "";
                Random randrom = new Random((int)DateTime.Now.Ticks);
                for (int i = 0; i < length; i++)
                {
                    pwd += chars[randrom.Next(chars.Length)];
                }
                if (int.TryParse(pwd, out int intpwd)) //过滤纯数字
                {
                    continue;
                }
                if (Regex.IsMatch(pwd, "^[A-Za-z]+$")) //过滤纯英文
                {
                    continue;
                }
                break;
            }
            return pwd;
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


        /// <summary>
        /// 保留position位有效小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string SignificantDigit(decimal value, int position)
        {
            if (value == 0)
                return "0";
            var str = ".";
            for (var i = 0; i < position; i++)
            {
                str += "#";
            }
            if (value > 0 && value < 1)
                return "0" + value.ToString(str);
            else if (value < 0 && value > -1)
                return "-0" + (-value).ToString(str);
            else
                return value.ToString(str);
        }


        /// <summary>
        /// 保留position位有效小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string SignificantDigit(double value, int position)
        {
            if (value == 0)
                return "0";
            var str = ".";
            for (var i = 0; i < position; i++)
            {
                str += "#";
            }
            if (value > 0 && value < 1)
                return "0" + value.ToString(str);
            else if (value < 0 && value > -1)
                return "-0" + (-value).ToString(str);
            else
                return value.ToString(str);
        }


        /// <summary>
        /// 保留position位有效小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string SignificantDigit(float value, int position)
        {
            if (value == 0)
                return "0";
            var str = ".";
            for (var i = 0; i < position; i++)
            {
                str += "#";
            }
            if (value > 0 && value < 1)
                return "0" + value.ToString(str);
            else if (value < 0 && value > -1)
                return "-0" + (-value).ToString(str);
            else
                return value.ToString(str);
        }

    }
}
