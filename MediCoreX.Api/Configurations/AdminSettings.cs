namespace MediCoreX.Api.Configurations
{
    public class AdminSettings
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool ResetPasswordOnStartup { get; set; }
    }
}
