namespace Personal.Finance.Api.DTOs.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
