using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Myproject.Models.DBConnection;
using Myproject.Models;
using Myproject.Services;
using Myproject.Base.Models;
using Myproject.Repository;
using Myproject.Model.Repository;
using Myproject.Models.Repository;

namespace Myproject.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : PPController
    {
        private readonly ConString _conString;
        private readonly IConfiguration _configuration;

        public UsersController(ConString conection, IConfiguration configuration)
        {
            _conString = conection;
            _configuration = configuration;
        }

        [Route("create_account")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateAccount(Users user)
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var createUser = new UsersService(_conString).CreateAccount(user);
                if (!createUser.IsOk)
                    throw new Error(createUser.ResponseCode, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = createUser.ResponseCode.ToString() }).ReturnObject.Description);
            }
            catch (Error error)
            {
                result.ResponseCode = error.Code;
                result.ResultMessage = error.Message;

                var _ = new LogService(_conString).SaveLog(new LogModel()
                {
                    ApiName = nameof(CreateAccount),
                    Request = Newtonsoft.Json.JsonConvert.SerializeObject(user).ToString(),
                    Response = Newtonsoft.Json.JsonConvert.SerializeObject(result).ToString()
                });
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCode.TECHNICAL_EXCEPTION;
                result.ResultMessage = ex.Message;
            }
            return Ok(result);
        }

        [Route("get_profile_info")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetProfileInfo()
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var profile = new UsersService(_conString).GetProfileInfo(new Users() { Id = UserContextId });
                if (!profile.IsOk)
                    throw new Error(profile.ResponseCode, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = profile.ResponseCode.ToString() }).ReturnObject.Description);

                return Json(new
                {
                    UserName = profile.ReturnObject.Username,
                    Email = profile.ReturnObject.Email
                });
            }
            catch (Error error)
            {
                result.ResponseCode = error.Code;
                result.ResultMessage = error.Message;

                var _ = new LogService(_conString).SaveLog(new LogModel()
                {
                    ApiName = nameof(CreateAccount),
                    Request = Newtonsoft.Json.JsonConvert.SerializeObject(UserContextId).ToString(),
                    Response = Newtonsoft.Json.JsonConvert.SerializeObject(result).ToString()
                });
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCode.TECHNICAL_EXCEPTION;
                result.ResultMessage = ex.Message;
            }
            return Ok(result);
        }

        [Route("modify_profile")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public IActionResult ModifyProfile(Users user)
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };
            try
            {
                user.Id = UserContextId;

                var modifyAction = new UsersService(_conString).ModifyProfileInfo(user);
                if (!modifyAction.IsOk)
                    throw new Error(modifyAction.ResponseCode, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = modifyAction.ResponseCode.ToString() }).ReturnObject.Description);
            }
            catch (Error error)
            {
                result.ResponseCode = error.Code;
                result.ResultMessage = error.Message;
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCode.TECHNICAL_EXCEPTION;
                result.ResultMessage = ex.Message;

                var _ = new LogRepository(_conString).SaveLog(new Models.EntityClasses.Log()
                {
                    ApiName = nameof(CreateAccount),
                    Request = Newtonsoft.Json.JsonConvert.SerializeObject(user).ToString(),
                    Response = Newtonsoft.Json.JsonConvert.SerializeObject(result).ToString()
                });
            }
            return Ok(result);
        }

        [Route("modify_password")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public IActionResult ModifyPassword(Users user)
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };
            try
            {
                user.Id = UserContextId;

                var modifyAction = new UsersService(_conString).ModifyPassword(user);
                if (!modifyAction.IsOk)
                    throw new Error(modifyAction.ResponseCode, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = modifyAction.ResponseCode.ToString() }).ReturnObject.Description);

                var updateStamp = new UsersService(_conString).UpdateSecurityStamp(Guid.NewGuid().ToString(), user.Id);
                if (!updateStamp.IsOk)
                    throw new Error(updateStamp.ResponseCode, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = updateStamp.ResponseCode.ToString() }).ReturnObject.Description);
            }
            catch (Error error)
            {
                result.ResponseCode = error.Code;
                result.ResultMessage = error.Message;
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCode.TECHNICAL_EXCEPTION;
                result.ResultMessage = ex.Message;
            }
            return Ok(result);
        }

        [Route("close_account")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public IActionResult CloseAccount()
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var closeAccount = new UsersService(_conString).CloseAccountById(UserContextId);
                if (!closeAccount.IsOk)
                    throw new Error(closeAccount.ResponseCode, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = closeAccount.ResponseCode.ToString() }).ReturnObject.Description);
            }
            catch (Error error)
            {
                result.ResponseCode = error.Code;
                result.ResultMessage = error.Message;
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCode.TECHNICAL_EXCEPTION;
                result.ResultMessage = ex.Message;
            }
            return Ok(result);
        }
    }


}
