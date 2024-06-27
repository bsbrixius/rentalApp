namespace Crosscutting.Mail
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string ServerEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
