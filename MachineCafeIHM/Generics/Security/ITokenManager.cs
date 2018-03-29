using System;
using System.Collections.Generic;
using System.Text;

namespace Generics.Security
{
    public interface ITokenManager
    {
        JsonWebToken PrepareTokenData(string appid);
        string GenerateToken(JsonWebToken tokendata, string appsecret);
    }
}
