
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
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //var _conString = context.HttpContext.RequestServices.GetService(typeof(ConString)) as ConString;

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var _conString = context.HttpContext.RequestServices.GetService(typeof(ConString)) as ConString;
            var y = context.HttpContext.Request.QueryString;
            //var s = context.HttpContext.Request.Form;
            var _ = new LogService(_conString).SaveLog(new LogModel()
            {
                ApiName = context.ActionDescriptor.AttributeRouteInfo.Template,
                Request = "",//Newtonsoft.Json.JsonConvert.SerializeObject(UserContextId).ToString(),
                Response = Newtonsoft.Json.JsonConvert.SerializeObject(context.Result).ToString()
            });
        }

        private int UserContext()
        {
            return Convert.ToInt32(HttpContext.User.Claims.Select(x => x).Where(x => x.Type == "UserId").FirstOrDefault().Value);
        }

        public int UserContextId { get { return UserContext(); } }
    }
}