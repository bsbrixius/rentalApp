using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

namespace NeoLabs.Security.Authorization
{
    //TODO - Utilizar autenticador custom para ter maior controle sobre acesso aos controllers
    public class AccessAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }
        public string Context { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Manage { get; set; }
        public bool Execute { get; set; }
        public bool Delete { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated == false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            if (userRoles.ToLower().Contains("admin"))
                return;

            if (Roles != null)
            {

            }
            if (Context == null)
            {
                var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                Context = controllerActionDescriptor.ControllerName ?? string.Empty;
            }

            //var contextPermissionFromToken = context.HttpContext.User.Claims
            //    .Where(x => x.Type == $"{Context}-{ContextPermissions.ContextPermission}").FirstOrDefault()?.Value;

            var contextPermissionFromToken = context.HttpContext.User.Claims
                .Where(x => x.Type == $"contextPermission").FirstOrDefault()?.Value;

            if (contextPermissionFromToken == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!Read && !Write && !Manage && !Execute && !Delete)
            {
                return;
            }

            var contextPermission = JsonSerializer.Deserialize<List<ContextPermissionDto>>(contextPermissionFromToken)
                .FirstOrDefault(x => x.Context == Context);

            if (Read && !contextPermission.Read ||
                Write && !contextPermission.Write ||
                Manage && !contextPermission.Manage ||
                Execute && !contextPermission.Execute ||
                Delete && !contextPermission.Delete)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            return;

            context.Result = new UnauthorizedResult();
            return;

            //The below line can be used if you are reading permissions from token
            //var permissionsFromToken = context.HttpContext.User.Claims.Where(x => x.Type == "Permissions").Select(x => x.Value).ToList();

            //return;

            //Identity.Name will have windows logged in user id, in case of Windows Authentication
            //Indentity.Name will have user name passed from token, in case of JWT Authenntication and having claim type "ClaimTypes.Name"
            //var userName = context.HttpContext.User.Identity.Name;
            //var assignedPermissionsForUser = MockData.UserPermissions.Where(x => x.Key == userName).Select(x => x.Value).ToList();


            //var requiredPermissions = Permissions.Split(","); //Multiple permissiosn can be received from controller, delimiter "," is used to get individual values
            //foreach (var x in requiredPermissions)
            //{
            //    if (assignedPermissionsForUser.Contains(x))
            //        return; //User Authorized. Wihtout setting any result value and just returning is sufficent for authorizing user
            //}

            //context.Result = new UnauthorizedResult();
            //return;
        }
    }
}

