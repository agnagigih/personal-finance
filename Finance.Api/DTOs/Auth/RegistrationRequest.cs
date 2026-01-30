namespace Personal.Finance.Api.DTOs.Auth
{
    public class RegistrationRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
