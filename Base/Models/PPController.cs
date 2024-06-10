
using Microsoft.AspNetCore.Mvc;
using Myproject.Base.Services.HttpTrace;
using Myproject.Models.DBConnection;

namespace Myproject.Base.Models
{
    public class PPController : Controller
    {
        //public HttpTrace Action = new HttpTrace();

        private int UserContext()
        {
            return Convert.ToInt32(HttpContext.User.Claims.Select(x => x).Where(x => x.Type == "UserId").FirstOrDefault().Value);
        }

        public int UserContextId { get { return UserContext(); } }
    }
}