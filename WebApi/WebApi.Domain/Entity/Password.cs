namespace WebApi.Domain.Entity
{
    public class Password
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string PassWord { get; set; }
        public string PassService { get; set; }
    }
}
