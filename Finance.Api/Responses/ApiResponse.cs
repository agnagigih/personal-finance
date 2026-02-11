namespace Personal.Finance.Api.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }

        public static ApiResponse<T> SuccessResponse(T? data) => new()
        {
            Success = true,
            Data = data
        };

        public static ApiResponse<T> Fail(string code, string message, object? details = null) => new()
        {
            Success = false,
            Error = new ApiError
            {
                Code = code,
                Message = message,
                Details = details,
            }
        };
    }
}
