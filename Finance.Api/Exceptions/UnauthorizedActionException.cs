namespace Personal.Finance.Api.Exceptions
{
    public class UnauthorizedActionException : AppException
    {
        public UnauthorizedActionException(string message) 
            : base(message, "FORBIDDEN", StatusCodes.Status403Forbidden) { }
    }
}
