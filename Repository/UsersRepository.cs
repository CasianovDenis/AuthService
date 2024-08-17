using Myproject.Models.DBConnection;
using Myproject.Models;
using Myproject.Base.Models;
using Myproject.Model.Repository;

namespace Myproject.Repository
{
    public class UsersRepository
    {
        private ConString _conString;

        public UsersRepository(ConString dbContext)
        {
            _conString = dbContext;
        }

        public ResultBase CreateAccount(Users user)
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var exists = _conString.Users.Select(data => data).Where(data => data.Username == user.Username && data.Status == UserStatus.Active).FirstOrDefault();
                if (exists == null)
                {
                    user.Status = UserStatus.Active;
                    user.SecurityStamp = Guid.NewGuid().ToString();

                    _conString.Add(user);
                    _conString.SaveChanges();
                }
                else
                    throw new Error(ResponseCode.USER_ALREADY_EXIST, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = ResponseCode.USER_ALREADY_EXIST.ToString() }).ReturnObject.Description);
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

        public Result<Users> ValidateUser(Users user)
        {
            var result = new Result<Users>() { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var userData = _conString.Users.Select(data => data).Where(data => data.Username == user.Username && data.Password == user.Password).FirstOrDefault();
                if (userData == null)
                    throw new Error(ResponseCode.USER_NOT_EXIST, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = ResponseCode.USER_NOT_EXIST.ToString() }).ReturnObject.Description);

                result.ReturnObject = userData;
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
                var userInfo = _conString.Users.Select(data => data).Where(data => data.Id == user.Id).FirstOrDefault();
                if (userInfo != null)
                    result.ReturnObject = userInfo;
                else
                    throw new Error(ResponseCode.NO_RECORD, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = ResponseCode.USER_ALREADY_EXIST.ToString() }).ReturnObject.Description);
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

        public ResultBase ModifyProfileInfo(Users user)
        {
            var result = new ResultBase { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var userInfo = _conString.Users.Select(data => data).Where(data => data.Id == user.Id).FirstOrDefault();
                if (userInfo == null)
                    throw new Error(ResponseCode.NO_RECORD, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = ResponseCode.USER_ALREADY_EXIST.ToString() }).ReturnObject.Description);

                userInfo.Email = user.Email ?? userInfo.Email;

                _conString.SaveChanges();
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

        public ResultBase CloseAccountById(int userId)
        {
            var result = new ResultBase { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var userInfo = _conString.Users.Select(data => data).Where(data => data.Id == userId).FirstOrDefault();
                if (userInfo == null)
                    throw new Error(ResponseCode.NO_RECORD, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = ResponseCode.USER_ALREADY_EXIST.ToString() }).ReturnObject.Description);

                userInfo.Username = userInfo.Username + DateTime.Now.ToString("dd-MM-yyyy");
                userInfo.Status = UserStatus.Closed;
                //userInfo.RefreshToken = null;
                //userInfo.RefreshTokenExpire = null;
                _conString.SaveChanges();
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
            var result = new ResultBase { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var userInfo = _conString.Users.Select(data => data).Where(data => data.Id == userId).FirstOrDefault();
                if (userInfo == null)
                    throw new Error(ResponseCode.NO_RECORD, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = ResponseCode.USER_ALREADY_EXIST.ToString() }).ReturnObject.Description);

                userInfo.SecurityStamp = stamp;

                _conString.SaveChanges();
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

        public ResultBase ModifyPassword(Users user)
        {
            var result = new ResultBase { ResponseCode = ResponseCode.SUCCES };
            try
            {
                var userInfo = _conString.Users.Select(data => data).Where(data => data.Id == user.Id).FirstOrDefault();
                if (userInfo == null)
                    throw new Error(ResponseCode.NO_RECORD, new DictionaryRepository(_conString).GetDictionary(new DictionaryModel() { Code = ResponseCode.USER_ALREADY_EXIST.ToString() }).ReturnObject.Description);

                userInfo.Password = user.Password ?? userInfo.Password;

                _conString.SaveChanges();
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
