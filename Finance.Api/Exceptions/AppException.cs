namespace Personal.Finance.Api.Exceptions
{
    public class AppException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode {  get; }
        protected AppException(string message, string errorCode, int StatusCode) : base(message)
        {
            ErrorCode = errorCode;
            this.StatusCode = StatusCode;
        }
    }
}
