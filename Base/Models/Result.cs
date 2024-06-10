
using Microsoft.IdentityModel.Tokens;

namespace Myproject.Base.Models
{
    public enum ResponseCode
    {
        SUCCES = 0,
        TECHNICAL_ERROR = 1,
        TECHNICAL_EXCEPTION = 2,
        USER_ALREADY_EXIST = 3,
        NO_RECORD = 4,
        USER_NOT_EXIST = 5
    }

    public class Result<T> : ResultBase
    {
        public T ReturnObject { get; set; }
    }

    public class ResultBase
    {
        private ResponseCode actionCode { get; set; }
        public ResponseCode ResponseCode { get { return actionCode; } set { actionCode = value; IsOk = value == 0; } }

        public bool IsOk { get; set; }
        public string ResultMessage { get; set; }
    }
}