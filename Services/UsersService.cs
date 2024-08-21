using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Myproject.Models.DBConnection;
using Myproject.Models;
using Myproject.Base.Models;
using Myproject.Model.Repository;
using Myproject.Repository;
using System.Security.Cryptography;

namespace Myproject.Services
{
    public class UsersService
    {
        private ConString _dbContext { get; set; }

        public UsersService(ConString DbContext)
        {
            _dbContext = DbContext;
        }

        private static string sha256(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public Result<Users> ValidateUser(Users user)
        {
            var result = new Result<Users>() { ResponseCode = ResponseCode.SUCCES };

            try
            {
                user.Password = sha256(user.Password);

                var validate = new UsersRepository(_dbContext).ValidateUser(user);
                if (!validate.IsOk)
                    throw new Error(validate.ResponseCode, new DictionaryRepository(_dbContext).GetDictionary(new DictionaryModel() { Code = validate.ResponseCode.ToString() }).ReturnObject.Description);

                result = validate;
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

            return result;
        }

        public Result<Users> GetProfileInfo(Users user)
        {
            var result = new Result<Users>() { ResponseCode = ResponseCode.SUCCES };

            try
            {
                var profile = new UsersRepository(_dbContext).GetProfileInfo(user);
                if (!profile.IsOk)
                    throw new Error(profile.ResponseCode, new DictionaryRepository(_dbContext).GetDictionary(new DictionaryModel() { Code = profile.ResponseCode.ToString() }).ReturnObject.Description);

                result = profile;
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

            return result;
        }

        public ResultBase UpdateSecurityStamp(string stamp, int userId)
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };

            try
            {
                var closeAccount = new UsersRepository(_dbContext).UpdateSecurityStamp(stamp, userId);
                if (!closeAccount.IsOk)
                    throw new Error(closeAccount.ResponseCode, new DictionaryRepository(_dbContext).GetDictionary(new DictionaryModel() { Code = closeAccount.ResponseCode.ToString() }).ReturnObject.Description);

                result = closeAccount;
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

            return result;
        }
    }


}
