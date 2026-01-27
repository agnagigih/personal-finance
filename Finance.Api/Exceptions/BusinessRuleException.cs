namespace Personal.Finance.Api.Exceptions
{
    public class BusinessRuleException : AppException
    {
        public BusinessRuleException(string message, string errorCode = "BUSINESS_RULE_VIOLATION") 
            : base(message, errorCode, StatusCodes.Status400BadRequest) { }
    }
}
