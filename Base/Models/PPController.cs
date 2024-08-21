using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Myproject.Models.DBConnection;
using Myproject.Models.Repository;
using Myproject.Services;

namespace Myproject.Base.Models
{
    [Authorize]
    [AuthorizeAndCheckStamp]
    public class PPController : Controller
    {
        private int UserContext()
        {
            return Convert.ToInt32(HttpContext.User.Claims.Select(x => x).Where(x => x.Type == "UserId").FirstOrDefault().Value);
        }

        public int UserContextId { get { return UserContext(); } }

        private static string request;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            request = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments).ToString();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var _conString = context.HttpContext.RequestServices.GetService(typeof(ConString)) as ConString;

            var _ = new LogService(_conString).SaveLog(new LogModel()
            {
                ApiName = context.ActionDescriptor.AttributeRouteInfo.Template,
                Request = request,
                Response = Newtonsoft.Json.JsonConvert.SerializeObject(context.Result).ToString()
            });
        }
    }
}