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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Myproject.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly ConString _conString;
        private readonly IConfiguration _configuration;
        // private readonly UserManager<ApplicationUser> _userManager;
        /// private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(ConString conection, IConfiguration configuration/*, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager*/)
        {
            _conString = conection;
            _configuration = configuration;
            // _userManager = userManager;
            // _roleManager = roleManager;
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

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
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

                var authClaims = new List<Claim>
                 {
                     new Claim("UserId", validateUser.ReturnObject.Id.ToString()),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 };

                var token = GetToken(authClaims);
                result.ReturnObject.Token = new JwtSecurityTokenHandler().WriteToken(token);
                result.ReturnObject.ExpireTimeToken = token.ValidTo;

                validateUser.ReturnObject.RefreshToken = GenerateRefreshToken();
                validateUser.ReturnObject.RefreshTokenExpire = DateTime.UtcNow.AddHours(2);
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

                var _ = new LogRepository(_conString).SaveLog(new Models.EntityClasses.Log()
                {
                    ApiName = nameof(Login),
                    Request = Newtonsoft.Json.JsonConvert.SerializeObject(user).ToString(),
                    Response = Newtonsoft.Json.JsonConvert.SerializeObject(result).ToString()
                });
            }

            return Ok(result.ReturnObject);
        }


    }


}
