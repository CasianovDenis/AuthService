
using Myproject.Models.DBConnection;
using Myproject.Base.Models;
using Myproject.Model.Repository;
using Myproject.Models.EntityClasses;
namespace Myproject.Repository
{
    public class DictionaryRepository
    {
        private readonly ConString _conString;

        public DictionaryRepository(ConString DbContext)
        {

            _conString = DbContext;
        }


        public Result<DictionaryModel> GetDictionary(DictionaryModel search)
        {
            var result = new Result<DictionaryModel>() { ResponseCode = ResponseCode.SUCCES, ReturnObject = new DictionaryModel() };

            try
            {
                var searchResult = _conString.Dictionaries.Select(data => data).Where(data => data.Code == search.Code).FirstOrDefault();

                if (searchResult != null)
                    result.ReturnObject = new DictionaryModel(searchResult);
                else
                    throw new Error(ResponseCode.NO_RECORD);
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

        /*  public string GetDescription(string searchCode)
          {
              var result = new Result<string>() { ResponseCode = ResponseCode.SUCCES };

              try
              {
                  var searchResult = _conString.Dictionaries.Select(data => data).Where(data => data.Code == searchCode).FirstOrDefault();

                    if (searchResult != null)
                       result.ReturnObject = searchResult.Description;
                    else
                        throw new Error(ResponseCode.NO_RECORD);
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
              return result.ReturnObject;
          }*/
    }


}
