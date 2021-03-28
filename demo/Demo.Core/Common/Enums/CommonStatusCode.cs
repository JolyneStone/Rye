using Rye;
using System.ComponentModel;

namespace Demo.Core.Common.Enums
{
    public enum CommonStatusCode
    {
        #region 成功失败

        [Description("成功"), LangKey("SUCCESS")]
        Success = 200,

        [Description("失败"), LangKey("FAIL")]
        Fail = -1,

        #endregion

        #region 公共基础

        [Description("未知错误"), LangKey("UNKNOWN_ERROR")]
        UnknownError = 2,

        [Description("接口异常"), LangKey("INTERFACE_EXCEPTION")]
        InterfaceError = 3,

        [Description("参数错误"), LangKey("INVALID_PARAMS")]
        ParametersError = 4,

        [Description("userToken失效"), LangKey("USERTOKEN_INVALID")]
        UsertokenInvalid = 400,

        [Description("无权访问"), LangKey("NO_PERMISSION")]
        NoPermission = 401,

        [Description("请求受限"), LangKey("LIMITED_REQUEST")]
        BanRequest = 7,

        [Description("网络错误，请联系客服！"), LangKey("BLACKUSERTIP")]
        BlackUserTip = 8,

        [Description("网络异常"), LangKey("NETWORK_ANOMALY")]
        NetworkAnomaly = 9,

        [Description("网络环境差, 请稍后重试"), LangKey("TRADE_NETWORK_ERROR")]
        TradeNetworkError = 10,

        [Description("数据不存在"), LangKey("DATA_NOT_FUND")]
        DataNotFund = 11,

        [Description("数据库异常"), LangKey("DATABASE_EXCEPTION")]
        DbError = 12,
        
        [Description("App不存在, 请检查App Key是否正确"), LangKey("APP_NOT_EXIST")]
        AppNotExist = 13,
        #endregion

        #region 用户
        [Description("验证码错误"), LangKey("VERIFY_CODE_ERROR")]
        VerifyCodeError = 100,
        [Description("非法的手机号或邮箱"), LangKey("INVALID_ACCOUNT")]
        InvalidAccount = 101,
        [Description("账户或密码不正确"), LangKey("ACCOUNT_ERROR")]
        AccountError = 102,
        [Description("账户已被锁定，请2小时后再试"), LangKey("ACCOUNT_LOCK")]
        AccountLock = 103,
        [Description("账户已被锁定，请稍后再试"), LangKey("ACCOUNT_ALREADY_LOCK")]
        AccountAlreadyLock = 104,
        #endregion
    }
}
