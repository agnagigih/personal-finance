namespace Personal.Finance.Api.Responses
{
    public class ApiError
    {
        public string Code { get; set; } = default!;
        public string Message { get; set; } = default!;
        public object? Details { get; set; }
    }
}
