using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ForumProject.Application.Features.Auths.DTOs;
using ForumProject.Application.Features.Auths.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ForumProject.Application.Features.Auths.Services
{
    public class AuthServiceHelper
    {
        private IConfiguration _config;

        public AuthServiceHelper(IConfiguration config)
        {
            _config = config;
        }

        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value + 
                Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );
        }


        public string CreateToken(int userId)
        {
            Claim[] claims = new Claim[]{
                new Claim("userId", userId.ToString())
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;
 
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    tokenKeyString != null ? tokenKeyString : ""
                )
            );

            SigningCredentials credentials = new SigningCredentials(
                    tokenKey, 
                    SecurityAlgorithms.HmacSha512Signature
                );

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> setPassword(LoginDto userForSetPassword
            , IAuthRepository authRepository)
        {
            byte[] passwordSalt = new byte[128 / 8];
            using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = GetPasswordHash(userForSetPassword.Password, passwordSalt);

            bool rowsAffected = await authRepository.UpdateAsync(
                new Domain.Entities.Auth
                {
                    Email = userForSetPassword.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                }
            );
            return rowsAffected;
        }
    }
}