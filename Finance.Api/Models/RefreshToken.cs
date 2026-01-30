using Personal.Finance.Api.Models;

namespace Personal.Finance.Api.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiredDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRevoked { get; set; }
        public User User { get; set; } = null!;
    }
}
