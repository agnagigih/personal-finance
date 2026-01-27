namespace Personal.Finance.Api.Exceptions
{
    public class AuthenticationException : AppException
    {
        public AuthenticationException(string message)
            : base("USER_NOT_FOUND", message, StatusCodes.Status401Unauthorized) { }
    }
}
