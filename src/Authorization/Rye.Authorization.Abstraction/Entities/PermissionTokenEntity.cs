using Rye.Jwt.Entities;

namespace Rye.Authorization.Entities
{
    public class PermissionTokenEntity: TokenEntityBase
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 角色Id集合
        /// </summary>
        public string RoleIds { get; set; }
    }

    //public class PermissionTokenEnt<TAppId, TUserId, TRoleId>
    //    where TAppId : IEquatable<TAppId>
    //    where TUserId : IEquatable<TUserId>
    //    where TRoleId : IEquatable<TRoleId>
    //{
    //    /// <summary>
    //    /// AppId
    //    /// </summary>
    //    public TAppId AppId { get; set; }

    //    /// <summary>
    //    /// 用户Id
    //    /// </summary>
    //    public TUserId UserId { get; set; }

    //    /// <summary>
    //    /// 角色Id集合
    //    /// </summary>
    //    public List<TRoleId> RoleIds { get; set; }
    //}

}
