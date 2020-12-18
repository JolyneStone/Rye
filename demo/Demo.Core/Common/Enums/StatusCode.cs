using Monica;
using System.ComponentModel;

namespace Demo.Core.Common.Enums
{
    public enum StatusCode
    {
        #region 成功失败

        [Description("成功"), LangKey("SUCCESS")]
        Success = 1,

        [Description("失败"), LangKey("FAIL")]
        Fail = -1,

        #endregion

        #region 公共基础

        [Description("未知错误"), LangKey("UNKNOWN_ERROR")]
        UnknownError = 2,

        [Description("接口异常"), LangKey("INTERFACE_EXCEPTION")]
        InterFaceError = 3,

        [Description("参数错误"), LangKey("INVALID_PARAMS")]
        ParametersError = 4,

        [Description("userToken失效"), LangKey("USERTOKEN_INVALID")]
        UsertokenInvalid = 5,

        [Description("请求受限"), LangKey("LIMITED_REQUEST")]
        BanRequest = 6,

        [Description("网络错误，请联系客服！"), LangKey("BLACKUSERTIP")]
        BlackUserTip = 7,

        [Description("网络异常"), LangKey("NETWORK_ANOMALY")]
        NetworkAnomaly = 8,

        [Description("网络环境差, 请稍后重试"), LangKey("TRADE_NETWORK_ERROR")]
        TradeNetworkError = 9,

        [Description("数据不存在"), LangKey("DATA_NOT_FUND")]
        DataNotFund = 10,

        [Description("数据库异常"), LangKey("DATABASE_EXCEPTION")]
        DbError = 11,

        #endregion

    }
}
