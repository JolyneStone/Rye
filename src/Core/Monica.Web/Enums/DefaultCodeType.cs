using System.ComponentModel;

namespace Monica.Web.Enums
{
    /// <summary>
    /// Api 返回值标识类型
    /// </summary>
    public enum DefaultCodeType
    {
        [Description("Success")]
        Success = 1,

        [Description("Fail")]
        Fail = -1,

        [Description("UserToken invalid")]
        UsertokenInvalid = 2,

        [Description("Parameters error")]
        ParametersError = 3,

        [Description("Database error")]
        DbError = 4,

        [Description("Data not fund")]
        DataNotFund = 5,
    }
}
