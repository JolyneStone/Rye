namespace Rye.Entities
{
    public interface IHaveTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        int TenantId { get; set; }
    }
}
