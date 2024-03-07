using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using hallocdoc_mvc_Service.Implementation;



namespace HalloDoc.Auth
{
    public class AuthManager
    {
    }

    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CustomAuthorize(string role)
        {
            string str = role;

            // Split the string using a comma as the delimiter
            string[] array = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            this._roles = array;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();
            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "Login", }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtService.ValidateJwtToken(token, out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin", }));
                return;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin", }));
                return;
            }

            if (_roles == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin", }));
                return;
            }

            if (!_roles.Any(role => role == roleClaim.Value))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin", }));
                return;
            }
            //var user = SessionUtils.GetLoggedInUser(context.HttpContext.Session);

            //if (user == null)
            //{
            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //}
            //if (!string.IsNullOrEmpty(_role))
            //{
            //    if (!(user.role == _role))
            //    {
            //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //    }
            //}
        }
    }
}

