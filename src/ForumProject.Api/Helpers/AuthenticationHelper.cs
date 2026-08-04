using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ForumProject.Api.Helpers
{
    public class AuthenticationHelper
    {
        public static TokenValidationParameters GetTokenValidationParameters(string? tokenKeyString )
        {
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    tokenKeyString != null ? tokenKeyString : ""
                )
            );

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = tokenKey,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ValidateAudience = false
            };

            return tokenValidationParameters;
        }
    }
}