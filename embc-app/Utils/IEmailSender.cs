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
}
