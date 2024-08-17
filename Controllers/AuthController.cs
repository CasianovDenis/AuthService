using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Myproject.Models.DBConnection;
using Myproject.Models;
using Myproject.Base.Models;
using Myproject.Services;
using Myproject.Repository;
using Myproject.Model.Repository;
using Myproject.Models.Repository;

namespace Myproject.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : PPController
    {
        private readonly ConString _conString;
        private readonly IConfiguration _configuration;

        public AuthController(ConString conection, IConfiguration configuration)
        {
            _conString = conection;
            _configuration = configuration;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signIn);

            return token;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Users user)
        {
            var result = new Result<LoginModel>() { ResponseCode = ResponseCode.SUCCES, ReturnObject = new LoginModel() };
            try
            {
                var validateUser = new UsersService(_conString).ValidateUser(user);
                if (!validateUser.IsOk)
                    throw new Error(validateUser.ResponseCode, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = validateUser.ResponseCode.ToString() }).ReturnObject.Description);

                if (string.IsNullOrEmpty(validateUser.ReturnObject.SecurityStamp))
                    new UsersService(_conString).UpdateSecurityStamp(Guid.NewGuid().ToString(), user.Id);

                var authClaims = new List<Claim>
                 {
                     new Claim(PPClaimTypes.UserId, validateUser.ReturnObject.Id.ToString()),
                     new Claim(PPClaimTypes.SecurityStamp, validateUser.ReturnObject.SecurityStamp),
                 };

                var token = GetToken(authClaims);
                result.ReturnObject.Token = new JwtSecurityTokenHandler().WriteToken(token);
                result.ReturnObject.ExpireTimeToken = token.ValidTo;

                _conString.SaveChanges();
            }
            catch (Error error)
            {
                result.ResponseCode = error.Code;
                result.ResultMessage = error.Message;

                var _ = new LogService(_conString).SaveLog(new LogModel()
                {
                    ApiName = nameof(Login),
                    Request = Newtonsoft.Json.JsonConvert.SerializeObject(user).ToString(),
                    Response = Newtonsoft.Json.JsonConvert.SerializeObject(result).ToString()
                });
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCode.TECHNICAL_EXCEPTION;
                result.ResultMessage = ex.Message;
            }

            return Ok(result.ReturnObject);
        }
    }
}
