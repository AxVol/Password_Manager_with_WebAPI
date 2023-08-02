namespace Desktop_client.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecretToken { get; set; }
    }
}
