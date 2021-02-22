namespace Demo.DataAccess
{
    public partial interface IAppInfo
    {
        AppInfo GetModel(string appKey);

        AppInfo GetModelWithCache(string appKey);

        AppInfo GetModelWithCache(int appId);
    }
}
