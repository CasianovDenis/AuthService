using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Myproject.Models.DBConnection;

namespace Myproject.Controllers
{
    [Route("api/token")]
    [ApiController]

    public class JWTController : Controller
    {

        private readonly ConString _conString;
        private readonly IConfiguration _configuration;


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("~/api/verifying_token_expiration/{token_time}")]
        [HttpGet]
        public JsonResult testjwt(string token_time)
        {

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(token_time)).DateTime;


            DateTime actual_time = DateTime.Now;
            var result = DateTime.Compare(expirationTime, actual_time);

            if (result <= 0)
                return Json("Token is actual");
            else
                return Json("Token isn't actual");
        }


    }


}
