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
using hallodoc_mvc_Repository.DataContext;

namespace hallocdoc_mvc_Service.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;
        private readonly IAdmin_Repository _Repository;
        private readonly ApplicationDbContext _db;

        public JwtService(IConfiguration configuration, IAdmin_Repository Repository, ApplicationDbContext db)
        {
            this.configuration = configuration;
            this._Repository = Repository;
            this._db = db;
        }
        //public string GenerateJwtToken(LoginViewModel aspNetUser)
        //{
        //    var roles = _Repository.GetAspNetRole(aspNetUser.Id);

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email, aspNetUser.Email),
        //        new Claim("Id", aspNetUser.Id.ToString())

        //    };
        //    roles.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));


        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qqweertyuiopsdfg5hdfghjklxcvbnmedfghjertyusxdcfvSADFAKSHDFGBASDHFBVAJKSHFDBAJKSBHDFASDGBFSADKJHvgbhnjmfg"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expires = DateTime.UtcNow.AddMinutes(30);

        //    var token = new JwtSecurityToken(
        //        "Issuer",
        //        "Audience",
        //        claims,
        //        expires: expires,
        //        signingCredentials: creds
        //        );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public (string, string) GenerateJwtToken(LoginViewModel aspNetUser, string role)
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

            string menus = "";

            if (role == "Patient")
            {
                menus = "";   
            }
            if (role == "Admin")
            {
                var admin = _db.Admins.FirstOrDefault(a => a.AspNetUserId == aspNetUser.Id);

                if (admin != null)
                {

                    var rolemenu = _db.RoleMenus.Where(r => r.RoleId == admin.RoleId).ToList();
                    for (var i = 0; i < rolemenu.Count; i++)
                    {
                        menus += _db.Menus.FirstOrDefault(r => r.MenuId == rolemenu[i].MenuId).Name + ",";
                    }
                }
            }
            if (role == "Provider")
            {
                var physician = _db.Physicians.FirstOrDefault(a => a.AspNetUserId == aspNetUser.Id);

                if (physician != null)
                {
                    var rolemenu = _db.RoleMenus.Where(r => r.RoleId == physician.RoleId).ToList();
                    for (var i = 0; i < rolemenu.Count; i++)
                    {
                        menus += _db.Menus.FirstOrDefault(r => r.MenuId == rolemenu[i].MenuId).Name + ",";
                    }
                }
            }




            return (new JwtSecurityTokenHandler().WriteToken(token), menus);

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
            catch (Exception ex)
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
            var expires = DateTime.UtcNow.AddMinutes(360);

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
