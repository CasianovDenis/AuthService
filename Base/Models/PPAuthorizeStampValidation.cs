using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Myproject.Models;
using Myproject.Models.DBConnection;

namespace Myproject.Base.Models
{
    public class AuthorizeAndCheckStamp : AuthorizeAttribute, IAuthorizationFilter
    {
        private void HandleUnauthorizedResult(AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedObjectResult("Unauthorized");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _conString = context.HttpContext.RequestServices.GetService(typeof(ConString)) as ConString;

            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
            var isAutorized = context.HttpContext.User.Identity.IsAuthenticated;

            if (!allowAnonymous && isAutorized)
            {
                var claims = context.HttpContext.User.Claims;
                if (claims == null)
                    HandleUnauthorizedResult(context);

                var tokenStamp = context.HttpContext.User.Claims.Where(x => x.Type == "SecurityStamp").Select(x => x.Value).FirstOrDefault();
                if (tokenStamp == null)
                    HandleUnauthorizedResult(context);
                else
                {
                    var userId = claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

                    var user = _conString.Users.Where(x => x.Id == Convert.ToInt32(userId)).FirstOrDefault();
                    if (user.Status != UserStatus.Active)
                        HandleUnauthorizedResult(context);

                    if (tokenStamp != user.SecurityStamp)
                        HandleUnauthorizedResult(context);
                }
            }
        }
    }
}