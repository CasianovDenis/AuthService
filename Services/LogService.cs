using Myproject.Models.DBConnection;
using Myproject.Base.Models;
using Myproject.Model.Repository;
using Myproject.Repository;
using Myproject.Models.Repository;
using Myproject.Models.EntityClasses;

namespace Myproject.Services
{
    public class LogService
    {
        private ConString _dbContext { get; set; }

        public LogService(ConString DbContext)
        {
            _dbContext = DbContext;
        }

        public ResultBase SaveLog(LogModel logModel)
        {
            var result = new ResultBase() { ResponseCode = ResponseCode.SUCCES };

            try
            {
                var logs = new Log()
                {
                    ApiName = logModel.ApiName,
                    Request = logModel.Request,
                    Response = logModel.Response
                };
                var save = new LogRepository(_dbContext).SaveLog(logs);
                if (!save.IsOk)
                    throw new Error(save.ResponseCode, new DictionaryRepository(_dbContext).GetDictionary(new DictionaryModel() { Code = save.ResponseCode.ToString() }).ReturnObject.Description);

                result = save;
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
