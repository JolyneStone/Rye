namespace Rye
{
    public enum LangKeyEnum
    {
        [Lang("JS_DATE_LENGTH_INVALID", "JS时间数值的长度不正确，必须为10位或13位")]
        JsDateLengthInValid,

        [Lang("AVOID_REPEATABLE_REQUEST", "请勿重复提交请求，请稍候再试")]
        AvoidRepeatableRequest,

        [Lang("NOT_LOGIN", "请登录后再尝试")]
        NotLogin,

        [Lang("TOKEN_EXPIRE", "token 过期，请重新登录")]
        TokenExpire,

        [Lang("TOKEN_ERROR", "token 错误，请重新登录")]
        TokenError,

        [Lang("PERMISSION_NOT_ALLOW", "权限不足")]
        PermissionNotAllow,

        [Lang("PARAMETER_INVALID", "参数{0}错误：{1}")]
        ParameterInValid,

        [Lang("PARAMETER_ERROR", "参数错误")]
        ParameterError,

        [Lang("ENCRYPT_ERROR", "加密错误")]
        EncryptError,

        [Lang("DYCRYPT_ERROR", "解密错误")]
        DecryptError,
    }
}
