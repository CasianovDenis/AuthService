
using Myproject.Models.DBConnection;
using Myproject.Base.Models;
using Myproject.Model.Repository;
using Myproject.Models.Repository;
using Myproject.Models.EntityClasses;
namespace Myproject.Repository
{
    public class LogRepository
    {
        private readonly ConString _conString;

        public LogRepository(ConString DbContext)
        {

            _conString = DbContext;
        }


        public ResultBase SaveLog(Log log)
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };

            try
            {
                _conString.Add(log);
                _conString.SaveChanges();
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
