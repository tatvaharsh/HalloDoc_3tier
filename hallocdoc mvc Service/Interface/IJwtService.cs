using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;

namespace hallocdoc_mvc_Service.Implementation
{
    public interface IJwtService
    {
        string GenerateJwtToken(LoginViewModel model);

        bool ValidateJwtToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}
