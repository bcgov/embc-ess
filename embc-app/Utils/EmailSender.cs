using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Gov.Jag.Embc.Public.Utils
{
    public interface IEmailSender
    {
        void Send(EmailMessage message);
    }

    public class EmailMessage
    {
        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Content { get; }

        public EmailMessage(string to, string subject, string body) : this(null, to, subject, body)
        {
        }

        public EmailMessage(string from, string to, string subject, string body)
        {
            Content = body;
            Subject = subject;
            To = to;
            From = from;
        }
    }

    public class EmailSender : IEmailSender
    {
        private readonly string smtpServer;
        private readonly string defaultSender;
        private readonly ILogger logger;
        private readonly ICredentialsByHost credentials;
        private readonly int smtpPort;
        private readonly bool enableSSL;

        private bool Enabled => !string.IsNullOrEmpty(smtpServer);

        public EmailSender(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            smtpServer = configuration.GetValue<string>("SMTP_HOST");
            smtpPort = configuration.GetValue<int?>("SMTP_PORT") ?? 25;
            defaultSender = configuration.GetValue<string>("SMTP_DEFAULT_SENDER");
            credentials = string.IsNullOrEmpty(configuration["SMTP_USER"]) ? null : new NetworkCredential(configuration.GetValue<string>("SMTP_USER"), configuration.GetValue<string>("SMTP_PASSWORD"));
            enableSSL = configuration.GetValue<bool?>("SMTP_ENABLE_SSL") ?? false;
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
