namespace Personal.Finance.Api.DTOs.Auth
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}
