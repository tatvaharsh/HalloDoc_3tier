using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;

namespace hallocdoc_mvc_Service.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;
        private readonly IAdmin_Repository _Repository;

        public JwtService(IConfiguration configuration, IAdmin_Repository Repository)
        {
            this.configuration = configuration;
            this._Repository = Repository;
        }
        public string GenerateJwtToken(LoginViewModel aspNetUser)
        {
            var roles = _Repository.GetAspNetRole(aspNetUser.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, aspNetUser.Email),
                new Claim("Id", aspNetUser.Id.ToString())

            };
            roles.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qqweertyuiopsdfg5hdfghjklxcvbnmedfghjertyusxdcfvSADFAKSHDFGBASDHFBVAJKSHFDBAJKSBHDFASDGBFSADKJHvgbhnjmfg"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityToken(
                "Issuer",
                "Audience",
                claims,
                expires: expires,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public bool ValidateJwtToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null)
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("qqweertyuiopsdfg5hdfghjklxcvbnmedfghjertyusxdcfvSADFAKSHDFGBASDHFBVAJKSHFDBAJKSBHDFASDGBFSADKJHvgbhnjmfg");

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
        ,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;

                if (jwtSecurityToken != null)
                {
                    return true;
                }
                return false;

            }
            catch
            {
                return false;
            }
        }

        public string GenerateJwtTokenByEmail(string email)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qqweertyuiopsdfg5hdfghjklxcvbnmedfghjertyusxdcfvSADFAKSHDFGBASDHFBVAJKSHFDBAJKSBHDFASDGBFSADKJHvgbhnjmfg"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityToken(
                "Issuer",
                "Audience",
                claims,
                expires: expires,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
