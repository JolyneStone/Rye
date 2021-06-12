namespace Rye
{
    public enum CheckEnum
    {
        [Lang("PARAMETERCHECK_BETWEEN", "参数“{0}”的值必须在“{1}”与“{2}”之间")]
        ParameterCheck_Between,

        [Lang("PARAMETERCHECK_BETWEENNOTEQUAL", "参数“{0}”的值必须在“{1}”与“{2}”之间，且不能等于“{3}”")]
        ParameterCheck_BetweenNotEqual,

        [Lang("PARAMETERCHECK_DIRECTORYNOTEXISTS", "指定的目录路径“{0}”不存在")]
        ParameterCheck_DirectoryNotExists,

        [Lang("PARAMETERCHECK_FILENOTEXISTS", "指定的文件路径“{0}”不存在")]
        ParameterCheck_FileNotExists,

        [Lang("PARAMETERCHECK_NOTCONTAINSNULL_COLLECTION", "集合“{0}”中不能包含null的项")]
        ParameterCheck_NotContainsNull_Collection,

        [Lang("PARAMETERCHECK_NOTEMPTY_GUID", "参数“{0}”的值不能为Guid.Empty")]
        ParameterCheck_NotEmpty_Guid,

        [Lang("PARAMETERCHECK_NOTGREATERTHAN", "参数“{0}”的值必须大于“{1}”")]
        ParameterCheck_NotGreaterThan,

        [Lang("PARAMETERCHECK_NOTGREATERTHANOREQUAL", "参数“{0}”的值必须大于或等于“{1}”")]
        ParameterCheck_NotGreaterThanOrEqual,

        [Lang("PARAMETERCHECK_NOTLESSTHAN", "参数“{0}”的值必须小于“{1}”")]
        ParameterCheck_NotLessThan,

        [Lang("PARAMETERCHECK_NOTLESSTHANOREQUAL", "参数“{0}”的值必须小于或等于“{1}”")]
        ParameterCheck_NotLessThanOrEqual,

        [Lang("PARAMETERCHECK_NOTNULL", "参数“{0}”不能为空引用")]
        ParameterCheck_NotNull,

        [Lang("PARAMETERCHECK_NOTNULLOREMPTY_COLLECTION", "参数“{0}”不能为空引用或空集合")]
        ParameterCheck_NotNullOrEmpty_Collection,

        [Lang("PARAMETERCHECK_NOTNULLOREMPTY_STRING", "参数“{0}”不能为空引用或空字符串")]
        ParameterCheck_NotNullOrEmpty_String,
    }
}
