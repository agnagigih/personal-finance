namespace Personal.Finance.Api.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message, string errorCode = "NOT_FOUND") 
            : base(errorCode, message, StatusCodes.Status404NotFound) { }
    }
}
