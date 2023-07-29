namespace WebApi.Domain.ViewModels.Password
{
    public class PasswordViewModel
    {
        public int Id { get; set; }
        public string SecretToken { get; set; }
        public string Password { get; set; }
        public string Service { get; set; }
    }
}
