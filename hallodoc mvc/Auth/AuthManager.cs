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
        private readonly string _menu;

        public CustomAuthorize(string menu, params string[] roles)
        {
            _roles = roles;
            _menu = menu;
        }


        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();
        //    if (jwtService == null)
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", }));
        //        return;
        //    }

        //    var request = context.HttpContext.Request;
        //    var token = request.Cookies["jwt"];

        //    if (token == null || !jwtService.ValidateJwtToken(token, out JwtSecurityToken jwtToken))
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "Admin_Login", }));
        //        return;
        //    }

        //    var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
        //    if (roleClaim == null)
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin", }));
        //        return;
        //    }

        //    if (_roles == null)
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin", }));
        //        return;
        //    }

        //    if (!_roles.Any(role => role == roleClaim.Value))
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin", }));
        //        return;
        //    }

        //}
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();
            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];


            var menus = context.HttpContext.Session.GetString("menus");

            if (_menu != null)
            {


                bool hasAccess = false;
                if (menus.Contains(_menu))
                {
                    hasAccess = true;
                }
                if (hasAccess == false)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Access" }));
                }
            }

            if (token == null || !jwtService.ValidateJwtToken(token, out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "Admin_Login", }));
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


