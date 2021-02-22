using Rye.Authorization.Entities;

namespace Demo.Library.Dto
{
    public class JwtTokenEntity: PermissionTokenEntity
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
    }
}
