using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Gov.Jag.Embc.Public.Utils
{
    public class EmailSender : IEmailSender
    {
        private readonly string smtpServer;
        private readonly string defaultSender;
        private readonly ILogger logger;
        private readonly ICredentialsByHost credentials;
        private int smtpPort;
        private bool enableSSL;

        private bool Enabled => !string.IsNullOrEmpty(smtpServer);

        public EmailSender(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            smtpServer = configuration["SMTP_HOST"];
            smtpPort = int.Parse(configuration["SMTP_PORT"] ?? "25");
            defaultSender = configuration["SMTP_DEFAULT_SENDER"];
            credentials = string.IsNullOrEmpty(configuration["SMTP_USER"]) ? null : new NetworkCredential(configuration["SMTP_USER"], configuration["SMTP_PASSWORD"]);
            enableSSL = bool.Parse(configuration["SMTP_ENABLE_SSL"] ?? "false");
            logger = loggerFactory.CreateLogger(typeof(EmailSender));
        }

        public void Send(EmailMessage message)
        {
            if (!Enabled)
            {
                logger.LogWarning("SMTP is not configured, check the environment variables for SMTP_HOST");
                return;
            }
            using (var client = new SmtpClient(smtpServer, smtpPort) { Credentials = credentials, EnableSsl = enableSSL })
            {
                var email = new MailMessage(message.From ?? defaultSender, message.To, message.Subject, message.Content) { IsBodyHtml = true };
                client.Send(email);
            }
        }
    }
}
