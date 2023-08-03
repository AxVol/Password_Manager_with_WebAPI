namespace Desktop_client.Models
{
    public class PasswordSendModel
    {
        public int Id { get; set; }
        public string SecretToken { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Service { get; set; }
    }
}
