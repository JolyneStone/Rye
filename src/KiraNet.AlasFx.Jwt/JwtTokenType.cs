using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.AlasFx.Jwt
{
    public enum JwtTokenType : byte
    {
        AccessToken = 1,
        RefreshToken = 2
    }
}
