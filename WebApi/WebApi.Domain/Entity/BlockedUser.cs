namespace WebApi.Domain.Entity
{
    public class BlockedUser
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime UnbanDate { get; set; }

    }
}
