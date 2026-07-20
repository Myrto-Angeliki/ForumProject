namespace ForumProject.Application.Features.Auths.DTOs
{
    public class RegistrationDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set;} = string.Empty;
        public string PasswordConfirm { get; set;} = string.Empty;
        public string Username { get; set; } = "";
    }
}