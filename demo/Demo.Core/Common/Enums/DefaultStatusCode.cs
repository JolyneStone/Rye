using Rye;
using System.ComponentModel;

namespace Demo.Common.Enums
{
    public enum DefaultStatusCode
    {
        #region 成功失败

        [Description("成功"), Lang("SUCCESS", "成功")]
        Success = 200,

        [Description("失败"), Lang("FAIL", "失败")]
        Fail = -1,

        #endregion

        #region 公共基础

        [Description("未知错误"), Lang("UNKNOWN_ERROR", "未知错误")]
        UnknownError = 2,

        [Description("接口异常"), Lang("INTERFACE_EXCEPTION")]
        InterfaceError = 3,

        [Description("参数错误"), Lang("INVALID_PARAMS", "参数错误")]
        ParametersError = 4,

        [Description("userToken失效"), Lang("USERTOKEN_INVALID", "userToken失效")]
        UsertokenInvalid = 400,

        [Description("无权访问"), Lang("NO_PERMISSION", "无权访问")]
        NoPermission = 401,

        [Description("请求受限"), Lang("LIMITED_REQUEST", "请求受限")]
        BanRequest = 7,

        [Description("网络错误，请联系客服"), Lang("BLACKUSERTIP", "网络错误，请联系客服")]
        BlackUserTip = 8,

        [Description("网络异常"), Lang("NETWORK_ANOMALY", "网络异常")]
        NetworkAnomaly = 9,

        [Description("网络环境差, 请稍后重试"), Lang("TRADE_NETWORK_ERROR", "网络环境差, 请稍后重试")]
        TradeNetworkError = 10,

        [Description("数据不存在"), Lang("DATA_NOT_FUND", "数据不存在")]
        DataNotFund = 11,

        [Description("数据库异常"), Lang("DATABASE_EXCEPTION", "数据库异常")]
        DbError = 12,
        
        [Description("App不存在, 请检查App Key是否正确"), Lang("APP_NOT_EXIST", "App不存在, 请检查App Key是否正确")]
        AppNotExist = 13,
        #endregion

        #region 用户
        [Description("验证码错误"), Lang("VERIFY_CODE_ERROR", "验证码错误")]
        VerifyCodeError = 100,
        [Description("非法的手机号或邮箱"), Lang("INVALID_ACCOUNT", "非法的手机号或邮箱")]
        InvalidAccount = 101,
        [Description("账户或密码不正确"), Lang("ACCOUNT_ERROR", "账户或密码不正确")]
        AccountError = 102,
        [Description("账户已被锁定，请稍后再试"), Lang("ACCOUNT_ALREADY_LOCK", "账户已被锁定，请稍后再试")]
        AccountAlreadyLock = 103,
        //[Description("账户已被锁定，请2小时后再试"), Lang("ACCOUNT_LOCK", "账户已被锁定，请2小时后再试")]
        //AccountLock = 104,
        #endregion
    }
}
