namespace Myproject.Base.Models
{
    public class Error : SystemException
    {
        public ResponseCode Code { get; set; }
        public string Message { get; set; }

        public Error(ResponseCode codeErr, string messagErr)
        {

            Code = codeErr;
            Message = messagErr;
        }

        public Error(ResponseCode codeErr)
        {

            Code = codeErr;

        }
    }
}